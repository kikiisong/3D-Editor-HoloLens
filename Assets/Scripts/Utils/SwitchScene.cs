using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable btn;
    public string nextScene;
    void Start()
    {
        if(btn)
            btn.OnClick.AddListener(Switch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Switch()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void SwitchTo(string toScene)
    {
        SceneManager.LoadScene(toScene);
    }
}
