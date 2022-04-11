using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachFollowArrow : MonoBehaviour
{
    // Start is called before the first frame update
    public Solver followArrow;
    private bool tracking = true;
    void Start()
    {
        gameObject.GetComponent<Interactable>().OnClick.AddListener(AttachFollowArrowHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AttachFollowArrowHandler()
    {
        var indicator = followArrow.GetComponent<DirectionalIndicator>();
        if (tracking==false)
        {
            indicator.DirectionalTarget = gameObject.transform;
            tracking = true;
        }
        else
        {
            indicator.DirectionalTarget = null;
            tracking = false;
        }
    }
}
