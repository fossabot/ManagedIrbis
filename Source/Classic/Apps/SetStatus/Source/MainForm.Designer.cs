﻿namespace SetStatus
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._browser = new System.Windows.Forms.WebBrowser();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._statusButton = new System.Windows.Forms.Button();
            this._statusBox = new System.Windows.Forms.ComboBox();
            this._checkButton = new System.Windows.Forms.Button();
            this._numberBox = new System.Windows.Forms.TextBox();
            this._firstTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.IsSplitterFixed = true;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._numberBox);
            this._splitContainer.Panel1.Controls.Add(this._checkButton);
            this._splitContainer.Panel1.Controls.Add(this._statusBox);
            this._splitContainer.Panel1.Controls.Add(this._statusButton);
            this._splitContainer.Panel1.Controls.Add(this.label1);
            this._splitContainer.Panel1.Controls.Add(this.label2);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._browser);
            this._splitContainer.Panel2.Controls.Add(this._logBox);
            this._splitContainer.Panel2.Controls.Add(this._busyStripe);
            this._splitContainer.Size = new System.Drawing.Size(582, 453);
            this._splitContainer.SplitterDistance = 120;
            this._splitContainer.TabIndex = 0;
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._busyStripe.ForeColor = System.Drawing.Color.LimeGreen;
            this._busyStripe.Location = new System.Drawing.Point(0, 306);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(582, 23);
            this._busyStripe.TabIndex = 3;
            this._busyStripe.Visible = false;
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 234);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(582, 72);
            this._logBox.TabIndex = 1;
            // 
            // _browser
            // 
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.MinimumSize = new System.Drawing.Size(20, 20);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(582, 234);
            this._browser.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "New status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Inventory number or barcode";
            // 
            // _statusButton
            // 
            this._statusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._statusButton.Location = new System.Drawing.Point(381, 71);
            this._statusButton.Name = "_statusButton";
            this._statusButton.Size = new System.Drawing.Size(189, 44);
            this._statusButton.TabIndex = 2;
            this._statusButton.Text = "Set status";
            this._statusButton.UseVisualStyleBackColor = true;
            // 
            // _statusBox
            // 
            this._statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._statusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._statusBox.FormattingEnabled = true;
            this._statusBox.Location = new System.Drawing.Point(12, 91);
            this._statusBox.Name = "_statusBox";
            this._statusBox.Size = new System.Drawing.Size(355, 24);
            this._statusBox.TabIndex = 3;
            // 
            // _checkButton
            // 
            this._checkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._checkButton.Location = new System.Drawing.Point(381, 8);
            this._checkButton.Name = "_checkButton";
            this._checkButton.Size = new System.Drawing.Size(189, 42);
            this._checkButton.TabIndex = 1;
            this._checkButton.Text = "Check exemplar";
            this._checkButton.UseVisualStyleBackColor = true;
            this._checkButton.Click += new System.EventHandler(this._checkButton_Click);
            // 
            // _numberBox
            // 
            this._numberBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._numberBox.Location = new System.Drawing.Point(12, 28);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(355, 22);
            this._numberBox.TabIndex = 0;
            // 
            // _firstTimer
            // 
            this._firstTimer.Enabled = true;
            this._firstTimer.Tick += new System.EventHandler(this._firstTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 453);
            this.Controls.Add(this._splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "MainForm";
            this.Text = "Set exemplar status";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel1.PerformLayout();
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.TextBox _numberBox;
        private System.Windows.Forms.Button _checkButton;
        private System.Windows.Forms.ComboBox _statusBox;
        private System.Windows.Forms.Button _statusButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.WebBrowser _browser;
        private AM.Windows.Forms.LogBox _logBox;
        private AM.Windows.Forms.BusyStripe _busyStripe;
        private System.Windows.Forms.Timer _firstTimer;
    }
}

