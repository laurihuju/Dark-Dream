using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] private Quest[] quests;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void ActivateQuest(int questID)
    {
        if (questID < 0 || questID >= quests.Length)
            return;
        if (quests[questID] == null)
            return;
        if (quests[questID].GetState() != Quest.QuestState.inactive)
            return;

        quests[questID].SetState(Quest.QuestState.active);
        Quest questToDisplay = GetQuestToDisplay();
        InventoryMenu.instance.SetDisplayQuest(questToDisplay, GetQuestID(questToDisplay));
        UpdateQuestState(questID);
    }

    public void CompleteQuest(Quest quest)
    {
        quest.SetState(Quest.QuestState.completed);

        Quest questToDisplay = GetQuestToDisplay();
        InventoryMenu.instance.SetDisplayQuest(questToDisplay, GetQuestID(questToDisplay));
        InventoryMenu.instance.UpdateQuestDisplay();
    }

    private void UpdateQuestState(int questID)
    {
        if (questID < 0 || questID >= quests.Length)
            return;
        if (quests[questID] == null)
            return;
        if (quests[questID].GetState() == Quest.QuestState.completed || quests[questID].GetState() == Quest.QuestState.inactive)
            return;
        if(quests[questID].GetItemToCollect() < 0)
        {
            quests[questID].SetState(Quest.QuestState.canBeCompleted);
            if (InventoryMenu.instance.GetDisplayQuestID() == questID)
            {
                InventoryMenu.instance.UpdateQuestDisplay();
            }
            return;
        }
        int itemAmount = ItemStorage.instance.GetItemAmount((ushort)quests[questID].GetItemToCollect());
        if (itemAmount >= quests[questID].GetCollectAmount())
        {
            quests[questID].SetState(Quest.QuestState.canBeCompleted);
        } else
        {
            quests[questID].SetState(Quest.QuestState.active);
        }
        if(InventoryMenu.instance.GetDisplayQuestID() == questID)
        {
            InventoryMenu.instance.SetDisplayQuestItemAmount(itemAmount);
            InventoryMenu.instance.UpdateQuestDisplay();
        }
    }

    public void UpdateQuestsWithItem(ushort itemTypeID)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if(quests[i] != null)
            {
                if (quests[i].GetItemToCollect() == itemTypeID)
                {
                    UpdateQuestState(i);
                }
            }
        }
    }

    public Quest GetQuest(int questID)
    {
        if (questID < 0 || questID >= quests.Length)
            return null;

        return quests[questID];
    }

    public int GetQuestID(Quest quest)
    {
        for(int i = 0; i < quests.Length; i++)
        {
            if(quests[i] == quest)
            {
                return i;
            }
        }

        return -1;
    }

    public Quest.QuestState[] GetQuestStates()
    {
        Quest.QuestState[] states = new Quest.QuestState[quests.Length];
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i] != null)
            {
                states[i] = quests[i].GetState();
            }
        }
        return states;
    }

    public void SetQuestStates(Quest.QuestState[] states)
    {
        if (states.Length < quests.Length)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if(quests[i] != null)
                {
                    quests[i].SetState(states[i]);
                }
            }
        } else
        {
            for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i] != null)
                {
                    quests[i].SetState(states[i]);
                }
            }
        }

        Quest questToDisplay = GetQuestToDisplay();
        InventoryMenu.instance.SetDisplayQuest(questToDisplay, GetQuestID(questToDisplay));

        for(int i = 0; i < quests.Length; i++)
        {
            UpdateQuestState(i);
        }
    }

    public Quest GetQuestToDisplay()
    {
        int biggestQuestNumber = -1;
        int biggestQuestNumberIndex = -1;
        for(int i = 0; i < quests.Length; i++)
        {
            if(quests[i] != null)
            {
                if (quests[i].GetOrder() > biggestQuestNumber && (quests[i].GetState() == Quest.QuestState.active || quests[i].GetState() == Quest.QuestState.canBeCompleted))
                {
                    biggestQuestNumber = quests[i].GetOrder();
                    biggestQuestNumberIndex = i;
                }
            }
        }

        if (biggestQuestNumberIndex == -1)
            return null;
        return quests[biggestQuestNumberIndex];
    }
}