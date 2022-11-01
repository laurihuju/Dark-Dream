using UnityEngine;

/// <summary>
/// Hoitaa pelaajan liikkumisen
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Input input;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;
    private float maxSpeed;
    private bool isSprinting;
    private bool canMove;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float sprintAnimSpeed;

    [SerializeField] private float sprintStaminaUsage;

    private void Awake()
    {
        //Tuhotaan gameobject, jos pelaaja on jo olemassa.
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Määritellään input ja rigidbody2d.
        input = new Input();
        rb = GetComponent<Rigidbody2D>();

        //Asetetaan metodit, joita kutsutaan, kun nappuloita painetaan ja lopetetaan painamasta.
        input.Game.Movement.performed += context => Move(context.ReadValue<Vector2>());
        input.Game.Movement.canceled += context => Move(context.ReadValue<Vector2>());
        input.Game.Sprint.performed += _ => Sprint();
        input.Game.Sprint.canceled += _ => Walk();
        anim = GetComponent<Animator>();
        //Asetetaan pelin alussa pelaajan maksiminopeudeksi kävelynopeus ja sallitaan liikkuminen
        maxSpeed = walkSpeed;
        canMove = true;

        //Asetetaan instance-muuttujan arvo.
        instance = this;
    }

    private void OnDestroy()
    {
        input.Disable();
    }

    /// <summary>
    /// Laskee liikkumisen tavoitenopeuden sille annetun suunnan perusteella. Kutsutaan input systemistä.
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        movement = direction * maxSpeed;

        MovementAminations(movement);
    }

    private void MovementAminations(Vector2 movement)
    {
        if (canMove && movement.magnitude != 0)
        {
            anim.SetFloat("MoveX", movement.x);
            anim.SetFloat("MoveY", movement.y);
        }
    }


    /// <summary>
    /// Muutetaan pelaajan nopeutta lerp-metodin avulla tasaisesti kohti tavoitenopeutta. Näin saavutetaan tasainen pelaajan kiihtyminen ja hidastuminen.
    /// </summary>
    private void FixedUpdate()
    {
        if (isSprinting && (Mathf.Abs(rb.velocity.x) > 0.1 || Mathf.Abs(rb.velocity.y) > 0.1)) //Suoritetaan vain, jos pelaaja juoksee
        {
            //Jos pelaajalla on riittävästi staminaa juoksemiseen ja pelaaja liikkuu, vähennetään pelaajan staminaa
            if (HealthManager.instance.GetCurrentStamina() - sprintStaminaUsage >= 0)
            {
                if (movement.magnitude > 0)
                {
                    HealthManager.instance.ChangeCurrentStamina(-sprintStaminaUsage);
                }
            } else //Jos pelaajalla ei ole riittävästi staminaa juoksemiseen, siirretään pelaaja kävelytilaan.
            {
                Walk();
            }
        }

        if (Mathf.Abs(rb.velocity.x) > 0.4 || Mathf.Abs(rb.velocity.y) > 0.4)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (!canMove) //Jos pelaajan liikkuminen on estetty, pelaajan tavoitenopeudeksi asetetaan 0.
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, acceleration); //Muutetaan pelaajan nopeutta tasaisesti kohti tavoitenopeutta

            //Jos pelaajan nopeus on erittäin lähellä tavoitenopeutta, asetetaan nopeus suoraan tavoitenopeudeksi.
            if (Mathf.Abs(rb.velocity.x) < 0.001)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (Mathf.Abs(rb.velocity.y - movement.y) < 0.001)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        } else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, movement, acceleration); //Muutetaan pelaajan nopeutta tasaisesti kohti tavoitenopeutta

            //Jos pelaajan nopeus on erittäin lähellä tavoitenopeutta, asetetaan nopeus suoraan tavoitenopeudeksi.
            if (Mathf.Abs(rb.velocity.x - movement.x) < 0.001)
            {
                rb.velocity = new Vector2(movement.x, rb.velocity.y);
            }
            if (Mathf.Abs(rb.velocity.y - movement.y) < 0.001)
            {
                rb.velocity = new Vector2(rb.velocity.x, movement.y);
            }
        }
    }

    /// <summary>
    /// Otetaan input system käyttöön.
    /// </summary>
    private void OnEnable()
    {
        input.Enable();
    }

    /// <summary>
    /// Asettaa pelaajan nopeudeksi juoksunopeuden.
    /// </summary>
    public void Sprint()
    {
        if (HealthManager.instance.GetCurrentStamina() - sprintStaminaUsage < 0) 
            return;

        movement = movement / maxSpeed;

        maxSpeed = sprintSpeed;

        movement = movement * maxSpeed;
        isSprinting = true;

        anim.speed = sprintAnimSpeed;
    }

    /// <summary>
    /// Asettaa pelaajan nopeudeksi kävelynopeuden.
    /// </summary>
    public void Walk()
    {
        movement = movement / maxSpeed;

        maxSpeed = walkSpeed;

        movement = movement * maxSpeed;
        isSprinting = false;

        anim.speed = 1;
    }

    /// <summary>
    /// Asettaa pelaajan kävelynopeuden.
    /// </summary>
    /// <param name="speed"></param>
    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;

        if (!isSprinting)
        {
            movement = movement / maxSpeed;

            maxSpeed = speed;

            movement = movement * maxSpeed;
        }
    }

    /// <summary>
    /// Palauttaa pelaajan kävelynopeuden.
    /// </summary>
    /// <returns></returns>
    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    /// <summary>
    /// Asettaa pelaajan juoksunopeuden.
    /// </summary>
    /// <param name="speed"></param>
    public void SetSprintSpeed(float speed)
    {
        sprintSpeed = speed;

        if (isSprinting)
        {
            movement = movement / maxSpeed;

            maxSpeed = speed;

            movement = movement * maxSpeed;
        }
    }

    /// <summary>
    /// Palauttaa pelaajan juoksunopeuden.
    /// </summary>
    /// <returns></returns>
    public float GetSprintSpeed()
    {
        return sprintSpeed;
    }

    /// <summary>
    /// Estää pelaajaa liikkumasta.
    /// </summary>
    public void DisableMoving()
    {
        canMove = false;
        MovementAminations(Vector2.zero);
    }

    /// <summary>
    /// Antaa pelaajan liikkua.
    /// </summary>
    public void AllowMoving()
    {
        canMove = true;
        MovementAminations(movement);
    }

    /// <summary>
    /// Palauttaa input-luokan instancen, jonka kautta luetaan syötettä.
    /// </summary>
    /// <returns></returns>
    public Input GetInput()
    {
        return input;
    }
}
