using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SharpDX;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections;
using static CGFXLibrary.Flags;
using System.Xml.Linq;
using CGFXLibrary.CGFXSection;
using CGFXLibrary.SOBJ_Format;
using CGFXLibrary.IO;

namespace CGFXLibrary
{
    public class CGFXFormat
    {
        public enum CGFXEntryData
        {
            Models = 1, //CMDL
            Textures = 2, //TXOB
            LookupTables = 3, //LUTS
            Materials = 4, //MTOB
            Shaders = 5, //SHDR
            Cameras = 6,
            Lights = 7,
            Fogs = 8, //CFOG
            Environments = 9, //CENV
            Skeleton_Animations = 10,
            Texture_Animations = 11,
            Visibility_Animations = 12,
            Camera_Animations = 13,
            Light_Animations = 14,
            Fog_Animations = 15,
            Emitters = 16
        }

        public static Dictionary<int, string> GetAllCGFXEntryName()
        {
            return Enum.GetValues(typeof(CGFXEntryData)).Cast<CGFXEntryData>().ToDictionary(Key => (int)Key, Value => Enum.GetName(typeof(CGFXEntryData), Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetCGFXEntryNameArray()
        {
            return Enum.GetNames(typeof(CGFXEntryData));
        }

        public class CGFX
        {
            public char[] CGFX_Header { get; set; } //0x4
            public byte[] BOM { get; set; } //0x2
            public short CGFXHeaderSize { get; set; } //0x2
            public int Revision { get; set; } //0x4
            public int FileSize { get; set; } //0x4
            public int NumOfEntries { get; set; } //0x4
            public DATA Data { get; set; }
            public class DATA
            {
                public char[] DATA_Header { get; set; } //0x4
                public int DATASize { get; set; } //0x4
                public Dictionary<CGFXEntryData, DictOffset> DictOffset_Dictionary { get; set; }

                public class DictOffset
                {
                    public int NumOfEntries { get; set; }
                    public int OffsetToDICT { get; set; }

                    public DictOffset(int NumOfEntries, int OffsetToDICT)
                    {
                        this.NumOfEntries = NumOfEntries;
                        this.OffsetToDICT = OffsetToDICT;
                    }

                    public DictOffset()
                    {
                        NumOfEntries = 0;
                        OffsetToDICT = 0;
                    }

                    public void ReadDictOffset(BinaryReader br, byte[] BOM)
                    {
                        EndianConvert endianConvert = new EndianConvert(BOM);
                        NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                        OffsetToDICT = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                    }
                }

                public DATA()
                {
                    DATA_Header = "DATA".ToCharArray();
                    DATASize = 0;
                    DictOffset_Dictionary = new Dictionary<CGFXEntryData, DictOffset>();
                    //DictOffset_Dictionary = new Dictionary<string, DictOffset>();
                }

                public void ReadDATA(BinaryReader br, byte[] BOM)
                {
                    DATA_Header = br.ReadChars(4);
                    if (new string(DATA_Header) != "DATA") throw new Exception("不明なフォーマットです");
                    EndianConvert endianConvert = new EndianConvert(BOM);
                    DATASize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                    foreach (var EntryName in GetCGFXEntryNameArray())
                    {
                        DictOffset dictOffset = new DictOffset();
                        dictOffset.ReadDictOffset(br, BOM);
                        DictOffset_Dictionary.Add((CGFXEntryData)Enum.Parse(typeof(CGFXEntryData), EntryName), dictOffset);
                    }
                }
            }

            public Dictionary<CGFXEntryData, DICT> DICTAndSectionData { get; set; }

            public void ReadCGFX(BinaryReader br)
            {
                CGFX_Header = br.ReadChars(4);
                if (new string(CGFX_Header) != "CGFX") throw new Exception("不明なフォーマットです");
                BOM = br.ReadBytes(2);

                EndianConvert endianConvert = new EndianConvert(BOM);
                CGFXHeaderSize = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                Revision = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                FileSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                Data.ReadDATA(br, BOM);

                foreach (var EntryName in GetCGFXEntryNameArray())
                {
                    CGFXEntryData Entry = (CGFXEntryData)Enum.Parse(typeof(CGFXEntryData), EntryName);
                    if (Data.DictOffset_Dictionary[Entry].NumOfEntries != 0)
                    {
                        if (Entry == CGFXEntryData.Skeleton_Animations || Entry == CGFXEntryData.Texture_Animations || Entry == CGFXEntryData.Visibility_Animations || Entry == CGFXEntryData.Camera_Animations || Entry == CGFXEntryData.Light_Animations || Entry == CGFXEntryData.Fog_Animations)
                        {
                            DICT dICT = new DICT();
                            dICT.ReadDICT(br, BOM, false, new CANM());
                            DICTAndSectionData.Add(Entry, dICT);
                        }
                        else
                        {
                            DICT dICT = new DICT();
                            dICT.ReadDICT(br, BOM, true);
                            DICTAndSectionData.Add(Entry, dICT);
                        }
                    }
                }
            }

            public CGFX()
            {
                CGFX_Header = "CGFX".ToCharArray();
                BOM = new byte[2];
                CGFXHeaderSize = 0;
                Revision = 0;
                FileSize = 0;
                NumOfEntries = 0;
                Data = new DATA();
                DICTAndSectionData = new Dictionary<CGFXEntryData, DICT>();
                //DICTAndSectionData = new Dictionary<string, DICT>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CGFXData : CGFXDataItemInterface.AbstractCGFXDataItem
        {
            //public Flags Flag => Flags;

            public IO.BinaryIOInterface.BinaryIO CGFXDataSection { get; set; }

            public override bool EnableFlagValue { get; set; }

            /// <summary>
            /// Get CGFXData
            /// </summary>
            /// <typeparam name="T">IO.BinaryIOInterface.BinaryIO</typeparam>
            /// <returns></returns>
            public T GetCGFXData<T>()
            {
                return (T)(object)CGFXDataSection;
            }

            public void Reader(BinaryReader br, byte[] BOM)
            {
                //TODO : 列挙型ではなくビット演算を使用して解決できるようにする

                if (EnableFlagValue == false)
                {
                    CGFXDataSection.Read(br, BOM);
                }
                else if (EnableFlagValue == true)
                {
                    CGFXFlags = new Flags(br.ReadBytes(4));

                    if (CGFXFlags.GetIdentFlagUInt() == 0)
                    {
                        #region DELETE
                        ////CANM (DataSection (?))
                        //CGFXDataSection = new CGFXSection.DataComponent.AnimationData();
                        //CGFXDataSection.Read(br, BOM);

                        ////AnimationDataSet.Read(br, BOM);
                        #endregion
                    }
                    else if (CGFXFlags.GetIdentFlagUInt() > 0)
                    {
                        bool IsUserData = (CGFXFlags.GetIdentFlagUInt() & 0x0FFFFFFF) == 0 ? true : false;
                        if (IsUserData)
                        {
                            if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F32))
                            {
                                //UserData (Real Number, Float)
                                CGFXDataSection = new CGFXSection.DataComponent.CGFXUserData.RealNumber(CGFXSection.DataComponent.CGFXUserData.RealNumber.UD_FloatType.Float);
                                CGFXDataSection.Read(br, BOM);
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F31))
                            {
                                //NameSet
                                CGFXDataSection = new CGFXSection.DataComponent.NameSetData();
                                CGFXDataSection.Read(br, BOM);
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F30))
                            {
                                //UserData (Int32)
                                CGFXDataSection = new CGFXSection.DataComponent.CGFXUserData.Int32Data();
                                CGFXDataSection.Read(br, BOM);
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F29))
                            {
                                //UserData (String)
                                CGFXDataSection = new CGFXSection.DataComponent.CGFXUserData.StringData();
                                CGFXDataSection.Read(br, BOM);
                            }
                        }
                        else
                        {
                            if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F32))
                            {
                                //Shader
                                if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //SHDR
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "SHDR")
                                    {
                                        //SHDR
                                        CGFXDataSection = new SHDR();
                                        CGFXDataSection.Read(br, BOM);
                                    }
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F31))
                            {
                                if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F6))
                                {
                                    if ((CGFXFlags.GetIdentFlagUInt() & 0x0000FF00) == 0)
                                    {
                                        //Fragment
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CFLT")
                                        {
                                            //CFLT
                                            CGFXDataSection = new CFLT();
                                            CGFXDataSection.Read(br, BOM);
                                        }
                                    }
                                    else if ((CGFXFlags.GetIdentFlagUInt() & 0x0000FF00) != 0)
                                    {
                                        if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F9))
                                        {
                                            //Hemisphere
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CHLT")
                                            {
                                                //CHLT
                                                CGFXDataSection = new CHLT();
                                                CGFXDataSection.Read(br, BOM);
                                            }
                                        }
                                        else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F10))
                                        {
                                            //VertexLight
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CVLT")
                                            {
                                                //CVLT
                                                CGFXDataSection = new CVLT();
                                                CGFXDataSection.Read(br, BOM);
                                            }
                                        }
                                        else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F11))
                                        {
                                            //AmbientLight
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CALT")
                                            {
                                                //CALT
                                                CGFXDataSection = new CALT();
                                                CGFXDataSection.Read(br, BOM);
                                            }
                                        }
                                    }
                                }
                                else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F4))
                                {
                                    //Has CameraSection
                                    if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F2))
                                    {
                                        //CCAM
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CCAM")
                                        {
                                            CGFXDataSection = new CCAM();
                                            CGFXDataSection.Read(br, BOM);
                                        }
                                    }
                                }
                                else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F2))
                                {
                                    if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F5))
                                    {
                                        if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F8))
                                        {
                                            //CMDL (Primitive)
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CMDL")
                                            {
                                                CGFXDataSection = new CMDL();
                                                CGFXDataSection.Read(br, BOM);
                                            }
                                        }
                                        else if (!CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F8))
                                        {
                                            //CMDL (SimplificationSkeletalModel)
                                            char[] ty = br.ReadChars(4);
                                            br.BaseStream.Seek(-4, SeekOrigin.Current);
                                            if (new string(ty) == "CMDL")
                                            {
                                                CGFXDataSection = new CMDL();
                                                CGFXDataSection.Read(br, BOM);
                                            }
                                        }
                                    }
                                    else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F7))
                                    {
                                        //CFOG
                                        char[] ty = br.ReadChars(4);
                                        br.BaseStream.Seek(-4, SeekOrigin.Current);
                                        if (new string(ty) == "CFOG")
                                        {
                                            //CFOG
                                            CGFXDataSection = new CFOG();
                                            CGFXDataSection.Read(br, BOM);
                                        }
                                    }
                                }
                                else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F3 & CGFXIdentFlag.F2))
                                {
                                    //PEMT
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "PEMT")
                                    {
                                        //PEMT
                                        CGFXDataSection = new PEMT();
                                        CGFXDataSection.Read(br, BOM);
                                    }
                                }
                                else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //CNOD
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "CNOD")
                                    {
                                        //CNOD
                                        CGFXDataSection = new CNOD();
                                        CGFXDataSection.Read(br, BOM);
                                    }
                                }
                                else
                                {

                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F30))
                            {
                                //Texture
                                if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F3))
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "TXOB")
                                    {
                                        //TXOB(Material)
                                        CGFXDataSection = new TXOB.MaterialInfo();
                                        CGFXDataSection.Read(br, BOM);

                                        //TXOB_MaterialSection.MaterialInfoSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                    }

                                }
                                else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F5) | CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "TXOB")
                                    {
                                        //TXOB(Texture:Shader)
                                        CGFXDataSection = new TXOB.Texture();
                                        CGFXDataSection.Read(br, BOM);


                                        //.TextureSection.ReadTXOB(br, new byte[] { 0xFF, 0xFE });
                                    }
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F29))
                            {
                                //Geometry
                                if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F1))
                                {
                                    //SOBJ (Shape)
                                    char[] ty = br.ReadChars(4);
                                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                                    if (new string(ty) == "SOBJ")
                                    {
                                        CGFXDataSection = new SOBJ(SOBJ.SOBJType.Shape);
                                        CGFXDataSection.Read(br, BOM);

                                        //SOBJ_Shape_Section.Read_SOBJ(br, BOM);
                                    }
                                }

                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F28))
                            {
                                //Material
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "MTOB")
                                {
                                    //MTOB
                                    CGFXDataSection = new MTOB();
                                    CGFXDataSection.Read(br, BOM);

                                    //MTOBSection.ReadMTOB(br, BOM);
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F27))
                            {
                                //LookUpTable
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "LUTS")
                                {
                                    //LUTS
                                    CGFXDataSection = new LUTS();
                                    CGFXDataSection.Read(br, BOM);

                                    //LUTSSection.ReadLUTS(br, BOM);
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F26))
                            {
                                //SOBJ_Skeleton

                                //char[] ty = br.ReadChars(4);
                                //br.BaseStream.Seek(-4, SeekOrigin.Current);
                                //if (new string(ty) == "SOBJ")
                                //{

                                //}
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F25))
                            {
                                //SOBJ_Mesh
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "SOBJ")
                                {
                                    CGFXDataSection = new SOBJ(SOBJ.SOBJType.Mesh);
                                    CGFXDataSection.Read(br, BOM);

                                    //SOBJ_Mesh_Section.Read_SOBJ(br, BOM);
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F24))
                            {
                                //CENV
                                char[] ty = br.ReadChars(4);
                                br.BaseStream.Seek(-4, SeekOrigin.Current);
                                if (new string(ty) == "CENV")
                                {
                                    //CENV
                                    CGFXDataSection = new CENV();
                                    CGFXDataSection.Read(br, BOM);

                                    //CENVSection.Read_CENV(br, BOM);
                                }
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F3))
                            {
                                #region DELETE
                                ////CANM (DataSection (?))
                                //CGFXDataSection = new CGFXSection.DataComponent.AnimationData();
                                //CGFXDataSection.Read(br, BOM);
                                #endregion
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F4))
                            {
                                #region DELETE
                                ////CANM (DataSection (?))
                                //CGFXDataSection = new CGFXSection.DataComponent.AnimationData();
                                //CGFXDataSection.Read(br, BOM);

                                ////AnimationDataSet.Read(br, BOM);
                                #endregion
                            }
                            else if (CGFXFlags.GetFlags().HasFlag(CGFXIdentFlag.F19))
                            {
                                //CFOG ColorProperty
                                CGFXDataSection = new CGFXSection.DataComponent.CFOG_UnknownData0();
                                CGFXDataSection.Read(br, BOM);
                            }
                        }
                    }
                }
            }

            public override void Read(BinaryReader br, byte[] BOM)
            {
                Reader(br, BOM);
            }

            public override void Write(BinaryWriter bw, byte[] BOM)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="dataEntry"></param>
            /// <param name="EnableFlag"></param>
            public CGFXData(BinaryIOInterface.BinaryIO dataEntry = null, bool EnableFlag = true) : base(EnableFlag)
            {
                CGFXDataSection = dataEntry;
                EnableFlagValue = EnableFlag;
            }
        }
    }
}
