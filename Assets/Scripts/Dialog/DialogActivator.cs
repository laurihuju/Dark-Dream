using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [SerializeField] private string blockToActivate;

    [SerializeField] private ushort dialogActivatorID;

    [SerializeField] private int questToActivate;
    [SerializeField] private int questNeedsToBeActive = -1;
    [SerializeField] private int questNeedsToBeCompletable = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DialogManager.instance.IsDialogActivated(dialogActivatorID))
        {
            bool canActivate = true;

            if (questNeedsToBeActive >= 0)
            {
                Quest questToBeActive = QuestManager.instance.GetQuest(questNeedsToBeActive);
                if (questToBeActive != null)
                {
                    if (questToBeActive.GetState() != Quest.QuestState.active)
                    {
                        canActivate = false;
                    }
                } else
                {
                    canActivate = false;
                }
            }

            if (questNeedsToBeCompletable >= 0)
            {
                Quest questToBeCompletable = QuestManager.instance.GetQuest(questNeedsToBeCompletable);
                if (questToBeCompletable != null)
                {
                    if (questToBeCompletable.GetState() != Quest.QuestState.canBeCompleted)
                    {
                        canActivate = false;
                    }
                }
                else
                {
                    canActivate = false;
                }
            }

            if (canActivate)
            {
                DialogManager.instance.DialogActivated(dialogActivatorID);
                DialogManager.instance.GetFlowchart().ExecuteBlock(blockToActivate);

                QuestManager.instance.ActivateQuest(questToActivate);
            }
        }
    }
}
