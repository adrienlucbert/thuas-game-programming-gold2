using UnityEngine;

public abstract class AHallwayCreator : MonoBehaviour
{
    public abstract AHallway Create(ARoom from, Vector2 fromPosition, ARoom to, Vector2 toPosition);
}
