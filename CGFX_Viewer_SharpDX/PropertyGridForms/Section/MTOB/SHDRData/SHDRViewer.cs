using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.MTOB.SHDRData
{
    public partial class SHDRViewer : Form
    {
        public SHDR SHDR { get; set; }

        public SHDRViewer(SHDR SHDR)
        {
            InitializeComponent();
            this.SHDR = SHDR;

            //Stage
            for (int i = 0; i < 6; i++)
            {
                StageN_Select.Items.Add("Stage_" + i);
            }

            CombinerEquation_Color_CMBox.DataSource = EquationList();
            COLOR_ScaleF_CMBox.DataSource = ScaleF_List();
            COLOR_SRC_A_CMBox.DataSource = SOURCE_List();
            COLOR_SRC_B_CMBox.DataSource = SOURCE_List();
            COLOR_SRC_C_CMBox.DataSource = SOURCE_List();
            COLOR_SRC_A_OP_CMBox.DataSource = COLOR_OPERAND_List();
            COLOR_SRC_B_OP_CMBox.DataSource = COLOR_OPERAND_List();
            COLOR_SRC_C_OP_CMBox.DataSource = COLOR_OPERAND_List();

            CombinerEquation_Alpha_CMBox.DataSource = EquationList();
            ALPHA_ScaleF_CMBox.DataSource = ScaleF_List();
            ALPHA_SRC_A_CMBox.DataSource = SOURCE_List();
            ALPHA_SRC_B_CMBox.DataSource = SOURCE_List();
            ALPHA_SRC_C_CMBox.DataSource = SOURCE_List();
            ALPHA_SRC_A_OP_CMBox.DataSource = ALPHA_OPERAND_List();
            ALPHA_SRC_B_OP_CMBox.DataSource = ALPHA_OPERAND_List();
            ALPHA_SRC_C_OP_CMBox.DataSource = ALPHA_OPERAND_List();
        }

        public class CombinerStageDataSource
        {
            public int Index;

            public Element Data;
            public class Element
            {
                public string Name;
                public byte Value;

                public Element(string Name, byte Value)
                {
                    this.Name = Name;
                    this.Value = Value;
                }
            }

            public CombinerStageDataSource(int Index, Element Data)
            {
                this.Index = Index;
                this.Data = Data;
            }

            public override string ToString()
            {
                return Data.Name;
            }
        }

        public List<CombinerStageDataSource> EquationList()
        {
            List<CombinerStageDataSource> combinerEquations = new List<CombinerStageDataSource>();
            combinerEquations.Add(new CombinerStageDataSource(0, new CombinerStageDataSource.Element("A", 0)));
            combinerEquations.Add(new CombinerStageDataSource(1, new CombinerStageDataSource.Element("A x B", 1)));
            combinerEquations.Add(new CombinerStageDataSource(2, new CombinerStageDataSource.Element("A + B", 2)));
            combinerEquations.Add(new CombinerStageDataSource(3, new CombinerStageDataSource.Element("A + B - 0.5", 3)));
            combinerEquations.Add(new CombinerStageDataSource(4, new CombinerStageDataSource.Element("A * C + B * (1 - C)", 4)));
            combinerEquations.Add(new CombinerStageDataSource(5, new CombinerStageDataSource.Element("A - B", 5)));
            combinerEquations.Add(new CombinerStageDataSource(6, new CombinerStageDataSource.Element("RGB <= DOT(A, B)", 6)));
            combinerEquations.Add(new CombinerStageDataSource(7, new CombinerStageDataSource.Element("RGBBA <= DOT(A, B)", 7)));
            combinerEquations.Add(new CombinerStageDataSource(8, new CombinerStageDataSource.Element("(A + B) * C", 8)));
            combinerEquations.Add(new CombinerStageDataSource(9, new CombinerStageDataSource.Element("(A * B) + C", 9)));
            return combinerEquations;
        }

        public List<CombinerStageDataSource> SOURCE_List()
        {
            List<CombinerStageDataSource> SOURCE_DATA_List = new List<CombinerStageDataSource>();
            SOURCE_DATA_List.Add(new CombinerStageDataSource(0, new CombinerStageDataSource.Element("VERTEX_SHADER_OUT", 0)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(1, new CombinerStageDataSource.Element("COLOR_PRIMARY", 1)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(2, new CombinerStageDataSource.Element("COLOR_SECONDARY", 2)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(3, new CombinerStageDataSource.Element("TEXTURE0", 3)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(4, new CombinerStageDataSource.Element("TEXTURE1", 4)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(5, new CombinerStageDataSource.Element("TEXTURE2", 5)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(6, new CombinerStageDataSource.Element("TEXTURE3", 6)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(7, new CombinerStageDataSource.Element("PREVIOUS_STEP_BUFFER", 13))); //1段目には表示しない
            SOURCE_DATA_List.Add(new CombinerStageDataSource(8, new CombinerStageDataSource.Element("CONSTANT_COLOR", 14)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(9, new CombinerStageDataSource.Element("PREVIOUS_STEP_RESULT", 15))); //1段目には表示しない
            return SOURCE_DATA_List;
        }

        public List<CombinerStageDataSource> COLOR_OPERAND_List()
        {
            List<CombinerStageDataSource> SOURCE_DATA_List = new List<CombinerStageDataSource>();
            SOURCE_DATA_List.Add(new CombinerStageDataSource(0, new CombinerStageDataSource.Element("RGB", 0)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(1, new CombinerStageDataSource.Element("1 - RGB", 1)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(2, new CombinerStageDataSource.Element("A", 2)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(3, new CombinerStageDataSource.Element("1 - A", 3)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(4, new CombinerStageDataSource.Element("R", 4)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(5, new CombinerStageDataSource.Element("1 - R", 5)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(6, new CombinerStageDataSource.Element("G", 6)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(7, new CombinerStageDataSource.Element("1 - G", 7)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(8, new CombinerStageDataSource.Element("B", 8)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(9, new CombinerStageDataSource.Element("1 - B", 9)));
            return SOURCE_DATA_List;
        }

        public List<CombinerStageDataSource> ALPHA_OPERAND_List()
        {
            List<CombinerStageDataSource> SOURCE_DATA_List = new List<CombinerStageDataSource>();
            SOURCE_DATA_List.Add(new CombinerStageDataSource(0, new CombinerStageDataSource.Element("A", 0)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(1, new CombinerStageDataSource.Element("1 - A", 1)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(2, new CombinerStageDataSource.Element("R", 2)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(3, new CombinerStageDataSource.Element("1 - R", 3)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(4, new CombinerStageDataSource.Element("G", 4)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(5, new CombinerStageDataSource.Element("1 - G", 5)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(6, new CombinerStageDataSource.Element("B", 6)));
            SOURCE_DATA_List.Add(new CombinerStageDataSource(7, new CombinerStageDataSource.Element("1 - B", 7)));
            return SOURCE_DATA_List;
        }

        public List<CombinerStageDataSource> ScaleF_List()
        {
            List<CombinerStageDataSource> ScaleF_List = new List<CombinerStageDataSource>();
            ScaleF_List.Add(new CombinerStageDataSource(0, new CombinerStageDataSource.Element("x1", 0)));
            ScaleF_List.Add(new CombinerStageDataSource(1, new CombinerStageDataSource.Element("x2", 1)));
            ScaleF_List.Add(new CombinerStageDataSource(2, new CombinerStageDataSource.Element("x4", 2)));
            return ScaleF_List;
        }

        public Dictionary<int, bool> TextureCombinerColorBufferFlagDict { get; set; }
        public Dictionary<int, bool> TextureCombinerAlphaBufferFlagDict { get; set; }

        private void SHDRViewer_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " [" + SHDR.Name.Replace("\0", "") + "] [" + SHDR.VertexShaderName.Replace("\0", "") + "]";

            var f = SHDR.TextureCombinerIsBufferFlag;
            Dictionary<int, bool> TextureCombinerColorBufferFlagDict = new Dictionary<int, bool>();
            TextureCombinerColorBufferFlagDict.Add(1, f.IsCombinerColor0_Buffering);
            TextureCombinerColorBufferFlagDict.Add(2, f.IsCombinerColor1_Buffering);
            TextureCombinerColorBufferFlagDict.Add(3, f.IsCombinerColor2_Buffering);
            TextureCombinerColorBufferFlagDict.Add(4, f.IsCombinerColor3_Buffering);

            Dictionary<int, bool> TextureCombinerAlphaBufferFlagDict = new Dictionary<int, bool>();
            TextureCombinerAlphaBufferFlagDict.Add(1, f.IsCombinerAlpha0_Buffering);
            TextureCombinerAlphaBufferFlagDict.Add(2, f.IsCombinerAlpha1_Buffering);
            TextureCombinerAlphaBufferFlagDict.Add(3, f.IsCombinerAlpha2_Buffering);
            TextureCombinerAlphaBufferFlagDict.Add(4, f.IsCombinerAlpha3_Buffering);

            this.TextureCombinerColorBufferFlagDict = TextureCombinerColorBufferFlagDict;
            this.TextureCombinerAlphaBufferFlagDict = TextureCombinerAlphaBufferFlagDict;

            IsColorStageBuffering.Enabled = false;
            IsAlphaStageBuffering.Enabled = false;


            SHDRViewer_PGS_Main.SelectedObject = new SHDRViewer_PropertyGrid(SHDR);
            StageN_Select.SelectedIndex = 0;

            //propertyGrid1.SelectedObject = SHDR.TextureCombinerStages[StageN_Select.SelectedIndex];
            propertyGrid1.SelectedObject = new CombinerStagePGSObject(SHDR.TextureCombinerStages[StageN_Select.SelectedIndex]);

        }

        private void StageN_Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            CombinerEquation_Color_CMBox.SelectedIndex = EquationList().Find(x => x.Data.Value == (int)SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].ColorCombinerEquationFlag).Index;
            COLOR_ScaleF_CMBox.SelectedIndex = ScaleF_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].ColorCombinerScaleFactorType).Index;
            COLOR_SRC_A_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Color_A).Index;
            COLOR_SRC_B_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Color_B).Index;
            COLOR_SRC_C_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Color_C).Index;
            COLOR_SRC_A_OP_CMBox.SelectedIndex = COLOR_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Color_A).Index;
            COLOR_SRC_B_OP_CMBox.SelectedIndex = COLOR_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Color_B).Index;
            COLOR_SRC_C_OP_CMBox.SelectedIndex = COLOR_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Color_C).Index;

            if (StageN_Select.SelectedIndex > 0 && StageN_Select.SelectedIndex < 5)
            {
                IsColorStageBuffering.Enabled = true;
                IsColorStageBuffering.Checked = TextureCombinerColorBufferFlagDict[StageN_Select.SelectedIndex];
            }
            else if (StageN_Select.SelectedIndex == 0 || StageN_Select.SelectedIndex == 5)
            {
                IsColorStageBuffering.Enabled = false;
                IsColorStageBuffering.Checked = false;
                
            }

            //IsColorStageBuffering.Checked = TextureCombinerColorBufferFlagDict[StageN_Select.SelectedIndex];

            CombinerEquation_Alpha_CMBox.SelectedIndex = EquationList().Find(x => x.Data.Value == (int)SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].AlphaCombinerEquationFlag).Index;
            ALPHA_ScaleF_CMBox.SelectedIndex = ScaleF_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].AlphaCombinerScaleFactorType).Index;
            ALPHA_SRC_A_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Alpha_A).Index;
            ALPHA_SRC_B_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Alpha_B).Index;
            ALPHA_SRC_C_CMBox.SelectedIndex = SOURCE_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].SourceValue.Source_Alpha_C).Index;
            ALPHA_SRC_A_OP_CMBox.SelectedIndex = ALPHA_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Alpha_A).Index;
            ALPHA_SRC_B_OP_CMBox.SelectedIndex = ALPHA_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Alpha_B).Index;
            ALPHA_SRC_C_OP_CMBox.SelectedIndex = ALPHA_OPERAND_List().Find(x => x.Data.Value == SHDR.TextureCombinerStages[StageN_Select.SelectedIndex].OperandValue.Operand_Alpha_C).Index;

            if (StageN_Select.SelectedIndex > 0 && StageN_Select.SelectedIndex < 5)
            {
                IsAlphaStageBuffering.Enabled = true;
                IsAlphaStageBuffering.Checked = TextureCombinerAlphaBufferFlagDict[StageN_Select.SelectedIndex];
            }
            else if (StageN_Select.SelectedIndex == 0 || StageN_Select.SelectedIndex == 5)
            {
                IsAlphaStageBuffering.Enabled = false;
                IsAlphaStageBuffering.Checked = false;
            }

            //propertyGrid1.SelectedObject = SHDR.TextureCombinerStages[StageN_Select.SelectedIndex];
            propertyGrid1.SelectedObject = new CombinerStagePGSObject(SHDR.TextureCombinerStages[StageN_Select.SelectedIndex]);
        }
    }
}
