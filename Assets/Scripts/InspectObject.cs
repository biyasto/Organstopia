using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    // Start is called before the first frame update
    float rotSpeed = 5;

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * 10 * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * 7 * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }
}
