using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] private GameData gameData;
    [SerializeField] private List<WeaponType> weaponTypes= new List<WeaponType>();

    private Transform firePoint;
    private float fireRate = 0.2f; // Interval between shots



    private void Start()
    {
        OnSelectWeaponType();
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStopTimer, OnStopTimer);
        EventManager.AddHandler(GameEvent.OnSelectWeaponType, OnSelectWeaponType);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStopTimer, OnStopTimer);
        EventManager.RemoveHandler(GameEvent.OnSelectWeaponType, OnSelectWeaponType);
    }

    private void OnStopTimer()
    {
        StartCoroutine(FireBullets());
    }

    private void OnSelectWeaponType()
    {
        for (int i = 0; i < weaponTypes.Count; i++)
        {
            weaponTypes[i].gameObject.SetActive(false);
        }

        weaponTypes[gameData.WeaponIndex].gameObject.SetActive(true);
        weaponTypes[gameData.WeaponIndex].SetBooster();
        firePoint=weaponTypes[gameData.WeaponIndex].CurrentFirePoint;
        fireRate=weaponTypes[gameData.WeaponIndex].CurrentFireRate;

    }

    IEnumerator FireBullets()
    {
        int bulletsToFire = gameData.RoundedTime+gameData.WeaponBoosterAmount;

        for (int i = 0; i < bulletsToFire; i++)
        {
            Transform target = FindNearestHexParent();

            if (target != null)
            {
                GameObject bullet = BulletPool.Instance.GetBullet();
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetTarget(target);
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }

    private Transform FindNearestHexParent()
    {
        HexParent[] hexParents = FindObjectsOfType<HexParent>();

        if (hexParents.Length == 0)
        {
            return null;
        }

        return hexParents
            .OrderBy(h => Vector3.Distance(firePoint.position, h.transform.position))
            .FirstOrDefault()?.transform;
    }
}
