using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType{Yellowness, Bacteria, Food};
    public EnemyType enemyType;
    public bool isTaken;
    private float t;
    public float takeSpeed;
    public Vector3 takenGoal;
    private MeshRenderer meshRenderer;

    public bool dropping;
    public float dropStopY;
    private float dropSpeed = 30f;

    private void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        if(enemyType == EnemyType.Yellowness)
            takenGoal = transform.position + new Vector3(0f, 10f, 30f);
        else if(enemyType == EnemyType.Bacteria || enemyType == EnemyType.Food)
            takenGoal = transform.position + new Vector3(0f, 10f, 0f);
    }

    public void SetTaken()
    {
        isTaken = true;
    }



    // Update is called once per frame
    void Update()
    {
        //if(Vector3.Distance(GameManager.instance.toothBrush.transform.position,transform.position) < 50f)
        //{
        //    meshRenderer.enabled = true;
        //}
        //else
        //{
        //    meshRenderer.enabled = false;
        //}
        if (dropping)
        {
            GetComponent<BoxCollider>().enabled = false;
            t = Time.deltaTime * dropSpeed;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,dropStopY,transform.position.z), t);
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, dropStopY, transform.position.z)) <= 0.01f)
            {
                dropping = false;
                GetComponent<BoxCollider>().enabled = true;

            }
        }


        if (isTaken)
        {
            t = Time.deltaTime * takeSpeed;
            transform.position = Vector3.MoveTowards(transform.position, takenGoal, t);
            if(Vector3.Distance(transform.position,takenGoal) <= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
