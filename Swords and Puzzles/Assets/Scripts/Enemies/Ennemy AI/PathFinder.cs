using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private GridManager gridRef;

    [SerializeField] private List<Vector2> cellsToSearch;
    [SerializeField] private List<Vector2> cellsAlreadySearched;
    [SerializeField] public List<Vector2> finalPath;
    //[SerializeField] public List<Vector2> finalPath = new List<Vector2>();

    [SerializeField] private Transform playerTransform;
    private Vector2 startCell = new Vector2(0, 1);
    private Vector2 endCell = new Vector2(8, 7);

    [SerializeField] private EnnemyMovement ennemyMovement;


    public void StartFindPath()
    {
        startCell = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        endCell = new Vector2(Mathf.Round(playerTransform.position.x), Mathf.Round(playerTransform.position.y));

        print("gus2");
        FindPath(startCell, endCell);
        //ennemyMovement.Move(finalPath);
        //ennemyMovement.pathToFollow = finalPath;
    }

    public void FindPath(Vector2 startPos, Vector2 endPos)
    {
        print("gus3");

        cellsAlreadySearched = new List<Vector2>();
        cellsToSearch = new List<Vector2> {startPos}; // are list created over and over each time FindPath() is called ?
        finalPath = new List<Vector2>();

        Cell startCell = gridRef.cells[startPos];
        startCell.gCost = 0;
        startCell.hCost = GetDistance(startPos, endPos);
        startCell.fCost = GetDistance(startPos, endPos);

        while (cellsToSearch.Count > 0)
        {
            Vector2 cellToSearch = cellsToSearch[0];

            foreach (Vector2 pos in cellsToSearch)
            {
                Cell c = gridRef.cells[pos];

                if (c.fCost < gridRef.cells[cellToSearch].fCost ||
                    c.fCost == gridRef.cells[cellToSearch].fCost && c.hCost < gridRef.cells[cellToSearch].hCost)
                {
                    cellToSearch = pos;
                }
            }
            cellsToSearch.Remove(cellToSearch);
            cellsAlreadySearched.Add(cellToSearch);


            // after finding the final cell find the correct path by redoing the path backwards, thanks to the connections
            if (cellToSearch == endPos)
            {
                Cell pathCell = gridRef.cells[endPos];

                while (pathCell.position != startPos)
                {
                    finalPath.Add(pathCell.position);
                    pathCell = gridRef.cells[pathCell.connection];
                }

                finalPath.Add(startPos);


                //new
                //ennemyMovement.Move(finalPath);
                ennemyMovement.pathToFollow = finalPath;
                ennemyMovement.index = 0;


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
                y = cellPos.y + gridRef.cellHeight;
            }
            if (checkNeighborCount == 1) {
                x = cellPos.x - gridRef.cellWidth;
                y = cellPos.y;
            }
            if (checkNeighborCount == 2) {
                x = cellPos.x + gridRef.cellWidth;
                y = cellPos.y;
            }
            if (checkNeighborCount == 3) {
                x = cellPos.x;
                y = cellPos.y - gridRef.cellHeight;
            }
            Vector2 neighborPos = new Vector2(x, y);
            if (gridRef.cells.TryGetValue(neighborPos, out Cell c) && !cellsAlreadySearched.Contains(neighborPos) &&
                !gridRef.cells[neighborPos].isObstacle)
            {
                int gCostToNeighbor = gridRef.cells[cellPos].gCost + GetDistance(cellPos, neighborPos);

                if (gCostToNeighbor < gridRef.cells[neighborPos].gCost)
                {
                    Cell neighborNode = gridRef.cells[neighborPos];

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

    private int GetDistance(Vector2 pos1, Vector2 pos2)
    {
        Vector2Int distance = new Vector2Int(Mathf.Abs((int)pos1.x - (int)pos2.x), Mathf.Abs((int)pos1.y - (int)pos2.y));

        int lowest = Mathf.Min(distance.x, distance.y);
        int highest = Mathf.Max(distance.x, distance.y);

        int horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }
}
