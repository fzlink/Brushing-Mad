using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrushTongueRush : MonoBehaviour
{
    private bool canStart;

    private RaycastHit hit;
    private bool isHit;

    void Start()
    {
        
    }

    void Update()
    {
        if (canStart && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, transform.position.z));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime *100);
            }

            Clean();
        }
    }

    private void Clean()
    {
        //Physics.Raycast(transform.position, Vector3.forward, out hit, 10f)
        
        if(Physics.BoxCast(transform.position,new Vector3(0.01f,0.02f,0.01f),Vector3.forward,out hit,transform.rotation,20f))
        {
            if(hit.transform != transform)
            {
                isHit = true;
                if (!hit.transform.gameObject.GetComponent<Enemy>().isTaken)
                {
                    hit.transform.gameObject.GetComponent<Enemy>().SetTaken();
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
        if (!Application.isMobilePlatform && canStart)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            objPosition.z = transform.position.z;
            transform.position = objPosition;
            Clean();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, new Vector3(0.5f, 1f, 0.5f));
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * 20f);

        }
        

    }
}
