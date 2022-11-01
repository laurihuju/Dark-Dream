using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [SerializeField] private int maxHP; //HP:n maksimim‰‰r‰
    public int currentHP; //Nykyinen HP:n m‰‰r‰

    [SerializeField] private Slider healthBarSlider; //Terveyspalkin slider
    [SerializeField] private TextMeshProUGUI healthBarText; //Terveyspalkin teksti

    [SerializeField] private float maxStamina; //Staminan maksimim‰‰r‰
    private float currentStamina; //Nykyinen staminan m‰‰r‰

    [SerializeField] private Slider staminaBarSlider; //Staminapalkin slider
    [SerializeField] private TextMeshProUGUI staminaBarText; //Staminapalkin teksti

    [SerializeField] private float staminaGenerateAmount; //M‰‰r‰, joka staminaa generoidaan yhden generointikerran aikana
    [SerializeField] private float staminaGenerateCooldown; //Aika, joka odotetaan staminan v‰hent‰misen j‰lkeen ennen kuin staminaa aletaan generoimaan uudelleen
    private float nextStaminaGenerateTime; //Muuttuja, johon m‰‰ritell‰‰n aika, jolloin seuraavan kerran staminaa voidaan generoida
    private bool isStaminaGenerating; //Muuttuja, joka m‰‰rittelee, voidaanko staminaa generoida t‰ll‰ hetkell‰
    //private Coroutine staminaGenerationStartCo; //Alirutiini, joka aloittaa staminan generoinnin uudelleen

    [SerializeField] GameObject DeathWindow;


    private void Awake()
    {
        //Jos toinen HealthManager on jo olemassa, tuhotaan t‰m‰ HealthManager
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        currentHP = maxHP; //Asetetaan nykyiseksi HP:n m‰‰r‰ksi HP:n maksimim‰‰r‰
        healthBarText.text = "HP: " + currentHP + " / " + maxHP; //Asetetaan terveyspalkin teksti
        currentStamina = maxStamina; //Asetetaan nykyiseksi staminan m‰‰r‰ksi staminan maksimim‰‰r‰
        staminaBarText.text = "Stamina: " + Mathf.RoundToInt(currentStamina) + " / " + maxStamina; //Asetetaan staminapalkin teksti

        instance = this; //Asetetaan instance-muuttujaan t‰m‰ HealthManager
    }

    private void Start()
    {
        isStaminaGenerating = true;
    }

    private void Update()
    {
        //Jos staminaa voidaan generoida, generoidaan lis‰‰ staminaa
        if (isStaminaGenerating)
        {
            ChangeCurrentStamina(staminaGenerateAmount * Time.deltaTime);
        }
        else if (Time.time >= nextStaminaGenerateTime) //Jos staminaa ei voida generoida, mutta nykyinen aika on ohittanut seuraavan ajan, jolloin staminaa voidaan generoida, m‰‰ritell‰‰n, ett‰ staminaa voidaan generoida.
        {
            isStaminaGenerating = true;
        }

        UpdateBar(healthBarSlider, healthBarText, "HP", currentHP, maxHP); //P‰ivitet‰‰n terveyspalkin t‰ytyys ja teksti
        UpdateBar(staminaBarSlider, staminaBarText, "Stamina", currentStamina, maxStamina); //P‰ivitet‰‰n staminapalkin t‰ytyys ja teksti
    }
    /// <summary>
    /// Muuttaa nykyisen HP:n m‰‰r‰‰ parametrina annetulla kokonaisluvulla. HP:n m‰‰r‰‰ voi lis‰t‰ antamalla positiivisen luvun ja v‰hent‰‰ antamalla negatiivisen luvun.
    /// </summary>
    /// <param name="changeAmount"></param>
    public void ChangeCurrentHP(int changeAmount)
    {
        currentHP += changeAmount; //Lis‰t‰‰n parametrina annettu HP:n muutos nykyiseen HP:n m‰‰r‰‰n

        //Estet‰‰n HP:n maksimim‰‰r‰n ylittyminen ja 0:n alittuminen
        if (currentHP > maxHP)
            currentHP = maxHP;
        if (currentHP < 0)
        {
            currentHP = 0;
            Death();
        }
    }

    /// <summary>
    /// Muuttaa nykyisen staminan m‰‰r‰‰ parametrina annetulla kokonaisluvulla. Staminan m‰‰r‰‰ voi lis‰t‰ antamalla positiivisen luvun ja v‰hent‰‰ antamalla negatiivisen luvun.
    /// </summary>
    /// <param name="changeAmount"></param>
    public void ChangeCurrentStamina(float changeAmount)
    {
        currentStamina += changeAmount; //Lis‰t‰‰n parametrina annettu staminan muutos nykyiseen staminan m‰‰r‰‰n

        //Estet‰‰n staminan maksimim‰‰r‰n ylittyminen ja 0:n alittuminen
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
        if (currentStamina < 0)
            currentStamina = 0;

        //Jos staminaa v‰hennet‰‰n, estet‰‰n staminan generoituminen m‰‰ritellyksi ajaksi
        if(changeAmount < 0)
        {
            isStaminaGenerating = false;
            nextStaminaGenerateTime = Time.time + staminaGenerateCooldown;
        }
    }

    public bool IsFullHP()
    {
        return currentHP >= maxHP;
    }

    /// <summary>
    /// Palauttaa nykyisen HP:n m‰‰r‰n
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHP()
    {
        return currentHP;
    }

    /// <summary>
    /// Palauttaa nykyisen staminan m‰‰r‰n
    /// </summary>
    /// <returns></returns>
    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    /// <summary>
    /// Asettaa nykyisen HP:n arvon.
    /// </summary>
    /// <param name="HP"></param>
    public void SetCurrentHP(int HP)
    {
        currentHP = HP;
    }

    /// <summary>
    /// Asettaa nykyisen staminan arvon.
    /// </summary>
    /// <param name="stamina"></param>
    public void SetCurrentStamina(float stamina)
    {
        currentStamina = stamina;
    }

    /// <summary>
    /// Asettaa palkin t‰ytyyden parametrina annettujen arvojen mukaiseksi. Asettaa myˆs palkin tekstin.
    /// </summary>
    /// <param name="barSlider"></param>
    /// <param name="barText"></param>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    private void UpdateBar(Slider barSlider, TextMeshProUGUI barText, string barDescription, float currentValue, float maxValue)
    {
        //Jos palkin t‰ytyys on jo halutussa arvossa, ei arvon p‰ivitt‰mist‰ tarvitse suorittaa
        if (barSlider.value == currentValue / maxValue)
            return;

        //Asetetaan palkin t‰ytyydeksi uusi arvo
        barSlider.value = currentValue / maxValue;

        //Asetetaan palkin tekstiksi palkin t‰ytyytt‰ vastaava arvo
        barText.text = barDescription + ": " + Mathf.RoundToInt(currentValue) + " / " + maxValue;
    }

    private void Death()
    {
        EnemyBehavior.instance.canAttack = false;
        DeathWindow.SetActive(true);
        AudioManager.instance.ChangeMusic("laugh");
    }

    public int GetMaxHP()
    {
        return maxHP;



    }

    
}
