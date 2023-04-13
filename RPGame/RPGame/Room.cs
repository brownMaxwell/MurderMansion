using System.Collections;
using System.Collections.Generic;
using System;

namespace RPGame
{
    public class TrapDoorRoom : RoomDelegate
    {

        public Dictionary<string, Door> Exits { get; set; }
        private Room _containingRoom;

        public Room ContainingRoom { set { _containingRoom = value;} get { return _containingRoom; } }
        public List <Room> PossibleRooms { get; set; }
        public TrapDoorRoom()
        {
            PossibleRooms = new List<Room>();
            NotificationCenter.Instance.AddObserver("PlayerDidEnterRoom", PlayerDidEnterRoom);
        }

        public Door GetExit(string exitName)
        {
            return null;
        }
        public string Description()
        {
            return "This room has a trap door! You will now be dropped to a random bottom floor room.";
        }
        public void PlayerDidEnterRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player.CurrentRoom == ContainingRoom)
            {
                player.OutputMessage(Description());
                Random random = new Random();
                int r = random.Next(0, PossibleRooms.Count);
                player.CurrentRoom = PossibleRooms[r];
                //player.OutputMessage(player.CurrentRoom.Description());
            }
        }
    }
    public class TrapRoom : RoomDelegate
    {
        
        private string _password;
        public string Password { set { _password = value; } }
        private Door _trickDoor;
        public List<Room> PossibleRooms { get; set; }
        public Dictionary<string, Door> Exits { get; set; }
        private Room _containingRoom;
        public Room ContainingRoom { set { _containingRoom = value; _trickDoor = new Door(_containingRoom, _containingRoom); } get { return _containingRoom; } }

        public TrapRoom()
        {
            Password = "";
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }

        public Door GetExit(string exitName)
        {
            return _trickDoor;
        }
        public string Description()
        {
            return "The doors have all locked. You must guess the keyword to get out of this trap.";
        }
        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player.CurrentRoom == ContainingRoom)
            {
                Dictionary<string, Object> userInfo = notification.userInfo;
                string word = (string)userInfo["word"];
                if (word.Equals(_password))
                {
                    player.OutputMessage("\n You said the right word.");
                    ContainingRoom.Delegate = null;
                }
            }
        }
    }
    /*
    public class EchoRoom : RoomDelegate
    {
        public Dictionary<string, Door> Exits { get; set; }
        private Room _containingRoom;
        public List<Room> PossibleRooms { get; set; }

        public Room ContainingRoom { set { _containingRoom = value; } get { return _containingRoom; } }

        public EchoRoom()
        {
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }

        public Door GetExit(string exitName)
        {
            Door door = null;
            Exits.TryGetValue(exitName, out door);
            
            return door;
        }
        public string Description()
        {
            return "You are in the Echo Room";
        }

        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player.CurrentRoom == ContainingRoom)
            {
                Dictionary<string, Object> userInfo = notification.userInfo;
                string word = (string)userInfo["word"];
                player.OutputMessage("\n" + word + "\n" + word + " ... " + word + " .... " + word + " hahahaha");
            }
        }
    }
    */

    public class Room
    {
        
        private NPC _npc;
        public NPC Character
        {
            get
            {
                return _npc;
            }
            set
            {
                _npc = value;
            }
        }
        
        private Dictionary<string, Door> exits;
        private Dictionary<string, IItem> roomItems;
        private string _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        private RoomDelegate _delegate;
        public RoomDelegate Delegate
        {
            get
            {
                return _delegate;
            }
            set
            {
                _delegate = value;
                if(_delegate != null)
                {
                    _delegate.Exits = exits;
                }
            }
        }
        public Room() : this("No Tag")
        {

        }

        public Room(string tag)
        {
            exits = new Dictionary<string, Door>();
            roomItems = new Dictionary<string, IItem>();
            this.Tag = tag;
            _delegate = null;
            //NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public void SetNPC(NPC npc)
        {
            _npc = npc;
        }
        public void SetExit(string exitName, Door door)
        {
            exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            if (_delegate == null)
            {
                Door door = null;
                exits.TryGetValue(exitName, out door);
                return door;
            }
            else
            {
                return _delegate.GetExit(exitName);
            }
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Door>.KeyCollection keys = exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += " " + exitName;
            }

            return exitNames;
        }
        public void HasItem(IItem item)
        {
            IItem newItem = item;
            if(newItem != null)
            {
                roomItems.Add(newItem.Name, newItem);

            }
        }

        /*
        public IItem GetItem(string itemName)
        {
            IItem target = roomItems[itemName];
            return target;
        }
        */
        public IItem FindRoomItem(string itemName)
        {
            IItem item = null;
            roomItems.TryGetValue(itemName, out item);
            return item;
        }
        public IItem removeItem(string name)
        {
            IItem item = null;
            roomItems.TryGetValue(name, out item);
            roomItems.Remove(name);
            return item;
        }
        public string RoomObjects()
        {
            string items = "This room contains: ";
            Dictionary<string, IItem>.KeyCollection keys = roomItems.Keys;
            foreach(string itemName in keys)
            {
                items += " " + itemName;
            }
            return items;
        }
        public string Description()
        {
            return (_delegate==null?"":_delegate.Description())+" You are now in " + this.Tag + ".\n *** " + this.GetExits()
                + "\n" + (roomItems.Count == 0 ? "" : this.RoomObjects()) +  "\n"+(_npc==null?"":(_npc.Name + " is in this room."));
        }
    }
}
