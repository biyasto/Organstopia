using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    /*// Start is called before the first frame update
    float rotSpeed = 5;

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * 10 * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * 7 * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }*/
    private Rigidbody rb;
    private bool dragging = false;
     void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

     private void OnMouseDrag()
     {
         dragging = true;
     }

     private void Update()
     {
         if (Input.GetMouseButtonUp(0))
         {
             dragging = false;
         }
     }

     private void FixedUpdate()
     {
         if (dragging)
         {
             float x = Input.GetAxis("Mouse X") * 300 * Time.fixedDeltaTime;
             float y = Input.GetAxis("Mouse Y") * 40 * Time.fixedDeltaTime;

             rb.AddTorque(Vector3.down*x);
             rb.AddTorque(Vector3.right*y);
         }
     }
}
