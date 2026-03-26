using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public Vector2Int Pos { get; private set; }
    public float Height;
    public float Slope;
    public float Current;

    public virtual void Init(Vector2Int position, float height, float slope, float current)
    {
        Pos = position;
        Height = height;
        Slope = slope;
        Current = current;
        gameObject.transform.position = GridManager.Instance.GridToWorld(position);

        UpdateColour();
    }

    protected abstract void UpdateColour();
}