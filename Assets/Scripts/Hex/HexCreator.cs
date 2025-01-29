using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HexCreator", menuName = "Hex/Hex Creator")]
public class HexCreator : ScriptableObject
{
    [System.Serializable]
    public class HexParentData
    {
        public GameObject hexParentPrefab; // Assign a prefab for each HexParent
        public int towerValue;
        public float yInterval;
        public GameObject hexChildPrefab;
    }

    public List<HexParentData> hexParents = new List<HexParentData>();
}