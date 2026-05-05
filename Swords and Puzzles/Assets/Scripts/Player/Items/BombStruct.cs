using System;
using UnityEngine.UI;

public struct BombStruct
{
    public Bomb bomb;
    [NonSerialized] public int bombsCount;
    public Image bombImage;
}
