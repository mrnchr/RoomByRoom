using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom.UI.Game
{
	public class ItemRenderUpdater : MonoBehaviour
	{
		[FormerlySerializedAs("_parent"),SerializeField] private Transform _weaponParent;
		[SerializeField] private Transform _armorParent;
		private GameObject _current;

		private void Awake()
		{
			CleanChildren(_weaponParent);
			CleanChildren(_armorParent);
		}

		private void CleanChildren(Transform parent)
		{
			for (int i = 0; i < parent.childCount; i++)
				Destroy(parent.GetChild(i).gameObject);
		}

		public void UpdateRender(ItemView item = null)
		{
			if(_current)
				Destroy(_current);

			if (!item) return;

			Transform parent = item is ArmorView ? _armorParent : _weaponParent;
			ItemView newItem = Instantiate(item, parent.position, parent.rotation, parent);

			_current = newItem.gameObject;
			foreach(Transform child in _current.GetComponentsInChildren<Transform>())
				child.gameObject.layer = LayerMask.NameToLayer("ItemModel");
			_current.transform.localPosition = -item.Center.localPosition;
		}
	}
}