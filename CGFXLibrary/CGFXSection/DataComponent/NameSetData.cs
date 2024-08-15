using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection.DataComponent
{
    public class NameSetData : IO.BinaryIOInterface.BinaryIO
    {
        public string Name;
        public int NameOffset { get; set; }
        public int UnknownData1 { get; set; }

        public void ReadNameSet(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
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
        }

        public override void Read(BinaryReader br, byte[] BOM)
        {
            ReadNameSet(br, BOM);
        }

        public override void Write(BinaryWriter bw, byte[] BOM)
        {
            throw new NotImplementedException();
        }

        public NameSetData()
        {
            NameOffset = 0;
            UnknownData1 = 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
