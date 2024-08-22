using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using System.Linq;
using System.Collections;
using EPOOutline;
using System;

namespace CosmicraftsSP
{
    public enum Team
    {
        Blue,
        Red
    }

    public enum TypeDmg
    {
        Normal,
        Direct,
        Shield
    }

    public class Unit : MonoBehaviour
    {
        public event Action<Unit> OnDeath;
        protected int Id;
        protected NFTsUnit NFTs;
        protected bool IsFake;
        public bool IsDeath;
        public int PlayerId = 1;
        public Team MyTeam;

        [Range(1, 999999)]
        public int HitPoints = 10;
        int MaxHp = 10;
        [Range(1, 999999)]
        public int Shield = 0;
        int MaxShield = 0;
        [Range(0, 10)]
        public float ShieldDelay = 3f;
        [Range(0.1f, 10)]
        public float Size = 1f;
        [Range(0, 30)]
        public float SpawnAreaSize = 0f;
        [Range(0, 1)]
        public float DodgeChance = 0f;
        [Range(1, 999)]
        public int Level = 1;

        [HideInInspector]
        public bool IsBaseStation = false;
        [HideInInspector]
        MainStation MainStationData;

        [HideInInspector]
        public bool IsInmortal = false;
        [HideInInspector]
        public bool flagShield = false;
        [HideInInspector]
        protected bool Disabled = false;
        [HideInInspector]
        protected float Casting = 1f;

        float ShieldLoad = 0f;
        float ShieldCharge = 0f;
        float ShieldSpeed = 1f;

        protected SphereCollider TrigerBase;
        protected SphereCollider SolidBase;

        public GameObject Mesh;
        public GameObject SA;
        public GameObject Explosion;
        public GameObject Portal;
        public GameObject ShieldGameObject;
        public UIUnit UI;
        protected Outlinable MyOutline;
        [SerializeField]
        protected Animator MyAnim;
        protected AnimationClip[] MyClips;
        protected Vector3 LastImpact;

        protected Rigidbody MyRb;

        Quaternion FakeRotation;

        private void Awake()
        {
            MyClips = MyAnim.runtimeAnimatorController.animationClips;
        }

        protected virtual void Start()
        {
            MainStationData = GetComponent<MainStation>();
            IsBaseStation = MainStationData != null;
            LastImpact = Vector3.zero;
            MaxShield = Shield;
            MaxHp = HitPoints;
            Level = Mathf.Clamp(Level, 1, 999);
            MyRb = GetComponent<Rigidbody>();
            MyOutline = Mesh.GetComponent<Outlinable>();
            TrigerBase = GetComponent<SphereCollider>();
            SolidBase = Mesh.GetComponent<SphereCollider>();

            UI.Init(MaxHp - 1, MaxShield - 1);
            UI.SetColorBars(!IsMyTeam(GameMng.P.MyTeam));
            MyOutline.OutlineParameters.Color = GameMng.GM.GetColorUnit(MyTeam, PlayerId);
            TrigerBase.radius = SolidBase.radius;
            transform.localScale = new Vector3(Size, Size, Size);
            MyAnim = Mesh.GetComponent<Animator>();
            Portal.transform.parent = null;
            transform.LookAt(CMath.LookToY(transform.position, GameMng.GM.GetDefaultTargetPosition(MyTeam)));
            SA.SetActive(IsMyTeam(GameMng.P.MyTeam) && SpawnAreaSize > 0f);
            Destroy(Portal, 3f);
            GameMng.GM.AddUnit(this);

            if (IsFake)
            {
                InitHasFake();
            }
        }

        protected virtual void Update()
        {
            if (Casting > 0f)
            {
                Casting -= Time.deltaTime;
                if (Casting <= 0f)
                {
                    CastComplete();
                }
            }

            if (FakeRotation != null && IsFake)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, FakeRotation, Time.time * 0.1f);
            }

            if (ShieldLoad > 0.1f)
            {
                ShieldLoad -= Time.deltaTime;
            }
            else if (!IsFake && Shield < MaxShield)
            {
                if (ShieldCharge < ShieldSpeed)
                {
                    ShieldCharge += Time.deltaTime;
                }
                else
                {
                    ShieldCharge = 12.5f;
                    Shield++;
                    UI.SetShieldBar((float)Shield / (float)MaxShield);
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (MyRb.velocity.magnitude > 0f)
            {
                MyRb.velocity = Vector3.zero;
            }
            if (MyRb.angularVelocity.magnitude > 0.5f)
            {
                MyRb.angularVelocity = Vector3.zero;
            }
            if (transform.rotation.x != 0f || transform.rotation.y != 0f)
            {
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
        }

        protected virtual void CastComplete()
        {
            MyAnim.SetBool("Idle", true);
            MyAnim.speed = 1;
            if (SpawnAreaSize > 0f && MyTeam == GameMng.P.MyTeam)
            {
                SA.SetActive(true);
                SA.transform.localScale = new Vector3(SpawnAreaSize, SpawnAreaSize);
            }
        }

        public void AddDmg(int dmg, TypeDmg typeDmg)
        {
            if (IsDeath || !InControl() || dmg <= 0)
                return;

            ShieldLoad = ShieldDelay;
            ShieldCharge = 0f;

            int DirectDmg = 0;

            if (Shield > 0 && typeDmg != TypeDmg.Direct)
            {
                Shield -= dmg;
                if (Shield < 0)
                {
                    HitPoints += Shield;
                    DirectDmg += Mathf.Abs(Shield);
                    Shield = 0;
                }
                UI.SetShieldBar((float)Shield / (float)MaxShield);
            }
            else if (typeDmg != TypeDmg.Shield)
            {
                HitPoints -= dmg;
                DirectDmg += dmg;
            }

            if (!IsMyTeam(GameMng.P.MyTeam))
            {
                GameMng.MT.AddDamage(DirectDmg);
            }

            if (HitPoints <= 0 && !IsInmortal)
            {
                HitPoints = 0;
                Die();
            }

            if (HitPoints <= 0 && !IsInmortal)
            {
                HitPoints = 0;
                Die();
            }

            UI.SetHPBar((float)HitPoints / (float)MaxHp);
        }

        public void AddDmg(int dmg)
        {
            AddDmg(dmg, TypeDmg.Normal);
        }

        public virtual void Die()
        {
            if (IsDeath)
                return;

            HitPoints = 0;
            IsDeath = true;

            // Broadcast the death event
            OnDeath?.Invoke(this);

            UI.HideUI();
            SA.SetActive(false);
            MyAnim.SetTrigger("Die");
            SolidBase.enabled = false;
        }

        public virtual void DisableUnit()
        {
            Disabled = true;

            Ship ship = GetComponent<Ship>();

            if (ship != null)
                ship.CanMove = false;

            Shooter shooter = GetComponent<Shooter>();

            if (shooter != null)
                shooter.CanAttack = false;
        }

        public virtual void EnableUnit()
        {
            Disabled = false;

            Ship ship = GetComponent<Ship>();

            if (ship != null)
                ship.CanMove = true;

            Shooter shooter = GetComponent<Shooter>();

            if (shooter != null)
                shooter.CanAttack = true;
        }

        public bool IsMyTeam(Team other)
        {
            return other == MyTeam;
        }

        public bool GetIsDeath()
        {
            return IsDeath;
        }

        public bool GetIsDisabled()
        {
            return Disabled;
        }

        public bool GetIsCasting()
        {
            return Casting > 0f;
        }

        public void DestroyUnit()
        {
            GameMng.GM.DeleteUnit(this);

            if (!GameMng.GM.IsGameOver() && IsBaseStation)
            {
                if (WaveController.instance != null && MyTeam == Team.Red)
                {
                    WaveController.instance.OnBaseDestroyed();
                }
                else
                {
                    GameMng.GM.EndGame(MyTeam == Team.Blue ? Team.Red : Team.Blue);
                }
            }

            if (!IsMyTeam(GameMng.P.MyTeam))
            {
                GameMng.MT.AddKills(1);
            }

            Destroy(gameObject);
        }

        public void BlowUpEffect()
        {
            GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
            explosion.transform.localScale = transform.localScale * 1.8f;
            Destroy(explosion, 4f);
        }

        public void SetImpactPosition(Vector3 position)
        {
            LastImpact = position;
        }

        public void setId(int id)
        {
            Id = id;
        }

        public int getId()
        {
            return Id;
        }

        public string getKey()
        {
            return NFTs == null ? string.Empty : NFTs.KeyId;
        }

        public int GetPlayerId()
        {
            return PlayerId;
        }

        public bool getIsFake()
        {
            return IsFake;
        }

        public void setHasFake()
        {
            IsFake = true;
        }

        void InitHasFake()
        {
            TrigerBase.enabled = false;
            SolidBase.enabled = false;
        }

        public void SetFakeRotation(Quaternion quaternion)
        {
            FakeRotation = quaternion;
        }

        public Animator GetAnimator()
        {
            return MyAnim;
        }

        public AnimationClip GetAnimationClip(string name)
        {
            return MyClips == null ? null : MyClips.FirstOrDefault(f => f.name == name);
        }

        public int GetMaxShield()
        {
            return MaxShield;
        }

        public void SetMaxShield(int maxshield)
        {
            MaxShield = maxshield;
        }

        public void SetFakeShield(int sh, int maxshield)
        {
            if (!IsFake)
                return;

            Shield = sh;
            MaxShield = maxshield;
            UI.SetShieldBar((float)sh / (float)maxshield);
        }

        public void SetMaxHitPoints(int maxhp)
        {
            MaxHp = maxhp;
            UI.SetHPBar((float)HitPoints / (float)MaxHp);
        }

        public int GetMaxHitPoints()
        {
            return MaxHp;
        }

        public void SetFakeHp(int hp, int maxhp)
        {
            if (!IsFake)
                return;

            float diference = Mathf.Abs(hp - HitPoints);
            if (!IsMyTeam(GameMng.P.MyTeam))
            {
                GameMng.MT.AddDamage(diference);
            }
            HitPoints = hp;
            MaxHp = maxhp;
            UI.SetHPBar((float)hp / (float)maxhp);
        }

        public virtual void SetNfts(NFTsUnit nFTsUnit)
        {
            NFTs = nFTsUnit;

            if (nFTsUnit == null)
                return;

            HitPoints = nFTsUnit.HitPoints;
            MaxHp = HitPoints;
            Shield = nFTsUnit.Shield;
            MaxShield = Shield;

            GetComponent<Shooter>()?.InitStatsFromNFT(nFTsUnit);
        }

        public bool InControl()
        {
            return (!Disabled && Casting <= 0f && !IsFake);
        }

        public void OnImpactShield(int dmg)
        {
            ShieldGameObject.SetActive(true);
            AddDmg(dmg);
            StopAllCoroutines();
            StartCoroutine(DesactiveShield());
        }

        public IEnumerator DesactiveShield()
        {
            yield return new WaitForSeconds(1f);
            ShieldGameObject.SetActive(false);
        }

        public int GetLevel()
        {
            return Level;
        }

        public void SetLevel(int newLevel)
        {
            Level = Mathf.Clamp(newLevel, 1, 99);
        }

        public virtual void MoveTo(Vector3 position)
        {
            Ship ship = GetComponent<Ship>();
            if (ship != null)
            {
                ship.SetDestination(position, ship.StoppingDistance);
            }
            else
            {
                Debug.LogWarning($"MoveTo called on {name}, but no Ship component was found.");
            }
        }
    }
}
