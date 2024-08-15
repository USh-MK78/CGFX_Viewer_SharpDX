using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CGFXLibrary;
using System.Windows.Media;
using SharpDX;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CANM
{
    public partial class CANMEditor : Form
    {

        public List<CGFXLibrary.CGFXSection.DataComponent.AnimationData> AnimationDataList { get; set; }

        public CANMEditor(List<CGFXLibrary.CGFXSection.DataComponent.AnimationData> animationDatas)
        {
            InitializeComponent();

            AnimationDataList = animationDatas;
        }

        private void CANMEditor_Load(object sender, EventArgs e)
        {

            animationTrackBar1.BarCurrentValue = 0;
            animationTrackBar1.BarMaxValue = 60;
            animationTrackBar1.IsLoop = true;

            #region TreeNode
            List<TreeNode> treeNodeList = new List<TreeNode>();

            foreach (var item in AnimationDataList)
            {
                TreeNode AnimDataSetTreeNode = new TreeNode(item.Name);
                treeNodeList.Add(AnimDataSetTreeNode);
            }

            TreeNode RootNode = new TreeNode("AnimationData_Root", treeNodeList.ToArray());
            CANM_AnimNodeTreeView.Nodes.Add(RootNode);
            CANM_AnimNodeTreeView.TopNode.Expand();
            #endregion
        }

        private void CANM_AnimNodeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (CANM_AnimNodeTreeView.SelectedNode != null)
            {
                CANM_AnimNodeTreeView.PathSeparator = ",";

                string[] Set = CANM_AnimNodeTreeView.SelectedNode.FullPath.Split(',');

                if (Set.Length == 2)
                {
                    propertyGrid1.SelectedObject = new CANM_CurveData_PropertyGrid(AnimationDataList.Find(x => x.Name == Set[1]));
                }
            }
        }
    }
}
