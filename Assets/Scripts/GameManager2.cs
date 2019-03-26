using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager2 : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private string roomName = "TestRoom";

    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name) {
        roomName = _name;
    }

    public void CreateRoom() {
        Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");

        networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
    }

}
