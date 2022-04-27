using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromJson : MonoBehaviour
{
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
        string filePath = "Assets/Resources/data.json";
        string UILayoutData = System.IO.File.ReadAllText(filePath);
        UILayout currentUILayout = JsonUtility.FromJson<UILayout>(UILayoutData);
        Debug.Log(currentUILayout.allTextPanels.ToString());
    }
}

