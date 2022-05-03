using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowElementSubMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable btn;
    public GameObject allMenus;
    public GameObject subMenu;
    void Start()
    {
        btn.OnClick.AddListener(ShowSubMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowSubMenu()
    {
        foreach (Transform obj in allMenus.GetComponentInChildren<Transform>())
        {
            GameObject menu = obj.gameObject;
            menu.SetActive(false);
        }
        subMenu.SetActive(true);
    }
}
