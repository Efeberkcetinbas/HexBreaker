using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexParent : MonoBehaviour
{
    public int towerValue;
    public float yInterval = 0.5f;
    public GameObject hexChildPrefab;
    public Color[] valueColors; // Assign colors in Inspector: blue-green-yellow-orange-red-purple-dark blue
    private Color currentColor;

    internal void SetInit()
    {
        UpdateColor();
        SpawnHexChildren();
    }

    void UpdateColor()
    {
        // Calculate the color index based on towerValue and divide by range size
        float range = 100f / valueColors.Length; // Since we have 7 colors and towerValue is 0-100
        int index = Mathf.FloorToInt(towerValue / range); // Map towerValue to color index

        // Ensure the index stays within valid bounds (0 to 6)
        index = Mathf.Clamp(index, 0, valueColors.Length - 1);
        currentColor = valueColors[index];

        // Optionally log the color
        Debug.Log($"Tower Value: {towerValue}, Color Index: {index}, Color: {currentColor}");
    }

    void SpawnHexChildren()
    {
        int childCount = towerValue / 5;

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = Instantiate(hexChildPrefab, transform);
            child.transform.localPosition = new Vector3(0, i * yInterval, 0);

            HexChild hexChildScript = child.GetComponent<HexChild>();
            if (hexChildScript != null)
            {
                hexChildScript.SetHexChild(currentColor, towerValue);
            }
        }
    }
}
