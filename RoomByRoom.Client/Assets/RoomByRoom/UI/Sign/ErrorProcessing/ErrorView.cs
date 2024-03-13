using TMPro;
using UnityEngine;

namespace UI.Menu.ErrorProcessing
{
    public class ErrorView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [field:SerializeField] public ErrorViewType Id { get; private set; }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}