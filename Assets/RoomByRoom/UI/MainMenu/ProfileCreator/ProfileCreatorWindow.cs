using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileCreatorWindow : MonoBehaviour
  {
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _errorMessage;
    private bool _isInputError;
    private string _defaultProfileName;
    private MainMenuMediator _mainMenuMediator;
    private readonly char[] _invalidFileNameSymbols = { '/', '\\', ':', '*', '?', '\"', '<', '>', '|' };

    private void Awake()
    {
      _mainMenuMediator = FindObjectOfType<MainMenuMediator>();
      _inputField.onValueChanged.AddListener(CheckProfileName);
    }

    public void Construct(string defaultProfileName)
    {
      _defaultProfileName = defaultProfileName;
    }

    public void Show()
    {
      _mainMenuMediator.SwitchNewProfile(true);
      _inputField.text = _defaultProfileName;
    }

    public void StartGame()
    {
      if (!_isInputError)
        _mainMenuMediator.StartGame(_inputField.text);
    }

    private void CheckProfileName(string text)
    {
      _isInputError = ContainsInvalidFileNameSymbols(text);
      _errorMessage.SetActive(_isInputError);
    }

    private bool ContainsInvalidFileNameSymbols(string text) => text.IndexOfAny(_invalidFileNameSymbols) >= 0;
  }
}