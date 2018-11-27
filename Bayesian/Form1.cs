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
            ExpertSystem es = new ExpertSystem();
            es = ExpertSystem.LoadData("AI Class Data Base");
            label1.UseMnemonic = false;
            label1.Text = es.WriteExpressionsList();
        }
    }
}

