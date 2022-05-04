using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;
    public string parentUuid="";
    public string uuid = "";
    string path;
    bool modelTargetActivated = false;
    ThreeDObject threeDObject=new ThreeDObject();
    ImportedObject importedObject = new ImportedObject();

    public string Path
    {
        get { return path; }
        set { path = value; }
    }
    public ThreeDObject ThreeDObject
    {
        get { return threeDObject; }
        set { threeDObject = value; }
    }
    public ImportedObject ImportedObject
    {
        get { return importedObject; }
        set { importedObject = value; }
    }
    private void Awake()
    {
        uuid = Guid.NewGuid().ToString();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    string getParentUuid()
    {
        string parent = "";
        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.parent.Equals(Camera.main.transform))
            {
                parent = "camera";
            }
            else
            {
                GameObject parentObj = gameObject.transform.parent.gameObject;
                Serializer parentSerializer = parentObj.GetComponent<Serializer>();
                if (parentSerializer != null)
                {
                    parent = parentSerializer.uuid;
                }
            }
        }
        return parent;

    }

    public ThreeDObject serializeToThreeDObject()
    {
        string parent = getParentUuid();
        this.threeDObject = new ThreeDObject(type: this.type, uuid: this.uuid, _transform: gameObject.transform, parentUuid: parent, isRoot:parent.Equals(""));
        return this.threeDObject;
    }

    public ImportedObject serializeToImportedObject()
    {
        string parent = getParentUuid();
        this.importedObject = new ImportedObject(path,uuid,gameObject.transform,type,parent,parent.Equals(""));

        return this.importedObject;
    }

    public bool serializeModelTarget()
    {
        this.modelTargetActivated = gameObject.transform.parent.gameObject.activeSelf;
        return this.modelTargetActivated;
    }

    public void DeserializeThreeDObjectStandAlone(ThreeDObject threeDObject)
    {
        this.threeDObject = threeDObject;
        type = threeDObject.type;
        threeDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
        //SetTransformParent(threeDObject.parentUuid, allGameObjects);
        uuid = threeDObject.uuid;
        parentUuid = threeDObject.parentUuid;
    }

    public void DeserializeImportedObjectStandAlone(ImportedObject importedObject)
    {
        path = importedObject.path;
        DeserializeThreeDObjectStandAlone(importedObject);
    }

    public void DeserializeModelTarget(bool active)
    {
        this.modelTargetActivated = active;
        gameObject.transform.parent.gameObject.SetActive(active);
        GameObject modelTargetSubMenu = GameObject.Find("3DObjSubMenuModelTarget");
        if (modelTargetSubMenu != null)
        {
            modelTargetSubMenu.SetActive(active);
        }
    }

    public void SetTransformParent(List<GameObject> allGameObjects)
    {
        if (parentUuid != "")
        {
            if (parentUuid == "Camera")
            {
                gameObject.transform.parent = Camera.main.transform;
            }
            else
            {
                foreach(GameObject obj in allGameObjects)
                {
                    Serializer serializer = obj.GetComponent<Serializer>();
                    if (serializer != null&&serializer.uuid==parentUuid)
                    {
                        ObjectAnchor objAnchorParent = obj.GetComponent<ObjectAnchor>();
                        ObjectAnchor thisObjAnchor = gameObject.GetComponent<ObjectAnchor>();
                        if(objAnchorParent!=null && thisObjAnchor != null)
                        {
                            objAnchorParent.objectAnchorManager.SetAsAnchor(obj,false);
                            thisObjAnchor.objectAnchorManager.Attache(gameObject);
                            objAnchorParent.objectAnchorManager.UnSetAnchor(obj);
                            threeDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                        }
                    }
                }
            }
        }
    }

}
