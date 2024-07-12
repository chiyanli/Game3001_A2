using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : Character
{
    private Path_Manager pathManager;

    void Start()
    {
        pathManager = FindObjectOfType<Path_Manager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetTargetPosition(targetPosition);
        }
    }

    void SetTargetPosition(Vector2 position)
    {
        // Convert position to tile coordinates
        //Vector3Int tilePosition = pathManager.Grid.WorldToCell(position);

        // Implement pathfinding to find the path to the target position
        //List<PathNode> path = pathManager.FindPath(transform.position, tilePosition);

        // Move the monster along the path
        //StartCoroutine(MoveAlongPath(path));
    }

    //IEnumerator MoveAlongPath(List<PathNode> path)
    //{
    //    foreach (PathNode node in path)
    //    {
    //        //Vector3 targetPosition = node.transform.position;
    //        //while ((targetPosition - transform.position).sqrMagnitude > 0.01f)
    //        //{
    //        //    transform.position = Vector3.MoveTowards(transform.position, targetPosition, MovementSpeed * Time.deltaTime);
    //        //    yield return null;
    //        //}
    //    }
    //}
}
