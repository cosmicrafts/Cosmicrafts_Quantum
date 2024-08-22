namespace CosmicraftsSP
{
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
        Vector3 LastTargetPosition;
        bool IsFake;

        // Enum for different trajectory types
        public enum TrajectoryType
        {
            Straight,
            Wavering,
            Zigzag,
            Circular
        }

        // Selected trajectory type
        public TrajectoryType trajectoryType = TrajectoryType.Straight;

        // Fields for Wavering Movement
        public float WaveringAmplitude = 0.5f; // How far it moves from the center
        public float WaveringFrequency = 2f; // How fast it oscillates

        // Fields for Zigzag Movement
        public float ZigzagAmplitude = 0.5f; // Amplitude of the zigzag
        public float ZigzagFrequency = 2f; // Frequency of the zigzag

        // Fields for Circular Movement
        public float CircularRadius = 1f; // Radius of the circular path
        public float CircularSpeed = 2f; // Speed of circular movement

        private Vector3 initialPosition;
        private float timeSinceStart;

        public bool IsAoE = false;  // Checkmark in Inspector
        public float AoERadius = 5f;  // Radius of AoE damage

        // Make maxLifespan public to customize in Inspector
        public float maxLifespan = 1f; // Maximum life of projectile

        private float timeAlive = 0f;

        private void Start()
        {
            initialPosition = transform.position;
        }

        private void FixedUpdate()
        {
            timeAlive += Time.fixedDeltaTime;
            timeSinceStart += Time.fixedDeltaTime;

            // Check if the projectile has exceeded its maximum lifespan
            if (timeAlive >= maxLifespan)
            {
                HandleImpact(null); // Impact at the last known target position if lifespan exceeded
                return;
            }

            // Check if the target is destroyed or null
            if (Target == null || (Target != null && Target.GetComponent<Unit>() != null && Target.GetComponent<Unit>().IsDeath))
            {
                Target = null; // Target destroyed or null, continue to last known position
            }

            if (Target == null)
            {
                MoveToLastPositionOrDestroy();
            }
            else
            {
                MoveProjectile();
            }
        }

        private void MoveProjectile()
        {
            switch (trajectoryType)
            {
                case TrajectoryType.Straight:
                    MoveStraight();
                    break;
                case TrajectoryType.Wavering:
                    MoveWavering();
                    break;
                case TrajectoryType.Zigzag:
                    MoveZigzag();
                    break;
                case TrajectoryType.Circular:
                    MoveCircular();
                    break;
            }
        }

        private void MoveStraight()
        {
            if (Target != null)
            {
                LastTargetPosition = Target.transform.position;
                RotateTowards(LastTargetPosition);
            }

            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Speed * Time.fixedDeltaTime);

            // Check if the projectile has reached the last known position of the target
            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.1f)
            {
                HandleImpact(null); // Impact at the last known position
            }
        }

        private void MoveWavering()
        {
            if (Target != null)
            {
                LastTargetPosition = Target.transform.position;
                RotateTowards(LastTargetPosition);
            }

            Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
            Vector3 waveringOffset = transform.right * Mathf.Sin(timeSinceStart * WaveringFrequency) * WaveringAmplitude;
            transform.position += forwardMove + waveringOffset;

            // Check if the projectile has reached the last known position of the target
            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.1f)
            {
                HandleImpact(null); // Impact at the last known position
            }
        }

        private void MoveZigzag()
        {
            if (Target != null)
            {
                LastTargetPosition = Target.transform.position;
                RotateTowards(LastTargetPosition);
            }

            Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
            Vector3 zigzagOffset = transform.right * Mathf.Sign(Mathf.Sin(timeSinceStart * ZigzagFrequency)) * ZigzagAmplitude;
            transform.position += forwardMove + zigzagOffset;

            // Check if the projectile has reached the last known position of the target
            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.1f)
            {
                HandleImpact(null); // Impact at the last known position
            }
        }

        private void MoveCircular()
        {
            if (Target != null)
            {
                LastTargetPosition = Target.transform.position;
                RotateTowards(LastTargetPosition);
            }

            Vector3 offset = new Vector3(Mathf.Sin(timeSinceStart * CircularSpeed), 0f, Mathf.Cos(timeSinceStart * CircularSpeed)) * CircularRadius;
            transform.position = initialPosition + offset;
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Speed * Time.fixedDeltaTime);

            // Check if the projectile has reached the last known position of the target
            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.1f)
            {
                HandleImpact(null); // Impact at the last known position
            }
        }

        private void MoveFakeProjectile()
        {
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.25f)
            {
                HandleImpact(null);
            }
        }

        private void MoveToLastPositionOrDestroy()
        {
            transform.position = Vector3.MoveTowards(transform.position, LastTargetPosition, Speed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, LastTargetPosition) <= 0.1f)
            {
                HandleImpact(null); // Impact at the last known target position
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
                if (target != null && !target.IsMyTeam(MyTeam))
                {
                    HandleImpact(target);
                }
            }
            else if (other.CompareTag("Out"))
            {
                HandleImpact(null); // Apply AoE even if it's out of bounds
            }
        }

        void HandleImpact(Unit target)
        {
            // Apply AoE if applicable, even if there's no target
            if (IsAoE)
            {
                ApplyAoEDamage();
            }

            // Impact at the last known target position or collision point
            InstantiateImpactEffect();

            if (target != null && !target.IsDeath)
            {
                ApplyDirectDamage(target);
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
                // Subscribe to the unit's OnDeath event
                Unit targetUnit = target.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    targetUnit.OnDeath += HandleTargetDeath;
                }
            }
        }

        private void HandleTargetDeath(Unit unit)
        {
            // Handle the target death, for example:
            Target = null;
            LastTargetPosition = unit.transform.position;

            // Ensure that the projectile directly moves towards the last position
            MoveToLastPositionOrDestroy();
        }

        private void OnDestroy()
        {
            if (Target != null)
            {
                Unit targetUnit = Target.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    targetUnit.OnDeath -= HandleTargetDeath;
                }
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
