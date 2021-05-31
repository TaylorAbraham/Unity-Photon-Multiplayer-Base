using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour {
  PhotonView PV;

  private void Awake() {
    PV = GetComponent<PhotonView>();
  }

  private void Start() {
    if (PV.IsMine) {
      CreateController();
    }
  }

  private void CreateController() {
    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
  }
}
