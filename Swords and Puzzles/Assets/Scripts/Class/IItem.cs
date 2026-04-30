using UnityEngine;

public interface IItem
{
    int StackSize { get; }
    Vector2Int ItemSize { get; }
    Sprite Sprite { get; }
}
