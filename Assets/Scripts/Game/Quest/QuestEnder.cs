using UnityEngine;

public class QuestEnder : MonoBehaviour
{
    [SerializeField] private string blockToActivate;

    [SerializeField] private int questToComplete;
    [SerializeField] private int questToActivate = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && QuestManager.instance.GetQuest(questToComplete).GetState() == Quest.QuestState.canBeCompleted)
        {
            Quest quest = QuestManager.instance.GetQuest(questToComplete);
            if (quest == null)
                return;
            if (quest.GetItemToCollect() > ushort.MaxValue)
                return;

            if(blockToActivate != "")
            {
                DialogManager.instance.GetFlowchart().ExecuteBlock(blockToActivate);
            }

            QuestManager.instance.CompleteQuest(quest);

            if(quest.GetItemToCollect() >= 0)
            {
                for (int i = 0; i < quest.GetCollectAmount(); i++)
                {
                    ItemStorage.instance.RemoveItem((ushort)quest.GetItemToCollect());
                }
            }

            if(quest.GetRewardItem() >= 0)
            {
                if(ItemManager.instance.GetItemType((ushort)quest.GetRewardItem()) != null)
                {
                    ItemStorage.instance.AddItem((ushort)quest.GetRewardItem());
                }
            }

            if(questToActivate >= 0)
            {
                QuestManager.instance.ActivateQuest(questToActivate);
            }

            SaveManager.Save();
        }
    }
}
