using CGFX_Viewer_SharpDX.CGFXPropertyGridSet;
using CGFXLibrary;
using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.MTOB.SHDRData
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class SHDRViewer_PropertyGrid
    {
        public string Name;
        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }
        public string VertexShaderName { get; set; }
        public int UnknownData3 { get; set; }

        //Fragment Shader
        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public FragmentColor Fragment_Color { get; set; } = new FragmentColor();
        public class FragmentColor
        {
            public float UnknownData1 { get; set; }
            public float UnknownData2 { get; set; }
            public float UnknownData3 { get; set; }
            public float UnknownData4 { get; set; }

            public FragmentColor(SHDR.FragmentColor fragmentColor)
            {
                UnknownData1 = fragmentColor.UnknownData1;
                UnknownData2 = fragmentColor.UnknownData2;
                UnknownData3 = fragmentColor.UnknownData3;
                UnknownData4 = fragmentColor.UnknownData4;
            }

            public FragmentColor()
            {
                UnknownData1 = 0;
                UnknownData2 = 0;
                UnknownData3 = 0;
                UnknownData4 = 0;
            }

            public override string ToString()
            {
                return "Fragment Color : " + UnknownData1 + " | " + UnknownData2 + " | " + UnknownData3 + " | " + UnknownData4;
            }
        }


        //public int LookupTableElementSetting { get; set; }

        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Unknown3 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public LookUpTableElementSetting LookupTableElementSetting { get; set; } = new LookUpTableElementSetting();
        public class LookUpTableElementSetting
        {
            public bool IsGeometricFactor0 { get; set; }
            public bool IsGeometricFactor1 { get; set; }
            public bool IsClampSpc { get; set; }

            public LookUpTableElementSetting(SHDR.LookupTableElementSettings lookupTableElementSettings)
            {
                IsGeometricFactor0 = lookupTableElementSettings.IsGeometricFactor0;
                IsGeometricFactor1 = lookupTableElementSettings.IsGeometricFactor1;
                IsClampSpc = lookupTableElementSettings.IsClampSpc;
            }

            public LookUpTableElementSetting()
            {
                IsGeometricFactor0 = false;
                IsGeometricFactor1 = false;
                IsClampSpc = false;
            }

            public override string ToString()
            {
                return "LookUpTableElementSetting";
            }
        }

        public SHDR.LayerType LayerType { get; set; }

        public int UnknownData17 { get; set; }
        public int UnknownData18 { get; set; }
        public int UnknownData19 { get; set; }
        public int UnknownData20 { get; set; }
        //public int UnknownDataAreaOffset { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public UnknownDataArea Unknown_DataArea { get; set; } = new UnknownDataArea();
        public class UnknownDataArea
        {
            public byte[] UnknownData14 { get; set; } //0x4
            public byte[] UnknownData15 { get; set; } //0x4
            public byte[] UnknownData16 { get; set; } //0x4

            //public int UnknownDataOffset1 { get; set; } //0x4
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSet UnknownDataSet1 { get; set; } = new UnknownDataSet();

            //public int UnknownDataOffset2 { get; set; } //0x4
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSet UnknownDataSet2 { get; set; } = new UnknownDataSet();

            //public int UnknownDataOffset3 { get; set; } //0x4
            [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
            public UnknownDataSet UnknownDataSet3 { get; set; } = new UnknownDataSet();

            /// <summary>
            /// 
            /// </summary>
            public class UnknownDataSet
            {
                public int UnknownData1 { get; set; } //0x4
                public int UnknownData2 { get; set; } //0x4
                public int UnknownData3 { get; set; } //0x4

                [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
                public UnknownNameSet Unknown_NameSet { get; set; } = new UnknownNameSet();
                public class UnknownNameSet
                {
                    public string Name;
                    public string SubName;

                    public Flags IdentFlag { get; set; }
                    //public int UnknownDataOffset0 { get; set; }
                    //public int UnknownDataOffset1 { get; set; }

                    public byte[] UnknownDataBytes0 { get; set; }

                    public UnknownNameSet(SHDR.UnknownDataArea.UnknownDataSet.UnknownNameSet unknownNameSet)
                    {
                        Name = unknownNameSet.Name;
                        SubName = unknownNameSet.SubName;
                        IdentFlag = unknownNameSet.IdentFlag;

                        //UnknownDataOffset0 = unknownNameSet.UnknownDataOffset0;
                        //UnknownDataOffset1 = unknownNameSet.UnknownDataOffset1;

                        UnknownDataBytes0 = unknownNameSet.UnknownDataBytes0;
                    }

                    public UnknownNameSet()
                    {
                        Name = "";
                        SubName = "";

                        IdentFlag = new Flags(new byte[4]);
                        //UnknownDataOffset0 = 0;
                        //UnknownDataOffset1 = 0;

                        UnknownDataBytes0 = new byte[4];
                    }

                    public override string ToString()
                    {
                        return "UnknownNameSet : " + Name +" | " + SubName;
                    }
                }

                public UnknownDataSet(SHDR.UnknownDataArea.UnknownDataSet unknownDataSet)
                {
                    UnknownData1 = unknownDataSet.UnknownData1;
                    UnknownData2 = unknownDataSet.UnknownData2;
                    UnknownData3 = unknownDataSet.UnknownData3;

                    Unknown_NameSet = new UnknownNameSet(unknownDataSet.Unknown_NameSet);
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
                    return "UnknownDataSet : " + "[" + UnknownData1 + " | " + UnknownData2 + " | " + UnknownData3 + "]";
                }
            }

            public UnknownDataArea(SHDR.UnknownDataArea unknownDataArea)
            {
                UnknownData14 = unknownDataArea.UnknownData14;
                UnknownData15 = unknownDataArea.UnknownData15;
                UnknownData16 = unknownDataArea.UnknownData16;

                UnknownDataSet1 = new UnknownDataSet(unknownDataArea.UnknownDataSet1);
                UnknownDataSet2 = new UnknownDataSet(unknownDataArea.UnknownDataSet2);
                UnknownDataSet3 = new UnknownDataSet(unknownDataArea.UnknownDataSet3);
            }

            public UnknownDataArea()
            {
                UnknownData14 = new byte[4];
                UnknownData15 = new byte[4];
                UnknownData16 = new byte[4];

                //UnknownDataOffset1 = 0;
                UnknownDataSet1 = new UnknownDataSet();

                //UnknownDataOffset2 = 0;
                UnknownDataSet2 = new UnknownDataSet();

                //UnknownDataOffset3 = 0;
                UnknownDataSet3 = new UnknownDataSet();
            }

            public override string ToString()
            {
                return "UnknownDataArea";
            }
        }

        public int UnknownData22 { get; set; }

        public byte[] UnknownData8 { get; set; } //4
        public byte[] UnknownData9 { get; set; } //4
        public byte[] UnknownData10 { get; set; } //4

        public byte UnknownByte0 { get; set; }

        [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomExpandableObjectSortTypeConverter))]
        public TextureCombinerIsBufferFlags TextureCombinerIsBufferFlag { get; set; } = new TextureCombinerIsBufferFlags(0x00);
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

            public override string ToString()
            {
                return "IsBufferFlags";
            }
        }
        public byte UnknownByte1 { get; set; }
        public byte UnknownByte2 { get; set; }

        public byte[] UnknownData11 { get; set; } //0x4
        public byte[] UnknownData12 { get; set; } //0x4
        public byte[] UnknownData13 { get; set; } //0x4

        public SHDRViewer_PropertyGrid(SHDR SHDR)
        {
            Name = SHDR.Name;
            UnknownData1 = SHDR.UnknownData1;
            UnknownData2 = SHDR.UnknownData2;
            VertexShaderName = SHDR.VertexShaderName;
            UnknownData3 = SHDR.UnknownData3;
            Fragment_Color = new FragmentColor(SHDR.Fragment_Color);

            Unknown1 = SHDR.Unknown1;
            Unknown2 = SHDR.Unknown2;
            Unknown3 = SHDR.Unknown3;

            LookupTableElementSetting = new LookUpTableElementSetting(SHDR.LookupTableElementSetting);
            LayerType = SHDR.LayerTypes;

            UnknownData17 = SHDR.UnknownData17;
            UnknownData18 = SHDR.UnknownData18;
            UnknownData19 = SHDR.UnknownData19;
            UnknownData20 = SHDR.UnknownData20;
            Unknown_DataArea = new UnknownDataArea(SHDR.Unknown_DataArea);
            UnknownData22 = SHDR.UnknownData22;

            UnknownData8 = SHDR.UnknownData8;
            UnknownData9 = SHDR.UnknownData9;
            UnknownData10 = SHDR.UnknownData10;

            UnknownByte0 = SHDR.UnknownByte0;
            TextureCombinerIsBufferFlag = new TextureCombinerIsBufferFlags(SHDR.TextureCombinerIsBufferFlag.Flags);
            UnknownByte1 = SHDR.UnknownByte1;
            UnknownByte2 = SHDR.UnknownByte2;

            UnknownData11 = SHDR.UnknownData11;
            UnknownData12 = SHDR.UnknownData12;
            UnknownData13 = SHDR.UnknownData13;
        }

        public SHDRViewer_PropertyGrid()
        {
            Name = "";
            UnknownData1 = 0;
            UnknownData2 = 0;
            VertexShaderName = "";
            UnknownData3 = 0;
            Fragment_Color = new FragmentColor();

            Unknown1 = 0x00;
            Unknown2 = 0x00;
            Unknown3 = 0x00;

            LookupTableElementSetting = new LookUpTableElementSetting();
            LayerType = SHDR.LayerType.Config0;

            UnknownData17 = 0;
            UnknownData18 = 0;
            UnknownData19 = 0;
            UnknownData20 = 0;
            Unknown_DataArea = new UnknownDataArea();
            UnknownData22 = 0;

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
    }

    public class CombinerStagePGSObject
    {
        public SHDR.TextureCombinerStage.Stage Stage { get; set; }
        public SHDR.TextureCombinerStage.CombinerEquation CombinerColorEquation { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerColorSource_A { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerColorSource_B { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerColorSource_C { get; set; }
        public byte UnknownSRCFlag0 { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerColorOperand CombinerColorOperand_A { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerColorOperand CombinerColorOperand_B { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerColorOperand CombinerColorOperand_C { get; set; }
        public byte UnknownOperandFlag0 { get; set; }

        public SHDR.TextureCombinerStage.CombinerEquation CombinerAlphaEquation { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerAlphaSource_A { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerAlphaSource_B { get; set; }
        public SHDR.TextureCombinerStage.Source.CombinerSource CombinerAlphaSource_C { get; set; }
        public byte UnknownSRCFlag1 { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerAlphaOperand CombinerAlphaOperand_A { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerAlphaOperand CombinerAlphaOperand_B { get; set; }
        public SHDR.TextureCombinerStage.Operand.CombinerAlphaOperand CombinerAlphaOperand_C { get; set; }
        public byte UnknownOperandFlag1 { get; set; }

        public byte UnknownByte0 { get; set; }
        public byte[] UnknownData0 { get; set; }
        public byte UnknownData2 { get; set; }
        public byte[] UnknownData3 { get; set; }
        public byte[] UnknownData4 { get; set; }

        public CombinerStagePGSObject(SHDR.TextureCombinerStage textureCombinerStage)
        {
            Stage = textureCombinerStage.StageType;
            CombinerColorEquation = textureCombinerStage.ColorCombinerEquationFlag;
            CombinerColorSource_A = textureCombinerStage.SourceValue.Source_Color_A_Flag;
            CombinerColorSource_B = textureCombinerStage.SourceValue.Source_Color_B_Flag;
            CombinerColorSource_C = textureCombinerStage.SourceValue.Source_Color_C_Flag;
            UnknownSRCFlag0 = textureCombinerStage.SourceValue.UnknownFlag0;
            CombinerColorOperand_A = textureCombinerStage.OperandValue.Operand_Color_A_Flag;
            CombinerColorOperand_B = textureCombinerStage.OperandValue.Operand_Color_B_Flag;
            CombinerColorOperand_C = textureCombinerStage.OperandValue.Operand_Color_C_Flag;
            UnknownOperandFlag0 = textureCombinerStage.OperandValue.UnknownFlag0;

            CombinerAlphaEquation = textureCombinerStage.AlphaCombinerEquationFlag;
            CombinerAlphaSource_A = textureCombinerStage.SourceValue.Source_Alpha_A_Flag;
            CombinerAlphaSource_B = textureCombinerStage.SourceValue.Source_Alpha_B_Flag;
            CombinerAlphaSource_C = textureCombinerStage.SourceValue.Source_Alpha_C_Flag;
            UnknownSRCFlag1 = textureCombinerStage.SourceValue.UnknownFlag1;
            CombinerAlphaOperand_A = textureCombinerStage.OperandValue.Operand_Alpha_A_Flag;
            CombinerAlphaOperand_B = textureCombinerStage.OperandValue.Operand_Alpha_B_Flag;
            CombinerAlphaOperand_C = textureCombinerStage.OperandValue.Operand_Alpha_C_Flag;
            UnknownOperandFlag1 = textureCombinerStage.OperandValue.UnknownFlag1;

            UnknownByte0 = textureCombinerStage.UnknownByte0;
            UnknownData0 = textureCombinerStage.UnknownData0;
            UnknownData2 = textureCombinerStage.UnknownData2;
            UnknownData3 = textureCombinerStage.UnknownData3;
            UnknownData4 = textureCombinerStage.UnknownData4;
        }
    }

    //SHDRViewer
    public class SHDREditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (svc != null && value != null)
            {
                SHDRViewer form = new SHDRViewer(value as SHDR);
                form.ShowDialog();

                value = form.SHDR;
            }
            return value; // can also replace the wrapper object here
        }
    }
}
