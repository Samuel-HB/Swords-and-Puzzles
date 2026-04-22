using UnityEngine;

public class WallOnlyForPlayer : MonoBehaviour, IBreakable
{
    private void WallDestruction()
    {
        print("wall destroyed");
    }

    public void Break()
    {

    }
}
