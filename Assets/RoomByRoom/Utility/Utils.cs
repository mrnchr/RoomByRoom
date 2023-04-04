using System;

using UnityEngine;

namespace RoomByRoom.Utility
{
    public static class Utils
    {
        public static void SetTransform(Transform from, Transform to)
        {
            from.position = to.position;
            from.rotation = to.rotation;
        }
        
        public static int GetEnumLength<T>() 
        where T : Enum => Enum.GetNames(typeof(T)).Length;
    }
}