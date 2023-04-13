using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Modify : Command
    {
        public Modify() : base()
        {
            this.Name = "modify";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.ModifyGun(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nModify with what?");
            }
            return false;
        }
    }
}
