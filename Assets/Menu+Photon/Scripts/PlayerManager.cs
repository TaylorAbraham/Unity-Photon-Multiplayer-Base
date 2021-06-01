using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour {
  PhotonView pv;

  private void Awake() {
    pv = GetComponent<PhotonView>();
  }

  private void Start() {
    if (pv.IsMine) {
      CreateController();
    }
  }

  private void CreateController() {
    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
  }
}
