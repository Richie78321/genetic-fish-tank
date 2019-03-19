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
            this.neuralPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // autoUpdateTimer
            // 
            this.autoUpdateTimer.Enabled = true;
            this.autoUpdateTimer.Interval = 15;
            this.autoUpdateTimer.Tick += new System.EventHandler(this.autoUpdateTimer_Tick);
            // 
            // neuralPanel
            // 
            this.neuralPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.neuralPanel.Location = new System.Drawing.Point(699, 12);
            this.neuralPanel.Name = "neuralPanel";
            this.neuralPanel.Size = new System.Drawing.Size(622, 423);
            this.neuralPanel.TabIndex = 0;
            // 
            // TankVisual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 823);
            this.Controls.Add(this.neuralPanel);
            this.DoubleBuffered = true;
            this.Name = "TankVisual";
            this.Text = "Fish Tank";
            this.Load += new System.EventHandler(this.TankVisual_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer autoUpdateTimer;
        private System.Windows.Forms.Panel neuralPanel;
    }
}

