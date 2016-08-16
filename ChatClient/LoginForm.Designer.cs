namespace ChatClient
{
    partial class LoginForm
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
            this.Text_ID = new System.Windows.Forms.TextBox();
            this.Text_PW = new System.Windows.Forms.TextBox();
            this.label_ID = new System.Windows.Forms.Label();
            this.label_PW = new System.Windows.Forms.Label();
            this.Btn_Login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Text_ID
            // 
            this.Text_ID.Location = new System.Drawing.Point(56, 12);
            this.Text_ID.Name = "Text_ID";
            this.Text_ID.Size = new System.Drawing.Size(178, 21);
            this.Text_ID.TabIndex = 0;
            // 
            // Text_PW
            // 
            this.Text_PW.Location = new System.Drawing.Point(56, 40);
            this.Text_PW.Name = "Text_PW";
            this.Text_PW.Size = new System.Drawing.Size(178, 21);
            this.Text_PW.TabIndex = 1;
            // 
            // label_ID
            // 
            this.label_ID.AutoSize = true;
            this.label_ID.Location = new System.Drawing.Point(12, 15);
            this.label_ID.Name = "label_ID";
            this.label_ID.Size = new System.Drawing.Size(16, 12);
            this.label_ID.TabIndex = 2;
            this.label_ID.Text = "ID";
            // 
            // label_PW
            // 
            this.label_PW.AutoSize = true;
            this.label_PW.Location = new System.Drawing.Point(12, 43);
            this.label_PW.Name = "label_PW";
            this.label_PW.Size = new System.Drawing.Size(23, 12);
            this.label_PW.TabIndex = 3;
            this.label_PW.Text = "PW";
            // 
            // Btn_Login
            // 
            this.Btn_Login.Location = new System.Drawing.Point(13, 71);
            this.Btn_Login.Name = "Btn_Login";
            this.Btn_Login.Size = new System.Drawing.Size(221, 23);
            this.Btn_Login.TabIndex = 4;
            this.Btn_Login.Text = "로그인";
            this.Btn_Login.UseVisualStyleBackColor = true;
            this.Btn_Login.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 102);
            this.Controls.Add(this.Btn_Login);
            this.Controls.Add(this.label_PW);
            this.Controls.Add(this.label_ID);
            this.Controls.Add(this.Text_PW);
            this.Controls.Add(this.Text_ID);
            this.Name = "LoginForm";
            this.Text = "채팅_로그인";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_ID;
        private System.Windows.Forms.TextBox Text_PW;
        private System.Windows.Forms.Label label_ID;
        private System.Windows.Forms.Label label_PW;
        private System.Windows.Forms.Button Btn_Login;
    }
}