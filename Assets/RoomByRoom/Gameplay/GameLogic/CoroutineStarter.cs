using System.Collections;
using UnityEngine;

namespace RoomByRoom
{
	public class CoroutineStarter : MonoBehaviour
	{
		public void PerformCoroutine(IEnumerator coroutine)
		{
			StartCoroutine(coroutine);
		}
		// public void PerformCoroutine<T>(Func<T, IEnumerator> coroutine, T arg) => StartCoroutine(coroutine(arg));
	}
}