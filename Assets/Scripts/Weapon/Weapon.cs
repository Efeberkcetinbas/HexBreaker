using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameData gameData;

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
        Fire();
    }

    
    void Fire()
    {
        for (int i = 0; i < gameData.RoundedTime; i++)
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
        }
    }
}
