using UnityEngine;

public class WallForEveryone : MonoBehaviour, IBreakable
{
    private void WallDestruction()
    {
        print("wall destroyed");
    }

    public void Break()
    {

    }
}
