using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Equip : Command
    {
        public Equip() : base()
        {
            this.Name = "equip";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.EquipGun(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nEquip what?");
            }
            return false;
        }
    }
}
