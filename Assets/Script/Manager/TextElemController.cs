using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextElemController : MonoBehaviour
{
    public GameObject textPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createText()
    {
        Instantiate(textPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity);
    }
}
