using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks {
  public static RoomManager Instance;

  private void Awake() {
    if (Instance) {
      Destroy(gameObject);
      return;
    }
    DontDestroyOnLoad(gameObject);
    Instance = this;
  }

  public override void OnEnable() {
    base.OnEnable();
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  public override void OnDisable() {
    base.OnDisable();
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
    if (scene.buildIndex == 1) {
      // This is the game scene
      PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
  }
}
