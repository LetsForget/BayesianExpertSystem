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
            //if (exp.PossibleEvents.Count > 0)                    
            //{
            //    First(exp);
            //    foreach (Expression t in exp.ChildExpressions)
            //        FindProbability(t);

            //    exp.Probability = exp.ChildExpressions[0].Probability;
            //    exp.Probability *= exp.ChildExpressions[1].Probability;
            //    return;
            //}
            #endregion

            
        }
        

        #region Misc

        
        //private static bool CheckIfChildInExactEvents(Expression exp)
        //{
        //    foreach (Event t in exp.ExactEvents)
        //        if (exp.IsChild(t))
        //            return true;
        //    return false;
        //}

        #endregion

        #endregion
    }
}
