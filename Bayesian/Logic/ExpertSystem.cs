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

        #region Operations with events and expressions

        #region Events

        private void FillTheEvents()
        {
            foreach (Expression e in CalculatedExpressions)
                GetEventsFromExpression(e);
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

        private Event ReturnExistingEvent(Event _event)
        {
            foreach (Event t in Events)
                if (_event.ReturnClearName() == t.ReturnClearName())
                    return t;
            return null;
        }

        private bool IfThereAnEvent(Event _event)
        {
            if (ReturnExistingEvent(_event) == null)
                return false;
            else
                return true;
        }

        private bool IfEventIsParent(Event possible, Event exact) // Доделать здеся проверку
        {
            Event possibleES = ReturnExistingEvent(possible);

            if (possibleES == null)
                throw new Exception("No such event");

            int parentsQuan = possibleES.Parents.Count;

            if (parentsQuan == 0)
                return false;

            foreach (Event e in possibleES.Parents)
                if (e.ReturnName() == exact.ReturnName())
                    return true;

            foreach (Event e in possibleES.Parents)
                return IfEventIsParent(e, exact);
            return false;
        }
        
        private bool IfEventIsParent(Expression exp)
        {
            if (exp.PossibleEvents.Count == 0 || exp.ExactEvents.Count == 0)
                return false;

            Event possible = exp.PossibleEvents[0];
            List<bool> exacts_b = new List<bool>();
            foreach (Event e in exp.ExactEvents)
                exacts_b.Add(IfEventIsParent(possible,e));

            int quantOftrue = exacts_b.Where(t => t == true).Count();
            if (quantOftrue > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Expressions

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

            GetEventsFromExpression(exp);
        }

        private Expression ReturnTheCalculatedExp(Expression exp)
        {
            foreach (Expression thisExps in CalculatedExpressions)
            {
                bool PossibleEventsSimmilarity = IfTwoListIsSimmilar(thisExps.PossibleEvents, exp.PossibleEvents);
                bool ExactEventsSimmilarity = IfTwoListIsSimmilar(thisExps.ExactEvents, exp.ExactEvents);

                if (PossibleEventsSimmilarity && ExactEventsSimmilarity)
                    return thisExps;
            }
            return null;
        }

        private bool IfThereAnExpression(Expression exp)
        {
            if (ReturnTheCalculatedExp(exp) != null)
                return true;
            else
                return false;
        }

        private double ReturnProbability(Expression exp)
        {
            Expression returnable = ReturnTheCalculatedExp(exp);
            if (returnable != null)
                return returnable.Probability;
            else
                return -1;
        }

        private static bool IfTwoListIsSimmilar(List<Event> first, List<Event> second)
        {
            int firstQuan = first.Count;
            int secondQuan = second.Count;

            if (firstQuan != secondQuan)
                return false;

            List<Event> firstList = CopyEventList(first).OrderBy(t => t.ReturnName()).ToList();
            List<Event> secondList = CopyEventList(second).OrderBy(t => t.ReturnName()).ToList();

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

        #endregion

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

        public void CalculateExpression(Expression exp, ref string answer)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            if (IfThereAnExpression(exp) == true)
            {
                exp.Probability = ReturnProbability(exp);
                return;
            }

            answer += "\n";
            answer += exp.Write();

            #region Checking the first item, if there are a conjuction in possible events (more than one possible event exists)

            if (exp.PossibleEvents.Count > 1)
            {
                answer += " = (1) ";
                First(exp);
                exp.Probability = 1;
                string firstChild_s = exp.ChildExpressions[0].Write();
                string seconChild_s = exp.ChildExpressions[1].Write();
                answer += firstChild_s + " * " + seconChild_s;
                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    exp.Probability *= e.Probability;
                }
                answer += "\n" + exp.Write() + " = ";
                string firstChild_prob = exp.ChildExpressions[0].Probability.ToString();
                string seconChild_prob = exp.ChildExpressions[1].Probability.ToString();
                string exp_prob = exp.Probability.ToString();
                answer += firstChild_prob + " * " + seconChild_prob + " = " + exp_prob;
                return;
            }

            #endregion

            #region Checking the second and the third item, if there are the same event in possible and exact events

            foreach (Event e in exp.ExactEvents)
            {
                if (e.ReturnName() == exp.PossibleEvents[0].ReturnName())
                {
                    answer += " = (2) ";
                  //  Second(exp);
                    exp.Probability = 1;
                    string expProba_s = exp.Probability.ToString();
                    answer += expProba_s;
                    return;
                }
                if (e.ReturnClearName() == exp.PossibleEvents[0].ReturnClearName())
                {
                    answer += " = (3) ";
                //    Second(exp);
                    exp.Probability = 0;
                    string expProba_s = exp.Probability.ToString();
                    answer += expProba_s;
                    return;
                }
            }

            #endregion

            #region Checking the fourth item, if there possible event with false sign

            if (exp.PossibleEvents[0].Sign == false)
            {
                answer += " = (4) ";
                Fourth(exp);
                string expChild_s = exp.ChildExpressions[1].Write();
                answer += "1 - " + expChild_s;

                CalculateExpression(exp.ChildExpressions[1], ref answer);
                double expChildProb = exp.ChildExpressions[1].Probability;
                string expChildProb_s = expChildProb.ToString();
                exp.Probability = 1 - expChildProb;
                answer += "1 - " + expChildProb_s;
                return;
            }

            #endregion

            #region Checking the fifth item, if there are a child in exact events to posiible event

            #region Five point A

            if (exp.ExactEvents.Count() < 2 && IfEventIsParent(exp)) 
            {
                answer += " = (5a) ";
                Fifth_A(exp);
                List<string> expChild_s = new List<string>();
                List<double> expChild_prob = new List<double>();

                foreach (Expression e in exp.ChildExpressions)
                    expChild_s.Add(e.Write());
                answer += expChild_s[0] + " * (" + expChild_s[1] + " / " + expChild_s[2] + ") ";

                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    expChild_prob.Add(e.Probability);
                }
                answer += exp.Write() + " = " + expChild_prob[0].ToString() + " * (" +
                                                expChild_prob[0].ToString() + " / " +
                                                expChild_prob[0].ToString() + " )" + "=" +
                                                exp.Probability.ToString();
                return;
            }

            #endregion

            #region Five point B

            if (IfEventIsParent(exp))
            {
                answer += " = (5b) ";
                Fifth_B(exp);
                List<string> expChild_s = new List<string>();
                List<double> expChild_prob = new List<double>();

                foreach (Expression e in exp.ChildExpressions)
                    expChild_s.Add(e.Write());
                answer += expChild_s[0] + " * (" + expChild_s[1] + " / " + expChild_s[2] + ") ";

                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    expChild_prob.Add(e.Probability);
                }
                answer += exp.Write() + " = " + expChild_prob[0].ToString() + " * (" +
                                                expChild_prob[0].ToString() + " / " +
                                                expChild_prob[0].ToString() + " )" + "=" +
                                                exp.Probability.ToString();
                return;
            }

            #endregion

            #endregion

            #region Checking the sixth item
            // Ты, наверное, сам знаешь что делать ))))0))0)))
            #endregion
        }

        #region Expressions calculations

        public void First(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            exp.ChildExpressions = new List<Expression>();
            List<Event> possibleEvents = CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = CopyEventList(exp.ExactEvents);

            List<Event> f_possibleEvents = possibleEvents.GetRange(0, 1);
            List<Event> f_exactEvents = new List<Event>(exactEvents);

            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = f_possibleEvents,
                ExactEvents = f_exactEvents
            });
            List<Event> s_possibleEvents = new List<Event>(possibleEvents);
            s_possibleEvents.RemoveAt(0);

            List<Event> s_exactEvents = new List<Event>();
            s_exactEvents.Add(possibleEvents[0]);
            s_exactEvents.InsertRange(1, exactEvents);

            exp.ChildExpressions.Add( new Expression()
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            });
        }

        public void Fourth(Expression exp)
        {
            exp.ChildExpressions = new List<Expression>();
            exp.ChildExpressions.Add(new Expression
            {
                Probability = 1
            });
            List<Event> s_possibleEvents = CopyEventList(exp.PossibleEvents);
            s_possibleEvents[0].Sign = true;
            List<Event> s_exactEvents = CopyEventList(exp.ExactEvents);

            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            });
        }

        public void Fifth_A(Expression exp)
        {
            List<Event> possibleEvents = CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = CopyEventList(exp.ExactEvents);
            exp.ChildExpressions = new List<Expression>();

            exp.ChildExpressions.Add(new Expression()
            {
                PossibleEvents = possibleEvents
            });
            exp.ChildExpressions.Add(new Expression()
            {
                PossibleEvents = exactEvents,
                ExactEvents = possibleEvents
            });
            exp.ChildExpressions.Add(new Expression()
            {
                PossibleEvents = exactEvents
            });
        }

        public void Fifth_B(Expression exp)
        {
            List<Event> possibleEvents = CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = CopyEventList(exp.ExactEvents);
            Event parent = new Event();
            foreach (Event e in exp.ExactEvents)
                if (IfEventIsParent(possibleEvents[0],e))
                {
                    parent = e;
                    exactEvents = exactEvents.Where(t => t.ReturnName() != parent.ReturnName()).ToList();
                    break;
                }
            exp.ChildExpressions = new List<Expression>();
            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = possibleEvents,
                ExactEvents = exactEvents
            });
            Expression second = new Expression();
            second.PossibleEvents.Add(parent);
            second.ExactEvents.Add(possibleEvents[0]);
            second.ExactEvents.AddRange(exactEvents);
            exp.ChildExpressions.Add(second);
            Expression third = new Expression();
            third.PossibleEvents.Add(parent);
            third.ExactEvents.AddRange(exactEvents);
            exp.ChildExpressions.Add(third);
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
