using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
#endif

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
        string path = EditorUtility.OpenFilePanel("Overwrite with image", "", "jpg,png,jpeg");
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

#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)

        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".PNG");
            //filepicker.FileTypeFilter.Add("*");
            //filepicker.FileTypeFilter.Add(".jpg");

            var file = await picker.PickSingleFileAsync();
            Windows.Storage.Streams.IBuffer buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);                      
            Windows.Storage.Streams.DataReader dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer);
            var fileContent = new byte[buffer.Length];     
            dataReader.ReadBytes(fileContent);
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                var tex = new Texture2D(2, 2);
                tex.LoadImage(fileContent);

                MeshRenderer imgRenderer = (MeshRenderer)(this.gameObject.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
                imgRenderer.material.mainTexture=tex;

                m_imagePath = file.Path;

                Debug.Log("***********************************");
                string name = (file != null) ? file.Name : "No data";
                Debug.Log("Name: " + name);
                Debug.Log("***********************************");
                string path = (file != null) ? file.Path : "No data";
                Debug.Log("Path: " + path);
                Debug.Log("***********************************");

                

                //This section of code reads through the file (and is covered in the link)
                // but if you want to make your own parcing function you can 
                // ReadTextFile(path);
                //StartCoroutine(ReadTextFileCoroutine(path));

            }, false);
        }, false);

#endif
    }

    public void deleteImg()
    {
        Destroy(this.gameObject);
    }
}
