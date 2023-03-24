using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace RoomByRoom
{
    internal class LoadSaveSystem : IEcsPreInitSystem
    {
        private EcsCustomInject<SavedData> _savedData = default;
        private EcsCustomInject<DefaultData> _defaultData = default;
        private EcsCustomInject<Configuration> _conf = default;
        private EcsCustomInject<SavingService> _savingSvc = default;

        public void PreInit(IEcsSystems systems)
        {
            // if it is possible to load data
            SavedData tempData = new SavedData();
            if(!_conf.Value.IsNewGame && _savingSvc.Value.LoadData(ref tempData))
            {
                _savedData.Value.Copy(tempData);
                return;
            }
            
            // else set data by default
            _savedData.Value.GameInfo = _defaultData.Value.GameInfo;
            _savedData.Value.Player = _defaultData.Value.Player;
            _savedData.Value.Room = _defaultData.Value.Room;
            _savedData.Value.Inventory = _defaultData.Value.Inventory;
            _savingSvc.Value.SaveData(_savedData.Value);
        }
    }
}