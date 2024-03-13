using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu.Registration
{
    public class RegistrationView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _loginField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private TMP_InputField _confirmedPasswordField;
        [SerializeField] private Button _button;
        private IRegistrationController _controller;

        [Inject]
        public void Construct(IRegistrationController controller)
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
            _controller.Register(_loginField.text,_passwordField.text, _confirmedPasswordField.text);
        }
    }
}