using System;

public static class MissionEvents
{
    public static Action<MissionData> OnMissionUpdated;
    public static Action<MissionData> OnMissionCompleted;
}
