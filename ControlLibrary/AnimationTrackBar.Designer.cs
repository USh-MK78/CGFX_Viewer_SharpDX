namespace ControlLibrary
{
    partial class AnimationTrackBar
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AnimStop_Btn = new System.Windows.Forms.Button();
            this.AnimPlay_Btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MaxAnimRangeValue_TXT = new System.Windows.Forms.TextBox();
            this.CurrentAnimPosValue_TXT = new System.Windows.Forms.TextBox();
            this.CANM_AnimTrackBar = new System.Windows.Forms.TrackBar();
            this.CANM_AnimationTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.CANM_AnimTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // AnimStop_Btn
            // 
            this.AnimStop_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AnimStop_Btn.Location = new System.Drawing.Point(8, 30);
            this.AnimStop_Btn.Name = "AnimStop_Btn";
            this.AnimStop_Btn.Size = new System.Drawing.Size(36, 19);
            this.AnimStop_Btn.TabIndex = 34;
            this.AnimStop_Btn.Text = "Stop";
            this.AnimStop_Btn.UseVisualStyleBackColor = true;
            this.AnimStop_Btn.Click += new System.EventHandler(this.AnimStop_Btn_Click);
            // 
            // AnimPlay_Btn
            // 
            this.AnimPlay_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AnimPlay_Btn.Location = new System.Drawing.Point(8, 4);
            this.AnimPlay_Btn.Name = "AnimPlay_Btn";
            this.AnimPlay_Btn.Size = new System.Drawing.Size(36, 19);
            this.AnimPlay_Btn.TabIndex = 33;
            this.AnimPlay_Btn.Text = "Play";
            this.AnimPlay_Btn.UseVisualStyleBackColor = true;
            this.AnimPlay_Btn.Click += new System.EventHandler(this.AnimPlay_Btn_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "MaxTime :";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "CurrentTime :";
            // 
            // MaxAnimRangeValue_TXT
            // 
            this.MaxAnimRangeValue_TXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxAnimRangeValue_TXT.Location = new System.Drawing.Point(365, 29);
            this.MaxAnimRangeValue_TXT.Name = "MaxAnimRangeValue_TXT";
            this.MaxAnimRangeValue_TXT.Size = new System.Drawing.Size(48, 19);
            this.MaxAnimRangeValue_TXT.TabIndex = 30;
            this.MaxAnimRangeValue_TXT.Text = "60";
            this.MaxAnimRangeValue_TXT.TextChanged += new System.EventHandler(this.MaxAnimRangeValue_TXT_TextChanged);
            // 
            // CurrentAnimPosValue_TXT
            // 
            this.CurrentAnimPosValue_TXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentAnimPosValue_TXT.Location = new System.Drawing.Point(365, 4);
            this.CurrentAnimPosValue_TXT.Name = "CurrentAnimPosValue_TXT";
            this.CurrentAnimPosValue_TXT.Size = new System.Drawing.Size(48, 19);
            this.CurrentAnimPosValue_TXT.TabIndex = 29;
            this.CurrentAnimPosValue_TXT.Text = "0";
            this.CurrentAnimPosValue_TXT.TextChanged += new System.EventHandler(this.CurrentAnimPosValue_TXT_TextChanged);
            // 
            // CANM_AnimTrackBar
            // 
            this.CANM_AnimTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CANM_AnimTrackBar.Location = new System.Drawing.Point(50, 4);
            this.CANM_AnimTrackBar.Name = "CANM_AnimTrackBar";
            this.CANM_AnimTrackBar.Size = new System.Drawing.Size(229, 45);
            this.CANM_AnimTrackBar.TabIndex = 28;
            this.CANM_AnimTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.CANM_AnimTrackBar.Scroll += new System.EventHandler(this.CANM_AnimTrackBar_Scroll);
            this.CANM_AnimTrackBar.ValueChanged += new System.EventHandler(this.CANM_AnimTrackBar_ValueChanged);
            // 
            // CANM_AnimationTimer
            // 
            this.CANM_AnimationTimer.Tick += new System.EventHandler(this.CANM_AnimationTimer_Tick);
            // 
            // AnimationTrackBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AnimStop_Btn);
            this.Controls.Add(this.AnimPlay_Btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MaxAnimRangeValue_TXT);
            this.Controls.Add(this.CurrentAnimPosValue_TXT);
            this.Controls.Add(this.CANM_AnimTrackBar);
            this.Name = "AnimationTrackBar";
            this.Size = new System.Drawing.Size(422, 52);
            this.Load += new System.EventHandler(this.AnimationTrackBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CANM_AnimTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AnimStop_Btn;
        private System.Windows.Forms.Button AnimPlay_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MaxAnimRangeValue_TXT;
        private System.Windows.Forms.TextBox CurrentAnimPosValue_TXT;
        private System.Windows.Forms.TrackBar CANM_AnimTrackBar;
        private System.Windows.Forms.Timer CANM_AnimationTimer;
    }
}
