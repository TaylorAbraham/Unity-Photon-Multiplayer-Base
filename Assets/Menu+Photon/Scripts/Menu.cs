using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
  public string menuName;
  public bool open;

  private void Start() {
    gameObject.SetActive(open);
  }

  public void Open() {
    open = true;
    gameObject.SetActive(true);
  }

  public void Close() {
    open = false;
    gameObject.SetActive(false);
  }
}
