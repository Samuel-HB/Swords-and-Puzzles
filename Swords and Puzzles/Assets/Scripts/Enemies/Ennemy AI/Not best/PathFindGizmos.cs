using System.Collections.Generic;
using UnityEngine;

public class PathFindGizmos : MonoBehaviour
{
    [SerializeField] private GridManager gridRef;
    [SerializeField] private PathFinder pathFindRef;

    [SerializeField] private bool canVisualizePath = false;

    private void OnDrawGizmos()
    {
        if (!canVisualizePath || gridRef.cells == null) {
            return;
        }
        foreach (KeyValuePair<Vector2Int, Cell> keyValuePair in gridRef.cells)
        {
            if (!keyValuePair.Value.isObstacle)
            {
                Gizmos.color = Color.white;
            }
            else {
                Gizmos.color = Color.black;
            }

            //if (pathFindRef.finalPath.Contains(keyValuePair.Key)) {
            //    Gizmos.color = Color.magenta;
            //}

            Gizmos.DrawCube(keyValuePair.Key + (Vector2)transform.position, new Vector3(gridRef.cellWidth, gridRef.cellHeight));
        }
    }
}
