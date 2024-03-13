using System;
using System.Collections.Generic;
using System.Linq;
using Authorization;
using RoomByRoom;
using SharedData.Authentication;
using UnityEngine;
using Zenject;

namespace Configuration
{
    [CreateAssetMenu(fileName = CreateAssetMenuNames.AUTH_ERROR_FILE_NAME, menuName = CreateAssetMenuNames.AUTH_ERROR_MENU_NAME)]
    public class AuthorizationErrorConfig : ScriptableObject, ISelfBinder<AuthorizationErrorConfig>
    {
        public List<AuthErrorTuple> AuthErrors;
        public List<InputErrorTuple> InputErrors;

        public string Get(AuthenticationErrorType type)
        {
            return AuthErrors.First(x => x.Type == type).Text;
        }

        public string Get(InputErrorType type)
        {
            return InputErrors.First(x => x.Type == type).Text;
        }
        
        public void Bind(DiContainer container)
        {
            (this as ISelfBinder<AuthorizationErrorConfig>).BindSelf(container);
        }
    }

    [Serializable]
    public struct AuthErrorTuple
    {
        public AuthenticationErrorType Type;
        public string Text;
    }

    [Serializable]
    public struct InputErrorTuple
    {
        public InputErrorType Type;
        public string Text;
    }
}