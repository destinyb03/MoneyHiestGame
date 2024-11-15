using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class Player
    {
        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        public Stack<Room> VisitedRooms { get;  } = new Stack<Room>();
        private IItemContainer _backPack;
        public Player(Room room)
        {
            _currentRoom = room;
            _backPack = new ItemContainer("backPack", 0f);
        }

        public void WaltTo(string direction)
        {
            Room nextRoom = this.CurrentRoom.GetExit(direction);
            if (nextRoom != null)
            {
                VisitedRooms.Push(CurrentRoom);
                CurrentRoom = nextRoom;
                Notification notification = new Notification("PlayerDidEnterRoom", this);
                NotificationCenter.Instance.PostNotification(notification);
                NormalMessage("\n" + this.CurrentRoom.Description());
            }
            else
            {
                ErrorMessage("\nThere is no door on " + direction);
            }
        }
        public void Say(string word)
        {
            NormalMessage(word);
            Notification notification = new Notification("PlayerDidSayAWord", this);
            Dictionary<string, object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            notification.UserInfo = userInfo;
            NotificationCenter.Instance.PostNotification(notification);
        }
        public void Inspect (string itemName)
        {
            IItem pickedUpItem = CurrentRoom.Pickup(itemName);
            if (pickedUpItem != null)
            {
                InfoMessage("\nItem info: " + pickedUpItem.Description);
                NormalMessage("\nItem weight is " + pickedUpItem.Weight) ;
                CurrentRoom.Drop(pickedUpItem);
            }
            else
            {
                ErrorMessage("There is no item named " + itemName + " on the floor");
            }
        }


        public void Inventory()
        {
            NormalMessage(_backPack.Description);
        }

        public void Give(IItem item)
        {
            _backPack.Insert(item);
        }

        public IItem Take(string itemName)
        {
            return _backPack.Remove(itemName);
        }

        public void Pickup(string itemName)
        {
            IItem item = CurrentRoom.Pickup(itemName);
            if (item != null)
            {
                Give(item);
                NormalMessage("You picked up " + itemName);
            }
            else
            {
                ErrorMessage("There is no item named " + itemName + " on the floor");
            }
        }

        public void Drop(string itemName)
        {
            IItem item = Take(itemName);
            if (item != null)
            {
                CurrentRoom.Drop(item);
                NormalMessage("You dropped " + itemName);
            }
            else
            {
                ErrorMessage("There is no item named " + itemName + " in the backpack");
            }
        }

        public void Back ()
        {
            if (VisitedRooms.Count > 1 )
            {
                Room previousRoom = VisitedRooms.Pop();
                CurrentRoom = previousRoom;
                
                Console.WriteLine($"\nYou went back to {CurrentRoom.Description}");
            }
            else
            {
                Console.WriteLine("You cannot go back any further.");
            }
        }

        

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void NormalMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.White);
        }

        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Blue);
        }

        public void WarningMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.DarkYellow);
        }

        public void ErrorMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Red);
        }
    }

}
