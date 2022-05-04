using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrbit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject to;
    private bool is_orbit=false;
    private Transform parent;
    void Start()
    {
        gameObject.GetComponent<Interactable>().OnClick.AddListener(Attach);
        parent = to.GetComponent<Transform>().parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attach()
    {
        if (!is_orbit)
        {
            to.transform.SetParent(Camera.main.transform);
            is_orbit = true;
        }
        else
        {
            to.transform.SetParent(parent);
            is_orbit = false;
        }
    }
}
