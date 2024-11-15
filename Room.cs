using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2024
     */
    public class Room
    {
        private Dictionary<string, Room> _exits;
        private string _tag;
        public IItem _itemOnFloor;
        public string Tag
        {
            get { return _roomDelegate == null ? _tag : _roomDelegate.OnTag(_tag); }
            set { _tag = value; }
        }
        private IRoomDelegate _roomDelegate;


        public IRoomDelegate RoomDelegate
        {
            get { return _roomDelegate; }
            set { _roomDelegate = value; }
        }

        private IItemContainer _itemsOnFloor;
        public Room() : this("No Tag") { }

        // Designated Constructor
        public Room(string tag)
        {
            _itemsOnFloor = new ItemContainer("floor", 0f);
            _roomDelegate = null;
            _exits = new Dictionary<string, Room>();
            this.Tag = tag;
            
        }
        public IItem GetItemOnFloor()
        {
            return _itemOnFloor;
        }

        public void SetItemOnFloor(IItem item)
        {
            _itemOnFloor = item;
        }
        public IItem Drop (IItem item)
        {
            IItem oldItem = _itemOnFloor;
            _itemsOnFloor.Insert(item);
            return oldItem;
        }

        public IItem Pickup(string itemName)
        {
            return _itemsOnFloor.Remove(itemName);

        }
        public void SetExit(string exitName, Room room)
        {
            _exits[exitName] = room;
        }

        public Room GetExit(string exitName)
        {
            Room room = null;
            _exits.TryGetValue(exitName, out room);
            if (_roomDelegate != null)
            {
                room = _roomDelegate.OnGetExit(room);
            }
            return room;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Room>.KeyCollection keys = _exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += " " + exitName;
            }

            return exitNames;
        }

        public string Description()
        {
            return "You are in " + this.Tag + ".\n *** " + this.GetExits() + "\nItem: " + _itemsOnFloor.Description;

        }
    }
    public class TrapRoom : IRoomDelegate
    {
        private bool _active;
        private Room _containingRoom;
        public Room ContainingRoom
        {
            set
            {
                _containingRoom = value;

            }
            get
            {
                return _containingRoom;
            }
        }
        public TrapRoom()
        {
            _active = true;
            //NotificationCenter.Instance.AddObserver("PlayerDidSayAWord", PlayerDidSayAWord);
        }
        public Room OnGetExit(Room room)
        {
            return _active ? null : room;
        }

        public string OnTag(string tag)
        {
            return tag + (_active ? "You are in a trap room!" : "");
        }

        public void PlayerDidSayAWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player != null)
            {
                if (player.CurrentRoom == ContainingRoom)
                {
                    Dictionary<string, object> userInfo = notification.UserInfo;
                    if (userInfo != null)
                    {
                        object spokenWordObject = null;
                        userInfo.TryGetValue("word", out spokenWordObject);
                        if (spokenWordObject != null)
                        {
                            string spokenWord = (string)spokenWordObject;
                            if (spokenWord.Equals("escape"))
                            {
                                _active = false;
                                player.InfoMessage("You disabled the trap");
                            }
                            else
                            {
                                player.ErrorMessage("You didn't say the magic word. Ah, ah, ah");
                            }
                        }
                    }
                }
            }
        }
    }
}

