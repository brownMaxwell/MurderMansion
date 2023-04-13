using System.Collections;
using System.Collections.Generic;
using System;

namespace RPGame
{
    public class Player : IPeople
    {
        private float _experience;
        private int _killCount;
        public float Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                _experience = value;
            }
        }
        private float _maxVolume;
        public float MaxVolume
        {
            get
            {
                return _maxVolume;
            }
        }
        private float _currentVolume;
        public float CurrentVolume
        {
            get
            {
                foreach (IItem item in Inventory.Values)
                {
                    _currentVolume += item.Volume;
                }
                return _currentVolume;
            }
        }
        private float _currentWeight;
        public float CurrentWeight
        {
            get
            {
                foreach(IItem item in Inventory.Values)
                {
                    _currentWeight += item.Weight;
                }
                return _currentWeight;
            }
        }
        private float _maxWeight;
        public float MaxWeight
        {
            get
            {
                return _maxWeight;
            }
        }
        public Dictionary<string, string> DialogueOptions { get; set; }
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
        private float _health;
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
        public string Name { get;}

        public Stack<Room> PreviousRooms;
        public Dictionary<string, IItem> Inventory { get; set; }
        //private Room _previous = null;
        /*
        public Room PreviousRoom
        {
            get
            {
                return _previous;
            }
            set
            {
                _previous = value;
            }
        }
        */
        private Room _currentRoom = null;
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

        public Player(Room room)
        {
            _size = 5;
            _maxWeight = 20.0f;
            _maxVolume = 20.0f;
            _health = 200;
            _currentRoom = room;
            _killCount = 0;
            Inventory = new Dictionary<string, IItem>();
            PreviousRooms = new Stack<Room>();
        }
        public void OpenContainer(string containerName)
        {
            ItemContainer container = (ItemContainer)CurrentRoom.FindRoomItem(containerName);
            if(container != null)
            {
                container.Open(CurrentRoom);                
            }
            else
            {
                WarningMessage("Can't find the container you want to open.");
            }
            InfoMessage(CurrentRoom.RoomObjects());
        }
        /*
         * Made to alleviate some user pain when avoiding the Unlock bug.
         */
        public void DropAll()
        {
            foreach(IItem item in Inventory.Values)
            {
                DropItem(item.Name);
            }
        }
        public void DropItem(string itemName)
        {
            IItem item = findItem(itemName);
            if(item != null)
            {
                if(_currentRoom.Character != null && _currentRoom.Character.Task != null)
                {
                    Inventory.Remove(itemName);
                    _currentRoom.HasItem(item);
                    _currentRoom.Character.Task.CompleteQuest();
                    Experience += 20.0f;
                }
                else
                {
                    Inventory.Remove(itemName);
                    _currentRoom.HasItem(item);
                }
            }
            else
            {
                WarningMessage("You don't have that item.");
            }
            InfoMessage(CurrentRoom.RoomObjects());
        }
        public void obtainItem(string itemName)
        {
            
            IItem item = this._currentRoom.removeItem(itemName);
            if (item != null)
            {
                if (item.CanTake)
                {
                    if (!(_currentWeight + item.Weight > _maxWeight) && !(_currentVolume + item.Volume > _maxVolume))
                    {
                        Inventory.Add(item.Name, item);
                        this.OutputMessage("\nObtained " + itemName + " from " + this._currentRoom.Tag);
                    }
                    else
                    {
                        this.OutputMessage("\n" + item.Name + " is too heavy. Try dropping something and trying again.");
                    }
                }
                else
                {
                    WarningMessage("Can't pick up this item.");
                }
            }
            else
            {
                WarningMessage("Item not found.");
            }
        }

        public void Inspect(string itemName)
        {
            IItem item = _currentRoom.removeItem(itemName);
            if(item != null)
            {
                if(Experience >= 20)
                {
                    InfoMessage(item.LongDescription);
                }
                else
                {
                    this.InfoMessage(item.Description());
                }
                if (item.FirstInspection)
                {
                    this.Experience += 10.0f;
                    item.FirstInspection = false;
                }
                _currentRoom.HasItem(item);
            }
            else
            {
                this.WarningMessage("Item not found.");
            }
        }
        public string InspectNPCInventory()
        {
            string items = "";
            IPeople NPC = _currentRoom.Character;
            if(NPC != null)
            {
                if (Experience >= 20)
                {
                    Dictionary<string, IItem>.ValueCollection values = NPC.Inventory.Values;
                    foreach (IItem item in values)
                    {
                        items += " " + item.LongDescription;
                    }
                    Experience += 10.0f;
                }
                else
                {
                    WarningMessage("You're not experienced enough to make " + NPC.Name + " show you their things.");
                }
            }
            else
            {
                WarningMessage("There's no one to inspect.");
            }
            return items;
        }
        public string GetInventory()
        {
            string items = "Your current Items: ";
            Dictionary<string, IItem>.KeyCollection keys = Inventory.Keys;
            foreach(string itemName in keys)
            {
                items += " " + itemName;
            }
            return items;
        }
        public Key FindKey(Room targetRoom)
        {
            Key key = null;
            Dictionary<string, IItem>.ValueCollection values = Inventory.Values;
            Dictionary<string, Key> keys = new Dictionary<string, Key>();
            foreach(IItem item in values)
            {
                if(item is Key)
                {
                    keys.Add(item.Name, (Key)item);
                }
            }
            foreach(Key k in keys.Values)
            {
                if(k.ForRoom == targetRoom)
                {
                    key = k;
                }
            }
            return key;
        }
        public void Unlock(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            Room ToRoom = door.otherRoom(_currentRoom);
            if (door != null)
            {
                if (door.IsLocked)
                {
                    Key key = FindKey(ToRoom);
                    if (key != null)
                    {
                        door.Unlock();
                        if (door.IsUnlocked)
                        {
                            this.OutputMessage("\nUnlocked door");
                            door.open();
                        }
                    }
                    else
                    {
                        WarningMessage("You don't have the key to unlock this door.");
                    }
                }
                else
                {
                    WarningMessage("There is no key.");
                }
            }
        }
        public IItem findItem(string itemName)
        {
            IItem found = null;
            Inventory.TryGetValue(itemName, out found);

            return found;
        }
        /*
        public void UseItem(string itemName)
        {
            IItem usingItem = findItem(itemName);
        }
        */
        public void EquipGun(string gunName)
        {
            Weapon gun = (Weapon)findItem(gunName);
            this._gun = gun;
            Inventory.Remove(gun.Name);
            this.InfoMessage("Equipped " + gun.Name);
            Experience += 5;
        }
        public bool Shoot(string targetName)
        {
            bool SolvedCase = false;
            if (_gun == null)
            {
                ErrorMessage("You don't have a gun.");
            }
            else
            {
                NPC target = null;
                target = (NPC)_currentRoom.Character;
                //GameWorld.Instance.NPCs.TryGetValue(targetName, out target);
                if (target != null)
                {
                    _gun.Use(target);

                    ErrorMessage("You shot  at " + target.Name);
                    if (!target.Innocent && target.Health <= 0)
                    {
                         SolvedCase = true;
                         InfoMessage("You got the murderer!");  
                        
                    }else if(!target.Innocent && target.Health > 0)
                    {
                        SolvedCase = false;
                        Notification notification = new Notification("PlayerDidMiss", this);
                        NotificationCenter.Instance.PostNotification(notification);
                    }
                    else if(target.Innocent && target.Health == 0)
                    {
                        SolvedCase = false;
                        target.CurrentRoom.Character = null;
                        _killCount++;
                        ErrorMessage("You killed an innocent person.");
                        if(_killCount == 3)
                        {
                            SolvedCase = true;
                            ErrorMessage("You've killed too many innocent people. You lose.");
                        }
                    }
                }
                else
                {
                    WarningMessage("Target not found.");
                }
            }
            return SolvedCase;
        }
        /*
        public void Shoot()
        {

            IPeople target = this.CurrentRoom.NPC;
            _gun.Use(target);
        }
        */
        public void Say(string word)
        {
            OutputMessage("\n" + word + "\n");
            Dictionary<string, Object> userInfo = new Dictionary<string, Object>();
            userInfo["word"] = word;

            Notification notification = new Notification("PlayerDidSayWord", this, userInfo);
            NotificationCenter.Instance.PostNotification(notification);
        }
        public void BackTrack()
        {
            if(_currentRoom.Delegate == null)
            {
                if (PreviousRooms.Count == 0)
                {
                    this.OutputMessage("Can't go back any further.");
                }
                else
                {
                    Room room = PreviousRooms.Pop();
                    this._currentRoom = room;
                    this.OutputMessage("\n" + this._currentRoom.Description());
                }
            }
            else
            {
                WarningMessage("Can't go back.");
            }
            
        }
        public void Walkto(string direction)
        {
            PreviousRooms.Push(_currentRoom);
            Door door = this._currentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.Open)
                {
                    Room nextRoom = door.otherRoom(CurrentRoom);
                    //Notification notification = new Notification("PlayerWillExitRoom", this);
                    //NotificationCenter.Instance.PostNotification(notification);
                    this._currentRoom = nextRoom;
                    Notification notification = new Notification("PlayerDidEnterRoom", this);
                    NotificationCenter.Instance.PostNotification(notification);
                    this.OutputMessage("\n" + this._currentRoom.Description());
                }
                else
                {
                    this.OutputMessage("Door is locked");
                }
               
            }
            else
            {
                this.OutputMessage("\nThere is no door on " + direction);
            }
        }

        public void ModifyGun(string DecoratorName)
        {
            IItem decorator = findItem(DecoratorName);
            
            if(decorator != null && _gun.Decoration == null)
            {
                _gun.addDecorator(decorator);
                Inventory.Remove(decorator.Name);
                InfoMessage("Added " + decorator.Name + " to your " + _gun.Name);
            }
            else if(decorator != null && _gun.Decoration != null)
            {
                InfoMessage("Your " + _gun.Name + " is already equipped with " + _gun.Decoration.Name + 
                    "\nWould you like to replace the current modification with " + DecoratorName + "?(Y/N)" );
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "y":
                        _gun.addDecorator(decorator);
                        Inventory.Remove(decorator.Name);
                        break;
                    case "n":
                        InfoMessage("You didn't equip the " + DecoratorName + ".");
                        break;
                }

            }
            {
                WarningMessage("Can't find attachment.");
            }
        }
        /*
         * CURRENT BUG. If you have any non Key items in your inventory, the FindKey method doesn't work.
         * Player work around is to drop any non Key items in your inventory, unlock the door, then pickup your items again.
         */
        

        public void Open(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            if(door != null)
            {
                if (door.Closed)
                {
                    door.open();
                    if (door.Open)
                    {
                        this.OutputMessage("\nOpened the door.");

                    }
                    else
                    {
                        this.OutputMessage("\nThe door is locked.");
                    }
                }
                else
                {
                    this.OutputMessage("\nDoor is not closed");
                }
                
            }
            else
            {
                this.OutputMessage("Can't open door");
            }
        }
        public void Close(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            if (door != null)
            {
                if (door.Open)
                {
                    door.Close();
                    this.OutputMessage("Closed the door");
                }
                else
                {
                    this.OutputMessage("Door is not open");
                }

            }
            else
            {
                this.OutputMessage("Can't close door");
            }
        }
        public void TakeDamage(float damage)
        {
           this._health -= damage;
        }
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ColoredMessage(string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }
        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);

        }
        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Yellow);
        }
        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Green);
        }
    }

}
