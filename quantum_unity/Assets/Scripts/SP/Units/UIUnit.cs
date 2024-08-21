namespace CosmicraftsSP
{
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;

    /*
     * This script manages the UI of the ships and stations
     * Shows HP Bars, Shields, etc.
     */
    public class UIUnit : MonoBehaviour
    {
        // The main game object canvas reference
        public GameObject Canvas;

        // HP bar image
        public Image Hp;
        public Image GHp;

        // Shield bar image
        public Image Shield;
        public Image GShield;

        public float DifDmgSpeed = 10f;
        public Color DifHpColor = Color.yellow;
        public Color DifShieldColor = Color.gray;

        // Reference to the TMP text to display the level
        public TextMeshProUGUI LevelText;

        // Reference to the Animation component
        public Animation Animation;

        // The main camera of the game
        Camera mainCamera;
        Quaternion originalRotation;

        float GhostHp;
        float GhostSH;

        // Variable to store previous HP state for comparison
        private float previousHp;
        private float previousShield;

        // Boolean to track if the animation has already been triggered
        private bool animationTriggered = false;

        void Start()
        {
            originalRotation = transform.rotation;
            mainCamera = Camera.main;

            // Get the Unit component
            Unit unit = GetComponentInParent<Unit>();
            
            // Check if the unit belongs to the Blue team and flip the UI
            if (unit != null && unit.MyTeam == Team.Blue)
            {
                // Flip the UI horizontally by inverting the X scale
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(-Mathf.Abs(rectTransform.localScale.x), rectTransform.localScale.y, rectTransform.localScale.z);
            }

            // Set the level text
            if (unit != null && LevelText != null)
            {
                LevelText.text = unit.GetLevel().ToString();
            }

            // Initialize previousHp and previousShield with current values
            previousHp = Hp.fillAmount;
            previousShield = Shield.fillAmount;
        }

        private void Update()
        {
            // The UI always looks at the camera
            transform.rotation = mainCamera.transform.rotation * originalRotation;

            // Detect damage by comparing the current HP and Shield with the previous state
            if (!animationTriggered && (Hp.fillAmount < previousHp || Shield.fillAmount < previousShield))
            {
                // Trigger animation only once when damage is detected
                OnDamageTaken();
                animationTriggered = true; // Set flag to true to prevent further triggers
            }

            // Lerp Ghost Bars
            GhostHp = Mathf.Lerp(GhostHp, Hp.fillAmount, Time.deltaTime * DifDmgSpeed);
            GhostSH = Mathf.Lerp(GhostSH, Shield.fillAmount, Time.deltaTime * DifDmgSpeed);
            GHp.fillAmount = GhostHp;
            GShield.fillAmount = GhostSH;

            // Update previous state for the next frame
            previousHp = Hp.fillAmount;
            previousShield = Shield.fillAmount;
        }

        public void Init(int maxhp, int maxshield)
        {
            GhostHp = maxhp;
            GhostSH = maxshield;
            GHp.color = DifHpColor;
            GShield.color = DifShieldColor;
        }

        public void SetHPBar(float percent)
        {
            Hp.fillAmount = percent;
        }

        public void SetShieldBar(float percent)
        {
            Shield.fillAmount = percent;
        }

        public void SetColorBars(bool imEnnemy)
        {
            Hp.color = GameMng.UI.GetHpBarColor(imEnnemy);
            Shield.color = GameMng.UI.GetShieldBarColor(imEnnemy);
        }

        public void HideUI()
        {
            Canvas.SetActive(false);
        }

        // Method to trigger the animation when the unit takes damage
        public void OnDamageTaken()
        {
            Debug.Log("Damage taken - triggering animation");
            if (Animation != null && Animation["ShowBars"] != null)
            {
                Animation.Play("ShowBars");
            }
            else
            {
                Debug.LogWarning("Animation or animation clip not assigned in the inspector.");
            }
        }
    }
}
