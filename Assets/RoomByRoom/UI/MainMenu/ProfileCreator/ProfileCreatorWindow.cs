using System.Linq;
using TMPro;
using UnityEngine;

namespace RoomByRoom.UI.MainMenu
{
  public class ProfileCreatorWindow : MonoBehaviour
  {
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _errorMessage;
    [SerializeField] private GameObject _equalMessage;
    private bool _isInputError;
    private bool _isInputEqual;
    private MainMenuMediator _mainMenuMediator;
    private readonly char[] _invalidFileNameSymbols = { '/', '\\', ':', '*', '?', '\"', '<', '>', '|' };

    private void Awake()
    {
      _mainMenuMediator = FindObjectOfType<MainMenuMediator>();
      _inputField.onValueChanged.AddListener(CheckProfileName);
    }

    public void Show()
    {
      _mainMenuMediator.SwitchNewProfile(true);
    }

    public void StartGame()
    {
      if (!_isInputError && !_isInputEqual)
        _mainMenuMediator.StartGame(_inputField.text);
    }

    private void CheckProfileName(string text)
    {
      _isInputError = ContainsInvalidFileNameSymbols(text);
      _isInputEqual = _mainMenuMediator.LoadProfiles().Contains(text);
      _errorMessage.SetActive(_isInputError);
      _equalMessage.SetActive(_isInputEqual);
    }

    private bool ContainsInvalidFileNameSymbols(string text) => text.IndexOfAny(_invalidFileNameSymbols) >= 0;
  }
}