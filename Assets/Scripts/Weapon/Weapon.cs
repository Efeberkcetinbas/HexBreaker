using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameData gameData;

    [Header("Weapon Prop")]
    [SerializeField] private List<WeaponType> weaponTypes= new List<WeaponType>();
    [SerializeField] private float rotationSpeed = 5f; // Speed for rotating to target
    private Transform firePoint;
    private float fireRate = 0.2f; // Interval between shots

    [Header("Critical Hit")]
    [SerializeField] private int RangeOfCriticalThreshold=5;



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

    #region WeaponLogic

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
        CheckHighDamage();

        int bulletsToFire = gameData.RoundedTime + gameData.WeaponBoosterAmount;

        EventManager.Broadcast(GameEvent.OnPlayerStopMove);

        for (int i = 0; i < bulletsToFire; i++)
        {
            Transform target = FindNearestHexParent();

            if (target != null)
            {
                // Rotate weapon to face the target smoothly
                RotateWeaponTowards(target);

                // Create shooting animation with DOTween (e.g., recoil effect)
                AnimateWeaponShooting();

                // Instantiate and fire bullet
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
        
        Debug.Log("ALL BULLETS FIRED");
        EventManager.Broadcast(GameEvent.OnPlayerStartMove);
        EventManager.Broadcast(GameEvent.OnCheckCredit);
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

    private void RotateWeaponTowards(Transform target)
    {
        // Smoothly rotate the weapon towards the target HexParent
        Vector3 direction = (target.position - firePoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void AnimateWeaponShooting()
    {
        // Example of a recoil effect (slightly move the weapon back when shooting)
        transform.DOPunchPosition(Vector3.back * 0.2f, 0.2f, 10, 1)
            .OnKill(() => transform.DOPunchRotation(new Vector3(0, 10, 0), 0.1f)); // Add slight rotation shake
    }

    #endregion


    #region CriticalHit
    // Checks Probability of Critical Hit
    private bool ProbabilityCheck()
    {
        float rand=Random.Range(0,101);
        Debug.Log("randomChance " + rand);
        if(rand<=gameData.ChanceOfCriticalHit)
        {
            return true;
        }
        return false;
    }

    private void CheckHighDamage()
    {
        if(gameData.maxTimerRange-gameData.RoundedTime<=RangeOfCriticalThreshold)
        {
            if(ProbabilityCheck())
            {
                gameData.RoundedTime+=gameData.CriticalHitDamage;
                EventManager.Broadcast(GameEvent.OnUpdateRounded);
                //EventManager.Broadcast(GameEvent.OnCriticalHit);
                Debug.Log("CRITICAL HIT");
            }
            else
                return;
        }
    }

    #endregion


    #region GameEvents
    private void OnRestart()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnNextLevel()
    {
        transform.rotation = Quaternion.identity;
    }

    #endregion
}
