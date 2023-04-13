using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Pickup : Command
    {
        public Pickup() : base()
        {
            this.Name = "pickup";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.obtainItem(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nPickup what?");
            }
            return false;
        }
    }
}
