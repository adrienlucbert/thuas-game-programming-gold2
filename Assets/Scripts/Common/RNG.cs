using UnityEngine;

public class RNG
{
    private System.Random _rng;

    public RNG(int seed = 0)
    {
        this._rng = new System.Random(seed);
    }

    public static int GenerateSeed()
    {
        return System.Environment.TickCount;
    }

    public int RandRange(int min, int max)
    {
        return this._rng.Next(min, max);
    }
    public float RandRange(float min, float max)
    {
        return this.RandRange((int)(min * 1000f), (int)(max * 1000f)) / 1000f;
    }
    public int RandRange(Vector2Int range)
    {
        return this.RandRange(range[0], range[1]);
    }
    public float RandRange(Vector2 range)
    {
        return this.RandRange((int)(range[0] * 1000f), (int)(range[1] * 1000f)) / 1000f;
    }
}
