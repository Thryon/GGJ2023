using UnityEngine;

public class DamageOnTriggerEnter : MonoBehaviour
{
    public int damage = 1;
    public bool kill = false;

    private void Start()
    {
        if (ReferencesSingleton.Instance != null)
            ReferencesSingleton.Instance.RegisterTreeRef(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            if (kill)
                health.Kill();
            else
                health.TakeDamage(damage);
        }
    }
}
