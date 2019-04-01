using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace WindowsFormsApp6
{
    public partial class Loading : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public Loading()
        {
          
            InitializeComponent();
            pictureBox1.Image = Image.FromFile("C:\\Users\\ashis\\Downloads\\Video\\load.gif");
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = 5000;
            timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Home h = new Home();
            this.Hide();
            h.Show();
            timer.Stop();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
