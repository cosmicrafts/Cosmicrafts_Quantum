using UnityEngine;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData", order = 1)]
public class MissionData : ScriptableObject
{
    public ulong expiration;
    public ulong finishDate;
    public bool finished;
    public int idMission;
    public MissionType missionType;
    public int progress;
    public int rewardAmount;
    public MissionRewardType rewardType;
    public ulong startDate;
    public int total;

    public void UpdateProgress(int newProgress)
    {
        progress = newProgress;
        if (progress >= total)
        {
            CompleteMission();
        }
        else
        {
            MissionEvents.OnMissionUpdated?.Invoke(this);
        }
    }

    public void CompleteMission()
    {
        finished = true;
        MissionEvents.OnMissionCompleted?.Invoke(this);
    }
}