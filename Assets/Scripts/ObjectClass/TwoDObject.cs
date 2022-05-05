using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TwoDObject : ThreeDObject
{
    public string m_Title;
    public string m_ImgPath;

    public TwoDObject(string m_ImgPath, string m_Title, string uuid, Transform _transform, 
        string type="Text", string parentUuid="",bool isRoot=false): base(uuid, _transform, type, parentUuid, isRoot)
    {
        this.m_ImgPath = m_ImgPath;
        this.m_Title = m_Title;

    }
    public TwoDObject():base()
    {
        m_ImgPath = "";
        m_Title = "";
    }
}
