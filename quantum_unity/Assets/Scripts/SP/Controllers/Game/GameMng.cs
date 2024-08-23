namespace CosmicraftsSP
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    public class GameMng : MonoBehaviour
    {
        public static GameMng GM;
        public static Player P;
        public static GameMetrics MT;
        public static UIGameMng UI;
        public BotEnemy BOT;

        public GameObject BotPrefab; // Prefab for bot instantiation
        public Vector3[] BS_Positions; // Base stations positions

        private List<Unit> units = new List<Unit>();
        private List<Spell> spells = new List<Spell>();
        private int idCounter = 0;
        private Dictionary<string, NFTsUnit> allPlayersNfts = new Dictionary<string, NFTsUnit>();

        public Unit[] Targets = new Unit[2]; // Array for managing base stations
        bool GameOver = false;

        // Time variables
        private TimeSpan timeOut;
        private DateTime startTime;

        private void Awake()
        {
            Debug.Log("--GAME MANAGER AWAKE--");

            // Init static unique controllers
            GM = this;

            Debug.Log("--GAME VARIABLES READY--");

            // Instantiate BOT if necessary
            if (BotPrefab != null)
            {
                BOT = Instantiate(BotPrefab).GetComponent<BotEnemy>();
                Debug.Log("--BOT INSTANTIATED--");
            }

            MT = new GameMetrics();
            MT.InitMetrics();
        }

        private void Start()
        {
            Debug.Log("--GAME MANAGER START--");

            BOT = Instantiate(BotPrefab).GetComponent<BotEnemy>();

            Debug.Log("--GAME MANAGER READY--");
        }

        public Unit InitBaseStations(GameObject baseStationPrefab)
        {
            int playerBaseIndex = P.MyTeam == Team.Blue ? 1 : 0;
            int botBaseIndex = P.MyTeam == Team.Red ? 1 : 0;

            Unit playerBaseStation = null;
            playerBaseStation = Instantiate(baseStationPrefab, BS_Positions[playerBaseIndex], Quaternion.identity).GetComponent<Unit>();
            Targets[playerBaseIndex] = playerBaseStation;

                GameObject botBaseStation = BotPrefab.GetComponent<BotEnemy>().prefabBaseStation;
                    Targets[botBaseIndex] = Instantiate(botBaseStation, BS_Positions[botBaseIndex], Quaternion.identity).GetComponent<Unit>();
                    Targets[botBaseIndex].PlayerId = 2;
                    Targets[botBaseIndex].MyTeam = Team.Red;

            // Set the IDs of the base stations
            for (int i = 0; i < Targets.Length; i++)
            {
                Targets[i].setId(GenerateUnitId());
            }

            return playerBaseStation;
        }

        public Unit CreateUnit(GameObject obj, Vector3 position, Team team, string nftKey = "none", int playerId = -1)
        {
            Unit unit = Instantiate(obj, position, Quaternion.identity).GetComponent<Unit>();
            unit.MyTeam = team;
            unit.PlayerId = playerId == -1 ? P.ID : playerId;
            unit.setId(GenerateUnitId());
            unit.SetNfts(GetNftCardData(nftKey, unit.PlayerId) as NFTsUnit);
            return unit;
        }

        public Spell CreateSpell(GameObject obj, Vector3 position, Team team, string nftKey = "none")
        {
            Spell spell = Instantiate(obj, position, Quaternion.identity).GetComponent<Spell>();
            spell.MyTeam = team;
            spell.setId(GenerateUnitId());
            AddSpell(spell);
            return spell;
        }

        public void AddUnit(Unit unit)
        {
            if (!units.Contains(unit))
            {
                units.Add(unit);
            }
        }

        public void AddSpell(Spell spell)
        {
            if (!spells.Contains(spell))
            {
                spells.Add(spell);
            }
        }

        public void DeleteUnit(Unit unit)
        {
            if (units.Contains(unit))
            {
                units.Remove(unit);
                Destroy(unit.gameObject); // Ensure the unit's GameObject is destroyed
            }
        }

        public void DeleteSpell(Spell spell)
        {
            if (spells.Contains(spell))
            {
                spells.Remove(spell);
                Destroy(spell.gameObject); // Ensure the spell's GameObject is destroyed
            }
        }

        public void KillUnit(Unit unit)
        {
            if (unit != null)
            {
                unit.Die();
                DeleteUnit(unit); // Ensure the unit is removed from the list
            }
        }

        public void EndGame(Team winner)
        {
            GameOver = true;
            Debug.Log($"Game Over! {winner} team wins!");
            UI.SetGameOver(winner);  // Update the UI with the game over status
        }

        public Color GetColorUnit(Team team, int playerId)
        {
            // Return appropriate color based on team and player ID
            return team == Team.Blue ? Color.blue : Color.red;
        }

        public Vector3 GetDefaultTargetPosition(Team team)
        {
            // Return the default target position based on team
            int index = team == Team.Blue ? 0 : 1;
            return BS_Positions[index];
        }

        public Transform GetFinalTransformTarget(Team team)
        {
            if (GameOver)
                return transform;

            return Targets[(int)team].transform;
        }

        public bool IsGameOver()
        {
            return GameOver;
        }

        public bool MainStationsExist()
        {
            // Check if both main stations (Targets) exist
            return Targets[0] != null && Targets[1] != null;
        }

        private int GenerateUnitId()
        {
            idCounter++;
            return idCounter;
        }

        public void AddNftCardData(NFTsUnit nFTsCard, int playerId)
        {
            // Implement the logic to store or manage the NFTs data associated with a player.
            string finalKey = $"{playerId}_{nFTsCard.KeyId}";
            if (!allPlayersNfts.ContainsKey(finalKey))
            {
                allPlayersNfts.Add(finalKey, nFTsCard);
            }
        }

        public int CountUnits()
        {
            return units.Count;
        }

        public int CountUnits(Team team)
        {
            return units.Where(f => f.IsMyTeam(team)).Count();
        }

        private NFTsUnit GetNftCardData(string nftKey, int playerId)
        {
            string finalKey = $"{playerId}_{nftKey}";
            return allPlayersNfts.ContainsKey(finalKey) ? allPlayersNfts[finalKey] : null;
        }

        public int GetRemainingSecs()
        {
            TimeSpan currentTime = timeOut.Add(startTime - DateTime.Now);
            return Mathf.Max(0, (int)currentTime.TotalSeconds);
        }
    }
}
