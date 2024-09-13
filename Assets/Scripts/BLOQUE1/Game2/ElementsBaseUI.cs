using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class ElementsBaseUI : MonoBehaviour
{
    public GameObject[] prefabsToInstantiate;
    public int numberOfPrefabs = 6;

    // This method will be called by the ScreenManager script
    public void GenerateInitialFruits()
    {
        // Instantiate prefabs and add them as children to the GridLayoutGroup
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            GameObject instantiatedPrefab = Instantiate(prefabsToInstantiate[i], transform);
        }
    }
    //Barajar números para que aparezcan de manera aleatoria
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
}
