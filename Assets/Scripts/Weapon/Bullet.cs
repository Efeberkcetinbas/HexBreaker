using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (target != null && Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }

        if (target != null)
        {
            HitTarget();
        }
        else
        {
            BulletPool.Instance.ReturnBullet(gameObject); // No target, return to pool
        }
    }

    void HitTarget()
    {
        HexParent hexParent = target.GetComponent<HexParent>();

        if (hexParent != null)
        {
            hexParent.DecreaseTowerValue();

            if (hexParent.towerValue <= 0)
            {
                Destroy(hexParent.gameObject);
            }
        }

        BulletPool.Instance.ReturnBullet(gameObject);
    }

    
}
