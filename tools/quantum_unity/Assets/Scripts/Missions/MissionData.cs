using UnityEngine;
using Cosmicrafts.MainCanister.Models;

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

}
