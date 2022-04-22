using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoiceTitleEdit : MonoBehaviour
{
    public TextMeshPro textContent;
    public TextMeshPro textTitle;
    // Start is called before the first frame update
    void Start()
    {
        
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
