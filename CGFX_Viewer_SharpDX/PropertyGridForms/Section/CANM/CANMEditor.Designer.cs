namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CANM
{
    partial class CANMEditor
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
            this.TreeViewPropSplitContainer = new System.Windows.Forms.SplitContainer();
            this.CANM_AnimNodeTreeView = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.AnimViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.animationTrackBar1 = new ControlLibrary.AnimationTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.TreeViewPropSplitContainer)).BeginInit();
            this.TreeViewPropSplitContainer.Panel1.SuspendLayout();
            this.TreeViewPropSplitContainer.Panel2.SuspendLayout();
            this.TreeViewPropSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimViewSplitContainer)).BeginInit();
            this.AnimViewSplitContainer.Panel1.SuspendLayout();
            this.AnimViewSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeViewPropSplitContainer
            // 
            this.TreeViewPropSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewPropSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.TreeViewPropSplitContainer.Name = "TreeViewPropSplitContainer";
            this.TreeViewPropSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // TreeViewPropSplitContainer.Panel1
            // 
            this.TreeViewPropSplitContainer.Panel1.Controls.Add(this.CANM_AnimNodeTreeView);
            // 
            // TreeViewPropSplitContainer.Panel2
            // 
            this.TreeViewPropSplitContainer.Panel2.Controls.Add(this.propertyGrid1);
            this.TreeViewPropSplitContainer.Size = new System.Drawing.Size(200, 567);
            this.TreeViewPropSplitContainer.SplitterDistance = 332;
            this.TreeViewPropSplitContainer.TabIndex = 2;
            // 
            // CANM_AnimNodeTreeView
            // 
            this.CANM_AnimNodeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CANM_AnimNodeTreeView.Location = new System.Drawing.Point(0, 0);
            this.CANM_AnimNodeTreeView.Name = "CANM_AnimNodeTreeView";
            this.CANM_AnimNodeTreeView.Size = new System.Drawing.Size(200, 332);
            this.CANM_AnimNodeTreeView.TabIndex = 0;
            this.CANM_AnimNodeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CANM_AnimNodeTreeView_AfterSelect);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(200, 231);
            this.propertyGrid1.TabIndex = 0;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.TreeViewPropSplitContainer);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.AnimViewSplitContainer);
            this.MainSplitContainer.Size = new System.Drawing.Size(935, 567);
            this.MainSplitContainer.SplitterDistance = 200;
            this.MainSplitContainer.TabIndex = 3;
            // 
            // AnimViewSplitContainer
            // 
            this.AnimViewSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AnimViewSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.AnimViewSplitContainer.Name = "AnimViewSplitContainer";
            // 
            // AnimViewSplitContainer.Panel1
            // 
            this.AnimViewSplitContainer.Panel1.Controls.Add(this.label1);
            this.AnimViewSplitContainer.Panel1.Controls.Add(this.animationTrackBar1);
            this.AnimViewSplitContainer.Size = new System.Drawing.Size(731, 567);
            this.AnimViewSplitContainer.SplitterDistance = 493;
            this.AnimViewSplitContainer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 452);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "CANMEditor_CurveView2D";
            // 
            // animationTrackBar1
            // 
            this.animationTrackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animationTrackBar1.BarCurrentValue = 0;
            this.animationTrackBar1.BarMaxValue = 60;
            this.animationTrackBar1.IsEnabled = true;
            this.animationTrackBar1.IsLoop = true;
            this.animationTrackBar1.Location = new System.Drawing.Point(3, 512);
            this.animationTrackBar1.Name = "animationTrackBar1";
            this.animationTrackBar1.Size = new System.Drawing.Size(487, 52);
            this.animationTrackBar1.TabIndex = 0;
            // 
            // CANMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 567);
            this.Controls.Add(this.MainSplitContainer);
            this.Name = "CANMEditor";
            this.Text = "CANMEditor";
            this.Load += new System.EventHandler(this.CANMEditor_Load);
            this.TreeViewPropSplitContainer.Panel1.ResumeLayout(false);
            this.TreeViewPropSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TreeViewPropSplitContainer)).EndInit();
            this.TreeViewPropSplitContainer.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.AnimViewSplitContainer.Panel1.ResumeLayout(false);
            this.AnimViewSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimViewSplitContainer)).EndInit();
            this.AnimViewSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer TreeViewPropSplitContainer;
        private System.Windows.Forms.TreeView CANM_AnimNodeTreeView;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.SplitContainer AnimViewSplitContainer;
        private ControlLibrary.AnimationTrackBar animationTrackBar1;
        private System.Windows.Forms.Label label1;
    }
}