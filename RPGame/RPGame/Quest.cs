using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    /*
     * This class allows me to create quests that require a certain item to be in the
     * quest giver's inventory in order to be completed. Then it will give the designated
     * reward to the player.
     */
    public class Quest
    {
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        private NPC _questGiver;
        public NPC QuestGiver
        {
            get
            {
                return _questGiver;
            }
            set
            {
                _questGiver = value;
            }
        }
        private string _requiredItemName;
        public string RequiredItemName
        {
            get
            {
                return _requiredItemName;
            }
            set
            {
                _requiredItemName = value;
            }
        }
        private string _rewardItemName;
        public string RewardItemName
        {
            get
            {
                return _rewardItemName;
            }
            set
            {
                _rewardItemName = value;
            }
        }

        public Quest(string description, NPC giver, string required, string reward)
        {
            _description = description;
            _questGiver = giver;
            _requiredItemName = required;
            _rewardItemName = reward;
        }

        public void CompleteQuest()
        {
            IItem required = QuestGiver.CurrentRoom.removeItem(_requiredItemName);
            if(required != null)
            {
                QuestGiver.GetItem(required);
                QuestGiver.OutputMessage("Thank you for giving me " + _requiredItemName + ".  I'll leave " + _rewardItemName + " here on the floor for you.");
                QuestGiver.DropItem(_rewardItemName);
                QuestGiver.Task = null; //Remove the quest.
            }
            else
            {
                QuestGiver.OutputMessage("You don't have " + _requiredItemName);
            }

        }
    }
}
