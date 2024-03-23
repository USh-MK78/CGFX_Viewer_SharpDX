using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary
{
    public class DICT
    {
        public char[] DICT_Header { get; set; } //0x4
        public int DICTSize { get; set; } //0x4
        public int DICT_NumOfEntries { get; set; } //0x4

        public RootNode RootNodeData { get; set; }
        public class RootNode
        {
            public string Name;
            public DICTEntryFlags RN_RefBit { get; set; } //0x4
            public short RN_LeftIndex { get; set; } //0x2
            public short RN_RightIndex { get; set; } //0x2
            public int RN_NameOffset { get; set; } //0x4
            public int RN_DataOffset { get; set; } //0x4

            public RootNode(uint RefBit, short LeftIndex, short RightIndex, int NameOffset, int DataOffset)
            {
                RN_RefBit = new DICTEntryFlags(RefBit);
                RN_LeftIndex = LeftIndex;
                RN_RightIndex = RightIndex;
                RN_NameOffset = NameOffset;
                RN_DataOffset = DataOffset;
            }

            public RootNode()
            {
                RN_RefBit = new DICTEntryFlags(0);
                RN_LeftIndex = 0;
                RN_RightIndex = 0;
                RN_NameOffset = 0;
                RN_DataOffset = 0;
            }

            public void Read_RootNode(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                RN_RefBit = new DICTEntryFlags(BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                RN_LeftIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RN_RightIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RN_NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (RN_NameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move RN_NameOffset
                    br.BaseStream.Seek(RN_NameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    Name = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;
                }

                RN_DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (RN_DataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move RN_DataOffset
                    br.BaseStream.Seek(RN_DataOffset, SeekOrigin.Current);


                    //Read Section Data (?)



                    br.BaseStream.Position = Pos;
                }
            }

            public int GetLength()
            {
                return 16;
            }
        }

        public List<DICT_Entry> DICT_Entries { get; set; }
        public class DICT_Entry
        {
            public string Name;

            public DICTEntryFlags RefBit { get; set; } //0x4
            public short LeftIndex { get; set; } //0x2
            public short RightIndex { get; set; } //0x2
            public int NameOffset { get; set; } //0x4
            public int DataOffset { get; set; } //0x4
            public CGFXData CGFXData { get; set; }

            public void Read_DICTEntry(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                RefBit = new DICTEntryFlags(BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
                LeftIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RightIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
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

                DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (DataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move DataOffset
                    br.BaseStream.Seek(DataOffset, SeekOrigin.Current);

                    CGFXData = new CGFXData(br.ReadBytes(4));
                    CGFXData.Reader(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            //public void Read_DICTEntry_ValueSet(BinaryReader br, byte[] BOM)
            //{
            //    EndianConvert endianConvert = new EndianConvert(BOM);
            //    RefBit = new DICTEntryFlags(BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0));
            //    LeftIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            //    RightIndex = BitConverter.ToInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
            //    NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            //    if (NameOffset != 0)
            //    {
            //        long Pos = br.BaseStream.Position;

            //        br.BaseStream.Seek(-4, SeekOrigin.Current);

            //        //Move NameOffset
            //        br.BaseStream.Seek(NameOffset, SeekOrigin.Current);

            //        ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
            //        readByteLine.ReadByte(br, 0x00);

            //        Name = new string(readByteLine.ConvertToCharArray());

            //        br.BaseStream.Position = Pos;
            //    }

            //    DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            //    if (DataOffset != 0)
            //    {
            //        long Pos = br.BaseStream.Position;

            //        br.BaseStream.Seek(-4, SeekOrigin.Current);

            //        //Move DataOffset
            //        br.BaseStream.Seek(DataOffset, SeekOrigin.Current);

            //        CGFXData = new CGFXData(br.ReadBytes(4), CGFXData.Key.ValueSet);
            //        CGFXData.ValueData.ReadValueDataItemList(br, BOM);
            //        //CGFXData = new CGFXData(br.ReadBytes(4));
            //        //CGFXData.Reader(br, BOM);

            //        br.BaseStream.Position = Pos;
            //    }
            //}

            //public void Write_DICTEntry(BinaryWriter bw, byte[] BOM, bool IdentFlag)
            //{
            //    bw.Write(RefBit);
            //    bw.Write(LeftIndex);
            //    bw.Write(RightIndex);
            //    //bw.Write(Name)
            //}

            public DICT_Entry(uint RefBit, short LeftIndex, short RightIndex, int NameOffset, int DataOffset, CGFXData CGFXData)
            {
                this.RefBit = new DICTEntryFlags(RefBit);
                this.LeftIndex = LeftIndex;
                this.RightIndex = RightIndex;
                this.NameOffset = NameOffset;
                this.DataOffset = DataOffset;
                this.CGFXData = CGFXData;
            }

            public DICT_Entry()
            {
                RefBit = new DICTEntryFlags(0);
                LeftIndex = 0;
                RightIndex = 0;
                NameOffset = 0;
                DataOffset = 0;
                CGFXData = null;
            }

            public int GetLength()
            {
                return 16;
            }

            public string GetEntryName()
            {
                return Name;
            }

            public override string ToString()
            {
                var f = RefBit.GetDICTRefBitFlags();
                string str = string.Join(" | ", f);
                return Name + " (" + str + ")";
            }
        }

        public DICT()
        {
            DICT_Header = "DICT".ToCharArray();
            DICTSize = 0;
            DICT_NumOfEntries = 0;
            RootNodeData = new RootNode();
            DICT_Entries = new List<DICT_Entry>();
        }

        public void ReadDICT(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            DICT_Header = br.ReadChars(4);
            if (new string(DICT_Header) != "DICT") throw new Exception("不明なフォーマットです");

            DICTSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            DICT_NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            RootNodeData.Read_RootNode(br, BOM);

            for (int i = 0; i < DICT_NumOfEntries; i++)
            {
                DICT_Entry dICT_Entry = new DICT_Entry();
                dICT_Entry.Read_DICTEntry(br, BOM);
                DICT_Entries.Add(dICT_Entry);
            }
        }

        public int GetLength()
        {
            int H = DICT_Header.Length;
            int Size = 4;
            int NumOfEntries = 4;
            int RNSize = RootNodeData.GetLength();
            int count = DICT_Entries.Select(x => x.GetLength()).Sum();

            return H + Size + NumOfEntries + RNSize + count;
        }
    }
}
