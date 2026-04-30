using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Cell> cells;

    [SerializeField] public int gridHeight = 10; // put in awake tilemap bounds instead
    [SerializeField] public int gridWidth = 10;
    [SerializeField] public float cellHeight = 1f;
    [SerializeField] public float cellWidth = 1f;

    [SerializeField] private bool canGeneratePath = false;
    private bool isPathGenerated = false;

    //new
    [SerializeField] private Tilemap tilemap;
    private List<Vector2Int> obstaclePositionsOnTilemap = new List<Vector2Int>();

    private int tilemapBoundX = 48;
    private int tilemapBoundY = 27;

    [SerializeField] private GameObject wallPrefab;
    private bool areObstaclesInstantiated = false;


    private void Awake()
    {
        GenerateGrid();
    }

    private void Update()
    {
        if (canGeneratePath && !isPathGenerated)
        {
            //GenerateGrid(); 
                            // if everything correct don't need to call it anymore than when load the level,
                            // just RemoveObstacleTile() is enough to update the tilemap and the pathfinding grid

            //FindPath(startCell, endCell); // to test
            isPathGenerated = true;
        }
        else if (!canGeneratePath)
        {
            isPathGenerated = false;
        }
    }

    private void GenerateGrid()
    {
        cells = new Dictionary<Vector2Int, Cell>();
        //for (float x = 0; x < gridWidth; x += cellWidth) {
        //    for (float y = 0; y < gridHeight; y += cellHeight)
        for (int x = 0; x < gridWidth; x += 1) {
            for (int y = 0; y < gridHeight; y += 1)
            {
                Vector2Int pos = new Vector2Int(x, y);
                cells.Add(pos, new Cell((Vector2Int)new Vector2Int((int)Mathf.Round(pos.x), (int)Mathf.Round(pos.y))));
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

    private void GetObstaclePositionsOnTilemap()
    {
        //print("tilemap.cellBounds.xMin" + tilemap.cellBounds.xMin);
        //print("tilemap.cellBounds.xMax" + tilemap.cellBounds.xMax);
        //print("tilemap.cellBounds.yMin" + tilemap.cellBounds.yMin);
        //print("tilemap.cellBounds.yMax" + tilemap.cellBounds.yMax);



        //print("size X" + tilemap.size.x);
        //print("size Y" + tilemap.size.y);
        //print("tilemap.cellBounds.size.x" + tilemap.cellBounds.size.x);
        //print("tilemap.cellBounds.size.y" + tilemap.localBounds.size.y);




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


                //print("tilemapBoundX: " + tilemapBoundX);
                //print("tilemapBoundY: " + tilemapBoundY);
                //print("gridWidth: " + gridWidth);
                //print("gridHeight: " + gridHeight);




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

    private void RemoveObstacleTile(Vector2Int obstacleRemovedPos)
    {
        tilemap.SetTile((Vector3Int)obstacleRemovedPos, null);
        cells[obstacleRemovedPos].isObstacle = false;
    }
}
