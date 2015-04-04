using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniAuditorServer
{
    public partial class Form1 : Form
    {
        DBEntities db;

        public Form1()
        {
            InitializeComponent();
            IPConnection ip = new IPConnection();
            ip.StartServer("192.168.88.30", 55567);
            ip.OnClientConnect += ip_OnClientConnect;

            db = new DBEntities();

        }

        void ip_OnClientConnect(object sender, EventArgs e)
        {
            string[] mess = ((IPConnection)sender).ReceiveMessage().Split(' ');
            db.Table.Add(new Table()
            {
                Name = mess[0],
                Manuf = mess[1],
                Model = mess[2],
                Mem = mess[3]
            });
            dataGridView1.DataSource = db.Table;
        }
    }
}
