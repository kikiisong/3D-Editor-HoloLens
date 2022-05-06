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

    private string currentTitle;
    private string currentContent;
    private DictationRecognizer dictationRecognizer_title;
    private DictationRecognizer dictationRecognizer_content;

    // Start is called before the first frame update
    void Start()
    {
        buttonTitle.OnClick.AddListener(startEditingTitle);
        buttonContent.OnClick.AddListener(startEditingContent);
        currentTitle = "";
        currentContent = "";
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void DictationRecognizer_OnDictationResult_title(string text, ConfidenceLevel confidence)
    {
        textTitle.text = currentTitle + " " + text + ".";
        currentTitle += text;
    }

    private void DictationRecognizer_OnDictationHypothesis_title(string text)
    {
        textTitle.text = currentTitle + " " + text + "...";
    }

    private void DictationRecognizer_OnDictationResult_content(string text, ConfidenceLevel confidence)
    {
        textContent.text = currentContent + " " + text + ".";
        currentContent += text;
    }

    private void DictationRecognizer_OnDictationHypothesis_content(string text)
    {
        textContent.text = currentContent + " " + text + "...";
    }

    public void startEditingTitle()
    {
        buttonTitle.OnClick.RemoveListener(startEditingTitle);
        buttonTitle.OnClick.AddListener(stopEditingTitle);
        buttonTitle.MainLabelText = "Stop Editing Title";

        PhraseRecognitionSystem.Shutdown();
        dictationRecognizer_title = new DictationRecognizer();
        dictationRecognizer_title.DictationHypothesis += DictationRecognizer_OnDictationHypothesis_title;
        dictationRecognizer_title.DictationResult += DictationRecognizer_OnDictationResult_title;

        textTitle.text = "";
        currentTitle = "";

        dictationRecognizer_title.Start();

    }

    public void stopEditingTitle()
    {
        buttonTitle.OnClick.RemoveListener(stopEditingTitle);
        buttonTitle.OnClick.AddListener(startEditingTitle);
        buttonTitle.MainLabelText = "Edit Title";

        dictationRecognizer_title.Stop();
        dictationRecognizer_title.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis_title;
        dictationRecognizer_title.DictationResult -= DictationRecognizer_OnDictationResult_title;
        dictationRecognizer_title.Dispose();
        PhraseRecognitionSystem.Restart();
    }

    public void startEditingContent()
    {
        buttonContent.OnClick.RemoveListener(startEditingContent);
        buttonContent.OnClick.AddListener(stopEditingContent);
        buttonContent.MainLabelText = "Stop Editing Content";

        PhraseRecognitionSystem.Shutdown();
        dictationRecognizer_content = new DictationRecognizer();
        dictationRecognizer_content.DictationHypothesis += DictationRecognizer_OnDictationHypothesis_content;
        dictationRecognizer_content.DictationResult += DictationRecognizer_OnDictationResult_content;

        textContent.text = "";
        currentContent = "";

        dictationRecognizer_content.Start();
    }

    public void stopEditingContent()
    {
        buttonContent.OnClick.RemoveListener(stopEditingContent);
        buttonContent.OnClick.AddListener(startEditingContent);
        buttonContent.MainLabelText = "Edit Content";

        dictationRecognizer_content.Stop();
        dictationRecognizer_content.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis_content;
        dictationRecognizer_content.DictationResult -= DictationRecognizer_OnDictationResult_content;
        dictationRecognizer_content.Dispose();
        PhraseRecognitionSystem.Restart();
    }

    public void deleteText()
    {
        Destroy(this.gameObject);
    }
}
