using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMake : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerTransform;
    [SerializeField] public GameObject avoidCirclePrefab;
    public LevelManager levelManager;

    public LineRenderer circleRenderer;
    public GameObject smallCirclePrefab;
    private Vector3 lastSpawnedCirclePosition;
    private Vector3 lastAvoidCirclePosition;
    public int sizeRadius = 5;
    public bool avoidSpawned = false;

    void Start()
    {
        drawCircle(100, sizeRadius);
        SpawnCircleTopLeft();
    }

    // Update is called once per frame
    void Update()
    {
        if (!avoidSpawned && levelManager.combo > 10)
        {
            avoidSpawned = true;
            SpawnAvoidCircle();
        }
    }

    void drawCircle(int steps, float radius){
        circleRenderer.positionCount = steps;

        for (int currentstep = 0; currentstep < steps; ++currentstep){
          float circumferenceProgress = (float) currentstep/steps;
          float currentradian = circumferenceProgress * 2 * Mathf.PI;
          float xScale = Mathf.Cos(currentradian) * radius;
          float yScale = Mathf.Sin(currentradian) * radius;

          Vector3 currentPosition = new Vector3(xScale,yScale,0);
          circleRenderer.SetPosition(currentstep, currentPosition);

        }
    }

    public void SpawnCircleTopLeft()
    {
        float randomAngle = Random.Range(0.5f, 1f * Mathf.PI);
        float x = Mathf.Cos(randomAngle) * sizeRadius;
        float y = Mathf.Sin(randomAngle) * sizeRadius;
        Instantiate(smallCirclePrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    public void SpawnRandomCircle()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float x = Mathf.Cos(randomAngle) * sizeRadius;
        float y = Mathf.Sin(randomAngle) * sizeRadius;

        Vector3 newCirclePosition = new Vector3(x, y, 0);
        float distanceToLastCircle = Vector3.Distance(newCirclePosition, lastSpawnedCirclePosition);
        float minDistanceToLastCircle = 5.0f;

        while (distanceToLastCircle < minDistanceToLastCircle)
        {
            // Generate a new random position
            randomAngle = Random.Range(0f, 2f * Mathf.PI);
            x = Mathf.Cos(randomAngle) * sizeRadius;
            y = Mathf.Sin(randomAngle) * sizeRadius;

            // Recalculate new distance
            newCirclePosition = new Vector3(x, y, 0);
            distanceToLastCircle = Vector3.Distance(newCirclePosition, lastSpawnedCirclePosition);
        }
        Instantiate(smallCirclePrefab, new Vector3(x, y, 0), Quaternion.identity);
        lastSpawnedCirclePosition = new Vector3(x, y, 0);
    }

    public void SpawnAvoidCircle()
    {
        /*float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float x = Mathf.Cos(randomAngle) * sizeRadius;
        float y = Mathf.Sin(randomAngle) * sizeRadius;

        Vector3 newCirclePosition = new Vector3(x, y, 0);
        float distanceToPlayer = Vector3.Distance(newCirclePosition, playerTransform.position);
        float minDistanceToPlayer = 1.2f;

        while (distanceToPlayer < minDistanceToPlayer)
        {
            // Generate a new random position
            randomAngle = Random.Range(0f, 2f * Mathf.PI);
            x = Mathf.Cos(randomAngle) * sizeRadius;
            y = Mathf.Sin(randomAngle) * sizeRadius;

            // Recalculate new distance
            newCirclePosition = new Vector3(x, y, 0);
            distanceToPlayer = Vector3.Distance(newCirclePosition, playerTransform.position);
        }*/
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float x = Mathf.Cos(randomAngle) * sizeRadius;
        float y = Mathf.Sin(randomAngle) * sizeRadius;

        Vector3 newCirclePosition = new Vector3(x, y, 0);
        float distanceToLastCircle = Vector3.Distance(newCirclePosition, lastAvoidCirclePosition);
        float minDistanceToLastCircle = 1.2f;

        while (distanceToLastCircle < minDistanceToLastCircle)
        {
            // Generate a new random position
            randomAngle = Random.Range(0f, 2f * Mathf.PI);
            x = Mathf.Cos(randomAngle) * sizeRadius;
            y = Mathf.Sin(randomAngle) * sizeRadius;

            // Recalculate new distance
            newCirclePosition = new Vector3(x, y, 0);
            distanceToLastCircle = Vector3.Distance(newCirclePosition, lastAvoidCirclePosition);
        }
        GameObject avoidCircle = Instantiate(avoidCirclePrefab, new Vector3(x, y, 0), Quaternion.identity);
        lastAvoidCirclePosition = new Vector3(x, y, 0);

        //GameObject avoidCircle = Instantiate(avoidCirclePrefab, new Vector3(x, y, 0), Quaternion.identity);
        Destroy(avoidCircle, 1.5f);
        levelManager.IncreasePoints();

        StartCoroutine(ResetAvoidSpawnedFlag());
    }
    private IEnumerator ResetAvoidSpawnedFlag()
    {
        yield return new WaitForSeconds(1.5f);
        avoidSpawned = false;
    }
}
