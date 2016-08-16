namespace ChatClient
{
    partial class ChatRoomScene
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
            this.RichTextBox_ReadChat = new System.Windows.Forms.RichTextBox();
            this.TextBox_WriteChat = new System.Windows.Forms.TextBox();
            this.Btn_SendChat = new System.Windows.Forms.Button();
            this.listBox_UserList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // RichTextBox_ReadChat
            // 
            this.RichTextBox_ReadChat.Location = new System.Drawing.Point(4, 4);
            this.RichTextBox_ReadChat.Name = "RichTextBox_ReadChat";
            this.RichTextBox_ReadChat.Size = new System.Drawing.Size(276, 230);
            this.RichTextBox_ReadChat.TabIndex = 0;
            this.RichTextBox_ReadChat.Text = "";
            // 
            // TextBox_WriteChat
            // 
            this.TextBox_WriteChat.Location = new System.Drawing.Point(4, 240);
            this.TextBox_WriteChat.Name = "TextBox_WriteChat";
            this.TextBox_WriteChat.Size = new System.Drawing.Size(276, 21);
            this.TextBox_WriteChat.TabIndex = 1;
            // 
            // Btn_SendChat
            // 
            this.Btn_SendChat.Location = new System.Drawing.Point(287, 240);
            this.Btn_SendChat.Name = "Btn_SendChat";
            this.Btn_SendChat.Size = new System.Drawing.Size(75, 23);
            this.Btn_SendChat.TabIndex = 2;
            this.Btn_SendChat.Text = "button1";
            this.Btn_SendChat.UseVisualStyleBackColor = true;
            // 
            // listBox_UserList
            // 
            this.listBox_UserList.FormattingEnabled = true;
            this.listBox_UserList.ItemHeight = 12;
            this.listBox_UserList.Location = new System.Drawing.Point(287, 4);
            this.listBox_UserList.Name = "listBox_UserList";
            this.listBox_UserList.Size = new System.Drawing.Size(75, 232);
            this.listBox_UserList.TabIndex = 3;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 265);
            this.Controls.Add(this.listBox_UserList);
            this.Controls.Add(this.Btn_SendChat);
            this.Controls.Add(this.TextBox_WriteChat);
            this.Controls.Add(this.RichTextBox_ReadChat);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBox_ReadChat;
        private System.Windows.Forms.TextBox TextBox_WriteChat;
        private System.Windows.Forms.Button Btn_SendChat;
        private System.Windows.Forms.ListBox listBox_UserList;
    }
}