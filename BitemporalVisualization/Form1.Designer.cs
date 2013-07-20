namespace BitemporalVisualization
{
    partial class Form1
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.hoveTextLabel = new System.Windows.Forms.Label();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.drawingPanel = new System.Windows.Forms.DoubleBufferedPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.drawingPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.hoveTextLabel);
            this.splitContainer.Size = new System.Drawing.Size(984, 561);
            this.splitContainer.SplitterDistance = 700;
            this.splitContainer.TabIndex = 0;
            // 
            // hoveTextLabel
            // 
            this.hoveTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hoveTextLabel.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.hoveTextLabel.Location = new System.Drawing.Point(0, 0);
            this.hoveTextLabel.Name = "hoveTextLabel";
            this.hoveTextLabel.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.hoveTextLabel.Size = new System.Drawing.Size(278, 559);
            this.hoveTextLabel.TabIndex = 0;
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.CreatePrompt = true;
            this.saveImageDialog.DefaultExt = "png";
            this.saveImageDialog.Filter = "PNG Files|*.png";
            this.saveImageDialog.Title = "Save Image";
            this.saveImageDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveImageDialog_FileOk);
            // 
            // drawingPanel
            // 
            this.drawingPanel.BackColor = System.Drawing.Color.White;
            this.drawingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingPanel.Location = new System.Drawing.Point(0, 0);
            this.drawingPanel.Name = "drawingPanel";
            this.drawingPanel.Size = new System.Drawing.Size(698, 559);
            this.drawingPanel.TabIndex = 0;
            this.drawingPanel.Click += new System.EventHandler(this.drawingPanel_Click);
            this.drawingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawingPanel_Paint);
            this.drawingPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawingPanel_MouseMove);
            this.drawingPanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.drawingPanel_MouseWheel);
            this.drawingPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.drawingPanel_PreviewKeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitContainer);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label hoveTextLabel;
        private System.Windows.Forms.DoubleBufferedPanel drawingPanel;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
    }
}

