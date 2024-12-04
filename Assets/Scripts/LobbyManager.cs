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

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.onClick.AddListener(OnClicked);
    }


    private void OnClicked()
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

        PhotonNetwork.JoinOrCreateRoom("Room1", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameplayScene");
        }

    }
}
