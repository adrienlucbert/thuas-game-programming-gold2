using UnityEngine;

public class HallwayCreator : AHallwayCreator
{
    public class Hallway : AHallway
    {
    };

    public GameObject HallwayPrefab;

    public override AHallway Create(ARoom from, Vector2 fromPosition, ARoom to, Vector2 toPosition)
    {
        Vector3 position = (fromPosition + toPosition) * 0.5f;
        position.z = 1f;
        GameObject hallway = Instantiate(this.HallwayPrefab, position, Quaternion.identity, this.transform);
        Vector2 hallwayPath = toPosition - fromPosition;
        hallway.transform.localScale = new Vector3(
            Mathf.Max(1f, Mathf.Abs(hallwayPath.x)),
            Mathf.Max(1f, Mathf.Abs(hallwayPath.y)),
            1f
        );
        return new HallwayCreator.Hallway
        {
            gameObject = hallway,
            from = from,
            fromPosition = fromPosition,
            to = to,
            toPosition = toPosition
        };
    }
}
