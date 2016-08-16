namespace ChatClient
{
    partial class LobbyScene
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_ChangeNickname = new System.Windows.Forms.Button();
            this.Btn_EnterChatRoom = new System.Windows.Forms.Button();
            this.Btn_CreateChatRoom = new System.Windows.Forms.Button();
            this.ListBox_RoomList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Btn_ChangeNickname
            // 
            this.Btn_ChangeNickname.Location = new System.Drawing.Point(197, 255);
            this.Btn_ChangeNickname.Name = "Btn_ChangeNickname";
            this.Btn_ChangeNickname.Size = new System.Drawing.Size(75, 23);
            this.Btn_ChangeNickname.TabIndex = 4;
            this.Btn_ChangeNickname.Text = "닉네임변경";
            this.Btn_ChangeNickname.UseVisualStyleBackColor = true;
            this.Btn_ChangeNickname.Click += new System.EventHandler(this.Btn_ChangeNickname_Click);
            // 
            // Btn_EnterChatRoom
            // 
            this.Btn_EnterChatRoom.Location = new System.Drawing.Point(12, 255);
            this.Btn_EnterChatRoom.Name = "Btn_EnterChatRoom";
            this.Btn_EnterChatRoom.Size = new System.Drawing.Size(75, 23);
            this.Btn_EnterChatRoom.TabIndex = 5;
            this.Btn_EnterChatRoom.Text = "방 입장";
            this.Btn_EnterChatRoom.UseVisualStyleBackColor = true;
            this.Btn_EnterChatRoom.Click += new System.EventHandler(this.Btn_EnterChatRoom_Click);
            // 
            // Btn_CreateChatRoom
            // 
            this.Btn_CreateChatRoom.Location = new System.Drawing.Point(105, 255);
            this.Btn_CreateChatRoom.Name = "Btn_CreateChatRoom";
            this.Btn_CreateChatRoom.Size = new System.Drawing.Size(75, 23);
            this.Btn_CreateChatRoom.TabIndex = 6;
            this.Btn_CreateChatRoom.Text = "방 생성";
            this.Btn_CreateChatRoom.UseVisualStyleBackColor = true;
            this.Btn_CreateChatRoom.Click += new System.EventHandler(this.Btn_CreateChatRoom_Click);
            // 
            // ListBox_RoomList
            // 
            this.ListBox_RoomList.FormattingEnabled = true;
            this.ListBox_RoomList.ItemHeight = 12;
            this.ListBox_RoomList.Location = new System.Drawing.Point(12, 12);
            this.ListBox_RoomList.Name = "ListBox_RoomList";
            this.ListBox_RoomList.Size = new System.Drawing.Size(259, 232);
            this.ListBox_RoomList.TabIndex = 7;
            // 
            // LobbyScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 283);
            this.Controls.Add(this.ListBox_RoomList);
            this.Controls.Add(this.Btn_CreateChatRoom);
            this.Controls.Add(this.Btn_EnterChatRoom);
            this.Controls.Add(this.Btn_ChangeNickname);
            this.Name = "LobbyScene";
            this.Text = "채팅_로비";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ChangeNickname;
        private System.Windows.Forms.Button Btn_EnterChatRoom;
        private System.Windows.Forms.Button Btn_CreateChatRoom;
        private System.Windows.Forms.ListBox ListBox_RoomList;
    }
}

