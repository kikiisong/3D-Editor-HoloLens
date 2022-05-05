using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachChildAndStore : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject child;
    private void Awake()
    {
        child.transform.parent = null;
        MeshRenderer[] render = child.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
        Collider[] collider = child.GetComponentsInChildren<Collider>(includeInactive: true);
        var canvasComponents = child.GetComponentsInChildren<Canvas>(true);
        foreach (var v in render)
        {
            v.enabled = true;
        }
        foreach (var v in collider)
        {
            v.enabled = true;
        }
        foreach (var component in canvasComponents)
            component.enabled = true;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Destroy(child);
    }
}
