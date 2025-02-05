using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexChild : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    private Renderer rendererobj;
    private static readonly int ColorProperty = Shader.PropertyToID("_Color"); // Cache shader property


    private void Awake()
    {
        rendererobj = GetComponent<Renderer>();
    }

    public void SetHexChild(int towerValue)
    {
        if (textUI != null)
        {
            textUI.text = towerValue.ToString();
        }
    }

    public void SetHexColor(Color parentColor)
    {
        if (rendererobj != null)
        {
            rendererobj.material.color = parentColor;
        }
    }

    internal void SetHexDecreaseColor(Color parentColor)
    {
        if (rendererobj != null)
        {
            rendererobj.sharedMaterial.SetColor(ColorProperty, parentColor);
        }
    }
}
