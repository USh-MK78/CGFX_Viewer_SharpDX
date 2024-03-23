using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Lookup Table (0x00000004)
    /// </summary>
    public class LUTS
    {
        public string Name;

        public char[] LUTS_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }
        public int NumOfUserDataDICTEntries { get; set; }
        public int UserDataDICTOffset { get; set; }
        public DICT UserdataDICT { get; set; }
        public int NumOfDICTEntries { get; set; }
        public int DICTEntriesOffset { get; set; }
        //public List<DICT> DICTList { get; set; }
        public DICT LUTS_DICTData { get; set; } //LUTData => 0x00000080, (Int16, Bool (?))

        #region Delete (?)
        //public class LookupTableData
        //{
        //    public string Name;
        //    public Flags Flags { get; set; } //0x4
        //    public int NameOffset { get; set; } //0x4
        //    public bool IsAbsoluteValue { get; set; } //0x4
        //    public int UnknownOffset { get; set; }
        //    public int UnknownData1 { get; set; }
        //    public int UnknownData2 { get; set; }

        //    public byte[] UnknownData3 { get; set; }

        //    public int UnknownData4 { get; set; }
        //    public int UnknownData5 { get; set; }
        //    public int UnknownData6 { get; set; }
        //    public int UnknownData7 { get; set; }

        //    //UnknownList<float> (?)
        //}

        //public List<LookUpTable> LookUpTables { get; set; }
        //public class LookUpTable
        //{

        //}


        //public List<>

        //public List<DICT> DICTList { get; set; }
        #endregion

        public void ReadLUTS(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            LUTS_Header = br.ReadChars(4);
            if (new string(LUTS_Header) != "LUTS") throw new Exception("不明なフォーマットです");

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
            NumOfUserDataDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            UserDataDICTOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (UserDataDICTOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move UserDataOffset
                br.BaseStream.Seek(UserDataDICTOffset, SeekOrigin.Current);

                DICT dICT = new DICT();
                dICT.ReadDICT(br, BOM);
                UserdataDICT = dICT;

                //DICTList.Add(dICT);

                br.BaseStream.Position = Pos;
            }

            NumOfDICTEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0); //(?)
            DICTEntriesOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            if (DICTEntriesOffset != 0)
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move LookupTableProperty Offset
                br.BaseStream.Seek(DICTEntriesOffset, SeekOrigin.Current);

                //DICT dICT = new DICT();
                //dICT.ReadDICT(br, BOM);

                br.BaseStream.Position = Pos;
            }
        }

        public LUTS()
        {
            LUTS_Header = "LUTS".ToArray();
            Revision = new byte[4];
            NameOffset = 0;
            Name = "";
            NumOfUserDataDICTEntries = 0;
            UserDataDICTOffset = 0;
            UserdataDICT = new DICT();
            NumOfDICTEntries = 0;
            DICTEntriesOffset = 0;
            LUTS_DICTData = new DICT();
            //DICTList = new List<DICT>();
        }
    }
}
