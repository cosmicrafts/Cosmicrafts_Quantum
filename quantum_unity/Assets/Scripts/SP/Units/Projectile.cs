namespace CosmicraftsSP {
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public Team MyTeam;

    GameObject Target;
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public int Dmg;

    public GameObject canvasDamageRef;
    public GameObject impact;
    //public GameObject Shieldimpact;
    Vector3 LastTargetPosition;
    bool IsFake;

    private void Update()
    {
        MoveProjectile();
        CheckTargetStatus();
    }

    private void MoveProjectile()
    {
        if (IsFake)
        {
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Time.deltaTime * Speed);
        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * Speed;
        }
    }

    private void CheckTargetStatus()
    {
        if (Target != null)
        {
            LastTargetPosition = Target.transform.position;
            RotateTowards(Target.transform.position);
        }
        else
        {
            RotateTowards(LastTargetPosition);
            if (Vector3.Distance(transform.position, LastTargetPosition) < 0.25f)
            {
                HandleImpact(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsFake) return;

        if (other.gameObject == Target)
        {
            HandleImpact(Target.GetComponent<Unit>());
        }
        else if (other.CompareTag("Unit"))
        {
            Unit target = other.gameObject.GetComponent<Unit>();
            if (!target.IsMyTeam(MyTeam))
            {
                HandleImpact(target);
            }
        }
        else if (other.CompareTag("Out"))
        {
            Destroy(gameObject);
        }
    }

    void HandleImpact(Unit target)
    {
        if (target == null || target.IsDeath)
        {
            InstantiateImpactEffect();
        }
        else
        {
            if (Random.value < target.DodgeChance)
            {
                Dmg = 0;
            }

            if (target.Shield > 0 && !target.flagShield)
            {
                target.OnImpactShield(Dmg);
            }
            else
            {
                InstantiateImpactEffect();
            }
            /*
            if (canvasDamageRef)
            {
                CreateDamageCanvas();
            }
            */
            target.AddDmg(Dmg);
            target.SetImpactPosition(transform.position);
        }

        Destroy(gameObject);
    }

    void InstantiateImpactEffect()
    {
        GameObject impactPrefab = Instantiate(impact, transform.position, Quaternion.identity);
        Destroy(impactPrefab, 0.25f);
    }

    /*void CreateDamageCanvas()
    {
        GameObject cloneDamageCanvas = Instantiate(canvasDamageRef, transform.position, Quaternion.Euler(Vector3.zero));
        CanvasDamage tempDamage = cloneDamageCanvas.GetComponent<CanvasDamage>();
        tempDamage.SetDamage(Dmg);
    }*/

    void RotateTowards(Vector3 target)
    {
        Quaternion lookRotation = Quaternion.LookRotation((target - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);
    }

    public void SetLastPosition(Vector3 lastPosition)
    {
        LastTargetPosition = lastPosition;
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            LastTargetPosition = target.transform.position;
        }
    }

    public void SetFake(bool isFake)
    {
        IsFake = isFake;
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
            sc.enabled = !isFake;
    }
}

}