using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static float PerlinNoise(float x, float y, float smoothness, float scale, float octaves, float persistence, float lacunarity) {
        float total = 0;
        for (int i = 1; i <= octaves; i++) {
            float smooth = smoothness * Mathf.Pow(lacunarity, i);
            float amplitude = scale * Mathf.Pow(persistence, i);
            total += Mathf.PerlinNoise(x / smooth, y / smooth) * amplitude;
        }
        return total;
    }
}
