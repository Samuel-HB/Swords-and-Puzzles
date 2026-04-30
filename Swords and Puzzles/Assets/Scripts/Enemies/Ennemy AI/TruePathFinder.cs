using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static UnityEditor.PlayerSettings;

class PathFindingCell : IComparable<PathFindingCell>
{
    //PathFindingCell(gFrom, 0, gTo);
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

    //public static bool operator ==(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.pos == R.pos;
    //}
    //public static bool operator !=(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.pos != R.pos;
    //}
    //public static bool operator >(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.GetCost() > R.GetCost();
    //}
    //public static bool operator <(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.GetCost() < R.GetCost();
    //}
    //public static bool operator >=(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.GetCost() >= R.GetCost();
    //}
    //public static bool operator <=(PathFindingCell L, PathFindingCell R)
    //{
    //    return L.GetCost() <= R.GetCost();
    //}
}

public class TruePathFinder : MonoBehaviour
{
    //new
    //public List<PathFindingCell> cOpen = new List<PathFindingCell>();


    public GameObject start;
    public GameObject stop;

    [SerializeField] private Tilemap tilemap;

    //private List<TileBase> wallTile;
    private List<TileBase> wallTile = new List<TileBase>();
    [SerializeField] private TileBase wallTileBase;

    //new
    //List<PathFindingCell> cPath = new List<PathFindingCell>();
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
        //InstantiateWalls(); // new
        FindPath(start.transform.position, stop.transform.position);
    }

    public Vector2Int ConvertToGrid(Vector2 worldPos)
    {
        return VectorHelpers.Vec3ToVec2(tilemap.WorldToCell(tilemap.WorldToCell(worldPos)));
    }

    // before not public
    public bool FindPath(Vector2 From, Vector2 To)
    {
        Vector2Int gFrom = ConvertToGrid(From);
        Vector2Int gTo = ConvertToGrid(To);
        PathFindingCell current = new PathFindingCell(gFrom, 0, gTo);

        List<PathFindingCell> cOpen = new List<PathFindingCell>() { current };
        //cOpen = new List<PathFindingCell>() { current };
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
                    //path.Add(gTo);
                    //endCell = succesor;
                    //while (endCell.previous != null)
                    //{
                    //    path.Add(endCell.pos);
                    //    endCell = endCell.previous;
                    //}
                    //cPath.Add(succesor);
                    break;
                }
                int cellIndex = 0;
                float cellCost = GetCostFromList(ref cOpen, succesor.pos, ref cellIndex);

                print("cellCost : " + cellCost);

                if (cellIndex != -1 && cellCost > succesor.GetCost()) {
                    cOpen[cellIndex] = succesor;
                }
                else if (cellIndex == -1) {
                    cOpen.Add(succesor);
                }
            }
            print("open nodes : " + cOpen.Count);
            cClose.Add(current);
        }

        print("keepGoing :" + keepGoing);
        if (keepGoing) return false;

        foreach (PathFindingCell cell in cClose) {
            print("cell position: " + cell.pos);
        }
        return true;
    }



            //
            //foreach (PathFindingCell succesor in cSuccesors)
            //{
            //    if (succesor.pos == gTo)
            //    {
            //        keepGoing = false;
            //        break;
            //        // end
            //    }
            //    int cellIndex = 0;
            //    float cellCost = GetCostFromList(ref cOpen, succesor.pos, ref cellIndex);
            //    if (cellIndex != -1 && cellCost < succesor.GetCost()) continue;

            //    cellCost = GetCostFromList(ref cClose, succesor.pos, ref cellIndex);
            //    if (cellIndex != -1)
            //    {
            //        if (cellCost < succesor.GetCost()) continue;
            //        //else {
            //        //    print($"{cellIndex} / {cClose.Count}");
            //        //    cOpen.Add(cClose[cellIndex]);
            //        //}
                    
            //            print($"{cellIndex} / {cClose.Count}");
            //            cOpen.Add(cClose[cellIndex]);
                    
            //    }
            //    cOpen.Add(succesor);
            //}
            //print("open nodes : " + cOpen.Count);
            //cClose.Add(current);
            //


    private void GetOpenNeighborTiles(PathFindingCell cell, Vector2Int dest, ref List<PathFindingCell> cNeighbors)
    {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                Vector2Int neighbor = cell.pos + new Vector2Int(x, y);
                if (IsWall(neighbor))
                {
                    print("wall");
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
        return tilemap.HasTile(VectorHelpers.Vec2ToVec3(pos)
        );
        //return tilemap.GetTile(VectorHelpers.Vec2ToVec3(pos)
        //);
    }

    //private bool IsWall(Vector2Int pos)
    //{
    //    return wallTile.Contains(
    //        tilemap.GetTile(VectorHelpers.Vec2ToVec3(pos))
    //    );
    //}

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


    // new

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;

    //    foreach (KeyValuePair<Vector2Int, PathFindingCell> keyValuePair in cells)
    //    {
    //        Gizmos.DrawWireSphere(new Vector3(keyValuePair.Key.x, keyValuePair.Key.y), .5f);
    //    }
    //}


    [SerializeField] private GameObject wallPrefab;
    //private bool areObstaclesInstantiated = false;

    [SerializeField] private int gridWidth = 48;
    [SerializeField] private int gridHeight = 27;

    private void GetAllWallTiles()
    {
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, (int)tilemap.transform.position.y);

                if (tilemap.HasTile(pos)) {
                    wallTile.Add(tilemap.GetTile(pos));
                }
            }
        }
    }

    //private void InstantiateWalls()
    //{
    //    for (int x = 0; x < gridWidth; x++){
    //        for (int y = 0; y < gridHeight; y++)
    //        {
    //            Vector3Int pos = VectorHelpers.Vec2ToVec3(ConvertToGrid(new Vector2Int(x, y)));
  
    //            if (tilemap.HasTile(pos))
    //            {
    //                if (!areObstaclesInstantiated) {
    //                    Instantiate(wallPrefab, pos, Quaternion.identity);
    //                }
    //            }
    //        }
    //    }
    //    areObstaclesInstantiated = true;
    //}



    //public override OnEditorGUI()
    //{
    //    DrawDefaultInspector();
    //    if (GUILayout.Button("Test Astar", GUILayout))
    //    {
    //        if (!Start || !Stop)
    //        {
    //            print("missing start or stop");
    //            return;
    //        }
    //    }
    //}
}
