using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedTransformPositionRotationScale
{
    public float[] _position = new float[3];
    public float[] _rotation = new float[4];
    public float[] _scale = new float[3];
    public bool worldSpace = false;


    public SerializedTransformPositionRotationScale()
    {
        _position[0] = 0;
        _position[1] = 0;
        _position[2] = 0;

        _rotation[0] = 0;
        _rotation[1] = 0;
        _rotation[2] = 0;
        _rotation[3] = 0;

        _scale[0] = 1;
        _scale[1] = 1;
        _scale[2] = 1;

    }
   public SerializedTransformPositionRotationScale(Transform transform, bool worldSpace = false)
    {
        this.worldSpace = worldSpace;
        if (this.worldSpace)
        {
            _position[0] = transform.position.x;
            _position[1] = transform.position.y;
            _position[2] = transform.position.z;

            _rotation[0] = transform.rotation.x;
            _rotation[1] = transform.rotation.y;
            _rotation[2] = transform.rotation.z;
            _rotation[3] = transform.rotation.w;

        }else
        { 
            _position[0] = transform.localPosition.x;
            _position[1] = transform.localPosition.y;
            _position[2] = transform.localPosition.z;

            _rotation[0] = transform.localRotation.x;
            _rotation[1] = transform.localRotation.y;
            _rotation[2] = transform.localRotation.z;
            _rotation[3] = transform.localRotation.w;
        }
        _scale[0] = transform.localScale.x;
        _scale[1] = transform.localScale.y;
        _scale[2] = transform.localScale.z;

    }

    public void DeserializeTransform(GameObject gameObject)
    {
        if (worldSpace)
        {
            gameObject.transform.position = new Vector3(_position[0], _position[1], _position[2]);
            gameObject.transform.rotation = new Quaternion(_rotation[0], _rotation[1], _rotation[2], _rotation[3]);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(_position[0], _position[1], _position[2]);
            gameObject.transform.localRotation = new Quaternion(_rotation[0], _rotation[1], _rotation[2], _rotation[3]);
        }
        gameObject.transform.localScale = new Vector3(_scale[0], _scale[1], _scale[2]);
    }
}
