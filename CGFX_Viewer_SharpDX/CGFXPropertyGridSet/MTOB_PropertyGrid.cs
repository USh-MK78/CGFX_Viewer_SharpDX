using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFX_Viewer_SharpDX.PropertyGridForms.Section.MTOB.SHDRData;
using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using static CGFXLibrary.CGFXFormat;

namespace CGFX_Viewer_SharpDX.CGFXPropertyGridSet
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class MTOB_PropertyGrid
    {
        public string Name { get; }

        //public char[] MTOB_Header { get; set; }
        public byte[] Revision { get; set; }
        //public int NameOffset { get; set; }

        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public MTOB.LightSetting LightSetting { get; set; } = new MTOB.LightSetting(0x00);

        public int Value => LightSetting.Value;
        public bool IsFragmentLighting => LightSetting.IsFragmentLighting;
        public bool IsVertexLighting => LightSetting.IsVertexLighting;
        public bool IsHemiSphereLighting => LightSetting.IsHemiSphereLighting;

        public bool EnableOcclusion => LightSetting.EnableOcclusion;

        //[ReadOnly(VertexLightSetting)]

        //public int IsFragmentLighting { get; set; }
        public int UnknownData4 { get; set; }
        public int DrawingLayer { get; set; }

        //MaterialColor
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public MaterialColor MaterialColors { get; set; } = new MaterialColor();
        public class MaterialColor
        {
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Emission EmissionData { get; set; } = new Emission();
            public class Emission
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Emission(MTOB.MaterialColor.Emission emission)
                {
                    R = emission.R;
                    G = emission.G;
                    B = emission.B;
                    A = emission.A;
                }

                public Emission(float R, float G, float B, float A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public Emission()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 0;
                }

                public override string ToString()
                {
                    return "Emission : [" + "R : " + R + " | " + "G : " + G + " | " + "B : " + B + " | " + "A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Ambient AmbientData { get; set; } = new Ambient();
            public class Ambient
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Ambient(MTOB.MaterialColor.Ambient ambient)
                {
                    R = ambient.R;
                    G = ambient.G;
                    B = ambient.B;
                    A = ambient.A;
                }

                public Ambient(float R, float G, float B, float A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public Ambient()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Ambient : [" + "R : " + R + " | " + "G : " + G + " | " + "B : " + B + " | " + "A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Diffuse DiffuseData { get; set; } = new Diffuse();
            public class Diffuse
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Diffuse(MTOB.MaterialColor.Diffuse diffuse)
                {
                    R = diffuse.R;
                    G = diffuse.G;
                    B = diffuse.B;
                    A = diffuse.A;
                }

                public Diffuse(float R, float G, float B, float A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public Diffuse()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Diffuse : [" + "R : " + R + " | " + "G : " + G + " | " + "B : " + B + " | " + "A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Speculer0 Speculer0Data { get; set; } = new Speculer0();
            public class Speculer0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Speculer0(MTOB.MaterialColor.Speculer0 speculer0)
                {
                    R = speculer0.R;
                    G = speculer0.G;
                    B = speculer0.B;
                    A = speculer0.A;
                }

                public Speculer0(float R, float G, float B, float A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public Speculer0()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Speculer0 : [" + "R : " + R + " | " + "G : " + G + " | " + "B : " + B + " | " + "A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Speculer1 Speculer1Data { get; set; } = new Speculer1();
            public class Speculer1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Speculer1(MTOB.MaterialColor.Speculer1 speculer1)
                {
                    R = speculer1.R;
                    G = speculer1.G;
                    B = speculer1.B;
                    A = speculer1.A;
                }

                public Speculer1(float R, float G, float B, float A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public Speculer1()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Speculer1 : [" + "R : " + R + " | " + "G : " + G + " | " + "B : " + B + " | " + "A : " + A + "]";
                }
            }

            public MaterialColor(MTOB.MaterialColor materialColor)
            {
                EmissionData = new Emission(materialColor.EmissionData);
                AmbientData = new Ambient(materialColor.AmbientData);
                DiffuseData = new Diffuse(materialColor.DiffuseData);
                Speculer0Data = new Speculer0(materialColor.Speculer0Data);
                Speculer1Data = new Speculer1(materialColor.Speculer1Data);
            }

            public MaterialColor()
            {
                EmissionData = new Emission();
                AmbientData = new Ambient();
                DiffuseData = new Diffuse();
                Speculer0Data = new Speculer0();
                Speculer1Data = new Speculer1();
            }

            public override string ToString()
            {
                return "Material Color";
            }
        }

        //ConstantColor
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public ConstantColor ConstantColorData { get; set; } = new ConstantColor();
        public class ConstantColor
        {
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant0 Constant0Data { get; set; } = new Constant0();
            public class Constant0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant0(MTOB.ConstantColor.Constant0 constant0)
                {
                    R = constant0.R;
                    G = constant0.G;
                    B = constant0.B;
                    A = constant0.A;
                }

                public Constant0()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 0 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant1 Constant1Data { get; set; } = new Constant1();
            public class Constant1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant1(MTOB.ConstantColor.Constant1 constant1)
                {
                    R = constant1.R;
                    G = constant1.G;
                    B = constant1.B;
                    A = constant1.A;
                }

                public Constant1()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 1 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant2 Constant2Data { get; set; } = new Constant2();
            public class Constant2
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant2(MTOB.ConstantColor.Constant2 constant2)
                {
                    R = constant2.R;
                    G = constant2.G;
                    B = constant2.B;
                    A = constant2.A;
                }

                public Constant2()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 2 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant3 Constant3Data { get; set; } = new Constant3();
            public class Constant3
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant3(MTOB.ConstantColor.Constant3 constant3)
                {
                    R = constant3.R;
                    G = constant3.G;
                    B = constant3.B;
                    A = constant3.A;
                }

                public Constant3()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 3 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant4 Constant4Data { get; set; } = new Constant4();
            public class Constant4
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant4(MTOB.ConstantColor.Constant4 constant4)
                {
                    R = constant4.R;
                    G = constant4.G;
                    B = constant4.B;
                    A = constant4.A;
                }

                public Constant4()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 4 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public Constant5 Constant5Data { get; set; } = new Constant5();
            public class Constant5
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public Constant5(MTOB.ConstantColor.Constant5 constant5)
                {
                    R = constant5.R;
                    G = constant5.G;
                    B = constant5.B;
                    A = constant5.A;
                }

                public Constant5()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }

                public override string ToString()
                {
                    return "Constant 5 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            public ConstantColor(MTOB.ConstantColor constantColor)
            {
                Constant0Data = new Constant0(constantColor.Constant0Data);
                Constant1Data = new Constant1(constantColor.Constant1Data);
                Constant2Data = new Constant2(constantColor.Constant2Data);
                Constant3Data = new Constant3(constantColor.Constant3Data);
                Constant4Data = new Constant4(constantColor.Constant4Data);
                Constant5Data = new Constant5(constantColor.Constant5Data);
            }

            public ConstantColor()
            {
                Constant0Data = new Constant0();
                Constant1Data = new Constant1();
                Constant2Data = new Constant2();
                Constant3Data = new Constant3();
                Constant4Data = new Constant4();
                Constant5Data = new Constant5();
            }

            public override string ToString()
            {
                return "Constant Color";
            }
        }

        public byte[] UnknownData5 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBitColor1 UnknownBitColor_1 { get; set; } = new UnknownBitColor1(0, 0, 0, 0);
        public class UnknownBitColor1
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public UnknownBitColor1(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public override string ToString()
            {
                return "UnknownBitColor 1 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
            }
        }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBitColor2 UnknownBitColor_2 { get; set; } = new UnknownBitColor2(0, 0, 0, 0);
        public class UnknownBitColor2
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public UnknownBitColor2(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public override string ToString()
            {
                return "UnknownBitColor 2 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
            }
        }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBitColor3 UnknownBitColor_3 { get; set; } = new UnknownBitColor3(0, 0, 0, 0);
        public class UnknownBitColor3
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public UnknownBitColor3(byte R, byte G, byte B, byte A)
            {
                this.R = R;
                this.G = G;
                this.B = B;
                this.A = A;
            }

            public override string ToString()
            {
                return "UnknownBitColor 3 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
            }
        }

        public byte[] UnknownData6 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBitColor4 UnknownBitColor_4 { get; set; } = new UnknownBitColor4();
        public class UnknownBitColor4
        {
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit0 ConstantColorBit_0 { get; set; } = new ConstantColorBit0(0, 0, 0, 0);
            public class ConstantColorBit0
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit0(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 0 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit1 ConstantColorBit_1 { get; set; } = new ConstantColorBit1(0, 0, 0, 0);
            public class ConstantColorBit1
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit1(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 1 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit2 ConstantColorBit_2 { get; set; } = new ConstantColorBit2(0, 0, 0, 0);
            public class ConstantColorBit2
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit2(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 2 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit3 ConstantColorBit_3 { get; set; } = new ConstantColorBit3(0, 0, 0, 0);
            public class ConstantColorBit3
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit3(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 3 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit4 ConstantColorBit_4 { get; set; } = new ConstantColorBit4(0, 0, 0, 0);
            public class ConstantColorBit4
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit4(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 4 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public ConstantColorBit5 ConstantColorBit_5 { get; set; } = new ConstantColorBit5(0, 0, 0, 0);
            public class ConstantColorBit5
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public ConstantColorBit5(byte R, byte G, byte B, byte A)
                {
                    this.R = R;
                    this.G = G;
                    this.B = B;
                    this.A = A;
                }

                public override string ToString()
                {
                    return "ConstantColorBit 5 : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
                }
            }

            public UnknownBitColor4(MTOB.UnknownColorBit4 unknownColorBit4)
            {
                this.ConstantColorBit_0 = new ConstantColorBit0(unknownColorBit4.ConstantBit_0.R, unknownColorBit4.ConstantBit_0.G, unknownColorBit4.ConstantBit_0.B, unknownColorBit4.ConstantBit_0.A);
                this.ConstantColorBit_1 = new ConstantColorBit1(unknownColorBit4.ConstantBit_1.R, unknownColorBit4.ConstantBit_1.G, unknownColorBit4.ConstantBit_1.B, unknownColorBit4.ConstantBit_1.A);
                this.ConstantColorBit_2 = new ConstantColorBit2(unknownColorBit4.ConstantBit_2.R, unknownColorBit4.ConstantBit_2.G, unknownColorBit4.ConstantBit_2.B, unknownColorBit4.ConstantBit_2.A);
                this.ConstantColorBit_3 = new ConstantColorBit3(unknownColorBit4.ConstantBit_3.R, unknownColorBit4.ConstantBit_3.G, unknownColorBit4.ConstantBit_3.B, unknownColorBit4.ConstantBit_3.A);
                this.ConstantColorBit_4 = new ConstantColorBit4(unknownColorBit4.ConstantBit_4.R, unknownColorBit4.ConstantBit_4.G, unknownColorBit4.ConstantBit_4.B, unknownColorBit4.ConstantBit_4.A);
                this.ConstantColorBit_5 = new ConstantColorBit5(unknownColorBit4.ConstantBit_5.R, unknownColorBit4.ConstantBit_5.G, unknownColorBit4.ConstantBit_5.B, unknownColorBit4.ConstantBit_5.A);
            }

            public UnknownBitColor4()
            {
                this.ConstantColorBit_0 = new ConstantColorBit0(0x00, 0x00, 0x00, 0xFF);
                this.ConstantColorBit_1 = new ConstantColorBit1(0x00, 0x00, 0x00, 0xFF);
                this.ConstantColorBit_2 = new ConstantColorBit2(0x00, 0x00, 0x00, 0xFF);
                this.ConstantColorBit_3 = new ConstantColorBit3(0x00, 0x00, 0x00, 0xFF);
                this.ConstantColorBit_4 = new ConstantColorBit4(0x00, 0x00, 0x00, 0xFF);
                this.ConstantColorBit_5 = new ConstantColorBit5(0x00, 0x00, 0x00, 0xFF);
            }

            public override string ToString()
            {
                return "ConstantColorBit";
            }
        }

        public int UnknownData19 { get; set; }
        public int UnknownData20 { get; set; }
        public int UnknownData21 { get; set; }
        public int UnknownData22 { get; set; }
        public int UnknownData23 { get; set; }
        public short UnknownData24 { get; set; }
        public short UnknownData25 { get; set; }
        public int UnknownData26 { get; set; }
        public int UnknownData27 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBit UnknownBits { get; set; } = new UnknownBit(0, 0, 0, 0);
        public class UnknownBit
        {
            public byte Bit0 { get; set; }
            public byte Bit1 { get; set; }
            public byte Bit2 { get; set; }
            public byte Bit3 { get; set; }

            public UnknownBit(MTOB.UnknownBit unknownBit)
            {
                Bit0 = unknownBit.Bit0;
                Bit1 = unknownBit.Bit1;
                Bit2 = unknownBit.Bit2;
                Bit3 = unknownBit.Bit3;
            }

            public UnknownBit(byte b0, byte b1, byte b2, byte b3)
            {
                Bit0 = b0;
                Bit1 = b1;
                Bit2 = b2;
                Bit3 = b3;
            }

            public override string ToString()
            {
                return "UnknownBit : [ " + Bit0 + " | " + Bit1 + " | " + Bit2 + " | " + Bit3 + "]";
            }
        }

        public byte[] UnknownData28 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownBit2 UnknownBit2s { get; set; } = new UnknownBit2(0, 0, 0, 0);
        public class UnknownBit2
        {
            public byte Bit0 { get; set; }
            public byte Bit1 { get; set; }
            public byte Bit2 { get; set; }
            public byte Bit3 { get; set; }

            public UnknownBit2(MTOB.UnknownBit2 unknownBit2)
            {
                Bit0 = unknownBit2.Bit0;
                Bit1 = unknownBit2.Bit1;
                Bit2 = unknownBit2.Bit2;
                Bit3 = unknownBit2.Bit3;
            }

            public UnknownBit2(byte b0, byte b1, byte b2, byte b3)
            {
                Bit0 = b0;
                Bit1 = b1;
                Bit2 = b2;
                Bit3 = b3;
            }

            public override string ToString()
            {
                return "UnknownBit2 : [ " + Bit0 + " | " + Bit1 + " | " + Bit2 + " | " + Bit3 + "]";
            }
        }

        public int UnknownData29 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public BlendColor BlendColorData { get; set; } = new BlendColor(0, 0, 0, 0);
        public class BlendColor
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public BlendColor(MTOB.BlendColor blendColor)
            {
                R = blendColor.R;
                G = blendColor.G;
                B = blendColor.B;
                A = blendColor.A;
            }

            public BlendColor(float Input_R, float Input_G, float Input_B, float Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }

            public BlendColor()
            {
                R = 0;
                G = 0;
                B = 0;
                A = 0;
            }

            public override string ToString()
            {
                return "Blend Color : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A +"]";
            }
        }

        public byte[] UnknownData7 { get; set; } //Color (?), 4byte
        public byte[] UnknownData8 { get; set; } //Color (?), 4byte

        public short UnknownData9 { get; set; } //2byte (?)
        public byte UnknownData10 { get; set; } //bool(?)
        public byte UnknownData11 { get; set; } //bool(?)

        public byte[] UnknownData12 { get; set; } //4byte

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public BlendColorBit BlendColorBitData { get; set; } = new BlendColorBit(0, 0, 0, 0);
        public class BlendColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public BlendColorBit(MTOB.BlendColorBit blendColorBit)
            {
                R = blendColorBit.R;
                G = blendColorBit.G;
                B = blendColorBit.B;
                A = blendColorBit.A;
            }

            public BlendColorBit(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }

            public override string ToString()
            {
                return "Blend Color (Bit)";
            }
        }

        public class UnknownColorBitSet
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public UnknownColorBitSet(MTOB.UnknownColorBitSet unknownColorBitSet)
            {
                R = unknownColorBitSet.R;
                G = unknownColorBitSet.G;
                B = unknownColorBitSet.B;
                A = unknownColorBitSet.A;
            }

            public UnknownColorBitSet(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }

            public override string ToString()
            {
                return "Unknown Color Bit : [ R : " + R + " | G : " + G + " | B : " + B + " | A : " + A + "]";
            }
        }

        //... => 20byte

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColorBitSet UnknownColorBitSet1 { get; set; } = new UnknownColorBitSet(0, 0, 0, 0);

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColorBitSet UnknownColorBitSet2 { get; set; } = new UnknownColorBitSet(0, 0, 0, 0);

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColorBitSet UnknownColorBitSet3 { get; set; } = new UnknownColorBitSet(0, 0, 0, 0);

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColorBitSet UnknownColorBitSet4 { get; set; } = new UnknownColorBitSet(0, 0, 0, 0);

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownColorBitSet UnknownColorBitSet5 { get; set; } = new UnknownColorBitSet(0, 0, 0, 0);


        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<TextureUVTransform>))]
        public List<TextureUVTransform> TextureUVTransformSetting { get; set; } = new List<TextureUVTransform>();
        public class TextureUVTransform
        {
            public int UVSetNumber { get; set; }
            public MTOB.TextureUVTransform.MappingType MappingType { get; set; }
            public float UnknownData8 { get; set; }
            public MTOB.TextureUVTransform.CalculateTextureCoordinateType CalculateTextureCoordinateType { get; set; }

            public float ScaleU { get; set; } //ScaleU
            public float ScaleV { get; set; } //ScaleV
            public float Rotate { get; set; } //Rotate
            public float TranslateU { get; set; } //TranslateU
            public float TranslateV { get; set; } //TranslateV
            public System.Windows.Media.Matrix Matrix { get; set; }

            public float UnknownData9 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public MTOB.TextureUVTransform.Matrix3x4 Matrix34 { get; set; } = new MTOB.TextureUVTransform.Matrix3x4();

            public TextureUVTransform(MTOB.TextureUVTransform textureUVTransform)
            {
                UVSetNumber = textureUVTransform.UVSetNumber;
                MappingType = textureUVTransform.Mapping_Type;
                UnknownData8 = textureUVTransform.UnknownData8;
                CalculateTextureCoordinateType = textureUVTransform.CalculateTextureCoordType;
                ScaleU = textureUVTransform.ScaleU;
                ScaleV = textureUVTransform.ScaleV;
                Rotate = textureUVTransform.Rotate;
                TranslateU = textureUVTransform.TranslateU;
                TranslateV = textureUVTransform.TranslateV;
                Matrix = textureUVTransform.Matrix;
                UnknownData9 = textureUVTransform.UnknownData9;
                Matrix34 = textureUVTransform.MatrixData;
            }

            public TextureUVTransform()
            {
                UVSetNumber = 0;
                MappingType = MTOB.TextureUVTransform.MappingType.TextureCoordinate;
                UnknownData8 = 0;
                CalculateTextureCoordinateType = MTOB.TextureUVTransform.CalculateTextureCoordinateType.AutodeskMaya;
                ScaleU = 0;
                ScaleV = 0;
                Rotate = 0;
                TranslateU = 0;
                TranslateV = 0;
                Matrix = System.Windows.Media.Matrix.Identity;
                UnknownData9 = 0;
                Matrix34 = new MTOB.TextureUVTransform.Matrix3x4();
            }

            public override string ToString()
            {
                return "TextureUVTransformSetting";
            }
        }

        //[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        //public MaterialInfoSlot MaterialInfo1 { get; set; } = new MaterialInfoSlot(1);

        //[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        //public MaterialInfoSlot MaterialInfo2 { get; set; } = new MaterialInfoSlot(2);

        //[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        //public MaterialInfoSlot MaterialInfo3 { get; set; } = new MaterialInfoSlot(3);

        //[TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        //public MaterialInfoSlot MaterialInfo4 { get; set; } = new MaterialInfoSlot(4);

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomListTypeConverter<MaterialInfoSlot>))]
        public List<MaterialInfoSlot> MaterialInfoSlotList { get; set; } = new List<MaterialInfoSlot>();
        public class MaterialInfoSlot
        {
            
            public int Index { get; }

            public string Name
            {
                get
                {
                    return TXOBMaterialSectionSetting.Name;
                }
            }

            public string SubName
            {
                get
                {
                    return TXOBMaterialSectionSetting.MTOB_MaterialName;
                }
            }

            //public string SubName => TXOBMaterialSectionSetting.MTOB_MaterialName;

            public Flags Flags { get; set; } //0x00000080
            public int UnknownData { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public TXOB_MaterialSectionSetting TXOBMaterialSectionSetting { get; set; } = new TXOB_MaterialSectionSetting();
            public class TXOB_MaterialSectionSetting
            {
                public string Name { get; }
                //public char[] TXOB_Header { get; set; }
                public int UnknownData1 { get; set; } //Reverse (?)
                //public int TXOBNameOffset { get; set; } //String itself is always empty (?)

                public byte[] UnknownByte1 { get; set; }
                public byte[] UnknownByte2 { get; set; }

                public string MTOB_MaterialName { get; }
                //public int MTOB_MaterialNameOffset { get; set; }

                public int UnknownByte3 { get; set; }
                public int UnknownByte4 { get; set; } //Reverse (?)

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public UnknownBitSet Unknown_BitSet { get; set; } = new UnknownBitSet();
                public class UnknownBitSet
                {
                    public byte Bit0 { get; set; }
                    public byte Bit1 { get; set; }
                    public byte Bit2 { get; set; }
                    public byte Bit3 { get; set; }

                    public UnknownBitSet(TXOB.MaterialInfo.UnknownBitSet unknownBitSet)
                    {
                        Bit0 = unknownBitSet.Bit0;
                        Bit1 = unknownBitSet.Bit1;
                        Bit2 = unknownBitSet.Bit2;
                        Bit3 = unknownBitSet.Bit3;
                    }

                    public UnknownBitSet()
                    {
                        Bit0 = 0;
                        Bit1 = 0;
                        Bit2 = 0;
                        Bit3 = 0;
                    }

                    public override string ToString()
                    {
                        return "UnknownBitSet";
                    }
                }

                public int UnknownByte5 { get; set; }
                public int UnknownByte6 { get; set; }
                public int UnknownByte7 { get; set; }
                public int UnknownByte8 { get; set; }

                public float UnknownByte9 { get; set; }
                public int UnknownByte10 { get; set; }

                public TXOB_MaterialSectionSetting(TXOB.MaterialInfo materialInfo)
                {
                    Name = materialInfo.Name;
                    UnknownData1 = materialInfo.UnknownData1;

                    UnknownByte1 = materialInfo.UnknownByte1;
                    UnknownByte2 = materialInfo.UnknownByte2;

                    MTOB_MaterialName = materialInfo.MTOB_MaterialName;

                    UnknownByte3 = materialInfo.UnknownByte3;
                    UnknownByte4 = materialInfo.UnknownByte4;

                    Unknown_BitSet = new UnknownBitSet(materialInfo.unknownBitSet);

                    UnknownByte5 = materialInfo.UnknownByte5;
                    UnknownByte6 = materialInfo.UnknownByte6;
                    UnknownByte7 = materialInfo.UnknownByte7;
                    UnknownByte8 = materialInfo.UnknownByte8;
                    UnknownByte9 = materialInfo.UnknownByte9;
                    UnknownByte10 = materialInfo.UnknownByte10;
                }

                public TXOB_MaterialSectionSetting()
                {
                    Name = " ";
                    UnknownData1 = 0;

                    UnknownByte1 = new byte[4];
                    UnknownByte2 = new byte[4];

                    MTOB_MaterialName = " ";

                    UnknownByte3 = 0;
                    UnknownByte4 = 0;

                    Unknown_BitSet = new UnknownBitSet();

                    UnknownByte5 = 0;
                    UnknownByte6 = 0;
                    UnknownByte7 = 0;
                    UnknownByte8 = 0;
                    UnknownByte9 = 0;
                    UnknownByte10 = 0;
                }

                public override string ToString()
                {
                    return Name.Replace("\0", "") + " | " + MTOB_MaterialName.Replace("\0", "");
                }
            }

            //public int UnknownDataOffset0 { get; set; }

            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSection1 UnknownDataSection_1 { get; set; } = new UnknownDataSection1();
            public class UnknownDataSection1
            {
                public Flags UnknownSectionFlag { get; set; }
                public byte[] UnknownData0 { get; set; } //0x4
                public byte[] UnknownData1 { get; set; } //0x4
                public byte[] UnknownData2 { get; set; } //0x14 (20byte)

                public UnknownDataSection1(MTOB.MaterialInfoSet.UnknownDataSection1 unknownDataSection1)
                {
                    UnknownSectionFlag = unknownDataSection1.UnknownSectionFlag;
                    UnknownData0 = unknownDataSection1.UnknownData0;
                    UnknownData1 = unknownDataSection1.UnknownData1;
                    UnknownData2 = unknownDataSection1.UnknownData2;
                }

                public UnknownDataSection1()
                {
                    UnknownSectionFlag = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x80 });
                    UnknownData0 = new byte[4];
                    UnknownData1 = new byte[4];
                    UnknownData2 = new byte[20];
                }

                public override string ToString()
                {
                    return "UnknownDataSection 1";
                }
            }

            //0x4 * 6, Float, Color(?)
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSection2 UnknownDataSection_2 { get; set; } = new UnknownDataSection2();
            public class UnknownDataSection2
            {
                public float UnknownValue0 { get; set; }
                public float UnknownValue1 { get; set; }
                public float UnknownValue2 { get; set; }
                public float UnknownValue3 { get; set; }
                public float UnknownValue4 { get; set; }
                public float UnknownValue5 { get; set; }

                public UnknownDataSection2(MTOB.MaterialInfoSet.UnknownDataSection2 unknownDataSection2)
                {
                    UnknownValue0 = unknownDataSection2.UnknownValue0;
                    UnknownValue1 = unknownDataSection2.UnknownValue1;
                    UnknownValue2 = unknownDataSection2.UnknownValue2;
                    UnknownValue3 = unknownDataSection2.UnknownValue3;
                    UnknownValue4 = unknownDataSection2.UnknownValue4;
                    UnknownValue5 = unknownDataSection2.UnknownValue5;
                }

                public UnknownDataSection2()
                {
                    UnknownValue0 = 0;
                    UnknownValue1 = 0;
                    UnknownValue2 = 0;
                    UnknownValue3 = 0;
                    UnknownValue4 = 0;
                    UnknownValue5 = 0;
                }

                public override string ToString()
                {
                    return "UnknownDataSection 2";
                }
            }

            public byte[] UnknownByteData0 { get; set; } //32byte

            //public int UnknownDataOffset1 { get; set; }

            //0x4 * 3 : Float(?)
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSection3 UnknownDataSection_3 { get; set; } = new UnknownDataSection3();
            public class UnknownDataSection3
            {
                public float UnknownValue0 { get; set; }
                public float UnknownValue1 { get; set; }
                public float UnknownValue2 { get; set; }

                public UnknownDataSection3(MTOB.MaterialInfoSet.UnknownDataSection3 unknownDataSection3)
                {
                    UnknownValue0 = unknownDataSection3.UnknownValue0;
                    UnknownValue1 = unknownDataSection3.UnknownValue1;
                    UnknownValue2 = unknownDataSection3.UnknownValue2;
                }

                public UnknownDataSection3()
                {
                    UnknownValue0 = 0;
                    UnknownValue1 = 0;
                    UnknownValue2 = 0;
                }

                public override string ToString()
                {
                    return "UnknownDataSection 3";
                }
            }


            public MaterialInfoSlot(MTOB.MaterialInfoSet materialInfoSet, int Index)
            {
                this.Index = Index;

                Flags = materialInfoSet.Flags;
                UnknownData = materialInfoSet.UnknownData;

                //MTOB.MaterialInfoSet materialInfoSet = null;
                if (materialInfoSet.TXOBMaterialDataSection.TXOB_MaterialSection != null)
                {
                    TXOBMaterialSectionSetting = new TXOB_MaterialSectionSetting(materialInfoSet.TXOBMaterialDataSection.TXOB_MaterialSection.MaterialInfoSection);
                }
                else if (materialInfoSet.TXOBMaterialDataSection.TXOB_MaterialSection == null)
                {
                    TXOBMaterialSectionSetting = new TXOB_MaterialSectionSetting(new TXOB.MaterialInfo());
                }

                //TXOBMaterialSectionSetting = new TXOB_MaterialSectionSetting(materialInfoSet.TXOBMaterialDataSection.TXOB_MaterialSection.MaterialInfoSection);

                UnknownDataSection_1 = new UnknownDataSection1(materialInfoSet.UnknownDataSection_1);
                UnknownDataSection_2 = new UnknownDataSection2(materialInfoSet.UnknownDataSection_2);

                UnknownByteData0 = materialInfoSet.UnknownByteData0;
                
                UnknownDataSection_3 = new UnknownDataSection3(materialInfoSet.UnknownDataSection_3);
            }

            public MaterialInfoSlot(int Index)
            {
                this.Index = Index;

                Flags = new Flags(new byte[4]);
                UnknownData = 0;
                //TXOBMaterialDataSection = new CGFXData(new byte[4]);

                TXOBMaterialSectionSetting = new TXOB_MaterialSectionSetting();

                UnknownDataSection_1 = new UnknownDataSection1();
                UnknownDataSection_2 = new UnknownDataSection2();

                UnknownByteData0 = new byte[32];

                UnknownDataSection_3 = new UnknownDataSection3();
            }

            public override string ToString()
            {
                return "MaterialInfo " + Index;
                //return "MaterialInfo " + Index + " : " + SubName.Replace("\0", "");
            }
        }


        [Editor(typeof(SHDREditor), typeof(UITypeEditor))]
        public SHDR SHDRData { get; set; }

        public MTOB_PropertyGrid(MTOB MTOB)
        {
            Name = MTOB.Name;
            //MTOB_Header = "MTOB".ToArray();
            Revision = MTOB.Revision;
            //NameOffset = 0;
            UnknownData1 = MTOB.UnknownData1;
            UnknownData2 = MTOB.UnknownData2;
            LightSetting = MTOB.LightSettings;
            UnknownData4 = MTOB.UnknownData4;
            DrawingLayer = MTOB.DrawingLayer;

            MaterialColors = new MaterialColor(MTOB.MaterialColors);
            ConstantColorData = new ConstantColor(MTOB.ConstantColorData);

            UnknownData5 = MTOB.UnknownData5;

            UnknownBitColor_1 = new UnknownBitColor1(MTOB.UnknownColorBit1Data.R, MTOB.UnknownColorBit1Data.G, MTOB.UnknownColorBit1Data.B, MTOB.UnknownColorBit1Data.A);
            UnknownBitColor_2 = new UnknownBitColor2(MTOB.UnknownColorBit2Data.R, MTOB.UnknownColorBit2Data.G, MTOB.UnknownColorBit2Data.B, MTOB.UnknownColorBit2Data.A);
            UnknownBitColor_3 = new UnknownBitColor3(MTOB.UnknownColorBit3Data.R, MTOB.UnknownColorBit3Data.G, MTOB.UnknownColorBit3Data.B, MTOB.UnknownColorBit3Data.A);
            UnknownData6 = MTOB.UnknownData6;
            UnknownBitColor_4 = new UnknownBitColor4(MTOB.UnknownColorBit4Data);

            UnknownData19 = MTOB.UnknownData19;
            UnknownData20 = MTOB.UnknownData20;
            UnknownData21 = MTOB.UnknownData21;
            UnknownData22 = MTOB.UnknownData22;
            UnknownData23 = MTOB.UnknownData23;
            UnknownData24 = MTOB.UnknownData24;
            UnknownData24 = MTOB.UnknownData25;
            UnknownData26 = MTOB.UnknownData26;
            UnknownData27 = MTOB.UnknownData27;

            UnknownBits = new UnknownBit(MTOB.UnknownBits);
            UnknownData28 = MTOB.UnknownData28;

            UnknownBit2s = new UnknownBit2(MTOB.UnknownBit2s);
            UnknownData29 = MTOB.UnknownData29;

            BlendColorData = new BlendColor(MTOB.BlendColorData);
            UnknownData7 = MTOB.UnknownData7;
            UnknownData8 = MTOB.UnknownData8;
            UnknownData9 = MTOB.UnknownData9;
            UnknownData10 = MTOB.UnknownData10;
            UnknownData11 = MTOB.UnknownData11;
            UnknownData12 = MTOB.UnknownData12;

            BlendColorBitData = new BlendColorBit(MTOB.BlendColorBitData);
            UnknownColorBitSet1 = new UnknownColorBitSet(MTOB.UnknownColorBitSet1);
            UnknownColorBitSet2 = new UnknownColorBitSet(MTOB.UnknownColorBitSet2);
            UnknownColorBitSet3 = new UnknownColorBitSet(MTOB.UnknownColorBitSet3);
            UnknownColorBitSet4 = new UnknownColorBitSet(MTOB.UnknownColorBitSet4);
            UnknownColorBitSet5 = new UnknownColorBitSet(MTOB.UnknownColorBitSet5);

            foreach (var TextureUVTransform in MTOB.TextureUVTransformSettingList)
            {

                //this.UVSetNumber = MTOB.UVSetNumber;
                //this.MappingType = MTOB.MappingType;
                TextureUVTransformSetting.Add(new TextureUVTransform(TextureUVTransform));
            }

            MaterialInfoSlotList.Add(new MaterialInfoSlot(MTOB.MaterialInfoSet1, 1));
            MaterialInfoSlotList.Add(new MaterialInfoSlot(MTOB.MaterialInfoSet2, 2));
            MaterialInfoSlotList.Add(new MaterialInfoSlot(MTOB.MaterialInfoSet3, 3));
            MaterialInfoSlotList.Add(new MaterialInfoSlot(MTOB.MaterialInfoSet4, 4));

            //MaterialInfo1 = new MaterialInfoSlot(MTOB.MaterialInfoSet1, 1);
            //MaterialInfo2 = new MaterialInfoSlot(MTOB.MaterialInfoSet2, 2);
            //MaterialInfo3 = new MaterialInfoSlot(MTOB.MaterialInfoSet3, 3);
            //MaterialInfo4 = new MaterialInfoSlot(MTOB.MaterialInfoSet4, 4);

            SHDRData = MTOB.SHDRData.SHDR;

        }

        public MTOB_PropertyGrid()
        {
            Name = " ";
            //MTOB_Header = "MTOB".ToArray();
            Revision = new List<byte>().ToArray();
            //NameOffset = 0;
            UnknownData1 = 0;
            UnknownData2 = 0;
            LightSetting = new MTOB.LightSetting(0);
            UnknownData4 = 0;
            DrawingLayer = 0;

            MaterialColors = new MaterialColor();
            ConstantColorData = new ConstantColor();

            UnknownData5 = new List<byte>().ToArray();

            UnknownBitColor_1 = new UnknownBitColor1(0, 0, 0, 0);
            UnknownBitColor_2 = new UnknownBitColor2(0, 0, 0, 0);
            UnknownBitColor_3 = new UnknownBitColor3(0, 0, 0, 0);
            UnknownData6 = new List<byte>().ToArray();
            UnknownBitColor_4 = new UnknownBitColor4();

            UnknownData19 = 0;
            UnknownData20 = 0;
            UnknownData21 = 0;
            UnknownData22 = 0;
            UnknownData23 = 0;
            UnknownData24 = 0;
            UnknownData24 = 0;
            UnknownData26 = 0;
            UnknownData27 = 0;

            UnknownBits = new UnknownBit(0, 0, 0, 0);
            UnknownData28 = new List<byte>().ToArray();

            UnknownBit2s = new UnknownBit2(0, 0, 0, 0);
            UnknownData29 = 0;

            BlendColorData = new BlendColor(0, 0, 0, 0);
            UnknownData7 = new List<byte>().ToArray();
            UnknownData8 = new List<byte>().ToArray();
            UnknownData9 = 0;
            UnknownData10 = 0;
            UnknownData11 = 0;
            UnknownData12 = new List<byte>().ToArray();

            BlendColorBitData = new BlendColorBit(0, 0, 0, 0);
            UnknownColorBitSet1 = new UnknownColorBitSet(0, 0, 0, 0);
            UnknownColorBitSet2 = new UnknownColorBitSet(0, 0, 0, 0);
            UnknownColorBitSet3 = new UnknownColorBitSet(0, 0, 0, 0);
            UnknownColorBitSet4 = new UnknownColorBitSet(0, 0, 0, 0);
            UnknownColorBitSet5 = new UnknownColorBitSet(0, 0, 0, 0);

            TextureUVTransformSetting = new List<TextureUVTransform>();

            MaterialInfoSlotList = new List<MaterialInfoSlot>();
            //MaterialInfo1 = new MaterialInfoSlot(1);
            //MaterialInfo2 = new MaterialInfoSlot(2);
            //MaterialInfo3 = new MaterialInfoSlot(3);
            //MaterialInfo4 = new MaterialInfoSlot(4);

            SHDRData = new SHDR();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
