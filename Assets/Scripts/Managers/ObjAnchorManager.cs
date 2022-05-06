using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAnchorManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject curAnchor;
    public Dictionary<GameObject, List<GameObject>> anchorChildrenPairs = new Dictionary<GameObject, List<GameObject>>();
    public HashSet<GameObject> allObejcts = new HashSet<GameObject>();
    public readonly string THREE_D_OBJ_SUB_MENU_NAME = "3DObjSubMenu";
    public readonly string TOGGLE_OBJ_ANCHOR_NAME = "ToggleObjectAnchorBtn";
    public readonly string ANCHOR_TO_OBJ_NAME = "AnchorToObject";
    public readonly string DEACT_ANCHOR_TEXT = "Unset anchor";
    public readonly string ACT_ANCHOR_TEXT = "Set as anchor";
    public readonly string ANCHOR_TO_OBJ_TEXT = "Anchor to the object";
    public readonly string DETACH_TO_OBJ_TEXT = "Detach";
    public readonly string UN_GROUP_BTN_NAME = "UnGroupBtn";

    private void Awake()
    {
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveFromAllObjects(GameObject obj)
    {
        allObejcts.Remove(obj);
    }

    public void AddToAllObjects(GameObject obj)
    {
        allObejcts.Add(obj);
    }

    void ChangeBtnText(GameObject btn, string text)
    {
        if (btn != null)
            btn.GetComponent<ButtonConfigHelper>().MainLabelText = text;
    }

    public GameObject GetBtn(GameObject objMenu, string name)
    {
        if (objMenu != null)
        {
            GameObject collection = objMenu.transform.Find("ButtonCollection").gameObject;
            Transform btn_transform = collection.transform.Find(name);
            if (btn_transform != null)
                return btn_transform.gameObject;
            else
                return null;
        }
        else
            return null;
    }

    void ActivateButton(GameObject objMenu, string name)
    {
        if (objMenu != null)
        {
            GameObject collection = objMenu.transform.Find("ButtonCollection").gameObject;
            Transform btn_transform = collection.transform.Find(name);
            if (btn_transform != null)
            {
                GameObject btn = btn_transform.gameObject;
                btn.SetActive(true);
            }
            collection.GetComponent<GridObjectCollection>().UpdateCollection();
        }
    }
    void DeactivateButton(GameObject objMenu, string name)
    {
        if (objMenu != null)
        {
            GameObject collection = objMenu.transform.Find("ButtonCollection").gameObject;
            Transform btn_transform = collection.transform.Find(name);
            if (btn_transform != null)
            {
                GameObject btn = btn_transform.gameObject;
                btn.SetActive(false);
            }
            collection.GetComponent<GridObjectCollection>().UpdateCollection();
        }
    }

    void AddObjectAnchorBtn(GameObject objMenu)
    {
        ActivateButton(objMenu, ANCHOR_TO_OBJ_NAME);
    }

    void RemoveObjectAnchorBtn(GameObject objMenu)
    {
        DeactivateButton(objMenu, ANCHOR_TO_OBJ_NAME);
    }

    public GameObject GetSubMenu(GameObject obj)
    {
        if (obj != null)
        {
            DetachChildAndStore detachChildAndStore = obj.GetComponent<DetachChildAndStore>();
            return detachChildAndStore == null ? null:detachChildAndStore.child;
        }
        else
            return null;
    }

    public void PostSetAnchorUpdateOtherObjects(GameObject anchorObj)
    {
        if (anchorObj != null)
        {
            foreach (GameObject otherObj in allObejcts)
            {
                if (otherObj.Equals(anchorObj))
                {
                    continue;
                }
                GameObject otherObjMenu = GetSubMenu(otherObj);
                DeactivateButton(otherObjMenu, TOGGLE_OBJ_ANCHOR_NAME);
                AddObjectAnchorBtn(otherObjMenu);
            }
        }
    }

    public void SetAsAnchor(GameObject obj, bool doDetach = true)
    {
        if (obj != null)
        {
            curAnchor = obj;
            if (doDetach)
                Detach(obj);
            if (!anchorChildrenPairs.ContainsKey(obj))
            {
                anchorChildrenPairs[obj] = new List<GameObject>();
            }
            GameObject menu = GetSubMenu(obj);
            ChangeBtnText(GetBtn(menu, TOGGLE_OBJ_ANCHOR_NAME), DEACT_ANCHOR_TEXT);
            RemoveObjectAnchorBtn(menu);
            PostSetAnchorUpdateOtherObjects(obj);
        }
    }

    public void UnSetAnchor(GameObject obj)
    {
        if (obj != null&&obj.Equals(curAnchor))
        {
            curAnchor = null;
            if (anchorChildrenPairs.ContainsKey(obj))
            {
                //anchorChildrenPairs.Remove(obj);
                GameObject menu = GetSubMenu(obj);
                ChangeBtnText(GetBtn(menu, TOGGLE_OBJ_ANCHOR_NAME), ACT_ANCHOR_TEXT);
                foreach (GameObject otherObj in allObejcts)
                {
                    GameObject otherObjMenu = GetSubMenu(otherObj);
                    ActivateButton(otherObjMenu, TOGGLE_OBJ_ANCHOR_NAME);
                    GameObject anchor_to_obj_btn = GetBtn(otherObjMenu, ANCHOR_TO_OBJ_NAME);
                    if (anchor_to_obj_btn != null)
                    {
                        if (anchor_to_obj_btn.GetComponent<ButtonConfigHelper>().MainLabelText != DETACH_TO_OBJ_TEXT)
                            RemoveObjectAnchorBtn(otherObjMenu);
                    }
                }
            }
        }
    }


    public void Attache(GameObject obj)
    {
        if (obj != null && curAnchor != null && anchorChildrenPairs.ContainsKey(curAnchor))
        {
            anchorChildrenPairs[curAnchor].Add(obj);
            obj.transform.parent = curAnchor.transform;
            GameObject menu = GetSubMenu(curAnchor);
            ActivateButton(menu, UN_GROUP_BTN_NAME);
            menu = GetSubMenu(obj);
            ChangeBtnText(GetBtn(menu, ANCHOR_TO_OBJ_NAME), DETACH_TO_OBJ_TEXT);
        }
    }

    public void Detach(GameObject obj, bool onDestroy = false)
    {
        if (obj != null)
        {
            GameObject parent = null;
            if (obj.transform.parent != null)
            {
                parent = obj.transform.parent.gameObject;
            }
            if (parent != null && anchorChildrenPairs.ContainsKey(parent))
            {
                anchorChildrenPairs[parent].Remove(obj);
                if (!onDestroy)
                {
                    obj.transform.parent = null;
                    GameObject menu = GetSubMenu(obj);
                    ChangeBtnText(GetBtn(menu, ANCHOR_TO_OBJ_NAME), ANCHOR_TO_OBJ_TEXT);
                    if (curAnchor == null)
                        RemoveObjectAnchorBtn(menu);
                }
                if (anchorChildrenPairs[parent].Count == 0)
                {
                    GameObject menu = GetSubMenu(parent);
                    DeactivateButton(menu, UN_GROUP_BTN_NAME);
                }
            }
        }
    }

    public void UnGroup(GameObject parent)
    {
        if (parent != null && anchorChildrenPairs.ContainsKey(parent))
        {
            foreach (GameObject child in anchorChildrenPairs[parent])
            {
                if (child == null)
                {
                    continue;
                }
                child.transform.parent = null;
                GameObject childMenu = GetSubMenu(child);
                ChangeBtnText(GetBtn(childMenu, ANCHOR_TO_OBJ_NAME), ANCHOR_TO_OBJ_TEXT);
            }
            anchorChildrenPairs[parent].Clear();
            GameObject menu = GetSubMenu(parent);
            DeactivateButton(menu, UN_GROUP_BTN_NAME);
        }
    }
}
