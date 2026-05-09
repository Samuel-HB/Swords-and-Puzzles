using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed;

    private Vector3 targetPosition;
    private Vector3 newPosition;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    //float cameraMovementDuration = 0.5f;
    //float cameraMovementDuration = 2f;


    private void LateUpdate()
    {
        if (transform.position != player.position)
        {
            targetPosition = player.position;

            Vector3 cameraBoundsPositions = new Vector3(
                Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x),
                Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y),
                Mathf.Clamp(targetPosition.z, minPosition.z, maxPosition.z)
                );

            newPosition = Vector3.Lerp(transform.position, cameraBoundsPositions, smoothSpeed);
            transform.position = newPosition;
        }
    }

    //public void CallUpdateCameraPositionTimer()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine(UpdateCameraPositionTimer(cameraMovementDuration));
    //}

    //public IEnumerator UpdateCameraPositionTimer(float duration)
    //{
    //    //if (transform.position != player.position) yield return null;

    //    targetPosition = player.position;

    //    Vector3 cameraBoundsPositions = new Vector3(
    //        Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x),
    //        Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y),
    //        Mathf.Clamp(targetPosition.z, minPosition.z, maxPosition.z)
    //        );

    //    float time = 0;
    //    while (time < duration)
    //    {
    //        time += Time.deltaTime;

    //        //new
    //        //Vector3 difference = new Vector3(Mathf.Abs(transform.position.x - cameraBoundsPositions.x),
    //        //                                 Mathf.Abs(transform.position.y - cameraBoundsPositions.y)
    //        //                                 );
    //        Vector3 difference = transform.position - cameraBoundsPositions;

    //        print("difference.x * (time / duration: " + difference.x * (time / duration));
    //        print("difference.x * 0.1f: " + difference.x * 0.1f);
    //        print("difference.x * 0.1f: " + difference.x * 0.3f);
    //        print("difference.x * 0.1f: " + difference.x * 0.5f);
    //        print("difference.x * 0.1f: " + difference.x * 0.7f);
    //        print("difference.x * 0.1f: " + difference.x * 0.8f);
    //        //newPosition = transform.position + (difference * (time / duration));

    //        //Vector3 gus = transform.position + (cameraBoundsPositions - transform.position) * (time / duration);
    //        //transform.position += gus;


    //        newPosition = (Vector2)transform.position + (new Vector2(difference.x * (time / duration),
    //                                                        difference.y * (time / duration))
    //                                            );



    //        //newPosition = Vector3.Lerp(transform.position, cameraBoundsPositions, smoothSpeed);

    //        transform.position = newPosition;
    //        yield return null;
    //    }        
    //}
}
