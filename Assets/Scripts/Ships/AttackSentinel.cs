using UnityEngine;

/// <summary>
///     Controls the Attack on the Sentinel
///     Enemy vessels attack sentinels by launching space mines.
/// </summary>
public class AttackSentinel : MonoBehaviour
{
    public float attackProbability = 10;
    public float projectileDuration = 10;
    public float projectileSpeed = 10;
    public float projectilePower = 10;
    public GameObject projectile;
    public SimpleSensor attackSensor;

    private void Start()
    {
        attackSensor = transform.Find("AttackSensor").gameObject.GetComponent<SimpleSensor>();
    }

    // Update is called once per frame
    private void Update()
    {
        _handleAttack();
    }

    private void _handleAttack()
    {
        if (attackSensor is null || !attackSensor.blocked) return;
        if (!(Random.value < attackProbability / 100)) return;
        _attack();
    }

    private void _attack()
    {
        var newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        var behaviour = newProjectile.GetComponent<ProjectileBehaviour>();
        if (behaviour is { })
        {
            behaviour.duration = projectileDuration;
            behaviour.power = projectilePower;
        }

        var flight = newProjectile.GetComponent<ProjectileFlight>();
        if (flight is { })
        {
            flight.speed = projectileSpeed;
            flight.SetTarget(attackSensor.blocking);
        }
    }
}