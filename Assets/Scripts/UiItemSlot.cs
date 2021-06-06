using UnityEngine;
using UnityEngine.UI;

public class UiItemSlot : MonoBehaviour
{
    [SerializeField] private UiInventory inv;
    [SerializeField] private int indexList;
    [SerializeField] private int id;

    public int GetID() => id;
    public int GetIndex() => indexList;
    
    public void SetButton(int indexList, int id)
    {
        this.indexList = indexList;
        this.id = id;
        Sprite sprite = GameplayManager.GetInstance().GetItemFromID(id).icon;
        GetComponent<Image>().sprite = sprite;
    }

    private void Awake()
    {
        inv = FindObjectOfType<UiInventory>();
    }

    public void MouseDown(RectTransform btn)
    {
        inv.MouseDown(btn);
    }

    public void MouseDrag()
    {
        inv.MouseDrag();
    }

    public void MouseUp(RectTransform btn)
    {
        inv.MouseUp(btn);
    }

    public void MouseEnterOver(RectTransform btn)
    {
        if (inv.secondParameter)
        {
            Vector2 mousePos = Input.mousePosition;
            if (inv.mousePos == mousePos)
            {
                inv.id2 = btn.GetComponent<UiItemSlot>();
                inv.SwapButtonsIDs();
            }
            inv.secondParameter = false;
        }

        Debug.Log("EnterOver");

        inv.toolTip.gameObject.SetActive(true);

        inv.MouseEnterOver(btn);
    }

    public void MouseExitOver()
    {
        Debug.Log("OverExit");
        inv.toolTip.gameObject.SetActive(false);
    }

    public void Arrastrando()
    {
        Debug.Log("Moviendo");
    }
}