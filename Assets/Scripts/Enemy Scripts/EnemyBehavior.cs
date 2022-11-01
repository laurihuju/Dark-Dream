using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyBehavior : MonoBehaviour
{
    /***************************************Referenssit******************************************************/
    [Header("Reference")]
    // Preferenssi scriptiin:
    [SerializeField] private GameObject AstarClass;
    [SerializeField] private GameObject AIDestination;
    [SerializeField] Animator anim;
    Vector3 lastposition;
    // pelaajan posiitio
    [SerializeField] private GameObject playerPos;

    // EnemyState
    public EnemyState currentState;
    private EnemyState previousState;
    // Singelton
    public static EnemyBehavior instance;

    /***************************************Vihollisen Liikkuminen******************************************************/
    [Header("Movement")]
    [Space(20, order = 0)]
    // Liikkuminen:
    [SerializeField] private float moveSpeed;  // Vihollisen perus nopeus patrollin aikana
    // Nopeus, kun vihollinen jahtaa pelaajaa määritetään AIPATH-scriptissä!

    /***************************************Vihollisen Hyökkäys******************************************************/
    [Header("Attacking")]
    //Vihollisen hyökkäys:
    [SerializeField] private float attackSpeed; //
    [SerializeField] private int attackDamage; // Vihollisen vahinko
    [SerializeField] private float attackRange;
    bool runningAttackCourotine = false;

    /***************************************Vihollisen Patroll-toiminto******************************************************/
    [Header("Patroll")]
    [Space(20, order = 0)]
    // Vihollisen patroll-toiminto:
    public Transform[] patrollPoints;  // Vihollinen kävelee näiden pisteiden välillä
    private int currentPointIndex;   // Piste, johon vihollinen suuntaa
    private int closestPoint;


    [SerializeField] float spotRadius = 5f;   // Vihollisen toka alua, jolla pelaaja voidaan huomata

    bool stateHasChanged = true;

    public bool canTurnTowardsTarget; // Voiko viholline katsoa suoraan pelaajaan (voi, jos pelaaja on lähellä vihollista)
    public bool needClosestPoint = true;
    /***************************************Vihollisen Chase-toiminto******************************************************/
    private Vector3 playerLastPosition;

    public bool canTeleport = true;

    [Header("BloodSplash")]
    [SerializeField] Animator Blood;
    public bool canAttack = true;

    /***************************************Vihollisen Audio******************************************************/




    /******************************************Vihollisen tilat************************************************************/
    [System.Serializable]
    public enum EnemyState
    {
        patroll, // vihollinen tarkistaa pisteitä
        chase, // vihollinen jahtaa pelaaja
        attack, // vihollinen lyö pelaajaa
        investigate, // vihollinen tarkistaa pisteen, jossa ollaan nähty pelaaja viimeisen kerran 
        idle
    }

    /*****************************************************Awake************************************************************/

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;


        }

        // Vihollinen suorittaa oletuksena patroll-toimintoa:
        currentState = EnemyState.patroll;
    }

    /*****************************************************Start************************************************************/

    void Start()
    {
        previousState = currentState;
        needClosestPoint = true;
        //Debug.Log(FindClosestPoint(patrollPoints));
        lastposition = transform.position;
    }
    private void LateUpdate()
    {
        // Tarkistetaan vihollisen ja pelaaja väklinen etäisyys.
        CheckDistance();
        lastposition = transform.position;
    }
    /*****************************************************FixedUpdate************************************************************/
    private void FixedUpdate()
    {
        if (previousState != currentState)
        {
            stateHasChanged = true;
            previousState = currentState;
           
        }



        /***************************************Vihollisen Hyökkäys******************************************************/




        /***************************************Vihollisen jahtaminen******************************************************/
        if (currentState == EnemyState.chase)
        {
            canTeleport = false;
            if (stateHasChanged)
            {
                stateHasChanged = false;
                // Käynnistetään AI-toiminto
                this.GetComponent<AIDestinationSetter>().investigating = false;

                AudioManager.instance.ChangeMusic("chase");

                canTurnTowardsTarget = true;
                this.GetComponent<AIPath>().enabled = true;
                this.GetComponent<Seeker>().enabled = true;
                AstarClass.GetComponent<AstarPath>().enabled = true;
                this.GetComponent<AIPath>().canMove = true;

            }






        }
        /***************************************Vihollisen etsintä******************************************************/
        else if (currentState == EnemyState.investigate)
        {
            canTeleport = true;
            if (stateHasChanged)
            {
                stateHasChanged = false;
                AudioManager.instance.ChangeMusic("gameBG");
                AudioManager.instance.Play("angry");



            }

            this.GetComponent<AIDestinationSetter>().investigating = true;




        }


        /******************************************Patrollin Suoritus************************************************/

        // Patrollin suoritus tapahtuu tässä:
        else if (currentState == EnemyState.patroll)
        {
            canTeleport = true;

            if (stateHasChanged)
            {
                stateHasChanged = false;

                this.GetComponent<AIDestinationSetter>().investigating = false;
                this.GetComponent<AIPath>().canMove = true;
                canTurnTowardsTarget = false;

                AudioManager.instance.Play("ghost");

                
                this.GetComponent<AIPath>().enabled = false;
                this.GetComponent<Seeker>().enabled = false;
                AstarClass.GetComponent<AstarPath>().enabled = false;

                
            }


            if (needClosestPoint)
            {


                // Etsitään lähin pisten sen jälkeen, kun vihollinen lopettaa INVESTIGATION-muodon ilman pelaajan löytämistä:
                currentPointIndex = FindClosestPoint(patrollPoints);
                needClosestPoint = false;
            }

            // Liikutetaan vihollinen pisteisiin:
            transform.position = Vector2.MoveTowards(transform.position, patrollPoints[currentPointIndex].position,
            moveSpeed * Time.deltaTime);

            // Onko vihollinen lähellä pistettä?
            if (Vector2.Distance(transform.position, patrollPoints[currentPointIndex].position) < 0.1f)
            {
                // Tarkistetaan, että onko nykyinen piste viimeinen:
                if (currentPointIndex < patrollPoints.Length - 1)
                {
                    // Nykyinen piste ei ole viimeinen, joten jatketaan prosessi:
                    currentPointIndex++;


                }
                else if (currentPointIndex == 19)
                {
                    // Vihollinen palaa ensimmäiseen pisteeseen eli index = 0:
                    currentPointIndex = 0;


                }

            }

        }




    }


    /*****************************************************FindClosestPoint-metodi************************************************************/

    // Etsii lähimmän pisteen sen jälkeen, kun vihollinen kadottaa pelaajan:
    int FindClosestPoint(Transform[] patrollPoints)
    {
        int closestPoint = 0;
        Transform closestTarget = null;
        float closestDistanceSquare = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialPoint in patrollPoints)
        {
            Vector3 directionToPoint = potentialPoint.position - currentPosition;
            float dirSquareToPoint = directionToPoint.sqrMagnitude;
            if (dirSquareToPoint < closestDistanceSquare)
            {
                closestDistanceSquare = dirSquareToPoint;
                closestTarget = potentialPoint;

            }

        }
        closestPoint = System.Array.IndexOf(patrollPoints, closestTarget);

        return closestPoint;
    }

    /****************************************Vihollisen ja pelaajan etäisyyden tarkistus******************************************************/

    private void CheckDistance()
    {
        changeAnim(transform.position - lastposition);
        // lasketaan etäisyys vihollisen ja pelaajan välillä:
        float distance = Vector3.Distance(playerPos.transform.position, transform.position);


        // Onko pelaaja hyökkäysalueella? 
        if (distance <= attackRange && currentState == EnemyState.chase && !runningAttackCourotine && canAttack)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.chase)
            {
                StartCoroutine(AttackPlayer(attackDamage, attackDamage));


                return;
            }



        }
        // Onko pelaaja havaintoalueella?
        else if (distance <= spotRadius)
        {


            currentState = EnemyState.chase;


        }



        /*************************************************Vihollisen hyökkäys metodi******************************************************/
        IEnumerator AttackPlayer(float attackSpeed, int attackDamage)
        {
            // Veriroiske
            Blood.SetTrigger("Blood");



            runningAttackCourotine = true;

            HealthManager.instance.ChangeCurrentHP(attackDamage);
            

            this.GetComponent<AIPath>().canMove = false;

            yield return new WaitForSeconds(2f);
            Blood.SetTrigger("Back");
            this.GetComponent<AIPath>().canMove = true;

            runningAttackCourotine = false;


        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spotRadius);


    }
    /// <summary>
    /// Kertoo animaattorille mikā animaatio suoritetaan.
    /// </summary>
    /// <param name=setVector"></param>



    private void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }




    /// <summary>
    /// Vaihtaa animaation (alas, ylös, vasen tai oikea) pelihahmon sijainnin perusteella
    /// </summary> 
    ///  <param name="direction"></param>

    private void changeAnim(Vector2 direction)
    {
        // Onko pelihahmo vihollisen vasemmalla tai oikealla puolella?
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {

            // Onko pelihahmo oikealle puolella?
            if (direction.x > 0)
            {

                //print("Oikealla");
                // Suoritetaan oikealle meneva animaatio (1,0)
                SetAnimFloat(Vector2.right);

            }


            // Onko pelihahmo vasemmalle puolella?

            else if (direction.x < 0)
            {
                //print("Vasemmalla");
                // Suoritetaan vasemmalle meneva animaatio (-1,0)
                SetAnimFloat(Vector2.left);
            }

        }
        // Onko pelihahmo vihollisen ala tai ylä puolella?
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))

        {

            // Onko pelihahmo yläpuolella?
            if (direction.y > 0)
            {
                // Suoritetaan ylös meneva animaatio (0, 1)
                SetAnimFloat(Vector2.up);
            }

            // Onko pelihahmo alapuolella?
            else if (direction.y < 0)
            {
                // Suoritetaan alas meneva animaatio (0,-1)
                SetAnimFloat(Vector2.down);
            }
        }
    }


    
}