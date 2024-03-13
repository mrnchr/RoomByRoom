using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public interface ICoroutineRunner
    {
        public Coroutine Run(IEnumerator routine);
        
        public void Abort(IEnumerator routine);
        public void Abort(Coroutine routine);
    }
}