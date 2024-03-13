using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine Run(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void Abort(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        public void Abort(Coroutine routine)
        {
            StopCoroutine(routine);
        }
    }
}