namespace CosmicraftsSP
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /*
     * This is the in-game UI controller
     * Contains the UI references and functions for update them
     * Only controls the game's data, the UI for the players and the tutorial are in other scripts
     */
    public class UIGameMng : MonoBehaviour
    {
        //Screens objects references
        public GameObject VictoryScreen;
        public GameObject DefeatScreen;
        public GameObject ResultsScreen;

        //UI modules objects references
        public GameObject TopMidInfo;
        public GameObject DeckPanel;

        //Cards objects references
        public UIGameCard[] UIDeck = new UIGameCard[8];

        //Time, energy number and energy bar references
        public TMP_Text TimeOut;
        public TMP_Text EnergyLabel;
        public Image EnergyBar;

        //Trigger grid for deploy cards
        public GameObject AreaDeploy;

        //Results Metrics text references
        public TMP_Text MTxtEnergyUsed;
        public TMP_Text MTxtEnergyGenerated;
        public TMP_Text MTxtEnergyWasted;
        public TMP_Text MTxtEnergyChargeRatePerSec;
        public TMP_Text MTxtXpEarned;
        public TMP_Text MTxtDamage;
        public TMP_Text MTxtDeploys;
        public TMP_Text MTxtKills;
        public TMP_Text MTxtSecRemaining;
        public TMP_Text MTxtScore;

        //Players panels references
        public UIPlayerGameInfo[] Players = new UIPlayerGameInfo[2];

        //HP and Shield Colors for every team
        Color FriendHpBarColor;
        Color FriendShieldBarColor;

        Color EnemyHpBarColor;
        Color EnemyShieldBarColor;

        private void Awake()
        {
            // Set the UI controller
            GameMng.UI = this;

            // Init the HP and shield colors
            FriendHpBarColor = new Color(0.25f, 1f, 0.28f, 1f);
            FriendShieldBarColor = new Color(0.25f, 0.66f, 1f, 1f);
            EnemyHpBarColor = new Color(1f, 0.25f, 0.25f, 1f);
            EnemyShieldBarColor = new Color(1f, 0.8f, 0.25f, 1f);
        }

        private void Start()
        {
            // Hardcoded player data for now
            UserGeneral hardcodedPlayerData = new UserGeneral
            {
                NikeName = "Player1",
                WalletId = "Wallet1",
                Level = 10,
                Xp = 2000,
                Avatar = 1
            };

            UserProgress hardcodedPlayerProgress = new UserProgress
            {
                // Fill with appropriate hardcoded values
            };

            NFTsCharacter hardcodedPlayerCharacter = new NFTsCharacter
            {
                // Fill with appropriate hardcoded values
            };

            // Init the UI info of the player with hardcoded values
            Players[GameMng.P.ID - 1].InitInfo(hardcodedPlayerData, hardcodedPlayerProgress, hardcodedPlayerCharacter);
        }

        // Shows the game over screen
        public void SetGameOver(Team winner)
        {
            TopMidInfo.SetActive(false);
            DeckPanel.SetActive(false);

            ResultsScreen.SetActive(true);
            if (winner == GameMng.P.MyTeam)
            {
                VictoryScreen.SetActive(true);
            }
            else
            {
                DefeatScreen.SetActive(true);
            }

            GameMng.MT.CalculateLastMetrics(winner);
            UpdateResults();
        }

        // Init the UI Cards
        public void InitGameCards(NFTsCard[] nftCard)
        {
            for (int i = 0; i < nftCard.Length; i++)
            {
                UIDeck[i].SpIcon.sprite = nftCard[i].IconSprite;
                UIDeck[i].EnergyCost = nftCard[i].EnergyCost;
                UIDeck[i].TextCost.text = nftCard[i].EnergyCost.ToString();
            }
        }

        // Update the UI time
        public void UpdateTimeOut(string newtime)
        {
            TimeOut.text = newtime;
        }

        // Shows a card as selected
        public void SelectCard(int idc)
        {
            UIDeck[idc].SetSelection(true);
            AreaDeploy.SetActive(true);
        }

        // Deselects all cards
        public void DeselectCards()
        {
            foreach (UIGameCard card in UIDeck)
            {
                card.SetSelection(false);
            }
            AreaDeploy.SetActive(false);
        }

        // Update the energy bar and text
        public void UpdateEnergy(float energy, float max)
        {
            EnergyLabel.text = ((int)energy).ToString(energy == max ? "F0" : "F0");
            EnergyBar.fillAmount = energy / max;

            foreach (UIGameCard card in UIDeck)
            {
                card.TextCost.color = energy >= card.EnergyCost ? Color.white : Color.red;
            }
        }

        // Update the results text panel with the game metrics
        public void UpdateResults()
        {
            MTxtEnergyUsed.text = GameMng.MT.GetEnergyUsed().ToString();
            MTxtEnergyGenerated.text = GameMng.MT.GetEnergyGenerated().ToString("F0");
            MTxtEnergyWasted.text = GameMng.MT.GetEnergyWasted().ToString("F0");
            MTxtEnergyChargeRatePerSec.text = GameMng.MT.GetEnergyChargeRatePerSec().ToString() + "/s";

            MTxtXpEarned.text = "+" + GameMng.MT.GetScore().ToString();

            MTxtDamage.text = GameMng.MT.GetDamage().ToString();
            MTxtKills.text = GameMng.MT.GetKills().ToString();
            MTxtDeploys.text = GameMng.MT.GetDeploys().ToString();
            MTxtSecRemaining.text = GameMng.MT.GetSecRemaining().ToString() + " s";

            MTxtScore.text = GameMng.MT.GetScore().ToString();
        }

        // Returns the HP color for a unit
        public Color GetHpBarColor(bool isEnnemy)
        {
            return isEnnemy ? EnemyHpBarColor : FriendHpBarColor;
        }

        // Returns the Shield color for a unit
        public Color GetShieldBarColor(bool isEnnemy)
        {
            return isEnnemy ? EnemyShieldBarColor : FriendShieldBarColor;
        }
    }
}
