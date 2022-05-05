using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsImpL;
using Microsoft.MixedReality.Toolkit.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Microsoft.MixedReality.Toolkit.Input;
#if ENABLE_WINMD_SUPPORT && UNITY_WSA
using Windows.Storage.Pickers;
using System;
#endif

public class ThreeDModelImporter : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isEditor = true;
    public string filePath;
    protected string objectName = "ImportedObj";
    protected ImportOptions importOptions = new ImportOptions();
    protected ObjectImporter objImporter;
    public GameObject buttonPinMenu;
    public GameObject threeDSubMenu;
    List<Action> customOnImporteCompletedScripts = new List<Action>();
    List<Action<GameObject, string>> customOnImportedScripts = new List<Action<GameObject, string>>();

    private void Awake()
    {
        importOptions.modelScaling = 0.001f;
        importOptions.localPosition = new Vector3(0, 0, 1);
        objImporter = gameObject.GetComponent<ObjectImporter>();
        if (objImporter == null)
        {
            objImporter = gameObject.AddComponent<ObjectImporter>();
        }
        objImporter.ImportedModel += AddHandInteractionToObj;
        foreach (var act in customOnImporteCompletedScripts)
        {
            objImporter.ImportingComplete += act;
        }
        foreach (var act in customOnImportedScripts)
        {
            objImporter.ImportedModel += act;
        }
    }

    public void AddCustomScriptOnImportCompleted(Action customScript)
    {
        if (objImporter == null)
        {
            customOnImporteCompletedScripts.Add(customScript);
        }
        else
        {
            objImporter.ImportingComplete += customScript;
        }
    }

    public void AddCustomOnImportedScript(Action<GameObject, string> customScript)
    {
        if (objImporter == null)
        {
            customOnImportedScripts.Add(customScript);
        }
        else
        {
            objImporter.ImportedModel += customScript;
        }
    }

    public void RemoveCustomOnImportedScript(Action<GameObject, string> customScript)
    {
        if (objImporter != null)
        {
            objImporter.ImportedModel -= customScript;
        }
    }

    void Start()
    {
        Interactable btn = gameObject.GetComponent<Interactable>();
        if (btn != null)
        {
            btn.OnClick.AddListener(ObjOpernerAsync);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ObjLoaderAsync(string objectName, string path)
    {
        filePath = path;
        objImporter.ImportModelAsync(objectName, path, null, importOptions);
    }

    async void ObjOpernerAsync()
    {
#if (!UNITY_EDITOR && ENABLE_WINMD_SUPPORT && UNITY_WSA)
        // Code borrowed from:
        // https://stackoverflow.com/questions/66281959/system-exception-getting-called-when-assessing-openfilepicker-on-hololens/66282480#66282480
        Debug.Log("***********************************");
        Debug.Log("File Picker start.");
        Debug.Log("***********************************");

        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            var filepicker = new FileOpenPicker();
            // filepicker.FileTypeFilter.Add("*");
            filepicker.FileTypeFilter.Add(".obj");

            var file = await filepicker.PickSingleFileAsync();
            UnityEngine.WSA.Application.InvokeOnAppThread(() => 
            {
                Debug.Log("***********************************");
                string name = (file != null) ? file.Name : "No data";
                Debug.Log("Name: " + name);
                Debug.Log("***********************************");
                string path = (file != null) ? file.Path : "No data";
                Debug.Log("Path: " + path);
                Debug.Log("***********************************");

                
                filePath = file.Path;
                objImporter.ImportModelAsync(objectName, file.Path, null, importOptions);

                //This section of code reads through the file (and is covered in the link)
                // but if you want to make your own parcing function you can 
                // ReadTextFile(path);
                //StartCoroutine(ReadTextFileCoroutine(path));


            }, false);
        }, false);

        
        Debug.Log("***********************************");
        Debug.Log("File Picker end.");
        Debug.Log("***********************************");
#elif UNITY_EDITOR
        Debug.Log("No uwp support");
        string path = EditorUtility.OpenFilePanel("Select a 3D model.", "", "obj");
        objImporter.ImportModelAsync(objectName, path, null, importOptions);
#else
        Debug.Log("None support");
#endif
    }

    void AddHandInteractionToObj(GameObject gameObject, string path)
    {
        var model = gameObject.transform.GetChild(0);
        var meshCollider = model.gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        if (isEditor)
        {
            model.gameObject.AddComponent<ObjectManipulator>();
            model.gameObject.AddComponent<NearInteractionGrabbable>();
            var subMenu = Instantiate<GameObject>(threeDSubMenu, model.transform);
            var displayAnchorBtn = subMenu.transform.Find("ButtonCollection").transform.Find("DisplayAnchorBtn").gameObject;
            if (displayAnchorBtn != null)
            {
                displayAnchorBtn.GetComponent<DisplayAnchor>().to = model.gameObject;
            }
            else
            {
                Debug.Log("displayAnchorBtn not found");
            }
            var deleteBtn = subMenu.transform.Find("ButtonCollection").transform.Find("DeleteBtn").gameObject;
            DestroyObj[] destroyObjs = deleteBtn.GetComponents<DestroyObj>();
            destroyObjs[0].obj = subMenu;
            destroyObjs[1].obj = model.gameObject;
            // subMenu.AddComponent<DetachTransformParent>();
            var detachChildStore = model.gameObject.AddComponent<DetachChildAndStore>();
            detachChildStore.enabled = true;
            detachChildStore.child = subMenu;
            subMenu.transform.parent = null;
            subMenu.SetActive(false);
            var btnPinMenu = Instantiate<GameObject>(buttonPinMenu, model.transform);
            btnPinMenu.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() =>
            {
                subMenu.SetActive(true);
            });

        }
        model.gameObject.AddComponent<ObjectAnchor>();
        Serializer serializer = model.gameObject.AddComponent<Serializer>();
        serializer.type = "ImportedObject";
        serializer.Path = path;
        model.gameObject.tag = "ImportedObject";
        model.transform.parent = null;
        gameObject.transform.parent = model.transform;
    }
}
