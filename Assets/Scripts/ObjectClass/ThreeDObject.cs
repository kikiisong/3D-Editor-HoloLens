using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThreeDObject
{
    // Start is called before the first frame update
    public string type;
    public string uuid;
    public string parentUuid = "";
    public SerializedTransformPositionRotationScale serializedTransformPositionRotation;

    public ThreeDObject(string uuid, Transform _transform, string type="Cube", string parentUuid="", bool isRoot=false)
    {
        this.type = type;
        serializedTransformPositionRotation = new SerializedTransformPositionRotationScale(_transform);
        this.uuid = uuid;
        this.parentUuid = parentUuid;
    }

    public ThreeDObject()
    {
        this.type = "Cube";
        serializedTransformPositionRotation = new SerializedTransformPositionRotationScale();
        this.uuid = Guid.NewGuid().ToString();
        this.parentUuid = "";
    }

    public ThreeDObject(string uuid) : this()
    {
        this.uuid = uuid;
    }
}

