using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipUi : MonoBehaviour
{
    public static ToolTipUi instance;

    public ToolTipText tooltip;

    

    void Awake()
    {
        instance = this;
    }

    public void Show(string content , string header = "")
    {
        tooltip.SetText(content, header);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
