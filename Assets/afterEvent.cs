using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class afterEvent : MonoBehaviour
{
    // Velhot, jotka ilmestyv‰t:
    [SerializeField] GameObject wizards;

    // Tulipallo: (Lopulta deaktivoidaan)
    [SerializeField] GameObject fireBall;

    // Collider: (Lopulta pit‰‰ aktivoida)
    [SerializeField] GameObject collider;

    // Pelaaja
    [SerializeField] GameObject player;

    // Piste, johon pelaajaa teleportataan:
    [SerializeField] Transform destination;

    // Pelaajan camera
    [SerializeField] private CameraController cameraController;

    [Header("Effects")]

    // Kirkasvalo- UIImage:
    [SerializeField] Animator flashBang;

    // (Global Volume -GameObject)
    [SerializeField] Volume volume;

    private Bloom Bloom;
    private ChromaticAberration CA;

    // Valon voimakkuuden s‰‰tˆ:
    private float intensity;

    private Vignette vignette;

    float bloomIntens;

    // Est‰‰ OnTriggerEnter2D metodin kutsumisen monta kertaa.
    bool isActive = false;

    private void Start()
    {
        // Heataan tarvittavien effectien referenssit Global Volume -objektista:
        volume.profile.TryGet(out CA);
        volume.profile.TryGet(out Bloom);
        volume.profile.TryGet(out vignette);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActive)
        {
            isActive = true;

            Debug.Log("Sis‰‰n");

            AudioManager.instance.ChangeMusic("whisper");  

            // Deaktivoidaan tulipallo, jotta ei tule ristiriitoja:
            fireBall.SetActive(false);

            // Vaihdetaan valon voimakkuutta ja v‰ri‰ LightManagerin SetGlobalLightForMainMap avulla:
            LightManager.instance.SetGlobalLightForMainMap();
           
            // Aloitetaan prosessi aktivoimalla ensimm‰inen alirutiini:
            StartCoroutine(Activate());
        }
    }
  
    IEnumerator Activate()
    {
        // K‰ynnistet‰‰n UI Image FlashBang (kirkas valo sen alussa):
        flashBang.SetTrigger("Flash");
        yield return new WaitForSeconds(0.2f); // (T‰m‰ tehty, koska muuten velhot tulevat n‰kyville enne kirkasta valoa)

        // Aktivoidaan velhot ja polygon collider, joka vangitsee pelaajan:
        wizards.SetActive(true);
        collider.SetActive(true);
        
        // K‰ynnistet‰‰n Chromatic Aberration -effekti (violetti efekti):
        CA.intensity.value = 1;

        yield return new WaitForSeconds(16f);

        LightManager.instance.SetGlobalLightForFirstMap(); // Bloom-efekti ei n‰y kokonaan/kunnolla, jos valon voimakkuus on alhainen.
        
        // Aloitetaan Bloom-efektin kasvatus k‰ynnist‰m‰ll‰ alirutiini IncreaseBloom, johon lis‰t‰‰ Bloom-efekti alkuper‰inen arvo eli 0.12f ja maksimi m‰‰r‰ eli 6000f.
        // Lis‰ksi m‰‰ritell‰‰n kuinka kauan efektin kasvatus kest‰‰. Sama p‰tee Decrease -alirutiiniin, joka tekee saman mutta toiste p‰in:

        StartCoroutine(IncreaseBloom(0.12f, 6000f, 1f));
        Bloom.intensity.value = 6000f;

        AudioManager.instance.StopPlay("whisper");

        // Annetaan IncreseBloom -alirutiinille aikaa tehd‰n muutoksen ja sitten k‰ynnistet‰‰n seuraava alirutiini DecreaseBloom:
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(DecreaseBloom(6000f, 0.12f, 1f));

        // siirret‰‰n pelihahmo destination-gameobjecktin pisteeseen eli seuraavaan mappiin (MainMap):
        player.transform.position = destination.position;

        //Asetetaan GameManagerin nykyiseksi mapiksi MainMap:
        GameManager.instance.ChangeCurrentMap("MainMap");

        // Muutetaan kameralle uudet rajat:
        cameraController.SetBounds("MainMap");

        // Muutetaan valot valot:
        LightManager.instance.SetLightsForNewMap("MainMap");

        // Asetetaan efekteille oikeat arvot MainMappia varten:
        Bloom.intensity.value = 0.12f;
        CA.intensity.value = 0;
        
        // Deaktivoidaan velhot:
        wizards.SetActive(false);

        // Asetteaan oikeat arvot globalLightille LightManagerin kautta
        VignetteController.instance.SetVignetteForMainMap();

        // Tuhotaan afteEvent ja kaikki muut siihen liittyv‰t objektiti, jotta s‰‰stet‰‰n tehoa:

        yield return new WaitForSeconds(5f);
       Destroy(gameObject);
        
    }

    // Kasvattaa BLoom -efektin intensity-arvoa:
    IEnumerator IncreaseBloom(float v_start, float v_end, float duration)
    {
        GameManager.instance.SetTeleportationState(true);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Bloom.intensity.value = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
           
        }
        Bloom.intensity.value = v_end;
    }

    // V‰hent‰‰ BLoom -efektin intensity-arvoa:
    IEnumerator DecreaseBloom(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Bloom.intensity.value = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Bloom.intensity.value = v_end;

        GameManager.instance.SetTeleportationState(false);
    }
}
