using System;

[Serializable]
public class RoomInfo
{
    public string roomId;
    public string roomName;
    public int currentCount;
    public int maxPlayerCount;
    public string captainName;

    public RoomInfo(string roomId, string roomName, int currentCount, int maxPlayerCount, string captainName)
    {
        this.roomId = roomId;
        this.roomName = roomName;
        this.currentCount = currentCount;
        this.maxPlayerCount = maxPlayerCount;
        this.captainName = captainName;
    }
}

