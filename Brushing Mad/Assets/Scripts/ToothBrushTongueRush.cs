using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrushTongueRush : MonoBehaviour
{
    private bool canStart;

    private bool isHit;

    public float speed = 15; // speed factor 
    public float maxSpeed = 25; // speed limit 
    private Transform dragObj = null;
    private RaycastHit hit;
    private float length;

    void Start()
    {
        
    }

    void Update()
    {
        if (canStart && !Application.isMobilePlatform)
        {
            if (Input.GetMouseButton(0))
            {  // if left mouse button pressed...
               // cast a ray from the mouse pointer
                //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                //transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
                Drag(Input.mousePosition);
            }
            else
            {  // no mouse button pressed
                dragObj = null;  // dragObj free to drag another object
            }
        }
        else if (canStart && Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero
                Drag(touch.position);

            }
            else
            {
                dragObj = null;
            }
        }
        if (!canStart)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        //if (canStart && Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

        //    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //    {
        //        // get the touch position from the screen touch to world point
        //        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, transform.position.z));
        //        // lerp and set the position of the current object to that of the touch, but smoothly over time.
        //        transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime *100);
        //    }

        //    Clean();
        //}
    }

    private void Drag(Vector3 inputPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPos);
        if (!dragObj)
        {  // if nothing picked yet...
           // and the ray hit some rigidbody...
            if (Physics.Raycast(ray, out hit) && hit.rigidbody)
            {
                dragObj = hit.transform;  // save picked object's transform
                length = hit.distance;  // get "distance from the mouse pointer"
            }
        }
        else
        {  // if some object was picked...
           // calc velocity necessary to follow the mouse pointer
            var vel = (ray.GetPoint(length) - dragObj.position) * speed;
            // limit max velocity to avoid pass through objects
            if (vel.magnitude > maxSpeed) vel *= maxSpeed / vel.magnitude;
            // set object velocity
            dragObj.gameObject.GetComponent<Rigidbody>().velocity = vel;
            Clean();
        }
    }

    private void Clean()
    {
        //Physics.Raycast(transform.position, Vector3.forward, out hit, 10f)
        RaycastHit hitBox;
        if(Physics.BoxCast(transform.position,new Vector3(0.01f,0.02f,0.01f),Vector3.forward,out hitBox, transform.rotation,20f))
        {
            if(hitBox.transform != transform)
            {
                isHit = true;
                if (!hitBox.transform.gameObject.GetComponent<Enemy>().isTaken)
                {
                    hitBox.transform.gameObject.GetComponent<Enemy>().SetTaken();
                    TongueRushManager.instance.IncreasePoint();
                }
            }
        }
    }

    public void SetCanStart(bool canStart)
    {
        this.canStart = canStart;
    }

    void OnMouseDrag()
    {
        //if (!Application.isMobilePlatform && canStart)
        //{
        //    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        //    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //    objPosition.z = transform.position.z;
        //    transform.position = objPosition;
        //    Clean();
        //}
    }



}
