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
using static CGFX_Viewer_SharpDX.CGFXFormat.SOBJ.Shape.VertexAttribute;
using static CGFX_Viewer_SharpDX.CGFXFormat.Transform;
using static CGFX_Viewer_SharpDX.VertexAttribute;
using Color = System.Windows.Media.Color;
using Matrix = SharpDX.Matrix;
//using static CGFX_Viewer.HTK_3DES.TSRSystem.TSRSystem3D;

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
            List<string> SectionNameList = new List<string>(CGFX.Data.DictOffset_Dictionary.Keys);

            List<TreeNode> SectionNodeList = new List<TreeNode>();
            for (int i = 0; i < SectionNameList.Count; i++)
            {
                List<TreeNode> EntryNameList = new List<TreeNode>();
                foreach (var sw in CGFX.DICTAndSectionData.Keys.Where(x => x == SectionNameList[i]).ToList())
                {
                    //var yt = CGFX.DICTAndSectionData[sw].DICT_Entries.Select(x => new TreeNode(x.Name)).ToList();
                    //EntryNameList.AddRange(yt.ToArray());

                    foreach (var r in CGFX.DICTAndSectionData[sw].DICT_Entries)
                    {
                        if (sw == "Model")
                        {
                            var lt = r.CGFXData.CGFXSectionData.CMDLSection.meshDatas.Select(x => new TreeNode(x.SOBJData.Meshes.MeshName)).ToList();
                            var mt = r.CGFXData.CGFXSectionData.CMDLSection.MTOB_DICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.CGFXSectionData.MTOBSection.Name)).ToList();
                            //var nt = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select(x => new TreeNode(x.SOBJData.Shapes.Name)).ToList();
                            var nt = r.CGFXData.CGFXSectionData.CMDLSection.shapeDatas.Select((x, Id) => new { Id, x }).Select(x => new TreeNode(x.Id.ToString())).ToList();

                            var mtName = r.CGFXData.CGFXSectionData.CMDLSection.UnknownDICT.DICT_Entries.Select(x => new TreeNode(x.CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set.Name)).ToList();

                            TreeNode treeNode = new TreeNode(r.Name);
                            treeNode.Nodes.Add(new TreeNode("Mesh", lt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("Material", mt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("Shape", nt.ToArray()));

                            treeNode.Nodes.Add(new TreeNode("LinkedMaterial", mtName.ToArray()));

                            EntryNameList.Add(treeNode);
                        }
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
                    // "Model", "Textures", "LUTS", "Materials", "Shaders", "Cameras", "Lights", "Fog", "Environments", "Skeleton_Animations", "Texture_Animations", "Visibility_Animations", "Camera_Animations", "Light_Animations", "Fog_Animations", "Emitters" 

                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;

                    if (Set[1] == "Model")
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        //Get TextureName
                        Dictionary<int, ArrayList> MaterialDictionary = new Dictionary<int, ArrayList>();
                        foreach (var MTOB in Models.MTOB_DICT.DICT_Entries.Select((value, i) => new { Value = value, Index = i }))
                        {
                            var DICTName = MTOB.Value.Name;

                            var MTOBSectionData = MTOB.Value.CGFXData.CGFXSectionData.MTOBSection;
                            ArrayList arrayList = new ArrayList();
                            arrayList.AddRange(new object[] { MTOBSectionData.Name, MTOBSectionData.UnknownDataAreas, MTOBSectionData.GetMaterialInfoSet() });
                            MaterialDictionary.Add(MTOB.Index, arrayList);
                        }

                        //Get Texture (Bitmap)
                        Dictionary<string, CGFXFormat.CGFXSection.TXOB.Texture> CMDL_BitmapDictionary = new Dictionary<string, CGFXFormat.CGFXSection.TXOB.Texture>();
                        foreach (var df in CGFX.DICTAndSectionData["Textures"].DICT_Entries)
                        {
                            string s = df.Name;

                            string TextureName = df.CGFXData.CGFXSectionData.TXOBSection.TextureSection.Name;
                            CMDL_BitmapDictionary.Add(TextureName, df.CGFXData.CGFXSectionData.TXOBSection.TextureSection);
                        }

                        foreach (var qs in Models.meshDatas)
                        {
                            int MtlId = qs.SOBJData.Meshes.MaterialIndex;

                            string MaterialName = (string)MaterialDictionary[MtlId][0];
                            CGFXFormat.CGFXSection.MTOB.UnknownDataArea unknownDataArea = (CGFXFormat.CGFXSection.MTOB.UnknownDataArea)MaterialDictionary[MtlId][1];
                            List<CGFXFormat.CGFXSection.MTOB.MaterialInfoSet> MaterialInfoSetList = MaterialDictionary[MtlId][2] as List<CGFXFormat.CGFXSection.MTOB.MaterialInfoSet>;

                            int ShapeID = qs.SOBJData.Meshes.ShapeIndex;
                            var Shape = Models.shapeDatas[ShapeID].SOBJData.Shapes;
                            var indexStreamCtrs = Shape.primitiveSets.Select(x => x.GetIndexStreamCtrPrimitive()).ToList();

                            foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                            {
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x02, 0x00, 0x00, 0x40 }))
                                {
                                    List<Color4> color4s = new List<Color4>();

                                    HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Mesh mesh = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Mesh(true, true, true);
                                    for (int q = 0; q < indexStreamCtrs[0][0].Count; q++) mesh.AddTriangleIndicesArray(indexStreamCtrs[0][0][q].FaceArray.ToArray());
                                    foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    {
                                        var Vertex = ym.Scaled<Vector3>(Polygon.DataType.Vt);
                                        var Normal = ym.Scaled<Vector3>(Polygon.DataType.Nr);
                                        var Tangent = ym.Tangent;
                                        var TexCoord = ym.Scaled<Vector2>(Polygon.DataType.TexCoord0);


                                        color4s.Add(ym.ColorData.ToColor4());

                                        mesh.Add(Vertex, Normal, Tangent, TexCoord);



                                        //if ((mesh.MeshBuilder.Positions.Contains(Vertex) && mesh.MeshBuilder.Normals.Contains(Normal) && mesh.MeshBuilder.TextureCoordinates.Contains(TexCoord)) == false)
                                        //{
                                        //    mesh.MeshBuilder.Tangents.Add(new Vector3(1, 0, 0));
                                        //    mesh.MeshBuilder.BiTangents.Add(new Vector3(0, 0, -1));
                                        //}
                                    }

                                    HelixToolkit.Wpf.SharpDX.MeshGeometry3D meshGeometry3D = mesh.ToMeshGeometry3D();
                                    meshGeometry3D.Colors = new Color4Collection(color4s);
                                    meshGeometry3D.UpdateColors();
                                    meshGeometry3D.UpdateOctree();
                                    meshGeometry3D.UpdateBounds();

                                    #region Test
                                    //var s = Matrix.Scaling(1, 1, 1).ToMatrix3D();
                                    //var rX = Matrix.RotationX(0).ToMatrix3D();
                                    //var rY = Matrix.RotationY(0).ToMatrix3D();
                                    //var rZ = Matrix.RotationZ(0).ToMatrix3D();
                                    //var t = Matrix.Translation(0, 0, 0).ToMatrix3D();

                                    //Matrix3D matrix3D = new Matrix3D();
                                    //matrix3D.Append(s);
                                    //matrix3D.Append(rX);
                                    //matrix3D.Append(rY);
                                    //matrix3D.Append(rZ);
                                    //matrix3D.Append(t);

                                    //var transform = matrix3D.ToMatrix();


                                    //MeshNode groupModel3D = new MeshNode { Geometry = meshGeometry3D, Material = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial { DiffuseColor = new Color4(255, 0, 255, 0) }, CullMode = SharpDX.Direct3D11.CullMode.None, ModelMatrix = transform };

                                    ////表示
                                    //render.Viewport3DX.Items.Add((SceneNode)groupModel3D);
                                    #endregion




                                    List<HelixToolkit.Wpf.SharpDX.Material> MaterialList = new List<HelixToolkit.Wpf.SharpDX.Material>();
                                    if (meshGeometry3D.Colors.Count != 0)
                                    {
                                        HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                        {
                                            DiffuseMap = null,
                                            //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                            UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                            EnableUnLit = false,
                                            VertexColorBlendingFactor = 1,
                                            Name = "VertColor"
                                        };

                                        MaterialList.Add(diffuseMaterial);
                                    }

                                    foreach (var i in MaterialInfoSetList)
                                    {
                                        var MatName = i.TXOBDataSection.TXOB.MaterialInfoSection.MTOB_MaterialName;

                                        if (MatName != null)
                                        {
                                            if (CMDL_BitmapDictionary[MatName].TXOB_Bitmap != null)
                                            {
                                                //Create Texture
                                                HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture texture = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                                UVTransform UV = Misc.GetUVTransform(unknownDataArea.CalculateTextureCoordinateTypeValue);

                                                if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.ImageFormat.LA8)
                                                {
                                                    //L8, LA8
                                                    HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                                    phongMaterial.UVTransform = UV;
                                                    phongMaterial.SpecularColorMap = texture.ToTextureModel(false);
                                                    phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                                    phongMaterial.SpecularShininess = 0.5f;
                                                    phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                                    MaterialList.Add(phongMaterial);
                                                }
                                                else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.ImageFormat.HILO8)
                                                {
                                                    HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                                    phongMaterial.UVTransform = UV;
                                                    phongMaterial.NormalMap = texture.ToTextureModel(false);
                                                    phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                                    phongMaterial.SpecularShininess = 0.5f;
                                                    phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                                    MaterialList.Add(phongMaterial);
                                                }
                                                else
                                                {
                                                    HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                                    {
                                                        DiffuseMap = texture.ToTextureModel(false),
                                                        //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                                        UVTransform = UV,
                                                        EnableUnLit = true,
                                                        VertexColorBlendingFactor = 0
                                                    };

                                                    diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                                    MaterialList.Add(diffuseMaterial);
                                                }

                                                #region BackUp
                                                //else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1A4 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.RGB565)
                                                //{
                                                //    //HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                                //    //{
                                                //    //    DiffuseMap = texture.ToTextureModel(false),
                                                //    //    //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                                //    //    UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                                //    //    EnableUnLit = true,
                                                //    //    VertexColorBlendingFactor = 0
                                                //    //};

                                                //    //diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                                //    //MaterialList.Add(diffuseMaterial);
                                                //}
                                                #endregion


                                                ////Create Texture
                                                //HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture texture = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                                //if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.LA8)
                                                //{
                                                //    //L8, LA8
                                                //    HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                                //    phongMaterial.UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
                                                //    phongMaterial.SpecularColorMap = texture.ToTextureModel(false);
                                                //    phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                                //    phongMaterial.SpecularShininess = 0.5f;
                                                //    phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                                //    MaterialList.Add(phongMaterial);
                                                //}
                                                //else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.HILO8)
                                                //{
                                                //    HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                                //    phongMaterial.UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
                                                //    phongMaterial.NormalMap = texture.ToTextureModel(false);
                                                //    phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                                //    phongMaterial.SpecularShininess = 0.5f;
                                                //    phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                                //    MaterialList.Add(phongMaterial);
                                                //}
                                                ////else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1A4 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.RGB565)
                                                ////{
                                                ////    //HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                                ////    //{
                                                ////    //    DiffuseMap = texture.ToTextureModel(false),
                                                ////    //    //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                                ////    //    UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                                ////    //    EnableUnLit = true,
                                                ////    //    VertexColorBlendingFactor = 0
                                                ////    //};

                                                ////    //diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                                ////    //MaterialList.Add(diffuseMaterial);
                                                ////}
                                                //else
                                                //{
                                                //    HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                                //    {
                                                //        DiffuseMap = texture.ToTextureModel(false),
                                                //        //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                                //        UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                                //        EnableUnLit = true,
                                                //        VertexColorBlendingFactor = 0
                                                //    };

                                                //    diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                                //    MaterialList.Add(diffuseMaterial);
                                                //}
                                            }
                                        }
                                    }

                                    var y = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet(MaterialList);
                                    var dict = y.ToIndexDictionary();


                                    //SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
                                    GroupModel3D sceneNodeGroupModel3D = new GroupModel3D();



                                    foreach (var m in dict)
                                    {
                                        ArrayList arrayList = m.Value;

                                        HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType materialType = (HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType)m.Value[1];

                                        //Check MatType
                                        if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Phong)
                                        {
                                            MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                            {
                                                Geometry = meshGeometry3D,
                                                Material = (PhongMaterial)m.Value[2],
                                                CullMode = SharpDX.Direct3D11.CullMode.None,
                                                WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF),
                                                RenderWireframe = false,
                                                IsThrowingShadow = true,
                                                IsTransparent = true,
                                                //DepthBias = m.Key
                                            };
                                            sceneNodeGroupModel3D.Children.Add(meshGeometryModel3D);
                                        }
                                        if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Normal)
                                        {
                                            MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D { Geometry = meshGeometry3D, Material = (NormalMaterial)m.Value[2], CullMode = SharpDX.Direct3D11.CullMode.None, WireframeColor = Color.FromArgb(0xFF, 0x39, 0xFF, 0x80), RenderWireframe = false, IsTransparent = true };
                                            render.Viewport3DX.Items.Add((SceneNode)meshGeometryModel3D);
                                        }

                                        if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Diffuse)
                                        {
                                            MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                            {
                                                Geometry = meshGeometry3D,
                                                Material = (HelixToolkit.Wpf.SharpDX.DiffuseMaterial)m.Value[2],
                                                CullMode = SharpDX.Direct3D11.CullMode.None,
                                                WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                                                RenderWireframe = false,
                                                IsTransparent = true,
                                                //DepthBias = m.Key
                                            };
                                            //sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                            sceneNodeGroupModel3D.Children.Add(meshGeometryModel3D);
                                        }
                                        else if ((materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Diffuse && (string)m.Value[0] == "VertColor") == true)
                                        {
                                            MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                            {
                                                Geometry = meshGeometry3D,
                                                Material = (HelixToolkit.Wpf.SharpDX.DiffuseMaterial)m.Value[2],
                                                CullMode = SharpDX.Direct3D11.CullMode.None,
                                                WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                                                RenderWireframe = false,
                                                IsTransparent = true,
                                                //DepthBias = 1,
                                                //SlopeScaledDepthBias = -1000,
                                                //IsDepthClipEnabled = true,
                                                //DepthBias = m.Key
                                            };
                                            //sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                            sceneNodeGroupModel3D.Children.Add(meshGeometryModel3D);
                                        }

                                        ////Vertex
                                        //if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.VertexColor)
                                        //{
                                        //    MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                        //    {
                                        //        Geometry = meshGeometry3D,
                                        //        Material = (VertColorMaterial)m.Value[2],
                                        //        CullMode = SharpDX.Direct3D11.CullMode.None,
                                        //        IsThrowingShadow = true,
                                        //        DepthBias = 2
                                        //    };
                                        //    sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                        //}
                                    }

                                    render.Viewport3DX.Items.Add(sceneNodeGroupModel3D);

                                    //List<HelixToolkit.Wpf.SharpDX.Material> MaterialList = new List<HelixToolkit.Wpf.SharpDX.Material>();
                                    //if (meshGeometry3D.Colors.Count != 0)
                                    //{
                                    //    HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                    //    {
                                    //        DiffuseMap = null,
                                    //        //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                    //        UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                    //        EnableUnLit = true,
                                    //        VertexColorBlendingFactor = 1,
                                    //    };

                                    //    diffuseMaterial.Name = "VertColor";
                                    //    MaterialList.Add(diffuseMaterial);
                                    //}

                                    //foreach (var i in MaterialInfoSetList)
                                    //{
                                    //    var MatName = i.TXOBDataSection.TXOB.MaterialInfoSection.MTOB_MaterialName;

                                    //    if (MatName != null)
                                    //    {
                                    //        if (CMDL_BitmapDictionary[MatName].TXOB_Bitmap != null)
                                    //        {
                                    //            //Create Texture
                                    //            HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture texture = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.Texture(CMDL_BitmapDictionary[MatName].TXOB_Bitmap);
                                    //            if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.LA8)
                                    //            {
                                    //                //L8, LA8
                                    //                HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                    //                phongMaterial.UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
                                    //                phongMaterial.SpecularColorMap = texture.ToTextureModel(false);
                                    //                phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                    //                phongMaterial.SpecularShininess = 0.5f;
                                    //                phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                    //                MaterialList.Add(phongMaterial);
                                    //            }
                                    //            else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.HILO8)
                                    //            {
                                    //                HelixToolkit.Wpf.SharpDX.PhongMaterial phongMaterial = new PhongMaterial();
                                    //                phongMaterial.UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
                                    //                phongMaterial.NormalMap = texture.ToTextureModel(false);
                                    //                phongMaterial.SpecularColor = PhongMaterials.ToColor(1, 1, 1, 0.5);
                                    //                phongMaterial.SpecularShininess = 0.5f;
                                    //                phongMaterial.Name = (string)texture.BitmapSource.Tag;

                                    //                MaterialList.Add(phongMaterial);
                                    //            }
                                    //            else if (CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.ETC1A4 || CMDL_BitmapDictionary[MatName].ImageFormat == CGFX_Viewer.CGFX.TextureFormat.Textures.ImageFormat.RGB565)
                                    //            {
                                    //                HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                    //                {
                                    //                    DiffuseMap = texture.ToTextureModel(false),
                                    //                    //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                    //                    UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                    //                    EnableUnLit = true,
                                    //                    VertexColorBlendingFactor = 0
                                    //                };

                                    //                diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                    //                MaterialList.Add(diffuseMaterial);
                                    //            }
                                    //            else
                                    //            {

                                    //                //HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                    //                //{
                                    //                //    DiffuseMap = null,
                                    //                //    //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                    //                //    UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                    //                //    EnableUnLit = true,
                                    //                //    VertexColorBlendingFactor = 1,
                                    //                //};

                                    //                //diffuseMaterial.Name = "VertColor";
                                    //                //MaterialList.Add(diffuseMaterial);

                                    //                //HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial
                                    //                //{
                                    //                //    DiffuseMap = texture.ToTextureModel(false),
                                    //                //    //DiffuseColor = new Color4(0x00, 0xFF, 0x00, 0xFF),
                                    //                //    UVTransform = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0)),
                                    //                //    EnableUnLit = true,
                                    //                //    VertexColorBlendingFactor = 0
                                    //                //};

                                    //                //diffuseMaterial.Name = (string)texture.BitmapSource.Tag;
                                    //                //MaterialList.Add(diffuseMaterial);
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    //var y = new HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet(MaterialList);
                                    //var dict = y.ToIndexDictionary();


                                    ////SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
                                    //GroupModel3D sceneNodeGroupModel3D = new GroupModel3D();



                                    //foreach (var m in dict)
                                    //{
                                    //    ArrayList arrayList = m.Value;

                                    //    HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType materialType = (HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType)m.Value[1];

                                    //    ////Check MatType
                                    //    //if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Phong)
                                    //    //{
                                    //    //    MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                    //    //    { 
                                    //    //        Geometry = meshGeometry3D,
                                    //    //        Material = (PhongMaterial)m.Value[2],
                                    //    //        CullMode = SharpDX.Direct3D11.CullMode.None,
                                    //    //        WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF),
                                    //    //        RenderWireframe = true,
                                    //    //        IsThrowingShadow = true,
                                    //    //        IsTransparent = true,
                                    //    //        DepthBias = 1
                                    //    //    };
                                    //    //    sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                    //    //}
                                    //    //if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Normal)
                                    //    //{
                                    //    //    MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D { Geometry = meshGeometry3D, Material = (NormalMaterial)m.Value[2], CullMode = SharpDX.Direct3D11.CullMode.None, WireframeColor = Color.FromArgb(0xFF, 0x39, 0xFF, 0x80), RenderWireframe = true, IsTransparent = true };
                                    //    //    render.Viewport3DX.Items.Add((SceneNode)meshGeometryModel3D);
                                    //    //}

                                    //    if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Diffuse)
                                    //    {
                                    //        MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                    //        {
                                    //            Geometry = meshGeometry3D,
                                    //            Material = (HelixToolkit.Wpf.SharpDX.DiffuseMaterial)m.Value[2],
                                    //            CullMode = SharpDX.Direct3D11.CullMode.None,
                                    //            WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                                    //            RenderWireframe = false,
                                    //            IsTransparent = true,
                                    //            DepthBias = 2
                                    //        };
                                    //        //sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                    //        sceneNodeGroupModel3D.Children.Add(meshGeometryModel3D);
                                    //    }
                                    //    else if ((materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.Diffuse && (string)m.Value[0] == "VertColor") == true)
                                    //    {
                                    //        MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                    //        {
                                    //            Geometry = meshGeometry3D,
                                    //            Material = (HelixToolkit.Wpf.SharpDX.DiffuseMaterial)m.Value[2],
                                    //            CullMode = SharpDX.Direct3D11.CullMode.None,
                                    //            WireframeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                                    //            RenderWireframe = false,
                                    //            IsTransparent = true,
                                    //            DepthBias = 1
                                    //        };
                                    //        //sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                    //        sceneNodeGroupModel3D.Children.Add(meshGeometryModel3D);
                                    //    }

                                    //    ////Vertex
                                    //    //if (materialType == HTK_3DES_WpfSharpDX.CustomMeshBuildHelper.MaterialSet.MaterialType.VertexColor)
                                    //    //{
                                    //    //    MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D
                                    //    //    {
                                    //    //        Geometry = meshGeometry3D,
                                    //    //        Material = (VertColorMaterial)m.Value[2],
                                    //    //        CullMode = SharpDX.Direct3D11.CullMode.None,
                                    //    //        IsThrowingShadow = true,
                                    //    //        DepthBias = 2
                                    //    //    };
                                    //    //    sceneNodeGroupModel3D.AddNode((SceneNode)meshGeometryModel3D);
                                    //    //}
                                    //}

                                    //render.Viewport3DX.Items.Add(sceneNodeGroupModel3D);


                                    ////PointGeometryModel3D d = new PointGeometryModel3D
                                    ////{
                                    ////    Geometry = new PointGeometry3D()
                                    ////    {
                                    ////        Positions = meshGeometry3D.Positions,
                                    ////        Indices = meshGeometry3D.Indices
                                    ////    },
                                    ////    Color = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00),
                                    ////    Size = new System.Windows.Size(5, 5)
                                    ////};
                                    ////render.Viewport3DX.Items.Add(d);


                                    ////LineGeometryModel3D L = new LineGeometryModel3D
                                    ////{
                                    ////    Geometry = new LineGeometry3D()
                                    ////    {
                                    ////        Positions = meshGeometry3D.Positions,
                                    ////        Indices = meshGeometry3D.Indices,
                                    ////    },
                                    ////    Color = Color.FromArgb(0xFF, 0x00, 0x00, 0xFF),
                                    ////    Smoothness = 5.0,
                                    ////    //FixedSize = false
                                    ////    //Size = new System.Windows.Size(5, 5)
                                    ////};
                                    ////render.Viewport3DX.Items.Add(L);


                                    ////SceneNode sceneNode = (SceneNode)groupModel3D;

                                    //////表示
                                    ////render.Viewport3DX.Items.Add(groupModel3D.SceneNode);
                                    //////render.UpdateLayout();
                                    /////

                                    #region GetPointColor (Test)
                                    //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    //{
                                    //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    pointsVisual3D.Points = new Point3DCollection();
                                    //    pointsVisual3D.Points.Add(ym.Vertex);
                                    //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    pointsVisual3D.Size = 5;

                                    //    render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    render.UpdateLayout();
                                    //}
                                    #endregion

                                    #region Point3D Only (Test)
                                    //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                                    //{
                                    //    //List<Point3D> point3Ds = new List<Point3D>();
                                    //    //point3Ds.Add(ym.Vertex);

                                    //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    pointsVisual3D.Points = new Point3DCollection();
                                    //    pointsVisual3D.Points.Add(ym.Vertex);
                                    //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    pointsVisual3D.Size = 5;

                                    //    render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    render.UpdateLayout();


                                    //    //List<Point3D> point3Ds = new List<Point3D>();
                                    //    //point3Ds.Add(ym.Vertex);

                                    //    //PointsVisual3D pointsVisual3D = new PointsVisual3D();
                                    //    //pointsVisual3D.Points = new Point3DCollection(point3Ds);
                                    //    //pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                                    //    //pointsVisual3D.Size = 5;

                                    //    //render.MainViewPort.Children.Add(pointsVisual3D);
                                    //    //render.UpdateLayout();
                                    //}
                                    #endregion
                                }
                                if (VertexAttr.Value.Flag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }))
                                {

                                }
                            }
                        }

                        #region Backup2
                        //var Models = ht.CMDLSection;
                        //propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        ////Get TextureName
                        //List<List<string>> MTOBNameSetList = new List<List<string>>();
                        //foreach (var Mtl in Models.MTOB_DICT.DICT_Entries)
                        //{
                        //    List<string> MTOBNameList = Mtl.CGFXData.CGFXSectionData.MTOBSection.GetMaterialInfoSet().Select(x => x.TXOBDataSection.TXOB.MaterialInfoSection.MTOB_MaterialName).ToList();
                        //    MTOBNameSetList.Add(MTOBNameList);
                        //}

                        ////Get Texture (Bitmap)
                        //Dictionary<string, Bitmap> CMDL_BitmapDictionary = new Dictionary<string, Bitmap>();
                        //foreach (var df in CGFX.DICTAndSectionData["Textures"].DICT_Entries)
                        //{
                        //    string s = df.Name;

                        //    string TextureName = df.CGFXData.CGFXSectionData.TXOBSection.TextureSection.Name;
                        //    CMDL_BitmapDictionary.Add(TextureName, df.CGFXData.CGFXSectionData.TXOBSection.TextureSection.TXOB_Bitmap);
                        //}

                        //foreach (var qs in Models.meshDatas)
                        //{
                        //    int MtlId = qs.SOBJData.Meshes.MaterialIndex;
                        //    List<string> strings = MTOBNameSetList[MtlId];

                        //    List<Bitmap> BitmapDict = new List<Bitmap>();
                        //    for (int i = 0; i < 4; i++) //4
                        //    {
                        //        if (strings[i] != null)
                        //        {
                        //            Bitmap Texture = CMDL_BitmapDictionary[strings[i]]; //TODO : If material has no texture, use a dummy texture
                        //            Texture.Tag = strings[i];
                        //            BitmapDict.Add(Texture);
                        //        }
                        //        if (strings[i] == null) BitmapDict.Add(null);
                        //    }

                        //    int ShapeID = qs.SOBJData.Meshes.ShapeIndex;
                        //    var Shape = Models.shapeDatas[ShapeID].SOBJData.Shapes;
                        //    var indexStreamCtrs = Shape.primitiveSets.Select(x => x.GetIndexStreamCtrPrimitive()).ToList();

                        //    foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                        //    {
                        //        HTK_3DES.CustomMeshBuildHelper.Mesh mesh = new HTK_3DES.CustomMeshBuildHelper.Mesh(true, true, true);
                        //        for (int q = 0; q < indexStreamCtrs[0][0].Count; q++) mesh.AddTriangleIndicesArray(indexStreamCtrs[0][0][q].FaceArray.ToArray());
                        //        foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        {
                        //            var Vertex = ym.Scaled<Point3D>(Polygon.DataType.Vt);
                        //            var Normal = ym.Scaled<Vector3D>(Polygon.DataType.Nr);
                        //            var TexCoord = ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint();
                        //            mesh.Add(Vertex, Normal, TexCoord);
                        //        }

                        //        MeshGeometry3D meshGeometry3D = mesh.ToMeshGeometry3D(true);

                        //        #region Del
                        //        //MeshBuilder meshBuilder = new MeshBuilder(true, true, true);
                        //        ////meshBuilder.Scale(100, 100, 100);

                        //        //for (int q = 0; q < indexStreamCtrs[0][0].Count; q++)
                        //        //{
                        //        //    foreach (var Indice in indexStreamCtrs[0][0][q].FaceArray) meshBuilder.TriangleIndices.Add(Indice);
                        //        //}

                        //        //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        //{
                        //        //    #region BackUp
                        //        //    //meshBuilder.Positions.Add(ym.Vertex);
                        //        //    //meshBuilder.Normals.Add(ym.Normal);
                        //        //    //meshBuilder.TextureCoordinates.Add(ym.TexCoord.ToPoint());

                        //        //    //meshBuilder.Positions.Add(ym.Scaled<Point3D>(Polygon.DataType.Vt));
                        //        //    //meshBuilder.Normals.Add(ym.Scaled<Vector3D>(Polygon.DataType.Nr));
                        //        //    //meshBuilder.TextureCoordinates.Add(ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint());

                        //        //    //ym.Scale_Factor.VtScale = 20;
                        //        //    //ym.Scale_Factor.TexCoordScale = -0.00005f;
                        //        //    #endregion

                        //        //    var Vertex = ym.Scaled<Point3D>(Polygon.DataType.Vt);
                        //        //    var Normal = ym.Scaled<Vector3D>(Polygon.DataType.Nr);
                        //        //    var TexCoord = ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint();

                        //        //    meshBuilder.Positions.Add(Vertex);
                        //        //    meshBuilder.Normals.Add(Normal);
                        //        //    meshBuilder.TextureCoordinates.Add(TexCoord);
                        //        //}

                        //        //MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);
                        //        #endregion

                        //        MaterialGroup material = new MaterialGroup();
                        //        foreach (var Bitmap in BitmapDict)
                        //        {
                        //            if (Bitmap != null)
                        //            {
                        //                //Create Texture
                        //                HTK_3DES.CustomMeshBuildHelper.Texture texture = new HTK_3DES.CustomMeshBuildHelper.Texture(Bitmap);
                        //                var imageBrush = texture.ToImageBrush(1, 1);

                        //                DiffuseMaterial Mtl = new DiffuseMaterial(imageBrush);
                        //                Mtl.SetName((string)Bitmap.Tag);
                        //                material.Children.Add(Mtl);
                        //            }
                        //        }

                        //        var m3dGrp = new Model3DGroup();
                        //        m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                        //        ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                        //        //mList.Add(m);

                        //        //表示
                        //        render.MainViewPort.Children.Add(m);
                        //        render.UpdateLayout();




                        //        //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        //{
                        //        //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                        //        //    pointsVisual3D.Points = new Point3DCollection();
                        //        //    pointsVisual3D.Points.Add(ym.Vertex);
                        //        //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                        //        //    pointsVisual3D.Size = 5;

                        //        //    render.MainViewPort.Children.Add(pointsVisual3D);
                        //        //    render.UpdateLayout();
                        //        //}



                        //        #region Point3D Only
                        //        //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        //{
                        //        //    //List<Point3D> point3Ds = new List<Point3D>();
                        //        //    //point3Ds.Add(ym.Vertex);

                        //        //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                        //        //    pointsVisual3D.Points = new Point3DCollection();
                        //        //    pointsVisual3D.Points.Add(ym.Vertex);
                        //        //    pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                        //        //    pointsVisual3D.Size = 5;

                        //        //    render.MainViewPort.Children.Add(pointsVisual3D);
                        //        //    render.UpdateLayout();


                        //        //    //List<Point3D> point3Ds = new List<Point3D>();
                        //        //    //point3Ds.Add(ym.Vertex);

                        //        //    //PointsVisual3D pointsVisual3D = new PointsVisual3D();
                        //        //    //pointsVisual3D.Points = new Point3DCollection(point3Ds);
                        //        //    //pointsVisual3D.Color = System.Windows.Media.Color.FromArgb(ym.ColorData.A, ym.ColorData.R, ym.ColorData.G, ym.ColorData.B);
                        //        //    //pointsVisual3D.Size = 5;

                        //        //    //render.MainViewPort.Children.Add(pointsVisual3D);
                        //        //    //render.UpdateLayout();
                        //        //}
                        //        #endregion
                        //    }

                        //    //int MtlId = qs.SOBJData.Meshes.MaterialIndex;
                        //    //List<string> strings = MTOBNameSetList[MtlId];
                        //    //if (strings[0] == null) return;

                        //    //Bitmap Texture = CMDL_BitmapDictionary[strings[0]]; //TODO : If material has no texture, use a dummy texture

                        //    //foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                        //    //{
                        //    //    MeshBuilder meshBuilder = new MeshBuilder(true, true, true);
                        //    //    //meshBuilder.Scale(100, 100, 100);

                        //    //    for (int q = 0; q < indexStreamCtrs[0][0].Count; q++)
                        //    //    {
                        //    //        foreach (var Indice in indexStreamCtrs[0][0][q].FaceArray) meshBuilder.TriangleIndices.Add(Indice);
                        //    //    }

                        //    //    foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //    //    {
                        //    //        #region BackUp
                        //    //        //meshBuilder.Positions.Add(ym.Vertex);
                        //    //        //meshBuilder.Normals.Add(ym.Normal);
                        //    //        //meshBuilder.TextureCoordinates.Add(ym.TexCoord.ToPoint());

                        //    //        //meshBuilder.Positions.Add(ym.Scaled<Point3D>(Polygon.DataType.Vt));
                        //    //        //meshBuilder.Normals.Add(ym.Scaled<Vector3D>(Polygon.DataType.Nr));
                        //    //        //meshBuilder.TextureCoordinates.Add(ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint());

                        //    //        //ym.Scale_Factor.VtScale = 20;
                        //    //        //ym.Scale_Factor.TexCoordScale = -0.00005f;
                        //    //        #endregion

                        //    //        var Vertex = ym.Scaled<Point3D>(Polygon.DataType.Vt);
                        //    //        var Normal = ym.Scaled<Vector3D>(Polygon.DataType.Nr);
                        //    //        var TexCoord = ym.Scaled<Polygon.TextureCoordinate>(Polygon.DataType.TexCoord0).ToPoint();

                        //    //        meshBuilder.Positions.Add(Vertex);
                        //    //        meshBuilder.Normals.Add(Normal);
                        //    //        meshBuilder.TextureCoordinates.Add(TexCoord);
                        //    //    }

                        //    //    MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);

                        //    //    //Create Texture
                        //    //    BitmapImage bitmapImage = new BitmapImage();
                        //    //    bitmapImage.BeginInit();
                        //    //    bitmapImage.StreamSource = CGFXHelper.BitmapToMemoryStream(Texture);
                        //    //    bitmapImage.EndInit();

                        //    //    ImageBrush imageBrush = new ImageBrush
                        //    //    {
                        //    //        ImageSource = bitmapImage,
                        //    //        TileMode = TileMode.Tile,
                        //    //        Stretch = Stretch.Fill,
                        //    //        ViewportUnits = BrushMappingMode.Absolute,
                        //    //        ViewboxUnits = BrushMappingMode.RelativeToBoundingBox,
                        //    //        AlignmentX = AlignmentX.Center,
                        //    //        AlignmentY = AlignmentY.Center,
                        //    //        Viewport = new Rect(0, 0, 1, 1)
                        //    //    };

                        //    //    DiffuseMaterial material = new DiffuseMaterial(imageBrush);

                        //    //    //MaterialGroup materialGroup = new MaterialGroup();
                        //    //    //materialGroup.Children.Add(new MapTexture().Material);

                        //    //    //Material material = MaterialHelper.CreateMaterial(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

                        //    //    var m3dGrp = new Model3DGroup();
                        //    //    m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                        //    //    ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                        //    //    //mList.Add(m);

                        //    //    //表示
                        //    //    render.MainViewPort.Children.Add(m);
                        //    //    render.UpdateLayout();

                        //    //    #region Point3D Only
                        //    //    //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //    //    //{
                        //    //    //    List<Point3D> point3Ds = new List<Point3D>();
                        //    //    //    point3Ds.Add(ym.Vertex);

                        //    //    //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                        //    //    //    pointsVisual3D.Points = new Point3DCollection(point3Ds);
                        //    //    //    pointsVisual3D.Color = Colors.Blue;
                        //    //    //    pointsVisual3D.Size = 5;

                        //    //    //    render.MainViewPort.Children.Add(pointsVisual3D);
                        //    //    //    render.UpdateLayout();
                        //    //    //}
                        //    //    #endregion
                        //    //}
                        //}
                        #endregion

                        #region Backup
                        //var Models = ht.CMDLSection;
                        //propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_PropertyGrid(Models);

                        ////Get Texture (Bitmap)
                        //var MeshInfoList = Models.meshDatas.Select(x => x.SOBJData.Meshes).ToList();
                        //List<Bitmap> CMDL_BitmapList = CGFX.DICTAndSectionData["Textures"].DICT_Entries.Select(x => x.CGFXData.CGFXSectionData.TXOBSection.TXOB_Bitmap).ToList();

                        //foreach (var shape in Models.shapeDatas.Select((value, i) => new { Value = value, Index = i }))
                        //{
                        //    List<List<List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr>>> indexStreamCtrs = new List<List<List<CGFXFormat.SOBJ.Shape.PrimitiveSet.Primitive.IndexStreamCtr>>>();

                        //    var Shape = shape.Value.SOBJData.Shapes;
                        //    foreach (var PrimitiveSet in Shape.primitiveSets)
                        //    {
                        //        var g = PrimitiveSet.Primitives.Select(x => x.IndexStreamCtrList).ToList();
                        //        indexStreamCtrs.Add(g);
                        //    }

                        //    //FindTexture
                        //    int MtlId = MeshInfoList[shape.Index].MaterialIndex;
                        //    Bitmap Texture = CMDL_BitmapList[MtlId];

                        //    foreach (var VertexAttr in Shape.VertexAttributes.Select((value, i) => new { Value = value, Index = i }))
                        //    {
                        //        MeshBuilder meshBuilder = new MeshBuilder(true, true, true);

                        //        foreach (var Indice in indexStreamCtrs[0][0][0].FaceArray) meshBuilder.TriangleIndices.Add(Indice);

                        //        foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        {
                        //            meshBuilder.Positions.Add(ym.Vertex);
                        //            meshBuilder.Normals.Add(ym.Normal);
                        //            meshBuilder.TextureCoordinates.Add(ym.TexCoord.ToPoint());
                        //        }

                        //        MeshGeometry3D meshGeometry3D = meshBuilder.ToMesh(true);

                        //        //Create Texture
                        //        BitmapImage bitmapImage = new BitmapImage();
                        //        bitmapImage.BeginInit();
                        //        bitmapImage.StreamSource = CGFXHelper.BitmapToMemoryStream(Texture);
                        //        bitmapImage.EndInit();
                        //        Material material = MaterialHelper.CreateImageMaterial(bitmapImage, 100, true);

                        //        //Material material = MaterialHelper.CreateMaterial(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));

                        //        var m3dGrp = new Model3DGroup();
                        //        m3dGrp.Children.Add(new GeometryModel3D { Geometry = meshGeometry3D, Material = material, BackMaterial = material });

                        //        ModelVisual3D m = new ModelVisual3D { Content = m3dGrp };
                        //        //mList.Add(m);

                        //        //表示
                        //        render.MainViewPort.Children.Add(m);

                        //        #region Point3D Only
                        //        //foreach (var ym in VertexAttr.Value.Streams.PolygonList)
                        //        //{
                        //        //    List<Point3D> point3Ds = new List<Point3D>();
                        //        //    point3Ds.Add(ym.Vertex);

                        //        //    PointsVisual3D pointsVisual3D = new PointsVisual3D();
                        //        //    pointsVisual3D.Points = new Point3DCollection(point3Ds);
                        //        //    pointsVisual3D.Color = Colors.Blue;
                        //        //    pointsVisual3D.Size = 5;

                        //        //    render.MainViewPort.Children.Add(pointsVisual3D);
                        //        //    render.UpdateLayout();
                        //        //}
                        //        #endregion
                        //    }
                        //}
                        #endregion
                    }
                    if (Set[1] == "Textures")
                    {
                        var Textures = ht.TXOBSection.TextureSection;
                        pictureBox1.Image = Textures.TXOB_Bitmap;
                        propertyGrid2.SelectedObject = new CGFXPropertyGridSet.TXOB_PropertyGrid(Textures);
                    }
                    if (Set[1] == "LUTS") return;
                    if (Set[1] == "Materials") return;
                    if (Set[1] == "Shaders") return;
                    if (Set[1] == "Cameras") return;
                    if (Set[1] == "Lights") return;
                    if (Set[1] == "Fog")
                    {
                        var fogs = ht.CFOGSection;
                        propertyGrid1.SelectedObject = new CGFXPropertyGridSet.CFOG_PropertyGrid(fogs);
                    }
                    if (Set[1] == "Environments")
                    {

                    }
                }
                if (Set.Length == 4)
                {
                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;
                }
                if (Set.Length == 5)
                {
                    var ht = CGFX.DICTAndSectionData[Set[1]].DICT_Entries.Find(x => x.Name == Set[2]).CGFXData.CGFXSectionData;

                    if (Set[1] == "Model")
                    {
                        var Models = ht.CMDLSection;
                        propertyGrid3.SelectedObject = null;
                        if (Set[3] == "Mesh")
                        {
                            //propertyGrid3.SelectedObject = Models.meshDatas.Find(x => x.SOBJData.Meshes.MeshName == Set[4]).SOBJData.Meshes;
                            propertyGrid3.SelectedObject = new CGFXPropertyGridSet.CMDL_MeshData_PropertyGrid(Models.meshDatas.Find(x => x.SOBJData.Meshes.MeshName == Set[4]));
                        }
                        if (Set[3] == "Material")
                        {
                            //propertyGrid3.SelectedObject = Models.MTOB_DICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.CGFXSectionData.MTOBSection;

                            propertyGrid3.SelectedObject = new CGFXPropertyGridSet.MTOB_PropertyGrid(Models.MTOB_DICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.CGFXSectionData.MTOBSection);
                        }
                        if (Set[3] == "Shape")
                        {
                            //propertyGrid3.SelectedObject = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes;
                            propertyGrid3.SelectedObject = new PropertyGridForms.Section.CMDL.ShapeData.ShapeData_PropertyGrid(Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes);

                            //var i = Models.shapeDatas[Convert.ToInt32(Set[4])].SOBJData.Shapes.VertexAttributes.Select(x => x.Streams.PolygonList).ToList();
                        }
                        if (Set[3] == "LinkedMaterial")
                        {
                            propertyGrid3.SelectedObject = Models.UnknownDICT.DICT_Entries.Find(x => x.Name == Set[4]).CGFXData.NativeDataSections.CMDL_Native.MaterialName_Set.Name;
                        }
                    }
                }
            }
        }
    }
}
