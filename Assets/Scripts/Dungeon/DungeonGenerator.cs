using UnityEngine;
using UnityEngine.Events;

public class DungeonGenerator : ADungeonGenerator
{
    public Vector2 DungeonSize = new Vector2(100, 100);
    public Vector2 RoomWidthRange = new Vector2(1, 10);
    public Vector2 RoomHeightRange = new Vector2(1, 10);
    public Vector2Int RoomCountRange = new Vector2Int(1, 10);
    public Vector2 HallwayLengthRange = new Vector2(1, 10);

    public bool RegenerateSeed = true;
    public int RandomSeed = 0;
    private System.Random _rng;

    private int RandRange(int min, int max)
    {
        return this._rng.Next(min, max);
    }
    private float RandRange(float min, float max)
    {
        return this.RandRange((int)(min * 1000f), (int)(max * 1000f)) / 1000f;
    }
    private int RandRange(Vector2Int range)
    {
        return this.RandRange(range[0], range[1]);
    }
    private float RandRange(Vector2 range)
    {
        return this.RandRange((int)(range[0] * 1000f), (int)(range[1] * 1000f)) / 1000f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(new Vector3(0f, 0f, 1f), new Vector3(this.DungeonSize[0], this.DungeonSize[1], 0f));
    }

    private void Init()
    {
        this.Clear();
        if (this.RegenerateSeed)
            this.RandomSeed = System.Environment.TickCount;
        this._rng = new System.Random(this.RandomSeed);
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
            this.DungeonSize.x,
            this.DungeonSize.y
        );
    }

    private void CreateNextRoom(ARoom room = null)
    {
    }

    public override void Generate()
    {
        base.Generate();
        this.Init();
        int roomsCount = this.RandRange(this.RoomCountRange);
        for (int i = 0; i < roomsCount; ++i)
        {
            float w = this.RandRange(this.RoomWidthRange);
            float h = this.RandRange(this.RoomHeightRange);
            float x = this.RandRange(-(this.DungeonSize[0] - w) / 2f, (this.DungeonSize[0] - w) / 2f);
            float y = this.RandRange(-(this.DungeonSize[1] - h) / 2f, (this.DungeonSize[1] - h) / 2f);
            ARoom room = this.RoomCreator.Create(new Vector2(x, y), new Vector2(w, h));
        }
    }
}
