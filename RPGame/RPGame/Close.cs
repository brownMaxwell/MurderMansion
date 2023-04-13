using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Close : Command
    {
        public Close() : base()
        {
            this.Name = "close";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Close(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nOpen what?");
            }
            return false;
        }
    }
}

