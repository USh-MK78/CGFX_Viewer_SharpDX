using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Material (0x00000008)
    /// </summary>
    public class MTOB : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;

        public char[] MTOB_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }
        public LightSetting LightSettings { get; set; } //IsFragmentLighting = 1, IsVertexLighting = 2, IsHemiSphereLighting = 4, EnableOcclusion = 8
        public class LightSetting
        {
            public int Value;
            public bool IsFragmentLighting
            {
                get => ((Value & 1) != 0);
                set
                {
                    //Off=Xor, On=Or
                    if (value == true) Value = Value | 1;
                    if (value == false) Value = Value ^ 1;
                }
            }

            public bool IsVertexLighting
            {
                get => ((Value & 2) != 0);
                set
                {
                    //Off=Xor, On=Or
                    if (value == true) Value = Value | 2;
                    if (value == false) Value = Value ^ 2;
                }
            }

            public bool IsHemiSphereLighting
            {
                get => ((Value & 4) != 0);
                set
                {
                    //Off=Xor, On=Or
                    if (value == true) Value = Value | 4;
                    if (value == false) Value = Value ^ 4;
                }
            }

            public bool EnableOcclusion
            {
                get
                {
                    bool b = new bool();
                    if (IsHemiSphereLighting == false) b = false;
                    if (IsHemiSphereLighting == true) b = ((Value & 8) != 0);
                    return b;
                }
                set
                {
                    if (IsHemiSphereLighting == false) Value = Value ^ 8;
                    if (IsHemiSphereLighting == true)
                    {
                        //Off=Xor, On=Or
                        if (value == true) Value = Value | 8;
                        if (value == false) Value = Value ^ 8;
                    }
                }
            }

            public LightSetting(int Flags)
            {
                Value = Flags;
            }
        }

        //public int IsFragmentLighting { get; set; }
        public int UnknownData4 { get; set; }
        public int DrawingLayer { get; set; }

        //MaterialColor
        public MaterialColor MaterialColors { get; set; }
        public class MaterialColor
        {
            public Emission EmissionData { get; set; }
            public class Emission
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; } //Emission Alpha (?)

                public void ReadEmissionColor(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Emission(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Emission()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Ambient AmbientData { get; set; }
            public class Ambient
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; } //Ambient Alpha (?)

                public void ReadAmbientColor(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Ambient(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Ambient()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }
            }

            public Diffuse DiffuseData { get; set; }
            public class Diffuse
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadDiffuseColor(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Diffuse(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Diffuse()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }
            }

            public Speculer0 Speculer0Data { get; set; }
            public class Speculer0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; } //Speculer0 Alpha (?)

                public void ReadSpeculer0Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Speculer0(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Speculer0()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }
            }

            public Speculer1 Speculer1Data { get; set; }
            public class Speculer1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; } //Speculer1 Alpha (?)

                public void ReadSpeculer1Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Speculer1(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Speculer1()
                {
                    R = 1;
                    G = 1;
                    B = 1;
                    A = 1;
                }
            }

            public void ReadMaterialColor(BinaryReader br, byte[] BOM)
            {
                EmissionData.ReadEmissionColor(br, BOM);
                AmbientData.ReadAmbientColor(br, BOM);
                DiffuseData.ReadDiffuseColor(br, BOM);
                Speculer0Data.ReadSpeculer0Color(br, BOM);
                Speculer1Data.ReadSpeculer1Color(br, BOM);
            }

            public MaterialColor()
            {
                EmissionData = new Emission();
                AmbientData = new Ambient();
                DiffuseData = new Diffuse();
                Speculer0Data = new Speculer0();
                Speculer1Data = new Speculer1();
            }
        }

        //ConstantColor
        public ConstantColor ConstantColorData { get; set; }
        public class ConstantColor
        {
            public Constant0 Constant0Data { get; set; }
            public class Constant0
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant0Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant0(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant0()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Constant1 Constant1Data { get; set; }
            public class Constant1
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant1Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant1(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant1()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Constant2 Constant2Data { get; set; }
            public class Constant2
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant2Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant2(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant2()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Constant3 Constant3Data { get; set; }
            public class Constant3
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant3Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant3(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant3()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Constant4 Constant4Data { get; set; }
            public class Constant4
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant4Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant4(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant4()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public Constant5 Constant5Data { get; set; }
            public class Constant5
            {
                public float R { get; set; }
                public float G { get; set; }
                public float B { get; set; }
                public float A { get; set; }

                public void ReadConstant5Color(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public SharpDX.Color4 GetColor4()
                {
                    return new Color4(R, G, B, A);
                }

                public Constant5(float Input_R, float Input_G, float Input_B, float Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }

                public Constant5()
                {
                    R = 0;
                    G = 0;
                    B = 0;
                    A = 1;
                }
            }

            public void ReadConstantColor(BinaryReader br, byte[] BOM)
            {
                Constant0Data.ReadConstant0Color(br, BOM);
                Constant1Data.ReadConstant1Color(br, BOM);
                Constant2Data.ReadConstant2Color(br, BOM);
                Constant3Data.ReadConstant3Color(br, BOM);
                Constant4Data.ReadConstant4Color(br, BOM);
                Constant5Data.ReadConstant5Color(br, BOM);
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
        }

        public byte[] UnknownData5 { get; set; }

        public UnknownColorBit1 UnknownColorBit1Data { get; set; }
        public class UnknownColorBit1
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadUnknownColorBit1(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public System.Windows.Media.Color GetColor()
            {
                return System.Windows.Media.Color.FromArgb(A, R, G, B);
            }

            public UnknownColorBit1(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        public UnknownColorBit2 UnknownColorBit2Data { get; set; }
        public class UnknownColorBit2
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadUnknownColorBit2(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public System.Windows.Media.Color GetColor()
            {
                return System.Windows.Media.Color.FromArgb(A, R, G, B);
            }

            public UnknownColorBit2(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        public UnknownColorBit3 UnknownColorBit3Data { get; set; }
        public class UnknownColorBit3
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadUnknownColorBit3(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public UnknownColorBit3(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        public byte[] UnknownData6 { get; set; }

        public UnknownColorBit4 UnknownColorBit4Data { get; set; } //ConstantColorBit(?)
        public class UnknownColorBit4
        {
            public ConstantBit0 ConstantBit_0 { get; set; }
            public class ConstantBit0
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit0(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit0(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public ConstantBit1 ConstantBit_1 { get; set; }
            public class ConstantBit1
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit1(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit1(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public ConstantBit2 ConstantBit_2 { get; set; }
            public class ConstantBit2
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit2(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit2(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public ConstantBit3 ConstantBit_3 { get; set; }
            public class ConstantBit3
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit3(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit3(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public ConstantBit4 ConstantBit_4 { get; set; }
            public class ConstantBit4
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit4(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit4(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public ConstantBit5 ConstantBit_5 { get; set; }
            public class ConstantBit5
            {
                public byte R { get; set; }
                public byte G { get; set; }
                public byte B { get; set; }
                public byte A { get; set; }

                public void ReadConstantBit5(BinaryReader br)
                {
                    R = br.ReadByte();
                    G = br.ReadByte();
                    B = br.ReadByte();
                    A = br.ReadByte();
                }

                public System.Windows.Media.Color GetColor()
                {
                    return System.Windows.Media.Color.FromArgb(A, R, G, B);
                }

                public ConstantBit5(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
                {
                    R = Input_R;
                    G = Input_G;
                    B = Input_B;
                    A = Input_A;
                }
            }

            public void ReadUnknownColorBit4(BinaryReader br)
            {
                ConstantBit_0 = new ConstantBit0(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                ConstantBit_1 = new ConstantBit1(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                ConstantBit_2 = new ConstantBit2(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                ConstantBit_3 = new ConstantBit3(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                ConstantBit_4 = new ConstantBit4(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
                ConstantBit_5 = new ConstantBit5(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
            }

            public UnknownColorBit4()
            {
                ConstantBit_0 = new ConstantBit0(0x00, 0x00, 0x00, 0xFF);
                ConstantBit_1 = new ConstantBit1(0x00, 0x00, 0x00, 0xFF);
                ConstantBit_2 = new ConstantBit2(0x00, 0x00, 0x00, 0xFF);
                ConstantBit_3 = new ConstantBit3(0x00, 0x00, 0x00, 0xFF);
                ConstantBit_4 = new ConstantBit4(0x00, 0x00, 0x00, 0xFF);
                ConstantBit_5 = new ConstantBit5(0x00, 0x00, 0x00, 0xFF);
            }
        }


        #region 1
        public int UnknownData19 { get; set; }
        public int UnknownData20 { get; set; }
        public int UnknownData21 { get; set; }
        public int UnknownData22 { get; set; }
        public int UnknownData23 { get; set; }
        public short UnknownData24 { get; set; }
        public short UnknownData25 { get; set; }
        public int UnknownData26 { get; set; }
        public int UnknownData27 { get; set; }

        public UnknownBit UnknownBits { get; set; }
        public class UnknownBit
        {
            public byte Bit0 { get; set; }
            public byte Bit1 { get; set; }
            public byte Bit2 { get; set; }
            public byte Bit3 { get; set; }

            public void ReadUnknownBit(BinaryReader br)
            {
                Bit0 = br.ReadByte();
                Bit1 = br.ReadByte();
                Bit2 = br.ReadByte();
                Bit3 = br.ReadByte();
            }

            public UnknownBit(byte b0, byte b1, byte b2, byte b3)
            {
                Bit0 = b0;
                Bit1 = b1;
                Bit2 = b2;
                Bit3 = b3;
            }
        }

        public byte[] UnknownData28 { get; set; }

        public UnknownBit2 UnknownBit2s { get; set; }
        public class UnknownBit2
        {
            public byte Bit0 { get; set; }
            public byte Bit1 { get; set; }
            public byte Bit2 { get; set; }
            public byte Bit3 { get; set; }

            public void ReadUnknownBit2(BinaryReader br)
            {
                Bit0 = br.ReadByte();
                Bit1 = br.ReadByte();
                Bit2 = br.ReadByte();
                Bit3 = br.ReadByte();
            }

            public UnknownBit2(byte b0, byte b1, byte b2, byte b3)
            {
                Bit0 = b0;
                Bit1 = b1;
                Bit2 = b2;
                Bit3 = b3;
            }
        }

        public int UnknownData29 { get; set; }
        #endregion



        public BlendColor BlendColorData { get; set; }
        public class BlendColor
        {
            public float R { get; set; }
            public float G { get; set; }
            public float B { get; set; }
            public float A { get; set; }

            public void ReadBlendColor(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public BlendColor(float Input_R, float Input_G, float Input_B, float Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        public byte[] UnknownData7 { get; set; } //Color (?), 4byte
        public byte[] UnknownData8 { get; set; } //Color (?), 4byte

        public short UnknownData9 { get; set; } //2byte (?)
        public byte UnknownData10 { get; set; } //bool(?)
        public byte UnknownData11 { get; set; } //bool(?)

        public byte[] UnknownData12 { get; set; } //4byte

        public BlendColorBit BlendColorBitData { get; set; }
        public class BlendColorBit
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadBlendColorBit(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public BlendColorBit(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        public class UnknownColorBitSet
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }
            public byte A { get; set; }

            public void ReadUnknownColorBitSet(BinaryReader br)
            {
                R = br.ReadByte();
                G = br.ReadByte();
                B = br.ReadByte();
                A = br.ReadByte();
            }

            public UnknownColorBitSet(byte Input_R, byte Input_G, byte Input_B, byte Input_A)
            {
                R = Input_R;
                G = Input_G;
                B = Input_B;
                A = Input_A;
            }
        }

        //... => 20byte
        public UnknownColorBitSet UnknownColorBitSet1 { get; set; }
        public UnknownColorBitSet UnknownColorBitSet2 { get; set; }
        public UnknownColorBitSet UnknownColorBitSet3 { get; set; }
        public UnknownColorBitSet UnknownColorBitSet4 { get; set; }
        public UnknownColorBitSet UnknownColorBitSet5 { get; set; }


        


        public int TextureUVTransformSlotCount { get; set; }

        public List<TextureUVTransform> TextureUVTransformSettingList { get; set; }
        public class TextureUVTransform
        {
            public enum MappingType
            {
                TextureCoordinate = 0,
                CameraCubeCoordinate = 1,
                CameraSphereCoordinate = 2,
                ProjectionMapping = 3
            }

            public enum CalculateTextureCoordinateType
            {
                AutodeskMaya = 0,
                Autodesk3dsMax = 1,
                SoftImage = 2
            }

            public int UVSetNumber { get; set; }

            public int MappingTypeValue { get; set; }
            public MappingType Mapping_Type
            {
                get
                {
                    return (MappingType)Enum.ToObject(typeof(MappingType), MappingTypeValue);
                }
                set
                {
                    MappingTypeValue = (int)value;
                }
            }

            public float UnknownData8 { get; set; }

            public int CalculateTextureCoordinateTypeValue { get; set; }
            public CalculateTextureCoordinateType CalculateTextureCoordType
            {
                get
                {
                    return (CalculateTextureCoordinateType)Enum.ToObject(typeof(CalculateTextureCoordinateType), CalculateTextureCoordinateTypeValue);
                }
                set
                {
                    CalculateTextureCoordinateTypeValue = (int)value; //value
                }
            }

            public float ScaleU { get; set; } //ScaleU
            public float ScaleV { get; set; } //ScaleV
            public float Rotate { get; set; } //Rotate
            public float TranslateU { get; set; } //TranslateU
            public float TranslateV { get; set; } //TranslateV

            public System.Windows.Media.Matrix Matrix { get => GetTextureMatrix(); }
            public System.Windows.Media.Matrix GetTextureMatrix()
            {
                System.Windows.Media.Matrix matrix = new System.Windows.Media.Matrix();
                matrix.Scale(ScaleU, ScaleV);
                matrix.RotateAt(Rotate, 0, 0);
                matrix.Translate(TranslateU, TranslateV);
                return matrix;
            }

            public float UnknownData9 { get; set; }

            public Matrix3x4 MatrixData { get; set; }
            public struct Matrix3x4
            {
                public float M11;
                public float M12;
                public float M13;
                public float M14;

                public float M21;
                public float M22;
                public float M23;
                public float M24;

                public float M31;
                public float M32;
                public float M33;
                public float M34;

                public Matrix3x4(float[] array)
                {
                    M11 = array[0];
                    M12 = array[1];
                    M13 = array[2];
                    M14 = array[3];

                    M21 = array[4];
                    M22 = array[5];
                    M23 = array[6];
                    M24 = array[7];

                    M31 = array[8];
                    M32 = array[9];
                    M33 = array[10];
                    M34 = array[11];
                }
            }

            public Matrix3x4 GetMatrix(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);

                float[] ary = new float[12];
                for (int i = 0; i < 12; i++)
                {
                    ary[i] = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                return new Matrix3x4(ary);
            }

            public void ReadTextureUVTransform(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UVSetNumber = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                MappingTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownData8 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                CalculateTextureCoordinateTypeValue = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                ScaleU = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                ScaleV = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Rotate = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                TranslateU = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                TranslateV = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownData9 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                MatrixData = GetMatrix(br, BOM);
            }

            public TextureUVTransform()
            {
                UVSetNumber = 0;
                Mapping_Type = 0;
                UnknownData8 = 0;
                CalculateTextureCoordinateTypeValue = 1;
                ScaleU = 0;
                ScaleV = 0;
                Rotate = 0;
                TranslateU = 0;
                TranslateV = 0;

                UnknownData9 = 0;
                MatrixData = new Matrix3x4();
            }
        }

        public int MaterialInfoSetOffset1 { get; set; }
        public MaterialInfoSet MaterialInfoSet1 { get; set; }

        public int MaterialInfoSetOffset2 { get; set; }
        public MaterialInfoSet MaterialInfoSet2 { get; set; }

        public int MaterialInfoSetOffset3 { get; set; }
        public MaterialInfoSet MaterialInfoSet3 { get; set; }

        public int MaterialInfoSetOffset4 { get; set; }
        public MaterialInfoSet MaterialInfoSet4 { get; set; }


        /// <summary>
        /// true => 4byte Padding, false => None
        /// </summary>
        public bool MaterialInfoDataPadding
        {
            get
            {
                bool b;
                if(MaterialInfoSetOffset1 != 0 && MaterialInfoSetOffset2 != 0 && MaterialInfoSetOffset3 != 0 && MaterialInfoSetOffset4 != 0)
                {
                    b = true;
                }
                else
                {
                    b = false;
                }
                return b;
            }
        }

        public class MaterialInfoSet
        {
            public Flags Flags { get; set; } //0x00000080
            public int UnknownData { get; set; }

            public int TXOB_MaterialInfoOffset { get; set; }
            public CGFXData TXOBMaterialDataSection { get; set; } //(Flag => 0x04000020)

            //public TXOBData TXOBDataSection { get; set; }
            //public class TXOBData
            //{
            //    //TXOB Section (Flag => 0x04000020)
            //    public Flags Flags { get; set; }
            //    public TXOB TXOB { get; set; }

            //    public void ReadTXOBMaterialInfo(BinaryReader br, byte[] BOM)
            //    {
            //        //EndianConvert endianConvert = new EndianConvert(BOM);
            //        Flags = new Flags(br.ReadBytes(4));
            //        if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x04, 0x00, 0x00, 0x20 }))
            //        {
            //            TXOB.MaterialInfoSection.ReadTXOB(br, BOM);
            //        }
            //    }

            //    public TXOBData()
            //    {
            //        Flags = new Flags(new byte[] { 0x04, 0x00, 0x00, 0x20 });
            //        TXOB = new TXOB(TXOB.Type.MaterialInfo);
            //    }
            //}

            public int UnknownDataOffset0 { get; set; }
            public UnknownDataSection1 UnknownDataSection_1 { get; set; }
            public class UnknownDataSection1
            {
                public Flags UnknownSectionFlag { get; set; }

                public byte[] UnknownData0 { get; set; } //0x4
                public byte[] UnknownData1 { get; set; } //0x4
                public byte[] UnknownData2 { get; set; } //0x14 (20byte)

                public void ReadUnknownSection1(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownSectionFlag = new Flags(br.ReadBytes(4));
                    if (UnknownSectionFlag.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }) == true)
                    {
                        UnknownData0 = br.ReadBytes(4);
                        UnknownData1 = br.ReadBytes(4);
                        UnknownData2 = br.ReadBytes(20);
                    }
                }

                public UnknownDataSection1()
                {
                    UnknownSectionFlag = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x80 });
                    UnknownData0 = new byte[4];
                    UnknownData1 = new byte[4];
                    UnknownData2 = new byte[20];
                }
            }

            //0x4 * 6, Float, Color(?)
            public UnknownDataSection2 UnknownDataSection_2 { get; set; }
            public class UnknownDataSection2
            {
                public float UnknownValue0 { get; set; }
                public float UnknownValue1 { get; set; }
                public float UnknownValue2 { get; set; }
                public float UnknownValue3 { get; set; }
                public float UnknownValue4 { get; set; }
                public float UnknownValue5 { get; set; }

                public void ReadUnknownSection2(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue5 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
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
            }

            public byte[] UnknownByteData0 { get; set; } //32byte

            public int UnknownDataOffset1 { get; set; }

            //0x4 * 3 : Float(?)
            public UnknownDataSection3 UnknownDataSection_3 { get; set; }
            public class UnknownDataSection3
            {
                public float UnknownValue0 { get; set; }
                public float UnknownValue1 { get; set; }
                public float UnknownValue2 { get; set; }

                public void ReadUnknownSection3(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownValue0 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownValue2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                }

                public UnknownDataSection3()
                {
                    UnknownValue0 = 0;
                    UnknownValue1 = 0;
                    UnknownValue2 = 0;
                }
            }

            //public Flags UnknownSectionFlag { get; set; }

            public void ReadMaterialInfoSet(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Flags = new Flags(br.ReadBytes(4));
                if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x80 }) == true)
                {
                    UnknownData = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    TXOB_MaterialInfoOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (TXOB_MaterialInfoOffset != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(TXOB_MaterialInfoOffset, SeekOrigin.Current);

                        //ReadMaterialInfo
                        TXOBMaterialDataSection = new CGFXData(null, true);
                        TXOBMaterialDataSection.Reader(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    UnknownDataOffset0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UnknownDataOffset0 != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(UnknownDataOffset0, SeekOrigin.Current);

                        //0x00 0x00 0x00 0x80
                        UnknownDataSection_1.ReadUnknownSection1(br, BOM);

                        br.BaseStream.Position = Pos;
                    }

                    UnknownDataSection_2.ReadUnknownSection2(br, BOM);

                    UnknownByteData0 = br.ReadBytes(32);

                    UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    if (UnknownDataOffset1 != 0)
                    {
                        long Pos = br.BaseStream.Position;

                        br.BaseStream.Seek(-4, SeekOrigin.Current);

                        //Move DataOffset
                        br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                        UnknownDataSection_3.ReadUnknownSection3(br, BOM);

                        br.BaseStream.Position = Pos;
                    }
                }
            }

            public MaterialInfoSet()
            {
                Flags = new Flags(new byte[] { 0x00, 0x00, 0x00, 0x80 });
                UnknownData = 0;
                TXOB_MaterialInfoOffset = 0;
                TXOBMaterialDataSection = new CGFXData(null, true);
                //TXOBMaterialDataSection.Flags = new Flags(new byte[] { 0x02, 0x00, 0x00, 0x40 });

                UnknownDataOffset0 = 0;
                UnknownDataSection_1 = new UnknownDataSection1();

                UnknownDataSection_2 = new UnknownDataSection2();
                UnknownByteData0 = new byte[32];
                UnknownDataOffset1 = 0;
                UnknownDataSection_3 = new UnknownDataSection3();
            }
        }

        public int SHDROffset { get; set; }
        public SHDRSection SHDRData { get; set; }
        public class SHDRSection
        {
            public Flags Flags { get; set; } //Flags : 0x01000080
            public SHDR SHDR { get; set; }

            public void ReadSHDRData(BinaryReader br, byte[] BOM)
            {
                Flags = new Flags(br.ReadBytes(4));
                if (Flags.IdentFlag.SequenceEqual(new byte[] { 0x01, 0x00, 0x00, 0x80 }))
                {
                    SHDR.ReadSHDR(br, BOM);
                }
            }

            public SHDRSection()
            {
                Flags = new Flags(new byte[] { 0x01, 0x00, 0x00, 0x80 });
                SHDR = new SHDR();
            }
        }

        public int SHDR_FragmentColorOffset { get; set; }

        /// <summary>
        /// SHDR Fragment Color (Write => SHDR)
        /// </summary>
        public FragmentColor Fragment_Color { get; set; }
        public class FragmentColor
        {
            public float UnknownData1 { get; set; }
            public float UnknownData2 { get; set; }
            public float UnknownData3 { get; set; }
            public float UnknownData4 { get; set; }

            public void ReadSHDRFragmentColor(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownData1 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData2 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData3 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownData4 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public FragmentColor(float d1, float d2, float d3, float d4)
            {
                UnknownData1 = d1;
                UnknownData2 = d2;
                UnknownData3 = d3;
                UnknownData4 = d4;
            }

            public FragmentColor()
            {
                UnknownData1 = 0;
                UnknownData2 = 0;
                UnknownData3 = 0;
                UnknownData4 = 0;
            }
        }


        public int UnknownData14 { get; set; }
        public int UnknownData15 { get; set; }

        public Matrix UnknownMatrix { get; set; } //Quaternion (?)

        public List<MaterialInfoSet> GetMaterialInfoSet()
        {
            List<MaterialInfoSet> materialInfoSets = new List<MaterialInfoSet>();
            materialInfoSets.Add(MaterialInfoSet1);
            materialInfoSets.Add(MaterialInfoSet2);
            materialInfoSets.Add(MaterialInfoSet3);
            materialInfoSets.Add(MaterialInfoSet4);
            return materialInfoSets;
        }

        public void ReadMTOB(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            MTOB_Header = br.ReadChars(4);
            if (new string(MTOB_Header) != "MTOB") throw new Exception("不明なフォーマットです");

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

            UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            LightSettings = new LightSetting(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
            UnknownData4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            DrawingLayer = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            MaterialColors.ReadMaterialColor(br, BOM);
            ConstantColorData.ReadConstantColor(br, BOM);

            UnknownData5 = endianConvert.Convert(br.ReadBytes(4));
            UnknownColorBit1Data.ReadUnknownColorBit1(br);
            UnknownColorBit2Data.ReadUnknownColorBit2(br);
            UnknownColorBit3Data.ReadUnknownColorBit3(br);
            UnknownData6 = endianConvert.Convert(br.ReadBytes(4));
            UnknownColorBit4Data.ReadUnknownColorBit4(br);

            #region 1
            UnknownData19 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData20 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData21 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData22 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData23 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData24 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            UnknownData25 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            UnknownData26 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData27 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownBits.ReadUnknownBit(br);
            UnknownData28 = endianConvert.Convert(br.ReadBytes(4));

            UnknownBit2s.ReadUnknownBit2(br);
            UnknownData29 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            #endregion

            BlendColorData.ReadBlendColor(br, BOM); //16
            UnknownData7 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData8 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData9 = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            UnknownData10 = br.ReadByte();
            UnknownData11 = br.ReadByte();
            UnknownData12 = endianConvert.Convert(br.ReadBytes(4));
            BlendColorBitData.ReadBlendColorBit(br);

            UnknownColorBitSet1.ReadUnknownColorBitSet(br);
            UnknownColorBitSet2.ReadUnknownColorBitSet(br);
            UnknownColorBitSet3.ReadUnknownColorBitSet(br);
            UnknownColorBitSet4.ReadUnknownColorBitSet(br);
            UnknownColorBitSet5.ReadUnknownColorBitSet(br);

            TextureUVTransformSlotCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //Slot数は増加するが、予め用意されているスロットデータは3つ分 (1 Slot : 0x88)
            for (int i = 0; i < 3; i++)
            {
                TextureUVTransform textureUVTransform = new TextureUVTransform();
                textureUVTransform.ReadTextureUVTransform(br, BOM);

                TextureUVTransformSettingList.Add(textureUVTransform);
            }

            //MaterialInfoSetOffset1 - 4 => 0x4 Byte Padding, MaterialInfoSetOffset 1-2-3-4 = 0 : No Padding
            MaterialInfoSetOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MaterialInfoSetOffset1 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MaterialInfoSetOffset1, SeekOrigin.Current);

                MaterialInfoSet materialInfoSet1 = new MaterialInfoSet();
                materialInfoSet1.ReadMaterialInfoSet(br, BOM);
                MaterialInfoSet1 = materialInfoSet1;

                br.BaseStream.Position = Pos;
            }

            MaterialInfoSetOffset2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MaterialInfoSetOffset2 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MaterialInfoSetOffset2, SeekOrigin.Current);

                MaterialInfoSet materialInfoSet2 = new MaterialInfoSet();
                materialInfoSet2.ReadMaterialInfoSet(br, BOM);
                MaterialInfoSet2 = materialInfoSet2;

                br.BaseStream.Position = Pos;
            }

            MaterialInfoSetOffset3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MaterialInfoSetOffset3 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MaterialInfoSetOffset3, SeekOrigin.Current);

                MaterialInfoSet materialInfoSet3 = new MaterialInfoSet();
                materialInfoSet3.ReadMaterialInfoSet(br, BOM);
                MaterialInfoSet3 = materialInfoSet3;

                br.BaseStream.Position = Pos;
            }

            MaterialInfoSetOffset4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MaterialInfoSetOffset4 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(MaterialInfoSetOffset4, SeekOrigin.Current);

                MaterialInfoSet materialInfoSet3 = new MaterialInfoSet();
                materialInfoSet3.ReadMaterialInfoSet(br, BOM);
                MaterialInfoSet3 = materialInfoSet3;

                br.BaseStream.Position = Pos;
            }

            //SHDR
            SHDROffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (SHDROffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(SHDROffset, SeekOrigin.Current);

                SHDRSection sHDRSection = new SHDRSection();
                sHDRSection.ReadSHDRData(br, BOM);
                SHDRData = sHDRSection;

                br.BaseStream.Position = Pos;
            }

            SHDR_FragmentColorOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //SHDR_Color Offset
            if (SHDR_FragmentColorOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(SHDROffset, SeekOrigin.Current);

                //SHDR_Color Offset
                Fragment_Color.ReadSHDRFragmentColor(br, BOM);

                br.BaseStream.Position = Pos;
            }

            UnknownData14 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData15 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            Matrix q = new Matrix();
            q.M11 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M12 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M13 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M14 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            q.M21 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M22 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M23 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M24 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            q.M31 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M32 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M33 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M34 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            q.M41 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M42 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M43 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            q.M44 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownMatrix = q;
        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            ReadMTOB(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        public MTOB()
        {
            MTOB_Header = "MTOB".ToArray();
            Revision = new List<byte>().ToArray();
            NameOffset = 0;
            UnknownData1 = 0;
            UnknownData2 = 0;
            LightSettings = new LightSetting(0);
            //IsFragmentLighting = 0;
            UnknownData4 = 0;
            DrawingLayer = 0;

            MaterialColors = new MaterialColor();
            ConstantColorData = new ConstantColor();

            UnknownData5 = new List<byte>().ToArray();
            UnknownColorBit1Data = new UnknownColorBit1(0x00, 0x00, 0x00, 0x00);
            UnknownColorBit2Data = new UnknownColorBit2(0x00, 0x00, 0x00, 0x00);
            UnknownColorBit3Data = new UnknownColorBit3(0x00, 0x00, 0x00, 0x00);
            UnknownData6 = new List<byte>().ToArray();

            UnknownColorBit4Data = new UnknownColorBit4();


            UnknownData19 = 0;
            UnknownData20 = 0;
            UnknownData21 = 0;
            UnknownData22 = 0;
            UnknownData23 = 0;
            UnknownData24 = 0;
            UnknownData25 = 0;
            UnknownData26 = 0;
            UnknownData27 = 0;

            UnknownBits = new UnknownBit(0, 0, 0, 0);
            UnknownData28 = new byte[4];

            UnknownBit2s = new UnknownBit2(0, 0, 0, 0);
            UnknownData29 = 0;

            BlendColorData = new BlendColor(0, 0, 0, 0);
            UnknownData7 = new List<byte>().ToArray();
            UnknownData8 = new List<byte>().ToArray();
            UnknownData9 = 0;
            UnknownData10 = 0x00;
            UnknownData11 = 0x00;
            UnknownData7 = new List<byte>().ToArray();
            BlendColorBitData = new BlendColorBit(0x00, 0x00, 0x00, 0x00);

            UnknownColorBitSet1 = new UnknownColorBitSet(0x00, 0x00, 0x00, 0x00);
            UnknownColorBitSet2 = new UnknownColorBitSet(0x00, 0x00, 0x00, 0x00);
            UnknownColorBitSet3 = new UnknownColorBitSet(0x00, 0x00, 0x00, 0x00);
            UnknownColorBitSet4 = new UnknownColorBitSet(0x00, 0x00, 0x00, 0x00);
            UnknownColorBitSet5 = new UnknownColorBitSet(0x00, 0x00, 0x00, 0x00);

            TextureUVTransformSlotCount = 0;
            TextureUVTransformSettingList = new List<TextureUVTransform>();

            MaterialInfoSetOffset1 = 0;
            MaterialInfoSet1 = new MaterialInfoSet();

            MaterialInfoSetOffset2 = 0;
            MaterialInfoSet2 = new MaterialInfoSet();

            MaterialInfoSetOffset3 = 0;
            MaterialInfoSet3 = new MaterialInfoSet();

            MaterialInfoSetOffset4 = 0;
            MaterialInfoSet4 = new MaterialInfoSet();

            SHDROffset = 0;
            SHDRData = new SHDRSection();

            SHDR_FragmentColorOffset = 0;
            Fragment_Color = new FragmentColor();

            UnknownData14 = 0;
            UnknownData15 = 0;

            UnknownMatrix = Matrix.Identity;
        }
    }
}
