using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "MarineMap/Settings")]
public class Settings : ScriptableObject
{
    public static Settings Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    [Header("Coral Settings")]
    public float idealHeight = 0.5f;
    public float idealCurrent = 0.5f;
    public float idealSlope = 0f;

    public float depthWeight = 1f;
    public float currentWeight = 1f;
    public float slopeWeight = 1f;
    public float randomWeight = 2f;

    public float spawnThreshold = 3f;

    [Header("Colours")]
    public Color LightColour;
    public Color ShadowColour;

    public Color CoralLightColour;
    public Color CoralShadowColour;
}