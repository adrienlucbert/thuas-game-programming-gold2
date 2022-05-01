using UnityEngine;

public abstract class ARoomCreator : MonoBehaviour
{
    public abstract ARoom Create(Vector2 position, Vector2 size);
}
