using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private string sceneName;

    [SerializeField] private Button slowSpeedButton;
    [SerializeField] private Button mediumSpeedButton;
    [SerializeField] private Button fastSpeedButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.onClick.AddListener(OnStartButtonClicked);

        slowSpeedButton.onClick.AddListener(() => SetShootingSpeed(1.5f)); //
        mediumSpeedButton.onClick.AddListener(() => SetShootingSpeed(3f)); 
        fastSpeedButton.onClick.AddListener(() => SetShootingSpeed(5f)); 
    }

    private void OnStartButtonClicked()
    {
        GameData.playerName = playerNameInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom("Room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    private void SetShootingSpeed(float speed)
    {
        GameData.shootingSpeed = speed; //
    }
}
