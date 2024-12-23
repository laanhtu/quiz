namespace Client
{
    partial class Main
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageLeader = new System.Windows.Forms.TabPage();
            this.dataGridViewLeaders = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumberOfQuiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageQuiz = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSessionParticipants = new System.Windows.Forms.Label();
            this.dataGridViewParticipants = new System.Windows.Forms.DataGridView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblQuizDesc = new System.Windows.Forms.Label();
            this.lblQuizName = new System.Windows.Forms.Label();
            this.colUserFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserSessionScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPageLeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeaders)).BeginInit();
            this.tabPageQuiz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParticipants)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageLeader);
            this.tabControl1.Controls.Add(this.tabPageQuiz);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1153, 643);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageLeader
            // 
            this.tabPageLeader.Controls.Add(this.dataGridViewLeaders);
            this.tabPageLeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageLeader.Location = new System.Drawing.Point(4, 22);
            this.tabPageLeader.Name = "tabPageLeader";
            this.tabPageLeader.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLeader.Size = new System.Drawing.Size(1145, 617);
            this.tabPageLeader.TabIndex = 0;
            this.tabPageLeader.Text = "Leader Board          ";
            this.tabPageLeader.UseVisualStyleBackColor = true;
            // 
            // dataGridViewLeaders
            // 
            this.dataGridViewLeaders.AllowUserToAddRows = false;
            this.dataGridViewLeaders.AllowUserToDeleteRows = false;
            this.dataGridViewLeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLeaders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colScore,
            this.colNumberOfQuiz});
            this.dataGridViewLeaders.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewLeaders.Name = "dataGridViewLeaders";
            this.dataGridViewLeaders.Size = new System.Drawing.Size(756, 388);
            this.dataGridViewLeaders.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "FullName";
            this.colName.HeaderText = "Full name";
            this.colName.Name = "colName";
            this.colName.Width = 200;
            // 
            // colScore
            // 
            this.colScore.DataPropertyName = "Score";
            this.colScore.HeaderText = "Score";
            this.colScore.Name = "colScore";
            this.colScore.ReadOnly = true;
            // 
            // colNumberOfQuiz
            // 
            this.colNumberOfQuiz.DataPropertyName = "NumberOfQuiz";
            this.colNumberOfQuiz.HeaderText = "Number of Quiz";
            this.colNumberOfQuiz.Name = "colNumberOfQuiz";
            this.colNumberOfQuiz.ReadOnly = true;
            this.colNumberOfQuiz.Width = 200;
            // 
            // tabPageQuiz
            // 
            this.tabPageQuiz.Controls.Add(this.splitContainer1);
            this.tabPageQuiz.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuiz.Name = "tabPageQuiz";
            this.tabPageQuiz.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQuiz.Size = new System.Drawing.Size(1145, 617);
            this.tabPageQuiz.TabIndex = 1;
            this.tabPageQuiz.Text = "Quiz            ";
            this.tabPageQuiz.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblSessionParticipants);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewParticipants);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnSubmit);
            this.splitContainer1.Panel2.Controls.Add(this.lblQuizDesc);
            this.splitContainer1.Panel2.Controls.Add(this.lblQuizName);
            this.splitContainer1.Size = new System.Drawing.Size(1139, 611);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblSessionParticipants
            // 
            this.lblSessionParticipants.AutoSize = true;
            this.lblSessionParticipants.Location = new System.Drawing.Point(3, 9);
            this.lblSessionParticipants.Name = "lblSessionParticipants";
            this.lblSessionParticipants.Size = new System.Drawing.Size(62, 13);
            this.lblSessionParticipants.TabIndex = 1;
            this.lblSessionParticipants.Text = "Participants";
            // 
            // dataGridViewParticipants
            // 
            this.dataGridViewParticipants.AllowUserToAddRows = false;
            this.dataGridViewParticipants.AllowUserToDeleteRows = false;
            this.dataGridViewParticipants.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewParticipants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewParticipants.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserFullName,
            this.colUserSessionScore});
            this.dataGridViewParticipants.Location = new System.Drawing.Point(3, 33);
            this.dataGridViewParticipants.Name = "dataGridViewParticipants";
            this.dataGridViewParticipants.Size = new System.Drawing.Size(340, 575);
            this.dataGridViewParticipants.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Enabled = false;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(11, 563);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(122, 39);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblQuizDesc
            // 
            this.lblQuizDesc.AutoSize = true;
            this.lblQuizDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuizDesc.Location = new System.Drawing.Point(8, 33);
            this.lblQuizDesc.Name = "lblQuizDesc";
            this.lblQuizDesc.Size = new System.Drawing.Size(93, 20);
            this.lblQuizDesc.TabIndex = 1;
            this.lblQuizDesc.Text = "Description:";
            // 
            // lblQuizName
            // 
            this.lblQuizName.AutoSize = true;
            this.lblQuizName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuizName.Location = new System.Drawing.Point(8, 9);
            this.lblQuizName.Name = "lblQuizName";
            this.lblQuizName.Size = new System.Drawing.Size(45, 20);
            this.lblQuizName.TabIndex = 0;
            this.lblQuizName.Text = "Quiz:";
            // 
            // colUserFullName
            // 
            this.colUserFullName.DataPropertyName = "FullName";
            this.colUserFullName.HeaderText = "User";
            this.colUserFullName.Name = "colUserFullName";
            this.colUserFullName.ReadOnly = true;
            this.colUserFullName.Width = 200;
            // 
            // colUserSessionScore
            // 
            this.colUserSessionScore.DataPropertyName = "Score";
            this.colUserSessionScore.HeaderText = "Score";
            this.colUserSessionScore.Name = "colUserSessionScore";
            this.colUserSessionScore.ReadOnly = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 667);
            this.Controls.Add(this.tabControl1);
            this.Name = "Main";
            this.Text = "Elas Quiz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageLeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeaders)).EndInit();
            this.tabPageQuiz.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParticipants)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageLeader;
        private System.Windows.Forms.TabPage tabPageQuiz;
        private System.Windows.Forms.DataGridView dataGridViewLeaders;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblSessionParticipants;
        private System.Windows.Forms.DataGridView dataGridViewParticipants;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberOfQuiz;
        private System.Windows.Forms.Label lblQuizName;
        private System.Windows.Forms.Label lblQuizDesc;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserSessionScore;
    }
}

