using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace Function.form
{
	/// <summary>
	/// BaseForm이나 subBaseForm에서 모두 사용되는 이벤트나 프로퍼티, 메서드 등록
	/// </summary>
	public class clsbaseForm : Form
	{
		List<Control> lstKeyDown = new List<Control>();

		enConfigFileType _configFileType = enConfigFileType.DefaultConfig;

		/// <summary>
		/// 초기화 사용 시 사용 타이머
		/// </summary>
		System.Windows.Forms.Timer tmrInit = new System.Windows.Forms.Timer();


		private bool _isUseFormInit = false;


		/// <summary>
		/// 폼 로드 시 타이머로 Form_Init 사용 여부를 가져오거나 설정한다. Form_Init를 override하여 사용할것
		/// </summary>
		[Description("폼 로드 시 타이머로 Form_Init 사용 여부를 가져오거나 설정한다. Form_Init를 override하여 사용할것")]
		public bool isUseFormInit
		{
			get { return _isUseFormInit; }
			set
			{
				_isUseFormInit = value;
			}
		}


		/// <summary>
		/// 프로그램에서 저장에 사용할 컨피그 저장할 타입
		/// </summary>
		[Description("프로그램에서 저장에 사용할 컨피그 저장할 타입")]
		public enConfigFileType SaveConfigFileType
		{
			get { return _configFileType; }
			set
			{
				_configFileType = value;
			}
		}


		object _saveposition_setting = null;
		/// <summary>
		/// 셑팅설정  클라스
		/// </summary>
		public object SavePosition_Setting
		{
			get { return _saveposition_setting; }
			set
			{
				_saveposition_setting = value;
			}
		}

		internal readonly string _FormControlSize_property_name = "FormControlSize";

		public bool _isSaveControlSize = false;

		/// <summary>
		/// 컨트롤 크기저장여부를 가져오거나 설정한다.<para />
		/// 폼상태 저장을 위해서는 SavePosition_Setting 변수에 사용할 프로퍼티 클래스를 할당하고(ex)SavePosition_Setting = Properties.Settings.Default <para />
		///		프로퍼티 클래스에 FormControlSize항목 추가한다(형식은 string) - 저장은 subBaseForm 이름을 기준으로 각컨트롤의 이름으로 사이즈 저장 처리를 한다.<para />
		/// </summary>
		[Description(@"컨트롤 크기저장여부를 가져오거나 설정한다.
폼상태 저장을 위해서는 SavePosition_Setting 변수에 사용할 프로퍼티 클래스를 할당하고(ex)SavePosition_Setting = Properties.Settings.Default
프로퍼티 클래스에 FormControlSize항목 추가한다(형식은 string) - 저장은 subBaseForm 이름을 기준으로 각컨트롤의 이름으로 사이즈 저장 처리를 한다.
(설정파일 사용 시는 필요 없음)")]
		public bool isSaveControlSize
		{
			get { return _isSaveControlSize; }
			set
			{
				_isSaveControlSize = value;
			}
		}


		//protected void control_Size_Load()
		//{
		//	if (isSaveControlSize)
		//	{
		//		fncForm.control_Size_Load(_saveposition_setting, _FormControlSize_property_name, this);
		//	}
		//}

		//protected void control_Size_Save()
		//{
		//	if (isSaveControlSize)
		//	{
		//		fncForm.control_Size_Save(_saveposition_setting, _FormControlSize_property_name, this);
		//	}
		//}

		public clsbaseForm()
		{
			this.Load += new EventHandler(clsbaseForm_Load);
			this.FormClosed += new FormClosedEventHandler(clsbaseForm_FormClosed);

			//폼 전역에서 키를 입력을 처리하는 이벤트를 처리 한다.
			foreach (Control c in this.Controls)
			{
				setKeyDown_Controls(c);
			}

			this.ControlAdded += new ControlEventHandler(frmBaseForm_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(frmBaseForm_ControlRemoved);
		}


		void setKeyDown_Controls(Control c, bool isRemove = false)
		{
			int i;

			if (c.GetType().Equals(typeof(usrMultiTextItem)))
			{
				return;
			}


			bool isInputBox = c.GetType().Equals(typeof(usrInputBox));
			

			if (isRemove)
			{
				//c.KeyDown -= new KeyEventHandler(c_KeyDown);
				//if (lstKeyDown.Contains(c)) lstKeyDown.Remove(c);
			}
			else
			{
				if (!lstKeyDown.Contains(c))
				{
					if (isInputBox)
					{
						usrInputBox u = c as usrInputBox;

						u.KeyDown += new KeyEventHandler(c_KeyDown);
					}
					else
						c.KeyDown += new KeyEventHandler(c_KeyDown);

					lstKeyDown.Add(c);
				}
			}

			if(isInputBox) return;

			foreach (Control cc in c.Controls)
			{
				setKeyDown_Controls(cc, isRemove);
			}
		}
		
		event KeyEventHandler _keyDown;

		public new event KeyEventHandler KeyDown
		{
			add { _keyDown += value; }
			remove { _keyDown -= value; }
		}

		void c_KeyDown(object sender, KeyEventArgs e)
		{
			if (_keyDown != null) _keyDown(sender, e);
		}

		public void frmBaseForm_ControlRemoved(object sender, ControlEventArgs e)
		{
			Control c = sender as Control;
			setKeyDown_Controls(c, true);
		}

		public void frmBaseForm_ControlAdded(object sender, ControlEventArgs e)
		{

			Control c = sender as Control;
			setKeyDown_Controls(c);
		}

		
		void clsbaseForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			formPostion_Save();
			//control_Size_Save();
			property_Save();
		}

		void clsbaseForm_Load(object sender, EventArgs e)
		{
			formPositon_Load();
			//control_Size_Load();
			property_Load();

			if (isUseFormInit)
			{
				//0.1초 후 폼 초기화를 처리 한다.
				tmrInit.Interval = 100;
				tmrInit.Tick += TmrInit_Tick;
				tmrInit.Start();
			}
		}


		private void TmrInit_Tick(object sender, EventArgs e)
		{
			tmrInit.Stop();

			Function.form.UserControls.usrPleaseWait u = new UserControls.usrPleaseWait();

			try
			{
				control_enable_begin();

				this.Controls.Add(u);

				control.Control2Center(this, u);
				u.BringToFront();
				
				Application.DoEvents();

				System.Threading.Thread.Sleep(500);

				Form_Init();
			}
			catch { }
			finally
			{
				this.Controls.Remove(u);
				u.Dispose();
				control_enable_fin();
			}


		}

		bool[] control_enable;
		private void control_enable_begin()
		{
			control_enable = new bool[this.Controls.Count];
			int idx = 0;
			foreach(Control c in this.Controls)
			{
				control_enable[idx] = c.Enabled;
				c.Enabled = false;
				idx++;
			}

		}


		private void control_enable_fin()
		{			
			int idx = 0;
			foreach (Control c in this.Controls)
			{
				if (control_enable.Length <= idx) break;

				c.Enabled = control_enable[idx];
			}

		}


		/// <summary>
		/// 폼 초기화 작업을 한다. - 폼로드후 처리 하므로 화면에서 내용을 볼수 있다.
		/// </summary>
		protected virtual void Form_Init()
		{
			
		}


		#region 폼위치 저장 처리부

		readonly static string _xmlSave_GroupName = "FORM_SAVE";
		readonly static string _savePosition_PropertyName = "BF_FORMPOSITION";

		bool _saveposition = false;

		/// <summary>
		/// 폼 위치 저장 여부
		/// 각 프로그램에서 Setting.setting 파일을 만든다<para/>
		/// 아래와 같은 항목을 추가 한다 <para/>
		/// 프로퍼티 클래스에 BF_FORMPOSITION항목 추가한다(형식은 STRING) 
		/// </summary>
		public bool SavePosition
		{
			get { return _saveposition; }
			set
			{
				_saveposition = value;
			}
		}

		private void formPositon_Load()
		{
			if (!_saveposition) return;


			try
			{
				string value;

				//폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
				switch (_configFileType)
				{
					case enConfigFileType.ConfigXml:
						Function.Setting s = SavePosition_Setting as Function.Setting;
						if (s == null)
							value = string.Empty;
						else
						{
							s.Group_Select(_xmlSave_GroupName);
							value = s.Value_Get(_savePosition_PropertyName, string.Empty);
						}

						break;

					default:
						value = Fnc.obj2String(Function.DFnc.Property_Get_Value(SavePosition_Setting, _savePosition_PropertyName));
						break;
				}

				value = fncForm.getSettingString(value, Name);

				//컨트롤정보는 '컨트롤이름:top:left:너비:높이;' 로구분
				string[] cValue = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				
				int Top = Fnc.obj2int(cValue[0]);
				int Left = Fnc.obj2int(cValue[1]);
				int Width = Fnc.obj2int(cValue[2]);
				int Height = Fnc.obj2int(cValue[3]);

				FormWindowState WindowState = (FormWindowState)Fnc.obj2int(cValue[4]);

				if (Width > 100 && Height > 100)
				{
					this.Width = Width < 10 ? this.Width : Width;
					this.Height = Height < 10 ? this.Height : Height;
				}

				this.Top = Top < 10 ? 500 : Top;
				this.Left = Left < 10 ? 500 : Left;

				this.WindowState = WindowState;

			}
			catch
			{
			}
			
		}

		/// <summary>
		/// 폼 위치 정보를 저장한다.
		/// </summary>
		protected void formPostion_Save()
		{
			//마지막 창정보를 저장한다. 
			if (!_saveposition) return;

			try
			{
				string value;
				Function.Setting xml = null;

				//폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
				switch (_configFileType)
				{
					case enConfigFileType.ConfigXml:
						xml = SavePosition_Setting as Function.Setting;
						if (xml == null)
							value = string.Empty;
						else
						{
							xml.Group_Select(_xmlSave_GroupName);
							value = xml.Value_Get(_savePosition_PropertyName, string.Empty);
						}
						break;


					default:
						value = Fnc.obj2String(Function.DFnc.Property_Get_Value(SavePosition_Setting, _savePosition_PropertyName));
						break;
				}

				string formNM = string.Format("/*{0}:", Name.ToUpper());

				value = fncForm.revSettingString(value, Name);

				value += formNM;

				FormWindowState state = WindowState;
				if (this.WindowState != FormWindowState.Normal)
				{
					this.WindowState = FormWindowState.Normal;
					this.Visible = true;
					Application.DoEvents();
				}

				value += string.Format("{0}:{1}:{2}:{3}:{4}", Top, Left, Width, Height, (int)state);

				value += "*/";

				switch (_configFileType)
				{

					case enConfigFileType.ConfigXml:
						if (xml != null)						
						{
							xml.Group_Select(_xmlSave_GroupName);
							xml.Value_Set(_savePosition_PropertyName, value);
							xml.Setting_Save();
						}
						break;

					default:
						Function.DFnc.Property_Set_Value(SavePosition_Setting, _savePosition_PropertyName, value);
						Function.DFnc.Method_Excute(SavePosition_Setting, "Save", null);
						break;
				}

			}
			catch
			{
			}
		
		}


		#endregion

#region Propertie처리부

		string _propertySave_property_name = "devProperties";

		string _propertySaveContolsName = string.Empty;

		/// <summary>
		/// 프로퍼티를 저장할 컨트롤 이름(대소문자 구분, ';'로 컨트롤별 구분)<para/>
		/// 지원 컨트롤:XtraControl.GridControl, XtraEditors.SplitterControl, CheckBox, Panel(크기), <para/>
		/// ListView(컬럼들 너비), (그외 컨트롤)Text만 저장<para/>
		/// 폼상태 저장을 위해서는 SavePosition_Setting 변수에 사용할 프로퍼티 클래스를 할당하고(ex)SavePosition_Setting = Properties.Settings.Default <para/>
		///		프로퍼티 클래스에 devProperties으 값을 이름으로 항목 추가한다(형식은 string)
		/// </summary>
		public string PropertySaveContolsName
		{
			get { return _propertySaveContolsName; }
			set
			{
				_propertySaveContolsName = value;
			}
		}


		/// <summary>
		/// 컨트롤의 프로퍼티 정보를 저장한다.
		/// </summary>
		protected void property_Save()
		{

			if (SavePosition_Setting == null) return;

			try
			{
				//폼내용은 '/*폼이름:컨트롤정보들...*/' 구성
				string value;


				Function.Setting xml = null;

				//폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
				switch (_configFileType)
				{
					case enConfigFileType.ConfigXml:
						xml = SavePosition_Setting as Function.Setting;
						if (xml == null)
							value = string.Empty;
						else
						{
							xml.Group_Select(_xmlSave_GroupName);
							value = xml.Value_Get(_propertySave_property_name, string.Empty);
						}
						break;


					default:
						value = Fnc.obj2String(Function.DFnc.Property_Get_Value(SavePosition_Setting, _propertySave_property_name));
						break;
				}



				string formNM = string.Format("/*{0}:", this.Name.ToUpper());

				value = Function.form.fncForm.revSettingString(value, this.Name);

				value += formNM;

				string[] pp = _propertySaveContolsName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				Control c;
				int idx;

				//컨트롤과 컨트롤 구분은 ';', 컨틀롤내 값사이구분은 '/'
				foreach (string p in pp)
				{
					try
					{
						c = Function.form.fncForm.FindControlInControl(this, p);
						if (c == null) continue;

						switch (c.GetType().ToString())
						{
							case "DevExpress.XtraGrid.GridControl":
								//case "DevExpress.XtraGrid.Views.Grid.GridView":
								//DevExpress.XtraGrid.GridControl gc = c as DevExpress.XtraGrid.GridControl;
								//DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gc.FocusedView;

								Object gv = Function.DFnc.Property_Get_Value(c, "FocusedView");


								//value += gc.Name + ":";

								value += Function.DFnc.Property_Get_Value(c, "Name").ToString() + ":";

								CollectionBase cols = (CollectionBase)Function.DFnc.Property_Get_Value(gv, "Columns");

								foreach (object col in cols)
								{
									//value += string.Format("{0}/{1}:", col.FieldName, col.Width);
									value += string.Format("{0}/{1}:", Function.DFnc.Property_Get_Value(col, "FieldName"), Function.DFnc.Property_Get_Value(col, "Width"));
								}
								value += ";";
								break;

							case "DevExpress.XtraEditors.SplitterControl":
								//DevExpress.XtraEditors.SplitterControl sc = c as DevExpress.XtraEditors.SplitterControl;
								//value += string.Format("{0}:{1};", sc.Name, sc.SplitPosition);
								value += string.Format("{0}:{1};", Function.DFnc.Property_Get_Value(c, "Name"), Function.DFnc.Property_Get_Value(c, "SplitPosition"));
								
								break;

							case "System.Windows.Forms.SplitContainer":
								value += string.Format("{0}:{1};", Function.DFnc.Property_Get_Value(c, "Name"), Function.DFnc.Property_Get_Value(c, "SplitterDistance"));

								break;


							case "System.Windows.Forms.CheckBox":
								CheckBox cbox = c as CheckBox;
								value += string.Format("{0}:{1};", cbox.Name, cbox.Checked);
								break;

							//크기 조정이 splitter로 안됌 -> 판넬 크기 기억으로 처리..
							//case "System.Windows.Forms.Splitter":
							//    Splitter sp = c as Splitter;
							//    value += string.Format("{0}:{1}:{2};", sp.Name, sp.Left, sp.Top);
							//    break;

							case "System.Windows.Forms.Panel":
								Panel pnl = c as Panel;
								value += string.Format("{0}:{1}:{2};", pnl.Name, pnl.Width, pnl.Height);
								break;

							case "System.Windows.Forms.ListView":
								ListView lst = c as ListView;

								value += Function.DFnc.Property_Get_Value(c, "Name").ToString() + ":";
																

								foreach (ColumnHeader col  in lst.Columns)
								{
									//value += string.Format("{0}/{1}:", col.FieldName, col.Width);
									value += string.Format("{0}/{1}:", col.Index, col.Width);
								}
								value += ";";

								break;

							case "System.Windows.Forms.DataGridView":
								System.Windows.Forms.DataGridView dgv = c as System.Windows.Forms.DataGridView;

								value += Function.DFnc.Property_Get_Value(c, "Name").ToString() + ":";
								
								foreach (DataGridViewColumn col in dgv.Columns)
								{
									//value += string.Format("{0}/{1}:", col.FieldName, col.Width);
									value += string.Format("{0}/{1}:", col.Index, col.Width);
								}
								value += ";";


								break;

							//case "Function.form.usrInputBox":
							//    Function.form.usrInputBox ib = c as Function.form.usrInputBox;
							//    value += string.Format("{0}:{1};", ib.Name, ib.Text);
							//    break;

							default:
								try
								{
									value += string.Format("{0}:{1};", c.Name, DFnc.Property_Get_Value(c, "Text"));
								}
								catch { }
								
								break;

						}



					}
					catch
					{
						continue;
					}
				}

				value += "*/";
				

				switch (_configFileType)
				{

					case enConfigFileType.ConfigXml:
						if (xml != null)
						{
							xml.Group_Select(_xmlSave_GroupName);
							xml.Value_Set(_propertySave_property_name, value);
							xml.Setting_Save();
						}
						break;

					default:
						Function.DFnc.Property_Set_Value(SavePosition_Setting, _propertySave_property_name, value);
						Function.DFnc.Method_Excute(SavePosition_Setting, "Save", null);						
						break;
				}



			}
			catch
			{

			}

		}


		protected void property_Load()
		{
			try
			{
				string value;
				
				//폼내용은 '/*폼이름:Top, Left, Width, Height, WindowState*/' 구성
				switch (_configFileType)
				{
					case enConfigFileType.ConfigXml:
						Function.Setting xml = SavePosition_Setting as Function.Setting;
						if (xml == null)
							value = string.Empty;
						else
						{
							xml.Group_Select(_xmlSave_GroupName);
							value = xml.Value_Get(_propertySave_property_name, string.Empty);
						}
						break;


					default:
						value = Fnc.obj2String(Function.DFnc.Property_Get_Value(SavePosition_Setting, _propertySave_property_name));
						break;
				}





				value = Function.form.fncForm.getSettingString(value, this.Name);
				string[] pp = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				Control c;
				string[] v;

				int idx;

				//컨트롤과 컨트롤 구분은 ';', 컨틀롤내 값사이구분은 '/'
				foreach (string p in pp)
				{
					try
					{
						v = p.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
						c = Function.form.fncForm.FindControlInControl(this, v[0]);
						if (c == null) continue;

						switch (c.GetType().ToString())
						{
							case "DevExpress.XtraGrid.GridControl":
								//DevExpress.XtraGrid.GridControl gc = c as DevExpress.XtraGrid.GridControl;
								//DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gc.FocusedView;

								Object gv = Function.DFnc.Property_Get_Value(c, "FocusedView");
								CollectionBase cols = (CollectionBase)Function.DFnc.Property_Get_Value(gv, "Columns");


								foreach (string cl in v)
								{
									string[] pv = cl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
									if (pv.Length < 2 || !Fnc.isNumeric(pv[1])) continue;

									//gv.Columns[pv[0]].Width = Fnc.obj2int(pv[1]);

									object col = Function.DFnc.Property_Get_Value(cols, "Item", new Type[] { typeof(string) }, new object[] { pv[0] });
									Function.DFnc.Property_Set_Value(col, "Width", Fnc.obj2int(pv[1]));
																											
								}


								break;


							case "DevExpress.XtraEditors.SplitterControl":
								//DevExpress.XtraEditors.SplitterControl sc = c as DevExpress.XtraEditors.SplitterControl;
								//sc.SplitPosition = Fnc.obj2int(v[1]);

								Function.DFnc.Property_Set_Value(c, "SplitPosition", Fnc.obj2int(v[1]));
								break;

							case "System.Windows.Forms.SplitContainer":
								Function.DFnc.Property_Set_Value(c, "SplitterDistance", Fnc.obj2int(v[1]));
								break;

							case "System.Windows.Forms.CheckBox":
								CheckBox cbox = c as CheckBox;
								cbox.Checked = bool.Parse(v[1]);
								break;

							//case "Function.form.usrInputBox":
							//    Function.form.usrInputBox ib = c as Function.form.usrInputBox;
							//    ib.Text = v[1];
							//    break;

							case "System.Windows.Forms.Panel":
								Panel pnl = c as Panel;
								pnl.Width = Fnc.obj2int(v[1]);
								pnl.Height = Fnc.obj2int(v[2]);
								break;

							case "System.Windows.Forms.Splitter":
								Splitter sp = c as Splitter;
								sp.Left = Fnc.obj2int(v[1]);
								sp.Top = Fnc.obj2int(v[2]);
								break;

							case "System.Windows.Forms.ListView":
								ListView lst = c as ListView;

								if (lst.Columns.Count != (v.Length-1)) return;

								idx = 0;

								foreach (string cl in v)
								{
									//첫번째는 이름이므로
									if(idx == 0)
									{
										idx++;
										continue;
									}

									try
									{
										string[] pv = cl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
										if (pv.Length < 2 || !Fnc.isNumeric(pv[1]) || !Fnc.isNumeric(pv[0])) continue;

										//gv.Columns[pv[0]].Width = Fnc.obj2int(pv[1]);
										lst.Columns[Fnc.obj2int(pv[0])].Width = Fnc.obj2int(pv[1]);
									}
									catch
									{

									}

									idx++;

								}

								break;

							case "System.Windows.Forms.DataGridView":
								System.Windows.Forms.DataGridView dgv = c as System.Windows.Forms.DataGridView;

								if (dgv.Columns.Count != (v.Length - 1)) return;

								idx = 0;

								foreach (string cl in v)
								{
									//첫번째는 이름이므로
									if (idx == 0)
									{
										idx++;
										continue;
									}

									try
									{
										string[] pv = cl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
										if (pv.Length < 2 || !Fnc.isNumeric(pv[1]) || !Fnc.isNumeric(pv[0])) continue;

										//gv.Columns[pv[0]].Width = Fnc.obj2int(pv[1]);
										dgv.Columns[Fnc.obj2int(pv[0])].Width = Fnc.obj2int(pv[1]);
									}
									catch
									{

									}

									idx++;

								}

								break;

							default:
								try
								{
									DFnc.Property_Set_Value(c, "Text", v[1]);

								}
								catch { }

								break;

						}



					}
					catch
					{
						continue;
					}
				}
			}
			catch
			{
			}


		}
		#endregion

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// clsbaseForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "clsbaseForm";
			this.ResumeLayout(false);

		}
	}
}
