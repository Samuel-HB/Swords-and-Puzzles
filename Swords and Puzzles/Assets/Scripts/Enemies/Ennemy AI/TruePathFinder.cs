using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class PathFindingCell : IComparable<PathFindingCell>
{
    public PathFindingCell(Vector2Int inPos, int inTravelCost, Vector2Int inDestination)
    {
        pos = inPos;
        travelCost = inTravelCost;
        heuristicCost = Vector2.Distance(pos, inDestination);
    }
    public PathFindingCell(Vector2Int inPos, PathFindingCell cell, Vector2Int inDestination)
    {
        pos = inPos;
        previous = cell;
        travelCost = cell.travelCost + 1;
        heuristicCost = Vector2.Distance(pos, inDestination);
    }

    public Vector2Int pos;
    public int travelCost;
    public float heuristicCost;
    public PathFindingCell previous = null;

    public float GetCost()
    {
        return travelCost + heuristicCost;
    }

    public int CompareTo(PathFindingCell other)
    {
        if (other.GetCost() > GetCost()) {
            return -1;
        }
        else if (other.GetCost() < GetCost()) {
            return 1;
        }
        return 0;
    }
}

public class TruePathFinder : MonoBehaviour
{
    public GameObject start;
    public GameObject stop;

    [SerializeField] private Tilemap tilemap;

    private List<TileBase> wallTile = new List<TileBase>();
    [SerializeField] private TileBase wallTileBase;

    [HideInInspector] public List<Vector2Int> path = new List<Vector2Int>();
    PathFindingCell endCell = new PathFindingCell(new Vector2Int(0, 0), 0, new Vector2Int(0, 0));

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++) {
                Gizmos.DrawWireSphere((Vector2)path[i], 0.5f);
            }
        }
    }

    private void Awake()
    {
        if (!TryGetComponent<Tilemap>(out Tilemap tilemap)) {
            print("missing tilemap");
        }
    }

    private void Start()
    {
        if (!tilemap) {
            return;
        }
        if (!start || !stop) {
            print("missing start or stop");
            return;
        }
        FindPath(start.transform.position, stop.transform.position);
    }

    public Vector2Int ConvertToGrid(Vector2 worldPos)
    {
        return VectorHelpers.Vec3ToVec2(tilemap.WorldToCell(tilemap.WorldToCell(worldPos)));
    }

    public bool FindPath(Vector2 From, Vector2 To)
    {
        Vector2Int gFrom = ConvertToGrid(From);
        Vector2Int gTo = ConvertToGrid(To);
        PathFindingCell current = new PathFindingCell(gFrom, 0, gTo);

        List<PathFindingCell> cOpen = new List<PathFindingCell>() { current };
        List<PathFindingCell> cClose = new List<PathFindingCell>();

        bool keepGoing = true;
        while (cOpen.Count > 0 && keepGoing)
        {
            Console.WriteLine(cOpen);
            Console.WriteLine(cClose);
            cOpen.Sort();
            current = cOpen[0];
            cOpen.RemoveAt(0);

            List<PathFindingCell> cSuccesors = new List<PathFindingCell>();
            GetOpenNeighborTiles(current, gTo, ref cSuccesors);

            foreach (PathFindingCell succesor in cSuccesors)
            {
                if (succesor.pos == gTo)
                {
                    keepGoing = false;
                    MakeFinalPath(succesor, gTo);
                    break;
                }
                int cellIndex = 0;
                float cellCost = GetCostFromList(ref cOpen, succesor.pos, ref cellIndex);

                if (cellIndex != -1 && cellCost > succesor.GetCost()) {
                    cOpen[cellIndex] = succesor;
                }
                else if (cellIndex == -1) {
                    cOpen.Add(succesor);
                }
            }
            cClose.Add(current);
        }

        if (keepGoing) return false;

        foreach (PathFindingCell cell in cClose) {
        }
        return true;
    }

    private void GetOpenNeighborTiles(PathFindingCell cell, Vector2Int dest, ref List<PathFindingCell> cNeighbors)
    {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                Vector2Int neighbor = cell.pos + new Vector2Int(x, y);
                if (IsWall(neighbor))
                {
                    continue;
                }

                PathFindingCell newCell = new PathFindingCell(neighbor, cell, dest);

                int index = cNeighbors.IndexOf(newCell);
                if (index != -1)
                {
                    if (cNeighbors[index].travelCost > newCell.travelCost) {
                        cNeighbors[index] = newCell;
                    }
                }
                else {
                    cNeighbors.Add(newCell);
                }
            }
        }
    }

    // problem here, in GetCostFromList()
    // ai seems to walk in rounds, like go left then right, and left again
    private float GetCostFromList(ref List<PathFindingCell> cellList, Vector2Int pos, ref int index)
    {
        int foundIndex = -1;
        foreach (PathFindingCell cell in cellList)
        {
            foundIndex++;
            if (cell.pos == pos) break;
        }
        index = foundIndex;

        if (foundIndex == -1) return -1;

        return cellList[foundIndex].GetCost();
    }

    private void MakeFinalPath(PathFindingCell succesor, Vector2Int gTo)
    {
        path.Add(gTo);
        endCell = succesor;
        while (endCell.previous != null)
        {
            path.Add(endCell.pos);
            endCell = endCell.previous;
        }
        path.Add(succesor.pos);
        path.Reverse();
    }


    private bool IsWall(Vector2Int pos)
    {
        return tilemap.HasTile(VectorHelpers.Vec2ToVec3(pos));
    }

    public static class VectorHelpers
    {
        public static Vector2Int Vec3ToVec2(Vector3Int In)
        {
            return new Vector2Int(
                In.x,
                In.y
            );
        }

        public static Vector3Int Vec2ToVec3(Vector2Int In)
        {
            return new Vector3Int(
                In.x,
                In.y,
                0
            );
        }
    }
}
