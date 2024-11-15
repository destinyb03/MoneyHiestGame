using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarterGame
{
    public interface IRoomDelegate
    {
        public Room ContainingRoom { set; get; }
        public Room OnGetExit(Room room);
        public string OnTag(string tage);
    }
    public interface IItem
    {
        public string Name { get; set; }
        public float Weight { get; }
        public string Description { get; }
        public void Decorate(IItem decorator);
        public bool IsContainer { get; }
    }

        public interface IItemContainer : IItem
        {
            public bool Insert(IItem item);
            public IItem Remove(string itemName);
        }
    }

