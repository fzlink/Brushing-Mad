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

    private void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        if(enemyType == EnemyType.Yellowness)
            takenGoal = transform.position + new Vector3(0f, 10f, 30f);
        else if(enemyType == EnemyType.Bacteria)
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
