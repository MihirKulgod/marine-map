using UnityEngine;

public class CoralTile : Tile
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject coral;

    private float size = 0f;

    public void Init(Vector2Int position, float height, float slope, float current, float size)
    {
        this.size = size;
        base.Init(position, height, slope, current);
    }

    protected override void UpdateColour()
    {
        background.GetComponent<SpriteRenderer>().color = Color.Lerp(
            Settings.Instance.ShadowColour,
            Settings.Instance.LightColour,
            Height);

        coral.GetComponent<SpriteRenderer>().color = Color.Lerp(
            Settings.Instance.CoralShadowColour,
            Settings.Instance.CoralLightColour,
            Height);

        coral.transform.localScale = 2 * size * new Vector3(1, 1);
    }
}
