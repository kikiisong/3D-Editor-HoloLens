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
        buttonTitle.OnClick.RemoveListener(startEditingTitle);
        buttonTitle.OnClick.AddListener(stopEditingTitle);
        textTitle.text += " start ";
    }

    public void stopEditingTitle()
    {
        buttonTitle.OnClick.RemoveListener(stopEditingTitle);
        buttonTitle.OnClick.AddListener(startEditingTitle);
        textTitle.text += " stop ";
    }

    public void startEditingContent()
    {
        buttonContent.OnClick.RemoveListener(startEditingContent);
        buttonContent.OnClick.AddListener(stopEditingContent);
        textContent.text += " start ";
    }

    public void stopEditingContent()
    {
        buttonContent.OnClick.RemoveListener(stopEditingContent);
        buttonContent.OnClick.AddListener(startEditingContent);
        textContent.text += " stop ";
    }
}
