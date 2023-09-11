using System.Collections.Generic;
using UnityEngine;

public class ListRandomizer : MonoBehaviour
{
    public static List<T> RandomizeList<T>(List<T> inputList)
    {
        List<T> randomList = new List<T>(inputList);
        int n = randomList.Count;

        // Fisher-Yates shuffle algorithm
        for (int i = 0; i < n; i++)
        {
            int randIndex = Random.Range(i, n);
            T temp = randomList[i];
            randomList[i] = randomList[randIndex];
            randomList[randIndex] = temp;
        }

        return randomList;
    }
}