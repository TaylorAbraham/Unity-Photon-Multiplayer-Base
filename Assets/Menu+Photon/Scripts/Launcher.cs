using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks {
  public static Launcher Instance;

  [SerializeField] TMP_InputField roomNameInputField;
  [SerializeField] TMP_Text roomNameText;
  [SerializeField] TMP_Text errorText;
  [SerializeField] Transform roomListContent;
  [SerializeField] GameObject roomListItemPrefab;
  [SerializeField] Transform playerListContent;
  [SerializeField] GameObject playerListItemPrefab;
  [SerializeField] GameObject startGameButton;

  private void Awake() {
    Instance = this;
  }

  private void Start() {
    Debug.Log("Connecting to master...");
    PhotonNetwork.ConnectUsingSettings();
  }

  public override void OnConnectedToMaster() {
    Debug.Log("Connected to master!");
    PhotonNetwork.JoinLobby();
    // Automatically load scene for all clients when the host loads a scene
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  public override void OnJoinedLobby() {
    MenuManager.Instance.OpenMenu("title");
    Debug.Log("Joined lobby");
    PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString();
  }

  public void CreateRoom() {
    if (!string.IsNullOrEmpty(roomNameInputField.text)) {
      PhotonNetwork.CreateRoom(roomNameInputField.text);
      MenuManager.Instance.OpenMenu("loading");
      roomNameInputField.text = "";
    } else {
      Debug.Log("No room name entered");
    }
  }

  public override void OnJoinedRoom() {
    // Called whenever you create or join a room
    MenuManager.Instance.OpenMenu("room");
    roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    Player[] players = PhotonNetwork.PlayerList;
    foreach (Transform trans in playerListContent) {
      Destroy(trans.gameObject);
    }
    for (int i = 0; i < players.Count(); i++) {
      Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
    }
    // Only enable the start button if the player is the host of the room
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public override void OnMasterClientSwitched(Player newMasterClient) {
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public void LeaveRoom() {
    PhotonNetwork.LeaveRoom();
    MenuManager.Instance.OpenMenu("loading");
  }

  public void JoinRoom(RoomInfo info) {
    PhotonNetwork.JoinRoom(info.Name);
    MenuManager.Instance.OpenMenu("loading");
  }

  public override void OnLeftRoom() {
    MenuManager.Instance.OpenMenu("title");
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList) {
    foreach (Transform trans in roomListContent) {
      Destroy(trans.gameObject);
    }
    for (int i = 0; i < roomList.Count; i++) {
      if (roomList[i].RemovedFromList) {
        // Don't instantiate stale rooms
        continue;
      }
      Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
    }
  }

  public override void OnCreateRoomFailed(short returnCode, string message) {
    errorText.text = "Room Creation Failed: " + message;
    MenuManager.Instance.OpenMenu("error");
  }

  public override void OnPlayerEnteredRoom(Player newPlayer) {
    Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
  }

  public void StartGame() {
    // 1 is used as the build index of the game scene, defined in the build settings
    // Use this instead of scene management so that *everyone* in the lobby goes into this scene
    PhotonNetwork.LoadLevel(1);
  }
}
