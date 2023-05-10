using System;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileGroup : MonoBehaviour
  {
    public delegate void SelectHandler(string name);
    public event SelectHandler OnSelect;

    public ProfileView Profile;
    public DeleteView Delete;

    [HideInInspector] public string ProfileName;

    public void SetProfile(string profileName)
    {
      ProfileName = profileName;
      Profile.Text.text = profileName;
    }

    public void DeleteProfile()
    {
      OnSelect?.Invoke(ProfileName);
      Destroy(gameObject);
    }
  }
}