using UnityEngine;

public class Cell
{
    public Vector2Int position;
    public float gCost = int.MaxValue;
    public float hCost = int.MaxValue;
    public float fCost = int.MaxValue;
    public Vector2Int connection;
    public bool isObstacle;

    public Cell(Vector2Int pos)
    {
        position = pos;
    }
}
