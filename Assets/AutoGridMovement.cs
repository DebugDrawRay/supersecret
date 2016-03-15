using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoGridMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public float delay;
    private float currentDelay;
    public float speed;

    //Grid properties
    public Vector2 currentPoint;
    public Vector2 goalPoint;

    private Vector3 currentPosition;
    private Vector3 goalPosition;

    private Grid targetGrid;
    private bool initialized;

    [InspectorButton("OnButtonClicked")]
    public bool Move;

    private void OnButtonClicked()
    {
        MoveToDestination(goalPoint);
    }

    [InspectorButton("Init")]
    public bool Initialize;


    void Init()
    {
        targetGrid = Grid.instance;
        transform.SetParent(targetGrid.transform);
        currentPosition = targetGrid.GridToWorldPoisiton(currentPoint);
        goalPosition = targetGrid.GridToWorldPoisiton(goalPoint);

        transform.localPosition = currentPosition;
        initialized = true;
    }

    void Update()
    {
        if(initialized)
        {
            currentDelay -= Time.deltaTime;
            if(currentDelay <= 0)
            {
                MoveToRandomDestination();
                currentDelay = delay;
            }
        }
    }
    void MoveToDestination(Vector2 goal)
    {
        List<Vector2> path = Pathfinder.FindPath(currentPoint, goal, targetGrid.gridSize);
        StartCoroutine(MoveAcrossPath(path, speed));
    }

    void MoveToRandomDestination()
    {
        int ranX = Random.Range(0, targetGrid.xUnits);
        int ranY = Random.Range(0, targetGrid.yUnits);

        Vector2 goal = new Vector2(ranX, ranY);
        Debug.Log(ranX + " " + ranY);
        List<Vector2> path = Pathfinder.FindPath(currentPoint, goal, targetGrid.gridSize);
        StartCoroutine(MoveAcrossPath(path, speed));
    }

    IEnumerator MoveAcrossPath(List<Vector2> path, float time)
    {
        time = time / path.Count;
        for(int i = 1; i < path.Count; ++i)
        {
            Vector3 startPosition = transform.localPosition;
            Vector3 nextPosition = targetGrid.GridToWorldPoisiton(path[i]);
            for (float t = 0; t <= time; t += Time.deltaTime)
            {
                float moveTime = t / time;
                transform.localPosition = Vector3.Lerp(startPosition, nextPosition, moveTime);
                yield return null;
            }
            currentPoint = path[i];
        }
    }
}
