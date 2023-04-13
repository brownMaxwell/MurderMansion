using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Open : Command
    {
        public Open() : base()
        {
            this.Name = "Open";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Open(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nOpen what?");
            }
            return false;
        }
    }
}

