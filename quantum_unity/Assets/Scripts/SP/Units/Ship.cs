namespace CosmicraftsSP {
using SensorToolkit;
using UnityEngine;

/*
 * This is an extension code from the Unit script
 * Adds and controls the movement for the units
 */

public class Ship : Unit
{
    //The movement controller reference
    public SteeringRig MySt;

    //Current unit speed
    float Speed = 0f;

    //Aceleration speed
    [Range(0,99)]
    public float Aceleration = 1f;
    //Max speed
    [Range(0,99)]
    public float MaxSpeed = 10f;
    //Drag speed
    [Range(0,99)]
    public float DragSpeed = 1f;
    //Turn speed
    [Range(0, 99)]
    public float TurnSpeed = 5f;
    //Desaceleration speed
    [Range(0, 99)]
    public float StopSpeed = 5f;
    //Stop distance from the target
    [Range(0, 10)]
    public float StoppingDistance = 0.5f;
    //Avoidance range from other units
    [Range(0, 50)]
    public float AvoidanceRange = 3f;

    //Current destination
    Transform Target;
    //Fake destination (for multiplayer)
    Vector3 FakeDestination;
    //Avoidance sensors references
    public RaySensor[] AvoidanceSensors;
    //Thrusters Parent game object reference
    public GameObject[] Thrusters;

    //Controls if the ship can move on
    [HideInInspector]
    public bool CanMove = true;

    //Deth ship rotation
    Vector3 DeathRot;

    protected override void Start()
    {
        base.Start();
        //Set the current destination (enemy´s base station)
        Target = GameMng.GM.GetFinalTransformTarget(MyTeam);
        FakeDestination = transform.position;
        if (IsFake)
        {
            //If this is a fake unit, disable the movement controller
            MySt.enabled = false;
        } else
        {
            //Set the values and destination to the movement controller
            MySt.Destination = Target.position;
            MySt.StoppingDistance = StoppingDistance;
            foreach (RaySensor sensor in AvoidanceSensors)
            {
                sensor.Length = AvoidanceRange;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        //Move the ship
        Move();
    }

    protected override void FixedUpdate()
    {
        //Normalize and control the movement boundaries
        if (MyRb.velocity.magnitude > MaxSpeed+1f)
        {
            MyRb.velocity = MyRb.velocity.normalized * (MaxSpeed + 1f);
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

    //Move the ship
    void Move()
    {
        //If the unit is death...
        if (IsDeath)
        {
            //Rotates the ship to the death rotation
            transform.Rotate(DeathRot, 100f * Time.deltaTime, Space.Self);
            return;
        }

        //If this is a fake ship
        if (IsFake)
        {
            //Just move to the destination smoothly
            transform.position = Vector3.Lerp(transform.position, FakeDestination, Time.deltaTime * MaxSpeed);
            EnableThrusters(Vector3.Distance(transform.position, FakeDestination) > Speed);
        } else //Normal Ship
        {
            //If the unit is active
            if (InControl())
            {
                //If the ship can move
                if (CanMove)
                {
                    //Controlls the speed
                    if (Speed < MaxSpeed)
                    {
                        Speed += Aceleration * Time.deltaTime;
                    }
                    else
                    {
                        Speed = MaxSpeed;
                    }

                    //Inject the movement values in the movement controller
                    MySt.TurnForce = TurnSpeed * 100f;
                    MySt.StrafeForce = DragSpeed * 100f;
                    MySt.MoveForce = Speed * 100f;
                    MySt.StopSpeed = StopSpeed;
                }
                else //The ship is stoped
                {
                    //Stop the movement controller
                    MySt.TurnForce = 0f;
                    MySt.MoveForce = 0f;
                    Speed = 0f;
                }

                //Enable and disable the thrusters when the ship moves or reachs the destination
                if (MySt.hasReachedDestination() && ThrustersAreEnable())
                {
                    EnableThrusters(false);
                }
                if (!MySt.hasReachedDestination() && !ThrustersAreEnable())
                {
                    EnableThrusters(true);
                }
            }//The ship has not control and the thrusters are active
            else if (ThrustersAreEnable())
            {
                //Disable the thrusters
                EnableThrusters(false);
                //Stop the movement controller
                MySt.TurnForce = 0f;
                MySt.MoveForce = 0f;
                Speed = 0f;
            }
        }
    }

    //Restore the default destination (enemy's base station)
    public void ResetDestination()
    {
        if (!InControl())
            return;
        
        if (Target != null)
        {
            MySt.Destination = Target.position;
            MySt.StoppingDistance = StoppingDistance;
        }
        
    }

    //Set the current destination
    public void SetDestination(Vector3 des, float stopdistance)
    {
        MySt.Destination = des;
        MySt.StoppingDistance = stopdistance;
    }

    //Set the fake destination
    public void SetFakeDestination(Vector3 des)
    {
        FakeDestination = des;
    }

    //Do something when the unit spawn is complete
    protected override void CastComplete()
    {
        base.CastComplete();
    }

    //Kill the Unit
    public override void Die()
    {
        base.Die();
        MySt.enabled = false;
        EnableThrusters(false);
        float AngleDeathRot = CMath.AngleBetweenVector2(LastImpact, transform.position);

        float z = Mathf.Sin(AngleDeathRot * Mathf.Deg2Rad);
        float x = Mathf.Cos(AngleDeathRot * Mathf.Deg2Rad);
        DeathRot = new Vector3(x, 0, z);
    }

    //Disable the unit
    public override void DisableUnit()
    {
        base.DisableUnit();
    }

    //Enable the unit
    public override void EnableUnit()
    {
        base.EnableUnit();
    }

    //Set the NFT data source
    public override void SetNfts(NFTsUnit nFTsUnit)
    {
        base.SetNfts(nFTsUnit);

        if (nFTsUnit == null)
            return;

        MaxSpeed = nFTsUnit.Speed;
    }

    //Enable or disable Thrusters
    void EnableThrusters(bool enable)
    {
        foreach (GameObject t in Thrusters)
            t.SetActive(enable);
    }

    bool ThrustersAreEnable()
    {
        return Thrusters == null ? false : (Thrusters.Length > 0 ? Thrusters[0].activeSelf : false);
    }
}
}