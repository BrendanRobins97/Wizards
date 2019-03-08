// File: RandomArray.cs
// Author: Brendan Robinson
// Date Created: 01/16/2019
// Date Last Modified: 01/18/2019
// Description: 

using UnityEngine;

public class RandomArray<T> {
    public int Length;

    private readonly T[] items;
    private readonly float[] chances;
    private readonly float totalProbability;

    public RandomArray(T[] items, float[] chances) {
        Length = items.Length;

        this.items = new T[Length];
        this.chances = new float[Length];

        totalProbability = 0;
        for (int i = 0; i < Length; i++) {
            this.items[i] = items[i];
            this.chances[i] = chances[i];
            totalProbability += chances[i];
        }
    }

    public T RandomItem() {
        float randomItem = Random.Range(0, totalProbability);
        float accumulatedProbability = 0;
        for (int i = 0; i < Length; i++) {
            accumulatedProbability += chances[i];
            if (randomItem <= accumulatedProbability) {
                return items[i];
            }
        }

        return items[Length - 1];
    }
}
