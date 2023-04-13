using System.Collections;
using System.Collections.Generic;

namespace RPGame
{
    public class GoCommand : Command
    {

        public GoCommand() : base()
        {
            this.Name = "go";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Walkto(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nGo Where?");
            }
            return false;
        }
    }
}
