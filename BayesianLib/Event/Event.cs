using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace BayesianLib
{
    public class Event
    {
        #region Fields
        public string Name
        {
            get
            {
                //if (Sign == true)
                //    return _name;
                //return "¬" + _name;
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private string _name;
        [XmlIgnore]
        public List<Event> Parents;
        [XmlIgnore]
        public List<Event> Childs;
        public bool Sign;
        #endregion

        #region Constructors

        public Event()
        {
            Parents = new List<Event>();
            Childs = new List<Event>();
        }

        public Event(string description)
        {
            if (description == null)
                throw new Exception("Empty event!");

            Parents = new List<Event>();
            Childs = new List<Event>();

            if (description.First() == '-' || description.First() == '¬')
            {
                Sign = false;
                Name = description.Remove(0, 1);
                return;
            }
            Sign = true;
            Name = description;   
        }

        #endregion

        #region Methods

        public void AddInRelativeList(Event _event, List<Event> RelativeList)
        {
            if (_event == null || _event.Name == null || RelativeList == null)
                throw new Exception("Empty event or list");

            foreach (Event p in RelativeList)
                if (p.ReturnClearName() == _event.ReturnClearName())
                    return;

            RelativeList.Add(_event);
        }
            
        public Event ReturnCopy()
        {
            
            Event copy = new Event(Name);
            copy.Sign = Sign;
            return copy;
        }

        public string ReturnName()
        {
            if (Sign == true)
                return _name;
            return "¬" + _name;
        }

        public string ReturnClearName()
        {
            return _name;
        }

        #endregion
    }
}
