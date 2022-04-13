using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR 
    using UnityEditor;
#endif

public class ImageElemController : MonoBehaviour
{
    public GameObject imgPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createImg()
    {
        GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            //GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture=tex;
        }
#endif
    }
}
