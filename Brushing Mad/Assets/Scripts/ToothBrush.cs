using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : MonoBehaviour
{

    public Transform pivot;
    public float speed;
    public float rotSpeed;

    Vector3 clampedPivotRotation;
    float pointer_x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        InputHandle();

    }

    private void Move()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    private void InputHandle()
    {
       // float xThrow = Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
       // //pivot.Rotate(Vector3.forward, xThrow);

      
        pointer_x = Input.GetAxis("Mouse X");
        //float pointer_y = Input.GetAxis("Mouse Y");
        if (Input.touchCount > 0)
        {
            pointer_x = Input.touches[0].deltaPosition.x;
            //pointer_y = Input.touches[0].deltaPosition.y;
        }
        pivot.Rotate(-Vector3.forward, pointer_x * rotSpeed * Time.deltaTime);

        ClampRotation();
    }

    private void ClampRotation()
    {
        clampedPivotRotation = pivot.eulerAngles;
        clampedPivotRotation.z = ClampAngle(clampedPivotRotation.z, -90f, 90f);
        pivot.rotation = Quaternion.Euler(clampedPivotRotation);
    }

    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.transform.position = Vector3.MoveTowards(other.transform.position, other.transform.position + new Vector3(0, 20, 0), 2000);
        Destroy(other.gameObject);
    }

}
