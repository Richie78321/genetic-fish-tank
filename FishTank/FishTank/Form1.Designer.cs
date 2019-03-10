namespace FishTank
{
    partial class TankVisual
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.autoUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // autoUpdateTimer
            // 
            this.autoUpdateTimer.Tick += new System.EventHandler(this.autoUpdateTimer_Tick);
            // 
            // TankVisual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 619);
            this.Name = "TankVisual";
            this.Text = "Fish Tank";
            this.Load += new System.EventHandler(this.TankVisual_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer autoUpdateTimer;
    }
}

