using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarterGame
{
    
    public class GameWorld
    {
        private static GameWorld _instance;
        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
        private Room _entrance;
        public Room Entrance { get { return _entrance; } }
        //private Room _triggerRoom;
        //private Room _worldOut;
        //private Room _worldInAnnex;
        private Dictionary<Room, WorldEvent> worldEvents;
        public GameWorld()
        {
            worldEvents = new Dictionary<Room, WorldEvent>();
            _entrance = CreateWorld();
            NotificationCenter.Instance.AddObserver("playerDidEnterRoom", PlayerDidEnterRoom);
        }
        public void PlayerDidEnterRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player != null)
            {
                WorldEvent we = null;
                worldEvents.TryGetValue(player.CurrentRoom, out we);
                if (we != null)
                {
                    we.ExecuteEvent();
                    player.WarningMessage("\n***We changed the world.***");
                }
                /*if(player.CurrentRoom == _triggerRoom)
                {
                    _worldOut.SetExit("west", _worldInAnnex);
                    _worldInAnnex.SetExit("east", _worldOut);
                    player.WarningMessage("\n***We changed the world.*** ");

                }*/
            }
            //Console.WriteLine("\n***Player entered a room***\n")
        }
        private class WorldEvent
        {
            private Room _trigger;
            public Room Trigger { get { return _trigger; } }
            private Room _worldOut;
            private Room _worldInAnnex;
            private string _directionFromWorld;
            private string _directionToWorld;

            public WorldEvent(Room trigger, Room worldOut, Room worldInAnnex, string directionFromWorld, string directionToWorld)
            {
                _trigger = trigger;
                _worldOut = worldOut;
                _worldInAnnex = worldInAnnex;
                _directionFromWorld = directionFromWorld;
                _directionToWorld = directionToWorld;
            }

            public void ExecuteEvent()
            {
                _worldOut.SetExit(_directionFromWorld, _worldInAnnex);
                _worldInAnnex.SetExit(_directionToWorld, _worldOut);
            }
        }
        private Room CreateWorld()
        {
            Room outside = new Room("outside the main entrance of the university");
            Room scctparking = new Room("in the parking lot at SCCT");
            Room boulevard = new Room("on the boulevard");
            Room universityParking = new Room("in the parking lot at University Hall");
            Room parkingDeck = new Room("in the parking deck");
            Room scct = new Room("in the SCCT building");
            Room theGreen = new Room("in the green in from of Schuster Center");
            Room universityHall = new Room("in University Hall");
            Room schuster = new Room("in the Schuster Center");

            //Annex world
            Room davidson = new Room("in Davidson Student Center");
            Room clockTower = new Room("at the Clock Tower");
            Room woodHall = new Room("in Woodhall");
            Room greekCenter = new Room("at the Greek Center");
            outside.SetExit("west", boulevard);

            boulevard.SetExit("east", outside);
            boulevard.SetExit("south", scctparking);
            boulevard.SetExit("west", theGreen);
            boulevard.SetExit("north", universityParking);

            scctparking.SetExit("west", scct);
            scctparking.SetExit("north", boulevard);

            scct.SetExit("east", scctparking);
            scct.SetExit("north", schuster);

            schuster.SetExit("south", scct);
            schuster.SetExit("north", universityHall);
            schuster.SetExit("east", theGreen);

            theGreen.SetExit("west", schuster);
            theGreen.SetExit("east", boulevard);

            universityHall.SetExit("south", schuster);
            universityHall.SetExit("east", universityParking);

            universityParking.SetExit("south", boulevard);
            universityParking.SetExit("west", universityHall);
            universityParking.SetExit("north", parkingDeck);

            parkingDeck.SetExit("south", universityParking);

            //Build Annex
            davidson.SetExit("west", clockTower);

            clockTower.SetExit("east", davidson);
            clockTower.SetExit("north", greekCenter);
            clockTower.SetExit("south", woodHall);

            
            greekCenter.SetExit("south", clockTower);
            woodHall.SetExit("north", clockTower);

            //Set up room delgates
            TrapRoom tp = new TrapRoom();
            scct.RoomDelegate = tp;

            //Set up World Events
            WorldEvent we = new WorldEvent(parkingDeck, schuster, davidson, "west", "east");
            worldEvents[we.Trigger] = we;

            //Set up special rooms
            //_triggerRoom = parkingDeck;
            //_entrance = outside;
            //_worldOut = schuster;
            //_worldInAnnex = davidson;

            //set up items
            IItem item = new Item("dagger");
            parkingDeck.Drop(item);

            IItem decorator = new Item("gem", 0.5f);
            item.Decorate(decorator);
            decorator = new Item("gold", 0.7f);
            item.Decorate(decorator);

            IItemContainer chest = new ItemContainer("chest", 0f);
            scct.Drop(chest);
            item = new Item("ball", 0.5f);
            chest.Insert(item);
            item = new Item("bat", 3.5f);
            chest.Insert(item);

            return outside;
        }


    }
}
