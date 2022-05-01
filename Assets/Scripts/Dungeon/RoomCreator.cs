using UnityEngine;

public class RoomCreator : ARoomCreator
{
    public class Room : ARoom
    {
    };

    public GameObject RoomPrefab;

    public override ARoom Create(Vector2 position, Vector2 size)
    {
        GameObject room = Instantiate(this.RoomPrefab, position, Quaternion.identity, this.transform);
        room.transform.localScale = size;
        return new RoomCreator.Room { gameObject = room, position = position, size = size };
    }
}
