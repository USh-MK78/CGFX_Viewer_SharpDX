﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary.CGFXSection
{
    /// <summary>
    /// Hemisphere Light
    /// </summary>
    public class CHLT
    {
        public string Name;
        public char[] CHLT_Header { get; set; }
        public byte[] Revision { get; set; }
        public int NameOffset { get; set; }

        public void ReadCHLT(BinaryReader br, byte[] BOM)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            CHLT_Header = br.ReadChars(4);
            if (new string(CHLT_Header) != "CHLT") throw new Exception("不明なフォーマットです");

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
        }

        public CHLT()
        {
            CHLT_Header = "CHLT".ToArray();
            Revision = new byte[4];
            NameOffset = 0;
        }
    }
}
