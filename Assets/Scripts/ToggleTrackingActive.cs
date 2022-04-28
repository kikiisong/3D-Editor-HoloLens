using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ToggleTrackingActive : MonoBehaviour
{
    // Start is called before the first frame update
    public ModelTargetBehaviour target;
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
        target.enabled = !target.enabled;
    }
}
