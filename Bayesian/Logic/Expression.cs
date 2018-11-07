using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian.Logic
{
    public class Expression
    {
        #region Fields
        public List<Event> PossibleEvents;
        public List<Event> ExactEvents;
        public double Probability;
        #endregion

        #region Constructors
        public Expression()
        {
            PossibleEvents = new List<Event>();
            ExactEvents = new List<Event>();
            Probability = -1;
        }
        #endregion

        #region Methods
        public string Write()
        {
            string temp;
            temp = "p(";
            foreach (Event t in PossibleEvents)
                temp += " " + t.Name + " &";
            temp = temp.Remove(temp.LastIndexOf("&"), 1);
            temp += " | ";
            foreach (Event t in ExactEvents)
                temp += " " + t.Name + " &";
            temp = temp.Remove(temp.LastIndexOf("&"), 1);
            temp += ")";
            return temp;
        }
        #endregion
    }
}
