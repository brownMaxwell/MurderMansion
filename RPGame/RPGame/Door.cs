using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
   /*
    * Didn't change this from class as it worked well for my needs as is.
    */
    public class RegularLock : ILocking
    {
        private bool _islocked;
        public bool IsLocked { get { return _islocked; } }
        public bool IsUnlocked { get { return !_islocked; } }
        public bool MayOpen { get { return _islocked ? false : true; } }
        public bool MayClose { get { return _islocked ? true : true; } }

        public RegularLock()
        {
            _islocked = false;
        }

        public void Lock()
        {
            _islocked = true;
        }
        public void Unlock()
        {
            _islocked = false;
        }
       



    }
    public class Door : ILocking
    {
        public bool IsLocked { get { return _lock != null ? _lock.IsLocked : false; } }
        public bool IsUnlocked { get { return _lock != null ? _lock.IsUnlocked : true; } }

        public void Lock()
        {
            if(_lock != null)
            {
                _lock.Lock();
            }
        }
        public void Unlock()
        {
            if(_lock != null)
            {
                _lock.Unlock();
            }
        }
        public bool MayOpen { get { return _lock != null ? _lock.MayOpen : true; } }
        public bool MayClose { get { return _lock != null ? _lock.MayClose : true; } }
        private Room _room1;
        private Room _room2;
        public Room Room1
        {
            get
            {
                return _room1;
            }
        }
        public Room Room2
        {
            get
            {
                return _room2;
            }
        }
        private ILocking _lock;

        private bool _closed;
        public bool Closed { get { return _closed; } }

        public bool Open { get { return !_closed; } }

        public Door(Room room1, Room room2)
        {
            _room1 = room1;
            _room2 = room2;
            _lock = null;
        }

        public Room otherRoom(Room thisRoom)
        {
            return thisRoom == _room1 ? _room2 : _room1; //ternary
        }

        public static Door connectRooms(Room fromRoom, Room toRoom, string fromTo, string toFrom)
        {
            Door door = new Door(fromRoom, toRoom);
            fromRoom.SetExit(toFrom, door);
            toRoom.SetExit(fromTo, door);
            return door;
        }

        public void Close()
        {
            if(_lock != null)
            {
                _closed = _lock.MayClose;
            }
            else
            {
                _closed = true;
            }

        }

        public void open()
        {
            if(_lock != null)
            {
                _closed = !_lock.MayOpen;
            }
            else
            {
                _closed = false;
            }
        }

        public void InstallLock(ILocking newLock)
        {
            _lock = newLock;
        }
    }
}
