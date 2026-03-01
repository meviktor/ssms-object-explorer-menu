namespace SSMSObjectExplorer.controls
{
    partial class AdvancedFilterControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serverLabel = new System.Windows.Forms.Label();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.schemaLabel = new System.Windows.Forms.Label();
            this.tableLabel = new System.Windows.Forms.Label();
            this.columnLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.tableTextBox = new System.Windows.Forms.TextBox();
            this.schemaTextBox = new System.Windows.Forms.TextBox();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.columnTextBox = new System.Windows.Forms.TextBox();
            this.additionalFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.additionalFilterGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(16, 14);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(47, 16);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server";
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point(277, 16);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(67, 16);
            this.databaseLabel.TabIndex = 1;
            this.databaseLabel.Text = "Database";
            // 
            // schemaLabel
            // 
            this.schemaLabel.AutoSize = true;
            this.schemaLabel.Location = new System.Drawing.Point(6, 53);
            this.schemaLabel.Name = "schemaLabel";
            this.schemaLabel.Size = new System.Drawing.Size(57, 16);
            this.schemaLabel.TabIndex = 2;
            this.schemaLabel.Text = "Schema";
            // 
            // tableLabel
            // 
            this.tableLabel.AutoSize = true;
            this.tableLabel.Location = new System.Drawing.Point(182, 50);
            this.tableLabel.Name = "tableLabel";
            this.tableLabel.Size = new System.Drawing.Size(43, 16);
            this.tableLabel.TabIndex = 3;
            this.tableLabel.Text = "Table";
            // 
            // columnLabel
            // 
            this.columnLabel.AutoSize = true;
            this.columnLabel.Location = new System.Drawing.Point(353, 50);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(52, 16);
            this.columnLabel.TabIndex = 4;
            this.columnLabel.Text = "Column";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(68, 14);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(200, 22);
            this.serverTextBox.TabIndex = 5;
            // 
            // tableTextBox
            // 
            this.tableTextBox.Location = new System.Drawing.Point(235, 50);
            this.tableTextBox.Name = "tableTextBox";
            this.tableTextBox.Size = new System.Drawing.Size(100, 22);
            this.tableTextBox.TabIndex = 6;
            // 
            // schemaTextBox
            // 
            this.schemaTextBox.Location = new System.Drawing.Point(71, 50);
            this.schemaTextBox.Name = "schemaTextBox";
            this.schemaTextBox.Size = new System.Drawing.Size(100, 22);
            this.schemaTextBox.TabIndex = 7;
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.Location = new System.Drawing.Point(353, 16);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(200, 22);
            this.databaseTextBox.TabIndex = 8;
            // 
            // columnTextBox
            // 
            this.columnTextBox.Location = new System.Drawing.Point(422, 50);
            this.columnTextBox.Name = "columnTextBox";
            this.columnTextBox.Size = new System.Drawing.Size(131, 22);
            this.columnTextBox.TabIndex = 9;
            // 
            // additionalFilterGroupBox
            // 
            this.additionalFilterGroupBox.Controls.Add(this.columnTextBox);
            this.additionalFilterGroupBox.Controls.Add(this.databaseTextBox);
            this.additionalFilterGroupBox.Controls.Add(this.schemaTextBox);
            this.additionalFilterGroupBox.Controls.Add(this.tableTextBox);
            this.additionalFilterGroupBox.Controls.Add(this.serverTextBox);
            this.additionalFilterGroupBox.Controls.Add(this.columnLabel);
            this.additionalFilterGroupBox.Controls.Add(this.tableLabel);
            this.additionalFilterGroupBox.Controls.Add(this.schemaLabel);
            this.additionalFilterGroupBox.Controls.Add(this.databaseLabel);
            this.additionalFilterGroupBox.Controls.Add(this.serverLabel);
            this.additionalFilterGroupBox.Location = new System.Drawing.Point(9, -3);
            this.additionalFilterGroupBox.Name = "additionalFilterGroupBox";
            this.additionalFilterGroupBox.Size = new System.Drawing.Size(561, 86);
            this.additionalFilterGroupBox.TabIndex = 10;
            this.additionalFilterGroupBox.TabStop = false;
            // 
            // AdvancedFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.additionalFilterGroupBox);
            this.Name = "AdvancedFilterControl";
            this.Size = new System.Drawing.Size(577, 86);
            this.additionalFilterGroupBox.ResumeLayout(false);
            this.additionalFilterGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.Label schemaLabel;
        private System.Windows.Forms.Label tableLabel;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.TextBox tableTextBox;
        private System.Windows.Forms.TextBox schemaTextBox;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.TextBox columnTextBox;
        private System.Windows.Forms.GroupBox additionalFilterGroupBox;
    }
}
