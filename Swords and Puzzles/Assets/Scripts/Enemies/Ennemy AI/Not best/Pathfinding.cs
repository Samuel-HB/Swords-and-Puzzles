using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    private Dictionary<Vector2, Cell> cells;

    [SerializeField] private List<Vector2> cellsToSearch;
    [SerializeField] private List<Vector2> cellsAlreadySearched;
    [SerializeField] private List<Vector2> finalPath;

    [SerializeField] private int gridHeight = 10; // put in awake tilemap bounds instead
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private float cellHeight = 1f;
    [SerializeField] private float cellWidth = 1f;

    [SerializeField] private bool canGeneratePath = false;
    private bool isPathGenerated = false;

    [SerializeField] private bool canVisualizeGrid = false;

    //new
    [SerializeField] private Tilemap tilemap;
    private List<Vector2Int> obstaclePositionsOnTilemap = new List<Vector2Int>();
    private Vector2 startCell = new Vector2(0, 1);
    private Vector2 endCell = new Vector2(8, 7);

    private int tilemapBoundX = 48;
    private int tilemapBoundY = 27;

    [SerializeField] private GameObject wallPrefab;
    private bool areObstaclesInstantiated = false;


    private void OnDrawGizmos()
    {
        if (!canVisualizeGrid || cells == null) { 
            return;
        }
        foreach (KeyValuePair<Vector2, Cell> keyValuePair in cells)
        {
            if (!keyValuePair.Value.isObstacle)
            {
                Gizmos.color = Color.white;
            }
            else {
                Gizmos.color = Color.black;
            }

            if (finalPath.Contains(keyValuePair.Key)) {
                Gizmos.color = Color.magenta;
            }

            Gizmos.DrawCube(keyValuePair.Key + (Vector2)transform.position, new Vector3(cellWidth, cellHeight));


            //to undrestand where are the grids
            Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, 2f);
            foreach (Vector2 gus in obstaclePositionsOnTilemap)
            {
                Gizmos.DrawWireSphere(transform.position, 2f);
            }
        }
    }

    private void Awake()
    {
        GenerateGrid();
    }

    private void GetObstaclePositionsOnTilemap()
    {
        //print("tilemap.cellBounds.xMin" + tilemap.cellBounds.xMin);
        //print("tilemap.cellBounds.xMax" + tilemap.cellBounds.xMax);
        //print("tilemap.cellBounds.yMin" + tilemap.cellBounds.yMin);
        //print("tilemap.cellBounds.yMax" + tilemap.cellBounds.yMax);
        print("size X" + tilemap.size.x);
        print("size Y" + tilemap.size.y);
        print("tilemap.cellBounds.size.x" + tilemap.cellBounds.size.x);
        print("tilemap.cellBounds.size.y" + tilemap.localBounds.size.y);

        //for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++) {
        //    for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)

        //for (int x = 0; x < tilemap.cellBounds.xMax; x++) {
        //    for (int y = 0; y < tilemap.cellBounds.yMax; y++)

        //for (int x = 0; x < tilemap.cellBounds.size.x; x++) {
        //    for (int y = 0; y < tilemap.cellBounds.size.y; y++)

        // manually indicate what are the bounds because prefab tile does'nt seems to be detected by cellBounds.size
        //for (int x = 0; x < gridWidth - 1; x++) {
        //    for (int y = 0; y < gridHeight - 1; y++)
        for (int x = 0; x < tilemapBoundX; x++) {
            for (int y = 0; y < tilemapBoundY; y++)
            {
                Vector3Int localPos = new Vector3Int(x, y, (int)tilemap.transform.position.y);

                print("tilemapBoundX: " + tilemapBoundX);
                print("tilemapBoundY: " + tilemapBoundY);
                print("gridWidth: " + gridWidth);
                print("gridHeight: " + gridHeight);


                // if prefab tile isn't visible by tilemap use classic tiles to position obstacles
                // then instantiating obstacles at these positions

                // when obstacle destroy, before get position to add null tile to erase tile from the
                // classic tilemap (with just tiles to position prefabs)

                // then call again the function to decide who's cells are obstacles or not,
                // based on the classic tilemap


                if (tilemap.HasTile(localPos))
                {
                    obstaclePositionsOnTilemap.Add((Vector2Int)localPos);

                    // only instantiate wall prefabs once when first time load level
                    // load every obstacles at the start and deactivate them until reach their levels ?
                    if (!areObstaclesInstantiated) {
                        Instantiate(wallPrefab, localPos, Quaternion.identity);
                    }
                }
            }
        }
        areObstaclesInstantiated = true;
    }

    private void Update()
    {
        if (canGeneratePath && !isPathGenerated)
        {
            //GenerateGrid(); 
                            // if everything correct don't need to call it anymore than when load the level,
                            // just RemoveObstacleTile() is enough to update the tilemap and the pathfinding grid
            FindPath(startCell, endCell); // to test
            isPathGenerated = true;
        }
        else if (!canGeneratePath)
        {
            isPathGenerated = false;
        }
    }

    private void GenerateGrid()
    {
        cells = new Dictionary<Vector2, Cell>();
        for (float x = 0; x < gridWidth; x += cellWidth) { // use int instead if float is not necessary
            for (float y = 0; y < gridHeight; y += cellHeight) // use int instead if float is not necessary
            {
                Vector2 pos = new Vector2(x, y);
                //comment because only vector2
                //cells.Add(pos, new Cell(pos));
            }
        }

        //for (int i = 0; i < 40; i++)
        //{
        //    Vector2 pos = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
        //    cells[pos].isObstacle = true;
        //}

        GetObstaclePositionsOnTilemap();
        for (int i = 0; i < obstaclePositionsOnTilemap.Count; i++)
        {
            Vector2Int pos = new Vector2Int(obstaclePositionsOnTilemap[i].x, obstaclePositionsOnTilemap[i].y);
            cells[pos].isObstacle = true;
        }
        obstaclePositionsOnTilemap.Clear();
    }

    private void RemoveObstacleTile(Vector2Int obstacleRemovedPos)
    {
        tilemap.SetTile((Vector3Int)obstacleRemovedPos, null);
        cells[obstacleRemovedPos].isObstacle = false;
    }

    private void FindPath(Vector2 startPos, Vector2 endPos)
    {
        cellsAlreadySearched = new List<Vector2>();
        cellsToSearch = new List<Vector2> {startPos};
        finalPath = new List<Vector2>();

        Cell startCell = cells[startPos];
        startCell.gCost = 0;
        startCell.hCost = GetDistance(startPos, endPos);
        startCell.fCost = GetDistance(startPos, endPos);

        while (cellsToSearch.Count > 0)
        {
            Vector2 cellToSearch = cellsToSearch[0];

            foreach (Vector2 pos in cellsToSearch)
            {
                Cell c = cells[pos];

                if (c.fCost < cells[cellToSearch].fCost ||
                    c.fCost == cells[cellToSearch].fCost && c.hCost < cells[cellToSearch].hCost)
                {
                    cellToSearch = pos;
                }
            }
            cellsToSearch.Remove(cellToSearch);
            cellsAlreadySearched.Add(cellToSearch);


            // after finding the final cell find the correct path by redoing the path backwards, thanks to the connections
            if (cellToSearch == endPos)
            {
                Cell pathCell = cells[endPos];

                while (pathCell.position != startPos)
                {
                    finalPath.Add(pathCell.position);
                    pathCell = cells[pathCell.connection];
                }

                finalPath.Add(startPos);
                return;
            }

            SearchCellNeighbors(cellToSearch, endPos);
        }
    }

    private void SearchCellNeighbors(Vector2 cellPos, Vector2 endPos)
    {
        //for (float x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth)
        //    for (float y = cellPos.y - cellHeight; y <= cellHeight + cellPos.y; y += cellHeight)
        float x = 0;
        float y = 0;

        for (int checkNeighborCount = 0; checkNeighborCount < 4; checkNeighborCount++)
        {
            if (checkNeighborCount == 0) {
                x = cellPos.x;
                y = cellPos.y + cellHeight;
            }
            if (checkNeighborCount == 1) {
                x = cellPos.x - cellWidth;
                y = cellPos.y;
            }
            if (checkNeighborCount == 2) {
                x = cellPos.x + cellWidth;
                y = cellPos.y;
            }
            if (checkNeighborCount == 3) {
                x = cellPos.x;
                y = cellPos.y - cellHeight;
            }
            Vector2 neighborPos = new Vector2(x, y);
            if (cells.TryGetValue(neighborPos, out Cell c) && !cellsAlreadySearched.Contains(neighborPos) && !cells[neighborPos].isObstacle)
            {
                //int gCostToNeighbor = cells[cellPos].gCost + GetDistance(cellPos, neighborPos);
                float gCostToNeighbor = cells[cellPos].gCost + Vector2.Distance(cellPos, neighborPos);

                if (gCostToNeighbor < cells[neighborPos].gCost)
                {
                    Cell neighborNode = cells[neighborPos];

                    // commment because only vector2
                    //neighborNode.connection = cellPos;
                    neighborNode.gCost = gCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborPos, endPos);
                    neighborNode.fCost = neighborNode.gCost + neighborNode.hCost;

                    if (!cellsToSearch.Contains(neighborPos)) {
                        cellsToSearch.Add(neighborPos);
                    }
                }
            }
        }
    }

    private int GetDistance(Vector2 pos1, Vector2 pos2)
    {
        Vector2Int distance = new Vector2Int(Mathf.Abs((int)pos1.x - (int)pos2.x), Mathf.Abs((int)pos1.y - (int)pos2.y));

        int lowest = Mathf.Min(distance.x, distance.y);
        int highest = Mathf.Max(distance.x, distance.y);

        int horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }
}
