using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerClass : MonoBehaviour
{
    public Item item1;
    public Item item2;
    public Item item3;
    public Inventory inventory;

    private void Start()
    {
        inventory.Add(item1, Vector2Int.zero);
        inventory.Add(item2, Vector2Int.right * 2);
        inventory.Add(item3, Vector2Int.right * 4);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerClass))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Open")) {
            (target as PlayerClass).inventory.Open();
        }
    }
}
#endif