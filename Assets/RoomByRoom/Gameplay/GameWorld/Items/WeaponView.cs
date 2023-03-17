using UnityEngine;

namespace RoomByRoom
{
    public class WeaponView : ItemView
    {
        public Collider[] AttackTriggers;

        private void Start()
        {
            SetActiveAttackTriggers(false);
        }

        public void SetActiveAttackTriggers(bool isActive)
        {
            foreach(var trigger in AttackTriggers)
            {
                trigger.enabled = isActive;
            }
        }
    }
}