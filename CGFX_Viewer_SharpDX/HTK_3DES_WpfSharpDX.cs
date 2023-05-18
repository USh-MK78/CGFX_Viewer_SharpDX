using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using HelixToolkit.Wpf.SharpDX.Model.Scene2D;
using SharpDX;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using HelixToolkit.Wpf.SharpDX.Model;
using System.CodeDom;
using SharpDX.Direct3D11;
using Matrix = SharpDX.Matrix;
using System.Windows.Controls;
using SharpDX.Direct3D9;

namespace CGFX_Viewer_SharpDX
{
    public class HTK_3DES_WpfSharpDX
    {
        public class TSRSystem
        {
            public class Transform
            {
                public Vector3 Rotate3D { get; set; }
                public Vector3 Scale3D { get; set; }
                public Vector3 Translate3D { get; set; }
            }
            ///// <summary>
            ///// objファイルを読み込み、ModelVisual3Dを返すメソッド
            ///// </summary>
            ///// <param name="Path">Model Path</param>
            ///// <returns>ModelVisual3D</returns>
            //public static SceneNodeGroupModel3D OBJReader(string Path)
            //{
            //    SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();

            //    #pragma warning disable CS0618
            //    ObjReader objRead = new ObjReader();
            //    List<Object3D> ObjModelList = objRead.Read(Path);




            //    #region delcode(?)
            //    //ObjReader objRead = new ObjReader();

            //    //SortingVisual3D sortingVisual3D = new SortingVisual3D
            //    //{
            //    //    Method = SortingMethod.BoundingSphereSurface,
            //    //    SortingFrequency = 2,
            //    //    Content = objRead.Read(Path)
            //    //};

            //    //ModelVisual3D dv3D = sortingVisual3D;
            //    #endregion

            //    return dv3D;
            //}

            /// <summary>
            /// ガベージコレクション
            /// </summary>
            public static void GC_Dispose(object f)
            {
                int GCNum = GC.GetGeneration(f);

                GC.Collect(GCNum);
                GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            ///// <summary>
            ///// ModelVisual3Dに文字列を関連付けて新しいModelVisual3Dを生成する
            ///// </summary>
            ///// <param name="MV3D">Input ModelVisual3D</param>
            ///// <param name="InputString">Input String</param>
            ///// <returns></returns>
            //public static ModelVisual3D SetStringAndNewSN3D(Sce MV3D, string InputString)
            //{
            //    MV3D.SetName(InputString);
            //    return MV3D;
            //}

            ///// <summary>
            ///// ModelVisual3Dに文字列を関連付ける
            ///// </summary>
            ///// <param name="MV3D">Input ModelVisual3D</param>
            ///// <param name="InputString">Input String</param>
            //public static void SetString_MV3D(ModelVisual3D MV3D, string InputString)
            //{
            //    MV3D.SetName(InputString);
            //}

            public static Vector3 ScaleFactor(Vector3 InputVector3, float ScaleFactor)
            {
                return new Vector3(InputVector3.X * ScaleFactor, InputVector3.Y * ScaleFactor, InputVector3.Z * ScaleFactor);
            }

            public static Vector3 ScaleFactor(float PointSize, float ScaleFactor)
            {
                return new Vector3(PointSize * ScaleFactor, PointSize * ScaleFactor, PointSize * ScaleFactor);
            }

            /// <summary>
            /// Radianを角度に変換
            /// </summary>
            /// <param name="Radian"></param>
            /// <returns></returns>
            public static float RadianToAngle(double Radian)
            {
                return (float)(Radian * (180 / Math.PI));
            }

            /// <summary>
            /// 角度をRadianに変換
            /// </summary>
            /// <param name="Angle"></param>
            /// <returns></returns>
            public static double AngleToRadian(double Angle)
            {
                return (float)(Angle * (Math.PI / 180));
            }

            public static Vector3 RadianToAngleVector3D(Vector3 vector3)
            {
                return new Vector3((float)(vector3.X * (180 / Math.PI)), (float)(vector3.Y * (180 / Math.PI)), (float)(vector3.Z * (180 / Math.PI)));
            }

            public static Vector3 AngleToRadianVector3D(Vector3 vector3)
            {
                return new Vector3((float)(vector3.X * (Math.PI / 180)), (float)(vector3.Y * (Math.PI / 180)), (float)(vector3.Z * (Math.PI / 180)));
            }

            public static Point3D CalculateModelCenterPoint(ModelVisual3D MV3D)
            {
                Rect3D r = MV3D.Content.Bounds;
                double cX = r.X + r.SizeX / 2;
                double cY = r.Y + r.SizeY / 2;
                double cZ = r.Z + r.SizeZ / 2;
                Point3D P3 = new Point3D(cX, cY, cZ);

                return P3;
            }

            public static Point3D CalculateModelCenterPoint(Model3D MV3D)
            {
                Rect3D r = MV3D.Bounds;
                double cX = r.X + r.SizeX / 2;
                double cY = r.Y + r.SizeY / 2;
                double cZ = r.Z + r.SizeZ / 2;
                Point3D P3 = new Point3D(cX, cY, cZ);

                return P3;
            }

            public static Vector3D Scalefactor(Vector3D v, double Factor)
            {
                return new Vector3D(v.X / Factor, v.Y / Factor, v.Z / Factor);
            }


            public class TSRSystem3D
            {
                public Transform TransformData { get; } = new Transform();
                public MeshNode MeshNode;
                public MeshGeometryModel3D MeshGeometryModel3D;

                public SceneNode SceneNode;

                public Type Types;
                public enum Type
                {
                    MeshNode,
                    MeshGeometryModel3D
                }

                public enum RotationType
                {
                    Angle,
                    Radian
                }

                public void Transform(RotationType rotationType = RotationType.Angle)
                {
                    Matrix3D matrix3D = new Matrix3D();
                    if (rotationType == RotationType.Angle)
                    {
                        //Matrix3D matrix3D = new Matrix3D();
                        matrix3D.Append(Matrix.Scaling(TransformData.Scale3D).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationX(TransformData.Rotate3D.X).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationY(TransformData.Rotate3D.Y).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationZ(TransformData.Rotate3D.Z).ToMatrix3D());
                        matrix3D.Append(Matrix.Translation(TransformData.Translate3D).ToMatrix3D());
                    }
                    if (rotationType == RotationType.Radian)
                    {
                        matrix3D.Append(Matrix.Scaling(TransformData.Scale3D).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationX(RadianToAngle(TransformData.Rotate3D.X)).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationY(RadianToAngle(TransformData.Rotate3D.Y)).ToMatrix3D());
                        matrix3D.Append(Matrix.RotationZ(RadianToAngle(TransformData.Rotate3D.Z)).ToMatrix3D());
                        matrix3D.Append(Matrix.Translation(TransformData.Translate3D).ToMatrix3D());
                    }

                    if (Types == Type.MeshNode) MeshNode.ModelMatrix = matrix3D.ToMatrix();
                    if (Types == Type.MeshGeometryModel3D) MeshGeometryModel3D.Transform = new MatrixTransform3D(matrix3D);
                }

                public TSRSystem3D(SceneNode sceneNode)
                {
                    if ((MeshNode)sceneNode is MeshNode)
                    {
                        Types = Type.MeshNode;
                        MeshNode = (MeshNode)sceneNode;
                    }
                    if ((MeshGeometryModel3D)((MeshNode)sceneNode) is MeshGeometryModel3D)
                    {
                        Types = Type.MeshGeometryModel3D;
                        MeshGeometryModel3D = (MeshGeometryModel3D)((MeshNode)sceneNode);
                    }
                }

                public TSRSystem3D(MeshNode meshNode)
                {
                    Types = Type.MeshNode;
                    MeshNode = meshNode;
                }

                public TSRSystem3D(MeshGeometryModel3D meshGeometryModel3D)
                {
                    Types = Type.MeshGeometryModel3D;
                    MeshGeometryModel3D = meshGeometryModel3D;
                }

                //public Transform Transform { get; } = new Transform();
                //public ModelVisual3D InputMV3D { get; }
                //public Model3D InputM3D;
                //public bool IsContent;

                ////public ModelVisual3D InputMV3D;
                ////public Model3D M3D
                ////{
                ////    get
                ////    {
                ////        return InputMV3D.Content ?? null;
                ////    }
                ////    set
                ////    {
                ////        InputMV3D = new ModelVisual3D { Content = M3D };
                ////    }
                ////}

                //public TSRSystem3D()
                //{
                //    return;
                //}

                ///// <summary>
                ///// TSRSystem3Dの初期化
                ///// </summary>
                ///// <param name="MV3D"></param>
                ///// <param name="transform"></param>
                //public TSRSystem3D(ModelVisual3D MV3D, Transform transform)
                //{
                //    InputMV3D = MV3D;
                //    InputM3D = null;
                //    Transform = transform;
                //    IsContent = MV3D.Content != null ? true : false;
                //}

                ///// <summary>
                ///// TSRSystem3Dの初期化
                ///// </summary>
                ///// <param name="MV3D"></param>
                ///// <param name="transform"></param>
                //public TSRSystem3D(Model3D M3D, Transform transform)
                //{
                //    InputMV3D = null;
                //    InputM3D = M3D;
                //    Transform = transform;
                //    IsContent = true;
                //}

                //#region Rotation
                //public RotateTransform3D Rotate_X { get; } = new RotateTransform3D();
                //public RotateTransform3D Rotate_Y { get; } = new RotateTransform3D();
                //public RotateTransform3D Rotate_Z { get; } = new RotateTransform3D();

                //public RotationCenterSetting RotationCenterSettings { get; }
                //public class RotationCenterSetting
                //{
                //    public Vector3D RotationX { get; set; } = new Vector3D(1, 0, 0);
                //    public Vector3D RotationY { get; set; } = new Vector3D(0, 1, 0);
                //    public Vector3D RotationZ { get; set; } = new Vector3D(0, 0, 1);

                //    public RotationCenterSetting()
                //    {
                //        RotationX = new Vector3D(1, 0, 0);
                //        RotationY = new Vector3D(0, 1, 0);
                //        RotationZ = new Vector3D(0, 0, 1);
                //    }
                //}

                //public enum RotationType
                //{
                //    Angle,
                //    Radian
                //}

                //public void TSR_Rotate(RotationCenterSetting RotationCenterSettings, RotationType RotationSettings = RotationType.Angle)
                //{
                //    double RotateX = new double();
                //    double RotateY = new double();
                //    double RotateZ = new double();

                //    if (RotationSettings == RotationType.Angle)
                //    {
                //        RotateX = Transform.Rotate3D.X;
                //        RotateY = Transform.Rotate3D.Y;
                //        RotateZ = Transform.Rotate3D.Z;
                //    }
                //    if (RotationSettings == RotationType.Radian)
                //    {
                //        RotateX = RadianToAngle(Transform.Rotate3D.X);
                //        RotateY = RadianToAngle(Transform.Rotate3D.Y);
                //        RotateZ = RadianToAngle(Transform.Rotate3D.Z);
                //    }

                //    Rotate_X.Rotation = new QuaternionRotation3D(new Quaternion(RotationCenterSettings.RotationX, RotateX));
                //    Rotate_Y.Rotation = new QuaternionRotation3D(new Quaternion(RotationCenterSettings.RotationY, RotateY));
                //    Rotate_Z.Rotation = new QuaternionRotation3D(new Quaternion(RotationCenterSettings.RotationZ, RotateZ));
                //}
                //#endregion

                //#region Scale
                //public ScaleTransform3D ScaleTransform3D;

                ///// <summary>
                ///// 
                ///// </summary>
                ///// <param name="ScaleFactor"></param>
                ///// <param name="Center"></param>
                //public void TSR_Scale3D(double ScaleFactorValue = 2, Point3D? Center = null, bool CenterFlag = false)
                //{
                //    if (CenterFlag == true) ScaleTransform3D = new ScaleTransform3D(Scalefactor(Transform.Scale3D, ScaleFactorValue), Center ?? new Point3D(0, 0, 0));
                //    if (CenterFlag == false) ScaleTransform3D = new ScaleTransform3D(Scalefactor(Transform.Scale3D, ScaleFactorValue));
                //}
                //#endregion

                //#region Translate
                //public TranslateTransform3D TranslateTransform3D;
                //public void TSR_Translate3D()
                //{
                //    TranslateTransform3D = new TranslateTransform3D(Transform.Translate3D);
                //}
                //#endregion

                //#region Transform
                //public ContentType GetContentType
                //{
                //    get
                //    {
                //        ContentType contentType = new ContentType();
                //        if ((InputMV3D != null && InputM3D == null) == true) contentType = ContentType.ModelVisual3D;
                //        if ((InputMV3D == null && InputM3D != null) == true) contentType = ContentType.Model3D;
                //        return contentType;
                //    }
                //}

                //public enum ContentType
                //{
                //    ModelVisual3D,
                //    Model3D
                //}

                //public void StartTransform()
                //{
                //    Transform3DCollection T3D_Collection = new Transform3DCollection();
                //    T3D_Collection.Add(ScaleTransform3D);
                //    T3D_Collection.Add(Rotate_X);
                //    T3D_Collection.Add(Rotate_Y);
                //    T3D_Collection.Add(Rotate_Z);
                //    T3D_Collection.Add(TranslateTransform3D);

                //    Transform3DGroup T3DGroup = new Transform3DGroup { Children = T3D_Collection };

                //    if (GetContentType == ContentType.ModelVisual3D)
                //    {
                //        if (IsContent == true) InputMV3D.Content.Transform = T3DGroup;
                //        if (IsContent == false) InputMV3D.Transform = T3DGroup;
                //    }
                //    if (GetContentType == ContentType.Model3D)
                //    {
                //        InputM3D.Transform = T3DGroup;
                //    }
                //}

                //public void TestTransform3D()
                //{
                //    TSRSystem3D tSRSystem3D = null;
                //    if (GetContentType == ContentType.ModelVisual3D) tSRSystem3D = new TSRSystem3D(InputMV3D, Transform);
                //    if (GetContentType == ContentType.Model3D) tSRSystem3D = new TSRSystem3D(InputM3D, Transform);
                //    tSRSystem3D.TSR_Rotate(new RotationCenterSetting(), RotationType.Angle);
                //    tSRSystem3D.TSR_Scale3D();
                //    tSRSystem3D.TSR_Translate3D();
                //    tSRSystem3D.StartTransform();
                //}

                //public void TestTransform3D(RotationCenterSetting rotationCenterSetting, RotationType rotationType, double ScaleFactor = 2, Point3D? Center = null, bool CenterFlag = false)
                //{
                //    TSRSystem3D tSRSystem3D = null;
                //    if (GetContentType == ContentType.ModelVisual3D) tSRSystem3D = new TSRSystem3D(InputMV3D, Transform);
                //    if (GetContentType == ContentType.Model3D) tSRSystem3D = new TSRSystem3D(InputM3D, Transform);
                //    tSRSystem3D.TSR_Rotate(rotationCenterSetting, rotationType);
                //    tSRSystem3D.TSR_Scale3D(ScaleFactor, Center, CenterFlag);
                //    tSRSystem3D.TSR_Translate3D();
                //    tSRSystem3D.StartTransform();
                //}
                //#endregion

                ////#region Get
                ////public Transform3DGroup GetTransform3DGroup()
                ////{
                ////    TSR_Rotate(new RotationCenterSetting(), RotationType.Angle);
                ////    TSR_Scale3D();
                ////    TSR_Translate3D();

                ////    Transform3DCollection T3D_Collection = new Transform3DCollection();
                ////    T3D_Collection.Add(ScaleTransform3D);
                ////    T3D_Collection.Add(Rotate_X);
                ////    T3D_Collection.Add(Rotate_Y);
                ////    T3D_Collection.Add(Rotate_Z);
                ////    T3D_Collection.Add(TranslateTransform3D);

                ////    return new Transform3DGroup { Children = T3D_Collection };
                ////}

                ////public TSRSystem3D(Transform transform)
                ////{
                ////    Transform = transform;
                ////}
                ////#endregion
            }
        }

        //public class UnionModel3D : TSRSystem
        //{
        //    ///// <summary>
        //    ///// Point3DのListからModelVisual3Dを生成
        //    ///// </summary>
        //    ///// <param name="P3DList">Point3D_List</param>
        //    ///// <param name="LV3D_List">LineVisual3D_List</param>
        //    ///// <param name="colors">Set Color</param>
        //    ///// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
        //    //public List<ModelVisual3D> CustomModelCreate(List<Point3D> P3DList, List<LinesVisual3D> LV3D_List, System.Windows.Media.Color colors)
        //    //{
        //    //    //List<Point3D>を使用して線を描く
        //    //    for (int i = 0; i < P3DList.Count; i++)
        //    //    {
        //    //        LV3D_List.Add(new LinesVisual3D { Points = new Point3DCollection(P3DList), Color = colors });
        //    //    }

        //    //    List<ModelVisual3D> ConvertLV3DToMV3D_List = new List<ModelVisual3D>();

        //    //    for (int LV3DCount = 0; LV3DCount < LV3D_List.Count; LV3DCount++)
        //    //    {
        //    //        //LinesVisual3DをModel3Dに変換
        //    //        Model3D LV3DToM3D = LV3D_List[LV3DCount].Content;
        //    //        ModelVisual3D M3DToMV3D = new ModelVisual3D { Content = LV3DToM3D };

        //    //        //Add
        //    //        ConvertLV3DToMV3D_List.Add(M3DToMV3D);
        //    //    }

        //    //    return ConvertLV3DToMV3D_List;
        //    //}

        //    public Model3DGroup UnionModelVisual3DGroup;
        //    public List<ModelVisual3D> InputModelVisual3DList { get; set; }

        //    /// <summary>
        //    /// List<ModelVisual3D>を1つのModelVisual3Dに結合する
        //    /// </summary>
        //    /// <param name="MV3D_List"></param>
        //    /// <returns>ModelVisual3D</returns>
        //    public ModelVisual3D UnionModelVisual3D()
        //    {
        //        for (int Count = 0; Count < InputModelVisual3DList.Count; Count++) UnionModelVisual3DGroup.Children.Add(InputModelVisual3DList[Count].Content);
        //        ModelVisual3D JoinedMV3D = new ModelVisual3D { Content = UnionModelVisual3DGroup };
        //        return JoinedMV3D;
        //    }

        //    public void UpdateModel3DGroup(Model3DGroup model3DGroup)
        //    {
        //        UnionModelVisual3DGroup = model3DGroup;
        //    }

        //    public Model3DGroup GetModel3DGroup()
        //    {
        //        return UnionModelVisual3DGroup;
        //    }

        //    public UnionModel3D(List<ModelVisual3D> InputMV3DList)
        //    {
        //        UnionModelVisual3DGroup = new Model3DGroup();
        //        InputModelVisual3DList = InputMV3DList;
        //    }
        //}

        //public class HitTestHelper
        //{
        //    public class Search
        //    {
        //        public enum HitTestType
        //        {
        //            Adorner,
        //            Geometry,
        //            Point,
        //            Ray,
        //            RayMeshGeometry3D
        //        }

        //        public static HelixToolkit.Wpf.SharpDX.HitTestResult HitTestViewport(Visual Target, System.Windows.Point Point2D, HitTestType hitTestType)
        //        {
        //            HelixToolkit.Wpf.SharpDX.HitTestResult HTR = null;
        //            HelixToolkit.Wpf.SharpDX.HitTestResult HTRs = VisualTreeHelper.HitTest(Target, Point2D);
        //            if (hitTestType == HitTestType.Adorner) HTR = HTRs as AdornerHitTestResult;
        //            if (hitTestType == HitTestType.Geometry) HTR = HTRs as GeometryHitTestResult;
        //            if (hitTestType == HitTestType.Point) HTR = HTRs as PointHitTestResult;
        //            if (hitTestType == HitTestType.Ray) HTR = HTRs as RayHitTestResult;
        //            if (hitTestType == HitTestType.RayMeshGeometry3D) HTR = HTRs as RayMeshGeometry3DHitTestResult;

        //            return HTR;
        //            //return HTRs as RayMeshGeometry3DHitTestResult;
        //        }
        //    }

        //    //public static T GetObjectName<T>(ModelVisual3D FindMV3D, HitTestResult hitTestResult, )
        //    //{
        //    //    object MDLStr_GetName = new object();
        //    //    if (typeof(ModelVisual3D) == hitTestResult.VisualHit.GetType())
        //    //    {
        //    //        //ダウンキャスト
        //    //        FindMV3D = (ModelVisual3D)hitTestResult.VisualHit;
        //    //        MDLStr_GetName = HTR.VisualHit.GetName().Split(' ');
        //    //    }
        //    //    if (typeof(LinesVisual3D) == HTR.VisualHit.GetType()) return;
        //    //    if (typeof(TubeVisual3D) == HTR.VisualHit.GetType()) return;


        //    //    return (T)MDLStr_GetName;
        //    //}

        //}

        //public class PathTools : TSRSystem
        //{
        //    public class Rail
        //    {
        //        public enum RailType
        //        {
        //            Line,
        //            Tube
        //        }

        //        public enum PointType
        //        {
        //            Model,
        //            Point3D
        //        }

        //        public List<ModelVisual3D> MV3D_List { get; set; }
        //        public List<LinesVisual3D> LV3D_List { get; set; }
        //        public List<TubeVisual3D> TV3D_List { get; set; }

        //        public Rail()
        //        {
        //            MV3D_List = new List<ModelVisual3D>();
        //            LV3D_List = new List<LinesVisual3D>();
        //            TV3D_List = new List<TubeVisual3D>();
        //        }

        //        public Rail(List<ModelVisual3D> MV3DList, List<LinesVisual3D> LV3DList, List<TubeVisual3D> TV3DList)
        //        {
        //            MV3D_List = MV3DList;
        //            LV3D_List = LV3DList;
        //            TV3D_List = TV3DList;
        //        }

        //        public List<Point3D> MV3DListToPoint3DList()
        //        {
        //            List<Point3D> point3Ds = new List<Point3D>();

        //            for (int i = 0; i < MV3D_List.Count; i++)
        //            {
        //                Model3D n = MV3D_List[i].Content;
        //                Point3D p3d = new Point3D(n.Transform.Value.OffsetX, n.Transform.Value.OffsetY, n.Transform.Value.OffsetZ);
        //                point3Ds.Add(p3d);
        //            }

        //            return point3Ds;
        //        }

        //        public List<LinesVisual3D> DrawPath_Line(UserControl1 UserCtrl, double Thickness, System.Windows.Media.Color color)
        //        {
        //            if (MV3DListToPoint3DList().Count > 1)
        //            {
        //                for (int i = 1; i < MV3DListToPoint3DList().Count; i++)
        //                {
        //                    List<Point3D> OneLine = new List<Point3D>();
        //                    OneLine.Add(MV3DListToPoint3DList()[i - 1]);
        //                    OneLine.Add(MV3DListToPoint3DList()[i]);

        //                    LinesVisual3D linesVisual3D = new LinesVisual3D
        //                    {
        //                        Points = new Point3DCollection(OneLine),
        //                        Thickness = Thickness,
        //                        Color = color
        //                    };

        //                    UserCtrl.Viewport3DX.Items.Add(linesVisual3D);

        //                    LV3D_List.Add(linesVisual3D);
        //                }
        //            }

        //            return LV3D_List;
        //        }

        //        public List<TubeVisual3D> DrawPath_Tube(UserControl1 UserCtrl, double TubeDiametor, System.Windows.Media.Color color)
        //        {
        //            if (MV3DListToPoint3DList().Count > 1)
        //            {
        //                for (int i = 1; i < MV3DListToPoint3DList().Count; i++)
        //                {
        //                    //TubeVisual3Dの直径を指定
        //                    double Diametor_Value = TubeDiametor;

        //                    TubeVisual3D tubeVisual3D = new TubeVisual3D();
        //                    tubeVisual3D.Fill = new SolidColorBrush(color);
        //                    tubeVisual3D.Path = new Point3DCollection();
        //                    tubeVisual3D.Path.Add(MV3DListToPoint3DList()[i - 1]);
        //                    tubeVisual3D.Path.Add(MV3DListToPoint3DList()[i]);
        //                    tubeVisual3D.Diameter = Diametor_Value;
        //                    tubeVisual3D.IsPathClosed = false;

        //                    TV3D_List.Add(tubeVisual3D);

        //                    //Add Tube
        //                    UserCtrl.MainViewPort.Children.Add(tubeVisual3D);
        //                }
        //            }

        //            return TV3D_List;
        //        }

        //        public void MoveRails(int MDLNum, Vector3D Pos, RailType railType)
        //        {
        //            if (railType == RailType.Line)
        //            {
        //                if (MDLNum == 0)
        //                {
        //                    LV3D_List[MDLNum].Points[0] = (Point3D)Pos;
        //                }
        //                if (MDLNum > 0 && MDLNum < LV3D_List.Count)
        //                {
        //                    LV3D_List[MDLNum - 1].Points[1] = (Point3D)Pos;
        //                    LV3D_List[MDLNum].Points[0] = (Point3D)Pos;
        //                }
        //                if (MDLNum == LV3D_List.Count)
        //                {
        //                    LV3D_List[MDLNum - 1].Points[1] = (Point3D)Pos;
        //                }
        //            }
        //            if (railType == RailType.Tube)
        //            {

        //                if (MDLNum == 0)
        //                {
        //                    TV3D_List[MDLNum].Path[0] = (Point3D)Pos;
        //                }
        //                if (MDLNum > 0 && MDLNum < TV3D_List.Count)
        //                {
        //                    TV3D_List[MDLNum - 1].Path[1] = (Point3D)Pos;
        //                    TV3D_List[MDLNum].Path[0] = (Point3D)Pos;
        //                }
        //                if (MDLNum == TV3D_List.Count)
        //                {
        //                    TV3D_List[MDLNum - 1].Path[1] = (Point3D)Pos;
        //                }
        //            }
        //        }

        //        public void DeleteRail(UserControl1 UserCtrl)
        //        {
        //            if (TV3D_List != null)
        //            {
        //                for (int TVCount = 0; TVCount < TV3D_List.Count; TVCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(TV3D_List[TVCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                TV3D_List.Clear();
        //            }

        //            if (LV3D_List != null)
        //            {
        //                for (int LVCount = 0; LVCount < LV3D_List.Count; LVCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(LV3D_List[LVCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                LV3D_List.Clear();
        //            }

        //            if (MV3D_List != null)
        //            {
        //                for (int MV3DCount = 0; MV3DCount < MV3D_List.Count; MV3DCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(MV3D_List[MV3DCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                MV3D_List.Clear();
        //            }
        //        }

        //        public void DeleteRailPoint(UserControl1 UserCtrl, int SelectedIdx, double TubeDiametor, System.Windows.Media.Color color, RailType railType)
        //        {
        //            Point3D? SelectedIndex_Next = null;
        //            Point3D? SelectedIndex_Current = null;
        //            Point3D? SelectedIndex_Prev = null;

        //            List<Point3D> point3Ds = new List<Point3D>();

        //            #region SelectedIndex_Next
        //            try
        //            {
        //                SelectedIndex_Next = new Point3D(MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetX, MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetY, MV3D_List[SelectedIdx + 1].Content.Transform.Value.OffsetZ);
        //            }
        //            catch (System.ArgumentOutOfRangeException)
        //            {
        //                SelectedIndex_Next = null;
        //            }
        //            #endregion

        //            #region SelectedIndex_Current
        //            try
        //            {
        //                SelectedIndex_Current = new Point3D(MV3D_List[SelectedIdx].Content.Transform.Value.OffsetX, MV3D_List[SelectedIdx].Content.Transform.Value.OffsetY, MV3D_List[SelectedIdx].Content.Transform.Value.OffsetZ);
        //            }
        //            catch (System.ArgumentOutOfRangeException)
        //            {
        //                SelectedIndex_Current = null;
        //            }
        //            #endregion

        //            #region SelectedIndex_Prev
        //            try
        //            {
        //                SelectedIndex_Prev = new Point3D(MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetX, MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetY, MV3D_List[SelectedIdx - 1].Content.Transform.Value.OffsetZ);
        //            }
        //            catch (System.ArgumentOutOfRangeException)
        //            {
        //                SelectedIndex_Prev = null;
        //            }
        //            #endregion

        //            if (railType == RailType.Tube)
        //            {
        //                if (SelectedIndex_Current != null)
        //                {
        //                    if ((SelectedIndex_Next == null && SelectedIndex_Prev == null) == true)
        //                    {
        //                        UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                        //MessageBox.Show("Point3D Only");
        //                    }
        //                    else if ((SelectedIndex_Next != null && SelectedIndex_Prev != null) == true)
        //                    {
        //                        point3Ds.Add(SelectedIndex_Prev.Value);
        //                        point3Ds.Add(SelectedIndex_Next.Value);

        //                        //Pointを削除
        //                        UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                        //Pointの両端に存在するTubeVisual3Dを削除
        //                        UserCtrl.MainViewPort.Children.Remove(TV3D_List[SelectedIdx]);
        //                        TV3D_List.Remove(TV3D_List[SelectedIdx]);

        //                        UserCtrl.MainViewPort.Children.Remove(TV3D_List[SelectedIdx - 1]);
        //                        TV3D_List.Remove(TV3D_List[SelectedIdx - 1]);

        //                        for (int i = 0; i < MV3D_List.Count; i++)
        //                        {
        //                            string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                            string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                            MV3D_List[i].SetName(New_MDLInfo);
        //                        }

        //                        //TubeVisual3Dの直径を指定
        //                        double Diametor_Value = TubeDiametor;

        //                        TubeVisual3D tubeVisual3D = new TubeVisual3D();
        //                        tubeVisual3D.Fill = new SolidColorBrush(color);
        //                        tubeVisual3D.Path = new Point3DCollection();
        //                        tubeVisual3D.Path.Add(point3Ds[0]);
        //                        tubeVisual3D.Path.Add(point3Ds[1]);
        //                        tubeVisual3D.Diameter = Diametor_Value;
        //                        tubeVisual3D.IsPathClosed = false;

        //                        TV3D_List.Insert(SelectedIdx - 1, tubeVisual3D);

        //                        //Add Tube
        //                        UserCtrl.MainViewPort.Children.Add(tubeVisual3D);

        //                        //MessageBox.Show("PrevPoint and NextPoint");
        //                    }
        //                    else if ((SelectedIndex_Next != null || SelectedIndex_Prev != null) == true)
        //                    {
        //                        if (SelectedIndex_Prev == null)
        //                        {
        //                            UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                            MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                            UserCtrl.MainViewPort.Children.Remove(TV3D_List[SelectedIdx]);
        //                            TV3D_List.Remove(TV3D_List[SelectedIdx]);

        //                            for (int i = 0; i < MV3D_List.Count; i++)
        //                            {
        //                                string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                                MV3D_List[i].SetName(New_MDLInfo);
        //                            }

        //                            //MessageBox.Show("PrevPoint not found : FirstPoint");
        //                        }
        //                        if (SelectedIndex_Next == null)
        //                        {
        //                            UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                            MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                            UserCtrl.MainViewPort.Children.Remove(TV3D_List[SelectedIdx - 1]);
        //                            TV3D_List.Remove(TV3D_List[SelectedIdx - 1]);

        //                            for (int i = 0; i < MV3D_List.Count; i++)
        //                            {
        //                                string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                                MV3D_List[i].SetName(New_MDLInfo);
        //                            }

        //                            //MessageBox.Show("NextPoint not found : EndPoint");
        //                        }
        //                    }
        //                }
        //            }
        //            if (railType == RailType.Line)
        //            {
        //                if (SelectedIndex_Current != null)
        //                {
        //                    if ((SelectedIndex_Next == null && SelectedIndex_Prev == null) == true)
        //                    {
        //                        UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                        //MessageBox.Show("Point3D Only");
        //                    }
        //                    else if ((SelectedIndex_Next != null && SelectedIndex_Prev != null) == true)
        //                    {
        //                        point3Ds.Add(SelectedIndex_Prev.Value);
        //                        point3Ds.Add(SelectedIndex_Next.Value);

        //                        //Pointを削除
        //                        UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                        //Pointの両端に存在するLinesVisual3Dを削除
        //                        UserCtrl.MainViewPort.Children.Remove(LV3D_List[SelectedIdx]);
        //                        LV3D_List.Remove(LV3D_List[SelectedIdx]);

        //                        UserCtrl.MainViewPort.Children.Remove(LV3D_List[SelectedIdx - 1]);
        //                        LV3D_List.Remove(LV3D_List[SelectedIdx - 1]);

        //                        for (int i = 0; i < MV3D_List.Count; i++)
        //                        {
        //                            string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                            string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                            MV3D_List[i].SetName(New_MDLInfo);
        //                        }

        //                        List<Point3D> OneLine = new List<Point3D>();
        //                        OneLine.Add(point3Ds[0]);
        //                        OneLine.Add(point3Ds[1]);

        //                        LinesVisual3D linesVisual3D = new LinesVisual3D
        //                        {
        //                            Points = new Point3DCollection(OneLine),
        //                            Thickness = TubeDiametor,
        //                            Color = color
        //                        };

        //                        LV3D_List.Insert(SelectedIdx - 1, linesVisual3D);

        //                        UserCtrl.MainViewPort.Children.Add(linesVisual3D);

        //                        //MessageBox.Show("PrevPoint and NextPoint");
        //                    }
        //                    else if ((SelectedIndex_Next != null || SelectedIndex_Prev != null) == true)
        //                    {
        //                        if (SelectedIndex_Prev == null)
        //                        {
        //                            UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                            MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                            UserCtrl.MainViewPort.Children.Remove(LV3D_List[SelectedIdx]);
        //                            LV3D_List.Remove(LV3D_List[SelectedIdx]);

        //                            for (int i = 0; i < MV3D_List.Count; i++)
        //                            {
        //                                string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                                MV3D_List[i].SetName(New_MDLInfo);
        //                            }

        //                            //MessageBox.Show("PrevPoint not found : FirstPoint");
        //                        }
        //                        if (SelectedIndex_Next == null)
        //                        {
        //                            UserCtrl.MainViewPort.Children.Remove(MV3D_List[SelectedIdx]);
        //                            MV3D_List.Remove(MV3D_List[SelectedIdx]);

        //                            UserCtrl.MainViewPort.Children.Remove(LV3D_List[SelectedIdx - 1]);
        //                            LV3D_List.Remove(LV3D_List[SelectedIdx - 1]);

        //                            for (int i = 0; i < MV3D_List.Count; i++)
        //                            {
        //                                string[] MDLInfo = MV3D_List[i].GetName().Split(' ');
        //                                string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
        //                                MV3D_List[i].SetName(New_MDLInfo);
        //                            }

        //                            //MessageBox.Show("NextPoint not found : EndPoint");
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <param name="UserCtrl"></param>
        //        /// <param name="rail"></param>
        //        /// <param name="railType"></param>
        //        public void ResetRail(UserControl1 UserCtrl, RailType railType)
        //        {
        //            if (railType == RailType.Line)
        //            {
        //                for (int i = 0; i < LV3D_List.Count; i++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(LV3D_List[i]);
        //                }

        //                LV3D_List.Clear();
        //            }
        //            if (railType == RailType.Tube)
        //            {
        //                for (int i = 0; i < TV3D_List.Count; i++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(TV3D_List[i]);
        //                }

        //                TV3D_List.Clear();
        //            }
        //        }
        //    }
        //}

        //public class Point3DSystem : TSRSystem
        //{
        //    /// <summary>
        //    /// List<DrawLine_Value>を使用してLinesVisual3Dを生成、ModelVisual3Dに変換する
        //    /// </summary>
        //    /// <param name="DrawPoint_Value_List">Point3D_List</param>
        //    /// <param name="colors">Set Color</param>
        //    /// <returns>List<ModelVisual3D>List<ModelVisual3D></returns>
        //    public ModelVisual3D DrawPointsVisual3D(Point3D DrawPoint3D, List<PointsVisual3D> PV3D_List, System.Windows.Media.Color colors, double PointSize)
        //    {
        //        List<Point3D> p3d = new List<Point3D>();
        //        p3d.Add(DrawPoint3D);
        //        PV3D_List.Add(new PointsVisual3D { Points = new Point3DCollection(p3d), Color = colors, Size = PointSize });
        //        return PV3D_List[0];
        //    }
        //}

        //public class KMP_3DCheckpointSystem : PathTools
        //{
        //    public class Checkpoint
        //    {
        //        public Rail Checkpoint_Left { get; set; }
        //        public Rail Checkpoint_Right { get; set; }
        //        public List<ModelVisual3D> SideWall_Left { get; set; }
        //        public List<ModelVisual3D> SideWall_Right { get; set; }
        //        public List<LinesVisual3D> Checkpoint_Line { get; set; }
        //        public List<TubeVisual3D> Checkpoint_Tube { get; set; }
        //        public List<ModelVisual3D> Checkpoint_SplitWallMDL { get; set; }

        //        public Checkpoint()
        //        {
        //            Checkpoint_Left = new Rail();
        //            Checkpoint_Right = new Rail();
        //            SideWall_Left = new List<ModelVisual3D>();
        //            SideWall_Right = new List<ModelVisual3D>();
        //            Checkpoint_Line = new List<LinesVisual3D>();
        //            Checkpoint_Tube = new List<TubeVisual3D>();
        //            Checkpoint_SplitWallMDL = new List<ModelVisual3D>();
        //        }

        //        public void DrawPath_SideWall(UserControl1 UserCtrl, System.Windows.Media.Color LeftWallColor, System.Windows.Media.Color RightWallColor, string LeftWallText = "SideWall -1 -1", string RightWallText = "SideWall -1 -1")
        //        {
        //            if (Checkpoint_Left.MV3DListToPoint3DList().Count > 1)
        //            {
        //                for (int i = 1; i < Checkpoint_Left.MV3DListToPoint3DList().Count; i++)
        //                {
        //                    #region Memo
        //                    //OneLine.Add(point3Ds[i - 1]); //1
        //                    //OneLine.Add(point3Ds[i]); //3
        //                    //OneLine.Add(new Point3D(point3Ds[i - 1].X, 0, point3Ds[i - 1].Z)); //0
        //                    //OneLine.Add(new Point3D(point3Ds[i].X, 0, point3Ds[i].Z)); //2
        //                    #endregion

        //                    List<Point3D> OneSiideWallLeftMDL = new List<Point3D>();
        //                    OneSiideWallLeftMDL.Add(new Point3D(Checkpoint_Left.MV3DListToPoint3DList()[i - 1].X, 0, Checkpoint_Left.MV3DListToPoint3DList()[i - 1].Z)); //0
        //                    OneSiideWallLeftMDL.Add(Checkpoint_Left.MV3DListToPoint3DList()[i - 1]); //1
        //                    OneSiideWallLeftMDL.Add(new Point3D(Checkpoint_Left.MV3DListToPoint3DList()[i].X, 0, Checkpoint_Left.MV3DListToPoint3DList()[i].Z)); //2
        //                    OneSiideWallLeftMDL.Add(Checkpoint_Left.MV3DListToPoint3DList()[i]); //3

        //                    ModelVisual3D SideWallLeft_MV3D = CustomModelCreateHelper.CustomRectanglePlane3D(CustomModelCreateHelper.ToPoint3DCollection(OneSiideWallLeftMDL), LeftWallColor, LeftWallColor);
        //                    HTK_3DES.TSRSystem.SetString_MV3D(SideWallLeft_MV3D, LeftWallText);
        //                    UserCtrl.MainViewPort.Children.Add(SideWallLeft_MV3D);
        //                    SideWall_Left.Add(SideWallLeft_MV3D);
        //                }
        //            }
        //            if (Checkpoint_Right.MV3DListToPoint3DList().Count > 1)
        //            {
        //                for (int i = 1; i < Checkpoint_Right.MV3DListToPoint3DList().Count; i++)
        //                {
        //                    #region Memo
        //                    //OneLine.Add(point3Ds[i - 1]); //1
        //                    //OneLine.Add(point3Ds[i]); //3
        //                    //OneLine.Add(new Point3D(point3Ds[i - 1].X, 0, point3Ds[i - 1].Z)); //0
        //                    //OneLine.Add(new Point3D(point3Ds[i].X, 0, point3Ds[i].Z)); //2
        //                    #endregion

        //                    List<Point3D> OneSiideWallRightMDL = new List<Point3D>();
        //                    OneSiideWallRightMDL.Add(new Point3D(Checkpoint_Right.MV3DListToPoint3DList()[i - 1].X, 0, Checkpoint_Right.MV3DListToPoint3DList()[i - 1].Z)); //0
        //                    OneSiideWallRightMDL.Add(Checkpoint_Right.MV3DListToPoint3DList()[i - 1]); //1
        //                    OneSiideWallRightMDL.Add(new Point3D(Checkpoint_Right.MV3DListToPoint3DList()[i].X, 0, Checkpoint_Right.MV3DListToPoint3DList()[i].Z)); //2
        //                    OneSiideWallRightMDL.Add(Checkpoint_Right.MV3DListToPoint3DList()[i]); //3

        //                    ModelVisual3D SideWallRight_MV3D = CustomModelCreateHelper.CustomRectanglePlane3D(CustomModelCreateHelper.ToPoint3DCollection(OneSiideWallRightMDL), RightWallColor, RightWallColor);
        //                    HTK_3DES.TSRSystem.SetString_MV3D(SideWallRight_MV3D, RightWallText);
        //                    UserCtrl.MainViewPort.Children.Add(SideWallRight_MV3D);
        //                    SideWall_Right.Add(SideWallRight_MV3D);
        //                }
        //            }
        //        }

        //        public enum SideWallType
        //        {
        //            Left,
        //            Right
        //        }

        //        public void MoveSideWalls(int MDLNum, Vector3D Pos, SideWallType sideWallType)
        //        {
        //            List<ModelVisual3D> ModelVisual3D_List = null;
        //            if (sideWallType == SideWallType.Left) ModelVisual3D_List = SideWall_Left;
        //            if (sideWallType == SideWallType.Right) ModelVisual3D_List = SideWall_Right;

        //            if (MDLNum == 0)
        //            {
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[0] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[1] = (Point3D)Pos;
        //            }
        //            if (MDLNum > 0 && MDLNum < ModelVisual3D_List.Count)
        //            {
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[2] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[3] = (Point3D)Pos;

        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[0] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum].Content).Positions[1] = (Point3D)Pos;
        //            }
        //            if (MDLNum == ModelVisual3D_List.Count)
        //            {
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[2] = new Point3D(((Point3D)Pos).X, 0, ((Point3D)Pos).Z);
        //                HTK_3DES.OBJData.GetMeshGeometry3D(ModelVisual3D_List[MDLNum - 1].Content).Positions[3] = (Point3D)Pos;
        //            }
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <param name="UserCtrl"></param>
        //        /// <param name="rail"></param>
        //        /// <param name="railType"></param>
        //        public void ResetSideWall(UserControl1 UserCtrl)
        //        {
        //            for (int i = 0; i < SideWall_Left.Count; i++) UserCtrl.MainViewPort.Children.Remove(SideWall_Left[i]);
        //            SideWall_Left.Clear();

        //            for (int i = 0; i < SideWall_Right.Count; i++) UserCtrl.MainViewPort.Children.Remove(SideWall_Right[i]);
        //            SideWall_Right.Clear();
        //        }

        //        public void DeleteRailChk(UserControl1 UserCtrl)
        //        {
        //            if (Checkpoint_Left.MV3D_List != null)
        //            {
        //                for (int ChkLeftCount = 0; ChkLeftCount < Checkpoint_Left.MV3D_List.Count; ChkLeftCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Left.MV3D_List[ChkLeftCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Left.MV3D_List.Clear();
        //            }
        //            if (Checkpoint_Right.MV3D_List != null)
        //            {
        //                for (int ChkRightCount = 0; ChkRightCount < Checkpoint_Right.MV3D_List.Count; ChkRightCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Right.MV3D_List[ChkRightCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Right.MV3D_List.Clear();
        //            }
        //            if (Checkpoint_Line != null)
        //            {
        //                for (int ChkLineCount = 0; ChkLineCount < Checkpoint_Line.Count; ChkLineCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Line[ChkLineCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Line.Clear();
        //            }
        //            if (Checkpoint_Tube != null)
        //            {
        //                for (int ChkTubeCount = 0; ChkTubeCount < Checkpoint_Tube.Count; ChkTubeCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Tube[ChkTubeCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Tube.Clear();
        //            }
        //            if (Checkpoint_Left.LV3D_List != null)
        //            {
        //                for (int ChkRLineLeftCount = 0; ChkRLineLeftCount < Checkpoint_Left.LV3D_List.Count; ChkRLineLeftCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Left.LV3D_List[ChkRLineLeftCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Left.LV3D_List.Clear();
        //            }
        //            if (Checkpoint_Right.LV3D_List != null)
        //            {
        //                for (int ChkRLineRightCount = 0; ChkRLineRightCount < Checkpoint_Right.LV3D_List.Count; ChkRLineRightCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_Right.LV3D_List[ChkRLineRightCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_Right.LV3D_List.Clear();
        //            }
        //            if (Checkpoint_SplitWallMDL != null)
        //            {
        //                for (int ChkSplitWallCount = 0; ChkSplitWallCount < Checkpoint_SplitWallMDL.Count; ChkSplitWallCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(Checkpoint_SplitWallMDL[ChkSplitWallCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                Checkpoint_SplitWallMDL.Clear();
        //            }
        //            if (SideWall_Left != null)
        //            {
        //                for (int ChkSideWallLeftCount = 0; ChkSideWallLeftCount < SideWall_Left.Count; ChkSideWallLeftCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(SideWall_Left[ChkSideWallLeftCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                SideWall_Left.Clear();
        //            }
        //            if (SideWall_Right != null)
        //            {
        //                for (int ChkSideWallRightCount = 0; ChkSideWallRightCount < SideWall_Right.Count; ChkSideWallRightCount++)
        //                {
        //                    UserCtrl.MainViewPort.Children.Remove(SideWall_Right[ChkSideWallRightCount]);
        //                    UserCtrl.UpdateLayout();
        //                }

        //                SideWall_Right.Clear();
        //            }
        //        }
        //    }
        //}

        public class OBJData
        {
            public string FilePath { get; set; }
            public List<Object3D> Object3DList { get; set; }
            #pragma warning disable CS0618
            public ObjReader ObjReader { get; set; }

            public void Read()
            {
                Object3DList = ObjReader.Read(FilePath);
            }

            public OBJData(string Path)
            {
                FilePath = Path;
                ObjReader = new ObjReader();
            }

            public static Matrix ReScale(Matrix Matrix, float ScaleFactor)
            {
                Matrix M = Matrix;
                M.M11 = M.M11 / ScaleFactor;
                M.M22 = M.M22 / ScaleFactor;
                M.M33 = M.M33 / ScaleFactor;
                return M;
            }

            public static Matrix3D ReScale(Matrix3D Matrix_3D, double ScaleFactor)
            {
                Matrix3D M = Matrix_3D;
                M.M11 = M.M11 / ScaleFactor;
                M.M22 = M.M22 / ScaleFactor;
                M.M33 = M.M33 / ScaleFactor;
                return M;
            }

            //public static SceneNodeGroupModel3D ToSceneNodeGroupModel3D(List<SceneNode> sceneNodes)
            //{
            //    SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
            //    foreach (var d in sceneNodes) sceneNodeGroupModel3D.AddNode(d);
            //    return sceneNodeGroupModel3D;
            //}

            //public static SceneNodeGroupModel3D ToSceneNodeGroupModel3D(List<MeshGeometryModel3D> meshGeometryModel3Ds)
            //{
            //    SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
            //    foreach (var d in meshGeometryModel3Ds) sceneNodeGroupModel3D.AddNode((SceneNode)d);
            //    return sceneNodeGroupModel3D;
            //}


            public enum Type
            {
                MeshNode,
                MeshGeometryModel3D
            }


            /// <summary>
            /// objファイルを読み込み、ModelVisual3Dを返すメソッド
            /// </summary>
            /// <param name="Path">Model Path</param>
            /// <returns>ModelVisual3D</returns>
            public static List<SceneNode> OBJReader(string Path, Type type = Type.MeshNode)
            {
                #pragma warning disable CS0618
                ObjReader objRead = new ObjReader();
                List<Object3D> ObjModelList = objRead.Read(Path);

                List<SceneNode> sceneNodes = new List<SceneNode>();
                foreach (var m in ObjModelList)
                {
                    if (type == Type.MeshNode)
                    {
                        MeshNode meshNode = new MeshNode { Geometry = m.Geometry, Material = m.Material };
                        sceneNodes.Add(meshNode);
                    }
                    if (type == Type.MeshGeometryModel3D)
                    {
                        MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D { Geometry = m.Geometry, Material = m.Material.ConvertToMaterial() };
                        sceneNodes.Add((SceneNode)meshGeometryModel3D);
                    }
                }

                return sceneNodes;
            }

            /// <summary>
            /// ModelVisual3D or ArrayList
            /// </summary>
            /// <param name="Path"></param>
            /// <returns></returns>
            public static Dictionary<string, SceneNode> OBJReader_Dictionary(string Path, Type type = Type.MeshNode)
            {
                Dictionary<string, SceneNode> Model_Dictionary = new Dictionary<string, SceneNode>();

                List<Object3D> M3D_Group = null;
                ObjReader OBJ_Reader = new ObjReader();
                M3D_Group = OBJ_Reader.Read(Path);

                for (int MDLCount = 0; MDLCount < M3D_Group.Count; MDLCount++)
                {
                    if (type == Type.MeshNode)
                    {
                        MeshNode meshNode = new MeshNode { Geometry = M3D_Group[MDLCount].Geometry, Material = M3D_Group[MDLCount].Material };

                        meshNode.ModelMatrix = ReScale(meshNode.ModelMatrix, 100);

                        //MV3D.Transform = new MatrixTransform3D(ReScale(MV3D.Content.Transform.Value, 100));

                        string MatName = M3D_Group[MDLCount].Material.Name;

                        //Give a name to MeshNode
                        meshNode.Name = MatName;

                        if (Model_Dictionary.Keys.Contains(MatName) && Model_Dictionary.Values.Contains(meshNode))
                        {
                            //マテリアルの名前が同じだった場合
                            Model_Dictionary.Add(MatName + MDLCount, meshNode);
                        }
                        else
                        {
                            Model_Dictionary.Add(MatName, meshNode);
                        }
                    }
                    if (type == Type.MeshGeometryModel3D)
                    {
                        MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D { Geometry = M3D_Group[MDLCount].Geometry, Material = M3D_Group[MDLCount].Material.ConvertToMaterial() };

                        meshGeometryModel3D.Transform = new MatrixTransform3D(ReScale(meshGeometryModel3D.Transform.Value, 100));

                        //MV3D.Transform = new MatrixTransform3D(ReScale(MV3D.Content.Transform.Value, 100));

                        string MatName = M3D_Group[MDLCount].Material.Name;

                        //Give a name to MeshGeometryModel3D
                        meshGeometryModel3D.Name = MatName;

                        if (Model_Dictionary.Keys.Contains(MatName) && Model_Dictionary.Values.Contains((SceneNode)meshGeometryModel3D))
                        {
                            //マテリアルの名前が同じだった場合
                            Model_Dictionary.Add(MatName + MDLCount, (SceneNode)meshGeometryModel3D);
                        }
                        else
                        {
                            Model_Dictionary.Add(MatName, (SceneNode)meshGeometryModel3D);
                        }
                    }
                }

                return Model_Dictionary;
            }

            //public static Dictionary<string, ArrayList> OBJReader_AryListDictionary(string Path)
            //{
            //    Dictionary<string, ArrayList> MV3D_Dictionary = new Dictionary<string, ArrayList>();

            //    List<Object3D> M3D_Group = null;
            //    ObjReader OBJ_Reader = new ObjReader();
            //    M3D_Group = OBJ_Reader.Read(Path);

            //    for (int MDLChildCount = 0; MDLChildCount < M3D_Group.Count; MDLChildCount++)
            //    {
            //        Model3D NewM3D = M3D_Group.Children[MDLChildCount];
            //        ModelVisual3D MV3D = new ModelVisual3D { Content = NewM3D };

            //        //MV3D.Transform = new MatrixTransform3D(ReScale(MV3D.Content.Transform.Value, 100));

            //        MeshGeometryModel3D meshGeometryModel3D = 

            //        GeometryModel3D GM3D = (GeometryModel3D)M3D_Group.Children[MDLChildCount];
            //        string MatName = GM3D.Material.GetName();

            //        //ModelVisual3Dに名前をつける
            //        MV3D.SetName(MatName + " -1 -1");

            //        ArrayList arrayList = new ArrayList();
            //        arrayList.Add(false);
            //        arrayList.Add(MV3D);


            //        if (MV3D_Dictionary.Keys.Contains(MatName) && MV3D_Dictionary.Values.Contains(arrayList))
            //        {
            //            //マテリアルの名前が同じだった場合
            //            MV3D_Dictionary.Add(MatName + MDLChildCount, arrayList);
            //        }
            //        else
            //        {
            //            MV3D_Dictionary.Add(MatName, arrayList);
            //        }
            //    }

            //    return MV3D_Dictionary;
            //}

            public static MeshNode GetMeshNode(SceneNode sceneNode)
            {
                return (MeshNode)sceneNode;
            }

            public static MeshGeometryModel3D GetMeshGeometryModel3D(SceneNode sceneNode)
            {
                return (MeshGeometryModel3D)((MeshNode)sceneNode);
            }
        }

        public class CustomMeshBuildHelper
        {
            ModelVisual3D Base;
            Model3DGroup Model3DGroup;

            public Mesh MeshData;
            public class Mesh
            {
                public MeshBuilder MeshBuilder { get; set; }

                public void AddTriangleIndices(int Indices)
                {
                    MeshBuilder.TriangleIndices.Add(Indices);
                }

                public void AddTriangleIndicesArray(int[] IndicesArray)
                {
                    foreach (var Indices in IndicesArray) MeshBuilder.TriangleIndices.Add(Indices);
                }

                public void Add(Vector3 Vertex, Vector3 Normal, Vector3 Tangent, Vector2 TextureCoord)
                {
                    MeshBuilder.Positions.Add(Vertex);
                    MeshBuilder.Normals.Add(Normal);
                    MeshBuilder.Tangents.Add(Tangent);
                    MeshBuilder.TextureCoordinates.Add(TextureCoord);
                }

                public Mesh(bool GenerateNormal, bool GenerateTexCoord, bool TangentSpace)
                {
                    MeshBuilder = new MeshBuilder(GenerateNormal, GenerateTexCoord, TangentSpace);
                }

                public MeshGeometry3D ToMeshGeometry3D()
                {
                    return MeshBuilder.ToMeshGeometry3D();
                    //return MeshBuilder.ToMesh();
                }

                //public void Test()
                //{
                //    Mesh mesh = new Mesh(true, true, true);
                //    mesh.AddTriangleIndicesArray(new int[] { 0, 1, 2 });

                //    List<Polygon> polygons = new List<Polygon>();
                //    for (int i = 0; i < polygons.Count; i++) mesh.Add(polygons[i].Vertex, polygons[i].Normal, polygons[i].TexCoord.ToPoint());
                //    var g = mesh.ToMeshGeometry3D();




                //    CustomMeshNode meshNode = new CustomMeshNode(g, new List<HelixToolkit.Wpf.SharpDX.Material>(), new Matrix(), CullMode.None);
                //    meshNode.ChangeMaterial(3);

                //    //MeshNode meshNode = new MeshNode { Geometry = g, }

                //}
            }

            public CustomMeshNode CustomMeshNodes { get; set; }
            public class CustomMeshNode
            {
                public List<HelixToolkit.Wpf.SharpDX.Material> Materials;
                public MeshNode BaseMeshNode;

                /// <summary>
                /// Change Material
                /// </summary>
                /// <param name="MaterialIndex"></param>
                public void ChangeMaterial(int MaterialIndex)
                {
                    BaseMeshNode.Material = Materials[MaterialIndex];
                }

                public CustomMeshNode(MeshGeometry3D InputGeometry, List<HelixToolkit.Wpf.SharpDX.Material> materials, Matrix matrix, CullMode cullMode = CullMode.Back)
                {
                    Materials = materials;
                    BaseMeshNode = new MeshNode { Geometry = InputGeometry, Material = materials[0], ModelMatrix = matrix, CullMode = cullMode };
                }
            }

            public class MaterialSet
            {
                public List<HelixToolkit.Wpf.SharpDX.Material> MaterialList { get; set; }

                public enum MaterialType
                {
                    Phong = 0,
                    Diffuse = 1,
                    Normal = 2,
                    Speculer = 3,
                    VertexColor = 4,
                    ColorStripe = 5,
                    Default
                }


                public Dictionary<string, ArrayList> ToNameDictionary()
                {
                    Dictionary<string, ArrayList> Dict = new Dictionary<string, ArrayList>();
                    foreach (var m in MaterialList)
                    {
                        ArrayList arrayList = new ArrayList();
                        if (m is HelixToolkit.Wpf.SharpDX.PhongMaterial) arrayList.AddRange(new object[] { MaterialType.Phong, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.DiffuseMaterial) arrayList.AddRange(new object[] { MaterialType.Diffuse, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.NormalMaterial) arrayList.AddRange(new object[] { MaterialType.Normal, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.VertColorMaterial) arrayList.AddRange(new object[] { MaterialType.VertexColor, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.Material) arrayList.AddRange(new object[] { MaterialType.Default, m });
                        Dict.Add(m.Name, arrayList);
                    }
                    return Dict;
                }

                public Dictionary<int, ArrayList> ToIndexDictionary()
                {
                    int idx = 0;

                    Dictionary<int, ArrayList> Dict = new Dictionary<int, ArrayList>();
                    foreach (var m in MaterialList)
                    {
                        ArrayList arrayList = new ArrayList();
                        if (m is HelixToolkit.Wpf.SharpDX.PhongMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Phong, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.DiffuseMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Diffuse, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.NormalMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Normal, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.VertColorMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.VertexColor, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.Material) arrayList.AddRange(new object[] { m.Name, MaterialType.Default, m });
                        else if (m is HelixToolkit.Wpf.SharpDX.ColorStripeMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.ColorStripe, m });
                        Dict.Add(idx, arrayList);

                        idx++;
                    }
                    return Dict;
                }

                public MaterialSet(List<HelixToolkit.Wpf.SharpDX.Material> InputMarterialList)
                {
                    MaterialList = InputMarterialList;
                }


                //public Dictionary<string, ArrayList> ToNameDictionary()
                //{
                //    Dictionary<string, ArrayList> Dict = new Dictionary<string, ArrayList>();
                //    foreach (var m in MaterialList)
                //    {
                //        ArrayList arrayList = new ArrayList();
                //        if (m is HelixToolkit.Wpf.SharpDX.PhongMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Phong, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.DiffuseMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Diffuse, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.NormalMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Normal, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.VertColorMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.VertexColor, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.Material) arrayList.AddRange(new object[] { m.Name, MaterialType.Default, m });
                //        Dict.Add(m.Name, arrayList);
                //    }
                //    return Dict;
                //}

                //public Dictionary<int, ArrayList> ToIndexDictionary()
                //{
                //    int idx = 0;

                //    Dictionary<int, ArrayList> Dict = new Dictionary<int, ArrayList>();
                //    foreach (var m in MaterialList)
                //    {
                //        ArrayList arrayList = new ArrayList();
                //        if (m is HelixToolkit.Wpf.SharpDX.PhongMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Phong, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.DiffuseMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Diffuse, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.NormalMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.Normal, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.VertColorMaterial) arrayList.AddRange(new object[] { m.Name, MaterialType.VertexColor, m });
                //        if (m is HelixToolkit.Wpf.SharpDX.Material) arrayList.AddRange(new object[] { m.Name, MaterialType.Default, m });
                //        Dict.Add(m.Name, arrayList);
                //    }
                //    return Dict;
                //}



                //public Dictionary<string, HelixToolkit.Wpf.SharpDX.Material> ToNameDictionary()
                //{
                //    Dictionary<string, HelixToolkit.Wpf.SharpDX.Material> Dict = new Dictionary<string, HelixToolkit.Wpf.SharpDX.Material>();
                //    foreach (var m in MaterialList) Dict.Add(m.Name, m);
                //    return Dict;
                //}

                //public Dictionary<int, HelixToolkit.Wpf.SharpDX.Material> ToIndexDictionary()
                //{
                //    Dictionary<int, HelixToolkit.Wpf.SharpDX.Material> Dict = new Dictionary<int, HelixToolkit.Wpf.SharpDX.Material>();
                //    foreach (var m in MaterialList) Dict.Add(m.Name, m);
                //    return Dict;
                //}

                //public enum DictionaryType
                //{
                //    Index,
                //    Name
                //}

                //public Dictionary<T, HelixToolkit.Wpf.SharpDX.Material> ToDictionary<T>()
                //{
                //    Dictionary<T, HelixToolkit.Wpf.SharpDX.Material> Dict = new Dictionary<T, HelixToolkit.Wpf.SharpDX.Material>();
                //    foreach (var m in MaterialList) Dict.Add(m.Name, m);
                //    return Dict;
                //}

                #region BackUp
                //public Setting Settings { get; set; }
                //public class Setting
                //{
                //    public TextureModel DiffuseMap { get; set; }
                //    public TextureModel DiffuseAlphaMap { get; set; }


                //    PhongMaterial PhongMaterial = new PhongMaterial
                //    {
                //        DiffuseMap = null,
                //        DiffuseAlphaMap = null,
                //        DiffuseColor = new Color4(),
                //        DiffuseMapSampler = new SamplerStateDescription { },
                        
                //        UVTransform = new UVTransform()
                //    };

                //    public Stream[] ImageStreamArray { get; set; }

                //}

                //public SamplerStateDescription SamplerStateDescriptions()
                //{
                //    SamplerStateDescription samplerStateDescription = new SamplerStateDescription
                //    {
                //        AddressU = TextureAddressMode.Wrap,
                //        AddressV = TextureAddressMode.Wrap,
                //        AddressW = TextureAddressMode.Wrap,
                //        MaximumAnisotropy = 1,
                //        Filter = SharpDX.Direct3D11.Filter.
                //    };

                //    return samplerStateDescription;
                //}

                //public UVTransform UVTransforms()
                //{
                //    UVTransform UVTransformData = new UVTransform { Rotation = 0, Scaling = new Vector2(1, 1), Translation = new Vector2(0, 0) };

                //    //    Matrix matrix = new Matrix();
                //    //    if (Type == 0) matrix = new Matrix(-1, 0, 0, -1, 0, 0); //Default(?)
                //    //    if (Type == 1) matrix = new Matrix(-1, 0, 0, -1, 0, 0);
                //    //    if (Type == 2) matrix = new Matrix(1, 0, 0, 1, 0, 0);
                //    //    if (Type == 3) matrix = new Matrix(0, -1, -1, 0, 0, 0);

                //    return UVTransformData;
                //}
                #endregion

                //public PhongMaterial PhongMaterial;

                //public void CreatePhongMaterial()
                //{
                //    PhongMaterial = new PhongMaterial
                //    {

                //    }
                //}
            }

            //public MaterialSet MaterialSetData;
            //public class MaterialSet
            //{

            //    Obse
            //    public PhongMaterialCollection PhongMaterialGroup { get; set; }

            //    public HelixToolkit.Wpf.SharpDX.Material Materialsf(System.Windows.Media.Media3D.MaterialGroup materialGroup)
            //    {
            //        MeshGeometryModel3D meshGeometryModel3D = new MeshGeometryModel3D();
            //        PhongMaterial PhongMaterialCollection = (PhongMaterial)meshGeometryModel3D.Material;

            //        HelixToolkit.Wpf.SharpDX.Material kl = PhongMaterialGroup[0].Core.ConvertToMaterial();

            //        var fw = (HelixToolkit.Wpf.SharpDX.Material)PhongMaterialGroup.;
            //        var u = (HelixToolkit.Wpf.SharpDX.Material)PhongMaterialCollection;


            //        var q = (PhongMaterial)(m0 = meshGeometryModel3D.Material);
            //    }


            //    public void AddMaterial(HelixToolkit.Wpf.SharpDX.PhongMaterial material)
            //    {
            //        PhongMaterialGroup.Add(material);
            //    }

            //    public HelixToolkit.Wpf.SharpDX.Material ToMaterial()
            //    {
            //        HelixToolkit.Wpf.SharpDX.Material qw = new HelixToolkit.Wpf.SharpDX.PhongMaterial();

            //        List<HelixToolkit.Wpf.SharpDX.Material> materials = new List<HelixToolkit.Wpf.SharpDX.Material>();

            //        HelixToolkit.Wpf.SharpDX.Model.MaterialCore reg;
            //        foreach (var q in materials)
            //        {
            //            reg.;
            //        }


            //        HelixToolkit.Wpf.SharpDX.Material ma = new HelixToolkit.Wpf.SharpDX.Material;

            //        HelixToolkit.Wpf.SharpDX.Material qw2 = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial(new DiffuseMaterialCore { DiffuseMapSampler });

            //        MaterialCore materialCore = (MaterialCore)qw;

            //        return new HelixToolkit.Wpf.SharpDX.Material()
            //        //return PhongMaterialGroup.ToList().Union();
            //    }

            //    public MaterialSet()
            //    {
            //        MaterialGroup = new MaterialGroup();
            //    }

            //    public List<System.Windows.Media.Media3D.Material> ToList()
            //    {
            //        return MaterialGroup.Children.ToList();
            //    }

            //    public static T operator +()

            //    public static HelixToolkit.Wpf.SharpDX.DiffuseMaterial Union(DiffuseMaterialCollection diffuseMaterials)
            //    {
            //        HelixToolkit.Wpf.SharpDX.DiffuseMaterial diffuseMaterial = new HelixToolkit.Wpf.SharpDX.DiffuseMaterial();

            //        foreach (var y in diffuseMaterials)
            //        {
            //            TextureModel texture = new TextureModel("texture.png");

            //            Mater


            //            TextureModel textureModel = new TextureModel()


            //            diffuseMaterial.DiffuseMap += y.DiffuseMap;
            //        }
            //    }

            //    //public void Test()
            //    //{
            //    //    MaterialSet materialSet = new MaterialSet();
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.DiffuseMaterial());
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.SpecularMaterial());
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.Material)

            //    //    Material material = materialSet.ToMaterial();
            //    //}
            //}


            //public MaterialSet MaterialSetData;
            //public class MaterialSet
            //{
            //    public MaterialGroup MaterialGroup { get; set; }

            //    public void AddMaterial(System.Windows.Media.Media3D.Material material)
            //    {
            //        MaterialGroup.Children.Add(material);
            //    }

            //    public System.Windows.Media.Media3D.Material ToMaterial()
            //    {
            //        return MaterialGroup;
            //    }

            //    public MaterialSet()
            //    {
            //        MaterialGroup = new MaterialGroup();
            //    }

            //    public List<System.Windows.Media.Media3D.Material> ToList()
            //    {
            //        return MaterialGroup.Children.ToList();
            //    }



            //    //public void Test()
            //    //{
            //    //    MaterialSet materialSet = new MaterialSet();
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.DiffuseMaterial());
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.SpecularMaterial());
            //    //    materialSet.AddMaterial(new System.Windows.Media.Media3D.Material)

            //    //    Material material = materialSet.ToMaterial();
            //    //}
            //}

            public class Texture
            {
                public Bitmap BitmapSource { get; set; }
                public Stream BitmapStream => ToBitmapStream();

                public TextureModel ToTextureModel(bool b)
                {
                    return new TextureModel(BitmapStream, b);
                }

                //Create Stream (from Bitmap)
                public Stream ToBitmapStream()
                {
                    Stream stream = BitmapToMemoryStream(BitmapSource);
                    return stream;
                }

                /// <summary>
                /// To MemoryStream
                /// </summary>
                /// <param name="bitmap">Input Bitmap</param>
                /// <returns>MemoryStream</returns>
                public static MemoryStream BitmapToMemoryStream(Bitmap bitmap)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    return memoryStream;
                }

                /// <summary>
                /// Bitmap[] to Bitmap
                /// </summary>
                /// <param name="bitmaps">Input Bitmap[]</param>
                /// <returns></returns>
                public static Bitmap BitmapArrayToBitmap(Bitmap[] bitmaps)
                {
                    Bitmap baseBitmap = new Bitmap(bitmaps[0]);
                    Graphics g = Graphics.FromImage(baseBitmap);
                    foreach (var i in bitmaps) g.DrawImage(i, 0, 0, i.Width, i.Height);
                    g.Dispose();
                    baseBitmap.Dispose();
                    return baseBitmap;
                }

                #region Unused(?)
                public static MemoryStream[] BitmapArrayToMemoryStreamArray(Bitmap[] bitmaps)
                {
                    List<MemoryStream> memoryStreams = new List<MemoryStream>();
                    foreach(var bitmap in bitmaps)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStreams.Add(memoryStream);
                    }

                    return memoryStreams.ToArray();
                }
                #endregion




                //public ImageBrush ToImageBrush(double TextureScaleX, double TextureScaleY, int Type)
                //{
                //    Matrix matrix = new Matrix();
                //    if (Type == 0) matrix = new Matrix(-1, 0, 0, -1, 0, 0); //Default(?)
                //    if (Type == 1) matrix = new Matrix(-1, 0, 0, -1, 0, 0);
                //    if (Type == 2) matrix = new Matrix(1, 0, 0, 1, 0, 0);
                //    if (Type == 3) matrix = new Matrix(0, -1, -1, 0, 0, 0);

                //    ImageBrush imageBrush = new ImageBrush
                //    {
                //        ImageSource = BitmapImage,
                //        TileMode = TileMode.Tile,
                //        Stretch = Stretch.Fill,
                //        ViewportUnits = BrushMappingMode.Absolute,
                //        ViewboxUnits = BrushMappingMode.RelativeToBoundingBox,
                //        AlignmentX = AlignmentX.Center,
                //        AlignmentY = AlignmentY.Center,
                //        RelativeTransform = new MatrixTransform { Matrix = matrix },
                //        //Viewbox = new Rect(0, 0, TextureScaleX, TextureScaleY),
                //        Viewport = new Rect(0, 0, TextureScaleX, TextureScaleY) //X=1, Y=1
                //    };

                //    return imageBrush;
                //}

                public Texture(Bitmap bitmap)
                {
                    BitmapSource = bitmap;
                }

                //public void Test()
                //{
                //    //Bitmap[] to TextureModel
                //    Texture texture0 = new Texture(BitmapArrayToBitmap(new List<Bitmap>().ToArray()));
                //    TextureModel textureModel0 = texture0.ToTextureModel();

                //    //Bitmap to TextureModel
                //    Texture texture1 = new Texture(new Bitmap(0, 0));
                //    TextureModel textureModel1 = texture1.ToTextureModel();
                //}
            }
        }

        //public class CustomModelCreateHelper
        //{
        //    public static List<Point3D> DefaultBoxData()
        //    {
        //        List<Point3D> point3Ds = new List<Point3D>();

        //        #region d1
        //        point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));
        //        point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));

        //        point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));
        //        point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));

        //        point3Ds.Add(new Point3D(0.5, -0.5, 0.5));
        //        point3Ds.Add(new Point3D(0.5, 0.5, 0.5));

        //        point3Ds.Add(new Point3D(0.5, 0.5, -0.5));
        //        point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
        //        #endregion

        //        #region d2
        //        point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
        //        point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));

        //        point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));
        //        point3Ds.Add(new Point3D(0.5, -0.5, 0.5));

        //        point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
        //        point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));

        //        point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));
        //        point3Ds.Add(new Point3D(0.5, 0.5, -0.5));
        //        #endregion

        //        #region d3
        //        point3Ds.Add(new Point3D(-0.5, -0.5, -0.5));
        //        point3Ds.Add(new Point3D(-0.5, -0.5, 0.5));

        //        point3Ds.Add(new Point3D(-0.5, 0.5, 0.5));
        //        point3Ds.Add(new Point3D(-0.5, 0.5, -0.5));

        //        point3Ds.Add(new Point3D(0.5, 0.5, 0.5));
        //        point3Ds.Add(new Point3D(0.5, 0.5, -0.5));

        //        point3Ds.Add(new Point3D(0.5, -0.5, 0.5));
        //        point3Ds.Add(new Point3D(0.5, -0.5, -0.5));
        //        #endregion

        //        return point3Ds;
        //    }

        //    public static Point3DCollection ToPoint3DCollection(List<Point3D> point3DList)
        //    {
        //        Point3DCollection P3DCollection = new Point3DCollection();
        //        for (int i = 0; i < point3DList.Count; i++) P3DCollection.Add(point3DList[i]);
        //        return P3DCollection;
        //    }

        //    public static List<Point3D> ToPoint3DList(Point3DCollection P3DCollection)
        //    {
        //        List<Point3D> point3DList = new List<Point3D>();
        //        for (int i = 0; i < P3DCollection.Count; i++) point3DList.Add(P3DCollection[i]);
        //        return point3DList;
        //    }

        //    public static MeshBuilder AddPoint3DList(List<Point3D> point3Ds)
        //    {
        //        var msb = new MeshBuilder();
        //        for (int i = 0; i < point3Ds.Count; i++) msb.Positions.Add(point3Ds[i]);
        //        return msb;
        //    }

        //    public static MeshBuilder AddPoint3DCollection(Point3DCollection P3DCollection)
        //    {
        //        var msb = new MeshBuilder();
        //        for (int i = 0; i < P3DCollection.Count; i++) msb.Positions.Add(P3DCollection[i]);
        //        return msb;
        //    }

        //    public static ModelVisual3D MeshGeometryToModelVisual3D(Point3DCollection point3Ds, System.Windows.Media.Color color, System.Windows.Media.Color Backcolor, string MDLName = null)
        //    {
        //        GeometryModel3D mesh = new GeometryModel3D
        //        {
        //            Geometry = new MeshGeometry3D
        //            {
        //                Positions = point3Ds
        //            },
        //            Material = MaterialHelper.CreateMaterial(color),
        //            BackMaterial = MaterialHelper.CreateMaterial(Backcolor)
        //        };

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = mesh };
        //        modelVisual3D.SetName(MDLName);

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D MeshGeometryToModelVisual3D(Point3DCollection point3Ds, Int32Collection TriangleIndicesSetting, System.Windows.Media.Color color, System.Windows.Media.Color Backcolor, string MDLName = null)
        //    {
        //        GeometryModel3D mesh = new GeometryModel3D
        //        {
        //            Geometry = new MeshGeometry3D
        //            {
        //                Positions = point3Ds,
        //                TriangleIndices = TriangleIndicesSetting
        //            },
        //            Material = MaterialHelper.CreateMaterial(color),
        //            BackMaterial = MaterialHelper.CreateMaterial(Backcolor)
        //        };

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = mesh };
        //        modelVisual3D.SetName(MDLName);

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CreateWireBoxLine()
        //    {
        //        LinesVisual3D linesVisual3D = new LinesVisual3D();
        //        linesVisual3D.Thickness = 5;
        //        linesVisual3D.Points = ToPoint3DCollection(DefaultBoxData());
        //        ModelVisual3D modelVisual3D = linesVisual3D;
        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CreateWireBoxTube()
        //    {
        //        TubeVisual3D tubeVisual3D = new TubeVisual3D();
        //        tubeVisual3D.Diameter = 0.04;
        //        tubeVisual3D.Path = ToPoint3DCollection(DefaultBoxData());
        //        tubeVisual3D.IsPathClosed = false;
        //        ModelVisual3D modelVisual3D = tubeVisual3D;
        //        return modelVisual3D;
        //    }

        //    public static List<Point3D> Vector3DToPoint3DList(Vector3D vector3D)
        //    {
        //        double v1 = vector3D.X / 2;
        //        double v2 = -(vector3D.X / 2);

        //        double v3 = vector3D.Y / 2;
        //        double v4 = -(vector3D.Y / 2);

        //        double v5 = vector3D.Z / 2;
        //        double v6 = -(vector3D.Z / 2);

        //        List<Point3D> point3Ds = new List<Point3D>();

        //        #region d1
        //        point3Ds.Add(new Point3D(v2, v3, v6));
        //        point3Ds.Add(new Point3D(v2, v4, v6));

        //        point3Ds.Add(new Point3D(v2, v4, v6));
        //        point3Ds.Add(new Point3D(v1, v4, v6));

        //        point3Ds.Add(new Point3D(v1, v4, v6));
        //        point3Ds.Add(new Point3D(v1, v3, v6));

        //        point3Ds.Add(new Point3D(v1, v3, v6));
        //        point3Ds.Add(new Point3D(v2, v3, v6));
        //        #endregion

        //        #region d2
        //        point3Ds.Add(new Point3D(v2, v3, v5));
        //        point3Ds.Add(new Point3D(v2, v4, v5));

        //        point3Ds.Add(new Point3D(v2, v4, v5));
        //        point3Ds.Add(new Point3D(v1, v4, v5));

        //        point3Ds.Add(new Point3D(v1, v4, v5));
        //        point3Ds.Add(new Point3D(v1, v3, v5));

        //        point3Ds.Add(new Point3D(v1, v3, v5));
        //        point3Ds.Add(new Point3D(v2, v3, v5));
        //        #endregion

        //        #region d3
        //        point3Ds.Add(new Point3D(v2, v4, v6));
        //        point3Ds.Add(new Point3D(v2, v4, v5));

        //        point3Ds.Add(new Point3D(v1, v4, v5));
        //        point3Ds.Add(new Point3D(v1, v4, v6));

        //        point3Ds.Add(new Point3D(v1, v3, v6));
        //        point3Ds.Add(new Point3D(v1, v3, v5));

        //        point3Ds.Add(new Point3D(v2, v3, v6));
        //        point3Ds.Add(new Point3D(v2, v3, v5));
        //        #endregion

        //        return point3Ds;
        //    }

        //    public static ModelVisual3D CreateWireFrameMDLLine(List<Point3D> point3Ds)
        //    {
        //        LinesVisual3D linesVisual3D = new LinesVisual3D();
        //        linesVisual3D.Thickness = 4;
        //        linesVisual3D.Points = ToPoint3DCollection(point3Ds);
        //        ModelVisual3D modelVisual3D = linesVisual3D;
        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CreateWireFrameMDLTube(List<Point3D> point3Ds)
        //    {
        //        TubeVisual3D tubeVisual3D = new TubeVisual3D();
        //        tubeVisual3D.Diameter = 0.05;
        //        tubeVisual3D.Path = ToPoint3DCollection(point3Ds);
        //        ModelVisual3D modelVisual3D = tubeVisual3D;
        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomCylinderVisual3D(System.Windows.Media.Color color, System.Windows.Media.Color BackColor)
        //    {
        //        Point3DCollection point3Ds = new Point3DCollection();
        //        point3Ds.Add(new Point3D(0, -0.5, 0));
        //        point3Ds.Add(new Point3D(0, 0.5, 0));

        //        TubeVisual3D tubeVisual3D = new TubeVisual3D
        //        {
        //            Diameter = 1,
        //            Path = point3Ds,
        //            AddCaps = true,
        //            Material = MaterialHelper.CreateMaterial(color),
        //            BackMaterial = MaterialHelper.CreateMaterial(BackColor)
        //        };

        //        Model3DGroup model3DGroup = new Model3DGroup();
        //        model3DGroup.Children.Add(tubeVisual3D.Content);

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomBoxVisual3D(Vector3D vector3D, Point3D center, System.Windows.Media.Color Color, System.Windows.Media.Color BackColor)
        //    {
        //        BoxVisual3D boxVisual3D = new BoxVisual3D
        //        {
        //            Length = vector3D.X,
        //            Width = vector3D.Y,
        //            Height = vector3D.Z,
        //            TopFace = true,
        //            BottomFace = true,
        //            Visible = true,
        //            Center = center,
        //            Material = MaterialHelper.CreateMaterial(Color),
        //            BackMaterial = MaterialHelper.CreateMaterial(BackColor),
        //        };

        //        Model3DGroup model3DGroup = new Model3DGroup();
        //        model3DGroup.Children.Add(boxVisual3D.Content);

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

        //        //ModelVisual3D modelVisual3D = boxVisual3D;

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomSphereVisual3D(int ThetaDivValue, int PhiDivValue, double RadiusValue, System.Windows.Media.Color Color, System.Windows.Media.Color BackColor)
        //    {
        //        //int ThetaDivValue = 30, int PhiDivValue = 10, double RadiusValue = 0.5
        //        SphereVisual3D sphereVisual3D = new SphereVisual3D
        //        {
        //            ThetaDiv = ThetaDivValue,
        //            PhiDiv = PhiDivValue,
        //            Radius = RadiusValue,
        //            Material = MaterialHelper.CreateMaterial(Color),
        //            BackMaterial = MaterialHelper.CreateMaterial(BackColor)
        //        };

        //        Model3DGroup model3DGroup = new Model3DGroup();
        //        model3DGroup.Children.Add(sphereVisual3D.Content);

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

        //        //ModelVisual3D modelVisual3D = sphereVisual3D;
        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomRectanglePlane3D(Point3DCollection P3DCollection, System.Windows.Media.Color color, System.Windows.Media.Color Backcolor, string MDLName = "")
        //    {
        //        RectangleVisual3D rectangleVisual3D = new RectangleVisual3D
        //        {
        //            Length = 1,
        //            Width = 1,
        //            DivLength = 1,
        //            DivWidth = 1,
        //            Origin = new Point3D(0, 0, 0)
        //        };

        //        ModelVisual3D modelVisual3D = rectangleVisual3D;
        //        HTK_3DES.OBJData.GetMeshGeometry3D(modelVisual3D.Content).Positions = P3DCollection;
        //        ((GeometryModel3D)modelVisual3D.Content).Material = MaterialHelper.CreateMaterial(color);
        //        ((GeometryModel3D)modelVisual3D.Content).BackMaterial = MaterialHelper.CreateMaterial(Backcolor);
        //        modelVisual3D.SetName(MDLName);

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomArrowVisual3D(double Diameter, int ThetaDiv, double HeadLength, Vector3D Direction, Point3D Origin, System.Windows.Media.Color Color, System.Windows.Media.Color BackColor)
        //    {
        //        ArrowVisual3D arrowVisual3D = new ArrowVisual3D
        //        {
        //            Diameter = Diameter,
        //            ThetaDiv = ThetaDiv,
        //            HeadLength = HeadLength,
        //            Direction = Direction,
        //            Origin = Origin,
        //            Material = MaterialHelper.CreateMaterial(Color),
        //            BackMaterial = MaterialHelper.CreateMaterial(BackColor)
        //        };

        //        ModelVisual3D modelVisual3D = arrowVisual3D;

        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomPointVector3D(System.Windows.Media.Color BoxColor, System.Windows.Media.Color BoxBackColor, System.Windows.Media.Color ArrowColor, System.Windows.Media.Color ArrowBackColor, System.Windows.Media.Color SphereColor, System.Windows.Media.Color SphereBackColor)
        //    {
        //        //BoxVisual3D boxVisual3D = (BoxVisual3D)CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1.65), BoxColor, BoxBackColor);
        //        ArrowVisual3D arrowVisual3D = (ArrowVisual3D)CustomArrowVisual3D(0.3, 5, 1, new Vector3D(0, 1, 0), new Point3D(0, -0.5, 0), ArrowColor, ArrowBackColor);

        //        HTK_3DES.TSRSystem.Transform transform = new HTK_3DES.TSRSystem.Transform
        //        {
        //            Rotate3D = new Vector3D(0, 0, 0),
        //            Scale3D = new Vector3D(1, 1, 1),
        //            Translate3D = new Vector3D(0, -0.1, 0)
        //        };

        //        HTK_3DES.TSRSystem.TSRSystem3D tSRSystem3D = new TSRSystem.TSRSystem3D(arrowVisual3D, transform);
        //        tSRSystem3D.TestTransform3D();

        //        //HTK_3DES.TSRSystem.TransformSetting transformSetting = new TSRSystem.TransformSetting { InputMV3D = arrowVisual3D };

        //        //HTK_3DES.TSRSystem.New_TransformSystem3D(transform, transformSetting);

        //        //HTK_3DES.TransformMV3D.Transform_MV3D(transform, arrowVisual3D, HTK_3DES.TSRSystem.RotationSetting.Angle);

        //        Model3DGroup model3DGroup = new Model3DGroup();
        //        model3DGroup.Children.Add(CustomBoxVisual3D(new Vector3D(0.3, 0.3, 5), new Point3D(0, 0, 2.65), BoxColor, BoxBackColor).Content);
        //        model3DGroup.Children.Add(arrowVisual3D.Content);
        //        model3DGroup.Children.Add(CustomSphereVisual3D(30, 10, 1, SphereColor, SphereBackColor).Content);

        //        ModelVisual3D modelVisual3D = new ModelVisual3D { Content = model3DGroup };

        //        return modelVisual3D;
        //    }

        //    public static CuttingPlaneGroup CreateCuttingPlaneGroup(List<Visual3D> visual3Ds, List<Plane3D> plane3Ds, CuttingOperation cuttingOperation, bool IsEnabled)
        //    {
        //        CuttingPlaneGroup cuttingPlaneGroup = new CuttingPlaneGroup
        //        {
        //            CuttingPlanes = plane3Ds,
        //            Operation = cuttingOperation,
        //            IsEnabled = IsEnabled
        //        };

        //        for (int f = 0; f < visual3Ds.Count; f++) cuttingPlaneGroup.Children.Add(visual3Ds[f]);

        //        return cuttingPlaneGroup;
        //    }

        //    public static CuttingPlaneGroup CreateCuttingPlaneGroup(Visual3D visual3D, List<Plane3D> plane3Ds, CuttingOperation cuttingOperation, bool IsEnabled)
        //    {
        //        CuttingPlaneGroup cuttingPlaneGroup = new CuttingPlaneGroup
        //        {
        //            CuttingPlanes = plane3Ds,
        //            Operation = cuttingOperation,
        //            IsEnabled = IsEnabled
        //        };

        //        cuttingPlaneGroup.Children.Add(visual3D);

        //        return cuttingPlaneGroup;
        //    }

        //    public enum Option
        //    {
        //        Setting1,
        //        Setting2,
        //        Setting3
        //    }

        //    public static ModelVisual3D CustomSphereHurf3D(int ThetaDivValue, int PhiDivValue, double RadiusValue, System.Windows.Media.Color Color, System.Windows.Media.Color BackColor, Option option)
        //    {
        //        //int ThetaDivValue = 30, int PhiDivValue = 10, double RadiusValue = 0.5
        //        SphereVisual3D sphereVisual3D = new SphereVisual3D
        //        {
        //            ThetaDiv = ThetaDivValue,
        //            PhiDiv = PhiDivValue,
        //            Radius = RadiusValue,
        //            Material = MaterialHelper.CreateMaterial(Color),
        //            BackMaterial = MaterialHelper.CreateMaterial(BackColor)
        //        };

        //        List<Plane3D> plane3Ds = new List<Plane3D>();

        //        if (option == Option.Setting1) plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 1, 0), Position = new Point3D(0, 0, 0) });
        //        if (option == Option.Setting2) plane3Ds.Add(new Plane3D { Normal = new Vector3D(1, 0, 0), Position = new Point3D(0, 0, 0) });
        //        if (option == Option.Setting3) plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 0, 1), Position = new Point3D(0, 0, 0) });

        //        ModelVisual3D modelVisual3D = CreateCuttingPlaneGroup(sphereVisual3D, plane3Ds, CuttingOperation.Intersect, true);
        //        return modelVisual3D;
        //    }

        //    public static ModelVisual3D CustomPointModel3D()
        //    {
        //        List<Plane3D> plane3Ds = new List<Plane3D>();
        //        plane3Ds.Add(new Plane3D { Normal = new Vector3D(0, 1, 0), Position = new Point3D(0, 0, 0) });

        //        List<Plane3D> plane3Ds2 = new List<Plane3D>();
        //        plane3Ds2.Add(new Plane3D { Normal = new Vector3D(0, -1, 0), Position = new Point3D(0, 0, 0) });

        //        ModelVisual3D sp1 = CustomSphereVisual3D(30, 10, 0.5, System.Windows.Media.Color.FromArgb(0x80, 0x00, 0xF0, 0x00), System.Windows.Media.Color.FromArgb(0x80, 0x00, 0xF0, 0x00));
        //        ModelVisual3D sp2 = CustomSphereVisual3D(30, 10, 0.5, System.Windows.Media.Color.FromArgb(0x80, 0xF0, 0x00, 0x00), System.Windows.Media.Color.FromArgb(0x80, 0xF0, 0x00, 0x00));
        //        ModelVisual3D Box1 = CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1), System.Windows.Media.Color.FromArgb(0x80, 0xF0, 0x00, 0xF0), System.Windows.Media.Color.FromArgb(0x80, 0xF0, 0x00, 0xF0));
        //        ModelVisual3D Box2 = CustomBoxVisual3D(new Vector3D(0.3, 0.3, 2.5), new Point3D(0, 0, 1), System.Windows.Media.Color.FromArgb(0x80, 0x00, 0xF0, 0xF0), System.Windows.Media.Color.FromArgb(0x80, 0x00, 0xF0, 0xF0));

        //        ModelVisual3D f1 = CreateCuttingPlaneGroup(sp1, plane3Ds, CuttingOperation.Intersect, true);
        //        ModelVisual3D f2 = CreateCuttingPlaneGroup(sp2, plane3Ds2, CuttingOperation.Intersect, true);
        //        ModelVisual3D f3 = CreateCuttingPlaneGroup(Box1, plane3Ds, CuttingOperation.Intersect, true);
        //        ModelVisual3D f4 = CreateCuttingPlaneGroup(Box2, plane3Ds2, CuttingOperation.Intersect, true);

        //        ModelVisual3D MV3D = new ModelVisual3D();
        //        MV3D.Children.Add(f1);
        //        MV3D.Children.Add(f2);
        //        MV3D.Children.Add(f3);
        //        MV3D.Children.Add(f4);

        //        return MV3D;
        //    }
        //}
    }
}
