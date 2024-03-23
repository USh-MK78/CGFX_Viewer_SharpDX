using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockBitImageTools
{
    public class LockBitsImageTool
    {
        public string FilePath { get; set; }
        public Bitmap Bitmap { get; set; }
        public LockBitsSetting LockBitsSettings { get; set; }
        public BitmapData BitmapData { get; set; }

        /// <summary>
        /// LockBitsSetting
        /// </summary>
        public struct LockBitsSetting
        {
            public Rectangle Rectangle;
            public ImageLockMode ImageLockMode;
            public PixelFormat PixelFormat;

            public int PixelFormatSize => Image.GetPixelFormatSize(PixelFormat);

            public LockBitsSetting(Rectangle rectangle, ImageLockMode imageLockMode, PixelFormat pixelFormat)
            {
                Rectangle = rectangle;
                ImageLockMode = imageLockMode;
                PixelFormat = pixelFormat;
            }

            public LockBitsSetting(int Width, int Height, ImageLockMode imageLockMode, PixelFormat pixelFormat)
            {
                Rectangle = new Rectangle(0, 0, Width, Height);
                ImageLockMode = imageLockMode;
                PixelFormat = pixelFormat;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Pos_X"></param>
            /// <param name="Pos_Y"></param>
            /// <param name="Width"></param>
            /// <param name="Height"></param>
            /// <param name="imageLockMode"></param>
            /// <param name="pixelFormat"></param>
            public LockBitsSetting(int Pos_X, int Pos_Y, int Width, int Height, ImageLockMode imageLockMode, PixelFormat pixelFormat)
            {
                Rectangle = new Rectangle(Pos_X, Pos_Y, Width, Height);
                ImageLockMode = imageLockMode;
                PixelFormat = pixelFormat;
            }
        }

        /// <summary>
        /// Lock Bitmap
        /// </summary>
        public void Lock()
        {
            BitmapData = Bitmap.LockBits(LockBitsSettings.Rectangle, LockBitsSettings.ImageLockMode, LockBitsSettings.PixelFormat);
        }

        /// <summary>
        /// Unlock Bitmap
        /// </summary>
        public void UnLock()
        {
            Bitmap.UnlockBits(BitmapData);
        }

        /// <summary>
        /// Get Scan0
        /// </summary>
        /// <returns>(IntPtr)</returns>
        public IntPtr GetScan0()
        {
            return BitmapData.Scan0;
        }

        /// <summary>
        /// BitmapData to byte[]
        /// </summary>
        public void BitmapToByteAry()
        {
            System.Runtime.InteropServices.Marshal.Copy(BitmapData.Scan0, ImgByteAry, 0, ImgByteAry.Length);
        }

        ///// <summary>
        ///// byte[] to BitmapData
        ///// </summary>
        //public void ByteAryToBitmap()
        //{
        //    System.Runtime.InteropServices.Marshal.Copy(ImgByteAry, 0, BitmapData.Scan0, ImgByteAry.Length);
        //}

        public byte[] ImgByteAry => new byte[Math.Abs(BitmapData.Stride) * BitmapData.Height];

        //public void GrayScale(int pixelCount, bool Reversed = false)
        //{
        //    for (int y = 0; y < BitmapData.Height; y++)
        //    {
        //        for (int x = 0; x < BitmapData.Width; x++)
        //        {
        //            //int pos = y * lockBitsImageTool.BitmapData.Stride + x * pixelCount;
        //            int pos = GetPosition(x, y, pixelCount);
        //            var array = GetByteArray(pos, pixelCount);

        //            byte gScale = MathTool.GrayScale(array, Reversed);

        //            if (pixelCount == 3)
        //            {
        //                array[2] = gScale;
        //                array[1] = gScale;
        //                array[0] = gScale;
        //            }
        //            else if (pixelCount == 4)
        //            {
        //                array[3] = 0xFF;
        //                array[2] = gScale;
        //                array[1] = gScale;
        //                array[0] = gScale;
        //            }

        //            SetByteArray(pos, pixelCount, array);
        //        }
        //    }
        //}


        public void GrayScale(int pixelCount, bool Reversed = false)
        {
            for (int y = 0; y < BitmapData.Height; y++)
            {
                for (int x = 0; x < BitmapData.Width; x++)
                {
                    int pos = GetPosition(x, y, pixelCount);
                    var array = GetByteArray(pos, pixelCount);

                    byte gScale = MathTool.GrayScale(array, Reversed);

                    if (pixelCount == 3)
                    {
                        array[2] = gScale;
                        array[1] = gScale;
                        array[0] = gScale;
                    }
                    else if (pixelCount == 4)
                    {
                        array[3] = gScale;
                        array[2] = gScale;
                        array[1] = gScale;
                        array[0] = gScale;
                    }

                    SetByteArray(pos, pixelCount, array);
                }
            }
        }

        public void RGBToRGBA(int pixelCount, byte value = 0xFF)
        {
            for (int y = 0; y < BitmapData.Height; y++)
            {
                for (int x = 0; x < BitmapData.Width; x++)
                {
                    int pos = GetPosition(x, y, pixelCount);
                    var array = GetByteArray(pos, pixelCount);

                    byte[] NewArray;
                    if (pixelCount == 3)
                    {
                        byte[] a = new byte[] { value };
                        NewArray = a.Concat(array).ToArray();
                    }
                    else
                    {
                        NewArray = array;
                    }

                    SetByteArray(pos, pixelCount, NewArray);
                }
            }
        }



        public void ToAlpha(int pixelCount, bool Reversed = false)
        {
            for (int y = 0; y < BitmapData.Height; y++)
            {
                for (int x = 0; x < BitmapData.Width; x++)
                {

                    int pos = GetPosition(x, y, pixelCount);
                    var array = GetByteArray(pos, pixelCount);

                    float[] HSVValue = MathTool.RGBToHSV(array);

                    byte[] RGBArray = MathTool.HSVToRGB(0, 0, HSVValue[2]);

                    //byte a = (byte)Math.Ceiling(2.55f * HSVValue[2]);

                    //byte gScale = MathTool.GrayScale(array, Reversed);

                    if (pixelCount == 4)
                    {
                        //array[3] = (byte)(int)(HSVValue[2] * 255);
                        //array[2] = array[2];
                        //array[1] = array[1];
                        //array[0] = array[0];

                        //array[3] = (byte)Math.Ceiling((10 * HSVValue[2]) * 0.255); //0xFF
                        array[3] = (byte)~(0xFF - RGBArray[2]);
                        array[2] = RGBArray[2];
                        array[1] = RGBArray[1];
                        array[0] = RGBArray[0];

                        SetByteArray(pos, pixelCount, array);
                    }
                    else if (pixelCount != 4)
                    {
                        throw new Exception("PixelCount : " + pixelCount + ", Not RGBA");
                    }

                    //SetByteArray(pos, pixelCount, array);
                }
            }
        }


        //public void GrayScale(int pixelCount, bool UseAlpha = true, bool RGBToRGBA = false, bool Reversed = false)
        //{
        //    //if (RGBToRGBA == true)
        //    //{
        //    //    //3 * 3 = 9, 4 * 3 = 12 => 
        //    //}

        //    for (int y = 0; y < BitmapData.Height; y++)
        //    {
        //        for (int x = 0; x < BitmapData.Width; x++)
        //        {
        //            //int pos = y * lockBitsImageTool.BitmapData.Stride + x * pixelCount;
        //            int pos = GetPosition(x, y, pixelCount);
        //            var array = GetByteArray(pos, pixelCount);

        //            byte gScale = MathTool.GrayScale(array, Reversed);

        //            if (pixelCount == 3)
        //            {
        //                if (RGBToRGBA == true)
        //                {
        //                    if (UseAlpha == true)
        //                    {
        //                        byte[] a = new byte[] { 0x00 };

        //                        a.Concat(array).ToArray();

        //                        //array[3] = 0x00;
        //                    }
        //                    else
        //                    {
        //                        byte[] a = new byte[] { 0xFF };

        //                        a.Concat(array).ToArray();

        //                        //array[3] = 0xFF;
        //                    }
        //                }

        //                array[2] = gScale;
        //                array[1] = gScale;
        //                array[0] = gScale;
        //            }
        //            else if (pixelCount == 4)
        //            {

        //                //if (UseAlpha == true)
        //                //{
        //                //    array[3] = 0x00;
        //                //}
        //                //else
        //                //{
        //                //    array[3] = 0xFF;
        //                //}

        //                array[3] = gScale; //0xFF
        //                array[2] = gScale;
        //                array[1] = gScale;
        //                array[0] = gScale;
        //            }

        //            SetByteArray(pos, pixelCount, array);
        //        }
        //    }
        //}

        public void Luminance(int pixelCount, int Factor = 100)
        {
            for (int y = 0; y < BitmapData.Height; y++)
            {
                for (int x = 0; x < BitmapData.Width; x++)
                {
                    //int pos = y * lockBitsImageTool.BitmapData.Stride + x * pixelCount;
                    int pos = GetPosition(x, y, pixelCount);

                    var array = GetByteArray(pos, pixelCount);

                    if (pixelCount == 3)
                    {
                        array[2] = MathTool.Luminance(array[2], Factor);
                        array[1] = MathTool.Luminance(array[1], Factor);
                        array[0] = MathTool.Luminance(array[0], Factor);
                    }
                    else if (pixelCount == 4)
                    {
                        array[3] = array[3];
                        array[2] = MathTool.Luminance(array[2], Factor);
                        array[1] = MathTool.Luminance(array[1], Factor);
                        array[0] = MathTool.Luminance(array[0], Factor);
                    }

                    SetByteArray(pos, pixelCount, array);
                }
            }
        }

        public int GetPosition(int x, int y, int Stride, int pixelCount)
        {
            return y * Stride + x * pixelCount;
        }

        public int GetPosition(int x, int y, int pixelCount)
        {
            return y * BitmapData.Stride + x * pixelCount;
        }

        public byte GetByte(int pos, int Count)
        {
            return System.Runtime.InteropServices.Marshal.ReadByte(GetScan0(), pos + Count);
        }

        public void SetByte(int pos, int Count, byte value)
        {
            System.Runtime.InteropServices.Marshal.WriteByte(GetScan0(), pos + Count, value);
        }

        public byte[] GetByteArray(int pos, int Length)
        {
            List<byte> bytes = new List<byte>();

            for (int count = 0; count < Length; count++)
            {
                bytes.Add(System.Runtime.InteropServices.Marshal.ReadByte(GetScan0(), pos + count));
            }

            return bytes.ToArray();
        }

        public void SetByteArray(int pos, int Length, byte[] values)
        {
            for (int count = 0; count < Length; count++)
            {
                System.Runtime.InteropServices.Marshal.WriteByte(GetScan0(), pos + count, values[count]);
            }
        }

        protected internal Color ToColor(byte Color_A, byte Color_R, byte Color_G, byte Color_B)
        {
            return Color.FromArgb(Color_A, Color_R, Color_G, Color_B);
        }

        protected internal Color ToColor(byte[] bytes)
        {
            return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        //protected internal Color ToColor(byte[] bytes, )
        //{
        //    return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
        //}

        //public int GetPosition(int x, int y, int ByteCount = 8)
        //{
        //    return y * BitmapData.Stride + x * (LockBitsSettings.PixelFormatSize / ByteCount);
        //}

        //public byte[] ImgByteAry
        //{
        //    get
        //    {
        //        return CreateNewImageByteArray();
        //    }
        //    set
        //    {
        //        ImgByteAry = value;
        //    }
        //}

        //public byte[] CreateNewImageByteArray()
        //{
        //    return new byte[Math.Abs(BitmapData.Stride) * BitmapData.Height];
        //}

        public LockBitsImageTool(Bitmap bitmap)
        {
            FilePath = "";
            Bitmap = bitmap;
        }

        public LockBitsImageTool(string path)
        {
            FilePath = path;
            Bitmap = new Bitmap(path);
        }

        public LockBitsImageTool(int Width, int Height)
        {
            FilePath = "";
            Bitmap = new Bitmap(Width, Height);
        }

        public LockBitsImageTool(int Width, int Height, Graphics graphics)
        {
            FilePath = "";
            Bitmap = new Bitmap(Width, Height, graphics);
        }

        public LockBitsImageTool(int Width, int Height, PixelFormat pixelFormat)
        {
            FilePath = "";
            Bitmap = new Bitmap(Width, Height, pixelFormat);
        }

        public LockBitsImageTool(int Width, int Height, int stride, PixelFormat pixelFormat, IntPtr Scan0)
        {
            FilePath = "";
            Bitmap = new Bitmap(Width, Height, stride, pixelFormat, Scan0);
        }
    }

    public class MathTool
    {
        public static byte[] RGBToRGBA(byte[] array, byte value = 0xFF)
        {
            byte[] NewArray;
            if (array.Length == 3)
            {
                byte[] a = new byte[] { value };
                NewArray = a.Concat(array).ToArray();
            }
            else
            {
                NewArray = array;
            }

            return NewArray;
        }

        public static byte GrayScale(byte[] array, bool IsReverse)
        {
            byte gScaleValue = new byte();
            if (IsReverse == true)
            {
                gScaleValue = (byte)~((array[2] + array[1] + array[0]) / 3);
            }
            else if (IsReverse == false)
            {
                gScaleValue = (byte)((array[2] + array[1] + array[0]) / 3);
            }

            return gScaleValue;
        }

        public static byte Luminance(byte b, int LuminanceFactor, int Unknown = 255)
        {
            return (byte)Math.Min(b + LuminanceFactor, Unknown);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">RGB Array</param>
        /// <returns>HSV : [0] => Hue, [1] => Saturation, [2] => Brightness</returns>
        public static float[] RGBToHSV(byte[] array)
        {
            float r = (float)Math.Round((array[0] / 255F), 2, MidpointRounding.AwayFromZero);
            float g = (float)Math.Round((array[1] / 255F), 2, MidpointRounding.AwayFromZero);
            float b = (float)Math.Round((array[2] / 255F), 2, MidpointRounding.AwayFromZero);

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));

            float brightness = max;

            float hue, saturation;

            if (max == min)
            {
                hue = 0;
                saturation = 0;
            }
            else
            {
                float c = max - min;
                if (max == r) hue = (g - b) / c;
                else if (max == g) hue = (b - r) / c + 2f;
                else hue = (r - g) / c + 4f;

                hue *= 60f;
                if (hue < 0f) hue += 360f;

                saturation = c / max;
            }

            return new float[] { hue, saturation, brightness };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hue">Hue Value</param>
        /// <param name="Saturation">Saturation Value</param>
        /// <param name="Brightness">Brightness (v) Value</param>
        /// <returns>RGB : [0] => R, [1] => G, [2] => B</returns>
        /// <exception cref="Exception">HSV Error</exception>
        public static byte[] HSVToRGB(float Hue, float Saturation, float Brightness)
        {
            float r, g, b;
            if (Saturation == 0)
            {
                r = Brightness;
                g = Brightness;
                b = Brightness;
            }
            else
            {
                float h = Hue / 60f;
                int i = (int)Math.Floor(h);
                float f = h - i;
                float p = Brightness * (1f - Saturation);

                float q;
                if (i % 2 == 0) q = Brightness * (1f - (1f - f) * Saturation);
                else q = Brightness * (1f - f * Saturation);

                if (i == 0)
                {
                    r = Brightness;
                    g = q;
                    b = p;
                }
                else if (i == 1)
                {
                    r = q;
                    g = Brightness;
                    b = p;
                }
                else if (i == 2)
                {
                    r = p;
                    g = Brightness;
                    b = q;
                }
                else if (i == 3)
                {
                    r = p;
                    g = q;
                    b = Brightness;
                }
                else if (i == 4)
                {
                    r = q;
                    g = p;
                    b = Brightness;
                }
                else if (i == 5)
                {
                    r = Brightness;
                    g = p;
                    b = q;
                }
                else
                {
                    throw new Exception("不正な色相 : HSV");
                }
            }

            return new byte[] { (byte)(int)(r * 255), (byte)(int)(g * 255), (byte)(int)(b * 255) };
        }
    }
}
