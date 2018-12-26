using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BayesianLib;

namespace Bayesian
{
    public partial class Form1 : Form
    {
        ExpertSystem es = new ExpertSystem();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Expression exp = new Expression();
            exp.PossibleEvents.Add(new Event("burglary"));
            exp.ExactEvents.Add(new Event("call"));
            string answer = "";
            es.CalculateExpression(exp, ref answer);
            textBox1.Text = answer.Replace("\n","\r\n");
        }

        private void Load_Click(object sender, EventArgs e)
        {
            es = ExpertSystem.LoadData(DataBaseName.Text);
            expressionsList.Text = es.ExpCalc.WriteExpressionsList().Replace("\n", "\r\n");
            es.SaveData("dfdf");
        }
    }
}

