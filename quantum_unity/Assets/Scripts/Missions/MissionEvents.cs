using System;

public static class MissionEvents
{
    public static event Action OnMissionsFetched;

    public static void RaiseMissionsFetched()
    {
        OnMissionsFetched?.Invoke();
    }
}
