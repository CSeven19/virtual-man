using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Rotate360 : MonoBehaviour
{
    [Tooltip("Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).")]
    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(rotationSpeed);
    }
}
