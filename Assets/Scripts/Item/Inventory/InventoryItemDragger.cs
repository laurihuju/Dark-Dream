using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItemDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isDragging;

    private Vector3 originalPos;

    private int ownItemIndex = -1;

    [SerializeField] private TextMeshProUGUI tooltipText;

    private static bool isTooltipActive;

    private int canUseCursor = 1;
    private int cannotUseCursor = 2;

    private void Start()
    {
        originalPos = transform.localPosition;

        for (int i = 0; i < Inventory.instance.GetSlots().Length; i++)
        {
            if (Inventory.instance.GetSlots()[i].GetBackground() == transform.parent.gameObject)
            {
                ownItemIndex = i;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.SetAsLastSibling();
        isDragging = true;

        isTooltipActive = false;
        CursorManager.instance.SetCursor(0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        transform.localPosition = originalPos;

        List <RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach(RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("InventorySlot"))
            {
                for (int i = 0; i < Inventory.instance.GetSlots().Length; i++)
                {
                    if (Inventory.instance.GetSlots()[i].GetBackground() == result.gameObject)
                    {
                        if (i != ownItemIndex && ownItemIndex >= 0)
                        {
                            ItemStorage.instance.ChangeItemStackPositions(i, ownItemIndex);
                            Inventory.instance.UpdateInventory();
                        }
                        return;
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isTooltipActive = true;

        int typeID = ItemStorage.instance.GetContent()[ownItemIndex].GetItemTypeInt();
        if (typeID == -1)
            return;
        ItemType type = ItemManager.instance.GetItemType((ushort)typeID);
        if (type == null)
            return;
        tooltipText.text = type.GetItemName();

        if (type.CanUse())
            CursorManager.instance.SetCursor(canUseCursor);
        else
            CursorManager.instance.SetCursor(cannotUseCursor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isTooltipActive = false;

        CursorManager.instance.SetCursor(0);
    }

    public static void InactivateTooltip()
    {
        isTooltipActive = false;
    }

    public static bool IsTooltipActive()
    {
        return isTooltipActive;
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
        }
    }
}
