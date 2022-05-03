using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ImportedImg : MonoBehaviour
{
    // Start is called before the first frame update
    public string m_imagePath;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useNewImg()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            MeshRenderer imgRenderer = (MeshRenderer)(this.gameObject.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture = tex;

            m_imagePath = path;
        }
#endif
    }
}
