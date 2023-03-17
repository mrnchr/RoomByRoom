using System.Collections.Generic;

using UnityEngine;

namespace RoomByRoom
{
    public class HumanoidView : GroundUnitView
    {
        [SerializeField] private ItemPlace Boots;
        [SerializeField] private ItemPlace Leggings;
        [SerializeField] private ItemPlace Gloves;
        [SerializeField] private ItemPlace BreastPlate;
        [SerializeField] private ItemPlace Helmet;
        [SerializeField] private ItemPlace Shield;

        private Dictionary<ArmorType, ItemPlace> _itemPlaces = new Dictionary<ArmorType, ItemPlace>();

        public ItemPlace this[ArmorType type]
        {
            get
            {
                return _itemPlaces[type];
            }
        }

        private void Awake()
        {
            _itemPlaces[ArmorType.Boots] = Boots;
            _itemPlaces[ArmorType.Leggings] = Leggings;
            _itemPlaces[ArmorType.Gloves] = Gloves;
            _itemPlaces[ArmorType.BreastPlate] = BreastPlate;
            _itemPlaces[ArmorType.Helmet] = Helmet;
            _itemPlaces[ArmorType.Shield] = Shield;
        }

        public override void PlayAttackAnimation(WeaponType weaponType)
        {            
            Animator.SetInteger("Weapon", (int)weaponType);
            Animator.SetTrigger("StartAttack");
            Debug.Log("Start Attack");

            // TODO: change when there is a bow
        }
    }
}