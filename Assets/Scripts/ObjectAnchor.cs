using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    private ObjAnchorManager objectAnchorManager;
    private void Awake()
    {
        objectAnchorManager = GameObject.Find("ObjectAnchorController").GetComponent<ObjAnchorManager>();
    }

    void Start()
    {
        objectAnchorManager.AddToAllObjects(gameObject);
        GameObject menu = objectAnchorManager.GetSubMenu(gameObject);
        GameObject toggleAnchorBtn = objectAnchorManager.GetBtn(menu, objectAnchorManager.TOGGLE_OBJ_ANCHOR_NAME);
        if(toggleAnchorBtn!=null)
            toggleAnchorBtn.GetComponent<Interactable>().OnClick.AddListener(ToggleAnchorBtnListener);
        GameObject anchorToObjectBtn = objectAnchorManager.GetBtn(menu, objectAnchorManager.ANCHOR_TO_OBJ_NAME);
        if(anchorToObjectBtn!=null)
            anchorToObjectBtn.GetComponent<Interactable>().OnClick.AddListener(AnchorToObjectBtnListener);
        GameObject unGroupBtn = objectAnchorManager.GetBtn(menu, objectAnchorManager.UN_GROUP_BTN_NAME);
        if(unGroupBtn!=null)
            unGroupBtn.GetComponent<Interactable>().OnClick.AddListener(UnGroupBtnListener);
        objectAnchorManager.PostSetAnchorUpdateOtherObjects(objectAnchorManager.curAnchor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ToggleAnchorBtnListener()
    {
        if (!gameObject.Equals(objectAnchorManager.curAnchor))
        {
            objectAnchorManager.SetAsAnchor(gameObject);
        }
        else
        {
            objectAnchorManager.UnSetAnchor(gameObject);
        }

    }

    void AnchorToObjectBtnListener()
    {
        if (objectAnchorManager.curAnchor != null && gameObject.transform.parent != null && objectAnchorManager.curAnchor.Equals(gameObject.transform.parent.gameObject))
        {
            objectAnchorManager.Detach(gameObject);
        }
        else
        {
            objectAnchorManager.Attache(gameObject);
        }
    }


    private void OnDestroy()
    {
        objectAnchorManager.Detach(gameObject);
        objectAnchorManager.RemoveFromAllObjects(gameObject);
    }

    void UnGroupBtnListener()
    {
        objectAnchorManager.UnGroup(gameObject);
    }
}
