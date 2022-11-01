using System.Collections;
using UnityEngine;

public class EndEvent : MonoBehaviour
{
    [SerializeField] Collider2D collider;

    [SerializeField] Transform nextDestination;

    [SerializeField] Animator lastFlash;

    [SerializeField] GameObject lastDialog;

    [SerializeField] GameObject endGame;

    [SerializeField] GameObject teleportation;

    [SerializeField] GameObject door_closed;

    [SerializeField] GameObject door_opened;

    [Header("Referenssit")]
    [SerializeField] private CameraController cameraController;

    //public bool canActivateCollider;
    [SerializeField] private int questNeededToBeActive = -1;
    
    void Start()
    {
        collider.enabled = false;
        lastDialog.SetActive(false);

        endGame.SetActive(false);

    }

    
    void Update()
    {
        if(questNeededToBeActive >= 0)
        {
            if (QuestManager.instance.GetQuest(questNeededToBeActive).GetState() == Quest.QuestState.canBeCompleted)
            {
                collider.enabled = true;

                AudioManager.instance.Play("moan");

                door_closed.SetActive(false);
                door_opened.SetActive(true);
            }
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        LightManager.instance.SetGlobalLightForFirstMap();
    
        lastFlash.SetTrigger("Start");

        AudioManager.instance.StopPlay("chase");
        AudioManager.instance.StopPlay("gameBG");


        yield return new WaitForSeconds(0.6f);
        LightManager.instance.SetOffEffects();
        cameraController.SetBounds("AnnaHouse");

        collision.transform.position = nextDestination.transform.position;

        teleportation.SetActive(false);

        
        yield return new WaitForSeconds(3.5f);

        lastDialog.SetActive(true);

        endGame.SetActive(true);

    }

}
