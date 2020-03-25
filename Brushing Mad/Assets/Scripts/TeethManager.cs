using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethManager : MonoBehaviour
{
    public GameObject toothPrefab;
    public GameObject enemyPrefab;
    public Transform rail;
    public Vector3 pointA;
    public Vector3 pointB;
    private LineRenderer lineRenderer;
    private int destroyToothStartIndex = 10;
    private bool destroyToothStart;

    int numOftoothToCreate;
    float distance;
    float lerpValue;
    Vector3 instantiatePosition;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(CreateRail());
        MakeEnemyTrail();
    }



    private IEnumerator CreateRail()
    {
        List<GameObject> teeth = new List<GameObject>();
        numOftoothToCreate = Mathf.RoundToInt(Vector3.Distance(pointA, pointB) / (toothPrefab.GetComponent<MeshRenderer>().bounds.extents.x * 2));
        distance = toothPrefab.GetComponent<MeshRenderer>().bounds.extents.x/2 / numOftoothToCreate;
        for (int i = 0; i < numOftoothToCreate; i++)
        {
            if(!destroyToothStart)
                destroyToothStartIndex--;
            if(destroyToothStartIndex == 0 || destroyToothStart)
            {
                destroyToothStart = true;
                Destroy(teeth[destroyToothStartIndex++]);
            }
            //We increase our lerpValue
            lerpValue += distance;
            //Get the position
            instantiatePosition = Vector3.Lerp(pointA, pointB, lerpValue);
            //Instantiate the object
            teeth.Add(Instantiate(toothPrefab, instantiatePosition,Quaternion.Euler(-90f,0f,0f), rail));
            yield return new WaitForSeconds(0.75f);
        }
        print(rail.childCount);
    }


    private void MakeEnemyTrail()
    {
        //lineRenderer.alignment = LineAlignment.Local;
        Vector3[] positions = new Vector3[21];
        lineRenderer.GetPositions(positions);

        for (int i = 0; i < positions.Length-1; i++)
        {
            PlaceEnemies(positions[i],positions[i+1]);
        }
    }

    private void PlaceEnemies(Vector3 a, Vector3 b)
    {
        Vector3 spawnPoint;
        for (int i = 0; i < 5; i++)
        {
            spawnPoint = Vector3.Lerp(a, b, i * 0.2f);
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
