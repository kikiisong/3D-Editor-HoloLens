using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;

#if UNITY_EDITOR 
    using UnityEditor;
#elif WINDOWS_UWP // We only have these namespaces if on an UWP device
    using Windows.Storage;
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

    public async Task createImg()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture=tex;
        }

#elif WINDOWS_UWP
        var picker = new Windows.Storage.Pickers.FileOpenPicker();
        picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
        {
             // Application now has read/write access to the picked file
             Windows.Storage.Streams.IBuffer buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);                      
             Windows.Storage.Streams.DataReader dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer);
             var fileContent = new byte[buffer.Length];     
             dataReader.ReadBytes(fileContent);

            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            GameObject imgPanel = Instantiate(imgPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
            MeshRenderer imgRenderer = (MeshRenderer)(imgPanel.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            imgRenderer.material.mainTexture=tex;
        }
#endif

        //Instantiate(imgPrefab, new Vector3(-0.3661f, 0, 0.2f), Quaternion.identity);
    }
}
