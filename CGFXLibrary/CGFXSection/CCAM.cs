using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Camera
    /// </summary>
    public class CCAM
    {
        public string Name;
        public char[] CCAM_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public byte[] UnknownBytes0 { get; set; } //0x8
        public int UnknownValue0 { get; set; } //0x4
        public int UnknownValue1 { get; set; } //0x4
        public byte[] UnknownBytes1 { get; set; } //0x8
        public int UnknownValue2 { get; set; } //0x4
        public int DICTOffset { get; set; }
        public DICT UnknownDICT { get; set; }

        //DefaultValue
        public float UnknownValue3 { get; set; }
        public float UnknownValue4 { get; set; }
        public float UnknownValue5 { get; set; }
        public float UnknownValue6 { get; set; }
        public float UnknownValue7 { get; set; }
        public float UnknownValue8 { get; set; }
        public float UnknownValue9 { get; set; }
        public float UnknownValue10 { get; set; }
        public float UnknownValue11 { get; set; }

        public MatrixData.Matrix3x4 UnknownMatrix34_0 { get; set; }
        public MatrixData.Matrix3x4 UnknownMatrix34_1 { get; set; }

        public enum ViewMode
        {
            Aim = 0,
            LookAt = 1,
            Rotate = 2
        }

        public int ViewModeValue { get; set; } //ViewMode
        public ViewMode ViewModeType
        {
            get
            {
                return (ViewMode)Enum.ToObject(typeof(ViewMode), ViewModeValue);
            }
            set
            {
                ViewModeValue = (int)value;
            }
        }

        
        public int UnknownValue13 { get; set; }

        public int CameraViewOffset { get; set; }

        public CameraView Camera_View { get; set; }
        public class CameraView
        {
            public enum CameraModePropertyType
            {
                Rotate = 0x20,
                LookAt = 0x40,
                Aim = 0x80
            }

            public int CameraModePropertyTypeValue { get; set; }
            public CameraModePropertyType CameraModePropertyTypes
            {
                get
                {
                    return (CameraModePropertyType)Enum.ToObject(typeof(CameraModePropertyType), CameraModePropertyTypeValue);
                }
                set
                {
                    CameraModePropertyTypeValue = (int)value;
                }
            }

            public CameraViewProperty CameraView_Property { get; set; }
            public class CameraViewProperty
            {
                public Rotate RotateView { get; set; }
                public class Rotate
                {
                    public float UnknownValue0 { get; set; }
                    public float Rotate_X { get; set; }
                    public float Rotate_Y { get; set; }
                    public float Rotate_Z { get; set; }

                    public void ReadRotateViewProperty(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Rotate_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Rotate_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        Rotate_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public Rotate(float v0, float Rot_X, float Rot_Y, float Rot_Z)
                    {
                        UnknownValue0 = v0;
                        Rotate_X = Rot_X;
                        Rotate_Y = Rot_Y;
                        Rotate_Z = Rot_Z;
                    }

                    public Rotate(float v0, Vector3 Rotate)
                    {
                        UnknownValue0 = v0;
                        Rotate_X = Rotate.X;
                        Rotate_Y = Rotate.Y;
                        Rotate_Z = Rotate.Z;
                    }

                    public Rotate()
                    {
                        UnknownValue0 = 0;
                        Rotate_X = 0;
                        Rotate_Y = 0;
                        Rotate_Z = 0;
                    }
                }

                public LookAt LookAtView { get; set; }
                public class LookAt
                {
                    public float UnknownValue0 { get; set; }
                    public float LookAt_X { get; set; }
                    public float LookAt_Y { get; set; }
                    public float LookAt_Z { get; set; }
                    public float UpVector_X { get; set; }
                    public float UpVector_Y { get; set; }
                    public float UpVector_Z { get; set; }

                    public void ReadLookAtViewProperty(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        LookAt_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        LookAt_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        LookAt_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UpVector_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UpVector_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UpVector_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public LookAt(float v0, float LookAt_X, float LookAt_Y, float LookAt_Z, float UpVector_X, float UpVector_Y, float UpVector_Z)
                    {
                        UnknownValue0 = v0;
                        this.LookAt_X = LookAt_X;
                        this.LookAt_Y = LookAt_Y;
                        this.LookAt_Z = LookAt_Z;
                        this.UpVector_X = UpVector_X;
                        this.UpVector_Y = UpVector_Y;
                        this.UpVector_Z = UpVector_Z;
                    }

                    public LookAt(float v0, Vector3 LookAt, Vector3 UpVector)
                    {
                        UnknownValue0 = v0;
                        this.LookAt_X = LookAt.X;
                        this.LookAt_Y = LookAt.Y;
                        this.LookAt_Z = LookAt.Z;
                        this.UpVector_X = UpVector.X;
                        this.UpVector_Y = UpVector.Y;
                        this.UpVector_Z = UpVector.Z;
                    }

                    public LookAt()
                    {
                        UnknownValue0 = 0;
                        this.LookAt_X = 0;
                        this.LookAt_Y = 0;
                        this.LookAt_Z = 0;
                        this.UpVector_X = 0;
                        this.UpVector_Y = 0;
                        this.UpVector_Z = 0;
                    }
                }

                public Aim AimView { get; set; }
                public class Aim
                {
                    public float UnknownValue0 { get; set; }
                    public float UnknownValue1 { get; set; }
                    public float UnknownValue2 { get; set; }
                    public float UnknownValue3 { get; set; }
                    public float UnknownValue4 { get; set; }

                    public void ReadAimViewProperty(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownValue1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownValue2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownValue3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                        UnknownValue4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }

                    public Aim(float v0, float v1, float v2, float v3, float v4)
                    {
                        UnknownValue0 = v0;
                        UnknownValue1 = v1;
                        UnknownValue2 = v2;
                        UnknownValue3 = v3;
                        UnknownValue4 = v4;
                    }

                    public Aim()
                    {
                        UnknownValue0 = 0;
                        UnknownValue1 = 0;
                        UnknownValue2 = 0;
                        UnknownValue3 = 0;
                        UnknownValue4 = 0;
                    }
                }

                public CameraViewProperty(CameraModePropertyType cameraModePropertyType)
                {
                    if (cameraModePropertyType == CameraModePropertyType.Rotate) RotateView = new Rotate();
                    else if (cameraModePropertyType == CameraModePropertyType.LookAt) LookAtView = new LookAt();
                    else if (cameraModePropertyType == CameraModePropertyType.Aim) AimView = new Aim();
                }
            }

            public void ReadCameraView(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                CameraModePropertyTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4).Reverse().ToArray()), 0);
                CameraView_Property = new CameraViewProperty(CameraModePropertyTypes);

                //Aim => 00 00 00 80 [float(?), x5] | LookAt => 00 00 00 40 [float, x7] | Rotate => 00 00 00 20 [float(?), x4]
                if (CameraModePropertyTypes == CameraModePropertyType.Rotate) CameraView_Property.RotateView.ReadRotateViewProperty(br, BOM);
                else if (CameraModePropertyTypes == CameraModePropertyType.LookAt) CameraView_Property.LookAtView.ReadLookAtViewProperty(br, BOM);
                else if (CameraModePropertyTypes == CameraModePropertyType.Aim) CameraView_Property.AimView.ReadAimViewProperty(br, BOM);
            }

            public CameraView(ViewMode viewMode)
            {
                CameraModePropertyTypeValue = 0;
                if (viewMode == ViewMode.Rotate) CameraView_Property = new CameraViewProperty(CameraModePropertyType.Rotate);
                else if (viewMode == ViewMode.LookAt) CameraView_Property = new CameraViewProperty(CameraModePropertyType.LookAt);
                else if (viewMode == ViewMode.Aim) CameraView_Property = new CameraViewProperty(CameraModePropertyType.Aim);
            }

            public CameraView()
            {
                CameraModePropertyTypeValue = 0;
                CameraView_Property = new CameraViewProperty(CameraModePropertyType.LookAt);
            }
        }

        #region delete
        //public UnknownData0 UnknownData_0 { get; set; }
        //public class UnknownData0
        //{
        //    public Flags Flags { get; set; }
        //    public List<NamedValue> NamedValueList { get; set; }

        //    public void ReadUnknownData(BinaryReader br, byte[] BOM)
        //    {
        //        EndianConvert endianConvert = new EndianConvert(BOM);
        //        Flags = new Flags(br.ReadBytes(4));

        //        //Aim => 00 00 00 80 [float(?), x5] | LookAt => 00 00 00 40 [float, x7] | Rotate => 00 00 00 20 [float(?), x4]
        //        if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x20 }))
        //        {
        //            NamedValueList.Add(new NamedValue("UnknownValue0", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("Rotate_X", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("Rotate_Y", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("Rotate_Z", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //        }
        //        else if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x40 }))
        //        {
        //            NamedValueList.Add(new NamedValue("UnknownValue0", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("LookAt_X", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("LookAt_Y", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("LookAt_Z", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UpVector_X", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UpVector_Y", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UpVector_Z", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //        }
        //        else if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }))
        //        {
        //            NamedValueList.Add(new NamedValue("UnknownValue0", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UnknownValue1", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UnknownValue2", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UnknownValue3", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //            NamedValueList.Add(new NamedValue("UnknownValue4", BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0)));
        //        }
        //    }

        //    public UnknownData0()
        //    {
        //        Flags = new Flags(new byte[4]);
        //        NamedValueList = new List<NamedValue>();
        //    }
        //}

        //public UnknownData0 UnknownData_0 { get; set; }
        //public class UnknownData0
        //{
        //    public Flags Flags { get; set; }
        //    public float UnknownValue0 { get; set; }
        //    public float LookAt_X { get; set; }
        //    public float LookAt_Y { get; set; }
        //    public float LookAt_Z { get; set; }
        //    public float UpVector_X { get; set; }
        //    public float UpVector_Y { get; set; }
        //    public float UpVector_Z { get; set; }

        //    public void ReadUnknownData(BinaryReader br, byte[] BOM)
        //    {
        //        EndianConvert endianConvert = new EndianConvert(BOM);
        //        Flags = new Flags(endianConvert.Convert(br.ReadBytes(4)));


        //        //Aim => 00 00 00 80 [float(?), x5] | LookAt => 00 00 00 40 [float, x7] | Rotate => 00 00 00 20 [float(?), x4]

        //        //00 00 00 40
        //        UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        LookAt_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        LookAt_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        LookAt_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        UpVector_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        UpVector_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //        UpVector_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
        //    }

        //    public UnknownData0()
        //    {
        //        Flags = new Flags(new byte[4]);
        //        UnknownValue0 = 0;
        //        LookAt_X = 0;
        //        LookAt_Y = 0;
        //        LookAt_Z = 0;
        //        UpVector_X = 0;
        //        UpVector_Y = 0;
        //        UpVector_Z = 0;
        //    }
        //}
        #endregion

        public int UnknownOffset1 { get; set; } //0x4

        public Projection ProjectionSetting { get; set; }
        public class Projection
        {
            public Flags Flags { get; set; }
            public float CameraNearValue { get; set; } //NearValue
            public float CameraFarValue { get; set; } //FarValue
            public float AspectRatio { get; set; } //AspectRatio => 15:9 = 1.6666... | 4.3 => 1.3333...
            public float UnknownValue3 { get; set; }

            public void ReadUnknownData(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Flags = new Flags(endianConvert.Convert(br.ReadBytes(4)));
                CameraNearValue = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                CameraFarValue = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                AspectRatio = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownValue3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public Projection(float Near = 1, float Far = 10000, float AspectRatio = 0)
            {
                Flags = new Flags(new byte[4]);
                CameraNearValue = Near;
                CameraFarValue = Far;
                this.AspectRatio = AspectRatio;
                UnknownValue3 = 0;
            }

            public Projection()
            {
                Flags = new Flags(new byte[4]);
                CameraNearValue = 0;
                CameraFarValue = 0;
                AspectRatio = 0;
                UnknownValue3 = 0;
            }
        }

        //true => W_FlipValue, false => W_FlipValue = 0
        private float _WFlipValue;
        public float W_FlipValue
        {
            get
            {
                return _WFlipValue;
            }
            set
            {
                _WFlipValue = value;
                IsW_Flip = value != 0 ? true : false;
            }
        }
        public bool IsW_Flip
        {
            get
            {
                return _WFlipValue != 0 ? true : false;
            }
            set
            {
                if (value == true)
                {
                    if (_WFlipValue == 0) _WFlipValue = 0.000100f;
                    else _WFlipValue = W_FlipValue;
                }
                else if (value == false) _WFlipValue = 0;
            }
        }

        public MatrixData.Matrix3x4 GetMatrix(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);

            float[] ary = new float[12];
            for (int i = 0; i < 12; i++)
            {
                ary[i] = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            return new MatrixData.Matrix3x4(ary);
        }

        public void ReadCCAM(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CCAM_Header = br.ReadChars(4);
            if (new string(CCAM_Header) != "CCAM") throw new Exception("不明なフォーマットです");

            Revision = endianConvert.Convert(br.ReadBytes(4));
            NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (NameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(NameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            UnknownBytes0 = br.ReadBytes(8);
            UnknownValue0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownBytes1 = br.ReadBytes(8);
            UnknownValue2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            DICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (DICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(DICTOffset, SeekOrigin.Current);

                UnknownDICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }

            UnknownValue3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue5 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue6 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue7 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue8 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue9 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue10 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownMatrix34_0 = GetMatrix(br, BOM); //3x4 matrix
            UnknownMatrix34_1 = GetMatrix(br, BOM); //3x4 Matrix

            ViewModeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownValue13 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            CameraViewOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CameraViewOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(CameraViewOffset, SeekOrigin.Current);

                //IdentFlag(?)
                //Aim => 00 00 00 80 [float(?), x5] | LookAt => 00 00 00 40 [float, x7] | Rotate => 00 00 00 20 [float(?), x4]
                //CameraView cameraView = new CameraView();
                //cameraView.ReadCameraView(br, BOM);
                //Camera_View = cameraView;

                //Aim => 00 00 00 80 [float(?), x5] | LookAt => 00 00 00 40 [float, x7] | Rotate => 00 00 00 20 [float(?), x4]
                CameraView cameraView = new CameraView(ViewModeType);
                cameraView.ReadCameraView(br, BOM);
                Camera_View = cameraView;



                br.BaseStream.Position = Pos;
            }

            UnknownOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownOffset1 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(UnknownOffset1, SeekOrigin.Current);

                //00 00 00 20 => IdentFlag
                Projection unknownData1 = new Projection();
                unknownData1.ReadUnknownData(br, BOM);
                ProjectionSetting = unknownData1;

                br.BaseStream.Position = Pos;
            }

            //Write => W_FlipValue = IsW_Flip != false ? W_FlipValue : 0;
            W_FlipValue = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);


        }

        public CCAM()
        {
            CCAM_Header = "CCAM".ToArray();
            Revision = new byte[4];
            NameOffset = 0;

            UnknownBytes0 = new byte[8];
            UnknownValue0 = 0;
            UnknownValue1 = 0;
            UnknownBytes1 = new byte[8];
            UnknownValue2 = 0;

            DICTOffset = 0;
            UnknownDICT = new DICT();

            UnknownValue3 = 0;
            UnknownValue4 = 0;
            UnknownValue5 = 0;
            UnknownValue6 = 0;
            UnknownValue7 = 0;
            UnknownValue8 = 0;
            UnknownValue9 = 0;
            UnknownValue10 = 0;
            UnknownValue11 = 0;

            UnknownMatrix34_0 = new MatrixData.Matrix3x4();
            UnknownMatrix34_1 = new MatrixData.Matrix3x4();

            ViewModeValue = 0;
            UnknownValue13 = 0;

            CameraViewOffset = 0;
            Camera_View = new CameraView(ViewMode.LookAt);

            UnknownOffset1 = 0;

            W_FlipValue = 0;
        }
    }

    public class NamedValue
    {
        public string ValueName { get; set; }
        public float Value { get; set; }

        public NamedValue(string Name = "DefaultName", float Value = 0)
        {
            ValueName = Name;
            this.Value = Value;
        }
    }
}
