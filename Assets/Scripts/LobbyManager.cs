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

    [SerializeField] private Button redBulletButton;
    [SerializeField] private Button blueBulletButton;
    [SerializeField] private Button greenBulletButton;

    [SerializeField] private Button redPlayerButton;
    [SerializeField] private Button bluePlayerButton;
    [SerializeField] private Button greenPlayerButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.onClick.AddListener(OnStartButtonClicked);

        slowSpeedButton.onClick.AddListener(() => SetShootingSpeed(1.5f));
        mediumSpeedButton.onClick.AddListener(() => SetShootingSpeed(3f));
        fastSpeedButton.onClick.AddListener(() => SetShootingSpeed(5f));

        redBulletButton.onClick.AddListener(() => SetBulletColor(Color.red));
        blueBulletButton.onClick.AddListener(() => SetBulletColor(Color.blue));
        greenBulletButton.onClick.AddListener(() => SetBulletColor(Color.green));

        redPlayerButton.onClick.AddListener(() => SetPlayerColor(Color.red));
        bluePlayerButton.onClick.AddListener(() => SetPlayerColor(Color.blue));
        greenPlayerButton.onClick.AddListener(() => SetPlayerColor(Color.green));
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
        GameData.shootingSpeed = speed;
    }

    private void SetBulletColor(Color color)
    {
        GameData.bulletColor = color;
    }

    private void SetPlayerColor(Color color)
    {
        GameData.playerColor = color;
    }
}
