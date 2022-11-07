using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

using TMPro;

public class ToolTipText : MonoBehaviour
{

    [SerializeField]    
    private TMP_Text headerText;

    [SerializeField]
    private TMP_Text contentText;


    [SerializeField]
    private RectTransform rectTransform;

    // Start is called before the first frame update
    public void SetText(string content, string header = "")
    {
        if(header == "")
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }
        contentText.text = content;
    }

    public void Update()
    {
        Vector2 mousepos = Mouse.current.position.ReadValue();
        float pivotX = mousepos.x / Screen.width;
        float pivoty = mousepos.y / Screen.width;

        rectTransform.pivot = new Vector2(pivotX, pivoty);

        transform.position = mousepos;
    }
}
