using System;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace RoomByRoom.Utility
{
	public static class EcsExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(this EcsWorld world, int entity)
			where T : struct =>
			ref world.GetPool<T>().Add(entity);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Get<T>(this EcsWorld world, int entity)
			where T : struct =>
			ref world.GetPool<T>().Get(entity);

		public static bool Has<T>(this EcsWorld world, int entity)
			where T : struct =>
			world.GetPool<T>().Has(entity);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Del<T>(this EcsWorld world, int entity)
			where T : struct
		{
			world.GetPool<T>().Del(entity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack(this EcsWorld world, EcsPackedEntity packed)
		{
			packed.Unpack(world, out int entity);
			return entity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Update<T>(this EcsWorld world, int entity)
			where T : struct =>
			ref world.Has<T>(entity)
				? ref world.Get<T>(entity)
				: ref world.Add<T>(entity);

		public static ref T Assign<T>(this ref T obj, Func<T, T> action)
			where T : struct
		{
			if (action != null)
				obj = action(obj);

			return ref obj;
		}
	}
}