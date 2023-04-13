using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class InspectInventory : Command
    {
        public InspectInventory() : base()
        {
            this.Name = "inspectcharacter";
        }

        override
        public bool Execute(Player player)
        {
            if (player.CurrentRoom.Character != null)
            {
                player.InfoMessage(player.InspectNPCInventory());
            }
            else
            {
                player.OutputMessage("\nNo one to inspect.");
            }
            return false;
        }
    }
}
