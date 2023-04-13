using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    /*
     * NPC class allows me to create different characters in the GameWorld.
     */
    public class NPC : IPeople
    {

        private float _health;
        private bool _innocent;
        private bool _firstTalk;
        public bool Innocent
        {
            get
            {
                return _innocent;
            }
        }
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
        
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        private int _size;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        private Room _currentRoom;
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                _currentRoom = value;
            }
        }
        private Weapon _gun;
        public Weapon Gun
        {
            get
            {
                return _gun;
            }
            set
            {
                _gun = value;
            }
        }
        public Dictionary<string, IItem> Inventory { get; set; }
        public Dictionary<string, bool> DialogueHistory { get; set; }
        public Dictionary<string, string> DialogueOptions { get; set; }
        private Quest _task;
        public Quest Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
            }
        }

        public NPC(string name, Room room, bool innocence)
        {
            _name = name;
            _currentRoom = room;
            _health = 100;
            _size = 5;
            DialogueOptions = new Dictionary<string, string>();
            DialogueHistory = new Dictionary<string, bool>();
            Inventory = new Dictionary<string, IItem>();
            _innocent = innocence;
            _firstTalk = true;
            NotificationCenter.Instance.AddObserver("PlayerDidMiss", PlayerDidMiss);
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public IItem findItem(string itemName)
        {
            IItem found = null;
            Inventory.TryGetValue(itemName, out found);

            return found;
        }
        public void EquipGun(IItem  gun)
        {
            this._gun = (Weapon)gun;
        }
        public void PlayerDidMiss(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (!_innocent)
            {
                this.OutputMessage("You'll never take me alive!");
                _gun.Use(player);
            }
        }
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
        }
        /*
         * Abandoned this functionality for now.
        public void GainAffinity()
        {
            Affinity += 10;
        }
        public void LoseAffinity()
        {
            Affinity -= 10;
        }
        */
        public void GetItem(IItem item)
        {          
            Inventory.Add(item.Name, item);
        }
        public void DropItem(string name)
        {
            IItem item = null;
            Inventory.TryGetValue(name, out item);
            Inventory.Remove(name);
            _currentRoom.HasItem(item);
        }
        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            string response = "";
            if (player.CurrentRoom.Tag == CurrentRoom.Tag)
            {
                Dictionary<string, Object> userInfo = notification.userInfo;
                string word = (string)userInfo["word"];
                if (word.Equals("Greeting") || word.Equals("Accuse") || word.Equals("Relationship") ||
                        word.Equals("Suspicion") || word.Equals("Insult") || word.Equals("Quest"))
                {
                    if (player.Experience >= 100 && !_firstTalk)
                    {
                        DialogueOptions.TryGetValue(word + " Advanced", out response);
                        bool historic = false;
                        if (DialogueHistory.TryGetValue(word, out historic))
                        {

                        }
                        DialogueHistory.Add(word, true);
                        
                        
                        OutputMessage(response);
                    }
                    else
                    {
                        DialogueOptions.TryGetValue(word, out response);
                        OutputMessage(response);
                        _firstTalk = false;
                    }
                }
                /*
                 * Code was originally in the Room class, but I later decided to move this to NPC as is makes more sense here.
                 * This was also my less efficient way of handling the logic.
                if (word.Equals("Greeting"))
                {
                    if(player.Experience >= 50)
                    {
                        _npc.DialogueOptions.TryGetValue(word + " Advanced", out response);
                        _npc.OutputMessage(response);
                    }
                    else
                    {
                        _npc.DialogueOptions.TryGetValue(word, out response);
                        _npc.OutputMessage(response);
                    }
                }
                else if (word.Equals("Accuse"))
                {
                    if (player.Experience >= 50)
                    {
                        _npc.DialogueOptions.TryGetValue(word + " Advanced", out response);
                        _npc.OutputMessage(response);
                    }
                    else
                    {
                        _npc.DialogueOptions.TryGetValue(word, out response);
                        _npc.OutputMessage(response);
                    }    
                }
                else if (word.Equals("Relationship"))
                {
                    _npc.DialogueOptions.TryGetValue(word, out response);
                    _npc.OutputMessage(response);
                }
                else if (word.Equals("Suspicion"))
                {
                    _npc.DialogueOptions.TryGetValue(word, out response);
                    _npc.OutputMessage(response);
                }
                else if (word.Equals("Insult"))
                {
                    _npc.DialogueOptions.TryGetValue(word, out response);
                    _npc.OutputMessage(response);
                }
                */
            }

        }
        public void OutputMessage(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}
