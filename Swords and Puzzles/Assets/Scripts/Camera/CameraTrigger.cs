using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager cameraManager;

    public Vector3 newCameraPosition;
    public Vector3 newPlayerPosition;

    private BoxCollider2D boxCollider;

    ContactFilter2D contactFilter = new ContactFilter2D();
    private int playerLayerMask = 0;


    void Start()
    {
        cameraManager = Camera.main.GetComponent<CameraManager>();

        boxCollider = GetComponent<BoxCollider2D>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = playerLayerMask;

        Collider2D[] colliders = new Collider2D[1] { null };
        int collidersAmount = Physics2D.OverlapCollider(boxCollider, contactFilter, colliders);

        if (colliders[0] != null )
        {
            EventManager.PlayerInvulnerability();

            cameraManager.minPosition += newCameraPosition;
            cameraManager.maxPosition += newCameraPosition;

            colliders[0].transform.position += newPlayerPosition;
            Array.Clear(colliders, 0, colliders.Length); // is it good ?

            //cameraManager.CallUpdateCameraPositionTimer();
        }
    }
}
