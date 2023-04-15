using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
	internal class LoadSaveSystem : IEcsPreInitSystem
	{
		private EcsCustomInject<Saving> _savedData = default;
		private EcsCustomInject<DefaultData> _defaultData = default;
		private EcsCustomInject<Configuration> _conf = default;
		private EcsCustomInject<SavingService> _savingSvc = default;
		private EcsCustomInject<GameInfo> _gameInfo = default;

		public void PreInit(IEcsSystems systems)
		{
			Saving tempData = new Saving();
			if (!_conf.Value.IsNewGame && _savingSvc.Value.LoadData(ref tempData))
			{
				_savedData.Value.CopyOf(tempData);
			}
			else
			{
				SetDefaultData();
				_savingSvc.Value.SaveData(_savedData.Value);
				SetSessionData();
			}
		}

		private void SetSessionData()
		{
			_gameInfo.Value = _savedData.Value.GameSave;
		}

		private void SetDefaultData()
		{
			_savedData.Value.GameSave = _defaultData.Value.GameInfo;
			_savedData.Value.Player = _defaultData.Value.Player;
			_savedData.Value.Room = _defaultData.Value.Room;
			_savedData.Value.Inventory = _defaultData.Value.Inventory;
		}
	}
}