using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayerY : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
        Vector3 rot = transform.localEulerAngles;
        rot.y += 180f;
        rot.x = 360f - rot.x;
        transform.localEulerAngles = rot;
    }
}
