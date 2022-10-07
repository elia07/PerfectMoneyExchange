using Saraf365.Core.InternetBankModules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saraf365.Test
{
    public partial class Form1 : Form
    {
        BankTejaratInternetBank tejarat = new BankTejaratInternetBank("0081869436", "iman54009723", "0003647520021", "5859831154009723", "9128376761ir", "e7876d24f7db5a381f05e0bb231a5f98", "00000", "0000", "00", "00");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tejarat.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = tejarat.GetBalance();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           var res= tejarat.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var res= tejarat.Login();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var fromDate = DateTime.Now.AddMinutes(-300000);
            var res = tejarat.GetCartTransferHistory(fromDate, DateTime.Now.Date.AddDays(1).AddMinutes(-1));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var res = tejarat.CardTransfer("6099997234209634", 50000);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            var res = tejarat.GetCardHolderName("5044291010415709");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var res = tejarat.NormalTransfer("0007732593611",50000);
        }
    }
}
