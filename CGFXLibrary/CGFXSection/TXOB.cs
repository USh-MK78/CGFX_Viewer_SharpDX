using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Texture
    /// </summary>
    public class TXOB
    {
        public enum Type
        {
            Texture,
            MaterialInfo
        }

        public Texture TextureSection { get; set; } //0x11000020
        public class Texture
        {
            public string Name;
            public char[] TXOB_Header { get; set; }
            public int UnknownData1 { get; set; }
            public int TXOBNameOffset { get; set; }

            public byte[] UnknownByte1 { get; set; }
            public byte[] UnknownByte2 { get; set; }

            public int TextureHeight { get; set; }
            public int TextureWidth { get; set; }

            public byte[] UnknownByte3 { get; set; }
            public byte[] UnknownByte4 { get; set; }

            public int MipMapLevel { get; set; }

            public byte[] UnknownByte5 { get; set; }
            public byte[] UnknownByte6 { get; set; }

            public int TexFormat { get; set; }
            public CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.ImageFormat ImageFormat
            {
                get
                {
                    return (CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.ImageFormat)TexFormat;
                }
                set
                {
                    TexFormat = (int)value;
                }
            }

            public int UnknownData2 { get; set; }

            public int TextureHeight2 { get; set; }
            public int TextureWidth2 { get; set; }

            public int TextureDataSize { get; set; }

            public int TextureDataOffset { get; set; }

            public byte[] TexData { get; set; }
            public Bitmap TXOB_Bitmap
            {
                get
                {
                    var bmp = CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.ToBitmap(TexData, TextureWidth, TextureHeight, ImageFormat);
                    bmp.Tag = Name;
                    return bmp;
                }
                set
                {
                    TexData = CGFX_Viewer_SharpDX.CGFX.TextureFormat.Textures.FromBitmap(value, ImageFormat);

                }
            }

            public byte[] UnknownByte7 { get; set; }
            public byte[] UnknownByte8 { get; set; }
            public byte[] UnknownByte9 { get; set; }
            public byte[] UnknownByte10 { get; set; }

            public Texture()
            {
                TXOB_Header = "TXOB".ToCharArray();
                UnknownData1 = 0;
                TXOBNameOffset = 0;

                UnknownByte1 = new List<byte>().ToArray();
                UnknownByte2 = new List<byte>().ToArray();

                TextureHeight = 0;
                TextureWidth = 0;

                UnknownByte3 = new List<byte>().ToArray();
                UnknownByte4 = new List<byte>().ToArray();

                MipMapLevel = 0;

                UnknownByte5 = new List<byte>().ToArray();
                UnknownByte6 = new List<byte>().ToArray();

                TexFormat = 0;

                UnknownData2 = 0;

                TextureHeight2 = 0;
                TextureWidth2 = 0;

                TextureDataSize = 0;
                TextureDataOffset = 0;

                TexData = new List<byte>().ToArray();

                UnknownByte7 = new List<byte>().ToArray();
                UnknownByte8 = new List<byte>().ToArray();
                UnknownByte9 = new List<byte>().ToArray();
                UnknownByte10 = new List<byte>().ToArray();
            }

            public void ReadTXOB(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                TXOB_Header = br.ReadChars(4);
                if (new string(TXOB_Header) != "TXOB") throw new Exception("不明なフォーマットです");
                UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                TXOBNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (TXOBNameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(TXOBNameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    Name = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;
                }

                UnknownByte1 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte2 = endianConvert.Convert(br.ReadBytes(4));

                TextureHeight = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                TextureWidth = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownByte3 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte4 = endianConvert.Convert(br.ReadBytes(4));

                MipMapLevel = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownByte5 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte6 = endianConvert.Convert(br.ReadBytes(4));

                TexFormat = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownData2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                TextureHeight2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                TextureWidth2 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                TextureDataSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                TextureDataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (TextureDataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(TextureDataOffset, SeekOrigin.Current);


                    TexData = endianConvert.Convert(br.ReadBytes(TextureDataSize));
                    //TexData = br.ReadBytes(TextureDataSize);

                    br.BaseStream.Position = Pos;
                }

                UnknownByte7 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte8 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte9 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte10 = endianConvert.Convert(br.ReadBytes(4));
            }
        }

        public MaterialInfo MaterialInfoSection { get; set; } //0x02000040
        public class MaterialInfo
        {
            public string Name;
            public char[] TXOB_Header { get; set; }
            public int UnknownData1 { get; set; } //Reverse (?)
            public int TXOBNameOffset { get; set; } //String itself is always empty (?)

            public byte[] UnknownByte1 { get; set; }
            public byte[] UnknownByte2 { get; set; }

            public string MTOB_MaterialName;
            public int MTOB_MaterialNameOffset { get; set; }

            public int UnknownByte3 { get; set; }
            public int UnknownByte4 { get; set; } //Reverse (?)

            public UnknownBitSet unknownBitSet { get; set; }
            public class UnknownBitSet
            {
                public byte Bit0 { get; set; }
                public byte Bit1 { get; set; }
                public byte Bit2 { get; set; }
                public byte Bit3 { get; set; }

                public void ReadUnknownBitSet(BinaryReader br)
                {
                    Bit0 = br.ReadByte();
                    Bit1 = br.ReadByte();
                    Bit2 = br.ReadByte();
                    Bit3 = br.ReadByte();
                }

                public UnknownBitSet(byte InputBit0, byte InputBit1, byte InputBit2, byte InputBit3)
                {
                    Bit0 = InputBit0;
                    Bit1 = InputBit1;
                    Bit2 = InputBit2;
                    Bit3 = InputBit3;
                }
            }

            public int UnknownByte5 { get; set; }
            public int UnknownByte6 { get; set; }
            public int UnknownByte7 { get; set; }
            public int UnknownByte8 { get; set; }

            public float UnknownByte9 { get; set; }
            public int UnknownByte10 { get; set; }

            public void ReadTXOB(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                TXOB_Header = br.ReadChars(4);
                if (new string(TXOB_Header) != "TXOB") throw new Exception("不明なフォーマットです");
                UnknownData1 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)).Reverse().ToArray(), 0);
                TXOBNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (TXOBNameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(TXOBNameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    Name = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;
                }

                UnknownByte1 = endianConvert.Convert(br.ReadBytes(4));
                UnknownByte2 = endianConvert.Convert(br.ReadBytes(4));

                MTOB_MaterialNameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (MTOB_MaterialNameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move NameOffset
                    br.BaseStream.Seek(MTOB_MaterialNameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    MTOB_MaterialName = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;
                }

                UnknownByte3 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownByte4 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)).Reverse().ToArray(), 0);

                unknownBitSet.ReadUnknownBitSet(br);

                UnknownByte5 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownByte6 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownByte7 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownByte8 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                UnknownByte9 = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                UnknownByte10 = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);

                //float
                //int

            }

            public MaterialInfo()
            {
                Name = "";
                MTOB_MaterialName = "";

                TXOB_Header = "TXOB".ToCharArray();
                UnknownData1 = 0;
                TXOBNameOffset = 0;

                UnknownByte1 = new byte[4];
                UnknownByte2 = new byte[4];

                MTOB_MaterialNameOffset = 0;

                UnknownByte3 = 0;
                UnknownByte4 = 0;

                unknownBitSet = new UnknownBitSet(0x00, 0x00, 0x00, 0x00);

                UnknownByte5 = 0;
                UnknownByte6 = 0;
                UnknownByte7 = 0;
                UnknownByte8 = 0;

                UnknownByte9 = 0;
                UnknownByte10 = 0;
            }
        }

        //public TXOB()
        //{
        //    TextureSection = new Texture();
        //    MaterialInfoSection = new MaterialInfo();
        //}

        public TXOB(Type type)
        {
            if (type == Type.Texture) TextureSection = new Texture();
            else if (type == Type.MaterialInfo) MaterialInfoSection = new MaterialInfo();
        }
    }
}
