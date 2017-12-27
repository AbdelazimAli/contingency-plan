using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApp.Reports.en_EmployeesReports
{
    // The DefaultBindableProperty attribute is intended to make the Position 
    // property bindable when an item is dropped from the Field List.
    [
    ToolboxItem(true),
    DefaultBindableProperty("Position")
    ]
    public class ProgressBar : XRControl
    {

        // The current position value.
        private float pos = 0;

        // The maximum value for the progress receiveBar position.
        private float maxVal = 100;

        public ProgressBar()
        {
            //this.ForeColor = SystemColors.Highlight;

            this.ForeColor = Color.Silver;
            //this.BackColor = Color.DarkOrange;
        }

        // Define the MaxValue property.
        [DefaultValue(100)]
        public float MaxValue
        {
            get { return this.maxVal; }
            set
            {
                if (value <= 0) return;
                this.maxVal = value;
            }
        }

        // Define the Position property. 
        [DefaultValue(0), Bindable(true)]
        public float Position
        {
            get { return this.pos; }
            set
            {
                if (value < 0 || value > maxVal)
                    return;
                this.pos = value;
            }
        }

        // Override the XRControl.CreateBrick method.
        protected override VisualBrick CreateBrick(VisualBrick[] childrenBricks)
        {
            // Use this code to make the progress receiveBar control 
            // always represented as a Panel brick.
            return new PanelBrick(this);
        }

        // Override the XRControl.PutStateToBrick method.
        protected override void PutStateToBrick(VisualBrick brick, PrintingSystemBase ps)
        {
            // Call the PutStateToBrick method of the base class.            
            base.PutStateToBrick(brick, ps);

            // Get the Panel brick which represents the current progress receiveBar control.
            PanelBrick panel = (PanelBrick)brick;

            int width = Convert.ToInt32(Math.Round(panel.Rect.Width));
            int height = Convert.ToInt32(Math.Round(panel.Rect.Height));

            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(300, 300);
            GdiGraphics gBmp = new GdiGraphics(Graphics.FromImage(bitmap), ps);

            int colorWidth = Convert.ToInt32(Math.Round(width * (Position / MaxValue)));

            //int colorWid = Convert.ToInt32(Math.Round((MaxValue-width) * (Position / MaxValue)));


            gBmp.FillRectangle(new SolidBrush(panel.Style.ForeColor), new Rectangle(0, 0, colorWidth, height));


           // gBmp.FillRectangle(new SolidBrush(panel.Style.BackColor), new Rectangle(0,colorWidth, colorWid, height));



            String drawString = Position.ToString();
            //
            drawString += " %";
            Font drawFont = new Font("Frutiger LT Arabic 55", 9);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            gBmp.DrawString(drawString, drawFont, drawBrush, new Rectangle(0, 0, width, height), drawFormat);

            gBmp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            ImageBrick image = new ImageBrick();
            image.Rect = new RectangleF(0, 0, panel.Rect.Width, panel.Rect.Height);
            image.SizeMode = ImageSizeMode.Squeeze;
            image.Image = bitmap;
            
            image.Sides = BorderSide.None;

            panel.Bricks.Add(image);
        }
    }
}