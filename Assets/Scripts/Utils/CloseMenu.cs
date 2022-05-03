using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable btn;
    public GameObject menu;
    void Start()
    {
        btn.OnClick.AddListener(CloseMenuHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CloseMenuHandler()
    {
        menu.SetActive(false);
    }
}
