using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX
{
    internal class BitOperationTool
    {
        public class Shift
        {
            public static uint ToLeft(int Shift, byte[] data)
            {
                return BitConverter.ToUInt32(data, 0) << Shift;
            }

            public static uint ToLeft(int Shift, uint data)
            {
                return data << Shift;
            }

            public static uint ToRight(int Shift, byte[] data)
            {
                return BitConverter.ToUInt32(data, 0) >> Shift;
            }

            public static uint ToRight(int Shift, uint data)
            {
                return data >> Shift;
            }
        }

        public struct StringConvert
        {
            public string BitCode;

            public int ToInt()
            {
                return Convert.ToInt32(BitCode.Replace("0b", "").Replace("_", "").ToString(), 2);
            }

            public string Replace(string BitCode, int g)
            {
                var t = BitCode.PadLeft(4);
                //if (BitCode.Length <= 4)
                this.BitCode.Replace("0b", "").Split('_').ToArray().ToList()[g - 1] = t;

                for (int i = 0; i < this.BitCode.Replace("0b", "").Split('_').ToArray().Length; i++)
                {
                    if (i == g)
                    {
                        this.BitCode.Replace("0b", "").Split('_').ToArray()[i] = t;
                    }
                }


                return BitCode;
            }

            public StringConvert(string BitCode)
            {
                string Bit = null;

                bool ch = false;
                foreach (var n in BitCode.Replace("0b", "").Split('_').ToArray())
                {
                    if ((n.Length % 4) == 0) ch = true;
                    else
                    {
                        ch = false;
                        break;
                    }
                }

                if (ch == true) Bit = BitCode;
                else Bit = "Error : " + BitCode;

                this.BitCode = "0b" + Bit;
            }
        }

        //public class Convert
        //{
        //    //public uint 
        //}

        public void t()
        {
            StringConvert stringConvert = new StringConvert("0b1111_1111_0000_1111");

        }
    }
}
