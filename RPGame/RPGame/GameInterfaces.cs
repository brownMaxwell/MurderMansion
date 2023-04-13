using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    public interface IPeople
    {
        string Name { get; }
        int Size { get; set; }
        float Health { get; set; }
        Room CurrentRoom { get; set; }
        Dictionary<string, IItem> Inventory { get; set; }
        Dictionary<string, string> DialogueOptions { get; set; }
        void TakeDamage(float damage);
        void OutputMessage(string message);
    }
    public interface ILocking
    {
        bool IsLocked { get; }
        bool IsUnlocked { get; }

        void Lock();
        void Unlock();
        bool MayOpen { get; }
        bool MayClose { get; }
    }
    public interface WorldEvent
    {
        void Execute();
    }
    public interface IItem
    {
        string Name { get; set; }
        Room ForRoom { get; set; }
        float Weight { get; set; }
        float Volume { get; set; }
        bool CanTake { get; set; }
        bool FirstInspection { get; set; }
        string LongDescription { get; }
        string LongName { get; }
        void addDecorator(IItem decor);
        void AddItem(IItem item);
        IItem RemoveItem(string name);
        string Description();

        IItem Clone();
    }

    public interface RoomDelegate
    {

        string Description();
        Door GetExit(string exitName);
        Room ContainingRoom { set; get; }
        List<Room> PossibleRooms { get; set; }

        Dictionary<string, Door> Exits { get; set; }
    }
    public interface GameInterfaces
    {
       
    }
}
