using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour
{
    public List<Vector2Int> pathToFollow = new List<Vector2Int>();
    public int index = 0;
    [SerializeField] private float movementPerCellDuration = 0.5f;
    [SerializeField] private float movementSpeed = 2f;
    //private bool isMoving = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        foreach (Vector2 pos in pathToFollow) {
            Gizmos.DrawWireSphere(pos, .5f);
        }
    }

    private void Awake()
    {
        //pathToFollow = null;
    }

    private void Update()
    {
        if (pathToFollow.Count > 0)
        {
            print("pathToFollow.Count: " + pathToFollow.Count);
            Vector3Int targetPos = (Vector3Int)pathToFollow[0];
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                Vector3 moveDirection = (targetPos - transform.position).normalized;
                transform.position += moveDirection * movementSpeed * Time.deltaTime;
            }
            else {
                pathToFollow.RemoveAt(0);
            }
        }

        //if (!isMoving) {
        //    StartCoroutine(FollowPath(pathToFollow));
        //}
    }

    //public void Move(List<Vector2> pathToFollow)
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(FollowPath(pathToFollow));
    //}

    IEnumerator FollowPath(List<Vector2> pathToFollow)
    {
        float time = 0f;

        //List<Vector2> positionsToReach = new List<Vector2>();

        if (pathToFollow == null) {
            yield return null;
        }

        //int i = pathToFollow.Count - 1;
        int i = 0;
        while (i < pathToFollow.Count - 1)
        {
            time = 0f;
            while (time < movementPerCellDuration)
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, pathToFollow[i], time / movementPerCellDuration);
                print("pathToFollow.Count: " + (pathToFollow.Count - 1));
                print("pathToFollow[i]: " + pathToFollow[i]);
                i++;
                //yield return new WaitForSeconds(0.5f);
            }
        }
    }


    //IEnumerator FollowPath(List<Vector2> pathToFollow)
    //{
    //    float time = 0f;

    //    //List<Vector2> positionsToReach = new List<Vector2>();

    //    if (pathToFollow == null) {
    //        yield return null;
    //    }
    //    // doesn't work for now
    //    //for (int i = pathToFollow.Count - 1; i > 0; i--)
    //    //{
    //    //    time = 0f;
    //    //    while (time < movementPerCellDuration)
    //    //    {
    //    //        time += Time.deltaTime;
    //    //        transform.position = pathToFollow[i];
    //    //        print("pathToFollow[i]: " + pathToFollow[i]);
    //    //        //yield return new WaitForSeconds(0.5f);
    //    //    }
    //    //}

    //    //int i = pathToFollow.Count - 1;
    //    int i = 0;
    //    while (i < pathToFollow.Count - 1)
    //    {
    //        time = 0f;
    //        while (time < movementPerCellDuration)
    //        {
    //            time += Time.deltaTime;
    //            transform.position = pathToFollow[i];
    //            print("pathToFollow.Count: " + (pathToFollow.Count - 1));
    //            print("pathToFollow[i]: " + pathToFollow[i]);
    //            i++;
    //            //yield return new WaitForSeconds(0.5f);
    //        }
    //    }
    //}
}
