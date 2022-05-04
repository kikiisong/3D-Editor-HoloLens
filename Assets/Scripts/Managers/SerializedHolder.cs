using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializedHolder
{
    public List<ThreeDObject> threeDObjects = new List<ThreeDObject>();
    public List<ImportedObject> importedObjects = new List<ImportedObject>();
    public List<TextObject> textObjects = new List<TextObject>();
    public List<TwoDObject> twoDObjects = new List<TwoDObject>();
    public bool modelTargetActivated=false;
}
