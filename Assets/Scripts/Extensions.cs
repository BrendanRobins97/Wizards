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

    public static Vector3Int Vector3IntFromVector3(Vector3 vec, bool round = true) {
        if (round) {
            return new Vector3Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
        }
        else {
            return new Vector3Int((int)(vec.x), (int)(vec.y), (int)(vec.z));
        }
    }

    public static T GetComponentOnObject<T>(this GameObject GO) {
        T component;
        component = GO.GetComponent<T>();
        if (component != null) { return component; }
        component = GO.GetComponentInChildren<T>();
        if (component != null) { return component; }
        component = GO.GetComponentInParent<T>();
        if (component != null) { return component; }
        return default(T);
    }

    public static float DistToSphereSurface(float x, float y, float z, float radius) {
        return x * x + y * y + z * z - radius * radius;
    }
    #endregion

}