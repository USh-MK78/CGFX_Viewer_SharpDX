using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary
{
    public class Transform
    {
        public Scale TrScale { get; set; }
        public class Scale
        {
            public float Scale_X { get; set; }
            public float Scale_Y { get; set; }
            public float Scale_Z { get; set; }

            public Scale()
            {
                Scale_X = 0;
                Scale_Y = 0;
                Scale_Z = 0;
            }

            public void ReadScale(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Scale_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Scale_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Scale_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public Rotate TrRotate { get; set; }
        public class Rotate
        {
            public float Rotate_X { get; set; }
            public float Rotate_Y { get; set; }
            public float Rotate_Z { get; set; }

            public Rotate()
            {
                Rotate_X = 0;
                Rotate_Y = 0;
                Rotate_Z = 0;
            }

            public void ReadRotate(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Rotate_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Rotate_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Rotate_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }

        public Translate TrTranslate { get; set; }
        public class Translate
        {
            public float Translate_X { get; set; }
            public float Translate_Y { get; set; }
            public float Translate_Z { get; set; }

            public Translate()
            {
                Translate_X = 0;
                Translate_Y = 0;
                Translate_Z = 0;
            }

            public void ReadTranslate(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                Translate_X = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Translate_Y = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
                Translate_Z = BitConverter.ToSingle(endianConvert.Convert(br.ReadBytes(4)), 0);
            }
        }
    }
}
