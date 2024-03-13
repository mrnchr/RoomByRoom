using System;
using UnityEngine;

namespace RoomByRoom
{
  [Serializable]
  public struct Controller
  {
    public KeyCode Forward;
    public KeyCode Back;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Jump;
    public KeyCode Attack;
    public KeyCode Take;
    public KeyCode OpenDoor;
    public KeyCode Inventory;
    public KeyCode Pause;
    [HideInInspector] public KeyCode CameraUp;
    [HideInInspector] public KeyCode CameraDown;
    [HideInInspector] public KeyCode CameraLeft;
    [HideInInspector] public KeyCode CameraRight;
  }
}