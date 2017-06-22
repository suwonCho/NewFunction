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
    public partial class Melsec_A : Form
    {
		DataTable dtAdd = new DataTable();
		string strLastCol = string.Empty;


		public Melsec_A(string Title)
        {
            InitializeComponent();
            this.Title = Title;

			dtAdd.Columns.Add("Address", Type.GetType("System.Int32"));
			dtAdd.Columns.Add("Value", Type.GetType("System.Int32"));
			dtAdd.Columns.Add("HexValue", Type.GetType("System.String"));

			dtAdd.ColumnChanged += new DataColumnChangeEventHandler(dtAdd_ColumnChanged);

			fpAdd_Sheet1.DataSource = dtAdd;

        }

		void dtAdd_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{

			if ( strLastCol != string.Empty && e.Column.Caption != strLastCol )
			{
				strLastCol = string.Empty;
				return;
			}

			int intValue;
			string hexValue = string.Empty;

			try
			{
				switch (e.Column.Caption)
				{
					case "Value":
						//value 변경..
						intValue = (int)e.ProposedValue;
						strLastCol = e.Column.Caption;
						e.Row["HexValue"] = intValue.ToString("X4");						
						break;

					case "HexValue":
						hexValue = (string)e.ProposedValue;
						strLastCol = e.Column.Caption;
						e.Row["Value"] = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);						
						break;

					default:
						strLastCol = string.Empty;
						break;
						
						
				}
			}
			catch
			{
				strLastCol = string.Empty;
				e.Row["Value"] = 0;				
			}
		}

        string Title;
        TcpServer Server = new TcpServer();
        byte[] bytBodySerial = new byte[] { 0x81, 0x00, 0x03, 0x01, 0x66, 0x88 };
        
        

        private void frmServer_Load(object sender, EventArgs e)
        {
            Server.PortNo = 6040;
            Server.evtRecieveData = new TcpServer.delRecieveData(this.RecieveData);
            Server.evtDisconnect = new TcpServer.delDisconnect(this.ServerDisconnect);

            fpView_Sheet1.Columns[0].Label = "DateTime";
            fpView_Sheet1.Columns[1].Label = "Send/Reci";

            for (int i = 2; i < 22; i++)
            {
                fpView_Sheet1.Columns[i].Label = (i-2).ToString(); 
            }

            fpView_Sheet1.RowCount = 0;
			            
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

				fpView_Sheet1.Rows.Add(0, 1);

				int Row = 0; // fpView_Sheet1.RowCount - 1;
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
			enCmdType cmd = enCmdType.None;

			switch (data[0])
			{
			   case 0x01:       //read command
				   cmd = enCmdType.Read;				   
				   break;

			   case 0x03:       //write command
				   cmd = enCmdType.Write;
				   break;

			   case 0xBD:
				   byte[] bytData = new byte[] { 0x01, 0xff };
				   Server.Send(bytData);
				   this.Fp_InsertRow(bytData, true);
				   break;

			   default:
				   break;
			       
			}

			if (cmd == enCmdType.None) return;

			string strAdd = data[5].ToString("X4") + data[4].ToString("X2");
			int intAdd = int.Parse(strAdd, System.Globalization.NumberStyles.HexNumber);
			
			DataRow dr;

			if (dtAdd.Select(string.Format("Address = {0}", intAdd)).Length > 0)
				dr = dtAdd.Select(string.Format("Address = {0}", intAdd))[0];
			else
			{
				dr = dtAdd.NewRow();
				dr["Address"] = intAdd;
				dr["Value"] = 0;
				dtAdd.Rows.Add(dr);
			}

			string strV;
			int intV;

			if (cmd == enCmdType.Read)
			{
				intV = (int)dr["Value"];
				strV = intV.ToString("X4");

				byte[] byt = new byte[] { 0x81, 0x00, 0x00, 0x00 };
				byt[3] = byte.Parse(strV.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
				byt[2] = byte.Parse(strV.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

				Server.Send(byt);
				this.Fp_InsertRow(byt, true);
			}
			else
			{
				strV = data[13].ToString("X4") + data[12].ToString("X2");
				intV = int.Parse(strV, System.Globalization.NumberStyles.HexNumber);
				dr["Value"] = intV;

				byte[] byt = new byte[] { 0x81, 0x00 };
				Server.Send(byt);
				this.Fp_InsertRow(byt, true);
			}
				   
        }

		



        private void ServerDisconnect()
        {
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

		private void btnAdd_Click(object sender, EventArgs e)
		{
			dtAdd.Rows.Add(dtAdd.NewRow());
		}
    }
}