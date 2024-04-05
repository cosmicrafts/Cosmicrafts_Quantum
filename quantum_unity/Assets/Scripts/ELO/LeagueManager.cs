using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager Instance { get; private set; } // Singleton instance
    public List<League> leagues;
    public Image leagueSpriteImage;
    public TMP_Text leagueNameText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitializeLeagues();
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        EloManagement.Instance.OnLeagueChanged += HandleLeagueChange;
    }

    void OnDisable()
    {
        EloManagement.Instance.OnLeagueChanged -= HandleLeagueChange;
    }

    void OnDestroy()
    {
        // Always good practice to unsubscribe from events to avoid memory leaks.
        EloManagement.Instance.OnLeagueChanged -= UpdateLeagueUI;
    }

    private void HandleLeagueChange(League newLeague)
    {
        // Show UI panel or update existing UI with newLeague information
    }

    private void InitializeLeagues()
    {
        leagues = new List<League>
        {
            // Begin at the middle and expand both ways
            new League { name = "Nebula League: Stardust Pioneers", minElo = int.MinValue, maxElo = 1199, leagueSprite = null},
            new League { name = "Nebula League: Cosmic Sparks", minElo = 1200, maxElo = 1299, leagueSprite = null},
            new League { name = "Nebula League: Nova Travelers", minElo = 1300, maxElo = 1399, leagueSprite = null},

            new League { name = "Asteroid League: Comet Chasers", minElo = 1400, maxElo = 1499, leagueSprite = null},
            new League { name = "Asteroid League: Asteroid Miners", minElo = 1500, maxElo = 1599, leagueSprite = null},
            new League { name = "Asteroid League: Meteor Ascendants", minElo = 1600, maxElo = 1699, leagueSprite = null},

            new League { name = "Moon League: Volcanic Moons", minElo = 1700, maxElo = 1799, leagueSprite = null},
            new League { name = "Moon League: Ice Moons", minElo = 1800, maxElo = 1899, leagueSprite = null},
            new League { name = "Moon League: Terra Moons", minElo = 1900, maxElo = 1999, leagueSprite = null},

            new League { name = "Planetary League: Dwarf Planets", minElo = 2000, maxElo = 2099, leagueSprite = null},
            new League { name = "Planetary League: Rocky Planets", minElo = 2100, maxElo = 2199, leagueSprite = null},
            new League { name = "Planetary League: Gas Giants", minElo = 2200, maxElo = 2299, leagueSprite = null},

            new League { name = "Star League: Red Dwarfs", minElo = 2300, maxElo = 2399, leagueSprite = null},
            new League { name = "Star League: Blue Giants", minElo = 2400, maxElo = 2499, leagueSprite = null},
            new League { name = "Star League: Supernova", minElo = 2500, maxElo = 2599, leagueSprite = null},

            new League { name = "Galactic League: Dwarf Galaxies", minElo = 2600, maxElo = 2699, leagueSprite = null},
            new League { name = "Galactic League: Spiral Galaxies", minElo = 2700, maxElo = 2799, leagueSprite = null},
            new League { name = "Galactic League: Quasar", minElo = 2800, maxElo = 2899, leagueSprite = null},

            new League { name = "Event Horizon League: Singularity Titans", minElo = 2900, maxElo = int.MaxValue, leagueSprite = null},
        };
    }

    public League GetCurrentLeague(double elo)
    {
        return leagues.FirstOrDefault(l => elo >= l.minElo && elo <= l.maxElo);
    }

    private void UpdateLeagueUI(League newLeague)
    {
        if (newLeague != null)
        {
            leagueNameText.text = newLeague.name;
            leagueSpriteImage.sprite = newLeague.leagueSprite;
        }
    }
}
