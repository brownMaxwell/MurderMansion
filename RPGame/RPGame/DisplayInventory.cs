using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class DisplayInventory : Command
    {
        public DisplayInventory () : base()
        {
            this.Name = "inventory";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nDon't add another word after the inventory command.");
            }
            else
            {
                player.OutputMessage(player.GetInventory());
            }
            return false;
        }
    }
}
