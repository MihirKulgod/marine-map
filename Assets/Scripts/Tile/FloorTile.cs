using UnityEngine;

public class FloorTile : Tile
{
    private SpriteRenderer _renderer;

    public override void Init(Vector2Int position, float height, float slope, float current)
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("FloorTile requires a SpriteRenderer component.");
            return;
        }

        base.Init(position, height, slope, current);
    }

    protected override void UpdateColour()
    {
        _renderer.color = Color.Lerp(
            Settings.Instance.ShadowColour,
            Settings.Instance.LightColour,
            Height);
    }
}