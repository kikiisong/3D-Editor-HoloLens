using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

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
        buttonTitle.OnClick.RemoveListener(startEditingTitle);
        buttonTitle.OnClick.AddListener(stopEditingTitle);
        buttonTitle.MainLabelText = "Stop Editing Title";
        PhraseRecognitionSystem.Shutdown();

    }

    public void stopEditingTitle()
    {
        textTitle.text += " stop ";
        buttonTitle.OnClick.RemoveListener(stopEditingTitle);
        buttonTitle.OnClick.AddListener(startEditingTitle);
        buttonTitle.MainLabelText = "Edit Title";
        PhraseRecognitionSystem.Restart();
    }

    public void startEditingContent()
    {
        buttonContent.OnClick.RemoveListener(startEditingContent);
        buttonContent.OnClick.AddListener(stopEditingContent);
        buttonContent.MainLabelText = "Stop Editing Content";
        textContent.text += " start ";
    }

    public void stopEditingContent()
    {
        buttonContent.OnClick.RemoveListener(stopEditingContent);
        buttonContent.OnClick.AddListener(startEditingContent);
        buttonContent.MainLabelText = "Edit Content";
        textContent.text += " stop ";
    }
}
