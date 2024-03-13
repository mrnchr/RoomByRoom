using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Profile
{
    public class ProfileView : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _gameCount;
        [SerializeField] private TMP_Text _winCount;
        [SerializeField] private TMP_Text _loseCount;

        public void SetAvatarPixels(Color[] colors)
        {
            _avatar.sprite.texture.SetPixels(colors);
            _avatar.sprite.texture.Apply();
        }

        public void SetNickname(string nickname)
        {
            _nickname.text = nickname;
        }
        
        public void SetGameCount(string count)
        {
            _gameCount.text = count;
        }
        
        public void SetWinCount(string count)
        {
            _winCount.text = count;
        }
        
        public void SetLoseCount(string count)
        {
            _loseCount.text = count;
        }
    }
}