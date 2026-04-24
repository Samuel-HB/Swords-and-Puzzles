using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Tilemap WallsPositions;

    private void Start()
    {
        if (WallsPositions.TryGetComponent<TilemapRenderer>(out TilemapRenderer tilemapRenderer)) {
            tilemapRenderer.enabled = false;
        }
    }
}
