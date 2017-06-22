using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using tcp_server;

namespace SocketPLC
{
    public partial class Q_TagPublish : Form
    {
        public Q_TagPublish(string Title)
        {
            InitializeComponent();
            this.Title = Title;
        }

        string Title;
        TcpServer Server = new TcpServer();
        byte[] bytBodySerial = new byte[] { 0x81, 0x00, 0x03, 0x01, 0x66, 0x88 };
        
        

        private void frmServer_Load(object sender, EventArgs e)
        {
            Server.PortNo = 9000;
            Server.evtRecieveData = new TcpServer.delRecieveData(this.RecieveData);
            Server.evtDisconnect = new TcpServer.delDisconnect(this.ServerDisconnect);

            fpView_Sheet1.Columns[0].Label = "DateTime";
            fpView_Sheet1.Columns[1].Label = "Send/Reci";

            for (int i = 2; i < 22; i++)
            {
                fpView_Sheet1.Columns[i].Label = (i-2).ToString(); 
            }

            fpView_Sheet1.RowCount = 0;

            this.txtBodySerial.Text = "38866";
            this.txtPort.Text = Server.PortNo.ToString();
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            if (Server.Server_Stats)
            {
                Server.Server_Stop();
                lblServerStats.Text = "SeverStop";
                lblServerStats.ForeColor = Color.White;
                lblServerStats.BackColor = Color.Red;
                this.Text = Title;
                this.txtPort.Enabled = true;
            }
            else
            {
                if (!Server.Server_start()) return;
                lblServerStats.Text = "SeverStart";
                lblServerStats.ForeColor = Color.Red;
                lblServerStats.BackColor = Color.AliceBlue;
                this.Text = string.Format("{0} (Port : {1})",this.Title,Server.PortNo);
                this.txtPort.Enabled = false;
            }
        }


        private void RecieveData(byte[] data)
        {
            
            this.Fp_InsertRow(data, false);
            Data_Proc(data);
        }

        delegate void del_Fp_InsertRow(byte[] data, bool isSend);

        private void Fp_InsertRow(byte[] data, bool isSend)
        {
            if (fpView.InvokeRequired)
            {
                del_Fp_InsertRow dele = new del_Fp_InsertRow(this.Fp_InsertRow);

                Invoke(dele, new object[] { data, isSend });
            }
            else
            {
                
                fpView_Sheet1.RowCount++;
                int Row = fpView_Sheet1.RowCount - 1;
                fpView_Sheet1.Cells[Row, 0].Value = DateTime.Now;

                int Col = 2;
                foreach (byte D in data)
                {
                    fpView_Sheet1.Cells[Row, Col].Value = string.Format("{0:X2}", D);
                    Col++;
                }

                if (isSend)
                {
                    fpView_Sheet1.Cells[Row,1].Value = "Send";
                    //fpView_Sheet1.Cells[Row, -1].BackColor = Color.FromArgb(74, 60, 140);
                    fpView_Sheet1.Rows[Row].BackColor = Color.Yellow;
                }
                else
                {
                    fpView_Sheet1.Cells[Row,1].Value = "Recieve";
                    fpView_Sheet1.Rows[Row].BackColor = Color.White;
                }


            }
        }
        

        private void Data_Proc(byte[] data)
        {
           switch (data[0])
           {
               case 0x01:       //read command
                   string strAdd = data[5].ToString("X2") + data[4].ToString("X2");
                   int intAdd = int.Parse(strAdd,  System.Globalization.NumberStyles.HexNumber);
                   break;

               case 0xEA:       //read command
                   
                   Server.Send(this.bytBodySerial);
                   this.Fp_InsertRow(this.bytBodySerial, true);
                   break;

               case 0xBD:
                   byte[] bytData = new byte[] { 0x01, 0xff };
                   Server.Send(bytData);
                   this.Fp_InsertRow(bytData, true);
                   break;

               default:
                   break;
                   
           }
        }

        private void ServerDisconnect()
        {
        }

        private void txtBodySerial_TextChanged(object sender, EventArgs e)
        {
            string value = String.Format("{0:D5}",Convert.ToInt32(txtBodySerial.Text,10));

            this.bytBodySerial[2] = Convert.ToByte("0" + value.Substring(0, 1),16);
            this.bytBodySerial[4] = Convert.ToByte(value.Substring(3, 2),16);
            this.bytBodySerial[5] = Convert.ToByte(value.Substring(1, 2),16);


        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            Server.PortNo = Convert.ToInt32(txtPort.Text, 10);
        }

        private void Q_TagPublish_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Dispose();
            Server = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.fpView.ActiveSheet.RowCount = 0;
        }
    }
}