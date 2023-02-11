using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttack : MonoBehaviour
{
    public TreeRoot root;
    public Transform target;

    public float attackTime;
    public float shrinkSpeed;
    public Vector2 impactTime;
    public AnimationCurve attackCurve;

    public int damage;
    public float force;

    public bool RootAvailable = true;

    private Collider enemyTarget;
    private Action OnAttackOver;

    public void Initialize()
    {
        root.ActivateOnTarget(target);
        root.growTime = attackTime;
        root.shrinkTime = shrinkSpeed;
        RootAvailable = true;
    }

    private Coroutine attackCoroutine;
    public void Attack(Collider enemy, Action AttackOver)
    {
        if (!RootAvailable) return;

        OnAttackOver = AttackOver;

        RootAvailable = false;
        enemyTarget = enemy;
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        root.ActivateOnTarget(enemyTarget.transform, true);

        yield return new WaitForSeconds(attackTime);

        root.RemoveTarget();
        if (enemyTarget)
        {
            enemyTarget.GetComponent<Health>().TakeDamage(damage);
            //enemyTarget.GetComponent<Ragdoll>().Fling(enemyTarget.transform.position + Vector3.down, Vector3.up, force);
        }

        //GlobalEvents.Instance.SendEvent(GlobalEventEnum.CameraShake);

        yield return new WaitForSeconds(UnityEngine.Random.Range(impactTime.x, impactTime.y));

        root.Shrink();

        yield return new WaitForSeconds(shrinkSpeed);

        OnAttackOver?.Invoke();
        target.position = root.transform.position;
        RootAvailable = true;
    }
}
