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
    ThreeDObject threeDObject=new ThreeDObject();

    public ThreeDObject ThreeDObject
    {
        get { return threeDObject; }
        set { threeDObject = value; }
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

    public ThreeDObject serializeToThreeDObject()
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

        this.threeDObject = new ThreeDObject(type: this.type, uuid: this.uuid, _transform: gameObject.transform, parentUuid: parent, isRoot:parent.Equals(""));
        return this.threeDObject;
    }

    public void DeserializeThreeDObjectsStandAlone(ThreeDObject threeDObject)
    {
        this.threeDObject = threeDObject;
        type = threeDObject.type;
        threeDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
        //SetTransformParent(threeDObject.parentUuid, allGameObjects);
        uuid = threeDObject.uuid;
        parentUuid = threeDObject.parentUuid;
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
                            objAnchorParent.objectAnchorManager.SetAsAnchor(obj);
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
