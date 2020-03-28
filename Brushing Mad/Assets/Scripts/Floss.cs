using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floss : MonoBehaviour
{

    public Transform pivot;

    private bool canStart;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 scanPos;
    private bool allowDrag = true;

    private Vector3 lastPos;


    public float speed= 15; // speed factor 
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
                Drag(Input.mousePosition);
            }
            else
            {  // no mouse button pressed
                dragObj = null;  // dragObj free to drag another object
            }
        }
        else if(canStart && Application.isMobilePlatform)
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
    }


    //if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
    //{
    //    // get the touch position from the screen touch to world point
    //    Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, transform.position.z));
    //    // lerp and set the position of the current object to that of the touch, but smoothly over time.
    //    transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 10);
    //}

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
        }
    }

    public void SetCanStart(bool canStart)
    {
        this.canStart = canStart;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.CompareTag("Enemy") && !collision.other.GetComponent<Enemy>().isTaken)
        {
            var normal = collision.contacts[0].normal;
            if (normal.y > 0)
            { //if the bottom side hit something 
                collision.collider.isTrigger = true;
                Debug.Log("You Hit the floor");
            }
            if (normal.y < 0)
            { //if the top side hit something
                collision.gameObject.GetComponent<Enemy>().SetTaken();
                FlossRushManager.instance.IncreasePoint();
                Debug.Log("You Hit the roof");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") && !other.gameObject.GetComponent<Enemy>().isTaken){
            other.isTrigger = false;
        }
    }

    //void OnMouseDown()
    //{
    //    screenPoint = Camera.main.WorldToScreenPoint(transform.position);


    //    offset = transform.position - Camera.main.ScreenToWorldPoint(
    //        new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    //}


    //void OnMouseDrag()
    //{

    //    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);


    //    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
    //    curPosition.z = 7.5f;
    //    lastPos = transform.position;
    //    if(allowDrag)
    //        transform.position = curPosition;
    //    //print(curPosition);
    //    //RaycastHit hit;
    //    //if(Physics.Raycast(transform.position, curPosition - transform.position ,out hit,1f))
    //    //{
    //    //    Debug.DrawLine(transform.position , hit.point, Color.green);
    //    //}
    //    //else
    //    //{

    //    //    Debug.DrawLine(transform.position, (offset - transform.position) * 0.001f, Color.red);
    //    //}
    //}



}
