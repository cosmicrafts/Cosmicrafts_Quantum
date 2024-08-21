using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CosmicraftsSP
{
    public class Shooter : MonoBehaviour
    {
        public SphereCollider EnemyDetector;
        [HideInInspector]
        public bool CanAttack = true;
        [Range(1, 99)]
        public float RangeDetector = 1f;
        [Range(0, 99)]
        public float CoolDown = 1f;
        [Range(1, 99)]
        public float BulletSpeed = 10f;
        [Range(1, 99)]
        public int BulletDamage = 1;
        [Range(0f, 1f)]
        public float criticalStrikeChance = 0f;
        public float criticalStrikeMultiplier = 2.0f;
        public bool RotateToEnemy = true;
        public bool StopToAttack = true;
        public GameObject Bullet;
        public Transform[] Cannons;

        private ParticleSystem[] MuzzleFlash;
        private float DelayShoot = 0f;
        private Ship MyShip;
        private Unit MyUnit;
        private List<Unit> InRange;
        private Unit Target;
        private Unit FakeTarget;

        void Start()
        {
            InRange = new List<Unit>();
            EnemyDetector.radius = RangeDetector;
            MyShip = GetComponent<Ship>();
            MyUnit = GetComponent<Unit>();
            MuzzleFlash = new ParticleSystem[Cannons.Length];
            for (int i = 0; i < Cannons.Length; i++)
            {
                if (Cannons[i].childCount > 0)
                {
                    MuzzleFlash[i] = Cannons[i].GetChild(0).GetComponent<ParticleSystem>();
                }
            }
            if (CoolDown <= 0f)
                CoolDown = 0.1f;
        }

        void Update()
        {
            if (MyUnit.getIsFake())
            {
                ShootFakeTarget();
            }
            else
            {
                ShootTarget();
            }
        }

        public void ShootTarget()
        {
            if (MyUnit.GetIsDeath() || !CanAttack || !MyUnit.InControl())
                return;

            if (Target != null)
            {
                if (Target == null || Target.GetIsDeath())
                {
                    RemoveEnemy(Target);
                    return;
                }

                if (RotateToEnemy)
                {
                    var direction = (Target.transform.position - transform.position).normalized;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 3f);
                }

                if (DelayShoot <= 0f)
                {
                    FireProjectiles();

                    DelayShoot = CoolDown;
                    MyUnit.GetAnimator().SetTrigger("Attack");
                }
                else
                {
                    DelayShoot -= Time.deltaTime;
                }
            }

            if (Target == null)
            {
                CleanEnemies();
            }
        }

        public void ShootFakeTarget()
        {
            if (MyUnit.GetIsDeath() || MyUnit.GetIsDisabled() && MyUnit.GetIsCasting())
                return;

            if (FakeTarget != null)
            {
                if (DelayShoot <= 0f)
                {
                    FireFakeProjectiles();

                    DelayShoot = CoolDown;
                    MyUnit.GetAnimator().SetTrigger("Attack");
                }
                else
                {
                    DelayShoot -= Time.deltaTime;
                }
            }
        }

        private void FireProjectiles()
        {
            for (int i = 0; i < Cannons.Length; i++)
            {
                Transform cannon = Cannons[i];

                GameObject bulletPrefab = Instantiate(Bullet, cannon.position, cannon.rotation);
                Projectile bullet = bulletPrefab.GetComponent<Projectile>();
                bullet.MyTeam = MyUnit.MyTeam;
                bullet.SetTarget(Target.gameObject);
                bullet.Speed = BulletSpeed;

                if (Random.value < criticalStrikeChance)
                {
                    bullet.Dmg = (int)(BulletDamage * criticalStrikeMultiplier);
                }
                else
                {
                    bullet.Dmg = BulletDamage;
                }

                if (MuzzleFlash[i] != null)
                {
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
                }
            }
        }

        private void FireFakeProjectiles()
        {
            for (int i = 0; i < Cannons.Length; i++)
            {
                Transform cannon = Cannons[i];

                GameObject bulletPrefab = Instantiate(Bullet, cannon.position, cannon.rotation);
                Projectile bullet = bulletPrefab.GetComponent<Projectile>();
                bullet.MyTeam = MyUnit.MyTeam;
                bullet.SetLastPosition(FakeTarget.transform.position);
                bullet.Speed = BulletSpeed;
                bullet.Dmg = 0;
                bullet.SetFake(true);

                if (MuzzleFlash[i] != null)
                {
                    MuzzleFlash[i].Clear();
                    MuzzleFlash[i].Play();
                }
            }
        }

        public void SetTarget(Unit target)
        {
            Target = target;

            if (MyShip == null || !StopToAttack)
                return;

            if (Target == null)
            {
                MyShip.ResetDestination();
            }
            else
            {
                MyShip.SetDestination(Target.transform.position, RangeDetector);
            }
        }

        public void SetFakeTarget(Unit target)
        {
            FakeTarget = target;
        }

        public void AddEnemy(Unit enemy)
        {
            if (!InRange.Contains(enemy))
                InRange.Add(enemy);

            if (Target == null)
            {
                CleanEnemies();
            }
        }

        public void RemoveEnemy(Unit enemy)
        {
            if (InRange.Contains(enemy))
                InRange.Remove(enemy);

            if (Target == null)
            {
                CleanEnemies();
            }
        }

        public void CleanEnemies()
        {
            InRange = InRange.Where(f => f != null && !f.GetIsDeath()).ToList();

            if (Target == null || Target.GetIsDeath())
            {
                if (InRange.Count == 0)
                {
                    SetTarget(null);
                }
                else
                {
                    Unit closer = InRange.OrderBy(o => Vector3.Distance(transform.position, o.transform.position)).FirstOrDefault();
                    SetTarget(closer);
                }
            }
        }

        public void StopAttack()
        {
            CanAttack = false;
            InRange.Clear();
            SetTarget(null);
        }

        public int GetIdTarget()
        {
            return Target == null ? 0 : Target.getId();
        }

        public virtual void InitStatsFromNFT(NFTsUnit nFTsUnit)
        {
            if (nFTsUnit != null)
            {
                BulletDamage = nFTsUnit.Dammage;
            }
        }
    }
}
