using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif
//#if ENABLE_WINMD_SUPPORT && UNITY_WSA
//using Windows.Storage.Pickers;
//using Windows.Storage.FileIO;
//using System;
//#endif


public class SaveLoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isEditor = true;
    public TextMeshPro messageTextPanel;
    public PhaseSwitchManager phaseSwitchManager;

    private void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void Save()
    {
        if (isEditor)
        {
            PhaseSwitchManagerSerializable serialized = new PhaseSwitchManagerSerializable(phaseSwitchManager);
            string jsonSerialized = JsonUtility.ToJson(serialized);
//#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
//        //// Solution found at:
//        //// https://stackoverflow.com/questions/66281959/system-exception-getting-called-when-assessing-openfilepicker-on-hololens/66282480#66282480
//        //UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
//        //{
//        //    var savePicker = new FileSavePicker();
//        //    savePicker.FileTypeChoices.Add("Json", new List<string>() { ".json" });
//        //    savePicker.SuggestedFileName = "data";

//        //    StorageFile file = await savePicker.PickSaveFileAsync();
//        //    UnityEngine.WSA.Application.InvokeOnAppThread(() => 
//        //    {
//        //        if(file!=null)
//        //        {
//        //            await WriteTextAsync(file, jsonSerialized);
//        //            messageTextPanel.text = "Message: "+"Saved!";
//        //        }else{
//        //            messageTextPanel.text = "Message: "+"Save failed!";
//        //        }
//        //    }, false);
//        //}, false);

//#elif UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel("Where to save", "", "data", ".json");
            File.WriteAllText(path, jsonSerialized);
            messageTextPanel.text = "Message: " + "Saved!";
//#endif
        }
    }

    public async void Load()
    {
//#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
//        // Code borrowed from:
//        // https://stackoverflow.com/questions/66281959/system-exception-getting-called-when-assessing-openfilepicker-on-hololens/66282480#66282480

//        //UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
//        //{
//        //    var filepicker = new FileOpenPicker();
//        //    // filepicker.FileTypeFilter.Add("*");
//        //    filepicker.FileTypeFilter.Add(".json");

//        //    var file = await filepicker.PickSingleFileAsync();
//        //    UnityEngine.WSA.Application.InvokeOnAppThread(() => 
//        //    {
//        //        if(file!=null){
//        //            string json = await ReadTextAsync(file);
//        //            PhaseSwitchManagerSerializable serialized = JsonUtility.FromJson<PhaseSwitchManagerSerializable>(json);
//        //            serialized.Deserialize(phaseSwitchManager);
//        //            messageTextPanel.text = "Message: "+"Loaded!";
//        //        }else
//        //        {
//        //            messageTextPanel.text = "Message: "+"Load Failed!";
//        //        }
//        //    }, false);
//        //}, false);

//#elif UNITY_EDITOR
        Debug.Log("No uwp support");
        string path = EditorUtility.OpenFilePanel("Select the saved scene.", "", "json");
        string json = File.ReadAllText(path);
        PhaseSwitchManagerSerializable serialized = JsonUtility.FromJson<PhaseSwitchManagerSerializable>(json);
        serialized.Deserialize(phaseSwitchManager);
        messageTextPanel.text = "Message: "+"Loaded!";
//#endif

    }
}
