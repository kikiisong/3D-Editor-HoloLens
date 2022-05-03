using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable btn;
    public GameObject obj;

    void Start()
    {
        btn.OnClick.AddListener(Active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Active()
    {
        obj.SetActive(true);
    }
}
