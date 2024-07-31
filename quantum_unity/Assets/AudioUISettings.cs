using TowerRush;
using UnityEngine;
using UnityEngine.UI;

public class AudioUISettings : MonoBehaviour
{
    [SerializeField] Slider m_MusicSlider;
    [SerializeField] Slider m_SoundsSlider;

    private void Start()
    {
        m_MusicSlider.onValueChanged.AddListener(OnMusicSlider);
        m_SoundsSlider.onValueChanged.AddListener(OnSoundsSlider);

        m_MusicSlider.value  = GameOptions.MusicVolume  * 10;
        m_SoundsSlider.value = GameOptions.SoundsVolume * 10;
    }
    
    private void OnMusicSlider(float value)
    {
        value *= 0.1f;

        Game.Instance.AudioService.SetMusicVolume(value);

        GameOptions.MusicVolume = value;
        GameOptions.Save();
    }

    private void OnSoundsSlider(float value)
    {
        value *= 0.1f;

        Game.Instance.AudioService.SetSoundsVolume(value);

        GameOptions.SoundsVolume = value;
        GameOptions.Save();
    }
    
    
    
}
