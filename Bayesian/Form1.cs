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
            Event b = new Event("-burglary");
            Event l = new Event("lightning");
            Expression p = new Expression();
            p.PossibleEvents.Add(s);
            p.ExactEvents.Add(b);
            p.ExactEvents.Add(l);
            var temp = p.Write();

        }
    }
}
