using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rooms
{
    public int IDRoom;
    public string RoomName;
    public int Players;

    public static Rooms CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Rooms>(jsonString);
    }
}
[System.Serializable]
public class Players
{
    public int IDPlayer, IDRoom;
    [System.NonSerialized]
    public string PlayerName;

    public static Players CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Players>(jsonString);
    }
}
[System.Serializable]
public class DirectionVectors
{
    public int IDDirectionVector;
    public float PosX, PosY;

    public static DirectionVectors CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<DirectionVectors>(jsonString);
    }
}
[System.Serializable]
public class GameTimes
{
    public int IDGameTime, IDRoom;
    public float GameTime;

    public static GameTimes CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameTimes>(jsonString);
    }
}

