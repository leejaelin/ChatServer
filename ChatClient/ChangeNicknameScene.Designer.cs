namespace ChatClient
{
    partial class ChangeNicknameScene
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
            this.TextBox_Nickname = new System.Windows.Forms.TextBox();
            this.Btn_SendChangeNickname = new System.Windows.Forms.Button();
            this.Btn_CancelChangeNickname = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBox_Nickname
            // 
            this.TextBox_Nickname.Location = new System.Drawing.Point(13, 13);
            this.TextBox_Nickname.Name = "TextBox_Nickname";
            this.TextBox_Nickname.Size = new System.Drawing.Size(179, 21);
            this.TextBox_Nickname.TabIndex = 0;
            // 
            // Btn_SendChangeNickname
            // 
            this.Btn_SendChangeNickname.Location = new System.Drawing.Point(13, 41);
            this.Btn_SendChangeNickname.Name = "Btn_SendChangeNickname";
            this.Btn_SendChangeNickname.Size = new System.Drawing.Size(84, 23);
            this.Btn_SendChangeNickname.TabIndex = 1;
            this.Btn_SendChangeNickname.Text = "닉네임 변경";
            this.Btn_SendChangeNickname.UseVisualStyleBackColor = true;
            this.Btn_SendChangeNickname.Click += new System.EventHandler(this.button1_Click);
            // 
            // Btn_CancelChangeNickname
            // 
            this.Btn_CancelChangeNickname.Location = new System.Drawing.Point(103, 41);
            this.Btn_CancelChangeNickname.Name = "Btn_CancelChangeNickname";
            this.Btn_CancelChangeNickname.Size = new System.Drawing.Size(89, 23);
            this.Btn_CancelChangeNickname.TabIndex = 2;
            this.Btn_CancelChangeNickname.Text = "취소";
            this.Btn_CancelChangeNickname.UseVisualStyleBackColor = true;
            this.Btn_CancelChangeNickname.Click += new System.EventHandler(this.button2_Click);
            // 
            // ChangeNicknameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 72);
            this.Controls.Add(this.Btn_CancelChangeNickname);
            this.Controls.Add(this.Btn_SendChangeNickname);
            this.Controls.Add(this.TextBox_Nickname);
            this.Name = "ChangeNicknameForm";
            this.Text = "채팅_닉네임변경";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox_Nickname;
        private System.Windows.Forms.Button Btn_SendChangeNickname;
        private System.Windows.Forms.Button Btn_CancelChangeNickname;
    }
}