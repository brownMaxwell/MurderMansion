using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Drop : Command
    {
        public Drop() : base()
        {
            this.Name = "drop";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.DropItem(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nDrop what?");
            }
            return false;
        }
    }
}
