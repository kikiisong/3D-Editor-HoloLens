using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Serializer : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;
    public string parentUuid = "";
    public string uuid = "";
    string path;
    bool modelTargetActivated = false;
    string imgPath;
    string title;
    string content;
    ThreeDObject threeDObject = new ThreeDObject();
    ImportedObject importedObject = new ImportedObject();
    TwoDObject twoDObject = new TwoDObject();
    TextObject textObject = new TextObject();

    public string Content
    {
        get { return content; }
        set { content = value; }
    }
    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    public string ImgPath
    {
        get { return imgPath; }
        set { imgPath = value; }
    }
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
        if (type != "ModelTarget")
            uuid = Guid.NewGuid().ToString();
        else
            uuid = "ModelTarget";
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    string GetParentUuid()
    {
        string parent = "";
        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.parent.Equals(Camera.main.transform))
            {
                parent = "Camera";
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

    public ThreeDObject SerializeToThreeDObject()
    {
        string parent = GetParentUuid();
        this.threeDObject = new ThreeDObject(type: this.type, uuid: this.uuid, _transform: gameObject.transform, parentUuid: parent, isRoot: parent.Equals(""));
        return this.threeDObject;
    }

    public ImportedObject SerializeToImportedObject()
    {
        string parent = GetParentUuid();
        this.importedObject = new ImportedObject(path, uuid, gameObject.transform, type, parent, parent.Equals(""));

        return this.importedObject;
    }

    public TwoDObject SerializeToTwoDObject()
    {
        string parent = GetParentUuid();
        TextMeshPro imgTitle = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        ImportedImg imagePath = gameObject.GetComponent<ImportedImg>();
        this.imgPath = imagePath.m_imagePath;
        this.title = imgTitle.text;
        this.twoDObject = new TwoDObject(imgPath, title, uuid, gameObject.transform, type, parent, parent.Equals(""));
        return this.twoDObject;
    }

    public TextObject SerializeToTextObject()
    {
        string parent = GetParentUuid();
        VoiceTitleEdit textPanel = gameObject.GetComponent<VoiceTitleEdit>();
        this.content = textPanel.textContent.text;
        this.title = textPanel.textTitle.text;
        this.textObject = new TextObject(content, title, uuid, gameObject.transform, type, parent, parent.Equals(""));

        return this.textObject;
    }

    public bool SerializeModelTarget()
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

    public void DeserializeTextObjectStandAlone(TextObject textObject)
    {
        this.textObject = textObject;
        type = textObject.type;
        textObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
        uuid = textObject.uuid;
        parentUuid = textObject.parentUuid;

        this.title = textObject.m_Title;
        this.content = textObject.m_Content;

        DialogShell dialogShell = gameObject.GetComponent<DialogShell>();
        dialogShell.TitleText.text = title;
        dialogShell.DescriptionText.text = content;
    }

    public void DesrializeTwoDObjectStandAlone(TwoDObject twoDObject)
    {
        this.twoDObject = twoDObject;
        type = twoDObject.type;
        twoDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
        uuid = twoDObject.uuid;
        parentUuid = twoDObject.parentUuid;

        this.title = twoDObject.m_Title;
        this.imgPath = twoDObject.m_ImgPath;

        ImportedImg imagePath = gameObject.GetComponent<ImportedImg>();
        imagePath.m_imagePath = this.imgPath;

        TextMeshPro imgTitle = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        imgTitle.text = this.title;

        var fileContent = File.ReadAllBytes(this.imgPath);
        var tex = new Texture2D(2, 2);
        tex.LoadImage(fileContent);

        MeshRenderer imgRenderer = gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        imgRenderer.material.mainTexture = tex;
    }

    public void DeserializeImportedObjectStandAlone(ImportedObject importedObject)
    {
        path = importedObject.path;
        this.importedObject = importedObject;
        type = importedObject.type;
        importedObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
        //SetTransformParent(threeDObject.parentUuid, allGameObjects);
        uuid = importedObject.uuid;
        parentUuid = importedObject.parentUuid;
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
                foreach (GameObject obj in allGameObjects)
                {
                    Serializer serializer = obj.GetComponent<Serializer>();
                    if (serializer != null && serializer.uuid == parentUuid)
                    {
                        ObjectAnchor objAnchorParent = obj.GetComponent<ObjectAnchor>();
                        ObjectAnchor thisObjAnchor = gameObject.GetComponent<ObjectAnchor>();
                        if (objAnchorParent != null && thisObjAnchor != null)
                        {
                            objAnchorParent.objectAnchorManager.SetAsAnchor(obj, false);
                            thisObjAnchor.objectAnchorManager.Attache(gameObject);
                            objAnchorParent.objectAnchorManager.UnSetAnchor(obj);
                            //threeDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                        }
                    }
                }
            }
            switch (gameObject.tag)
            {
                case PhaseSwitchManager.TEXT_OBJECT_TAG:
                    textObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                    break;
                case PhaseSwitchManager.THREE_D_OBJECT_TAG:
                    threeDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                    break;
                case PhaseSwitchManager.TWO_D_OBJECT_TAG:
                    twoDObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                    break;
                case PhaseSwitchManager.IMPORTED_OBJECT_TAG:
                    importedObject.serializedTransformPositionRotation.DeserializeTransform(gameObject);
                    break;
                case PhaseSwitchManager.MODEL_TARGET_TAG:
                    break;
            }
        }
    }

}
