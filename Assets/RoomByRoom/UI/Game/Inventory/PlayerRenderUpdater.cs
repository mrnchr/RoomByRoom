using Leopotam.EcsLite;
using UnityEngine;

namespace RoomByRoom.UI.Game.Inventory
{
  public class PlayerRenderUpdater : MonoBehaviour
  {
    [SerializeField] private Transform _parent;
    private GameObject _current;

    private void Awake()
    {
      for (var i = 0; i < _parent.childCount; i++)
        Destroy(_parent.GetChild(i).gameObject);
    }

    public void UpdateRender(PlayerView player)
    {
      if (_current)
        Destroy(_current);

      PlayerView newPlayer = Instantiate(player, _parent);
      GameObject cameraHolder = newPlayer.CameraHolder.gameObject;
      cameraHolder.SetActive(false);
      Destroy(cameraHolder);
      Destroy(newPlayer.Rb);
      newPlayer.AnimateJump(false);
      newPlayer.AnimateRun(false);
      newPlayer.Anim.Play("Idle_Up");
      newPlayer.Anim.Play("Idle_Legs");
      newPlayer.transform.position = _parent.position;
      newPlayer.Character.localRotation = Quaternion.identity;

      _current = newPlayer.gameObject;
      foreach (Transform child in _current.GetComponentsInChildren<Transform>())
        child.gameObject.layer = LayerMask.NameToLayer("PlayerModel");
    }
  }
}