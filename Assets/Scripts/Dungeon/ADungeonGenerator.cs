using UnityEngine.Events;
using UnityEngine;

public abstract class ADungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    public ARoomCreator RoomCreator;
    public AHallwayCreator HallwayCreator;
    public UnityEvent OnBeforeGenerate;

    public virtual void Generate()
    {
        this.OnBeforeGenerate?.Invoke();
    }
}
