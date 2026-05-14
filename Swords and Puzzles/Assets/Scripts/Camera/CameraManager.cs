using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed;

    private Vector3 targetPosition;
    private Vector3 newPosition;

    public Vector3 minPosition;
    public Vector3 maxPosition;


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
}
