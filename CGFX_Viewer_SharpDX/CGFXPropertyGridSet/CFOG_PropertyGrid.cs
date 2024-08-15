using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using CGFXLibrary;
using CGFXLibrary.CGFXSection;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class CFOG_PropertyGrid
	{
        public string Name { get; set; }

        public List<CGFXFormat.CGFXData> UserData_List = new List<CGFXFormat.CGFXData>();
        [Editor(typeof(CGFX_CustomPropertyGridClass.UserDataDictionaryEditor), typeof(UITypeEditor))]
        public List<CGFXFormat.CGFXData> userDataList { get => UserData_List; set => UserData_List = value; }

        public CFOG.FogFlipSetting.FlipSetting FlipSetting { get; set; }

        public byte[] CFOG_UnknownBytes_5 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_6 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_7 { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_8 { get; set; } //0x4
        public int CFOG_CFOGAnimation_DICTOffset { get; set; } //0x4

        public DICT FogAnimationDICT { get; set; }
        public DICT ColorDICT { get; set; }

        public Transform.Scale Transform_Scale { get; set; }
        public Transform.Rotate Transform_Rotate { get; set; }
        public Transform.Translate Transform_Translate { get; set; }
        public MatrixData.LocalMatrix CFOG_4x4_Matrix { get; set; }
        public MatrixData.WorldMatrix_Transform CFOG_4x4_Matrix_Transform { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public RGB RGB_Color { get; set; } = new RGB();
        public class RGB
		{
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public Color ToColor()
			{
                var ColorR = (int)(R * 255);
                var ColorG = (int)(G * 255);
                var ColorB = (int)(B * 255);
                var ColorA = (int)(A * 255);

                return Color.FromArgb(ColorA, ColorR, ColorG, ColorB);
            }

            [Editor(typeof(CGFX_CustomPropertyGridClass.CustomRGBAColorEditor), typeof(UITypeEditor))]
            public Color RGBColor
			{
				get => ToColor();
				set
				{
					R = (float)Math.Round((value.R / 255F), 2, MidpointRounding.AwayFromZero);
					G = (float)Math.Round((value.G / 255F), 2, MidpointRounding.AwayFromZero);
					B = (float)Math.Round((value.B / 255F), 2, MidpointRounding.AwayFromZero);
					A = (float)Math.Round((value.A / 255F), 2, MidpointRounding.AwayFromZero);
				}
			}

			public RGB()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }

            public RGB(float InputR, float InputG, float InputB, float InputA)
			{
                R = InputR;
                G = InputG;
                B = InputB;
                A = InputA;
            }

			public override string ToString()
			{
				return "RGBA Color";
			}
		}

        public CGFXFormat.CGFXData UnknownUserData { get; set; } //ColorData

        public CFOG.CFOGSetting.FogSuffixType FogSuffixType { get; set; }
        public float FogStart { get; set; }
        public float FogEnd { get; set; }
        public float Concentration { get; set; }

        public CFOG_PropertyGrid(CFOG CFOGData)
		{
            Name = CFOGData.Name;

            userDataList = CFOGData.UserDataDict.DICT_Entries.Select(x => x.CGFXData).ToList();
            FlipSetting = CFOGData.FogFlipSettings.FlipSettings;

            CFOG_UnknownBytes_5 = CFOGData.CFOG_UnknownBytes_5;
            CFOG_UnknownBytes_6 = CFOGData.CFOG_UnknownBytes_6;
            CFOG_UnknownBytes_7 = CFOGData.CFOG_UnknownBytes_7;
            CFOG_UnknownBytes_8 = CFOGData.CFOG_UnknownBytes_8;

            FogAnimationDICT = CFOGData.FogAnimationDICT;
            ColorDICT = CFOGData.ColorDICT;
            Transform_Scale = CFOGData.Transform_Scale;
            Transform_Rotate = CFOGData.Transform_Rotate;
            Transform_Translate = CFOGData.Transform_Translate;
            CFOG_4x4_Matrix = CFOGData.CFOG_4x4_Matrix;
            CFOG_4x4_Matrix_Transform = CFOGData.CFOG_4x4_Matrix_Transform;

            RGB_Color = new RGB(CFOGData.Color_RGBA.Color_R, CFOGData.Color_RGBA.Color_G, CFOGData.Color_RGBA.Color_B, CFOGData.Color_RGBA.Color_A);

            UnknownUserData = CFOGData.UnknownUserData;

            FogSuffixType = CFOGData.CFOGSettings.FogSuffixTypes;
            FogStart = CFOGData.CFOGSettings.FogStart;
            FogEnd = CFOGData.CFOGSettings.FogEnd;
            Concentration = CFOGData.CFOGSettings.Concentration;
        }

        public override string ToString()
		{
			return Name;
		}
	}
}
