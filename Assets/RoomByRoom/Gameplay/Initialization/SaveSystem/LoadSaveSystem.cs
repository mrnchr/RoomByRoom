using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
	internal class LoadSaveSystem : IEcsPreInitSystem
	{
		private readonly EcsCustomInject<Configuration> _conf = default;
		private readonly EcsCustomInject<DefaultData> _defaultData = default;
		private readonly EcsCustomInject<Saving> _savedData = default;
		private readonly EcsCustomInject<SavingService> _savingSvc = default;
		private EcsCustomInject<GameInfo> _gameInfo;

		public void PreInit(IEcsSystems systems)
		{
			var tempData = new Saving();
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