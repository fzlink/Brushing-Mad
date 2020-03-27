using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floss : MonoBehaviour
{

    public Transform pivot;

    private bool canStart;

    // Start is called before the first frame update
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
                transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 100);
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
        }
    }

}
