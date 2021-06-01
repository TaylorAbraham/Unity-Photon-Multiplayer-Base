using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
  [SerializeField] private float playerSpeed = 2.0f;
  [SerializeField] private float jumpHeight = 1.0f;
  [SerializeField] private float gravityValue = -9.81f;

  private CharacterController controller;
  private PhotonView pv;
  private Vector3 playerVelocity;
  private bool groundedPlayer;
  private Transform cameraTransform;

  private void Start() {
    controller = GetComponent<CharacterController>();
    pv = GetComponent<PhotonView>();

    if (!pv.IsMine) {
      Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
      Destroy(GetComponentInChildren<CharacterController>());
      Destroy(GetComponentInChildren<Rigidbody>());
    }

    Cursor.visible = false;
    cameraTransform = Camera.main.transform;
  }

  void Update() {
    if (!pv.IsMine) {
      // This isn't our player, so ignore controls and let Photon update their movement
      return;
    }
    groundedPlayer = controller.isGrounded;
    if (groundedPlayer && playerVelocity.y < 0) {
      playerVelocity.y = 0f;
    }

    Vector2 movement = InputManager.Instance.GetPlayerMovement();
    Vector3 move = new Vector3(movement.x, 0f, movement.y);
    move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
    move.y = 0f; // Y should always be 0 in a move, as jump is handled afterwards
    controller.Move(move * Time.deltaTime * playerSpeed);

    // Changes the height position of the player..
    if (InputManager.Instance.playerJumpedThisFrame() && groundedPlayer) {
      playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    playerVelocity.y += gravityValue * Time.deltaTime;
    controller.Move(playerVelocity * Time.deltaTime);
  }
}
