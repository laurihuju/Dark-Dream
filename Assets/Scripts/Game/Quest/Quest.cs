using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private int itemToCollect;
    [SerializeField] private int collectAmount;

    [SerializeField] private int questOrder;

    [SerializeField] private string activeText;
    [SerializeField] private string canBeCompletedText;

    [SerializeField] private int rewardItem;

    [SerializeField] private GameObject activeMiniMapMark;
    [SerializeField] private GameObject canBeCompletedMiniMapMark;

    private QuestState state;

    [System.Serializable]
    public enum QuestState
    {
        inactive,
        active,
        canBeCompleted,
        completed
    }

    private void Awake()
    {
        state = QuestState.inactive;
    }

    public void SetState(QuestState state)
    {
        if(state == QuestState.active)
        {
            if(activeMiniMapMark != null)
                activeMiniMapMark.SetActive(true);
            if(canBeCompletedMiniMapMark != null)
                canBeCompletedMiniMapMark.SetActive(false);
        } else if (state == QuestState.canBeCompleted)
        {
            if(canBeCompletedMiniMapMark != null)
                canBeCompletedMiniMapMark.SetActive(true);
            if(activeMiniMapMark != null)
                activeMiniMapMark.SetActive(false);
        } else
        {
            if(activeMiniMapMark != null)
                activeMiniMapMark.SetActive(false);
            if(canBeCompletedMiniMapMark != null)
                canBeCompletedMiniMapMark.SetActive(false);
        }
        this.state = state;
    }

    public QuestState GetState()
    {
        return state;
    }

    public int GetItemToCollect()
    {
        return itemToCollect;
    }

    public int GetCollectAmount()
    {
        return collectAmount;
    }

    public int GetOrder()
    {
        return questOrder;
    }

    public string GetActiveText()
    {
        return activeText;
    }

    public string GetCanBeCompletedText()
    {
        return canBeCompletedText;
    }

    public int GetRewardItem()
    {
        return rewardItem;
    }
}
