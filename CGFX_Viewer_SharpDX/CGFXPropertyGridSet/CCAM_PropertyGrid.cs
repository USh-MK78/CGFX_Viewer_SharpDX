using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CCAM_PropertyGrid
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

        public int ViewModeValue { get; set; } //ViewMode
        public CCAM.ViewMode ViewModeType
        {
            get
            {
                return (CCAM.ViewMode)Enum.ToObject(typeof(CCAM.ViewMode), ViewModeValue);
            }
            set
            {
                ViewModeValue = (int)value;
            }
        }

        public int UnknownValue13 { get; set; }

        public int CameraViewOffset { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public CameraView Camera_View { get; set; } = new CameraView(CCAM.ViewMode.LookAt);
        public class CameraView
        {
            public int CameraModePropertyTypeValue { get; set; }
            public CCAM.CameraView.CameraModePropertyType CameraModePropertyTypes
            {
                get
                {
                    return (CCAM.CameraView.CameraModePropertyType)Enum.ToObject(typeof(CCAM.CameraView.CameraModePropertyType), CameraModePropertyTypeValue);
                }
                set
                {
                    CameraModePropertyTypeValue = (int)value;
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public CameraViewProperty CameraView_Property { get; set; } = new CameraViewProperty(CCAM.CameraView.CameraModePropertyType.LookAt);
            public class CameraViewProperty
            {
                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public Rotate RotateView { get; set; } = new Rotate();
                public class Rotate
                {
                    public float UnknownValue0 { get; set; }
                    public float Rotate_X { get; set; }
                    public float Rotate_Y { get; set; }
                    public float Rotate_Z { get; set; }

                    public Rotate(CCAM.CameraView.CameraViewProperty.Rotate rotate)
                    {
                        UnknownValue0 = rotate.UnknownValue0;
                        Rotate_X = rotate.Rotate_X;
                        Rotate_Y = rotate.Rotate_Y;
                        Rotate_Z = rotate.Rotate_Z;
                    }

                    public Rotate()
                    {
                        UnknownValue0 = 0;
                        Rotate_X = 0;
                        Rotate_Y = 0;
                        Rotate_Z = 0;
                    }

                    public override string ToString()
                    {
                        return "RotateView";
                    }
                }

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public LookAt LookAtView { get; set; } = new LookAt();
                public class LookAt
                {
                    public float UnknownValue0 { get; set; }
                    public float LookAt_X { get; set; }
                    public float LookAt_Y { get; set; }
                    public float LookAt_Z { get; set; }
                    public float UpVector_X { get; set; }
                    public float UpVector_Y { get; set; }
                    public float UpVector_Z { get; set; }

                    public LookAt(CCAM.CameraView.CameraViewProperty.LookAt lookAt)
                    {
                        UnknownValue0 = lookAt.UnknownValue0;
                        LookAt_X = lookAt.LookAt_X;
                        LookAt_Y = lookAt.LookAt_Y;
                        LookAt_Z = lookAt.LookAt_Z;
                        UpVector_X = lookAt.UpVector_X;
                        UpVector_Y = lookAt.UpVector_Y;
                        UpVector_Z = lookAt.UpVector_Z;
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

                    public override string ToString()
                    {
                        return "LookAtView";
                    }
                }

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public Aim AimView { get; set; } = new Aim();
                public class Aim
                {
                    public float UnknownValue0 { get; set; }
                    public float UnknownValue1 { get; set; }
                    public float UnknownValue2 { get; set; }
                    public float UnknownValue3 { get; set; }
                    public float UnknownValue4 { get; set; }

                    public Aim(CCAM.CameraView.CameraViewProperty.Aim aim)
                    {
                        UnknownValue0 = aim.UnknownValue0;
                        UnknownValue1 = aim.UnknownValue1;
                        UnknownValue2 = aim.UnknownValue2;
                        UnknownValue3 = aim.UnknownValue3;
                        UnknownValue4 = aim.UnknownValue4;
                    }

                    public Aim()
                    {
                        UnknownValue0 = 0;
                        UnknownValue1 = 0;
                        UnknownValue2 = 0;
                        UnknownValue3 = 0;
                        UnknownValue4 = 0;
                    }

                    public override string ToString()
                    {
                        return "AimView";
                    }
                }

                public CameraViewProperty(CCAM.CameraView.CameraViewProperty cameraViewProperty)
                {
                    if (cameraViewProperty.RotateView != null) RotateView = new Rotate(cameraViewProperty.RotateView);
                    if (cameraViewProperty.LookAtView != null) LookAtView = new LookAt(cameraViewProperty.LookAtView);
                    if (cameraViewProperty.AimView != null) AimView = new Aim(cameraViewProperty.AimView);
                }

                public CameraViewProperty(CCAM.CameraView.CameraModePropertyType cameraModePropertyType)
                {
                    if (cameraModePropertyType == CCAM.CameraView.CameraModePropertyType.Rotate)
                    {
                        RotateView = new Rotate();
                    }
                    else if (cameraModePropertyType == CCAM.CameraView.CameraModePropertyType.LookAt)
                    {
                        LookAtView = new LookAt();
                    }
                    else if (cameraModePropertyType == CCAM.CameraView.CameraModePropertyType.Aim)
                    {
                        AimView = new Aim();
                    }
                }

                public override string ToString()
                {
                    return "CameraViewProperty";
                }
            }

            public CameraView(CCAM.CameraView cameraView)
            {
                CameraModePropertyTypeValue = cameraView.CameraModePropertyTypeValue;
                CameraView_Property = new CameraViewProperty(cameraView.CameraView_Property);
            }

            public CameraView(CCAM.ViewMode viewMode)
            {
                CameraModePropertyTypeValue = 0;
                if (viewMode == CCAM.ViewMode.Rotate)
                {
                    CameraView_Property = new CameraViewProperty(CameraModePropertyTypes);
                }
            }

            public override string ToString()
            {
                return "CameraView";
            }
        }

        public int UnknownOffset1 { get; set; } //0x4

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public Projection ProjectionSetting { get; set; } = new Projection();
        public class Projection
        {
            public Flags Flags { get; set; }
            public float CameraNearValue { get; set; } //NearValue => Default : 1
            public float CameraFarValue { get; set; } //FarValue = Default : 10000
            public float AspectRatio { get; set; } //AspectRatio => 15:9 = 1.6666... | 4.3 => 1.3333...
            public float UnknownValue3 { get; set; }

            public Projection(CCAM.Projection projection)
            {
                Flags = projection.Flags;
                CameraNearValue = projection.CameraNearValue;
                CameraFarValue = projection.CameraFarValue;
                AspectRatio = projection.AspectRatio;
                UnknownValue3 = projection.UnknownValue3;
            }

            public Projection()
            {
                Flags = new Flags(new byte[4]);
                CameraNearValue = 0;
                CameraFarValue = 0;
                AspectRatio = 0;
                UnknownValue3 = 0;
            }

            public override string ToString()
            {
                return "Projection";
            }
        }

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

        public CCAM_PropertyGrid(CCAM cCAM)
        {
            CCAM_Header = cCAM.CCAM_Header;
            Revision = cCAM.Revision;
            //public int NameOffset { get; set; }
            UnknownBytes0 = cCAM.UnknownBytes0;
            UnknownValue0 = cCAM.UnknownValue0;
            UnknownValue1 = cCAM.UnknownValue1;
            UnknownBytes1 = cCAM.UnknownBytes1;
            UnknownValue2 = cCAM.UnknownValue2;

            //public int DICTOffset { get; set; }
            UnknownDICT = cCAM.UnknownDICT;

            UnknownValue3 = cCAM.UnknownValue3;
            UnknownValue4 = cCAM.UnknownValue4;
            UnknownValue5 = cCAM.UnknownValue5;
            UnknownValue6 = cCAM.UnknownValue6;
            UnknownValue7 = cCAM.UnknownValue7;
            UnknownValue8 = cCAM.UnknownValue8;
            UnknownValue9 = cCAM.UnknownValue9;
            UnknownValue10 = cCAM.UnknownValue10;
            UnknownValue11 = cCAM.UnknownValue11;

            UnknownMatrix34_0 = cCAM.UnknownMatrix34_0;
            UnknownMatrix34_1 = cCAM.UnknownMatrix34_1;

            ViewModeValue = cCAM.ViewModeValue;

            UnknownValue13 = cCAM.UnknownValue13;

            Camera_View = new CameraView(cCAM.Camera_View);
            //UnknownOffset1

            ProjectionSetting = new Projection(cCAM.ProjectionSetting);
            W_FlipValue = cCAM.W_FlipValue;
            IsW_Flip = cCAM.IsW_Flip;
        }
    }
}
