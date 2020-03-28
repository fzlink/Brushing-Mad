using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDropper : MonoBehaviour
{

    public float[] xOffsets;
    public Vector2 defaultYZ;
    public GameObject food;
    public Transform foodContainer;
    private float foodCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator DropFood()
    {
        foodCount = FlossRushManager.instance.timeLimit;
        for (int i = 0; i < foodCount; i++)
        {
            int randomInd = Random.Range(0, 3);
            GameObject foodTmp = Instantiate(food, new Vector3(xOffsets[randomInd], 10f, defaultYZ.y), Quaternion.identity, foodContainer);
            foodTmp.GetComponent<Enemy>().dropStopY = defaultYZ.x;
            foodTmp.GetComponent<Enemy>().dropping = true;

            yield return new WaitForSeconds(1f);
        }
    }

    public void StartDropping()
    {
        StartCoroutine(DropFood());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
