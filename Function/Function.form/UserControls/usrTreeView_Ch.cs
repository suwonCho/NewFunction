using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Function.form
{
	public partial class usrTreeView_Ch : UserControl
	{
		public TreeNode SelectedNode
		{
			get { return trMain.SelectedNode; }
			set { trMain.SelectedNode = value; }
		}

		public TreeNodeCollection Nodes
		{
			get { return trMain.Nodes; }			
		}

		TreeViewEventHandler _afterSelect;
		public event TreeViewEventHandler AfterSelect
		{
			add 
			{
				_afterSelect += value;
			}
			remove
			{
				_afterSelect -= value;
			}
		}

		TreeViewEventHandler _afterMoved;
		public event TreeViewEventHandler AfterMoved
		{
			add
			{
				_afterMoved += value;
			}
			remove
			{
				_afterMoved -= value;
			}
		}




		public bool HideSelection
		{
			get
			{
				return trMain.HideSelection;
			}
			set
			{
				trMain.HideSelection = value;
			}
		}

		public int ImageIndex
		{
			get
			{
				return trMain.ImageIndex;
			}
			set
			{
				trMain.ImageIndex = value;
			}
		}

		public int SelectedImageIndex
		{
			get
			{
				return trMain.SelectedImageIndex;
			}
			set
			{
				trMain.SelectedImageIndex = value;
			}
		}

		public ImageList ImageList
		{
			get
			{
				return trMain.ImageList;
			}
			set
			{
				trMain.ImageList = value;
			}
		}


		private void tree_move(bool isUp)
		{
			TreeNode tn = trMain.SelectedNode;
			if (tn == null) return;
			TreeNode pr = tn.Parent;

			

			if (isUp && tn.Index == 0) return;
			if (pr == null)
			{
				if (!isUp && tn.Index == trMain.Nodes.Count - 1) return;
			}
			else if (!isUp && tn.Index == pr.Nodes.Count - 1) return;

			int index = tn.Index;
			index += isUp ? -1 : 1;

			if (pr != null)
			{
				pr.Nodes.Remove(tn);
				pr.Nodes.Insert(index, tn);
			}
			else
			{
				trMain.Nodes.Remove(tn);
				trMain.Nodes.Insert(index, tn);
			}
			
			trMain.SelectedNode = tn;

			if(_afterMoved != null) _afterMoved(this, new TreeViewEventArgs(tn));
		}


		public usrTreeView_Ch()
		{	
			InitializeComponent();

			btnMC_Up.Image = Function.resIcon16.up;
			btnMC_Down.Image = Function.resIcon16.down;

		}

		private void trMain_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (_afterSelect != null) _afterSelect(this, e);
		}

		private void btnMC_Up_Click(object sender, EventArgs e)
		{
			tree_move(true);
		}

		private void btnMC_Down_Click(object sender, EventArgs e)
		{
			tree_move(false);
		}
	}
}
