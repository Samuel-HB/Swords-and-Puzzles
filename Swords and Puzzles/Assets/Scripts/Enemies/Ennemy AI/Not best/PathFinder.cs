using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private GridManager gridRef;

    //[SerializeField] private List<Vector2> cellsToSearch;
    //[SerializeField] private List<Vector2> cellsAlreadySearched;
    //[SerializeField] public List<Vector2> finalPath = new List<Vector2>();

    [SerializeField] private Transform playerTransform;
    //private Vector2 startCell = new Vector2(0, 1);
    //private Vector2 endCell = new Vector2(8, 7);

    [SerializeField] private EnnemyMovement ennemyMovement;


    public void StartFindPath()
    {
        Vector2Int startCell = new Vector2Int((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
        Vector2Int endCell = new Vector2Int((int)Mathf.Round(playerTransform.position.x), (int)Mathf.Round(playerTransform.position.y));

        FindPath(startCell, endCell);
        //ennemyMovement.Move(finalPath);
        //ennemyMovement.pathToFollow = finalPath;
    }

    public void FindPath(Vector2Int startPos, Vector2Int endPos)//, out List<Vector2> outPath)
    {
        //print("startPos:" + startPos);
        //print("endPos:" + endPos);


        List<Vector2Int> cellsAlreadySearched = new List<Vector2Int>();
        List<Vector2Int> cellsToSearch = new List<Vector2Int> {startPos};
        List<Vector2Int> finalPath = new List<Vector2Int>();

        Cell startCell = gridRef.cells[startPos];
        startCell.gCost = 0;
        //startCell.hCost = GetDistance(startPos, endPos);
        startCell.hCost = Vector2.Distance(startPos, endPos);
        //startCell.fCost = GetDistance(startPos, endPos);
        startCell.fCost = Vector2.Distance(startPos, endPos);

        while (cellsToSearch.Count > 0)
        {
            Vector2Int cellToSearch = cellsToSearch[0];

            foreach (Vector2Int pos in cellsToSearch)
            {
                Cell c = gridRef.cells[pos];

                if (c.fCost < gridRef.cells[cellToSearch].fCost ||
                    c.fCost == gridRef.cells[cellToSearch].fCost && c.hCost < gridRef.cells[cellToSearch].hCost)
                {
                    cellToSearch = pos;
                    print("enter if for each");
                }
                print("enter for each");
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


                //ennemyMovement.Move(finalPath);
                finalPath.Reverse();
                print("finalPath.Count: " + finalPath.Count);
                ennemyMovement.pathToFollow = finalPath;
                //ennemyMovement.index = 0;


                return;
            }

            SearchCellNeighbors(cellToSearch, endPos, cellsToSearch);
        }
    }

    private void SearchCellNeighbors(Vector2Int cellPos, Vector2Int endPos, List<Vector2Int> cellsToSearch)
    {
        //for (float x = cellPos.x - cellWidth; x <= cellWidth + cellPos.x; x += cellWidth)
        //    for (float y = cellPos.y - cellHeight; y <= cellHeight + cellPos.y; y += cellHeight)

        List<Vector2> cellsAlreadySearched = new List<Vector2>();

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

            Vector2Int neighborPos = new Vector2Int((int)Mathf.Round(x), (int)Mathf.Round(y));
            if (gridRef.cells.TryGetValue(neighborPos, out Cell c) && !cellsAlreadySearched.Contains(neighborPos) &&
                !gridRef.cells[neighborPos].isObstacle)
            {
                //int gCostToNeighbor = gridRef.cells[cellPos].gCost + GetDistance(cellPos, neighborPos);
                float gCostToNeighbor = gridRef.cells[cellPos].gCost + Vector2.Distance(cellPos, neighborPos);
                print("gCostNeighbor" + gCostToNeighbor);
                print("cell.gcost" + gridRef.cells[cellPos].gCost);

                if (gCostToNeighbor < gridRef.cells[neighborPos].gCost)
                {
                    print("pas ok");
                    Cell neighborNode = gridRef.cells[neighborPos];

                    neighborNode.connection = cellPos;
                    neighborNode.gCost = gCostToNeighbor;
                    //neighborNode.hCost = GetDistance(neighborPos, endPos);
                    neighborNode.hCost = Vector2.Distance(neighborPos, endPos);
                    neighborNode.fCost = neighborNode.gCost + neighborNode.hCost;

                    if (!cellsToSearch.Contains(neighborPos)) {
                        cellsToSearch.Add(neighborPos);
                    }
                }
            }
        }
    }

    //private int GetDistance(Vector2 neighborPos, Vector2 endPos)
    //{
    //    int toReturn = 0;
    //    return toReturn = (int)Vector2Int.Distance(
    //                                                  new Vector2Int(
    //                                                                Mathf.Abs((int)neighborPos.x),
    //                                                                Mathf.Abs((int)neighborPos.y)
    //                                                                ),
    //                                                  new Vector2Int(
    //                                                                Mathf.Abs((int)endPos.x),
    //                                                                Mathf.Abs((int)endPos.y)
    //                                                                )
    //                                                  );
    //}

    //private int GetDistance(Vector2 pos1, Vector2 pos2)
    //{
    //    Vector2Int distance = new Vector2Int(Mathf.Abs((int)pos1.x - (int)pos2.x), Mathf.Abs((int)pos1.y - (int)pos2.y));

    //    int lowest = Mathf.Min(distance.x, distance.y);
    //    int highest = Mathf.Max(distance.x, distance.y);

    //    int horizontalMovesRequired = highest - lowest;

    //    return lowest * 14 + horizontalMovesRequired * 10;
    //}
}
