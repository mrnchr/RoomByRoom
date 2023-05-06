using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileCreatorWindow : MonoBehaviour
  {
    private bool _isInputCorrected;
    private Configuration _config;
    private MainMenuMediator _mainMenuMediator;
    private ProfileFieldView _newProfileField;
    private GameObject _errorMessage;
    private readonly char[] _invalidFileNameSymbols = { '/', '\\', ':', '*', '?', '\"', '<', '>', '|' };

    private void Awake()
    {
      _mainMenuMediator = FindObjectOfType<MainMenuMediator>();
      _isInputCorrected = true;
    }

    public void Construct(Configuration config, ProfileFieldView fieldView, GameObject errorMessage)
    {
      _config = config;
      _newProfileField = fieldView;
      _errorMessage = errorMessage;

      _newProfileField.OnValueChanged += CheckProfileName;
    }

    private void CheckProfileName(string text)
    {
      _isInputCorrected = !ContainsInvalidFileNameSymbols(text);
      _errorMessage.SetActive(!_isInputCorrected);
    }

    private bool ContainsInvalidFileNameSymbols(string text) => text.IndexOfAny(_invalidFileNameSymbols) >= 0;

    public void Show()
    {
      _mainMenuMediator.SwitchNewProfile(true);
      _newProfileField.InputField.text = _config.DefaultSaveName;
    }

    public void TryStartGame()
    {
      if (_isInputCorrected)
        _mainMenuMediator.StartGame(_newProfileField.InputField.text);
    }
  }
}