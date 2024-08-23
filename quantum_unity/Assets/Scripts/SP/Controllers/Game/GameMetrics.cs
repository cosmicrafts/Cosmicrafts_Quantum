namespace CosmicraftsSP {
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
        Kills = 0;
        Deploys = 0;
        SecRemaining = 0;

        Score = 0;
    }

    //Calculate final metrics when game ends
    public void CalculateLastMetrics(Team winner)
    {
        EnergyWasted += GameMng.P.CurrentEnergy;
        EnergyChargeRatePerSec = GameMng.P.SpeedEnergy;
        SecRemaining = GameMng.GM.GetRemainingSecs();
    }

    //Add energy used
    public void AddEnergyUsed(float value)
    {
        EnergyUsed += value;
    }

    //Add energy generated
    public void AddEnergyGenerated(float value)
    {
        EnergyGenerated += value;
    }

    //Add energy generated when players energy is full
    public void AddEnergyWasted(float value)
    {
        EnergyWasted += value;
    }

    //Damage done
    public void AddDamage(float value)
    {
        Damage += value;
        Score += (int)value;
    }

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
    public float GetEnergyUsed()
    {
        return EnergyUsed;
    }
    public float GetEnergyGenerated()
    {
        return EnergyGenerated;
    }
    public float GetEnergyWasted()
    {
        return EnergyWasted;
    }
    public float GetEnergyChargeRatePerSec()
    {
        return EnergyChargeRatePerSec;
    }
    public float GetDamage()
    {
        return Damage;
    }
    public int GetKills()
    {
        return Kills;
    }
    public int GetDeploys()
    {
        return Deploys;
    }
    public int GetSecRemaining()
    {
        return SecRemaining;
    }
    public int GetScore()
    {
        return Score;
    }
    #endregion
}
}