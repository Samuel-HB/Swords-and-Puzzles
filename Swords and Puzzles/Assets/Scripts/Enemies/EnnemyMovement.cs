using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour
{
    public List<Vector2> pathToFollow = new List<Vector2>();
    public int index = 0;
    [SerializeField] private float movementPerCellDuration = 0.5f;
    [SerializeField] private float movementSpeed = 2f;

    private void Awake()
    {
        pathToFollow = null;
    }

    private void Update()
    {
        if (pathToFollow != null)
        {
            print("pathToFollow[index]: " + pathToFollow[index]);
            print("pathToFollow.Count: " + pathToFollow.Count);
            print("(pathToFollow.Count)-index: " + ((pathToFollow.Count)-index));
            Vector3 targetPos = pathToFollow[index];
            if (Vector2.Distance(transform.position, targetPos) > 1f)
            {
                Vector3 moveDirection = (targetPos - transform.position).normalized;
                transform.position += moveDirection * movementSpeed * Time.deltaTime;
            }
            else {
                index++;
                if (index >= pathToFollow.Count)
                {
                    pathToFollow = null;
                    index = 0;
                }
            }            
        }
    }

    //public void Move(List<Vector2> pathToFollow)
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(FollowPath(pathToFollow));
    //}


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
