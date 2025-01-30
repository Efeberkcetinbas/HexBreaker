using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGenerator : MonoBehaviour
{
    [SerializeField] private HexCreator hexCreator; // Assign the ScriptableObject in the Inspector
    [SerializeField] private Transform[] spawnPositions; // Set possible positions in Inspector

    [SerializeField] private GameData gameData;

    private void Start()
    {
        if (hexCreator == null)
        {
            Debug.LogError("HexCreator is not assigned! Assign it in the Inspector.");
            return;
        }

        GenerateHexParents();
    }

    void GenerateHexParents()
    {
        if (spawnPositions.Length == 0)
        {
            Debug.LogError("SpawnPositions not assigned!");
            return;
        }

        List<Transform> availablePositions = new List<Transform>(spawnPositions);

        for (int i = 0; i < hexCreator.hexParents.Count; i++)
        {
            if (availablePositions.Count == 0) break; // Stop if no more positions available

            HexCreator.HexParentData data = hexCreator.hexParents[i];

            if (data.hexParentPrefab == null)
            {
                Debug.LogError($"HexParentPrefab is missing for entry {i}!");
                continue;
            }

            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform spawnPoint = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex); // Remove position to avoid duplicates

            GameObject hexParentObj = Instantiate(data.hexParentPrefab, spawnPoint.position, Quaternion.identity);
            HexParent hexParentScript = hexParentObj.GetComponent<HexParent>();

            if (hexParentScript != null)
            {
                hexParentScript.towerValue = data.towerValue;
                hexParentScript.yInterval = data.yInterval;
                hexParentScript.hexChildPrefab = data.hexChildPrefab;

                hexParentScript.SetInit();
            }
        }

        gameData.LevelHexParentNumber=hexCreator.hexParents.Count;
        
    }


    private void OnRestart()
    {
        GenerateHexParents();
    }
}
