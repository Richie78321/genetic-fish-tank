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
            this.layerIndexNumeric = new System.Windows.Forms.NumericUpDown();
            this.neuronIndexNumeric = new System.Windows.Forms.NumericUpDown();
            this.neuronInputLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.layerIndexNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.neuronIndexNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // autoUpdateTimer
            // 
            this.autoUpdateTimer.Enabled = true;
            this.autoUpdateTimer.Interval = 10;
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
            // layerIndexNumeric
            // 
            this.layerIndexNumeric.Location = new System.Drawing.Point(699, 441);
            this.layerIndexNumeric.Name = "layerIndexNumeric";
            this.layerIndexNumeric.Size = new System.Drawing.Size(48, 20);
            this.layerIndexNumeric.TabIndex = 0;
            // 
            // neuronIndexNumeric
            // 
            this.neuronIndexNumeric.Location = new System.Drawing.Point(753, 441);
            this.neuronIndexNumeric.Name = "neuronIndexNumeric";
            this.neuronIndexNumeric.Size = new System.Drawing.Size(48, 20);
            this.neuronIndexNumeric.TabIndex = 1;
            // 
            // neuronInputLabel
            // 
            this.neuronInputLabel.AutoSize = true;
            this.neuronInputLabel.Location = new System.Drawing.Point(807, 445);
            this.neuronInputLabel.Name = "neuronInputLabel";
            this.neuronInputLabel.Size = new System.Drawing.Size(71, 13);
            this.neuronInputLabel.TabIndex = 2;
            this.neuronInputLabel.Text = "Neuron Label";
            // 
            // TankVisual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 823);
            this.Controls.Add(this.neuronInputLabel);
            this.Controls.Add(this.neuronIndexNumeric);
            this.Controls.Add(this.layerIndexNumeric);
            this.Controls.Add(this.neuralPanel);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "TankVisual";
            this.Text = "Fish Tank";
            this.Load += new System.EventHandler(this.TankVisual_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            ((System.ComponentModel.ISupportInitialize)(this.layerIndexNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.neuronIndexNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer autoUpdateTimer;
        private System.Windows.Forms.Panel neuralPanel;
        private System.Windows.Forms.NumericUpDown layerIndexNumeric;
        private System.Windows.Forms.NumericUpDown neuronIndexNumeric;
        private System.Windows.Forms.Label neuronInputLabel;
    }
}

