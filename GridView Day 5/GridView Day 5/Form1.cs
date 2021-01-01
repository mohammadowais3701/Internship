using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GridView_Day_5
{
    public partial class Form1 : Form
        
    {
        FormCollection fm; 
        Boolean isstart = false;
        Boolean isstop = true;
        Boolean isbind=false;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)

        {
            
            isstart = true;
            isstop = false;
            
            if (isbind == false)
            {
                isbind = true;
               // List<Thread> data = new List<Thread>();
                List<DataClass> data = new List<DataClass>();
                try
                {
                  //  Thread t1=new Thread(()=>new DataClass { counter = 0, name = "Ali" });
                    //data.Add(t1);
                    
                    data.Add(new DataClass { counter = 0, name = "Ali" });
                    data.Add(new DataClass { counter = 4, name = "Owais" });
                    data.Add(new DataClass { counter = 8, name = "Ahmed" });
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                
                }
                try
                {
                    dataGridView1.DataSource = data;
                }
                catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        new Thread(() =>
                        {
                     fm = Application.OpenForms;
                     try
                     {
                         while (fm[0].Text == "Form1")
                         {
                             if (isstart)
                             {
                                 isstop = false;
                                 row.Cells["counter"].Value = Convert.ToInt32(row.Cells["counter"].Value) + 1;

                             

                                 Thread.Sleep(1000);
                               

                             }
                       
                             fm = Application.OpenForms;

                         }
                     }
                     catch (Exception ex) {
                     
                     }

                        }).Start();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }

        
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            isstop = true;
            isstart = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
