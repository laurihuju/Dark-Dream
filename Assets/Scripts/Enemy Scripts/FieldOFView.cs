using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOFView : MonoBehaviour
{



    [Header("References")]
    // Referenssi vihollisen behaviour-luokkaan
    [SerializeField] private EnemyBehavior enemyBehavior;
    [SerializeField] private GameObject Target;


    [Header("Mesh")]
    // Näköalueen verkko:
    private Mesh mesh;
    // esineet, joihin raycast voi osua:
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask layerMask2;

    [SerializeField] private float fov; // näköalueen leveys
    [SerializeField] private float Angle; // Kulma, josta aloitetaan raycastien piirto

    [SerializeField] int rayCount = 50;   // säteiden/viivojen määrä
    Vector3 previousPosition;

    public bool canActivate;

    [Header("Kääntyminen Patroll-toiminnossa")]
    [SerializeField] private float LerpSmoother;

    [Header("Pelaajan Etsiminen/INVESTIGATION")]

    [SerializeField] float maxAttentionTime = 4; // Kuinka kauan vihollinen voi jahtaa näkemättä pelaajaa
    [SerializeField] float currentAttentionTime;


    private float currentDeltaTime; // Time.delTatime


    [SerializeField] float currentInvestigationTimer;
    [SerializeField] float maxInvestigationTime; // Kuinka kauan vihollinen voi etsiä pelaajaa (olla INVESTIGATION-muodossa)

    bool courotineISRunning = false;
    float randomAngle; // Satunnainen kulma. (Käytetään, kun vihollinen on INVESTIGATION-muodossa)
    [SerializeField] float changeAngleInterval; // Kuinka usein viholllinen liikuttaa näköaluetta INVESTIGATION-muodossa

    float min = 0;  // Minimi kulman raja, joka voidaan satunnaisesti generoida.
    float max = 360;  // Maximi kulman raja, joka voidaan satunnaisesti generoida.



    public float currentViewDistance = 0f; // oletuksena nolla
    [SerializeField] float defaultViewDistance;



    void Start()
    {

        mesh = new Mesh();  // Luodaan mesh verkko
        GetComponent<MeshFilter>().mesh = mesh; // referenssi FieldOfView mesh-komponenttiin
        

        // Random generaattorin siemen:
        Random.InitState(12345);

        // Alussa verkon distance on 0, joka kasvatetaan hitaasti alirutiinin avulla:
        StartCoroutine(IncreaseViewDistance(3f, currentViewDistance, defaultViewDistance));
    }
    /************************************************************FixedUpdate/toiminnot, jotka riippuvat ajasta***********************************************************/
    private void FixedUpdate()
    {
        currentDeltaTime = Time.deltaTime; // Päivitetään aikamuuttuja





        previousPosition = transform.position;

        /************************************************************Ajan laskenta***********************************************************/
        // Vihollinen voi jahtaa pelaajan näkemättä tietyn ajan.
        if (currentAttentionTime < maxAttentionTime && enemyBehavior.currentState == EnemyBehavior.EnemyState.chase)
        {
            currentAttentionTime += currentDeltaTime;


        }
        else if (enemyBehavior.currentState == EnemyBehavior.EnemyState.chase)
        {
            Debug.Log("toimii");

            enemyBehavior.currentState = EnemyBehavior.EnemyState.investigate;
            currentViewDistance = 0;
            StartCoroutine(IncreaseViewDistance(3f, currentViewDistance, defaultViewDistance));

        }

        if (enemyBehavior.currentState == EnemyBehavior.EnemyState.investigate && currentInvestigationTimer < maxInvestigationTime)
        {

            currentInvestigationTimer += currentDeltaTime;




        }
        else if (enemyBehavior.currentState == EnemyBehavior.EnemyState.investigate && currentInvestigationTimer > maxInvestigationTime)
        {


            enemyBehavior.needClosestPoint = true;
            enemyBehavior.currentState = EnemyBehavior.EnemyState.patroll;

        }
    }
    /*****************************************************************Update*******************************************************************/
    private void Update()
    {
        float previousAngle = Angle;

        Vector3 originPos = Vector3.zero; // posiitio, josta viivat/säteet tulevat. Nollataan alussa.

        rayCount = 50;

        float angleIncrease = fov / rayCount; // yksittäisten viivojen välin leveys
        // float defaultViewDistance = 6f; // Näköalueen pituus


        Vector3[] vertices = new Vector3[rayCount + 1 + 1];  // taulukko kulmista
        Vector2[] uv = new Vector2[vertices.Length];         // taulukko kulmien pisteiden sijainnista, joihin yhdistetään textuuri
        int[] triangles = new int[rayCount * 3];             // yksittäiset kolmiot/triangelit (triangelin pisteet tallennetaan tähän)

        vertices[0] = originPos;

        int vertexIndex = 1;
        int triangleIndex = 0;

        /****************************************************Ensimmäinen Mesh-verkko**********************************************************/
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D rayCastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(Angle), currentViewDistance, layerMask);

            if (rayCastHit2D.collider == null)
            {
                vertex = GetVectorFromAngle(Angle) * currentViewDistance;



            }
            else
            {
                // Jos raycast osuu pelaajan = luodaan uusi raycast pelaajan kautta:
                if (rayCastHit2D.collider.gameObject.CompareTag("Player"))
                {



                    //Debug.Log("Pelaaja löydettiin");
                    currentAttentionTime = 0;

                    enemyBehavior.currentState = EnemyBehavior.EnemyState.chase;



                    RaycastHit2D rayCastHit2D_2 = Physics2D.Raycast(transform.position, GetVectorFromAngle(Angle), currentViewDistance, layerMask2);
                    if (rayCastHit2D_2.collider == null)
                    {
                        vertex = GetVectorFromAngle(Angle) * currentViewDistance;

                    }
                    else
                    {

                        vertex = rayCastHit2D_2.point - new Vector2(transform.position.x, transform.position.y);

                    }



                    enemyBehavior.currentState = EnemyBehavior.EnemyState.chase;



                }
                else
                {

                    vertex = rayCastHit2D.point - new Vector2(transform.position.x, transform.position.y);
                }
                //Debug.Log("raycast osui esteeseen");

            }

            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;


            Angle -= angleIncrease;




        }
        // asetetaan pisteet, sijainnit ja triangelit mesh-verkkoon.
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        Angle = previousAngle;
    }


    /************************************************Liikutetaan vihollinen*****************************************************************/
    private void LateUpdate()
    {
        if (previousPosition != transform.position && enemyBehavior.currentState == EnemyBehavior.EnemyState.chase)
        {

            // Lasketaan suunta, johon mennään:
            // lasketaan kulma suunnasta (float from vector3) 
            Angle = GetAngleFromVector(GetDirection(previousPosition, transform.position)) + (fov / 2);


        }
        else if (enemyBehavior.canTurnTowardsTarget && enemyBehavior.currentState == EnemyBehavior.EnemyState.chase)
        {
            // Jos vihollinen pysähtyy --> katotaan suoraan pelaajaan
            Angle = GetAngleFromVector(GetDirection(previousPosition, Target.transform.position)) + (fov / 2);


        }
        else if (enemyBehavior.currentState == EnemyBehavior.EnemyState.patroll)
        {
            currentAttentionTime = 0;
            currentInvestigationTimer = 0;

            Angle = Mathf.Lerp(Angle, GetAngleFromVector(GetDirection(previousPosition, transform.position)) + (fov / 2), LerpSmoother);

        }
        else if (enemyBehavior.currentState == EnemyBehavior.EnemyState.investigate)
        {
            currentAttentionTime = 0;

            if (!courotineISRunning)
            {


                randomAngle = GenerateRandomAngle();

            }

            Angle = Mathf.Lerp(Angle, randomAngle + (fov / 2), LerpSmoother);

        }


    }
    /*****************************************************************GenerateRandomAngle*******************************************************************/

    float GenerateRandomAngle()
    {

        // Generoidaan satunnainen Angle
        float randomAngle2 = Random.Range(min, max);

        StartCoroutine(StopCounting());
        return randomAngle2;


    }
    /*****************************************************************StopCounting*******************************************************************/

    IEnumerator StopCounting()
    {
        courotineISRunning = true;

        yield return new WaitForSeconds(changeAngleInterval);

        courotineISRunning = false;


    }
    /*****************************************************************GetvectorFromAngle*******************************************************************/

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));



    }
    /*****************************************************************GetAngleFromVector*******************************************************************/

    public float GetAngleFromVector(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;

    }
    /*****************************************************************GetDirection*******************************************************************/

    public Vector3 GetDirection(Vector3 prev, Vector3 currentPos)
    {

        // Lasketaan suunta, johon mennään:
        Vector3 movementDirection = currentPos - prev;

        return movementDirection;

    }




    /*****************************************************************Muut metodit*******************************************************************/

    // Lisää nälöaluuen pituutta hitaasti:
    IEnumerator IncreaseViewDistance(float durationOfChange, float currentDis, float newDistance)
    {
        float elapsed = 0.0f;

        while (elapsed < durationOfChange)
        {


            currentViewDistance = Mathf.Lerp(currentDis, newDistance, elapsed / durationOfChange);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(0f);
        }
        currentViewDistance = newDistance;

    }
    // Vähentää nälöaluuen pituutta hitaasti:
    IEnumerator DecreaseViewDistance(float durationOfChange, float currentDis, float newDistance)
    {
        float elapsed = 0.0f;

        while (elapsed < durationOfChange)
        {


            currentViewDistance = Mathf.Lerp(currentDis, newDistance, elapsed / durationOfChange);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(0f);
        }
        currentViewDistance = newDistance;

    }


}
