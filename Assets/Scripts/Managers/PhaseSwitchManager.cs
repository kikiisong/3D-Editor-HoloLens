using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhaseSwitchManager : MonoBehaviour
{
    // Start is called before the first frame update
    List<SerializedHolder> phases = new List<SerializedHolder>();
    public GameObject cubePrefab;
    public GameObject arrowPrefab;
    public GameObject spherePrefab;
    public GameObject textPanelPrefab;
    public GameObject imgPanelPrefab;
    public GameObject logoPrefab;

    public TextMeshPro phaseNameText;
    int curPhase = 0;
    ThreeDModelImporter importer;
    List<GameObject> all;
    public GameObject modelTarget;

    public const string THREE_D_OBJECT_TAG = "ThreeDObject"; // Note logo is considered as ThreeDObject
    public const string IMPORTED_OBJECT_TAG = "ImportedObject";
    public const string MODEL_TARGET_TAG = "ModelTarget";
    public const string TEXT_OBJECT_TAG = "TextPanel";
    public const string TWO_D_OBJECT_TAG = "ImagePanel";

    private void Awake()
    {
        all = new List<GameObject>();
        phases.Add(new SerializedHolder());
        importer = gameObject.GetComponent<ThreeDModelImporter>();
        importer.AddCustomOnImportedScript(OnLoaded);
        importer.AddCustomScriptOnImportCompleted(OnImportCompleted);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void updatePhaseNameText()
    {
        phaseNameText.text = "Phase " + curPhase.ToString();
    }

    public void PreviousPhase()
    {
        if (curPhase > 0)
        {
            StoreAllObjects();
            RemoveAllObjects();
            curPhase -= 1;
            LoadAllObjects();
            updatePhaseNameText();
        }
    }

    public void NextPhase()
    {
        Debug.Log("NextPhase called");
        StoreAllObjects();
        RemoveAllObjects();
        curPhase += 1;
        if (phases.Count <= curPhase)
        {
            phases.Add(new SerializedHolder());
        }
        else
        {
            LoadAllObjects();
        }
        updatePhaseNameText();
    }

    void LoadAllObjects()
    {
        all.Clear();
        StartLoadAllImportedObjects();
        LoadAllTwoDObjects();
        LoadAllThreeDObjects();
        LoadAllTextObjects();
        LoadModelTarget();
        setupTransformParentForAll();
    }

    void setupTransformParentForAll()
    {
        foreach (GameObject obj in all)
        {
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.SetTransformParent(all);
            }
        }
        PostSetTransformDisableMeshOfModelTargetChildren();
    }

    void PostSetTransformDisableMeshOfModelTargetChildren()
    {
        MeshRenderer[] render = modelTarget.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        Collider[] collider = modelTarget.GetComponentsInChildren<Collider>(includeInactive: true);
        var canvasComponents = modelTarget.GetComponentsInChildren<Canvas>(true);
        foreach (var v in render)
        {
            v.enabled = false;
        }
        foreach (var v in collider)
        {
            v.enabled = false;
        }
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    void LoadModelTarget()
    {
        Serializer serializer = modelTarget.GetComponent<Serializer>();
        if (serializer != null)
        {
            serializer.DeserializeModelTarget(phases[curPhase].modelTargetActivated);
        }
        all.Add(modelTarget);
    }

    void LoadAllThreeDObjects()
    {
        foreach (ThreeDObject threeDObject in phases[curPhase].threeDObjects)
        {
            GameObject obj = null;
            switch (threeDObject.type)
            {
                case "Cube":
                    obj = Instantiate<GameObject>(cubePrefab);
                    break;
                case "Sphere":
                    obj = Instantiate<GameObject>(spherePrefab);
                    break;
                case "Arrow":
                    obj = Instantiate<GameObject>(arrowPrefab);
                    break;
                case "Logo":
                    obj = Instantiate<GameObject>(logoPrefab);
                    break;
            }
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.DeserializeThreeDObjectStandAlone(threeDObject);
            }
            all.Add(obj);
        }
    }

    void LoadAllTwoDObjects()
    {
        foreach (TwoDObject twoDObject in phases[curPhase].twoDObjects)
        {
            GameObject obj = Instantiate<GameObject>(imgPanelPrefab);
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.DesrializeTwoDObjectStandAlone(twoDObject);
            }
            all.Add(obj);
        }
    }

    void LoadAllTextObjects()
    {
        foreach (TextObject textObject in phases[curPhase].textObjects)
        {
            GameObject obj = Instantiate<GameObject>(textPanelPrefab);
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.DeserializeTextObjectStandAlone(textObject);
            }
            all.Add(obj);
        }
    }

    void OnLoaded(GameObject gameObject, string path)
    {
        if (gameObject.transform.parent == null)
        {
            all.Add(gameObject.transform.GetChild(0).gameObject);
        }
        else
        {
            all.Add(gameObject.transform.parent.gameObject);
        }
    }

    void OnImportCompleted()
    {
        GameObject[] allImported = GameObject.FindGameObjectsWithTag(IMPORTED_OBJECT_TAG);
        foreach (GameObject obj in allImported)
        {
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer == null)
            {
                continue;
            }
            GameObject uuidNamedObj = null;
            uuidNamedObj = obj.transform.GetChild(1).gameObject;
            string uuid = uuidNamedObj.name.Substring(0, 36);
            int phase = int.Parse(uuidNamedObj.name.Substring(36));
            foreach (ImportedObject importedObject in phases[phase].importedObjects)
            {
                if (importedObject.uuid == uuid)
                {
                    serializer.DeserializeImportedObjectStandAlone(importedObject);
                    break;
                }
            }
        }
        setupTransformParentForAll();
    }

    void StartLoadAllImportedObjects()
    {
        for (int i = 0; i < phases[curPhase].importedObjects.Count; ++i)
        {
            ImportedObject importedObject = phases[curPhase].importedObjects[i];
            string path = importedObject.path;
            if (path != "")
            {
                importer.ObjLoaderAsync(importedObject.uuid + curPhase.ToString(), path);
            }
        }
    }

    void StoreAllObjects()
    {
        StoreModelTarget();
        modelTarget.transform.parent.gameObject.SetActive(true);
        StoreAllThreeDObjects();
        StoreAllTextObjects();
        StoreAllTwoDObjects();
        StoreAllImportedObjects();
        modelTarget.transform.parent.gameObject.SetActive(phases[curPhase].modelTargetActivated);
    }

    void StoreAllThreeDObjects()
    {
        phases[curPhase].threeDObjects.Clear();
        string tag = THREE_D_OBJECT_TAG;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].threeDObjects.Add(serializer.SerializeToThreeDObject());
            }
        }
    }

    void StoreAllTwoDObjects()
    {
        phases[curPhase].twoDObjects.Clear();
        string tag = TWO_D_OBJECT_TAG;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].twoDObjects.Add(serializer.SerializeToTwoDObject());
            }
        }
    }

    void StoreAllTextObjects()
    {
        phases[curPhase].textObjects.Clear();
        string tag = TEXT_OBJECT_TAG;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].textObjects.Add(serializer.SerializeToTextObject());
            }
        }
    }

    void StoreAllImportedObjects()
    {
        phases[curPhase].importedObjects.Clear();
        string tag = IMPORTED_OBJECT_TAG;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].importedObjects.Add(serializer.SerializeToImportedObject());
            }
        }
    }

    void StoreModelTarget()
    {
        Serializer serializer = modelTarget.GetComponent<Serializer>();
        if (serializer != null)
        {
            phases[curPhase].modelTargetActivated = serializer.SerializeModelTarget();
        }
    }

    void RemoveAllObjects()
    {
        RemoveAllObjectsGivenTag(THREE_D_OBJECT_TAG);
        RemoveAllObjectsGivenTag(IMPORTED_OBJECT_TAG);
        RemoveAllObjectsGivenTag(TWO_D_OBJECT_TAG);
        RemoveAllObjectsGivenTag(TEXT_OBJECT_TAG);
        RemoveModelTarget();
    }

    void RemoveModelTarget()
    {
        modelTarget.transform.parent.gameObject.SetActive(false);
        List<GameObject> toBeDestroied = new List<GameObject>();
        for (int i = 0; i < modelTarget.transform.childCount; i++)
        {
            GameObject child = modelTarget.transform.GetChild(i).gameObject;
            if (child.GetComponent<Serializer>() != null)
            {
                toBeDestroied.Add(child);
            }
        }
        foreach (GameObject child in toBeDestroied)
        {
            Destroy(child);
        }
        GameObject modelTargetSubMenu = GameObject.Find("3DObjSubMenuModelTarget");
        if (modelTargetSubMenu != null)
        {
            modelTargetSubMenu.SetActive(false);
        }
        ObjectAnchor anchor = modelTarget.GetComponent<ObjectAnchor>();
        if (anchor != null)
        {
            anchor.objectAnchorManager.UnGroup(modelTarget);
            anchor.objectAnchorManager.UnSetAnchor(modelTarget);
        }
    }

    void RemoveAllObjectsGivenTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
