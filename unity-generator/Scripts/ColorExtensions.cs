using UnityEngine;

public static class ColorExtensions
{
    public static Color ToGray(this float value)
    {
        return new Color(value, value, value);
    }
}