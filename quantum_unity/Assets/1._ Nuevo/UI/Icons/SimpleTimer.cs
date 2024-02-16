using UnityEngine;
using TMPro;

public class SimpleTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isActive = false;

    void OnEnable()
    {
        StartTimer();
    }

    void OnDisable()
    {
        StopTimer();
        ResetTimer();
    }

    void Update()
    {
        if (isActive)
        {
            UpdateTimer();
        }
    }

    void StartTimer()
    {
        isActive = true;
        startTime = Time.time;
        Debug.Log("Timer started");
    }

    void StopTimer()
    {
        isActive = false;
    }

    void ResetTimer()
    {
        if (timerText != null)
            timerText.text = "0:00";
        Debug.Log("Timer reset");
    }

    void UpdateTimer()
    {
        float time = Time.time - startTime;
        string minutes = ((int)time / 60).ToString();
        string seconds = ((int)time % 60).ToString("00");

        if (timerText != null)
            timerText.text = minutes + ":" + seconds;
    }

}
