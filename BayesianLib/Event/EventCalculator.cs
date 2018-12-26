using System;
using System.Collections.Generic;
using System.Linq;

namespace BayesianLib
{
    public class EventCalculator
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
        ExpertSystem _es;

        private List<Event> Events;
        private List<Expression> CalculatedExpressions;
        #endregion

        #region Constructors

        public EventCalculator(ExpertSystem es)
        {
            ES = es;
        }

        #endregion

        #region Methods

        internal Event ReturnExistingEvent(Event _event)
        {
            return Events.FirstOrDefault(x => x.ReturnClearName() == _event.ReturnClearName());
        }

        internal void RefreshEventRelatives(Event _event)
        {
            if (!IfThereAnEvent(_event))
                throw new Exception("No such event!");

            _event.Parents = ReturnExistingEvent(_event).Parents;
            _event.Childs = ReturnExistingEvent(_event).Childs;
        }

        internal bool IfThereAnEvent(Event _event)
        {
            if (ReturnExistingEvent(_event) == null)
                return false;
            else
                return true;
        }

        internal bool IfEventIsParent(Event posChild, Event posParent)
        {
            if (!IfThereAnEvent(posChild) || !IfThereAnEvent(posParent))
                throw new Exception("No such event!");

            RefreshEventRelatives(posChild);
            if (posChild.Parents.Count == 0)
                return false;

            foreach (Event e in posChild.Parents)
                if (e.ReturnName() == posParent.ReturnName())
                    return true;

            foreach (Event e in posChild.Parents)
                return IfEventIsParent(e, posParent);
            return false;
        }

        internal bool IfEventIsParent(Expression exp)
        {
            if (exp.PossibleEvents.Count == 0 || exp.ExactEvents.Count == 0)
                return false;

            if (exp.PossibleEvents.Count > 1)
                throw new Exception("There are more then two possible events!");

            foreach (Event e in exp.ExactEvents)
                if (IfEventIsParent(e, exp.PossibleEvents[0]))
                    return true;
            return false;
        }

        internal bool HasEventAParent(Event _event)
        {
            if (ReturnExistingEvent(_event) == null)
                throw new Exception("There a no such event!");

            RefreshEventRelatives(_event);
            if (_event.Parents.Count > 0)
                return true;
            else
                return false;
        }
        internal static List<Event> CopyEventList(List<Event> copyable)
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
    }
}
