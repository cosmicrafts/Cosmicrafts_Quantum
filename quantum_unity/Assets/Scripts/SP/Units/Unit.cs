namespace CosmicraftsSP {
//using EPOOutline;
using System.Linq;
using UnityEngine;
using System.Collections;
/*
 * This code is the base controller for any unit (ship or station)
 * Manages the basic stats and unit behaviors
 */

//The game teams
public enum Team
{
    Blue,
    Red
}

//Type of damages
public enum TypeDmg
{
    Normal,
    Direct,
    Shield
}


public class Unit : MonoBehaviour
{
    //The Id of the unit in the game
    protected int Id;
    //The NFTs source data
    protected NFTsUnit NFTs;
    //A fake unit is used for multiplayer, to show a visual representation of the original unit in master
    protected bool IsFake;
    //This unit is Death
    public bool IsDeath;
    //The Player owner
    public int PlayerId = 1;
    //My game team
    public Team MyTeam;

    //Health Points
    [Range(1, 9999)]
    public int HitPoints = 10;
    //Max Health Points
    int MaxHp = 10;
    //Shield points
    [Range(1, 9999)]
    public int Shield = 0;
    //Max Shield points
    int MaxShield = 0;
    //Shield regeneration delay
    [Range(0, 10)]
    public float ShieldDelay = 3f;
    //Size of the unit
    [Range(0.1f, 10)]
    public float Size = 1f;
    //Spawn area size for other units
    [Range(0, 30)]
    public float SpawnAreaSize = 0f;
    //Chances to dodge an attack 
    [Range(0, 1)]
    public float DodgeChance = 0f; // 0% chance

    //Returns if this unit is the base station
    [HideInInspector]
    public bool IsBaseStation = false;
    //Main Station Source code (can be null)
    [HideInInspector]
    MainStation MainStationData;

    //This unit can't get damage
    [HideInInspector]
    public bool IsInmortal = false;
    [HideInInspector]
    public bool flagShield = false;
    //This unit is disabled (can´t do anything)
    [HideInInspector]
    protected bool Disabled = false;
    //Cast Delay
    [HideInInspector]
    protected float Casting = 1f;

    //Variables for shield local manage
    float ShieldLoad = 0f;
    float ShieldCharge = 0f;
    float ShieldSpeed = 1f;
 
    //Trigger for logical detections
    protected SphereCollider TrigerBase;
    //Trigger for physical detections
    protected SphereCollider SolidBase;

    //Mesh of the unit
    public GameObject Mesh;
    //Spawn area object reference
    public GameObject SA;
    //Explosion game object reference (used when the unit is destroyed)
    public GameObject Explosion;
    //Portal game object reference (Used when the unit is casting)
    public GameObject Portal;
    //Shield game object reference (Used when the unit is impact)
    public GameObject ShieldGameObject;
    //The unit UI reference code
    public UIUnit UI;
    //The outline shader effect reference
   // protected Outlinable //MyOutline;
    //The Animator controller reference
    [SerializeField]
    protected Animator MyAnim;
    protected AnimationClip[] MyClips;
    //Last bullet impact recived
    protected Vector3 LastImpact;

    //RigidBody Reference
    protected Rigidbody MyRb;

    //The retation target when the unit is fake
    Quaternion FakeRotation;

    private void Awake()
    {
        MyClips = MyAnim.runtimeAnimatorController.animationClips;
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        //Initialize te variables
        MainStationData = GetComponent<MainStation>();
        IsBaseStation = MainStationData != null;
        LastImpact = Vector3.zero;
        MaxShield = Shield;
        MaxHp = HitPoints;
        MyRb = GetComponent<Rigidbody>();
        //MyOutline = Mesh.GetComponent<Outlinable>();
        TrigerBase = GetComponent<SphereCollider>();
        SolidBase = Mesh.GetComponent<SphereCollider>();
        //Initialize UI
        UI.Init(MaxHp - 1, MaxShield - 1);
        UI.SetColorBars(!IsMyTeam(GameMng.P.MyTeam));
        //Set the outline color
        //MyOutline.OutlineParameters.Color = GameMng.GM.GetColorUnit(MyTeam, PlayerId);
        //Initialize more variables
        TrigerBase.radius = SolidBase.radius;
        transform.localScale = new Vector3(Size, Size, Size);
        MyAnim = Mesh.GetComponent<Animator>();
        Portal.transform.parent = null;
        //Set the start rotation
        transform.LookAt(CMath.LookToY(transform.position, GameMng.GM.GetDefaultTargetPosition(MyTeam)));
        //Active the spawn area if the size is more than 0
        SA.SetActive(IsMyTeam(GameMng.P.MyTeam) && SpawnAreaSize > 0f);
        //Destroy the cast portal after 3 sec
        Destroy(Portal, 3f);
        //Add the unit at the game manager
        GameMng.GM.AddUnit(this);
        //Initlialize as fake unit if this unit is fake
        if (IsFake)
        {
            InitHasFake();
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //Casting time controller
        if (Casting > 0f)
        {
            Casting -= Time.deltaTime;
            if (Casting <= 0f)
            {
                CastComplete();
            }
        }

        //Fake rotation
        if (FakeRotation != null && IsFake)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, FakeRotation, Time.time * 0.1f);
        }

        //Shield regeneration controller
        if (ShieldLoad > 0.1f)
        {
            ShieldLoad -= Time.deltaTime;
        } else if (!IsFake)
        {
            if (Shield < MaxShield)
            {
                if (ShieldCharge < ShieldSpeed)
                {
                    ShieldCharge += Time.deltaTime;
                } else
                {
                    ShieldCharge = 12.5f;
                    Shield++;
                    UI.SetShieldBar((float)Shield / (float)MaxShield);
                }
            }
        }
    }

    //Normalize and control the physics boundaries
    virtual protected void FixedUpdate()
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

    //Called when the cast is complete
    virtual protected void CastComplete()
    {
        MyAnim.SetBool("Idle", true);
        MyAnim.speed = 1;
        if (SpawnAreaSize > 0f && MyTeam == GameMng.P.MyTeam)
        {
            SA.SetActive(true);
            SA.transform.localScale = new Vector3(SpawnAreaSize, SpawnAreaSize);
        }
    }

    //Add specific kind of damage to the unit
    public void AddDmg(int dmg, TypeDmg typeDmg)
    {
        //Check the state of the unit (can get damage?)
        if (IsDeath || !InControl() || dmg <= 0 )
            return;

        //Reset the shield regeneation delay
        ShieldLoad = ShieldDelay;
        ShieldCharge = 0f;

        //Direct Damage varaible
        int DirectDmg = 0;

        //Check the current shield points
        if (Shield > 0 && typeDmg != TypeDmg.Direct)
        {
            //Reduce the shield points
            Shield -= dmg;
            if (Shield < 0)
            {
                //Reduce the HP if the shield points are negative
                HitPoints += Shield;
                DirectDmg += Mathf.Abs(Shield);
                Shield = 0;
            }
            //Update the unit Sield UI
            UI.SetShieldBar((float)Shield / (float)MaxShield);
        } else if (typeDmg != TypeDmg.Shield)
        {
            //Direct Damage
            HitPoints -= dmg;
            DirectDmg += dmg;
        }

        //Add the data to the metrics
        if (!IsMyTeam(GameMng.P.MyTeam))
        {
            GameMng.MT.AddDamage(DirectDmg);
        }

        //Check if the unit is already death
        if (HitPoints <= 0 && !IsInmortal)
        {
            //Die
            HitPoints = 0;
            Die();
        }

        //Update HP UI
        UI.SetHPBar((float)HitPoints / (float)MaxHp);
    }

    //Add normal damage
    public void AddDmg(int dmg)
    {
        AddDmg(dmg, TypeDmg.Normal);
    }

    //Kill the unit
    public virtual void Die()
    {
        if (IsDeath)
            return;

        //Set the variables
        HitPoints = 0;
        IsDeath = true;
        //Hide UI
        UI.HideUI();
        //Disable Spawn area
        SA.SetActive(false);
        //Run death animation
        MyAnim.SetTrigger("Die");
        //Disable physics
        SolidBase.enabled = false;
    }

    //Disable Unit
    public virtual void DisableUnit()
    {
        Disabled = true;

        //Disable movement component (if exist)
        Ship ship = GetComponent<Ship>();

        if (ship != null)
            ship.CanMove = false;

        //Disable Shooter component (if exist)
        Shooter shooter = GetComponent<Shooter>();

        if (shooter != null)
            shooter.CanAttack = false;
    }

    //Enable unit
    public virtual void EnableUnit()
    {
        Disabled = false;

        //Enable movement component (if exist)
        Ship ship = GetComponent<Ship>();

        if (ship != null)
            ship.CanMove = true;

        //Enable shooter component (if exist)
        Shooter shooter = GetComponent<Shooter>();

        if (shooter != null)
            shooter.CanAttack = true;
    }

    //Check is some team is the same team of this unit
    public bool IsMyTeam(Team other)
    {
        return other == MyTeam;
    }

    //Get is the unit is death
    public bool GetIsDeath()
    {
        return IsDeath;
    }

    //Get if the unit is disabled
    public bool GetIsDisabled()
    {
        return Disabled;
    }

    //Get if the unit is casting
    public bool GetIsCasting()
    {
        return Casting > 0f;
    }

    //Destroy the unit (after death animation)
    public void DestroyUnit()
    {
        //Remove from the game manager
        GameMng.GM.DeleteUnit(this);

        //Chec if is the base station
        if (!GameMng.GM.IsGameOver() && IsBaseStation)
        {
            if (WaveController.instance != null && MyTeam == Team.Red)
            {
                WaveController.instance.OnBaseDestroyed();
            }
            else
            {
                //End the game
                GameMng.GM.EndGame(MyTeam == Team.Blue ? Team.Red : Team.Blue);
            }
            
        }

        //Add metrics data
        if (!IsMyTeam(GameMng.P.MyTeam))
        {
            GameMng.MT.AddKills(1);
        }

        //Destroy the object
        Destroy(gameObject);

        //Call WaveController to Verify if Wave is completed
        
    }

    //Explosion Effect
    public void BlowUpEffect()
    {
        //Instantiate explosion object effect
        GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale * 1.8f;
        Destroy(explosion, 4f);
    }

    //Set the last impact position recived
    public void SetImpactPosition(Vector3 position)
    {
        LastImpact = position;
    }

    //Set the unit ID
    public void setId(int id)
    {
        Id = id;
    }

    //Get the unit ID
    public int getId()
    {
        return Id;
    }

    //Get the NFT key
    public string getKey()
    {
        return NFTs == null ? string.Empty : NFTs.KeyId;
    }

    //Get the owners player ID
    public int GetPlayerId()
    {
        return PlayerId;
    }

    //Get if this unit is fake
    public bool getIsFake()
    {
        return IsFake;
    }

    //Set this unit as fake
    public void setHasFake()
    {
        IsFake = true;
    }

    //Initialize this unit as fake
    void InitHasFake()
    {
        TrigerBase.enabled = false;
        SolidBase.enabled = false;
    }

    //Set the fake rotation
    public void SetFakeRotation(Quaternion quaternion)
    {
        FakeRotation = quaternion;
    }

    //Get the unit animator controller
    public Animator GetAnimator()
    {
        return MyAnim;
    }

    //Get animation clip from animator controller
    public AnimationClip GetAnimationClip(string name)
    {
        return MyClips == null ? null : MyClips.FirstOrDefault(f => f.name == name);
    }

    //Get the max shield points
    public int GetMaxShield()
    {
        return MaxShield;
    }

    //Set the max shield points
    public void SetMaxShield(int maxshield)
    {
        MaxShield = maxshield;
    }

    //Set the shield data as fake data
    public void SetFakeShield(int sh, int maxshield)
    {
        if (!IsFake)
            return;

        Shield = sh;
        MaxShield = maxshield;
        UI.SetShieldBar((float)sh / (float)maxshield);
    }

    //Set the max HP 
    public void SetMaxHitPoints(int maxhp)
    {
        MaxHp = maxhp;
        UI.SetHPBar((float)HitPoints / (float)MaxHp);
    }

    //Get the max HP
    public int GetMaxHitPoints()
    {
        return MaxHp;
    }

    //Set the HP data as fake data
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

    //Set the NFT data source
    public virtual void SetNfts(NFTsUnit nFTsUnit)
    {
        NFTs = nFTsUnit;

        //Validate the data
        if (nFTsUnit == null)
            return;

        //Remplace the variables
        HitPoints = nFTsUnit.HitPoints;
        MaxHp = HitPoints;
        Shield = nFTsUnit.Shield;
        MaxShield = Shield;

        //Initialize the data in the shooter controller (if exist)
        GetComponent<Shooter>()?.InitStatsFromNFT(nFTsUnit);
    }

    //Returns if the unit is in his normal state
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
}
}