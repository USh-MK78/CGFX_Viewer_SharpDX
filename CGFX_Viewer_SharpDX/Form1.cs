using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using HelixToolkit.Wpf.SharpDX.Shaders;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using CGFXLibrary;
using Color = System.Windows.Media.Color;
using Matrix = SharpDX.Matrix;
using Polygon = CGFXLibrary.Polygon;
using CGFX_Viewer_SharpDX.Component.Material;
using static CGFX_Viewer_SharpDX.Component.Data.TextureConbinerStage;
using CGFX_Viewer_SharpDX.Component;
using CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.MeshGeometry;
using CGFX_Viewer_SharpDX.MeshBuilderComponent.Mesh.GoemetryModel;
using CGFX_Viewer_SharpDX.MeshBuilderComponent.Node;
//using static CGFXLibrary.CGFXFormat.Transform;
using CGFXLibrary.CGFXSection;
using static CGFXLibrary.Flags;
using static CGFXLibrary.VertexAttribute;

namespace CGFX_Viewer_SharpDX
{
    public partial class Form1 : Form
    {
        //UserControl1.xamlの初期化
        //ここは作成時の名前にも影響されるので必ず確認すること
        public UserControl1 render = new UserControl1();

        public Form1()
        {
            InitializeComponent();
            render.Viewport3DX.EffectsManager = CGFXEffectManager.Create();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            elementHost1.Child = render;

            DirectionalLight3D directionalLight3D = new DirectionalLight3D
            {
                Direction = new System.Windows.Media.Media3D.Vector3D(-1, -1, -1),
                Color = Color.FromArgb(50, 255, 255, 255)
            };

            render.Viewport3DX.Items.Add(directionalLight3D);
            render.Viewport3DX.Items.Add(new AmbientLight3D() { Color = Color.FromArgb(255, 50, 50, 50) });

            LineGeometryModel3D GridModel = new LineGeometryModel3D
            {
                Name = "Grid",
                Thickness = 1.0,
                Smoothness = 2,
                Transform = new System.Windows.Media.Media3D.TranslateTransform3D(0, 0, 0),
                Geometry = LineBuilder.GenerateGrid(),
                Color = Color.FromArgb(255, 0, 255, 0)
            };

            render.Viewport3DX.Items.Add(GridModel);
        }

        CGFXFormat.CGFX CGFX { get; set; }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //elementHost1.Child = render;

            OpenFileDialog Open_CGFX = new OpenFileDialog()
            {
                Title = "Open(CGFX)",
                InitialDirectory = @"C:\Users\User\Desktop",
                Filter = "bcmdl file|*.bcmdl|All file|*.*"
            };

            if (Open_CGFX.ShowDialog() != DialogResult.OK) return;

            System.IO.FileStream fs1 = new FileStream(Open_CGFX.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br1 = new BinaryReader(fs1);

            CGFX = new CGFXFormat.CGFX();
            CGFX.ReadCGFX(br1);

            #region TreeView
            treeView1.HideSelection = false;
            List<string> SectionNameList = CGFXFormat.GetCGFXEntryNameArray().ToList();

            List<TreeNode> SectionNodeList = new List<TreeNode>();
            for (int i = 0; i < SectionNameList.Count; i++)
            {
                List<TreeNode> EntryNameList = new List<TreeNode>();
                foreach (var sw in CGFX.DICTAndSectionData.Keys.Where(x => x == (CGFXFormat.CGFXEntryData)Enum.Parse(typeof(CGFXFormat.CGFXEntryData), SectionNameList[i])).ToList())
                {
                    //var yt = CGFX.DICTAndSectionData[sw].DICT_Entries.Select(x => new TreeNode(x.Name)).ToList();
                    //EntryNameList.AddRange(yt.ToArray());

                    foreach (var r in CGFX.DICTAndSectionData[sw].DICT_Entries)
                    {
                        if (sw == CGFXFormat.CGFXEntryData.Models)
                        {
                            var lt = r.CGFXData.CMDLSection.meshDatas.Select(x => new TreeNode(x.SOBJData.SOBJ_Mesh_Section.Meshes.MeshName)).ToList();
                            var mt = r.CGFXData.CMDLSection.MTOB_DICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.MTOBSection.Name)).ToList();
                            var nt = r.CGFXData.CMDLSection.shapeDatas.Select((x, Id) => new { Id, x }).Select(x => new TreeNode(x.Id.ToString())).ToList();

                            //var mtName = r.CGFXData.CMDLSection.UnknownDICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set.Name)).ToList();
                            var mtName = r.CGFXData.CMDLSection.UnknownDICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.Name_Set.Name)).ToList();

                            TreeNode treeNode = new TreeNode(r.Name);
                            treeNode.Nodes.Add(new TreeNode("Mesh", lt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("Material", mt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("Shape", nt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("LinkedMaterial", mtName.ToArray()));

                            EntryNameList.Add(treeNode);
                        }
                        //else if (sw == "Textures")
                        //{
                        //    var TextureSection = r.CGFXData.CGFXSectionData.TXOBSection.TextureSection.Name;
                        //    //var MaterialSection = r.CGFXData.CGFXSectionData.TXOBSection.MaterialInfoSection.Name;

                        //    TreeNode treeNode = new TreeNode(r.Name);
                        //    treeNode.Nodes.Add(new TreeNode(TextureSection));
                        //    //treeNode.Nodes.Add(new TreeNode(MaterialSection));
                        //    EntryNameList.Add(treeNode);

                        //}
                        else
                        {
                            TreeNode treeNode = new TreeNode(r.Name);
                            EntryNameList.Add(treeNode);
                        }
                    }
                }

                TreeNode SectionNameNode = new TreeNode(SectionNameList[i], EntryNameList.ToArray());
                SectionNodeList.Add(SectionNameNode);
            }

            TreeNode RootNode = new TreeNode("CGFX_Root", SectionNodeList.ToArray());
            treeView1.Nodes.Add(RootNode);
            treeView1.TopNode.Expand();
            #endregion
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.PathSeparator = ",";

                string[] Set = treeView1.SelectedNode.FullPath.Split(',');

                if (Set.Length == 1)
                {
                    EndianConvert endianConvert = new EndianConvert(CGFX.BOM);
                    textBox1.Text += endianConvert.EndianCheck() + "\r\n";

                    textBox1.Text += "Revision : " + CGFX.Revision + "\r\n" + "\r\n";

                    foreach (var SectionCount in CGFX.Data.DictOffset_Dictionary)
                    {
                        textBox1.Text += SectionCount.Key + ":" + SectionCount.Value.NumOfEntries + "\r\n";
                    }
                }
                if (Set.Length == 2)
                {
                    System.Windows.MessageBox.Show("Selected " + Set[1]);
                }
                if (Set.Length == 3)
                {
                    CGFXFormat.CGFXEntryData EntryData = (CGFXFormat.CGFXEntryData)Enum.Parse(typeof(CGFXFormat.CGFXEntryData), Set[1]);
                    var CGFXData = CGFX.DICTAndSectionData[EntryData].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData;

                    if (EntryData == CGFXFormat.CGFXEntryData.Models)
                    {
                        var Models = CGFXData.CMDLSection;
                        propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        //MaterialDictionary
                        Dictionary<int, object> MaterialDictionary = new Dictionary<int, object>();
                        foreach (var MTOB in Models.MTOB_DICT.DICT_Entries.Select((value, i) => new { Value = value, Index = i }))
                        {
                            var DICTName = MTOB.Value.Name;
                            MaterialDictionary.Add(MTOB.Index, MTOB.Value.CGFXData.MTOBSection);
                        }

                        //Get Texture (Bitmap)
                        Dictionary<string, TXOB.Texture> CMDL_BitmapDictionary = new Dictionary<string, TXOB.Texture>();
                        foreach (var df in CGFX.DICTAndSectionData[CGFXFormat.CGFXEntryData.Textures].DICT_Entries)
                        {
                            string s = df.Name;

                            string TextureName = df.CGFXData.TXOBSection.TextureSection.Name;
                            CMDL_BitmapDictionary.Add(TextureName, df.CGFXData.TXOBSection.TextureSection);
                        }

                        foreach (var qs in Models.meshDatas)
                        {
                            //int MtlId = qs.SOBJData.Meshes.MaterialIndex;
                            int MtlId = qs.SOBJData.SOBJ_Mesh_Section.Meshes.MaterialIndex;
                            MTOB MTOB = MaterialDictionary[MtlId] as MTOB;

                            string MaterialName = MTOB.Name;
                            int textureUVTransformListCount = MTOB.TextureUVTransformSlotCount;
                            List<MTOB.TextureUVTransform> textureUVTransforms = MTOB.TextureUVTransformSettingList;
                            List<MTOB.MaterialInfoSet> MaterialInfoSetList = MTOB.GetMaterialInfoSet();
                            List<SHDR.TextureCombinerStage> TextureCombinerStages = MTOB.SHDRData.SHDR.TextureCombinerStages;
                            var f = MTOB.SHDRData.SHDR.TextureCombinerIsBufferFlag;

                            var MaterialColor = MTOB.MaterialColors;
                            var ConstantColor = MTOB.ConstantColorData;


                            int ShapeID = qs.SOBJData.SOBJ_Mesh_Section.Meshes.ShapeIndex;
                            var Shape = Models.shapeDatas[ShapeID].SOBJData.SOBJ_Shape_Section.Shapes;
                            var indexStreamCtrs = Shape.primitiveSets.Select(x => x.GetIndexStreamCtrPrimitive()).ToList();

                            foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                            {
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x02, 0x00, 0x00, 0x40 }))
                                {
                                    List<Color4> color4s = new List<Color4>();

                                    MeshBuilderComponent.Builder.CustomMeshBuilder mesh = new MeshBuilderComponent.Builder.CustomMeshBuilder(true, true, true, true, true);
                                    for (int q = 0; q < indexStreamCtrs[0][0].Count; q++) mesh.TriangleIndices.AddRange(indexStreamCtrs[0][0][q].FaceArray.ToArray());
                                    foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    {
                                        var Vertex = new Vector3(ym.GetScaledValue<float>(Polygon.DataType.Vt));
                                        var Normal = new Vector3(ym.GetScaledValue<float>(Polygon.DataType.Nr));
                                        var Tangent = ym.Tangent;
                                        var TexCoord = new Vector2(ym.GetScaledValue<float>(Polygon.DataType.TexCoord0));

                                        Vector2 t1 = ym.TexCoord1 != null ? new Vector2(ym.GetScaledValue<float>(Polygon.DataType.TexCoord1)) : new Vector2(0, 0);
                                        Vector2 t2 = ym.TexCoord2 != null ? new Vector2(ym.GetScaledValue<float>(Polygon.DataType.TexCoord2)) : new Vector2(0, 0);

                                        color4s.Add(new Color4(ym.ColorData.R, ym.ColorData.G, ym.ColorData.B, ym.ColorData.A));

                                        mesh.Add(Vertex, Normal, Tangent, new Vector3(0,0,1), TexCoord, t1, t2);
                                    }

                                    var cgfxMeshGeometry3D = mesh.ToCGFXMeshGeometry3D();
                                    cgfxMeshGeometry3D.Colors = new Color4Collection(color4s);
                                    cgfxMeshGeometry3D.UpdateColors();
                                    cgfxMeshGeometry3D.UpdateOctree();
                                    cgfxMeshGeometry3D.UpdateBounds();


                                    List<Data.TextureConbinerStage.TextureConbinerEquation> textureConbinerEquations = new List<Data.TextureConbinerStage.TextureConbinerEquation>();
                                    foreach (var Stage in TextureCombinerStages.Select((item, Index) => new {item, Index}))
                                    {
                                        bool IsBufferingColor = false;
                                        bool IsBufferingAlpha = false;
                                        if (Stage.Index == 2)
                                        {
                                            IsBufferingColor = f.IsCombinerColor0_Buffering;
                                            IsBufferingAlpha = f.IsCombinerAlpha0_Buffering;
                                        }
                                        else if (Stage.Index == 3)
                                        {
                                            IsBufferingColor = f.IsCombinerColor1_Buffering;
                                            IsBufferingAlpha = f.IsCombinerAlpha1_Buffering;
                                        }
                                        else if (Stage.Index == 4)
                                        {
                                            IsBufferingColor = f.IsCombinerColor2_Buffering;
                                            IsBufferingAlpha = f.IsCombinerAlpha2_Buffering;
                                        }
                                        else if (Stage.Index == 5)
                                        {
                                            IsBufferingColor = f.IsCombinerColor3_Buffering;
                                            IsBufferingAlpha = f.IsCombinerAlpha3_Buffering;
                                        }

                                        ConbinerSource SRC_COLOR_A = new ConbinerSource(Stage.item.SourceValue.Source_Color_A, Stage.item.OperandValue.Operand_Alpha_A, OPERANDTYPE.Color);
                                        ConbinerSource SRC_COLOR_B = new ConbinerSource(Stage.item.SourceValue.Source_Color_B, Stage.item.OperandValue.Operand_Alpha_B, OPERANDTYPE.Color);
                                        ConbinerSource SRC_COLOR_C = new ConbinerSource(Stage.item.SourceValue.Source_Color_C, Stage.item.OperandValue.Operand_Alpha_C, OPERANDTYPE.Color);
                                        var color = new Data.TextureConbinerStage.Equation(SRC_COLOR_A, SRC_COLOR_B, SRC_COLOR_C, Stage.item.ColorCombinerEquationType, Stage.item.ColorCombinerScaleFactorType, IsBufferingColor);

                                        ConbinerSource SRC_ALPHA_A = new ConbinerSource(Stage.item.SourceValue.Source_Color_A, Stage.item.OperandValue.Operand_Alpha_A, OPERANDTYPE.Alpha);
                                        ConbinerSource SRC_ALPHA_B = new ConbinerSource(Stage.item.SourceValue.Source_Color_B, Stage.item.OperandValue.Operand_Alpha_B, OPERANDTYPE.Alpha);
                                        ConbinerSource SRC_ALPHA_C = new ConbinerSource(Stage.item.SourceValue.Source_Color_C, Stage.item.OperandValue.Operand_Alpha_C, OPERANDTYPE.Alpha);
                                        var alpha = new Data.TextureConbinerStage.Equation(SRC_ALPHA_A, SRC_ALPHA_B, SRC_ALPHA_C, Stage.item.AlphaCombinerEquationType, Stage.item.AlphaCombinerScaleFactorType, IsBufferingAlpha);

                                        textureConbinerEquations.Add(new Data.TextureConbinerStage.TextureConbinerEquation(color, alpha));
                                    }

                                    var ft1 = textureConbinerEquations[0].ColorEquation.SOURCE_A.GetOPERAND<Data.TextureConbinerStage.COLOR_OPERAND>();
                                    var ft2 = textureConbinerEquations[0].AlphaEquation.SOURCE_A.GetOPERAND<Data.TextureConbinerStage.ALPHA_OPERAND>();

                                    //Data.Blending.ColorBlendMode colorBlendMode = new Data.Blending.ColorBlendMode(Data.Blending.BLEND_ELEMENT.FRAMEBUFFER_OUT_RGB, Data.Blending.BLEND_ELEMENT.COMBINER_OUT_RGB);
                                    //Data.Blending.AlphaBlendMode alphaBlendMode = new Data.Blending.AlphaBlendMode(Data.Blending.BLEND_ELEMENT.FRAMEBUFFER_OUT_A, Data.Blending.BLEND_ELEMENT.COMBINER_OUT_RGB);
                                    //Data.Blending.BlendEquation blendEquation = new Data.Blending.BlendEquation(colorBlendMode, Data.Blending.BlendEquationType.SRC_ADD_DEST, alphaBlendMode, Data.Blending.BlendEquationType.SRC_ADD_DEST);
                                    //Data.Blending blending = new Data.Blending(Data.Blending.BLENDMODE.SEPARATE, blendEquation, null);

                                    Data.Blending blending = new Data.Blending(Data.Blending.BLENDMODE.NONE, null, null);

                                    Data.ColorProperty.ConstantColor constantColor = new Data.ColorProperty.ConstantColor(ConstantColor.Constant0Data.GetColor4(),
                                                                                                                          ConstantColor.Constant1Data.GetColor4(),
                                                                                                                          ConstantColor.Constant2Data.GetColor4(),
                                                                                                                          ConstantColor.Constant3Data.GetColor4(),
                                                                                                                          ConstantColor.Constant4Data.GetColor4(),
                                                                                                                          ConstantColor.Constant5Data.GetColor4());


                                    Dictionary<int, Data.Texture.TextureModelSlot> textureModels = new Dictionary<int, Data.Texture.TextureModelSlot>();
                                    for (int Count = 0; Count < MaterialInfoSetList.Count; Count++)
                                    {
                                        if (MaterialInfoSetList[Count].TXOBMaterialDataSection.TXOB_MaterialSection != null)
                                        {
                                            var MatName = MaterialInfoSetList[Count].TXOBMaterialDataSection.TXOB_MaterialSection.MaterialInfoSection.MTOB_MaterialName;
                                            if (MatName != null)
                                            {
                                                if (CMDL_BitmapDictionary[MatName].TXOB_Bitmap != null)
                                                {
                                                    //Create Texture
                                                    HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture texture = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                                    textureModels.Add(Count, new Data.Texture.TextureModelSlot(Count, MatName, texture.ToTextureModel(true), Data.Texture.TextureType.Diffuse, Data.Texture.AlphaMap.FROM_TEXTURE, true));
                                                }
                                            }
                                        }


                                    }

                                    #region Backup
                                    //int c = 0;
                                    //Dictionary<int, Data.Texture.TextureModelSlot> textureModels = new Dictionary<int, Data.Texture.TextureModelSlot>();
                                    //foreach (var i in MaterialInfoSetList)
                                    //{
                                    //    var MatName = i.TXOBDataSection.TXOB.MaterialInfoSection.MTOB_MaterialName;

                                    //    if (MatName != null)
                                    //    {
                                    //        if (CMDL_BitmapDictionary[MatName].TXOB_Bitmap != null)
                                    //        {
                                    //            //Create Texture
                                    //            HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture texture = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                    //            //UVTransform UV = Misc.GetUVTransform(unknownDataArea.CalculateTextureCoordinateTypeValue);

                                    //            //Dictionary<int, Data.Texture.TextureModelSlot> textureModels = new Dictionary<int, Data.Texture.TextureModelSlot>();
                                    //            textureModels.Add(c, new Data.Texture.TextureModelSlot(c, MatName, texture.ToTextureModel(true), Data.Texture.TextureType.Diffuse, Data.Texture.AlphaMap.FROM_TEXTURE));
                                    //            c++;
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    //UVTransform UV = new UVTransform();
                                    //if (textureUVTransforms.CalculateTextureCoordinateTypeValue == 0) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(0)), new Vector2(-1, -1), new Vector2(0, 0)); //Default(?)
                                    //else if (textureUVTransforms.CalculateTextureCoordinateTypeValue == 1) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(0)), new Vector2(-1, -1), new Vector2(0, 0));
                                    //else if (textureUVTransforms.CalculateTextureCoordinateTypeValue == 2) UV = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
                                    //else if (textureUVTransforms.CalculateTextureCoordinateTypeValue == 3) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(90)), new Vector2(-1, -1), new Vector2(0, 0));

                                    //UVTransform UV = Misc.GetUVTransform(unknownDataArea.CalculateTextureCoordinateTypeValue);

                                    UVTransform[] UVTransformArray = new UVTransform[3];
                                    for (int i = 0; i < textureUVTransformListCount; i++)
                                    {
                                        float Rotate = textureUVTransforms[i].Rotate;
                                        float ScaleU = textureUVTransforms[i].ScaleU;
                                        float ScaleV = textureUVTransforms[i].ScaleV;
                                        float TranslateU = textureUVTransforms[i].TranslateU;
                                        float TranslateV = textureUVTransforms[i].TranslateV;

                                        UVTransformArray[i] = new UVTransform(Rotate, new Vector2(ScaleU, ScaleV), new Vector2(TranslateU, TranslateV));
                                    }

                                    List<UVTransform> uVTransforms = UVTransformArray.ToList();

                                    var BaseColor = new Data.ColorProperty.Base(MaterialColor.AmbientData.GetColor4(), MaterialColor.DiffuseData.GetColor4(), MaterialColor.EmissionData.GetColor4(), MaterialColor.Speculer0Data.GetColor4(), MaterialColor.Speculer1Data.GetColor4());
                                    CGFXMaterial cgfxMaterial = new CGFXMaterial { TextureSlotList = textureModels, TextureConbinerEquationList = textureConbinerEquations, ConstantColor = constantColor, Blending = blending, IsFragmentLighting = true, UVTransform = uVTransforms, BaseColor = BaseColor };
                                    CGFXMeshGeometryModel3D bufferMeshGeometryModel3D = new CGFXMeshGeometryModel3D { Geometry = cgfxMeshGeometry3D, Material = cgfxMaterial, WireframeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF), RenderWireframe = true };

                                    //render.Viewport3DX.Items.Add(bufferMeshGeometryModel3D);

                                    CGFXMeshNode bufferMeshNode = (CGFXMeshNode)bufferMeshGeometryModel3D;
                                    render.Viewport3DX.Items.Add(bufferMeshNode);
                                }
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }))
                                {

                                }
                            }
                        }
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Textures)
                    {
                        var Textures = CGFXData.TXOBSection.TextureSection;
                        pictureBox1.Image = Textures.TXOB_Bitmap;
                        propertyGrid2.SelectedObject = new CGFXPropertyGridSet.TXOB_PropertyGrid(Textures);
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.LookupTables)
                    {
                        var LUTS = CGFXData.LUTSSection;
                        //LUTS_PropertyGrid.SelectedObject = ht.LUTSSection;
                        LUTS_PropertyGrid.SelectedObject = new CGFXPropertyGridSet.LUTS_PropertyGrid(LUTS);
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Materials) return;
                    if (EntryData == CGFXFormat.CGFXEntryData.Shaders) return;
                    if (EntryData == CGFXFormat.CGFXEntryData.Cameras)
                    {
                        //Camera
                        CCAM_PropertyGrid.SelectedObject = new CGFXPropertyGridSet.CCAM_PropertyGrid(CGFXData.CCAMSection);
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Lights)
                    {
                        if (CGFXData.Flag.GetFlags().HasFlag(CGFXIdentFlag.F6))
                        {
                            if ((CGFXData.Flag.GetIdentFlagUInt() & 0x0000FF00) == 0)
                            {
                                //Fragment
                                propertyGrid6.SelectedObject = CGFXData.CFLTSection;
                            }
                            else if ((CGFXData.Flag.GetIdentFlagUInt() & 0x0000FF00) != 0)
                            {
                                if (CGFXData.Flag.GetFlags().HasFlag(CGFXIdentFlag.F9))
                                {
                                    //Hemisphere
                                    propertyGrid6.SelectedObject = CGFXData.CHLTSection;

                                }
                                else if (CGFXData.Flag.GetFlags().HasFlag(CGFXIdentFlag.F10))
                                {
                                    //VertexLight
                                    propertyGrid6.SelectedObject = CGFXData.CVLTSection;
                                }
                                else if (CGFXData.Flag.GetFlags().HasFlag(CGFXIdentFlag.F11))
                                {
                                    //AmbientLight
                                    propertyGrid6.SelectedObject = CGFXData.CALTSection;
                                }
                            }
                        }
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Fogs)
                    {
                        var fogs = CGFXData.CFOGSection;
                        propertyGrid1.SelectedObject = new CGFXPropertyGridSet.CFOG_PropertyGrid(fogs);
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Environments)
                    {
                        var Envs = CGFXData.CENVSection;
                        propertyGrid7.SelectedObject = new CGFXPropertyGridSet.CENV_PropertyGrid(Envs);
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Texture_Animations)
                    {
                        var TexAnim = CGFXData.CANMSection;
                        propertyGrid8.SelectedObject = TexAnim;
                    }
                    if (EntryData == CGFXFormat.CGFXEntryData.Emitters)
                    {
                        var Emitter = CGFXData.PEMTSection;
                        propertyGrid9.SelectedObject = Emitter;
                    }
                }
                if (Set.Length == 4)
                {
                    CGFXFormat.CGFXEntryData EntryData = (CGFXFormat.CGFXEntryData)Enum.Parse(typeof(CGFXFormat.CGFXEntryData), Set[1]);
                    var ht = CGFX.DICTAndSectionData[EntryData].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData;
                }
                if (Set.Length == 5)
                {
                    CGFXFormat.CGFXEntryData EntryData = (CGFXFormat.CGFXEntryData)Enum.Parse(typeof(CGFXFormat.CGFXEntryData), Set[1]);
                    var ht = CGFX.DICTAndSectionData[EntryData].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData;

                    if (EntryData == CGFXFormat.CGFXEntryData.Models)
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = null;
                        if (Set[3] == "Mesh")
                        {
                            propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_MeshData_PropertyGrid(Models.meshDatas.Find(x => x.SOBJData.SOBJ_Mesh_Section.Meshes.MeshName == Set[4]));
                        }
                        if (Set[3] == "Material")
                        {
                            propertyGrid4.SelectedObject = new CGFXPropertyGridSet.MTOB_PropertyGrid(Models.MTOB_DICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.MTOBSection);
                        }
                        if (Set[3] == "Shape")
                        {
                            propertyGrid3.SelectedObject = new PropertyGridForms.Section.CMDL.ShapeData.ShapeData_PropertyGrid(Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.SOBJ_Shape_Section.Shapes);
                            //var i = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes.VertexAttributes.Select(x => x.Streams.PolygonList).ToList();
                        }
                        if (Set[3] == "LinkedMaterial")
                        {
                            //propertyGrid3.SelectedObject = Models.UnknownDICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set;
                            propertyGrid3.SelectedObject = Models.UnknownDICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.Name_Set;
                        }
                    }
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CGFX = null;
            render.Viewport3DX.Items.Clear();

            DirectionalLight3D directionalLight3D = new DirectionalLight3D
            {
                Direction = new System.Windows.Media.Media3D.Vector3D(-1, -1, -1),
                Color = Color.FromArgb(50, 255, 255, 255)
            };

            render.Viewport3DX.Items.Add(directionalLight3D);
            render.Viewport3DX.Items.Add(new AmbientLight3D() { Color = Color.FromArgb(255, 50, 50, 50) });

            LineGeometryModel3D GridModel = new LineGeometryModel3D
            {
                Name = "Grid",
                Thickness = 1.0,
                Smoothness = 2,
                Transform = new System.Windows.Media.Media3D.TranslateTransform3D(0, 0, 0),
                Geometry = LineBuilder.GenerateGrid(),
                Color = Color.FromArgb(255, 0, 255, 0)
            };

            render.Viewport3DX.Items.Add(GridModel);
            render.Viewport3DX.UpdateLayout();

            propertyGrid1.SelectedObject = null;
            propertyGrid2.SelectedObject = null;
            propertyGrid3.SelectedObject = null;
            propertyGrid4.SelectedObject = null;
            propertyGrid5.SelectedObject = null;

            propertyGrid6.SelectedObject = null;
            propertyGrid7.SelectedObject = null;
            propertyGrid8.SelectedObject = null;

            treeView1.Nodes.Clear();
        }
    }
}
