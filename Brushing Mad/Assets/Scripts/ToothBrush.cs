using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : MonoBehaviour
{
    public Transform pivot;
    public float speed;
    public float rotSpeed;
    public float holdRotPitch;
    public float notHoldRotPitch;

    public ParticleSystem bubbleParticles;

    Vector3 clampedPivotRotation;
    float pointer_x;

    public bool isHold;
    private bool canStart;
    public float scalingFactor = 0.25f;
    public float backAndForthDistance;
    public float backAndForthSpeed;

    public AudioSource brushSound;
    private Vector3 pivotStartPosition;
    public Vector3 backVector = new Vector3(0f, 0f, -5f);
    public Vector3 forwardVector = new Vector3(0f, 0f, 5f);
    private bool onTeeth;
    private const float tau = Mathf.PI * 2f;


    private void Start()
    {
        pivotStartPosition = pivot.transform.localPosition;
    }

    void Update()
    {
        if (canStart)
        {
            Move();
            InputHandle();
            BackAndForth();
            HoldHandle();
            ClampRotation();
        }
    }


    public void SetCanStart(bool canStart)
    {
        this.canStart = canStart;
    }

    private void BackAndForth()
    {
        if (isHold)
        {
            pivot.transform.localPosition = new Vector3(pivot.transform.localPosition.x, pivot.transform.localPosition.y, Mathf.Sin( (Time.time / backAndForthSpeed) * tau)  / backAndForthDistance + 0.5f);
        }
    }

    private void Move()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    private void HoldHandle()
    {
        float y = pivot.rotation.eulerAngles.y;
        float z = pivot.rotation.eulerAngles.z;
        if (isHold)
        {
            pivot.rotation = Quaternion.Slerp(pivot.rotation, Quaternion.Euler(holdRotPitch,y,z), Time.deltaTime / scalingFactor);
            if(Quaternion.Angle(pivot.rotation, Quaternion.Euler(holdRotPitch, y, z)) < 20f)
            {
                onTeeth = true;
                bubbleParticles.Play();
                if(!brushSound.isPlaying)
                    brushSound.Play();
            }

        }
        else
        {
            pivot.rotation = Quaternion.Slerp(pivot.rotation, Quaternion.Euler(notHoldRotPitch,y,z), Time.deltaTime / scalingFactor);
            pivot.transform.localPosition = Vector3.MoveTowards(pivot.transform.localPosition, pivotStartPosition, Time.deltaTime / 0.01f);
            bubbleParticles.Stop();
            if (brushSound.isPlaying)
                brushSound.Stop();
            if (Quaternion.Angle(pivot.rotation, Quaternion.Euler(notHoldRotPitch, y, z)) < 20f)
            {
                onTeeth = false;
            }
        }

    }

    private void InputHandle()
    {
        if (Application.isMobilePlatform)
        {
            pointer_x = Input.GetAxis("Mouse X");
            if (Input.touchCount > 0)
            {
                isHold = true;
                pointer_x = Input.touches[0].deltaPosition.x;
            }
            else
            {
                isHold = false;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                isHold = true;
            }
            else
            {
                isHold = false;
            }
            pointer_x = Input.GetAxis("Horizontal") *10;
        }

        if (isHold)
        {
            pivot.Rotate(-Vector3.forward, pointer_x * rotSpeed * Time.deltaTime);
        }


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
        if (other.CompareTag("Enemy"))
        {
            //other.transform.position = Vector3.MoveTowards(other.transform.position, other.transform.position + new Vector3(0, 20, 0), 2000);
            other.gameObject.GetComponent<Enemy>().SetTaken();
            GameManager.instance.IncreasePoint();
        }
        else if (other.CompareTag("Obstacle"))
        {
            SendWinLoseState(false);
        }
        else if (other.CompareTag("FinishLine"))
        {
            SendWinLoseState(true);
        }
    }

    private void SendWinLoseState(bool isOnFinish)
    {
        bubbleParticles.Stop();
        brushSound.Stop();
        GameManager.instance.StopGame();
        GameManager.instance.DecideWinLoseState(isOnFinish);
    }
}
