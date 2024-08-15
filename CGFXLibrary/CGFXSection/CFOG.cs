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
    /// Fogs
    /// </summary>
    public class CFOG : IO.BinaryIOInterface.BinaryIO
    {
        //public long EndPos;
        public string Name;

        public char[] CFOG_Header { get; set; } //0x4
        public byte[] CFOG_UnknownBytes_2 { get; set; } //0x4
        public int CFOG_NameStringOffset { get; set; } //0x4
        public int CFOG_NumOfUserDataDICTEntries { get; set; } //0x4
        public int CFOG_UserDataDICTOffset { get; set; } //0x4
        public DICT UserDataDict { get; set; }
        public FogFlipSetting FogFlipSettings { get; set; }
        public class FogFlipSetting
        {
            public int Z_flip { get; set; } //0x4

            public FlipSetting FlipSettings
            {
                get => (FlipSetting)Z_flip;
                set => Z_flip = (int)value;
            }

            public enum FlipSetting
            {
                Enable = 3,
                Disable = 1
            }

            /// <summary>
            /// 1 : Disable, 3 = Enable
            /// </summary>
            /// <param name="InputZFlip"></param>
            public FogFlipSetting(int InputZFlip)
            {
                Z_flip = InputZFlip;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="flipSetting"></param>
            public FogFlipSetting(FlipSetting flipSetting = FlipSetting.Disable)
            {
                FlipSettings = flipSetting;
            }
        }

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
        public CFOG_RGBA Color_RGBA { get; set; } //RGBA
        public class CFOG_RGBA
        {
            public float Color_R { get; set; } //0x4
            public float Color_G { get; set; } //0x4
            public float Color_B { get; set; } //0x4
            public float Color_A { get; set; } //0x4

            public CFOG_RGBA(float R, float G, float B, float A)
            {
                Color_R = R;
                Color_G = G;
                Color_B = B;
                Color_A = A;
            }

            public void ReadCFOGRGBA(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Color_R = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Color_G = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Color_B = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Color_A = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public int CFOG_UnknownOffset1 { get; set; } //0x4(UserData : float)
        public CGFXData UnknownUserData { get; set; } //ColorData

        public int FogSettingOffset { get; set; } //0x4
        public CFOGSetting CFOGSettings { get; set; }
        public class CFOGSetting
        {
            public int FogType { get; set; } //0x4()
            public FogSuffixType FogSuffixTypes => GetFogType();

            public float FogStart { get; set; } //0x4
            public float FogEnd { get; set; } //0x4
            public float Concentration { get; set; } //0x4

            public enum FogSuffixType
            {
                LinearExponential = 1,
                ExponentialFunctions = 2,
                SquareOfExponentialFunctions = 3
            }

            public FogSuffixType GetFogType()
            {
                return (FogSuffixType)FogType;
            }

            public void ReadCFOGSetting(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                FogType = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                FogStart = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                FogEnd = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Concentration = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }

            public CFOGSetting()
            {
                FogType = 0;
                FogStart = 0;
                FogEnd = 0;
                Concentration = 0;
            }
        }

        public void ReadCFOG(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CFOG_Header = br.ReadChars(4);
            if (new string(CFOG_Header) != "CFOG") throw new Exception("不明なフォーマットです");

            CFOG_UnknownBytes_2 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_NameStringOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CFOG_NameStringOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(CFOG_NameStringOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                Name = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            CFOG_NumOfUserDataDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CFOG_UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CFOG_UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CFOG_UserDataDICTOffset, SeekOrigin.Current);

                UserDataDict.ReadDICT(br, BOM, true);

                br.BaseStream.Position = Pos;
            }

            FogFlipSettings.Z_flip = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            CFOG_UnknownBytes_5 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_6 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_7 = endianConvert.Convert(br.ReadBytes(4));
            CFOG_UnknownBytes_8 = endianConvert.Convert(br.ReadBytes(4)); //FogAnimCount (?)
            CFOG_CFOGAnimation_DICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CFOG_CFOGAnimation_DICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CFOG_CFOGAnimation_DICTOffset, SeekOrigin.Current);

                FogAnimationDICT.ReadDICT(br, BOM, true);
                //ColorDICT.ReadDICT(br, BOM, true);

                #region ColorDICT, 0x00000400
                var SetPos = ((CGFXSection.DataComponent.CGFXUserData.RealNumber)FogAnimationDICT.DICT_Entries.Last().CGFXData.CGFXDataSection).EndPos;
                //br.BaseStream.Position = SetPos;

                //long p = br.BaseStream.Position;

                //ColorDICT.ReadDICT(br, BOM, true);
                #endregion

                br.BaseStream.Position = Pos;
            }

            Transform_Scale.ReadScale(br, BOM);
            Transform_Rotate.ReadRotate(br, BOM);
            Transform_Translate.ReadTranslate(br, BOM);
            CFOG_4x4_Matrix.ReadLocalMatrix(br, BOM);
            CFOG_4x4_Matrix_Transform.ReadMatrix4x4Transform(br, BOM);
            Color_RGBA.ReadCFOGRGBA(br, BOM);

            //UserDataType => byte (?)(No DICT)
            CFOG_UnknownOffset1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CFOG_UnknownOffset1 != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(CFOG_UnknownOffset1, SeekOrigin.Current);

                //TODO : ValueSet(DICTSetionの根本的な修正が必要 (?))
                //UserData => Float
                //UnknownUserData.Reader(br, BOM);

                br.BaseStream.Position = Pos;
            }

            FogSettingOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (FogSettingOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(FogSettingOffset, SeekOrigin.Current);

                CFOGSettings.ReadCFOGSetting(br, BOM);

                br.BaseStream.Position = Pos;
            }
        }

        public override void Read(BinaryReader br, byte[] BOM = null)
        {
            ReadCFOG(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM = null)
        {
            throw new NotImplementedException();
        }

        public CFOG()
        {
            CFOG_Header = "CFOG".ToCharArray();
            CFOG_UnknownBytes_2 = new byte[4];
            CFOG_NameStringOffset = 0;
            CFOG_NumOfUserDataDICTEntries = 0;
            CFOG_UserDataDICTOffset = 0;
            UserDataDict = new DICT();
            FogFlipSettings = new FogFlipSetting(1);
            CFOG_UnknownBytes_5 = new byte[4];
            CFOG_UnknownBytes_6 = new byte[4];
            CFOG_UnknownBytes_7 = new byte[4];
            CFOG_UnknownBytes_8 = new byte[4];
            CFOG_CFOGAnimation_DICTOffset = 0;

            FogAnimationDICT = new DICT();
            ColorDICT = new DICT();

            Transform_Scale = new Transform.Scale();
            Transform_Rotate = new Transform.Rotate();
            Transform_Translate = new Transform.Translate();
            CFOG_4x4_Matrix = new MatrixData.LocalMatrix();
            CFOG_4x4_Matrix_Transform = new MatrixData.WorldMatrix_Transform();
            Color_RGBA = new CFOG_RGBA(0, 0, 0, 0);
            CFOG_UnknownOffset1 = 0;
            UnknownUserData = new CGFXData(null, true);

            FogSettingOffset = 0;
            CFOGSettings = new CFOGSetting();
        }
    }
}
