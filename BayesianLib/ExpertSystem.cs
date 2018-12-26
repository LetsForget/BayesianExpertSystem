using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace BayesianLib
{
    [Serializable]
    public class ExpertSystem
    {
        #region Fields
        public List<Expression> CalculatedExpressions { get; set; }
        [XmlIgnore]
        public List<Event> Events;
        [XmlIgnore]
        internal EventCalculator EventCalc;
        [XmlIgnore]
        public ExpressionCalculator ExpCalc;
        #endregion

        #region Constructors

        public ExpertSystem()
        {
            CalculatedExpressions = new List<Expression>();
            Events = new List<Event>();
            EventCalc = new EventCalculator(this);
            ExpCalc = new ExpressionCalculator(this);
        }

        #endregion

        #region Methods

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

        internal void GetEventsFromExpression(Expression exp)
        {
            foreach (Event p in exp.PossibleEvents)
            {
                Event Pos = p;                   // Pos and exact is needed to connect events in expression with Events list in expert system 
                                                 // They are pointers to Events in Events list
                if (!EventCalc.IfThereAnEvent(Pos))
                    Events.Add(Pos);
                else
                    Pos = EventCalc.ReturnExistingEvent(Pos);

                foreach (Event e in exp.ExactEvents)
                {
                    Event Exact = e;

                    if (!EventCalc.IfThereAnEvent(e))
                        Events.Add(e);
                    else
                        Exact = EventCalc.ReturnExistingEvent(Exact);

                    Pos.AddInRelativeList(Exact, Pos.Parents);
                    Exact.AddInRelativeList(Pos, Exact.Childs);
                }

            }
        }

        private void FillTheEvents()
        {
            foreach (Expression e in CalculatedExpressions)
                GetEventsFromExpression(e);
        }


        #endregion

        #region Calculation probability

        public void CalculateExpression(Expression exp, ref string answer)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            if (ExpCalc.IfThereAnExpression(exp) == true)
            {
                exp.Probability = ExpCalc.ReturnProbability(exp);
                answer += exp.Write() + " = " + exp.Probability + "\n"; 
                return;
            }
            answer += exp.Write();

            #region Checking the first item, if there are a conjuction in possible events (more than one possible event exists)

            if (exp.PossibleEvents.Count > 1)
            {
                answer += " = (1) ";

                First(exp);
                string firstChild_s = exp.ChildExpressions[0].Write();
                string seconChild_s = exp.ChildExpressions[1].Write();
                answer += firstChild_s + " * " + seconChild_s + "\n";

                exp.Probability = 1;
                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    exp.Probability *= e.Probability;
                }
                string firstChild_prob = exp.ChildExpressions[0].Probability.ToString();
                string seconChild_prob = exp.ChildExpressions[1].Probability.ToString();
                string exp_prob = exp.Probability.ToString();
                answer += exp.Write() + " = " + firstChild_prob + " * " + seconChild_prob + " = " + exp_prob + "\n";
                return;
            }

            #endregion

            #region Checking the second and the third item, if there are the same event in possible and exact events

            foreach (Event e in exp.ExactEvents)
            {
                if (e.ReturnName() == exp.PossibleEvents[0].ReturnName())
                {
                    answer += " = (2) ";
                    exp.Probability = 1;
                    string expProba_s = exp.Probability.ToString();
                    answer += expProba_s + "\n";
                    return;
                }
                if (e.ReturnClearName() == exp.PossibleEvents[0].ReturnClearName())
                {
                    answer += " = (3) ";
                    exp.Probability = 0;
                    string expProba_s = exp.Probability.ToString();
                    answer += expProba_s + "\n";
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
                answer += "1 - " + expChild_s + "\n";
                CalculateExpression(exp.ChildExpressions[1], ref answer);
                double expChildProb = exp.ChildExpressions[1].Probability;
                exp.Probability = 1 - expChildProb;
                answer += exp.Write() + " = " + "1 - " + expChildProb + " = " + exp.Probability + "\n";
                return;
            }

            #endregion

            #region Checking the fifth item, if there are a child in exact events to posiible event

            #region Five point A

            if (exp.ExactEvents.Count() < 2 && EventCalc.IfEventIsParent(exp)) 
            {
                answer += " = (5a) ";
                Fifth_A(exp);
                List<string> expChild_s = new List<string>();
                List<double> expChild_prob = new List<double>();

                foreach (Expression e in exp.ChildExpressions)
                    expChild_s.Add(e.Write());

                answer += expChild_s[0] + " * (" + expChild_s[1] + " / " + expChild_s[2] + ") " + "\n";

                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    expChild_prob.Add(e.Probability);
                }
                exp.Probability = exp.ChildExpressions[0].Probability * (exp.ChildExpressions[1].Probability / exp.ChildExpressions[2].Probability);
                answer += exp.Write() + " = " + expChild_prob[0] + " * (" +
                                                       expChild_prob[1] + " / " +
                                                       expChild_prob[2] + " )" + "=" +
                                                       exp.Probability + "\n";
                return;
            }

            #endregion

            #region Five point B

            if (EventCalc.IfEventIsParent(exp))
            {
                answer += " = (5b) ";
                Fifth_B(exp);
                List<string> expChild_s = new List<string>();
                List<double> expChild_prob = new List<double>();

                foreach (Expression e in exp.ChildExpressions)
                    expChild_s.Add(e.Write());

                answer += expChild_s[0] + " * (" + expChild_s[1] + " / " + expChild_s[2] + ") " + "\n";

                foreach (Expression e in exp.ChildExpressions)
                {
                    CalculateExpression(e, ref answer);
                    expChild_prob.Add(e.Probability);
                }
                exp.Probability = exp.ChildExpressions[0].Probability * (exp.ChildExpressions[1].Probability / exp.ChildExpressions[2].Probability);
                answer += exp.Write() + " = " + expChild_prob[0] + " * (" +
                                                       expChild_prob[1] + " / " +
                                                       expChild_prob[2] + " )" + "=" +
                                                       exp.Probability + "\n";
                return;
            }

            #endregion

            #endregion

            #region Checking the sixth item

            #region Six point A

            if (!EventCalc.HasEventAParent(exp.PossibleEvents[0]))
            {
                answer += " = (6a) ";
                Sixth_A(exp);
                answer += exp.ChildExpressions[0].Write() + "\n";
                CalculateExpression(exp.ChildExpressions[0], ref answer);
                exp.Probability = exp.ChildExpressions[0].Probability;
                answer += exp.Write() + " = " + exp.ChildExpressions[0].Probability + "\n";
                return;
            }

            #endregion

            #region Six point B
            if (EventCalc.HasEventAParent(exp.PossibleEvents[0]))
            {
                answer += " = (6b) ";
                Sixth_B(exp);
                int quan = exp.ChildExpressions.Count;
                for (int i = 0; i < quan; i += 2)
                {
                    answer += exp.ChildExpressions[i].Write() + " * " + exp.ChildExpressions[i + 1].Write() + "\n";
                    if (i != quan - 2)
                        answer += " + ";
                }
             //   answer += "\n";
                foreach (Expression e in exp.ChildExpressions)
                    CalculateExpression(e, ref answer);

                exp.Probability = 0;
                for (int i = 0; i < quan; i += 2)
                    exp.Probability += exp.ChildExpressions[i].Probability * exp.ChildExpressions[i + 1].Probability;
                answer += exp.Write() + " = ";
                for (int i = 0; i < quan; i += 2)
                {
                    answer += exp.ChildExpressions[i].Probability + " * " + exp.ChildExpressions[i + 1].Probability;
                    if (i != quan - 2)
                        answer += " + ";
                }
                answer += " = " + exp.Probability + "\n";
                return;
            }
            #endregion
            // Ты, наверное, сам знаешь что делать ))))0))0)))
            #endregion
        }

        #region Expressions calculations

        public void First(Expression exp)
        {
            if (exp.PossibleEvents.Count() == 0)
                throw new Exception("Empty expression");

            exp.ChildExpressions = new List<Expression>();
            List<Event> possibleEvents = EventCalculator.CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = EventCalculator.CopyEventList(exp.ExactEvents);

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
            List<Event> s_possibleEvents = EventCalculator.CopyEventList(exp.PossibleEvents);
            s_possibleEvents[0].Sign = true;
            List<Event> s_exactEvents = EventCalculator.CopyEventList(exp.ExactEvents);

            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = s_possibleEvents,
                ExactEvents = s_exactEvents
            });
        }

        public void Fifth_A(Expression exp)
        {
            List<Event> possibleEvents = EventCalculator.CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = EventCalculator.CopyEventList(exp.ExactEvents);
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
            List<Event> possibleEvents = EventCalculator.CopyEventList(exp.PossibleEvents);
            List<Event> exactEvents = EventCalculator.CopyEventList(exp.ExactEvents);

            Event child = new Event();

            foreach (Event e in exp.ExactEvents)
                if (EventCalc.IfEventIsParent(e, possibleEvents[0]))
                {
                    child = e;
                    exactEvents = exactEvents.Where(t => t.ReturnName() != child.ReturnName()).ToList();
                    break;
                }
            exp.ChildExpressions = new List<Expression>();
            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = possibleEvents,
                ExactEvents = exactEvents
            });
            Expression second = new Expression();
            second.PossibleEvents.Add(child);
            second.ExactEvents.Add(possibleEvents[0]);
            second.ExactEvents.AddRange(exactEvents);
            exp.ChildExpressions.Add(second);
            Expression third = new Expression();
            third.PossibleEvents.Add(child);
            third.ExactEvents.AddRange(exactEvents);
            exp.ChildExpressions.Add(third);
        }

        public void Sixth_A(Expression exp)
        {
            exp.ChildExpressions = new List<Expression>();
            exp.ChildExpressions.Add(new Expression
            {
                PossibleEvents = exp.PossibleEvents
            });
        }

        public void Sixth_B(Expression exp)
        {
            exp.ChildExpressions = new List<Expression>();
            Event possible = exp.PossibleEvents[0];
            var listOfConnectedEvents = ExpCalc.ReturnTheListOfExps(possible);

            foreach (Expression e in listOfConnectedEvents)
            {
                exp.ChildExpressions.Add(e);
                exp.ChildExpressions.Add(new Expression
                {
                    PossibleEvents = e.ExactEvents,
                    ExactEvents = exp.ExactEvents
                });
            }
        }

        

        #endregion

        #endregion

        #endregion
    }
}
