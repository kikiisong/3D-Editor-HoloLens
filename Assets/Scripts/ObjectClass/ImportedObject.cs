using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImportedObject : ThreeDObject
{
    public string path;
    
    public ImportedObject(string path, string uuid, Transform _transform, string type="ImportedObject", string parentUuid="", bool isRoot = false) : base(uuid, _transform, type, parentUuid, isRoot)
    {
        this.path = path;
    }

    public ImportedObject() : base()
    {
        this.path = "";
    }

}
