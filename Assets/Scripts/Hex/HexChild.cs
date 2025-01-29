using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexChild : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public void SetHexChild(Color parentColor, int towerValue)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = parentColor;
        }

        if (textUI != null)
        {
            textUI.text = towerValue.ToString();
        }
    }
}
