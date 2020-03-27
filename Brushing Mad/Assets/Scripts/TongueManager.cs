using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueManager : MonoBehaviour
{

    public GameObject bacteria;
    public MeshRenderer meshRenderer;
    public Transform bacteriaContainer;

    [Range(1,20)]
    public int bacteriaCount;
    private float boundMarginX = 0.15f;
    private float boundMarginY = 0.15f;

    void Start()
    {
        GenerateBacterias();
        TongueRushManager.instance.winBar.SetMaxBar(bacteriaCount);
    }

    public void GenerateBacterias()
    {
        for (int i = 0; i < bacteriaCount; i++)
        {
            Instantiate(bacteria, RandomPoint(), Quaternion.identity, bacteriaContainer);
        }

    }

    private Vector3 RandomPoint()
    {
        Bounds bounds = meshRenderer.bounds;
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x+ boundMarginX, bounds.max.x- boundMarginX),
            UnityEngine.Random.Range(bounds.min.y+ boundMarginY, bounds.max.y- boundMarginY),
            0.82f
        );
    }

    void Update()
    {
        
    }
}
