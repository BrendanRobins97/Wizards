// File: MultiplayerSetup.cs
// Author: Brendan Robinson
// Date Created: 03/23/2019
// Date Last Modified: 03/23/2019

using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetup : MonoBehaviour {

    #region Fields

    [SerializeField] private uint roomSize = 6;

    private string roomName = "TestRoom";

    private NetworkManager networkManager;

    #endregion

    #region Methods

    private void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) { networkManager.StartMatchMaker(); }
    }

    public void SetRoomName(string name) { roomName = name; }

    public void CreateRoom() {
        Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");

        networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
    }

    #endregion

}