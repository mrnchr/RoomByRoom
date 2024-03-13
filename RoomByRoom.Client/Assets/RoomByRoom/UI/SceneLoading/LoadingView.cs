using UnityEngine;
using UnityEngine.UI;

namespace UI.SceneLoading
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}