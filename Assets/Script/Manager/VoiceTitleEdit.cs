using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoiceTitleEdit : MonoBehaviour
{
    public TextMeshPro textContent;
    public TextMeshPro textTitle;
    public ButtonConfigHelper buttonTitle;
    public ButtonConfigHelper buttonContent;

    // Start is called before the first frame update
    void Start()
    {
        buttonTitle.OnClick.AddListener(startEditingTitle);
        buttonContent.OnClick.AddListener(startEditingContent);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void startEditingTitle()
    {
        textTitle.text += " start ";
    }

    public void stopEditingTitle()
    {
        textTitle.text += " stop ";
    }

    public void startEditingContent()
    {
        textContent.text += " start ";
    }

    public void stopEditingContent()
    {
        textContent.text += " stop ";
    }
}
