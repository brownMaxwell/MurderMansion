using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    /*
     * Made to help make it easier to empty the inventory before unlocking a door thanks to the bug.
     */
    class DropAll : Command
    {
        public DropAll() : base()
        {
            this.Name = "dropall";
        }

        override
        public bool Execute(Player player)
        {
            bool answer = false;
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nDrop All doesn't require a second word.");

            }
            else
            {
                player.DropAll();
            }
            return answer;
        }
    }
}
