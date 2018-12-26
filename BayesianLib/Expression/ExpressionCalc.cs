using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianLib
{
    public class ExpressionCalculator
    {
        #region Fields

        public ExpertSystem ES
        {
            get
            {
                return _es;
            }
            set
            {
                _es = value;
                Events = value.Events;
                CalculatedExpressions = value.CalculatedExpressions;
            }
        }
        private ExpertSystem _es;

        private List<Event> Events;
        private List<Expression> CalculatedExpressions;

        #endregion

        #region Constructors

        public ExpressionCalculator(ExpertSystem es)
        {
            ES = es;
        }

        #endregion

        #region Methods

        public string WriteExpressionsList()
        {
            string list;
            list = "There are precalculated expressions which is used, when we calculating expressions \n";
            foreach (Expression t in CalculatedExpressions)
                list += t.Write() + " = " + t.Probability + "\n";
            return list;
        }

        public void AddAnExpression(Expression exp)
        {
            if (exp.Probability == -1)
                return;

            if (!IfThereAnExpression(exp))
                CalculatedExpressions.Add(exp);

            ES.GetEventsFromExpression(exp);
        }

        internal Expression ReturnTheCalculatedExp(Expression exp)
        {
            foreach (Expression e in CalculatedExpressions)
            {
                bool PossibleEventsSimmilarity = IfTwoListIsSimmilar(e.PossibleEvents, exp.PossibleEvents);
                bool ExactEventsSimmilarity = IfTwoListIsSimmilar(e.ExactEvents, exp.ExactEvents);

                if (PossibleEventsSimmilarity && ExactEventsSimmilarity)
                    return e;
            }
            return null;
        }

        internal List<Expression> ReturnTheListOfExps(Event _event)
        {
            List<Expression> returnable = new List<Expression>();

            foreach (Expression e in CalculatedExpressions)
                if (e.PossibleEvents[0].ReturnName() == _event.ReturnName())
                    returnable.Add(e);

            return returnable;
        }

        internal static bool IfTwoListIsSimmilar(List<Event> first, List<Event> second)
        {
            int firstQuan = first.Count;
            int secondQuan = second.Count;

            if (firstQuan != secondQuan)
                return false;

            List<Event> firstList = EventCalculator.CopyEventList(first).OrderBy(t => t.ReturnName()).ToList();
            List<Event> secondList = EventCalculator.CopyEventList(second).OrderBy(t => t.ReturnName()).ToList();

            for (int i = 0; i < firstQuan;)
                for (int j = 0; j < secondQuan;)
                {
                    if (firstList[i].ReturnName() == secondList[j].ReturnName())
                    {
                        i += 1;
                        j += 1;
                    }
                    else return false;
                }
            return true;
        }

        internal bool IfThereAnExpression(Expression exp)
        {
            if (ReturnTheCalculatedExp(exp) != null)
                return true;
            else
                return false;
        }

        internal double ReturnProbability(Expression exp)
        {
            Expression returnable = ReturnTheCalculatedExp(exp);
            if (returnable != null)
                return returnable.Probability;
            else
                return -1;
        }


        #endregion
    }
}
