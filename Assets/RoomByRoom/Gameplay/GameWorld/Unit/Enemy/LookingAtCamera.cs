using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoomByRoom
{
	public class LookingAtCamera : MonoBehaviour
	{
		[SerializeField] private List<Transform> _lookings;
		private Transform _mainCamera;

		private void Start()
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		}

		private void Update()
		{
			foreach (Transform looking in _lookings)
			{
				looking.LookAt(_mainCamera);
			}
		}
	}
}