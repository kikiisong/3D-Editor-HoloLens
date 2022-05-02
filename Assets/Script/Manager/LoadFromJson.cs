using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadFromJson : MonoBehaviour
{
    public GameObject textPanelPrefab;
    public GameObject debugPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadFromFile()
    {
        DialogShell texts = (DialogShell)(debugPanel.GetComponent("DialogShell"));
#if UNITY_EDITOR
        string filePath = "Assets/Resources/data.json";
#endif

#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
        string filePath = Path.Combine(Application.persistentDataPath, "data.json");
        texts.DescriptionText.text += " " + filePath;
#endif
        string UILayoutData = System.IO.File.ReadAllText(filePath);
        texts.DescriptionText.text += " " + "UI Layout: " + UILayoutData;
        /*using (StreamReader reader = File.OpenText(filePath))
        {
            result = new char[reader.BaseStream.Length];
            await reader.ReadAsync(result, 0, (int)reader.BaseStream.Length);
        }*/

        UILayout currentUILayout = JsonUtility.FromJson<UILayout>(UILayoutData);
        //Debug.Log(currentUILayout.allTextPanels.ToString());
        foreach (TextPanel currTextPanel in currentUILayout.allTextPanels)
        {
            //Debug.Log(JsonUtility.ToJson(currTextPanel));
            InstantiateTextPanel(currTextPanel);
        }
    }

    public void InstantiateTextPanel(TextPanel m_textPanel)
    {
        GameObject currPanel = Instantiate(textPanelPrefab, new Vector3(m_textPanel.m_addonSerializedTransform._position[0], m_textPanel.m_addonSerializedTransform._position[1], m_textPanel.m_addonSerializedTransform._position[2]), 
            new Quaternion(m_textPanel.m_addonSerializedTransform._rotation[1], m_textPanel.m_addonSerializedTransform._rotation[2], m_textPanel.m_addonSerializedTransform._rotation[3],
            m_textPanel.m_addonSerializedTransform._rotation[0])); 
        currPanel.transform.localScale = new Vector3(m_textPanel.m_addonSerializedTransform._scale[0], m_textPanel.m_addonSerializedTransform._scale[1], m_textPanel.m_addonSerializedTransform._scale[2]);
        DialogShell texts = (DialogShell)(currPanel.GetComponent("DialogShell"));
        texts.TitleText.text = m_textPanel.m_Title;
        texts.DescriptionText.text = m_textPanel.m_Content;
    }
}

