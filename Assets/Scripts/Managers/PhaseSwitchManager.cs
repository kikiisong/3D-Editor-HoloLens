using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitchManager : MonoBehaviour
{
    // Start is called before the first frame update
    List<SerializedHolder> phases = new List<SerializedHolder>();
    public GameObject cubePrefab;
    public GameObject arrowPrefab;
    public GameObject spherePrefab;
    int curPhase = 0;
    void Start()
    {
        phases.Add(new SerializedHolder());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviousPhase()
    {
        if (curPhase > 0)
        {
            RemoveAllObjects();
            curPhase -= 1;
            LoadAllObjects();
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
    }

    void LoadAllObjects()
    {
        List<GameObject> all = new List<GameObject>();
        LoadAllThreeDObjects(all);
        foreach(GameObject obj in all)
        {
            Serializer serializer = obj.GetComponent<Serializer>();
            if (serializer != null)
            {
                serializer.SetTransformParent(all);
            }
        }
    }

    void LoadAllThreeDObjects(List<GameObject> all)
    {
        foreach(ThreeDObject threeDObject in phases[curPhase].threeDObjects)
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
                serializer.DeserializeThreeDObjectsStandAlone(threeDObject);
            }
            all.Add(obj);
        }
    }

    void StoreAllObjects()
    {
        phases[curPhase].threeDObjects.Clear();
        StoreAllThreeDObjects();
    }

    void StoreAllThreeDObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ThreeDObject");
        foreach(GameObject gameObject in gameObjects)
        {
            Serializer serializer = gameObject.GetComponent<Serializer>();
            if (serializer != null)
            {
                phases[curPhase].threeDObjects.Add(serializer.serializeToThreeDObject());
            }
            //ThreeDObject threeDObject = new ThreeDObject()
        }
    }

    void RemoveAllObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ThreeDObject");
        foreach(GameObject gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
