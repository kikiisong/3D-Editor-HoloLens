using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObj : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable btn;
    public GameObject obj;

    void Start()
    {
        btn.OnClick.AddListener(Create);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Create()
    {
        Vector3 pos = new Vector3(0.4f, 0, 0.2f);
        Quaternion rot = new Quaternion(0, 1, 1, 30);
        Instantiate(obj, pos, rot);
    }
}
