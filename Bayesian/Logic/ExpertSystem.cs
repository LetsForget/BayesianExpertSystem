using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Bayesian.Logic
{
    [Serializable]
    public class ExpertSystem
    {
        #region Fields

        public List<Expression> CalculatedExpressions { get; set; }
        [XmlIgnore]
        private List<Event> Events; 

        #endregion

        #region Constructors

        public ExpertSystem()
        {
            CalculatedExpressions = new List<Expression>();
            Events = new List<Event>();
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
        private void FillTheEvents()
        {
            foreach (Expression e in CalculatedExpressions)
               GetEventsFromExpression(e);
        }

        #region Adding new expressions

        public void AddAnExpression(Expression exp)
        {
            if (exp.Probability == -1)
                return;

            if (!IfThereAnExpression(exp))
                CalculatedExpressions.Add(exp);

            GetEventsFromExpression(exp);         
        }

        private Event ReturnExistingEvent(Event _event)
        {
            foreach (Event t in Events)
                if (_event.ReturnClearName() == t.ReturnClearName())
                    return t;
            throw new Exception("There are now such event!");
        }

        private bool IfThereAnEvent(Event _event)
        {
            foreach (Event t in Events)
                if (_event.ReturnClearName() == t.ReturnClearName())
                    return true;
            return false;
        }

        private bool IfThereAnExpression(Expression exp)
        {
            foreach (Expression thisExps in CalculatedExpressions)
            {
                bool PossibleEventsSimmilarity = IfTwoListIsSimmilar(thisExps.PossibleEvents, exp.PossibleEvents);
                bool ExactEventsSimmilarity = IfTwoListIsSimmilar(thisExps.ExactEvents, exp.ExactEvents);

                if (PossibleEventsSimmilarity && ExactEventsSimmilarity)
                    return true;
            }
            return false;
        }

        private static bool IfTwoListIsSimmilar(List<Event> first, List<Event> second)
        {
            int firsttQuan = first.Count;
            int secondQuan = second.Count;

            for (int i = 0; i < firsttQuan; i++)
                for (int j = 0; j < secondQuan; j++)
                {
                    if (first[i] == second[j])
                    {
                        i += 1;
                        j += 1;
                    }
                    else return false;
                }
            return true;
        }

        private void GetEventsFromExpression(Expression exp)
        {
            foreach (Event p in exp.PossibleEvents)
            {
                Event Pos = p;                   // Pos and exact is needed to connect events in expression with Events list in expert system 
                                                 // They are pointers to Events in Events list
                if (!IfThereAnEvent(Pos))
                    Events.Add(Pos);
                else
                    Pos = ReturnExistingEvent(Pos);

                foreach (Event e in exp.ExactEvents)
                {
                    Event Exact = e;

                    if (!IfThereAnEvent(e))
                        Events.Add(e);
                    else
                        Exact = ReturnExistingEvent(Exact);

                    Pos.AddInRelativeList(Exact, Pos.Parents);
                    Exact.AddInRelativeList(Pos, Exact.Childs);
                }

            }
        }

        #endregion

        #region Operations with data

        public void SaveData(string name)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ExpertSystem));

            using (FileStream fs = new FileStream(name, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, this);
            }            
        }

        public static ExpertSystem LoadData(string name)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ExpertSystem));

            using (FileStream fs = new FileStream(name, FileMode.OpenOrCreate))
            {
                ExpertSystem returnable = (ExpertSystem)xml.Deserialize(fs);
                returnable.FillTheEvents();
                return returnable;
            }
        }

        #endregion

        #region Calculation probability

        public string FindProbability(Expression exp)
        {
            string answer = exp.Write();
            
            if (exp.Probability == -1)

        }
        public string FindProbability(Expression exp, string answer)
        {

        }
        private List<Expression> CalculateExpression(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");
            // Здесь надо сделать проверку есть ли данное выражение в списке уже вычисленных выражений, если есть, то остановить рекурсию
            #region Checking the first item, if there are a conjuction in possible events (more than one possible event exists)

            if (exp.PossibleEvents.Count > 1)
            {
                First(exp);
                foreach (Expression t in exp.ChildExpressions)
                    FindProbability(t);

                exp.Probability = exp.ChildExpressions[0].Probability;
                exp.Probability *= exp.ChildExpressions[1].Probability;
               // return;
            }

            #endregion

            #region Checking the second and the third item, if there are the same event in possible and exact events

            foreach (Event e in exp.ExactEvents)
            {
                if (e.ReturnName() == exp.PossibleEvents[0].ReturnName())
                {
                    Second(exp);
                    return;
                }
                if (e.ReturnClearName() == exp.PossibleEvents[0].ReturnClearName())
                {
                    Third(exp);
                    return;
                }
            }

            #endregion

            #region Checking the fourth item, if there possible event with false sign

            if (exp.PossibleEvents[0].Sign == false)
            {
                Fourth(exp);
                return;
            }

            #endregion

            #region Checking the fifth item, if there are a child in exact events to posiible event
            // if (exp.is)
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
            //exp.ChildExpressions.Add(first);
            //exp.ChildExpressions.Add(second);
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

        public void Sixth_A(Expression exp)
        {

        }

        public void Sixth_B(Expression exp)
        {

        }

        private static List<Event> CopyEventList(List<Event> copyable)
        {
            List<Event> copiedList = new List<Event>();
            foreach (Event t in copyable)
            {
                Event copyOfEvent = t.ReturnCopy();
                copiedList.Add(copyOfEvent);
            }
            return copiedList;
        }

        #endregion

        #endregion

        #endregion
    }
}
