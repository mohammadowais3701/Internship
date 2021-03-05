using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuageTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void c1RadialGauge2_ItemClick(object sender, C1.Win.C1Gauge.ItemEventArgs e)
        {
            
        }

        private void c1RadialGauge2_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("sss");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            c1RadialGauge4.Value = c1RadialGauge4.Value + 1;
        }
    }
}
