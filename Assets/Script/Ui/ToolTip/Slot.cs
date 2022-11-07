using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;
    public Image itemVisual;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            ToolTipUi.instance.Show(item.desciption, item.name);
        }
    }

    // Update is called once per frame
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipUi.instance.Hide();
    }

    public void ClickOnslot()
    {
        Inventory.instance.OpenActionPanel(item, transform.position);
    }


}
