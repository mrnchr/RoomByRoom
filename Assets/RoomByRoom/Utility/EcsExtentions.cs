using System;
using System.Runtime.CompilerServices;

using Leopotam.EcsLite;

namespace RoomByRoom.Utility
{
    public static class EcsExtentions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddComponent<T>(this EcsWorld world, int entity)
        where T : struct => ref world.GetPool<T>().Add(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetComponent<T>(this EcsWorld world, int entity)
        where T : struct => ref world.GetPool<T>().Get(entity);

        public static bool HasComponent<T>(this EcsWorld world, int entity)
        where T : struct => world.GetPool<T>().Has(entity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DelComponent<T>(this EcsWorld world, int entity)
        where T : struct => world.GetPool<T>().Del(entity);

        public static ref T Initialize<T>(this ref T obj, Func<T, T> action)
        where T : struct
        {
            if(action != null)
                obj = action(obj);

            return ref obj;
        }
    }
}
