using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMenu: MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject curMenu;
    public GameObject btn;
    public GameObject replaceMenu;
    void Start()
    {
        Interactable b = btn.GetComponent<Interactable>();
        b.OnClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        replaceMenu.transform.rotation = curMenu.transform.rotation;
        replaceMenu.transform.position = curMenu.transform.position;
        curMenu.SetActive(false);
        replaceMenu.SetActive(true);
    }

}
