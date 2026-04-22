using UnityEngine;

public class Cell
{
    public Vector2 position;
    public int gCost = int.MaxValue;
    public int hCost = int.MaxValue;
    public int fCost = int.MaxValue;
    public Vector2 connection;
    public bool isObstacle;

    public Cell(Vector2 pos)
    {
        position = pos;
    }
}
