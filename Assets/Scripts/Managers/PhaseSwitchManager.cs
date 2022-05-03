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
    public TextMeshPro phaseNameText;
    int curPhase = 0;
    ThreeDModelImporter importer;
    List<GameObject> all;
    public GameObject modelTarget;
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
        updatePhaseNameText();
    }

    void LoadAllObjects()
    {
        all.Clear();
        StartLoadAllImportedObjects();
        LoadAllThreeDObjects();
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
            }
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.DeserializeThreeDObjectStandAlone(threeDObject);
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
        GameObject[] allImported = GameObject.FindGameObjectsWithTag("ImportedObject");
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
        StoreAllThreeDObjects();
        StoreAllImportedObjects();
    }


    void StoreAllThreeDObjects()
    {
        phases[curPhase].threeDObjects.Clear();
        string tag = "ThreeDObject";
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].threeDObjects.Add(serializer.serializeToThreeDObject());
            }
        }
    }

    void StoreAllImportedObjects()
    {
        phases[curPhase].importedObjects.Clear();
        string tag = "ImportedObject";
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].importedObjects.Add(serializer.serializeToImportedObject());
            }
        }
    }

    void RemoveAllObjects()
    {
        RemoveAllObjectsGivenTag("ThreeDObject");
        RemoveAllObjectsGivenTag("ImportedObject");
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
