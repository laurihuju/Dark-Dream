using System.Collections;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [Header("Referenssit")]
    [SerializeField] private CameraController cameraController;
    
    [Header("Settings")]
    //Alue, jonne pelaaja siirret‰‰n
    [SerializeField] private string nextMap;

    private float waitBeforeGameManagerUpdate = 0.8f;

    /*********************************************************************Colliderin tarkinstus****************************************************/
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && EnemyBehavior.instance.canTeleport)
        {
            GameManager.instance.SetTeleportationState(true);

            yield return StartCoroutine(ScreenFader.instance.FadeToBlack());

            LightManager.instance.SetLightsForNewMap(nextMap);
          
            //Piillotetaan pelihahmo:
            other.gameObject.SetActive(false);

            //Siirret‰‰n pelihahmo:
            other.transform.position = transform.GetChild(0).transform.position;

            //Asetetaan GameManageriin nykyinen mappi
            GameManager.instance.ChangeCurrentMap(nextMap);

            other.gameObject.SetActive(true);
            //Muutetaan kameralle uudet rajat:

            cameraController.SetBounds(nextMap);

            StartCoroutine(UpdateGameManagerStateToFalse());

            yield return StartCoroutine(ScreenFader.instance.FadeToClear());
        }
    }

    private IEnumerator UpdateGameManagerStateToFalse()
    {
        yield return new WaitForSeconds(waitBeforeGameManagerUpdate);
        GameManager.instance.SetTeleportationState(false);
    }
}
