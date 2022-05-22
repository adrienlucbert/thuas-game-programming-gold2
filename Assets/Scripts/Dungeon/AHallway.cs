using UnityEngine;

public abstract class AHallway
{
    public GameObject gameObject;
    public ARoom from;
    public Vector2 fromPosition;
    public ARoom to;
    public Vector2 toPosition;
}
