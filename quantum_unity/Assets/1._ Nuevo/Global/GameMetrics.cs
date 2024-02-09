/*
 * This is the in-game metrics controller
 * Save the game statistics and calculates the game results
*/
public class GameMetrics
{
    //Energy variables
    float EnergyUsed;
    float EnergyGenerated;
    float EnergyWasted;
    float EnergyChargeRatePerSec;

    //Battle variables
    float Damage;
    float DamageReceived;
    float DamageCritic;
    float DamageEvaded;
    
    int Kills;
    int Deploys;

    //Remaining time
    int SecRemaining;
    //Score Token
    int Score;

    // Start is called before the first frame update
    public void InitMetrics()
    {
        //Init metrics
        EnergyUsed = 0;
        EnergyGenerated = 0;
        EnergyWasted = 0;
        EnergyChargeRatePerSec = 0f;

        Damage = 0f;
        DamageReceived= 0f;
        DamageCritic= 0f;
        DamageEvaded= 0f;
        
        Kills = 0;
        Deploys = 0;
        SecRemaining = 0;

        Score = 0;
    }

    //Calculate final metrics when game ends
    public void CalculateLastMetrics(float CurrentEnergy, float SpeedEnergy )
    {
        EnergyWasted += CurrentEnergy;
        EnergyChargeRatePerSec = SpeedEnergy;
        SecRemaining = 0;

        //Score = (int)Damage + (Kills * 10) + (Deploys * 10) + (SecRemaining * 3) + (int)EnergyUsed - (int) EnergyWasted;

        
        //Mandar datos a Web
        
        /*if (GlobalManager.GMD.CurrentMatch == Match.bots)
        {
            if (GlobalManager.GMD.IsProductionWeb())
            {
                GameNetwork.JSSaveScore(Score);
            }
            GlobalManager.GMD.GetUserProgress().AddBattlePoints(Score);
        }
        */
        
        
    }

  
    public void AddEnergyUsed(float value) { EnergyUsed += value; }
    public void AddEnergyGenerated(float value) { EnergyGenerated += value; }

    //Add energy generated when players energy is full
    public void AddEnergyWasted(float value) { EnergyWasted += value; }

    //Damage done
    public void AddDamage(float value)
    {
        Damage += value;
        Score += (int)value;
    }
    public void AddDamageReceived(float value) { DamageReceived += value; }
    public void AddDamageCritic(float value) { DamageCritic += value; }
    public void AddDamageEvaded(float value) { DamageEvaded += value; }

    //TakeDowns
    public void AddKills(int value)
    {
        Kills += value;
        Score += value * 1000;
    }

    //Player cards deploys
    public void AddDeploys(int value)
    {
        Deploys += value;
    }

    //Gets
    #region
    public float GetEnergyUsed() { return EnergyUsed; }
    public float GetEnergyGenerated() { return EnergyGenerated; }
    public float GetEnergyWasted() { return EnergyWasted; }
    public float GetEnergyChargeRatePerSec() { return EnergyChargeRatePerSec; }
    public float GetDamage() { return Damage; }
    public float GetDamageReceived() { return DamageReceived; }
    public float GetDamageCritic() { return DamageCritic; }
    public float GetDamageEvaded() { return DamageEvaded; }
    public int GetKills() { return Kills; }
    public int GetDeploys() { return Deploys; }
    public int GetSecRemaining() { return SecRemaining; }
    public int GetScore() { return Score; }
    
    #endregion
}
