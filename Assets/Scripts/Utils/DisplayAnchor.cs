using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject to;
    void Start()
    {
        gameObject.GetComponent<Interactable>().OnClick.AddListener(Attach);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attach()
    {
        if (to.transform.parent == null)
            to.transform.SetParent(Camera.main.transform);
        else
            to.transform.parent = null;
    }
}
