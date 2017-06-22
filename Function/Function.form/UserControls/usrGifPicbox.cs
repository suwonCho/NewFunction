using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Function.form.UserControls
{
	public class usrGifPicbox : PictureBox
	{

		public new Image Image
		{
			get { return base.Image; }
			set
			{
				if(base.Image != null)
				{
					ImageAnimator.StopAnimate(base.Image, new EventHandler(this.OnFrameChanged));
				}						

				base.Image = value;

				if (base.Image != null)
				{
					ImageAnimator.Animate(base.Image, new EventHandler(this.OnFrameChanged));
				}
				
			}
		}


		public usrGifPicbox()
		{
			
		}



		protected override void OnPaint(PaintEventArgs e)

		{
			if (base.Image != null)			{

				ImageAnimator.UpdateFrames();
				e.Graphics.DrawImage(base.Image, new Point(0, 0));
			}

			base.OnPaint(e);

		}

		private void OnFrameChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}

	}
}
