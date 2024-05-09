using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    protected virtual void Init()
    {
        Util.GetOrAddComponent<CameraController>(Camera.main.gameObject);
    }
}
