using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexParent : MonoBehaviour
{
    public int towerValue;
    public float yInterval = 0.5f;
    public GameObject hexChildPrefab;
    public Color[] valueColors; // Assign colors in Inspector: blue-green-yellow-orange-red-purple-dark blue

    [SerializeField] private GameData gameData;

    private Color currentColor;

    internal void SetInit()
    {
        UpdateColor();
        SpawnHexChildren();
    }

    void UpdateColor()
    {
        if (valueColors.Length < 2) return; // Ensure there are at least two colors

        float range = 100f / (valueColors.Length - 1); // Divide 100 by (color count - 1)
        float normalizedValue = towerValue / range; // Get the floating point index

        int lowerIndex = Mathf.FloorToInt(normalizedValue); // Get lower bound index
        int upperIndex = Mathf.Clamp(lowerIndex + 1, 0, valueColors.Length - 1); // Get upper bound index

        float lerpFactor = normalizedValue - lowerIndex; // Get interpolation factor between two colors
        currentColor = Color.Lerp(valueColors[lowerIndex], valueColors[upperIndex], lerpFactor);

        // Apply color to HexParent
        //GetComponent<Renderer>().material.color = currentColor;

        //Debug.Log($"Tower Value: {towerValue}, Color Index: {lowerIndex}-{upperIndex}, Lerp: {lerpFactor}, Color: {currentColor}");
    }

    public void DecreaseTowerValue()
    {
        towerValue = Mathf.Max(0, towerValue - gameData.BulletDamageAmount); // Reduce by 1 per bullet

        UpdateHexChildren();

        if (towerValue == 0)
        {
            EventManager.Broadcast(GameEvent.OnHexDestroyed);
            Destroy(gameObject);
        }
        else
        {
            UpdateColor();
            UpdateHexChildText();
        }
    }

    void UpdateHexChildren()
    {
        int expectedChildCount = Mathf.Max(1, towerValue / 5); // Ensure at least 1 remains
        int currentChildCount = transform.childCount;

        // Remove excess HexChildren if needed
        while (currentChildCount > expectedChildCount)
        {
            Destroy(transform.GetChild(currentChildCount - 1).gameObject);
            currentChildCount--;
        }

        // Reposition remaining HexChildren
        for (int i = 0; i < currentChildCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(0, i * yInterval, 0);
        }
    }

    void UpdateHexChildText()
    {
        foreach (HexChild child in GetComponentsInChildren<HexChild>())
        {
            child.SetHexChild(currentColor, towerValue);
        }
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
