using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private GameData gameData;

    public Transform CurrentFirePoint;
    public float CurrentFireRate;
    


    internal void SetBooster()
    {
        gameData.WeaponBoosterAmount=weaponConfig.BoosterForBulletAmount;
    }

    
}
