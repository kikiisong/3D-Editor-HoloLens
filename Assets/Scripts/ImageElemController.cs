using System.Collections;
using System.Collections.Generic;
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
//#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Overwrite with jpg", "", "jpg");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);

            GameObject a = Instantiate(imgPrefab, new Vector3(-0.3661f, 0, 0.2f), Quaternion.identity);
            MeshRenderer b = (MeshRenderer)(a.transform.GetChild(1).gameObject.GetComponent("MeshRenderer"));
            b.material.mainTexture=tex;
        }
       
     
    
//#elif WINDOWS_UWP
        /*var picker = new Windows.Storage.Pickers.FileOpenPicker();
        picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
        {
             // Application now has read/write access to the picked file
             this.textBlock.Text = "Picked photo: " + file.Name;
        }
        else
        {
            this.textBlock.Text = "Operation cancelled.";
        }*/
//#endif

        //Instantiate(imgPrefab, new Vector3(-0.3661f, 0, 0.2f), Quaternion.identity);
    }
}
