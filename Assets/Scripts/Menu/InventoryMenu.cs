using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryMenu : MenuController
{
    public static InventoryMenu instance;

    [SerializeField] private GameObject menu;
    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private TextMeshProUGUI questAmountText;
    [SerializeField] private Image questItemImage;
    private Quest displayQuest;
    private int displayQuestID;
    private int displayQuestItemAmount;

    [SerializeField] private GameObject tooltip;

    private bool isFading;
    private bool active;
    private bool isDestroyed;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        displayQuestItemAmount = 0;
        displayQuestID = -1;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public override void ToggleMenu()
    {
        if (isDestroyed || isFading || (DialogManager.instance.IsDialogActive() && !menu.gameObject.activeSelf))
            return;
        if (DialogManager.instance.IsDialogActive())
        {
            active = false;
        } else
        {
            active = !menu.gameObject.activeSelf;
        }

        isFading = true;

        if (active)
        {
            menu.SetActive(true);
            GameManager.instance.SetInventoryState(true);

            //Jos minikartta on aktiivinen, suljetaan kartta
            if (MiniMap.instance.IsMapActive())
                MiniMap.instance.ActivateMiniMap();

            animator.SetTrigger("open");
        } else
        { 
            animator.SetTrigger("close");
        }
    }

    public void ForceDeactivateMenu()
    {
        active = false;
        isFading = true;
        animator.SetTrigger("close");
    }

    public void FadeComplete()
    {
        if (!active)
        {
            menu.SetActive(false);
            GameManager.instance.SetInventoryState(false);

            InventoryItemDragger.InactivateTooltip();
            CursorManager.instance.SetCursor(0);
        }

        isFading = false;
    }

    public void ContinueButtonClick()
    {
        ToggleMenu();
    }

    public void MainMenuButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void SetDisplayQuest(Quest quest, int questID)
    {
        if (displayQuest == quest)
            return;
        displayQuest = quest;

        displayQuestID = questID;
    }

    public void SetDisplayQuestItemAmount(int amount)
    {
        displayQuestItemAmount = amount;
    }

    public int GetDisplayQuestID()
    {
        return displayQuestID;
    }

    public void UpdateQuestDisplay()
    {
        if(displayQuest == null)
        {
            questAmountText.gameObject.SetActive(false);
            questItemImage.gameObject.SetActive(false);

            questText.text = "Sinulla ei ole aktiivisia tehtäviä.";
            return;
        }

        if (displayQuest.GetItemToCollect() >= 0 && displayQuest.GetItemToCollect() < ushort.MaxValue)
        {
            questAmountText.gameObject.SetActive(true);
            questItemImage.gameObject.SetActive(true);
            questAmountText.text = displayQuestItemAmount + " / " + displayQuest.GetCollectAmount();

            ItemType type = ItemManager.instance.GetItemType((ushort)displayQuest.GetItemToCollect());
            if (type != null)
            {
                Sprite itemSprite = type.GetTypeSprite();
                if (itemSprite != null)
                {
                    questItemImage.sprite = itemSprite;
                }
            }
        } else
        {
            questAmountText.gameObject.SetActive(false);
            questItemImage.gameObject.SetActive(false);
        }

        if (displayQuest.GetState() == Quest.QuestState.active)
        {
            questText.text = displayQuest.GetActiveText();
        } else if (displayQuest.GetState() == Quest.QuestState.canBeCompleted)
        {
            questText.text = displayQuest.GetCanBeCompletedText();
        }
    }

    public bool IsMenuActive()
    {
        return active;
    }

    private void Update()
    {
        if (InventoryItemDragger.IsTooltipActive())
        {
            if (!tooltip.activeSelf)
            {
                tooltip.SetActive(true);
                tooltip.transform.SetAsLastSibling();
            }
            tooltip.transform.position = new Vector2(UnityEngine.Input.mousePosition.x + 30, UnityEngine.Input.mousePosition.y + 30);
        } else
        {
            if (tooltip.activeSelf)
                tooltip.SetActive(false);
        }
    }
}
