using UnityEngine;

namespace RoomByRoom
{
    public abstract class View : MonoBehaviour
    {
        public int Entity;

        public static void InstantiateView<T>(GameObject prefab, out T view)
        where T : View
        {
            view = GameObject.Instantiate(prefab)
                .GetComponent<T>();
        }
    }
}