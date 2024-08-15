using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Animation (CANM) => 0x00000000 (IdentFlag : None)
    /// 
    /// Skeleton Animations
    /// Texture Animations
    /// Visibility Animations
    /// Camera Animations
    /// Light Animations
    /// </summary>
    public class CANM : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;
        public string CANM_TangentAttributeName;

        public char[] CANM_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }
        public int CANM_AttributeNameOffset { get; set; }
        public int LoopMode { get; set; }
        public float AnimFrameCount { get; set; }

        public int MemberAnimationDICTCount { get; set; }
        public int MemberAnimationDICTOffset { get; set; }
        public DICT MemberAnimationDICT { get; set; }

        public int NumOfUserDataDICTCount { get; set; }
        public int UserDataDICTOffset { get; set; }
        public DICT UserDataDICT { get; set; }

        public void ReadCANM(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CANM_Header = br.ReadChars(4);
            if (new string(CANM_Header) != "CANM") throw new Exception("不明なフォーマットです");

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

            CANM_AttributeNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (CANM_AttributeNameOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move NameOffset
                br.BaseStream.Seek(CANM_AttributeNameOffset, SeekOrigin.Current);

                ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                readByteLine.ReadByte(br, 0x00);

                CANM_TangentAttributeName = new string(readByteLine.ConvertToCharArray());

                br.BaseStream.Position = Pos;
            }

            LoopMode = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            AnimFrameCount = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);

            MemberAnimationDICTCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            MemberAnimationDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (MemberAnimationDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DICTOffset
                br.BaseStream.Seek(MemberAnimationDICTOffset, SeekOrigin.Current);

                MemberAnimationDICT.ReadDICT(br, BOM, false, new CGFXSection.DataComponent.AnimationData());

                br.BaseStream.Position = Pos;
            }

            NumOfUserDataDICTCount = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DICTOffset
                br.BaseStream.Seek(UserDataDICTOffset, SeekOrigin.Current);

                UserDataDICT.ReadDICT(br, BOM, true);

                br.BaseStream.Position = Pos;
            }
        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            ReadCANM(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        public CANM()
        {
            CANM_Header = "CANM".ToArray();
            Revision = new byte[4];
            NameOffset = 0;
            CANM_AttributeNameOffset = 0;

            LoopMode = 0;
            AnimFrameCount = 0f;

            MemberAnimationDICTCount = 0;
            MemberAnimationDICTOffset = 0;
            MemberAnimationDICT = new DICT();

            NumOfUserDataDICTCount = 0;
            UserDataDICTOffset = 0;
            UserDataDICT = new DICT();
        }
    }
}
