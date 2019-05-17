using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEventApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventBusLibrary.EventBus.Default.Subscribe<string>(Update);

        }

        void Update(String message)
        {
            this.rtbReciever.Text = message;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EventBusLibrary.EventBus.Default.Unsubcribe<string>(Update);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            EventBusLibrary.EventBus.Default.Publish(this.tbSender.Text);
        }
       
    }

    
}
