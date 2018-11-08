using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bayesian.Logic;

namespace Bayesian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Event s = new Event("-sensor");
            Event falseS = new Event("sensor");
            Event b = new Event("-burglary");
            Event l = new Event("lightning");
            Expression p = new Expression();
            p.PossibleEvents.Add(s);
            p.ExactEvents.Add(b);
            p.ExactEvents.Add(l);
            p.Probability = 0.9;
            ExpertSystem es = new ExpertSystem();
            es.AddAnEvent(s);
            es.AddAnEvent(s);
            es.AddAnEvent(falseS);
            es.AddAnEvent("sensor");
            es.AddAnEvent("-sensor");
            es.AddAnEvent(b);
            es.AddAnEvent(l);
            es.AddAnExpression(p);

            Expression exp = new Expression();
            exp.PossibleEvents.Add(new Event(s.Name));
            exp.ExactEvents.Add(new Event(b.Name));
            exp.ExactEvents.Add(new Event(l.Name));
            Calculator c = new Calculator();
            c.Fourth(exp);
        }
    }
}
