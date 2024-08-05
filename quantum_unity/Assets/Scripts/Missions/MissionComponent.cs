using TMPro;
using UnityEngine;

public class MissionComponent : MonoBehaviour
{
    public MissionData mission;
    public TMP_Text descriptionText;
    public TMP_Text progressText;

    private void OnEnable()
    {
        MissionEvents.OnMissionUpdated += UpdateUI;
        MissionEvents.OnMissionCompleted += UpdateUI;
        UpdateUI(mission);
    }

    private void OnDisable()
    {
        MissionEvents.OnMissionUpdated -= UpdateUI;
        MissionEvents.OnMissionCompleted -= UpdateUI;
    }

    private void UpdateUI(MissionData updatedMission)
    {
        if (mission == updatedMission)
        {
            descriptionText.text = mission.missionType.ToString();
            progressText.text = $"Progress: {mission.progress}/{mission.total}";
            if (mission.finished)
            {
                progressText.text += " - Completed!";
            }
        }
    }
}
