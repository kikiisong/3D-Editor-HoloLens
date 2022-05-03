using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    void Start()
    {
        gameObject.GetComponent<Interactable>().OnClick.AddListener(Destroy_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Destroy_()
    {
        Destroy(obj);
    }
}
