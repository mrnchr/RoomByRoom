using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace UI.Menu
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _loginField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Button _button;
        private ILoginController _controller;

        [Inject]
        public void Construct(ILoginController controller)
        {
            _controller = controller;
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _controller.Login(_loginField.text, _passwordField.text);
        }
    }
}