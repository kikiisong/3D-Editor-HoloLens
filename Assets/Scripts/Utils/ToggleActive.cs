using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ToggleActive : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    void Start()
    {
        gameObject.GetComponent<Interactable>().OnClick.AddListener(ToggleActiveHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleActiveHandler()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
