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
        public void FindProbability(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            #region Checking the first item, if there are a conjuction in possible events (more than one possible event exists)
            if (exp.PossibleEvents.Count > 0)                    
            {
                First(exp);
                foreach (Expression t in exp.ChildExpressions)
                    FindProbability(t);

                exp.Probability = exp.ChildExpressions[0].Probability;
                exp.Probability *= exp.ChildExpressions[1].Probability;
                return;
            }
            #endregion

            #region Checking the second item, if there are the same event in possible and exact events
            if (exp.ExactEvents.Contains(exp.PossibleEvents[0])) 
            {
                Second(exp);
                return;
            }
            #endregion

            #region Checking the third item, if there are the same event with different signs in possible and exact events
            if (exp.ExactEvents.Where(t => t.ReturnClearName() == exp.PossibleEvents[0].ReturnClearName()).Count() > 0)
            {
                Third(exp);
                return;
            }
            #endregion

            #region Checking the fourth item, if there possible event with false sign
            if (exp.PossibleEvents[0].Sign == false)
            {
                Fourth(exp);
                FindProbability(exp.ChildExpressions[1]);
                exp.Probability = exp.ChildExpressions[0].Probability - exp.ChildExpressions[1].Probability;
                return;
            }
            #endregion

        }
        #region Expressions calculations
        public void First(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            List<Event> possibleEvents = CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = CopyEventList(exp.ExactEvents);

            List<Event> f_possibleEvents = possibleEvents.GetRange(0, 1);
            List<Event> f_exactEvents = new List<Event>(exactEvents); 
            Expression first = new Expression
            {
                PossibleEvents = f_possibleEvents,
                ExactEvents = f_exactEvents
            };

            List<Event> s_possibleEvents = new List<Event>(possibleEvents);
            s_possibleEvents.RemoveAt(0);

            List<Event> s_exactEvents = new List<Event>();
            s_exactEvents.Add(possibleEvents[0]);
            s_exactEvents.InsertRange(1, exactEvents);

            Expression second = new Expression()
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            };
            exp.ChildExpressions.Add(first);
            exp.ChildExpressions.Add(second);
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

            List<Event> s_possibleEvents = CopyEventList(exp.PossibleEvents);
            s_possibleEvents[0].Sign = true;
            List<Event> s_exactEvents = CopyEventList(exp.ExactEvents);

            Expression second = new Expression
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            };
        }

        public void Fifth_A(Expression exp)
        {
            List<Event> possibleEvents = CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = CopyEventList(exp.ExactEvents);

            Expression first = new Expression()
            {
                PossibleEvents = possibleEvents
            };

            Expression second = new Expression()
            {
                PossibleEvents = exactEvents,
                ExactEvents = possibleEvents
            };
            Expression third = new Expression()
            {
                PossibleEvents = exactEvents
            };
        }

        public void Fifth_B(Expression exp)
        {

        }

        public void Sixth(Expression exp)
        {

        }
        #endregion

        #region Misc

        private static List<Event> CopyEventList(List<Event> Copyable)
        {
            List<Event> copiedList = new List<Event>();
            foreach (Event t in Copyable)
            {
                Event copyOfEvent = t.ReturnCopy();
                copiedList.Add(copyOfEvent);
            }
            return copiedList;
        }

        #endregion

        #endregion
    }
}
