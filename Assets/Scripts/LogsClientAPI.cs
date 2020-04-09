using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LogsClientAPI : MonoBehaviour
{
    public string Host { get; set; }

    private string response;
    public int ID { get; private set; }

    public bool Check { get; private set; }
    private void Start()
    {
        Host = "https://newlogsserverapi20200402121056.azurewebsites.net/api/";
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    }

    public IEnumerator OnCheck(string roomName)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Host +"Rooms/" + roomName))
        {
            yield return webRequest.SendWebRequest();
            response = webRequest.downloadHandler.text;
            var sub = response.Substring(2, 7);
            if (sub == "Message")
                Check = false;
            else Check = true;
        }
    }
        IEnumerator GetRequest(string uri)
        {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Host + uri))
        {
            yield return webRequest.SendWebRequest();
            response = webRequest.downloadHandler.text;
        }
        }

    public IEnumerator GetID(string entity, string identify)
    {
        yield return StartCoroutine(GetRequest(entity + identify));
        switch(entity)
        {
            case "Rooms/":
                Rooms room = Rooms.CreateFromJSON(response);
                ID = room.IDRoom;
                break;
            case "Players/":
                Players player = Players.CreateFromJSON(response);
                ID = player.IDPlayer;
                break;
            case "DirectionVectors/":
                DirectionVectors dv = DirectionVectors.CreateFromJSON(response);
                ID = dv.IDDirectionVector;
                break;
            case "GameTimes/":
                GameTimes gt = GameTimes.CreateFromJSON(response);
                ID = gt.IDGameTime;
                Debug.Log("Time: " + gt.GameTime.ToString());
                break;
        }
    }

    IEnumerator PostRequest(string entity, WWWForm param)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(Host + entity, param))
        {
            yield return www.SendWebRequest();
        }
    }
    public IEnumerator PostBall(float posX, float posY, float velocity, int gametime)
    {
        WWWForm param = new WWWForm();
        param.AddField("PosX", posX.ToString());
        param.AddField("PosY", posY.ToString());
        param.AddField("Velocity", velocity.ToString());
        param.AddField("IDGameTime", gametime);
        yield return StartCoroutine(PostRequest("Balls", param));
    }
    public IEnumerator PostDirectVector(float posX, float posY)
    {
        WWWForm param = new WWWForm();
        param.AddField("PosX", posX.ToString());
        param.AddField("PosY", posY.ToString());
        yield return StartCoroutine(PostRequest("DirectionVectors", param));
    }
    public IEnumerator PostGameTime(float gameTime, int roomID)
    {
        WWWForm param = new WWWForm();
        param.AddField("GameTime1", gameTime.ToString());
        param.AddField("IDRoom", roomID);
        Debug.Log(gameTime);
        yield return StartCoroutine(PostRequest("GameTimes", param));
    }
    public IEnumerator PostImpact(int gameTimeID, int dirVectID, int force, int playerID, int roomID)
    {
        WWWForm param = new WWWForm();
        param.AddField("IDGameTime", gameTimeID);
        param.AddField("IDDirectionVector", dirVectID);
        param.AddField("Force", force);
        param.AddField("IDPlayer", playerID);
        param.AddField("IDRoom", roomID);
        yield return StartCoroutine(PostRequest("Impacts", param));
    }
    public IEnumerator PostPlayer(string nickName, int roomID)
    {
        WWWForm param = new WWWForm();
        param.AddField("PlayerName", nickName.ToString());
        param.AddField("IDRoom", roomID);
        yield return StartCoroutine(PostRequest("Players", param));
    }
    public IEnumerator PostRoom(string roomName, int players) 
    {
        WWWForm param = new WWWForm();
        param.AddField("RoomName", roomName);
        param.AddField("Players", players);
        yield return StartCoroutine(PostRequest("Rooms", param));
    }

    private IEnumerator PutRequest(string entity, string param)
    {
        byte[] json = System.Text.Encoding.UTF8.GetBytes(param);
        using (UnityWebRequest www = UnityWebRequest.Put(Host + entity, json))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log("Error While Sending: " + www.error);
            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
            }
        }
    }

    public IEnumerator PutRoom(string roomName)
    {
        yield return StartCoroutine(GetID("Rooms/", roomName));
        Rooms room = new Rooms()
        {
            IDRoom = ID,
            RoomName = roomName,
            Players = 0
        };
        string param = JsonUtility.ToJson(room);
        yield return StartCoroutine(PutRequest("Rooms/"+ ID.ToString(), param));
    }
}
