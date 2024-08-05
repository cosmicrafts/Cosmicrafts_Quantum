using UnityEngine;
using EdjCase.ICP.Candid.Models;
using CanisterPK.CanisterLogin.Models;

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/MissionData", order = 1)]
public class MissionData : ScriptableObject
{
    public ulong expiration;
    public ulong finishDate;
    public bool finished;
    public UnboundedUInt idMission;
    public MissionType missionType;
    public UnboundedUInt progress;
    public UnboundedUInt rewardAmount;
    public MissionRewardType rewardType;
    public ulong startDate;
    public UnboundedUInt total;

    public void UpdateProgress(UnboundedUInt newProgress)
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
