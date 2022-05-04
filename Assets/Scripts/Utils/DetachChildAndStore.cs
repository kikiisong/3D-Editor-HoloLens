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
