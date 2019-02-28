// File: Extensions.cs
// Author: Brendan Robinson
// Date Created: 02/22/2019
// Date Last Modified: 02/28/2019

using UnityEngine;

public static class Utilities {

    #region Methods

    public static float PerlinNoise(float x, float y, float smoothness, float scale, float octaves, float persistence,
        float lacunarity) {
        float total = 0;
        for (int i = 1; i <= octaves; i++) {
            float smooth = smoothness * Mathf.Pow(lacunarity, i);
            float amplitude = scale * Mathf.Pow(persistence, i);
            total += Mathf.PerlinNoise(x / smooth, y / smooth) * amplitude;
        }
        return total;
    }

    #endregion

}