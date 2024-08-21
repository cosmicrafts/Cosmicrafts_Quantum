namespace CosmicraftsSP
{
    using UnityEngine;
    using System.Collections.Generic;

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
        Vector3 LastTargetPosition;
        bool IsFake;

        // New Fields
        public bool IsAoE = false;  // Checkmark in Inspector
        public float AoERadius = 5f;  // Radius of AoE damage

        private void FixedUpdate()
        {
            if (IsFake)
            {
                MoveFakeProjectile();
            }
            else
            {
                MoveProjectile();
            }
        }

        private void MoveProjectile()
        {
            if (Target != null)
            {
                LastTargetPosition = Target.transform.position;
                RotateTowards(LastTargetPosition);
            }
            else
            {
                RotateTowards(LastTargetPosition);
                if (Vector3.Distance(transform.position, LastTargetPosition) < 0.25f)
                {
                    HandleImpact(null);
                    return;
                }
            }

            transform.position += transform.forward * Speed * Time.fixedDeltaTime;
        }

        private void MoveFakeProjectile()
        {
            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.25f)
            {
                HandleImpact(null);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Speed * Time.fixedDeltaTime);
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
                if (IsAoE)
                {
                    ApplyAoEDamage();
                }
                else
                {
                    ApplyDirectDamage(target);
                }

                target.SetImpactPosition(transform.position);
            }

            Destroy(gameObject);
        }

        void ApplyDirectDamage(Unit target)
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

            target.AddDmg(Dmg);
        }

        void ApplyAoEDamage()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AoERadius);
            foreach (Collider hitCollider in hitColliders)
            {
                Unit unit = hitCollider.GetComponent<Unit>();
                if (unit != null && !unit.IsMyTeam(MyTeam))
                {
                    ApplyDirectDamage(unit);
                }
            }
            InstantiateImpactEffect();
        }

        void InstantiateImpactEffect()
        {
            GameObject impactPrefab = Instantiate(impact, transform.position, Quaternion.identity);
            Destroy(impactPrefab, 0.25f);
        }

        void RotateTowards(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
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
