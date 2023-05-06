using System.Collections.Generic;
using Leopotam.EcsLite;
using RoomByRoom.UI.Game.HUD;
using RoomByRoom.UI.Game.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoomByRoom.UI.Game
{
  public class GameMediator : MonoBehaviour
  {
    [SerializeField] private Can _can;
    [SerializeField] private GameObject _bonusCard;
    private GameWindowSwitcher _windowSwitcher;
    private TurnWindowService _turnWindowSvc;
    private InventoryUpdater _inventory;
    private PlayerRenderUpdater _playerRender;
    private ItemRenderUpdater _itemRender;
    private ItemDescriptionUpdater _itemDesc;
    private RoomCountUpdater _roomCount;
    private EquipService _equipSvc;
    private ItemDragger _itemDragger;
    private WinUISwitcher _winUI;
    private ReloadSceneService _sceneSvc;

    public void Construct(TurnWindowService turnWindowSvc, EquipService equipSvc, ReloadSceneService sceneSvc)
    {
      _turnWindowSvc = turnWindowSvc;
      _equipSvc = equipSvc;
      _sceneSvc = sceneSvc;
    }

    private void Awake()
    {
      _windowSwitcher = FindObjectOfType<GameWindowSwitcher>();
      _inventory = FindObjectOfType<InventoryUpdater>();
      _playerRender = FindObjectOfType<PlayerRenderUpdater>();
      _itemRender = FindObjectOfType<ItemRenderUpdater>();
      _itemDesc = FindObjectOfType<ItemDescriptionUpdater>();
      _roomCount = FindObjectOfType<RoomCountUpdater>();
      _itemDragger = FindObjectOfType<ItemDragger>();
      _winUI = FindObjectOfType<WinUISwitcher>();
    }

    public void TurnWindow(WindowType window) => _windowSwitcher.TurnWindow(window);
    public void ContinueGame() => _turnWindowSvc.SendSwitchWindowMessage(SwitchWindowMessageType.Pause);
    public void ExitToMenu() => SceneManager.LoadScene(0);
    public void AddItemToInventory(ItemInfoForSlot itemInfo) => _inventory.AddItem(itemInfo);
    public void CleanAllSlots() => _inventory.CleanAll();
    public void UpdatePlayerRender(PlayerView player) => _playerRender.UpdateRender(player);
    public void UpdateItemRender(ItemView item = null) => _itemRender.UpdateRender(item);
    public void UpdateItemDescription(params Characteristic[] chars) => _itemDesc.UpdateDescription(chars);
    public void UpdateRoomCount(string text) => _roomCount.SetText(text);
    public void ChangeEquip(int item) => _equipSvc.ChangeEquip(item);
    public void BreakDragItem() => _itemDragger.BreakDrag();
    public List<EcsPackedEntity> ClearCan() => _can.Clear();
    public void SetActiveBonusCard(bool active) => _bonusCard.SetActive(active);
    public void SetWinUI(bool active) => _winUI.SetWin(active);
    public void SetStartUI(bool active) => _winUI.SetStart(active);
    public void ActivateScene() => _sceneSvc.AllowSceneActivation();
  }
}