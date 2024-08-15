using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Shader (Flag : 0x01000080)
    /// </summary>
    public class SHDR : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;

        public char[] SHDR_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }
        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }
        public string VertexShaderName;
        public int VertexShaderNameOffset { get; set; }
        public int UnknownData3 { get; set; }

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

        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }

        //Note : フレネルの設定 (プライマリおよびセカンダリ) は参照テーブルの指定が無い場合、たとえTrueであってもFalseになる (?)
        public LookupTableElementSettings LookupTableElementSetting { get; set; }
        public class LookupTableElementSettings
        {
            public byte Bit;

            public bool IsGeometricFactor0 //0x8
            {
                get
                {
                    return Convert.ToBoolean((Bit & 0x0F) & 0x08);
                }
            }

            public bool IsGeometricFactor1 //0x10
            {
                get
                {
                    return Convert.ToBoolean((Bit & 0xF0) & 0x10);
                }
            }

            public bool IsClampSpc //0x01
            {
                get
                {
                    return Convert.ToBoolean((Bit & 0x0F) & 0x08);
                }
            }

            public LookupTableElementSettings(byte Input)
            {
                Bit = Input;
            }
        }

        public int LayerConfigNum { get; set; }
        public LayerType LayerTypes => (LayerType)LayerConfigNum;

        public enum LayerType
        {
            Config0 = 0, //スポット, 分布0, 反射R
            Config1 = 1, //スポット, 反射R, フレネル
            Config2 = 2, //分布0, 分布1, 反射R
            Config3 = 3,
            Config4 = 4,
            Config5 = 5,
            Config6 = 6,
            Config7 = 7
        }

        //Texture Environment Info
        //

        public int UnknownData17 { get; set; }
        public int UnknownData18 { get; set; }
        public int UnknownData19 { get; set; }
        public int UnknownData20 { get; set; }

        public int UnknownDataAreaOffset { get; set; }
        public UnknownDataArea Unknown_DataArea { get; set; }
        public class UnknownDataArea
        {
            public byte[] UnknownData14 { get; set; } //0x4
            public byte[] UnknownData15 { get; set; } //0x4
            public byte[] UnknownData16 { get; set; } //0x4

            public int UnknownDataOffset1 { get; set; } //0x4
            public UnknownDataSet UnknownDataSet1 { get; set; }

            public int UnknownDataOffset2 { get; set; } //0x4
            public UnknownDataSet UnknownDataSet2 { get; set; }

            public int UnknownDataOffset3 { get; set; } //0x4
            public UnknownDataSet UnknownDataSet3 { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public class UnknownDataSet
            {
                public int UnknownData1 { get; set; } //0x4
                public int UnknownData2 { get; set; } //0x4
                public int UnknownData3 { get; set; } //0x4

                public UnknownNameSet Unknown_NameSet { get; set; }
                public class UnknownNameSet
                {
                    public string Name;
                    public string SubName;

                    public Flags IdentFlag { get; set; }
                    public int UnknownDataOffset0 { get; set; }
                    public int UnknownDataOffset1 { get; set; }

                    public byte[] UnknownDataBytes0 { get; set; }

                    public void ReadUnknownNameSet(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        IdentFlag = new Flags(endianConvert.Convert(br.ReadBytes(4)));

                        UnknownDataOffset0 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (UnknownDataOffset0 != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move NameOffset
                            br.BaseStream.Seek(UnknownDataOffset0, SeekOrigin.Current);

                            ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                            readByteLine.ReadByte(br, 0x00);

                            Name = new string(readByteLine.ConvertToCharArray());

                            br.BaseStream.Position = Pos;
                        }

                        UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        if (UnknownDataOffset1 != 0)
                        {
                            long Pos = br.BaseStream.Position;

                            br.BaseStream.Seek(-4, SeekOrigin.Current);

                            //Move NameOffset
                            br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                            ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                            readByteLine.ReadByte(br, 0x00);

                            SubName = new string(readByteLine.ConvertToCharArray());

                            br.BaseStream.Position = Pos;
                        }

                        UnknownDataBytes0 = endianConvert.Convert(br.ReadBytes(4));
                    }

                    //public UnknownNameSet(string Name, string SubName)
                    //{

                    //}

                    public UnknownNameSet()
                    {
                        Name = "";
                        SubName = "";

                        IdentFlag = new Flags(new byte[4]);
                        UnknownDataOffset0 = 0;
                        UnknownDataOffset1 = 0;

                        UnknownDataBytes0 = new byte[4];
                    }

                    public override string ToString()
                    {
                        return "UnknownNameSet";
                    }
                }

                public void ReadUnknownDataSet(BinaryReader br, byte[] BOM)
                {
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    Unknown_NameSet.ReadUnknownNameSet(br, BOM);
                }

                public UnknownDataSet()
                {
                    UnknownData1 = 0;
                    UnknownData2 = 0;
                    UnknownData3 = 0;

                    Unknown_NameSet = new UnknownNameSet();
                }

                public override string ToString()
                {
                    return "UnknownDataSet";
                }
            }

            public void ReadUnknownDataArea(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                UnknownData14 = endianConvert.Convert(br.ReadBytes(4));
                UnknownData15 = endianConvert.Convert(br.ReadBytes(4));
                UnknownData16 = endianConvert.Convert(br.ReadBytes(4));

                UnknownDataOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset1 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(UnknownDataOffset1, SeekOrigin.Current);

                    UnknownDataSet1.ReadUnknownDataSet(br, BOM);

                    br.BaseStream.Position = Pos;
                }

                UnknownDataOffset2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset2 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(UnknownDataOffset2, SeekOrigin.Current);

                    UnknownDataSet2.ReadUnknownDataSet(br, BOM);

                    br.BaseStream.Position = Pos;
                }

                UnknownDataOffset3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (UnknownDataOffset3 != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move Offset
                    br.BaseStream.Seek(UnknownDataOffset3, SeekOrigin.Current);

                    UnknownDataSet3.ReadUnknownDataSet(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public UnknownDataArea()
            {
                UnknownData14 = new byte[4];
                UnknownData15 = new byte[4];
                UnknownData16 = new byte[4];

                UnknownDataOffset1 = 0;
                UnknownDataSet1 = new UnknownDataSet();

                UnknownDataOffset2 = 0;
                UnknownDataSet2 = new UnknownDataSet();

                UnknownDataOffset3 = 0;
                UnknownDataSet3 = new UnknownDataSet();
            }
        }

        public int UnknownData22 { get; set; }

        public List<TextureCombinerStage> TextureCombinerStages { get; set; }
        public class TextureCombinerStage
        {
            public enum Stage
            {
                Stage0 = 192,
                Stage1 = 200,
                Stage2 = 208,
                Stage3 = 216,
                Stage4 = 240,
                Stage5 = 248
            }

            public enum ScaleFactor
            {
                x1 = 0,
                x2 = 1,
                x4 = 2
            }

            public enum CombinerEquation
            {
                /// <summary>SOURCE = A</summary>
                A = 0,

                /// <summary>SOURCE = A * B</summary>
                A_MULTIPLY_B = 1,

                /// <summary>SOURCE = A + B</summary>
                A_ADD_B = 2,

                /// <summary>SOURCE = A + B - 0.5</summary>
                A_ADD_B_SUB_NOUGHT_PT_FIVE = 3,

                /// <summary>SOURCE =  A * C + B * (1 - C)</summary>
                A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT = 4,

                /// <summary>SOURCE =  A - B</summary>
                A_SUB_B = 5,

                /// <summary>SOURCE =  RGB <= DOT(A, B)</summary>
                RGB_DOT_BRKT_A_B_BRKT = 6,

                /// <summary>SOURCE =  RGBB <= DOT(A, B)</summary>
                RGBA_DOT_BRKT_A_B_BRKT = 7,

                /// <summary>SOURCE =  (A + B) * C</summary>
                BRKT_A_ADD_B_BRKT_MULTIPLY_C = 8,

                /// <summary>SOURCE =  (A * B) + C</summary>
                BRKT_A_MULTIPLY_B_BRKT_ADD_C = 9
            }

            public Source SourceValue { get; set; }
            public class Source
            {
                public int SourceValueNum;

                public enum CombinerSource
                {
                    /// <summary>頂点シェーダーの出力結果</summary>
                    VERTEX_SHADER_OUT = 0,

                    /// <summary>プライマリカラー</summary>
                    COLOR_PRIMARY = 1,

                    /// <summary>セカンダリカラー</summary>
                    COLOR_SECONDARY = 2,

                    /// <summary>Texture 0</summary>
                    TEXTURE0 = 3,

                    /// <summary>Texture 1</summary>
                    TEXTURE1 = 4,

                    /// <summary>Texture 2</summary>
                    TEXTURE2 = 5,

                    /// <summary>Texture 3</summary>
                    TEXTURE3 = 6,

                    /// <summary>前段のバッファ (1段目には表示しない)</summary>
                    PREVIOUS_STEP_BUFFER = 13,

                    /// <summary>Constant Color</summary>
                    CONSTANT_COLOR = 14,

                    /// <summary>前段の出力結果 (1段目には表示しない)</summary>
                    PREVIOUS_STEP_RESULT = 15,

                    //Unknown
                }

                public CombinerSource Source_Color_A_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Color_A);
                public byte Source_Color_A
                {
                    get
                    {
                        return Convert.ToByte(SourceValueNum & 0xF);
                    }
                }

                public CombinerSource Source_Color_B_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Color_B);
                public byte Source_Color_B
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF0) >> 4);
                    }
                }

                public CombinerSource Source_Color_C_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Color_C);
                public byte Source_Color_C
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF00) >> 8);
                    }
                }

                public byte UnknownFlag0
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF000) >> 12);
                    }
                }

                public CombinerSource Source_Alpha_A_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Alpha_A);
                public byte Source_Alpha_A
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF0000) >> 16); //16
                    }
                }

                public CombinerSource Source_Alpha_B_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Alpha_B);
                public byte Source_Alpha_B
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF00000) >> 20);
                    }
                }

                public CombinerSource Source_Alpha_C_Flag => (CombinerSource)Enum.ToObject(typeof(CombinerSource), Source_Alpha_C);
                public byte Source_Alpha_C
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF000000) >> 24);
                    }
                }

                public byte UnknownFlag1
                {
                    get
                    {
                        return Convert.ToByte((SourceValueNum & 0xF000000) >> 28);
                    }
                }

                public Source(int value)
                {
                    SourceValueNum = value;
                }
            }

            public byte StageFlag { get; set; } //0x1
            public Stage StageType => (Stage)Enum.ToObject(typeof(Stage), StageFlag);
            public byte[] UnknownData0 { get; set; } //0x3

            public Operand OperandValue { get; set; }
            public class Operand
            {
                public int OperandValueNum;

                public enum CombinerColorOperand
                {
                    /// <summary>RGB</summary>
                    RGB = 0,

                    /// <summary>1 - RGB</summary>
                    ONE_SUB_RGB = 1,

                    /// <summary>A</summary>
                    A = 2,

                    /// <summary>1 - A</summary>
                    ONE_SUB_A = 3,

                    /// <summary>R</summary>
                    R = 4,

                    /// <summary>1 - R</summary>
                    ONE_SUB_R = 5,

                    /// <summary>G</summary>
                    G = 6,

                    /// <summary>1 - B</summary>
                    ONE_SUB_G = 7,

                    /// <summary>B</summary>
                    B = 8,

                    /// <summary>1 - G</summary>
                    ONE_SUB_B = 9
                }

                public enum CombinerAlphaOperand
                {
                    /// <summary>A</summary>
                    A = 0,

                    /// <summary>1 - A</summary>
                    ONE_SUB_A = 1,

                    /// <summary>R</summary>
                    R = 2,

                    /// <summary>1 - R</summary>
                    ONE_SUB_R = 3,

                    /// <summary>G</summary>
                    G = 4,

                    /// <summary>1 - B</summary>
                    ONE_SUB_G = 5,

                    /// <summary>B</summary>
                    B = 6,

                    /// <summary>1 - G</summary>
                    ONE_SUB_B = 7
                }

                public CombinerColorOperand Operand_Color_A_Flag => (CombinerColorOperand)Enum.ToObject(typeof(CombinerColorOperand), Operand_Color_A);
                public byte Operand_Color_A
                {
                    get
                    {
                        return Convert.ToByte(OperandValueNum & 0xF);
                    }
                }

                public CombinerColorOperand Operand_Color_B_Flag => (CombinerColorOperand)Enum.ToObject(typeof(CombinerColorOperand), Operand_Color_B);
                public byte Operand_Color_B
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF0) >> 4);
                    }
                }

                public CombinerColorOperand Operand_Color_C_Flag => (CombinerColorOperand)Enum.ToObject(typeof(CombinerColorOperand), Operand_Color_C);
                public byte Operand_Color_C
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF00) >> 8);
                    }
                }

                public CombinerAlphaOperand Operand_Alpha_A_Flag => (CombinerAlphaOperand)Enum.ToObject(typeof(CombinerAlphaOperand), Operand_Alpha_A);
                public byte Operand_Alpha_A
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF000) >> 12);
                    }
                }

                public CombinerAlphaOperand Operand_Alpha_B_Flag => (CombinerAlphaOperand)Enum.ToObject(typeof(CombinerAlphaOperand), Operand_Alpha_B);
                public byte Operand_Alpha_B
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF0000) >> 16);
                    }
                }

                public CombinerAlphaOperand Operand_Alpha_C_Flag => (CombinerAlphaOperand)Enum.ToObject(typeof(CombinerAlphaOperand), Operand_Alpha_C);
                public byte Operand_Alpha_C
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF00000) >> 20);
                    }
                }

                public byte UnknownFlag0
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF000000) >> 24);
                    }
                }

                public byte UnknownFlag1
                {
                    get
                    {
                        return Convert.ToByte((OperandValueNum & 0xF000000) >> 28);
                    }
                }

                public Operand(int value)
                {
                    OperandValueNum = value;
                }
            }

            public byte ColorCombinerEquationType { get; set; }
            public CombinerEquation ColorCombinerEquationFlag => (CombinerEquation)Enum.ToObject(typeof(CombinerEquation), ColorCombinerEquationType);

            public byte UnknownData2 { get; set; } //0x1

            public byte AlphaCombinerEquationType { get; set; }
            public CombinerEquation AlphaCombinerEquationFlag => (CombinerEquation)Enum.ToObject(typeof(CombinerEquation), AlphaCombinerEquationType);
            public byte[] UnknownData3 { get; set; } //0x5

            public byte ColorCombinerScaleFactorType { get; set; }
            public ScaleFactor ColorCombinerScaleFactorFlag => (ScaleFactor)Enum.ToObject(typeof(ScaleFactor), ColorCombinerScaleFactorType);

            public byte UnknownByte0 { get; set; }
            public byte AlphaCombinerScaleFactorType { get; set; }
            public ScaleFactor AlphaCombinerScaleFactorFlag => (ScaleFactor)Enum.ToObject(typeof(ScaleFactor), AlphaCombinerScaleFactorType);

            public byte[] UnknownData4 { get; set; } //0x5

            public void ReadTextureCombinerStage(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                SourceValue = new Source(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));

                StageFlag = br.ReadByte();
                UnknownData0 = br.ReadBytes(3);

                OperandValue = new Operand(BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0));

                ColorCombinerEquationType = br.ReadByte();
                UnknownData2 = br.ReadByte();

                AlphaCombinerEquationType = br.ReadByte();
                UnknownData3 = br.ReadBytes(5);
                ColorCombinerScaleFactorType = br.ReadByte();

                UnknownByte0 = br.ReadByte();
                AlphaCombinerScaleFactorType = br.ReadByte();

                UnknownData4 = br.ReadBytes(5);


                //StageData = br.ReadBytes(28);
            }

            public TextureCombinerStage()
            {
                SourceValue = new Source(0);

                StageFlag = 0x00;
                UnknownData0 = new byte[3];

                //OperandValueNum = 0;
                OperandValue = new Operand(0);
                ColorCombinerEquationType = 0x00;
                UnknownData2 = 0x00;
                AlphaCombinerEquationType = 0x00;
                UnknownData3 = new byte[5];
                ColorCombinerScaleFactorType = 0x00;

                UnknownByte0 = 0x00;
                AlphaCombinerScaleFactorType = 0x00;
                UnknownData4 = new byte[5];
            }


        }

        public byte[] UnknownData8 { get; set; } //0x4
        public byte[] UnknownData9 { get; set; } //0x4
        public byte[] UnknownData10 { get; set; } //0x4

        public byte UnknownByte0 { get; set; } //0x1

        public TextureCombinerIsBufferFlags TextureCombinerIsBufferFlag { get; set; }
        public class TextureCombinerIsBufferFlags
        {
            public byte Flags;

            public bool IsCombinerColor0_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x8) != 0 ? true : false;
                }
            }

            public bool IsCombinerColor1_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x4) != 0 ? true : false;
                }
            }

            public bool IsCombinerColor2_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x2) != 0 ? true : false;
                }
            }

            public bool IsCombinerColor3_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x1) != 0 ? true : false;
                }
            }

            public bool IsCombinerAlpha0_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x80) != 0 ? true : false;
                }
            }

            public bool IsCombinerAlpha1_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x40) != 0 ? true : false;
                }
            }

            public bool IsCombinerAlpha2_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x20) != 0 ? true : false;
                }
            }

            public bool IsCombinerAlpha3_Buffering
            {
                get
                {
                    return Convert.ToByte(Flags & 0x10) != 0 ? true : false;
                }
            }

            public TextureCombinerIsBufferFlags(byte Flags)
            {
                this.Flags = Flags;
            }
        }

        public byte UnknownByte1 { get; set; } //0x1
        public byte UnknownByte2 { get; set; } //0x1

        public byte[] UnknownData11 { get; set; } //0x4
        public byte[] UnknownData12 { get; set; } //0x4
        public byte[] UnknownData13 { get; set; } //0x4

        public void ReadSHDR(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            SHDR_Header = br.ReadChars(4);
            if (new string(SHDR_Header) != "SHDR") throw new Exception("不明なフォーマットです");

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

            VertexShaderNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (VertexShaderNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(VertexShaderNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                VertexShaderName = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            UnknownData3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            //Fragment
            Fragment_Color.ReadSHDRFragmentColor(br, BOM);

            Unknown1 = br.ReadByte();
            Unknown2 = br.ReadByte();
            Unknown3 = br.ReadByte();
            LookupTableElementSetting = new LookupTableElementSettings(br.ReadByte());
            LayerConfigNum = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownData17 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData18 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData19 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UnknownData20 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            UnknownDataAreaOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UnknownDataAreaOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move Offset
                br.BaseStream.Seek(UnknownDataAreaOffset, SeekOrigin.Current);

                Unknown_DataArea.ReadUnknownDataArea(br, BOM);

                br.BaseStream.Position = Pos;
            }

            UnknownData22 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

            for (int StageCount = 0; StageCount < 6; StageCount++)
            {
                TextureCombinerStage textureCombinerStage = new TextureCombinerStage();
                textureCombinerStage.ReadTextureCombinerStage(br, BOM);
                TextureCombinerStages.Add(textureCombinerStage);
            }

            UnknownData8 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData9 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData10 = endianConvert.Convert(br.ReadBytes(4));

            UnknownByte0 = br.ReadByte();
            TextureCombinerIsBufferFlag = new TextureCombinerIsBufferFlags(br.ReadByte()); //0 or IsC0Buffering = false : 0x00
            UnknownByte1 = br.ReadByte();
            UnknownByte2 = br.ReadByte();

            UnknownData11 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData12 = endianConvert.Convert(br.ReadBytes(4));
            UnknownData13 = endianConvert.Convert(br.ReadBytes(4));
        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            ReadSHDR(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        public SHDR()
        {
            SHDR_Header = "SHDR".ToArray();
            Revision = new List<byte>().ToArray();
            NameOffset = 0;

            UnknownData1 = 0;
            UnknownData2 = 0;

            VertexShaderNameOffset = 0;

            UnknownData3 = 0;

            Fragment_Color = new FragmentColor();

            //LookupTableElementSetting = 0;

            Unknown1 = 0x00;
            Unknown2 = 0x00;
            Unknown3 = 0x00;
            LookupTableElementSetting = new LookupTableElementSettings(0x00);
            LayerConfigNum = 0;


            UnknownData17 = 0;
            UnknownData18 = 0;
            UnknownData19 = 0;
            UnknownData20 = 0;
            UnknownDataAreaOffset = 0;
            Unknown_DataArea = new UnknownDataArea();
            UnknownData22 = 0;


            TextureCombinerStages = new List<TextureCombinerStage>();

            UnknownData8 = new byte[4];
            UnknownData9 = new byte[4];
            UnknownData10 = new byte[4];

            UnknownByte0 = 0x00;
            TextureCombinerIsBufferFlag = new TextureCombinerIsBufferFlags(0x00);
            UnknownByte1 = 0x00;
            UnknownByte2 = 0x00;

            UnknownData11 = new byte[4];
            UnknownData12 = new byte[4];
            UnknownData13 = new byte[4];
        }

        public override string ToString()
        {
            return "SHDR : " + Name.Replace("\0", "") + " | " + VertexShaderName.Replace("\0", "");
        }
    }
}
