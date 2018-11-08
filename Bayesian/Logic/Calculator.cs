using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian.Logic
{
    public class Calculator
    {
        #region Constructors
        public Calculator()
        {

        }
        #endregion

        #region Methods
        public void First(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            List<Event> f_possibleEvents = exp.PossibleEvents.GetRange(0, 1);
            List<Event> f_exactEvents = new List<Event>(exp.ExactEvents); 
            Expression first = new Expression
            {
                PossibleEvents = f_possibleEvents,
                ExactEvents = f_exactEvents
            };

            List<Event> s_possibleEvents = new List<Event>(exp.PossibleEvents);
            s_possibleEvents.RemoveAt(0);

            List<Event> s_exactEvents = new List<Event>();
            s_exactEvents.Add(exp.PossibleEvents[0]);
            s_exactEvents.InsertRange(1, exp.ExactEvents);

            Expression second = new Expression()
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            };
        }

        public void Second(Expression exp)
        {
            exp.Probability = 1;
        }
        public void Third(Expression exp)
        {
            exp.Probability = 0;
        }
        public void Fourth(Expression exp)
        {
            Expression first = new Expression
            {
                Probability = 1
            };

            List<Event> s_possibleEvents = new List<Event>(exp.PossibleEvents);
            s_possibleEvents[0].Sign = true;
            List<Event> s_exactEvents = new List<Event>(exp.ExactEvents);

            Expression second = new Expression
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            };
        }
        #endregion
    }
}
