//using HelixToolkit.Wpf.SharpDX;
//using SharpDX;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CANM
//{
//    public class PathTool
//    {
//        public enum PointType
//        {
//            Model,
//            Point3D
//        }

//        public List<MeshGeometryModel3D> MV3D_List { get; set; }
//        public List<Vector3> PathPointList { get; set; }
//        public List<LineGeometryModel3D> LV3D_List { get; set; }

//        public PathTool()
//        {
//            //MV3D_List = new List<MeshGeometryModel3D>();
//            PathPointList = new List<Vector3>();
//            LV3D_List = new List<LineGeometryModel3D>();
//        }

//        public PathTool(List<Vector3> PathPointList, List<LineGeometryModel3D> LV3DList)
//        {
//            this.PathPointList = PathPointList;
//            LV3D_List = LV3DList;
//        }

//        public List<Vector3> MV3DListToPoint3DList()
//        {
//            List<Vector3> point3Ds = new List<Vector3>();

//            for (int i = 0; i < MV3D_List.Count; i++)
//            {
//                var n = MV3D_List[i];
//                Vector3 p3d = new Vector3((float)n.Transform.Value.OffsetX, (float)n.Transform.Value.OffsetY, (float)n.Transform.Value.OffsetZ);
//                point3Ds.Add(p3d);
//            }

//            return point3Ds;
//        }

//        public List<LineGeometryModel3D> DrawPath_Line(CurveViewUserControl UserCtrl, double Thickness, System.Windows.Media.Color color)
//        {
//            if (PathPointList.Count > 1)
//            {
//                for (int i = 1; i < PathPointList.Count; i++)
//                {
//                    List<Vector3> OneLine = new List<Vector3>();
//                    OneLine.Add(PathPointList[i - 1]);
//                    OneLine.Add(PathPointList[i]);

//                    LineGeometryModel3D lineGeometryModel3D = new LineGeometryModel3D
//                    {
//                        Geometry = new LineGeometry3D
//                        {
//                            Positions = new Vector3Collection(OneLine)
//                        },

//                        Color = color,
//                        Thickness = Thickness,
//                        Smoothness = 2,
//                        Transform = new System.Windows.Media.Media3D.TranslateTransform3D(0, 0, 0),
//                    };

//                    UserCtrl.Viewport3DX.Items.Add(lineGeometryModel3D);

//                    LV3D_List.Add(lineGeometryModel3D);
//                }
//            }

//            return LV3D_List;
//        }

//        public List<LineGeometryModel3D> DrawPath_Line(CurveViewUserControl UserCtrl, List<Vector3> vector3s, double Thickness, System.Windows.Media.Color color)
//        {
//            PathPointList = vector3s;

//            if (PathPointList.Count > 1)
//            {
//                for (int i = 1; i < PathPointList.Count; i++)
//                {
//                    List<Vector3> OneLine = new List<Vector3>();
//                    OneLine.Add(PathPointList[i - 1]);
//                    OneLine.Add(PathPointList[i]);

//                    LineGeometryModel3D lineGeometryModel3D = new LineGeometryModel3D
//                    {
//                        Geometry = new LineGeometry3D
//                        {
//                            Positions = new Vector3Collection(OneLine)
//                        },

//                        Color = color,
//                        Thickness = Thickness,
//                        Smoothness = 2,
//                        Transform = new System.Windows.Media.Media3D.TranslateTransform3D(0, 0, 0),
//                    };

//                    UserCtrl.Viewport3DX.Items.Add(lineGeometryModel3D);

//                    UserCtrl.UpdateLayout();

//                    LV3D_List.Add(lineGeometryModel3D);
//                }
//            }

//            return LV3D_List;
//        }

//        public void MoveRails(int MDLNum, Vector3 Pos)
//        {
//            if (MDLNum == 0)
//            {
//                LV3D_List[MDLNum].Geometry.Positions[0] = Pos;
//            }
//            if (MDLNum > 0 && MDLNum < LV3D_List.Count)
//            {
//                LV3D_List[MDLNum - 1].Geometry.Positions[1] = Pos;
//                LV3D_List[MDLNum].Geometry.Positions[0] = Pos;
//            }
//            if (MDLNum == LV3D_List.Count)
//            {
//                LV3D_List[MDLNum - 1].Geometry.Positions[1] = Pos;
//            }
//        }

//        public void DeleteRail(CurveViewUserControl UserCtrl)
//        {
//            if (LV3D_List != null)
//            {
//                for (int LVCount = 0; LVCount < LV3D_List.Count; LVCount++)
//                {
//                    UserCtrl.Viewport3DX.Items.Remove(LV3D_List[LVCount]);
//                    UserCtrl.UpdateLayout();
//                }

//                LV3D_List.Clear();
//            }

//            if (MV3D_List != null)
//            {
//                for (int MV3DCount = 0; MV3DCount < MV3D_List.Count; MV3DCount++)
//                {
//                    UserCtrl.Viewport3DX.Items.Remove(MV3D_List[MV3DCount]);
//                    UserCtrl.UpdateLayout();
//                }

//                MV3D_List.Clear();
//            }
//        }

//        public void DeleteRailPoint(CurveViewUserControl UserCtrl, int SelectedIdx, double TubeDiametor, System.Windows.Media.Color color)
//        {
//            Vector3? SelectedIndex_Next = null;
//            Vector3? SelectedIndex_Current = null;
//            Vector3? SelectedIndex_Prev = null;

//            List<Vector3> point3Ds = new List<Vector3>();

//            #region SelectedIndex_Next
//            try
//            {
//                SelectedIndex_Next = new Vector3((float)MV3D_List[SelectedIdx + 1].Transform.Value.OffsetX, (float)MV3D_List[SelectedIdx + 1].Transform.Value.OffsetY, (float)MV3D_List[SelectedIdx + 1].Transform.Value.OffsetZ);
//            }
//            catch (System.ArgumentOutOfRangeException)
//            {
//                SelectedIndex_Next = null;
//            }
//            #endregion

//            #region SelectedIndex_Current
//            try
//            {
//                SelectedIndex_Current = new Vector3((float)MV3D_List[SelectedIdx].Transform.Value.OffsetX, (float)MV3D_List[SelectedIdx].Transform.Value.OffsetY, (float)MV3D_List[SelectedIdx].Transform.Value.OffsetZ);
//            }
//            catch (System.ArgumentOutOfRangeException)
//            {
//                SelectedIndex_Current = null;
//            }
//            #endregion

//            #region SelectedIndex_Prev
//            try
//            {
//                SelectedIndex_Prev = new Vector3((float)MV3D_List[SelectedIdx - 1].Transform.Value.OffsetX, (float)MV3D_List[SelectedIdx - 1].Transform.Value.OffsetY, (float)MV3D_List[SelectedIdx - 1].Transform.Value.OffsetZ);
//            }
//            catch (System.ArgumentOutOfRangeException)
//            {
//                SelectedIndex_Prev = null;
//            }
//            #endregion

//            if (SelectedIndex_Current != null)
//            {
//                if ((SelectedIndex_Next == null && SelectedIndex_Prev == null) == true)
//                {
//                    UserCtrl.Viewport3DX.Items.Remove(MV3D_List[SelectedIdx]);
//                    MV3D_List.Remove(MV3D_List[SelectedIdx]);

//                    //MessageBox.Show("Point3D Only");
//                }
//                else if ((SelectedIndex_Next != null && SelectedIndex_Prev != null) == true)
//                {
//                    point3Ds.Add(SelectedIndex_Prev.Value);
//                    point3Ds.Add(SelectedIndex_Next.Value);

//                    //Pointを削除
//                    UserCtrl.Viewport3DX.Items.Remove(MV3D_List[SelectedIdx]);
//                    MV3D_List.Remove(MV3D_List[SelectedIdx]);

//                    //Pointの両端に存在するLinesVisual3Dを削除
//                    UserCtrl.Viewport3DX.Items.Remove(LV3D_List[SelectedIdx]);
//                    LV3D_List.Remove(LV3D_List[SelectedIdx]);

//                    UserCtrl.Viewport3DX.Items.Remove(LV3D_List[SelectedIdx - 1]);
//                    LV3D_List.Remove(LV3D_List[SelectedIdx - 1]);

//                    for (int i = 0; i < MV3D_List.Count; i++)
//                    {
//                        string[] MDLInfo = MV3D_List[i].Name.Split(' ');
//                        string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
//                        MV3D_List[i].Name = New_MDLInfo;
//                    }

//                    List<Vector3> OneLine = new List<Vector3>();
//                    OneLine.Add(point3Ds[0]);
//                    OneLine.Add(point3Ds[1]);

//                    LineGeometryModel3D lineGeometryModel3D = new LineGeometryModel3D
//                    {
//                        Geometry = new LineGeometry3D
//                        {
//                            Positions = new Vector3Collection(OneLine),
//                        },
//                        Thickness = TubeDiametor,
//                        Color = color,
//                        Smoothness = 2,
//                        Transform = new System.Windows.Media.Media3D.TranslateTransform3D(0, 0, 0),
//                    };

//                    LV3D_List.Insert(SelectedIdx - 1, lineGeometryModel3D);

//                    UserCtrl.Viewport3DX.Items.Add(lineGeometryModel3D);

//                    //MessageBox.Show("PrevPoint and NextPoint");
//                }
//                else if ((SelectedIndex_Next != null || SelectedIndex_Prev != null) == true)
//                {
//                    if (SelectedIndex_Prev == null)
//                    {
//                        UserCtrl.Viewport3DX.Items.Remove(MV3D_List[SelectedIdx]);
//                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

//                        UserCtrl.Viewport3DX.Items.Remove(LV3D_List[SelectedIdx]);
//                        LV3D_List.Remove(LV3D_List[SelectedIdx]);

//                        for (int i = 0; i < MV3D_List.Count; i++)
//                        {
//                            string[] MDLInfo = MV3D_List[i].Name.Split(' ');
//                            string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];
//                            MV3D_List[i].Name = New_MDLInfo;
//                        }

//                        //MessageBox.Show("PrevPoint not found : FirstPoint");
//                    }
//                    if (SelectedIndex_Next == null)
//                    {
//                        UserCtrl.Viewport3DX.Items.Remove(MV3D_List[SelectedIdx]);
//                        MV3D_List.Remove(MV3D_List[SelectedIdx]);

//                        UserCtrl.Viewport3DX.Items.Remove(LV3D_List[SelectedIdx - 1]);
//                        LV3D_List.Remove(LV3D_List[SelectedIdx - 1]);

//                        for (int i = 0; i < MV3D_List.Count; i++)
//                        {
//                            string[] MDLInfo = MV3D_List[i].Name.Split(' ');
//                            string New_MDLInfo = MDLInfo[0] + " " + i.ToString() + " " + MDLInfo[2];

//                            MV3D_List[i].Name = New_MDLInfo;
//                        }

//                        //MessageBox.Show("NextPoint not found : EndPoint");
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
//        public void ResetRail(CurveViewUserControl UserCtrl)
//        {
//            for (int i = 0; i < LV3D_List.Count; i++)
//            {
//                UserCtrl.Viewport3DX.Items.Remove(LV3D_List[i]);
//            }

//            LV3D_List.Clear();
//        }
//    }
//}
