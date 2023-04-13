using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    public class Item : IItem
    {
        private string _name;
        private float _weight;
        private float _volume;
        private bool _canTake;
        private bool _firstInspection;
        private IItem _decoration;
        public IItem Decoration
        {
            get
            {
                return _decoration;
            }
        }

        public virtual string LongDescription
        {
            get
            {
                return "Name: " + LongName + ", Weight: " + Weight + ", Volume: " + Volume;
            }
        }
        public string LongName { get { return Name + (_decoration != null ? ", " + _decoration.LongName : ""); } }
        public string Name { get { return _name; } set { _name = value; } }
        public virtual float Weight { get { return _weight + (_decoration != null ? _decoration.Weight : 0); } set { _weight = value; } }
        public virtual float Volume { get { return _volume + (_decoration != null ? _decoration.Volume : 0); } set { _volume = value; } }
        public virtual bool CanTake
        {
            get
            {
                return _canTake;
            }
            set
            {
                _canTake = value;
            }
        }
        public bool FirstInspection
        {
            get
            {
                return _firstInspection;
            }
            set
            {
                _firstInspection = value;
            }
        }
        public Room ForRoom { get; set; }
        public Item() : this("No Name") { }

        public Item(string name) : this(name, 1.0f) { }
        public Item(string name, float weight) : this(name, weight, 1.0f) { }
        public Item(string name, float weight, float volume) : this(name, weight, volume, true) { }

        public Item(string name, float weight, float volume, bool canTake)
        {
            Name = name;
            Weight = weight;
            Volume = volume;
            CanTake = canTake;
            FirstInspection = true;
            _decoration = null;
            ForRoom = null;
        }

        public virtual string Description()
        {
            return Name + " " + Weight + " " + Volume;
        }

        public void addDecorator(IItem decoration)
        {
            if (_decoration == null)
            {
                _decoration = decoration;

            }
            else
            {
                _decoration.addDecorator(decoration);
            }
        }
        public virtual IItem Clone()
        {
            Item clone = (Item)this.MemberwiseClone();
            clone._name = "" + _name;
            clone._weight = _weight;
            clone._decoration = _decoration != null ? _decoration.Clone() : null;
            return clone;
        }
        public virtual void AddItem(IItem item)
        {

        }

        public virtual IItem RemoveItem(string name)
        {
            return null;
        }
    }

    class ItemContainer : Item
    {
        private Dictionary<string, IItem> items;
        override
            public float Weight
        {
            get
            {
                float tempWeight = base.Weight;
                foreach (IItem item in items.Values)
                {
                    tempWeight += item.Weight;
                }
                return tempWeight;
            }
        }
        override
            public float Volume
        {
            get
            {
                float tempVol = base.Volume;
                foreach(IItem item in items.Values)
                {
                    tempVol += item.Volume;
                }
                return tempVol;
            }
        }
        public ItemContainer(string name, float weight, float volume, bool canTake) : base(name, weight, volume, canTake)
        {
            items = new Dictionary<string, IItem>();
        }

        override
            public void AddItem(IItem Item)
        {
            items[Item.Name] = Item;
        }
        override
            public IItem RemoveItem(string name)
        {
            IItem item = null;
            items.TryGetValue(name, out item);
            items.Remove(name);
            return item;
        }
        public void Open(Room room)
        {
            foreach(IItem item in items.Values)
            {
                room.HasItem(RemoveItem(item.Name));
            }
        }
        override
            public string LongDescription
        {
            get
            {
                string description = base.LongDescription;
                foreach (IItem item in items.Values)
                {
                    description += "\t - " + item.LongDescription + "\n";
                }
                return description;
            }
        }
    }

    public abstract class Weapon : Item
    {
        public Weapon() : this("No name") { }
        public Weapon(string name) : this(name, 1.0f) { }
        public Weapon(string name, float weight) : base (name, weight) { }
        public Weapon(string name, float weight, float volume, bool canTake) : base(name, weight, volume, canTake) { }
        public abstract void Load();
        public abstract void Prepare();
        public abstract void Fire(IPeople target);
        public void Use(IPeople target)
        {
            Load();
            Prepare();
            Fire(target);
        }
    }

    public class Revolver : Weapon
    {
        private int _accuracy;
        public int Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = value;
            }
        }
        private int _loadedAmmo;
        public Revolver(string name, float weight, float volume, bool canTake) : base(name, weight,volume, canTake)
        {
            _loadedAmmo = 0;
            _accuracy = 0;
        }
        override
            public void Load()
        {
            _loadedAmmo = 6;
        }
        override 
            public void Prepare()
        {
            Random random = new Random();
            _accuracy = random.Next(1, 10);
            Console.Out.WriteLine(_accuracy);
            if(this.Decoration != null && this.Decoration.Name == "Scope")
            {
                _accuracy += 2;
            }
            
        }
        override
        public void Fire(IPeople target)
        {
            if(_accuracy > target.Size)
            {
                _loadedAmmo--;
                target.TakeDamage(100.0f);

            }
            else
            {
                target.OutputMessage("Missed!");
            }
            
        }
    }

    public class Pistol : Weapon
    {
        private int _accuracy;
        public int Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = value;
            }
        }
        private int _loadedAmmo;
        public Pistol(string name, float weight, float volume, bool canTake) : base(name, weight, volume, canTake)
        {
            _loadedAmmo = 0;
            _accuracy = 0;
        }
        override
            public void Load()
        {
            _loadedAmmo = 14;
        }
        override
            public void Prepare()
        {
            Random random = new Random();
            _accuracy += random.Next(1, 15);
            if (this.Decoration != null && this.Decoration.Name == "Scope")
            {
                _accuracy += 2;
                Console.Out.WriteLine(_accuracy);
            }

        }
        override
        public void Fire(IPeople target)
        {
            if (_accuracy > target.Size)
            {
                _loadedAmmo--;
                target.TakeDamage(100.0f);
                if(target.Health > 0.0f)
                {
                    target.OutputMessage(target.Name + " now has " + target.Health + " health left.");
                }
            }
            else
            {
                target.OutputMessage("Missed!");
            }

        }
    }

    public class Key : Item
    {
        private Room _forRoom;
        public Room ForRoom
        {
            get
            {
                return _forRoom;
            }
            set
            {
                _forRoom = value;
            }
        }
        public Key(string name, float weight, float volume, bool canTake, Room forRoom) : base(name, weight, volume, canTake)
        {
            _forRoom = forRoom;
        }
    }
}

