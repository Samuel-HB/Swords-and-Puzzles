using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Dictionary<Vector2, Cell> cells;

    [SerializeField] private List<Vector2> cellsToSearch;
    [SerializeField] private List<Vector2> cellsAlreadySearched;
    [SerializeField] private List<Vector2> finalPath;

    [SerializeField] private int gridHeight = 10; // put in awake tilemap bounds instead
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private float cellHeight = 1f; // change to int if float not necessary
    [SerializeField] private float cellWidth = 1f; // change to int if float not necessary

    [SerializeField] private bool canGeneratePath = false;
    private bool isPathGenerated = false;

    [SerializeField] private bool canVisualizeGrid = false;

    //new
    [SerializeField] private Tilemap tilemap;
    private List<Vector2> obstaclePositionsOnTilemap = new List<Vector2>();
    private Vector2 startCell = new Vector2(0, 1);
    private Vector2 endCell = new Vector2(8, 7);

    private int tilemapBoundX = 27;
    private int tilemapBoundY = 48;

    [SerializeField] private GameObject wallPrefab;


    //to undrestand where are the grids
    Vector3Int gusPos = new Vector3Int();


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
        GetObstaclePositionsOnTilemap();
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
        for (int x = 0; x < tilemapBoundX - 1; x++) {
            for (int y = 0; y < tilemapBoundY - 1; y++)
            {
                //Vector3Int localPos = new Vector3Int(x, y, (int)tilemap.transform.localPosition.y);
                Vector3Int localPos = new Vector3Int(x, y, (int)tilemap.transform.position.y);
                gusPos = localPos;
                //Vector3 worldPos = tilemap.CellToWorld(localPos);
                print("has not tile");





                // if prefab tile isn't visible by tilemap use classic tiles to position obstacles
                // then instantiating obstacles at these positions

                // when obstacle destroy, before get position to add null tile to erase tile from the
                // classic tilemap (with just tiles to position prefabs)

                // then call again the function to decide who's cells are obstacles or not,
                // based on the classic tilemap



                //just to test how much it covers the screen
                //obstaclePositionsOnTilemap.Add((Vector2Int)localPos);




                if (tilemap.HasTile(localPos))
                //TileBase tile = tilemap.GetTile(localPos);
                //if (tilemap.ContainsTile(TileBase gus))
                //if (tile != null)
                {
                    print("has tile");
                    //obstaclePositionsOnTilemap.Add(worldPos);

                    //because of testing, comment this line
                    obstaclePositionsOnTilemap.Add((Vector2Int)localPos);

                    //new
                    Instantiate(wallPrefab, localPos, Quaternion.identity);
                }
            }
        }
    }

    private void Start()
    {        
    }

    private void Update()
    {
        if (canGeneratePath && !isPathGenerated)
        {
            GenerateGrid();
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
                cells.Add(pos, new Cell(pos));
            }
        }

        // change this to have obstacles based on coordinates of obstacles on tilemap
        // (coordinated registered on other dictionary)
        //for (int i = 0; i < 40; i++)
        //{
        //    Vector2 pos = new Vector2(Random.Range(0, gridWidth), Random.Range(0, gridHeight));
        //    cells[pos].isObstacle = true;
        //}



        //print("x: " + obstaclePositionsOnTilemap[0].x + "y: " + obstaclePositionsOnTilemap[0].y);
                                                                        // for now nothing is added to the list

        GetObstaclePositionsOnTilemap();

        for (int i = 0; i < obstaclePositionsOnTilemap.Count; i++)
        //for (int i = 0; i < obstaclePositionsOnTilemap.Count - 1; i++) // -1 because perhaps of the 0.5f offset
        {
            //Vector2Int pos = new Vector2Int((int)obstaclePositionsOnTilemap[i].x, (int)obstaclePositionsOnTilemap[i].y);
            Vector2 pos = new Vector2(obstaclePositionsOnTilemap[i].x, obstaclePositionsOnTilemap[i].y);
            print("x: " + obstaclePositionsOnTilemap[i].x + "y: " + obstaclePositionsOnTilemap[i].y);
            cells[pos].isObstacle = true;
        }
        obstaclePositionsOnTilemap.Clear();
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
        for (float x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth) {
            for (float y = cellPos.y - cellHeight; y <= cellHeight + cellPos.y; y += cellHeight)
            {
                Vector2 neighborPos = new Vector2(x, y);
                if (cells.TryGetValue(neighborPos, out Cell c) && !cellsAlreadySearched.Contains(neighborPos) && !cells[neighborPos].isObstacle)
                {
                    int gCostToNeighbor = cells[cellPos].gCost + GetDistance(cellPos, neighborPos);

                    if (gCostToNeighbor < cells[neighborPos].gCost)
                    {
                        Cell neighborNode = cells[neighborPos];

                        neighborNode.connection = cellPos;
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
