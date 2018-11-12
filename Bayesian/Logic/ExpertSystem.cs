using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian.Logic
{
    public class ExpertSystem
    {
        #region Fields
        private List<Expression> CalculatedExpressions;
        private List<Event> PossibleEvents;
        #endregion

        #region Constructors
        public ExpertSystem()
        {
            CalculatedExpressions = new List<Expression>();
            PossibleEvents = new List<Event>();
        }
        #endregion

        #region Methods
        public void AddAnEvent(string description)
        {
            if (description == null)
                throw new Exception("Null description");

            bool ifPositiveEventAlready = PossibleEvents.Where(t => t.Name == description).Count() > 0;
            bool ifNegativeEventAlready = PossibleEvents.Where(t => t.Name == description.Remove(0, 1)).Count() > 0;

            if (ifPositiveEventAlready | ifNegativeEventAlready)
                return;

            if (description[0] == '-')
            {
                PossibleEvents.Add(new Event(description.Remove(0, 1)));
                return;
            }

            PossibleEvents.Add(new Event(description));
        }

        public void AddAnEvent(Event _event)
        {
            bool ifPositiveEventAlready = PossibleEvents.Where(t => t.Name == _event.Name).Count() > 0;
            bool ifNegativeEventAlready = PossibleEvents.Where(t => t.Name == _event.Name.Remove(0, 1)).Count() > 0;

            if (ifPositiveEventAlready | ifNegativeEventAlready )
                return;

            if (_event.Sign == false)
            {
                PossibleEvents.Add(new Event(_event.Name.Remove(0, 1)));
                return;
            }

            PossibleEvents.Add(_event);
        }

        public void AddAnExpression(Expression exp)
        {
            if (exp.Probability == -1)
                return;

            if (!CalculatedExpressions.Contains(exp))
                CalculatedExpressions.Add(exp);
        }
  
        
        #endregion
    }
}
