using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextObject : ThreeDObject
{

    public string m_Title;
    public string m_Content;

    public TextObject(string m_Content, string m_Title, string uuid, Transform _transform, 
        string type="Text", string parentUuid="",bool isRoot=false): base(uuid, _transform, type, parentUuid, isRoot)
    {
        this.m_Content = m_Content;
        this.m_Title = m_Title;

    }
    public TextObject():base()
    {
        m_Content = "";
        m_Title = "";
    }
}
