using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject, IItem
{
    [field:SerializeField]
    public int StackSize { get; private set; }

    [field: SerializeField]
    public Vector2Int ItemSize { get; private set; }

    [field: SerializeField]
    public Sprite Sprite { get; private set; }
}
