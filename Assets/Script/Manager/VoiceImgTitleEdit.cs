using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceImgTitleEdit : MonoBehaviour
{
    public TextMeshPro textTitle;
    public ButtonConfigHelper buttonTitle;
    private string currentTitle;
    private DictationRecognizer dictationRecognizer_title;

    // Start is called before the first frame update
    void Start()
    {
        buttonTitle.OnClick.AddListener(startEditingImgTitle);
        currentTitle = "";
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

    public void startEditingImgTitle()
    {
        buttonTitle.OnClick.RemoveListener(startEditingImgTitle);
        buttonTitle.OnClick.AddListener(stopEditingImgTitle);
        buttonTitle.MainLabelText = "Stop Editing Title";

        PhraseRecognitionSystem.Shutdown();
        dictationRecognizer_title = new DictationRecognizer();
        dictationRecognizer_title.DictationHypothesis += DictationRecognizer_OnDictationHypothesis_title;
        dictationRecognizer_title.DictationResult += DictationRecognizer_OnDictationResult_title;

        dictationRecognizer_title.Start();

    }

    public void stopEditingImgTitle()
    {
        buttonTitle.OnClick.RemoveListener(stopEditingImgTitle);
        buttonTitle.OnClick.AddListener(startEditingImgTitle);
        buttonTitle.MainLabelText = "Edit Title";

        dictationRecognizer_title.Stop();
        dictationRecognizer_title.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis_title;
        dictationRecognizer_title.DictationResult -= DictationRecognizer_OnDictationResult_title;
        dictationRecognizer_title.Dispose();
        PhraseRecognitionSystem.Restart();
    }

}
