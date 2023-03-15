using UnityEngine;

namespace RoomByRoom
{
    public class ItemView : View
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