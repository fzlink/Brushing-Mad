using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethManager : MonoBehaviour
{
    public GameObject toothPrefab;
    public GameObject enemyPrefab;
    public GameObject gum;
    public GameObject finishLine;
    public Transform rail;
    public Transform enemies;
    public Transform gums;
    public List<LineRenderer> lines = new List<LineRenderer>();

    private List<GameObject> enemyList = new List<GameObject>();

    private Vector3 pointA;
    private Vector3 pointB;

    private int enemyCount;

    private int numOftoothToCreate;
    private float distance;
    private float lerpValue;
    private Vector3 instantiatePosition;

    // Start is called before the first frame update
    void Start()
    {
        pointA = Vector3.zero;
        pointB = Vector3.zero;
        CreateRail();
        GameManager.instance.winBar.SetMaxBar(enemyCount);
    }

    private void CreateRail()
    {
        //List<GameObject> teeth = new List<GameObject>();
        for (int railIndex = 0; railIndex < lines.Count; railIndex++)
        {
            Vector3[] positions = new Vector3[lines[railIndex].positionCount];
            lines[railIndex].GetPositions(positions);
            pointB = new Vector3(0,0,pointA.z + positions[positions.Length-1].z + positions[1].z);
            MakeEnemyTrail(positions);
            numOftoothToCreate = Mathf.RoundToInt(Vector3.Distance(pointA, pointB) / (toothPrefab.GetComponent<MeshRenderer>().bounds.extents.x * 2));
            distance = toothPrefab.GetComponent<MeshRenderer>().bounds.extents.x / 2 / numOftoothToCreate;
            for (int i = 0; i < numOftoothToCreate; i++)
            {
                instantiatePosition = Vector3.Lerp(pointA, pointB, lerpValue);
                //Instantiate the object
                Instantiate(toothPrefab, instantiatePosition, Quaternion.Euler(-90f, 0f, 0f), rail);
                //We increase our lerpValue
                lerpValue += distance;
                //Get the position
            }
            lerpValue = 0;
            if(railIndex < lines.Count - 1)
                pointA = CreateGum(pointB);

        }
        Instantiate(finishLine, pointB + new Vector3(-3f,0,-2f) , Quaternion.identity);
        print(rail.childCount);
    }

    private Vector3 CreateGum(Vector3 startGumLoc)
    {
        GameObject newGum = Instantiate(gum, startGumLoc, Quaternion.identity);
        newGum.transform.position += new Vector3(3, -1f, -newGum.GetComponent<MeshRenderer>().bounds.extents.z/2.5f);
        return startGumLoc + new Vector3(0, 0, newGum.GetComponent<MeshRenderer>().bounds.extents.z * 2f);
    }

    private void MakeEnemyTrail(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length - 1; i++)
        {
            PlaceEnemies(positions[i] + pointA, positions[i + 1] + pointA);
        }
    }

    private void PlaceEnemies(Vector3 a, Vector3 b)
    {
        Vector3 spawnPoint;
        for (int i = 0; i < 5; i++)
        {
            spawnPoint = Vector3.Lerp(a, b, i * 0.2f);
            GameObject tmp = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity,enemies);
            enemyList.Add(tmp);
            enemyCount++;
        }
    }
}
