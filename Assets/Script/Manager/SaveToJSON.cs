using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.UI;

public class SaveToJSON : MonoBehaviour
{
    public enum UIType { Text, Image };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveToFile()
    {      
        GameObject[] allTextPanel = GameObject.FindGameObjectsWithTag("TextPanel");
        UILayout currentUILayout = new UILayout();
        foreach (GameObject currTextElem in allTextPanel)
        {
            
                VoiceTitleEdit currTextPanel = (VoiceTitleEdit)(currTextElem.GetComponent("VoiceTitleEdit"));
                TextPanel textPanelToSave = new TextPanel(currTextPanel.textTitle.text, currTextPanel.textContent.text, currTextElem.transform);
                string textPanelData = JsonUtility.ToJson(textPanelToSave);
                Debug.Log(textPanelData);
                currentUILayout.allTextPanels.Add(textPanelToSave);
                //System.IO.File.WriteAllText("Assets/Resources/data.json", textPanelData);
        }

        string UILayoutData = JsonUtility.ToJson(currentUILayout);
#if UNITY_EDITOR
        File.WriteAllText("Assets/Resources/data.json", UILayoutData);
#endif
#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
        string path = Path.Combine(Application.persistentDataPath, "data.json");
        using (TextWriter writer = File.CreateText(path))
        {
            // TODO write text here
            writer.Write(UILayoutData);
        }
#endif
        //Debug.Log(UILayoutData);

    }

    public void saveToFileTest()
    {
        GameObject[] allUIElemInScene = GameObject.FindGameObjectsWithTag("TextPanelTest");
        UILayout currentUILayout = new UILayout();
        foreach (GameObject currUIElem in allUIElemInScene)
        {
    
                DialogShell currTextPanel = (DialogShell)(currUIElem.GetComponent("DialogShell")); 
                TextPanel textPanelToSave = new TextPanel(currTextPanel.TitleText.text, currTextPanel.DescriptionText.text, currUIElem.transform);
                string textPanelData = JsonUtility.ToJson(textPanelToSave);
                Debug.Log(textPanelData);
                currentUILayout.allTextPanels.Add(textPanelToSave);
                //System.IO.File.WriteAllText("Assets/Resources/data.json", textPanelData);
            
        }

        string UILayoutData = JsonUtility.ToJson(currentUILayout);
#if UNITY_EDITOR
        File.WriteAllText("Assets/Resources/data.json", UILayoutData);
#endif
#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
        string path = Path.Combine(Application.persistentDataPath, "data.json");
        using (TextWriter writer = File.CreateText(path))
        {
            // TODO write text here
            writer.Write(UILayoutData);
        }
#endif
        Debug.Log(UILayoutData);

    }

    public void goToLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

[System.Serializable]
public class UILayout
{
    public List<TextPanel> allTextPanels = new List<TextPanel>();

}


[System.Serializable]
public class TextPanel
{
    public string m_Title;
    public string m_Content;
    public SerializedTransform m_addonSerializedTransform;

    public TextPanel(string a_Title, string a_Content, Transform a_transform)
    {
        m_Title = a_Title;
        m_Content = a_Content;
        m_addonSerializedTransform = new SerializedTransform(a_transform);
    }
}

// Found on Unity Forum (TO REF)
[System.Serializable]
public class SerializedTransform
{
    public float[] _position = new float[3];
    public float[] _rotation = new float[4];
    public float[] _scale = new float[3];


    public SerializedTransform(Transform transform, bool worldSpace = false)
    {
        _position[0] = transform.localPosition.x;
        _position[1] = transform.localPosition.y;
        _position[2] = transform.localPosition.z;

        _rotation[0] = transform.localRotation.w;
        _rotation[1] = transform.localRotation.x;
        _rotation[2] = transform.localRotation.y;
        _rotation[3] = transform.localRotation.z;

        _scale[0] = transform.localScale.x;
        _scale[1] = transform.localScale.y;
        _scale[2] = transform.localScale.z;

    }
}

