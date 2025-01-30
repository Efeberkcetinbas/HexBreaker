using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexParent : MonoBehaviour
{
    public int towerValue;
    public float yInterval = 0.5f;
    public GameObject hexChildPrefab;
    public Color[] valueColors; // Assign colors in Inspector: blue-green-yellow-orange-red-purple-dark blue

    [SerializeField] private GameData gameData;

    private Color currentColor;
    private WaitForSeconds colorDamageWaitforseconds;

    private void Start()
    {
        colorDamageWaitforseconds=new WaitForSeconds(0.05f);
    }


    internal void SetInit()
    {
        UpdateColor();
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

    private IEnumerator SetDecreaseColor()
    {
        HexChild[] children = GetComponentsInChildren<HexChild>(); // Cache results

        foreach (HexChild child in children)
        {
            child.SetHexDecreaseColor(Color.white);
        }

        yield return colorDamageWaitforseconds;

        UpdateColor();
        
        foreach (HexChild child in children)
        {
            child.SetHexColor(currentColor);
        }
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
            //UpdateColor();
            StartCoroutine(SetDecreaseColor());
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
            child.SetHexChild(towerValue);
            //child.SetHexColor(currentColor);
        }
    }

    public IEnumerator SpawnHexChildrenWithEffect()
    {
        int childCount = Mathf.Max(1, towerValue / 5);

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = Instantiate(hexChildPrefab, transform);
            child.transform.localPosition = new Vector3(0, i * yInterval, 0);
            child.transform.localScale = Vector3.zero; // Start with scale 0

            HexChild hexChildScript = child.GetComponent<HexChild>();
            if (hexChildScript != null)
            {
                hexChildScript.SetHexColor(currentColor);
                hexChildScript.SetHexChild(towerValue);
            }

            Debug.Log("HexChild Spawn");

            // DOTween Animation: Scale up and rotate
            child.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            child.transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(0.1f); // Small delay between HexChild spawns
        }
    }

    
    
}
