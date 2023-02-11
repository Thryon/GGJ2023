using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackTree : MonoBehaviour
{
    [Header("Properties")]
    public List<RootAttack> roots = new List<RootAttack>();
    public RootAttack rootPrefab;
    public LayerMask detectionLayer;

    public Vector2 attackCooldown = new Vector2(3f, 5f);
    public float detectionRadius = 50f;


    [Header("Status")]
    Collider enemy;
    private bool readyToAttack;
    private float cooldown;
    private float cooldownTime;

    [Header("Upgrades")]
    public int rootsAmount = 3;
    public int rootsAddedPerUpgrade = 3;
    public int damage = 10;
    public int damageAddedPerUpgrade = 5;

    [Header("Cheats")]
    [SerializeField] private bool upgradeRoots;
    [SerializeField] private bool upgradeDamage;

    private List<Collider> targetedEnemies = new List<Collider>();

    private void Start()
    {
        foreach (var r in roots)
        {
            r.Initialize();
            r.damage = damage;
        }
    }

    private void Update()
    {
        AttackLoop();
        Cheats();
    }

    private void AttackLoop()
    {
        if (!readyToAttack)
        {
            if (cooldown < cooldownTime)
                cooldown += Time.deltaTime;
            else
                readyToAttack = true;
        }
        else
        {
            FindEnemy(ref enemy);

            if (enemy)
            {
                RootAttack root = GetAvailableRoot(out var success);
                if (success)
                {
                    Collider targetedEnemy = enemy;
                    root.Attack(targetedEnemy, () => targetedEnemies.Remove(targetedEnemy));
                    targetedEnemies.Add(targetedEnemy);

                    cooldownTime = Random.Range(attackCooldown.x, attackCooldown.y);
                    cooldown = 0f;
                    readyToAttack = false;
                }
            }
        }
    }
    private void Cheats()
    {
        if (upgradeRoots)
        {
            UpgradeRootsAmount();
            upgradeRoots = false;
        }
        if (upgradeDamage)
        {
            UpgradeDamage();
            upgradeDamage = false;
        }
    }

    private void FindEnemy(ref Collider enemy)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer, QueryTriggerInteraction.Collide);
        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (targetedEnemies.Contains(enemies[i]))
                    continue;

                enemy = enemies[Random.Range(i, enemies.Length)];
                return;
            }

            enemy = enemies[0];
            return;
        }
        enemy = null;
    }

    private RootAttack GetAvailableRoot(out bool success)
    {
        for (int i = 0; i < Mathf.Min(rootsAmount, roots.Count); i++)
        {
            if (roots[i].RootAvailable)
            {
                success = true;
                return roots[i];
            }
        }

        success = false;
        return null;
    }

    public void UpgradeRootsAmount()
    {
        rootsAmount += rootsAddedPerUpgrade;

        while (roots.Count < rootsAmount)
        {
            RootAttack root = Instantiate(rootPrefab, roots[0].transform.position, roots[0].transform.rotation, roots[0].transform.parent);
            roots.Add(root);
            root.Initialize();
        }
    }
    public void UpgradeDamage()
    {
        damage += damageAddedPerUpgrade;
        foreach (var r in roots)
            r.damage = damage;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position;
        float radius = detectionRadius;
        int segments = 16;


        Gizmos.color = enemy ? Color.red : Color.grey;
        float TWO_PI = Mathf.PI * 2;
        float step = TWO_PI / (float)segments;
        float theta = 0;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = center + new Vector3(x, 0, y);
        Vector3 newPos;
        Vector3 lastPos = pos;

        for (theta = step; theta < TWO_PI; theta += step)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos = center + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }
}
