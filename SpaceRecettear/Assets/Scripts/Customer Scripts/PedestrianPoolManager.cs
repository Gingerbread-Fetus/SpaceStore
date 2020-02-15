using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianPoolManager : MonoBehaviour
{
    [SerializeField] GameObject customerObject;
    [SerializeField] GameObject topSpawnArea;
    [SerializeField] GameObject topGoal;
    [SerializeField] GameObject botSpawnArea;
    [SerializeField] GameObject botGoal;
    [SerializeField] int numberOfPedestrians;
    [SerializeField] float spawnTimerLimit = 3.0f;

    PedestrianController pedestrianController;
    BoxCollider2D topCollider;
    BoxCollider2D botCollider;
    float xOffset;

    private void Start()
    {
        topCollider = topSpawnArea.GetComponent<BoxCollider2D>();
        botCollider = botSpawnArea.GetComponent<BoxCollider2D>();

        int x = 0;
        for(int i = 0; i <= numberOfPedestrians; i++)
        {
            x = 1 - x;
            StartCoroutine(SpawnOnTimer(x));
        }
    }

    IEnumerator SpawnOnTimer(int x)
    {
        yield return new WaitForSeconds(Random.Range(0f,spawnTimerLimit));
        if(x == 0)
        {
            //Spawn on top
            GameObject newPedestrian = Instantiate(customerObject,RandomPointsInBounds(topCollider.bounds),Quaternion.identity);
            pedestrianController = newPedestrian.GetComponent<PedestrianController>();
            pedestrianController.SetGoal(botGoal);
        }
        else
        {
            //Spawn on bottom
            GameObject newPedestrian = Instantiate(customerObject, RandomPointsInBounds(botCollider.bounds), Quaternion.identity);
            pedestrianController = newPedestrian.GetComponent<PedestrianController>();
            pedestrianController.SetGoal(topGoal);
        }
    }

    private Vector3 RandomPointsInBounds(Bounds bounds)
    {
        return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        0
    );
    }
}
