// File: MultiplayerManager.cs
// Contributors: Brendan Robinson
// Date Created: 03/26/2019
// Date Last Modified: 03/26/2019

using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerManager : MonoBehaviour {

    #region Constants

    public static MultiplayerManager instance;

    #endregion

    #region Fields

    private NetworkManager networkManager;

    #endregion

    #region Methods

    private void Start() {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        //DontDestroyOnLoad(gameObject);

        networkManager = NetworkManager.singleton;
        networkManager.StartMatchMaker();
    }

    private void Update() { }

    #endregion

}