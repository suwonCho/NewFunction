using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;


namespace System.IO.Ports
{
	/// <summary>
	/// �ٿ�巹��Ʈ : bit/sec
	/// </summary>
	public enum BaudRate : int
	{
		b110 = 110,
		b300 = 300,
		b1200 = 1200,
		b2400 = 2400,
		b4800 = 4800,
		b9600 = 9600,
		b19200 = 19200,
		b38400 = 38400,
		b57600 = 57600,
		b115200 = 115200,
		b230400 = 230400,
		b460800 = 460800,
		b921600 = 921600
	};
	/// <summary>
	/// �����ͺ�Ʈ bit
	/// </summary>
	public enum DataBits : int
	{
		Bit5 = 5,
		Bit6 = 6,
		Bit7 = 7,
		Bit8 = 8
	};
	
}

namespace Function.Comm
{
	public class Serial : IDisposable
	{
		SerialPort clsSn;

		public delegate void delReceive(Serial sender, byte[] yReceiveData);		
		/// <summary>
		/// Receive �̺�Ʈ�� ����ϱ� ���� �̺�Ʈ�� �����Ѵ�. 
		/// </summary>
		public event delReceive OnDataReceived;


		public object Tag
		{
			get;
			set;
		}


		public string Name
		{
			get;
			set;
		}

		
		/// <summary>
		/// Serial PortName
		/// </summary>
		public string PortName
		{
			get
			{
				return clsSn.PortName;
			}
			set
			{
				clsSn.PortName = value;
			}
		}
		/// <summary>
		/// ��ü�� BaudRate�� �������ų� �����մϴ�.
		/// </summary>
		public BaudRate BaudRate
		{
			get
			{
				return (BaudRate)clsSn.BaudRate;
			}
			set
			{
				clsSn.BaudRate = (int)value;
			}
		}

		/// <summary>
		/// ��ü�� �и�Ƽ ��Ʈ�� �������ų� �����մϴ�.
		/// </summary>
		public Parity Parity
		{
			get
			{
				return clsSn.Parity;
			}
			set
			{
				clsSn.Parity = value;
			}
		}


		/// <summary>
		/// ����Ʈ�� ������ ��Ʈ�� ǥ�� ���̸� �������ų� �����մϴ�.
		/// </summary>
		public DataBits DataBits
		{
			get
			{
				return (DataBits)clsSn.DataBits;
			}
			set
			{
				clsSn.DataBits = (int)value;
			}
		}


		/// <summary>
		/// ����Ʈ�� ���� ��Ʈ�� ǥ�� ������ �������ų� �����մϴ�.
		/// </summary>
		public StopBits StopBits
		{
			get
			{
				return clsSn.StopBits;
			}
			set
			{
				clsSn.StopBits = value;
			}		

		}

		/// <summary>
		/// �������� ���� ��Ʈ ������ ���� �ڵ����ŷ ���������� �������ų� �����մϴ�.
		/// </summary>
		public Handshake Handshake
		{
			get
			{
				return clsSn.Handshake;
			}
			set
			{
				clsSn.Handshake = value;
			}
		}

		/// <summary>
		/// ������Ʈ ���� �Ǵ� ���� ���¸� ��Ÿ���� ���� �����ɴϴ�.
		/// </summary>
		public bool IsOpen
		{
			get
			{
				return clsSn.IsOpen;
			}
		}



		/// <summary>
		/// Ŭ��������
		/// </summary>
		public Serial()
		{
			clsSn = new SerialPort();
			
			Init(); 			
		}

		/// <summary>
		/// Ŭ��������
		/// </summary>
		/// <param name="portName"></param>
		/// <param name="baudRate"></param>
		/// <param name="parity"></param>
		/// <param name="dataBits"></param>
		/// <param name="stopBits"></param>
		public Serial(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			clsSn = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
			Init();
		}		


		/// <summary>
		/// Ŭ��������
		/// </summary>
		/// <param name="portName"></param>
		/// <param name="baudRate"></param>
		/// <param name="parity"></param>
		/// <param name="dataBits"></param>
		/// <param name="stopBits"></param>
		public Serial(string portName, int baudRate, string parity, int dataBits, string stopBits)			
		{
			clsSn = new SerialPort(
								portName,
								baudRate,
								String2Parity(parity),
								dataBits,
								String2stopBits(stopBits));

			Init();
		}

		public Serial(string portName, BaudRate baudRate, Parity parity, DataBits dataBits, StopBits stopBits)
		{
			clsSn = new SerialPort();

			this.PortName = portName;
			this.BaudRate = baudRate;
			this.Parity = parity;
			this.DataBits = dataBits;
			this.StopBits = stopBits;

			Init();
		}





		/// <summary>
		/// �� ���� ��Ʈ ������ ���ϴ�.
		/// </summary>
		public void Open()
		{
			clsSn.Open();
		}

		/// <summary>
		/// ��Ʈ ������ �ݰ�, System.IO.Ports.SerialPort.IsOpen �Ӽ��� false�� �����ϰ�, ���� System.IO.Stream
		/// ��ü�� �����մϴ�.
		/// </summary>
		public void Close()
		{
			clsSn.Close();
		}

		//
		// ���:
		//     �Ű� ���� ���ڿ��� ��¿� ���ϴ�.
		//
		// �Ű� ����:
		//   text:
		//     ����� ���ڿ��Դϴ�.
		//
		// ����:
		//   System.ArgumentNullException:
		//     str�� null�� ���
		//
		//   System.ServiceProcess.TimeoutException:
		//     �ð� ������ ������ ���� �۾��� �Ϸ���� ���� ���
		//
		//   System.InvalidOperationException:
		//     ������ ��Ʈ�� ���� ���� ���� ���
		public void Write(string text)
		{
			clsSn.Write(text);
		}
		//
		// ���:
		//     ������ ���� ����Ʈ�� ��� ������ ������ �����¿� ���ϴ�.
		//
		// �Ű� ����:
		//   offset:
		//     ���⸦ ������ ���� �迭�� �������Դϴ�.
		//
		//   count:
		//     �� ����Ʈ ���Դϴ�.
		//
		//   buffer:
		//     ��� ������ �� ����Ʈ �迭�Դϴ�.
		//
		// ����:
		//   System.ArgumentException:
		//     offset�� count�� ���� buffer�� ���̺��� ū ���
		//
		//   System.ServiceProcess.TimeoutException:
		//     �ð� ������ ������ ���� �۾��� �Ϸ���� ���� ���
		//
		//   System.ArgumentNullException:
		//     ���޵� buffer�� null�� ���
		//
		//   System.ArgumentOutOfRangeException:
		//     offset �Ǵ� count �Ű� ������ ���޵� buffer�� �ùٸ� ���� �ۿ� �ִ� ��� offset �Ǵ� count�� 0���� ����
		//     ���
		//
		//   System.InvalidOperationException:
		//     ������ ��Ʈ�� ���� ���� ���� ���
		public void Write(byte[] buffer, int offset, int count)
		{
			clsSn.Write(buffer, offset, count);
		}
		//
		// ���:
		//     ������ ���� ���ڸ� ��� ������ ������ �����¿� ���ϴ�.
		//
		// �Ű� ����:
		//   offset:
		//     ���⸦ ������ ���� �迭�� �������Դϴ�.
		//
		//   count:
		//     �� ���� ���Դϴ�.
		//
		//   buffer:
		//     ��� ������ �� ���� �迭�Դϴ�.
		//
		// ����:
		//   System.ArgumentException:
		//     offset�� count�� ���� buffer�� ���̺��� ū ���
		//
		//   System.ServiceProcess.TimeoutException:
		//     �ð� ������ ������ ���� �۾��� �Ϸ���� ���� ���
		//
		//   System.ArgumentNullException:
		//     ���޵� buffer�� null�� ���
		//
		//   System.ArgumentOutOfRangeException:
		//     offset �Ǵ� count �Ű� ������ ���޵� buffer�� �ùٸ� ���� �ۿ� �ִ� ��� offset �Ǵ� count�� 0���� ����
		//     ���
		//
		//   System.InvalidOperationException:
		//     ������ ��Ʈ�� ���� ���� ���� ���
		public void Write(char[] buffer, int offset, int count)
		{
			clsSn.Write(buffer, offset, count);
		}



		public void Init()
		{
			clsSn.DataReceived += new SerialDataReceivedEventHandler(clsSerial_DataReceived);
		}

		/// <summary>
		/// ���� �� ���ð� : ��ȣ�� ���� ������ ����
		/// </summary>
		public int intRecieveWatingTime = 200;

		private void clsSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			Thread.Sleep(intRecieveWatingTime);
			int intLength = clsSn.BytesToRead;
			if (intLength < 1) return;

			byte[] bytReceive = new byte[intLength];
			clsSn.Read(bytReceive, 0, intLength);

			Console.WriteLine("[serial]{0} / {1}", Fnc.Bytes2String(bytReceive), Encoding.Default.GetString(bytReceive));

			if(OnDataReceived != null)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(RaiseEvent), bytReceive);
			}
		}

		private void RaiseEvent(object obj)
		{
			if (OnDataReceived != null)
			{
				byte[] bytData = (byte[])obj;
				OnDataReceived(this, bytData);
			}
		}


		/// <summary>
		/// Parity Enum ���·� ��ȯ
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Parity String2Parity(string value)
		{
			Parity enParity = Parity.None;

			value = value.ToUpper();

			Type type = enParity.GetType();


			foreach (Parity e in Enum.GetValues(type))   //Parity.GetType()))
			{
				if (value == e.ToString().ToUpper())
				{
					enParity = e;
					break;
				}
			}


			return enParity;
	
		}

		/// <summary>
		/// StopBit Enum ���·� ��ȯ
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static StopBits String2stopBits(string value)
		{
			StopBits enStopBits = StopBits.One;

			value = value.ToUpper();

			Type type = enStopBits.GetType();


			foreach (StopBits e in Enum.GetValues(type))   //Parity.GetType()))
			{
				if (value == e.ToString().ToUpper())
				{
					enStopBits = e;
					break;
				}
			}


			return enStopBits;

		}

		/// <summary>
		/// Ŭ������ ���� �Ѵ�.
		/// </summary>
		public void Dispose()
		{
			Close();
			clsSn.Dispose();
		}

	}
}
