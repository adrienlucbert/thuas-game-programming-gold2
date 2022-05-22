using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : ADungeonGenerator
{
    public GameObject DungeonWallPrefab;
    [SerializeField] private DungeonGeneratorViewModel _vm;
    /*   public Vector2 DungeonSize = new Vector2(100, 100);
       public Vector2 RoomWidthRange = new Vector2(1, 10);
       public Vector2 RoomHeightRange = new Vector2(1, 10);
       public Vector2Int RoomCountRange = new Vector2Int(1, 10);
       public Vector2 HallwayLengthRange = new Vector2(1, 10);

       public bool RegenerateSeed = true;
       public int RandomSeed = 0;
    */
    private RNG _rng;

    private enum Direction
    {
        North,
        South,
        West,
        East,
        COUNT
    }
    private readonly Dictionary<Direction, Vector2> DirectionVectors = new Dictionary<Direction, Vector2>{
        { Direction.North, Vector2.up },
        { Direction.South, Vector2.down },
        { Direction.West, Vector2.left },
        { Direction.East, Vector2.right }
    };

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(new Vector3(0f, 0f, 1f), new Vector3(this._vm.DungeonSize[0], this._vm.DungeonSize[1], 0f));
    }

    private void Awake()
    {
        // Default values
        this._vm.DungeonSize = new Vector2(50, 50);
        this._vm.RoomWidthRange = new Vector2(4, 10);
        this._vm.RoomHeightRange = new Vector2(2.5f, 10);
        this._vm.RoomCountRange = new Vector2Int(5, 10);
        this._vm.HallwayLengthRange = new Vector2(5, 10);
        this._vm.RandomizeSeed = true;
        this._vm.RandomSeed = 382042875;
    }

    private void Init()
    {
        this.Clear();
        if (this._vm.RandomizeSeed)
            this._vm.RandomSeed = RNG.GenerateSeed();
        this._rng = new RNG(this._vm.RandomSeed);
        // Generate dungeon invisible walls
        GameObject topWall = Instantiate(this.DungeonWallPrefab, new Vector2(0f, -(this._vm.DungeonSize.y + 1f) / 2f), Quaternion.identity, this.transform);
        topWall.transform.localScale = new Vector3(this._vm.DungeonSize.x, -1f, 1f);
        GameObject rightWall = Instantiate(this.DungeonWallPrefab, new Vector2((this._vm.DungeonSize.x + 1f) / 2f, 0f), Quaternion.identity, this.transform);
        rightWall.transform.localScale = new Vector3(1f, this._vm.DungeonSize.y, 1f);
        GameObject bottomWall = Instantiate(this.DungeonWallPrefab, new Vector2(0f, (this._vm.DungeonSize.y + 1f) / 2f), Quaternion.identity, this.transform);
        bottomWall.transform.localScale = new Vector3(this._vm.DungeonSize.x, 1f, 1f);
        GameObject leftWall = Instantiate(this.DungeonWallPrefab, new Vector2(-(this._vm.DungeonSize.x + 1f) / 2f, 0f), Quaternion.identity, this.transform);
        leftWall.transform.localScale = new Vector3(-1f, this._vm.DungeonSize.y, 1f);
    }

    private void Clear()
    {
        for (var i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }
    }

    public void FitCamera()
    {
        Camera.main.GetComponent<CameraScaler>()?.Fit(
            this._vm.DungeonSize.x,
            this._vm.DungeonSize.y
        );
    }

    private bool TryCreateNextRoomInDirection(ARoom from, out ARoom to, out AHallway hallway, Direction direction)
    {
        to = null;
        hallway = null;
        Vector2 directionVector = this.DirectionVectors[direction];
        Vector2 roomEdge = new Vector2(
            from.position.x + (from.size.x * directionVector.x) / 2f,
            from.position.y + (from.size.y * directionVector.y) / 2f
        );
        // Determine the nearest dungeon wall in the provided direction and the distance to it
        RaycastHit2D hit = Physics2D.Raycast(roomEdge, directionVector, (directionVector * this._vm.DungeonSize).magnitude, LayerMask.GetMask("Wall"));
        if (hit.collider == null)
            return false;
        Vector2 minRoomSize = new Vector2(this._vm.RoomWidthRange[0], this._vm.RoomHeightRange[0]);
        float minRoomSizeInDirection = (minRoomSize * directionVector).magnitude;
        Vector2 clampedHallwayLengthRange = new Vector2(
            this._vm.HallwayLengthRange[0],
            Mathf.Min(this._vm.HallwayLengthRange[1], hit.distance - minRoomSizeInDirection)
        );
        // If the minimum-sized room cannot fit in the space between the minimum-sized hallway end and the wall, can't create the room
        if (clampedHallwayLengthRange[0] > clampedHallwayLengthRange[1])
            return false;
        float hallwayLength = this._rng.RandRange(clampedHallwayLengthRange);
        Vector2 clampedRoomWidthRange = this._vm.RoomWidthRange;
        if (directionVector.x != 0f)
            clampedRoomWidthRange = new Vector2(
                this._vm.RoomWidthRange[0],
                Mathf.Min(this._vm.RoomWidthRange[1], hit.distance - (Vector2.right * directionVector * hallwayLength).magnitude)
            );
        Vector2 clampedRoomHeightRange = this._vm.RoomHeightRange;
        if (directionVector.y != 0f)
            clampedRoomHeightRange = new Vector2(
                this._vm.RoomHeightRange[0],
                Mathf.Min(this._vm.RoomHeightRange[1], hit.distance - (Vector2.up * directionVector * hallwayLength).magnitude)
            );
        float w = this._rng.RandRange(clampedRoomWidthRange);
        float h = this._rng.RandRange(clampedRoomHeightRange);
        Vector2 size = new Vector2(w, h);
        Vector2 position = roomEdge + directionVector * hallwayLength + directionVector * size * 0.5f;
        to = this.RoomCreator.Create(position, size);
        hallway = this.HallwayCreator.Create(from, roomEdge, to, roomEdge + directionVector * hallwayLength);
        return true;
    }

    private bool TryCreateNextRoom(ARoom prevRoom, out ARoom nextRoom, out AHallway hallway)
    {
        // Generate a random direction for the next room
        Direction randomDirection = (Direction)this._rng.RandRange(0, (int)Direction.COUNT - 1);
        Direction direction = randomDirection;
        do
        {
            // Check if a room can be generated in the chosen direction
            if (this.TryCreateNextRoomInDirection(prevRoom, out nextRoom, out hallway, direction))
                return true;
            // If not, cycle through all directions until one is correct
            direction = (Direction)(((int)direction + 1) % (int)Direction.COUNT);
        } while (direction != randomDirection);
        // If no room could be generated in any direction, it is impossible to generate a room
        return false;
    }

    private ARoom CreateNextRoom(ARoom prevRoom = null)
    {
        if (prevRoom == null)
        {
            float w = this._rng.RandRange(this._vm.RoomWidthRange);
            float h = this._rng.RandRange(this._vm.RoomHeightRange);
            return this.RoomCreator.Create(Vector2.zero, new Vector2(w, h));
        }
        else if (this.TryCreateNextRoom(prevRoom, out ARoom nextRoom, out AHallway hallway))
            return nextRoom;
        throw new System.Exception("Could not generate a room.");
    }

    public IEnumerator GenerateRooms()
    {
        yield return new WaitForFixedUpdate();
        int roomsCount = this._rng.RandRange(this._vm.RoomCountRange);
        ARoom room = null;
        for (int i = 0; i < roomsCount; ++i)
        {
            room = this.CreateNextRoom(room);
        }
        yield return null;
    }

    public override void Generate()
    {
        base.Generate();
        this.Init();
        this.StartCoroutine(this.GenerateRooms());
    }
}