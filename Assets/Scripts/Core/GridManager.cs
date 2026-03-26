using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public Vector2Int GridSize = new Vector2Int(100, 100);
    public float TerrainNoiseScale = 0.05f;
    public float FeatureNoiseScale = 0.1f;
    public float CoralSizeNoiseScale = 0.07f;
    public float CurrentNoiseScale = 0.2f;
    private int seed;

    public Settings settings;

    [SerializeField] GameObject floorTilePrefab;
    [SerializeField] GameObject coralTilePrefab;

    public Dictionary<Vector2Int, Tile> Tiles { get; private set; } = new Dictionary<Vector2Int, Tile>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

        GenerateSeed();
        GenerateGrid();
    }

    private void GenerateSeed()
    {
        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                Tile tile = DecideAndCreateTile(position);
                Tiles[position] = tile;
            }
        }
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x, gridPos.y);
    }

    private Tile DecideAndCreateTile(Vector2Int position)
    {
        float featureNoise = GetSeededNoise(position, FeatureNoiseScale);
        float height = GetSeededNoise(position, TerrainNoiseScale);
        float slope = GetSlope(position);
        float current = GetSeededNoise(position, CurrentNoiseScale);

        float depthScore = 1f - Mathf.Abs(height - Settings.Instance.idealHeight);
        float slopeScore = 1f - Mathf.Abs(slope - Settings.Instance.idealSlope);
        float currentScore = 1f - Mathf.Abs(current - Settings.Instance.idealCurrent);

        float score = depthScore * Settings.Instance.depthWeight +
                      slopeScore * Settings.Instance.slopeWeight +
                      currentScore * Settings.Instance.currentWeight +
                      featureNoise * Settings.Instance.randomWeight;

        if (score > Settings.Instance.spawnThreshold)
        {
            GameObject coralTileGameObject = Instantiate(coralTilePrefab);
            CoralTile coralTile = coralTileGameObject.GetComponent<CoralTile>();

            coralTile.Init(position, height, slope, current,
                GetSeededNoise(position, CoralSizeNoiseScale)
            );

            return coralTile;
        }

        GameObject gameObj = Instantiate(floorTilePrefab);
        FloorTile floorTile = gameObj.GetComponent<FloorTile>();

        floorTile.Init(position, height, slope, current);

        return floorTile;
    }

    private float GetSeededNoise(Vector2Int position, float scale)
    {
        float offset = 10000 * Mathf.PerlinNoise1D(scale);
        return Mathf.PerlinNoise(
            (position.x + seed + offset) * scale,
            (position.y + seed + offset) * scale
        );
    }

    private float GetSlope(Vector2Int position)
    {
        float height = GetSeededNoise(position, TerrainNoiseScale);
        float heightRight = GetSeededNoise(position + Vector2Int.right, TerrainNoiseScale);
        float heightUp = GetSeededNoise(position + Vector2Int.up, TerrainNoiseScale);
        float heightLeft = GetSeededNoise(position + Vector2Int.left, TerrainNoiseScale);
        float heightDown = GetSeededNoise(position + Vector2Int.down, TerrainNoiseScale);

        return Mathf.Max(
            Mathf.Abs(height - heightRight),
            Mathf.Abs(height - heightUp),
            Mathf.Abs(height - heightLeft),
            Mathf.Abs(height - heightDown)
        );
    }
}
