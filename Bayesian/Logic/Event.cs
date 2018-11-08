using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian.Logic
{
    public class Event
    {
        #region Fields
        public string Name
        {
            get
            {
                if (Sign == true)
                    return _name;
                return "¬" + _name;
            }
            set
            {
                _name = value;
            }
        }
        private string _name;
        public bool Sign;
        #endregion

        #region Constructors
        public Event(string description)
        {
            if (description == null)
                throw new Exception("Empty event!");

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
    }
}
