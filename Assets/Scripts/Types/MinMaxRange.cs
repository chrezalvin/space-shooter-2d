using UnityEngine;

public class MinMaxRange
{
    public float min;
    public float max;

    public MinMaxRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    // get random value between min and max
    public float GetRandom()
    {
        return Random.Range(min, max);
    }

    public int GetRandomInt()
    {
        return Random.Range(Mathf.FloorToInt(min), Mathf.CeilToInt(max));
    }

    public float GetRange()
    {
        return max - min;
    }

    public void Set(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
