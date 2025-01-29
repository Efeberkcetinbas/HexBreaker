using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameData gameData;
    [SerializeField] private float fireRate = 0.2f; // Interval between shots

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStopTimer, OnStopTimer);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStopTimer, OnStopTimer);
    }

    private void OnStopTimer()
    {
        StartCoroutine(FireBullets());
    }

    IEnumerator FireBullets()
    {
        int bulletsToFire = gameData.RoundedTime;

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
