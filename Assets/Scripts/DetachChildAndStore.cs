using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachChildAndStore : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject child;
    void Start()
    {
        child.transform.parent = null;
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
