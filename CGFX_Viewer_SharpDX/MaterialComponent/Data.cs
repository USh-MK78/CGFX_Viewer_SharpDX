using Assimp;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockBitImageTools;
using CGFXLibrary;
using System.Drawing.Imaging;

namespace CGFX_Viewer_SharpDX.Component
{
    public class Data
    {
        public class Texture
        {
            public enum TextureType
            {
                Diffuse = 1,
                Normal = 2,
                Specular = 3
            }

            //public enum From
            //{
            //    Path = 0,
            //    Stream = 1,
            //    Bitmap = 2
            //}

            public enum AlphaMap
            {
                /// <summary>[MODE : ZERO] TextureModelのサイズと同じアルファマップを自動生成します。全てのピクセルデータは(A:0x00, R:0x00, G:0x00, B:0x00)に固定されます</summary>
                ZERO = 0,

                /// <summary>[MODE : ONE] TextureModelのサイズと同じアルファマップを自動生成します。全てのピクセルデータは(A:0xFF, R:0xFF, G:0xFF, B:0xFF)に固定されます</summary>
                ONE = 1,

                /// <summary>[MODE : FROM_TEXTURE] TextureModelからアルファマップを自動生成します。</summary>
                FROM_TEXTURE = 2,

                /// <summary>[MODE : CUSTOM] 既に存在するアルファマップを使用します</summary>
                CUSTOM = 3,
            }

            public struct TextureModelSlot
            {
                public int TextureNumber { get; set; }
                public string TextureName { get; set; }
                public TextureModel TextureModel { get; set; }
                //public From From { get; set; }
                public TextureType TextureType { get; set; }


                public AlphaMap GenTypeAlphaMap { get; set; }
                public bool IsReversed { get; set; }
                public TextureModel AlphaMapSlot { get; set; }

                //private TextureModel _AlphaMapSlot;
                //public TextureModel AlphaMapSlot
                //{
                //    get
                //    {
                //        return GenerateAlphaMap(GenTypeAlphaMap);
                //    }
                //    set
                //    {
                //        _AlphaMapSlot = value;
                //    }
                //}

                public enum From
                {
                    Diffuse,
                    Alpha
                }

                public System.Drawing.Bitmap GetBitmap(From from)
                {
                    Stream stream = null;
                    if (from == From.Diffuse)
                    {
                        var guid = TextureModel.Guid;
                        stream = TextureModel.TextureInfoLoader.Load(guid).Texture;
                    }
                    else if (from == From.Diffuse)
                    {
                        var guid = AlphaMapSlot.Guid;
                        stream = AlphaMapSlot.TextureInfoLoader.Load(guid).Texture;
                    }

                    return new System.Drawing.Bitmap(stream);
                }

                //public System.Drawing.Bitmap FromTextureModel(From from)
                //{
                //    System.Drawing.Bitmap bitmap = null;

                //    if (from == From.Path)
                //    {
                //        var d = TextureModel.TextureInfoLoader;
                //        var n = d.GetType().GetProperty("FilePath").GetValue(d);
                //        bitmap = new System.Drawing.Bitmap((string)n);
                //    }
                //    else if (from == From.Stream)
                //    {
                //        var d = TextureModel.TextureInfoLoader;

                //    }


                //    return bitmap;
                //}

                //public void GetRepo()
                //{
                //    var guid = TextureModel.Guid;
                //    var df = (TextureModel)this.TextureModel.TextureModelRepository;

                //}

                public TextureModel GenerateAlphaMap(AlphaMap AlphaMap)
                {
                    TextureModel textureModel = null;

                    if (AlphaMap == AlphaMap.ZERO)
                    {
                        var guid = TextureModel.Guid;
                        var df = TextureModel.TextureInfoLoader.Load(guid).Texture;
                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(df);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

                        graphics.FillRectangle(System.Drawing.Brushes.Black, graphics.VisibleClipBounds);
                        graphics.Dispose();

                        MemoryStream memoryStream = new MemoryStream();
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                        textureModel = new TextureModel(memoryStream);

                        //memoryStream.Close();
                    }
                    else if (AlphaMap == AlphaMap.ONE)
                    {
                        var guid = TextureModel.Guid;
                        var df = TextureModel.TextureInfoLoader.Load(guid).Texture;

                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(df);
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

                        graphics.FillRectangle(System.Drawing.Brushes.White, graphics.VisibleClipBounds);
                        graphics.Dispose();

                        MemoryStream memoryStream = new MemoryStream();
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                        textureModel = new TextureModel(memoryStream);

                        //memoryStream.Close();
                    }
                    else if (AlphaMap == AlphaMap.FROM_TEXTURE)
                    {
                        var guid = TextureModel.Guid;
                        var df = TextureModel.TextureInfoLoader.Load(guid).Texture;

                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(df);

                        LockBitsImageTool lockBitsImageTool = new LockBitsImageTool(bitmap);
                        lockBitsImageTool.LockBitsSettings = new LockBitsImageTool.LockBitsSetting(lockBitsImageTool.Bitmap.Width, lockBitsImageTool.Bitmap.Height, System.Drawing.Imaging.ImageLockMode.ReadWrite, lockBitsImageTool.Bitmap.PixelFormat);
                        lockBitsImageTool.Lock();

                        int pixelCount = lockBitsImageTool.LockBitsSettings.PixelFormatSize / 8;

                        try
                        {
                            lockBitsImageTool.BitmapToByteAry();

                            //lockBitsImageTool.GrayScale(pixelCount, true, true, IsReversed);

                            lockBitsImageTool.GrayScale(pixelCount, IsReversed);
                            lockBitsImageTool.ToAlpha(pixelCount);

                            #region OLD
                            //for (int y = 0; y < lockBitsImageTool.BitmapData.Height; y++)
                            //{
                            //    for (int x = 0; x < lockBitsImageTool.BitmapData.Width; x++)
                            //    {
                            //        //int pos = y * lockBitsImageTool.BitmapData.Stride + x * pixelCount;
                            //        int pos = lockBitsImageTool.GetPosition(x, y, pixelCount);

                            //        #region Memo
                            //        //var B = System.Runtime.InteropServices.Marshal.ReadByte(lockBitsImageTool.GetScan0(), pos + 0);
                            //        //var G = System.Runtime.InteropServices.Marshal.ReadByte(lockBitsImageTool.GetScan0(), pos + 1);
                            //        //var R = System.Runtime.InteropServices.Marshal.ReadByte(lockBitsImageTool.GetScan0(), pos + 2);
                            //        //var A = System.Runtime.InteropServices.Marshal.ReadByte(lockBitsImageTool.GetScan0(), pos + 3);
                            //        #endregion

                            //        var array = lockBitsImageTool.GetByteArray(pos, pixelCount);

                            //        //Reverse
                            //        //var gScale = (byte)~((array[2] + array[1] + array[0]) / 3);

                            //        if (pixelCount == 3)
                            //        {
                            //            var gScale = (byte)((array[2] + array[1] + array[0]) / 3);

                            //            array[2] = gScale;
                            //            array[1] = gScale;
                            //            array[0] = gScale;
                            //        }
                            //        else if (pixelCount == 4)
                            //        {
                            //            //array[3] = array[3];
                            //            //if (array[3] == 0x00)
                            //            //{

                            //            //}

                            //            //Math math = new Math().

                            //            array[3] = 0xFF;

                            //            var gScale = (byte)((array[2] + array[1] + array[0]) / 3);

                            //            array[2] = gScale;
                            //            array[1] = gScale;
                            //            array[0] = gScale;
                            //        }

                            //        lockBitsImageTool.SetByteArray(pos, pixelCount, array);
                            //    }
                            //}
                            #endregion

                        }
                        finally
                        {
                            lockBitsImageTool.UnLock();
                        }

                        //Bitmap to Stream
                        MemoryStream memoryStream = new MemoryStream();
                        lockBitsImageTool.Bitmap.Save(memoryStream, ImageFormat.Png);

                        textureModel = new TextureModel(memoryStream);

                        //memoryStream.Close();
                    }
                    else if (AlphaMap == AlphaMap.CUSTOM)
                    {
                        textureModel = AlphaMapSlot;
                    }

                    return textureModel;
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="TextureNumber"></param>
                /// <param name="TextureName"></param>
                /// <param name="Texture"></param>
                /// <param name="textureType"></param>
                /// <param name="alphaMapType"></param>
                /// <param name="IsReversed"></param>
                /// <param name="CustomAlphaMap"></param>
                public TextureModelSlot(int TextureNumber, string TextureName, TextureModel Texture, TextureType textureType, AlphaMap alphaMapType = AlphaMap.ONE, bool IsReversed = true, TextureModel CustomAlphaMap = null)
                {
                    this.TextureNumber = TextureNumber;
                    this.TextureName = TextureName;
                    this.TextureModel = Texture;
                    this.TextureType = textureType;
                    this.IsReversed = IsReversed;

                    //if (alphaMapType == AlphaMap.ZERO || alphaMapType == AlphaMap.ONE || alphaMapType == AlphaMap.FROM_TEXTURE)
                    //{

                    //    AlphaMapSlot = GenerateAlphaMap(alphaMapType);
                    //}
                    //else if (alphaMapType == AlphaMap.CUSTOM)
                    //{
                    //    AlphaMapSlot = CustomAlphaMap;
                    //}

                    AlphaMapSlot = GenerateAlphaMap(alphaMapType);
                }
            }

            public void Test()
            {
                TextureModelSlot textureModelSlot = new TextureModelSlot(0, "Tex0", new TextureModel("Tex_0.png"), TextureType.Diffuse, AlphaMap.FROM_TEXTURE);
                textureModelSlot.GenTypeAlphaMap = AlphaMap.CUSTOM;

                textureModelSlot.AlphaMapSlot = new TextureModel("");
            }
        }




        public class ColorProperty
        {
            public struct Base
            {
                public Color4 AmbientColor { get; set; }
                public Color4 DiffuseColor { get; set; }
                public Color4 EmissiveColor { get; set; }
                public Color4 ReflectiveColor { get; set; }
                public Color4 SpecularColor { get; set; }

                ///// <summary>
                ///// Create Default()
                ///// </summary>
                ///// <returns></returns>
                //public static Base Default()
                //{
                //    return new Base(Color.DarkGray, Color.White, Color.Black, Color.Black, Color.Gray);
                //}

                /// <summary>
                /// Create Default()
                /// </summary>
                /// <returns></returns>
                public static Base Default()
                {
                    return new Base(Color.DarkGray, Color.White, Color.Black, Color.Black, Color.Black);
                }

                /// <summary>
                /// Initialize Base (Color)
                /// </summary>
                /// <param name="Ambient">Ambient Color</param>
                /// <param name="Diffuse">Diffuse Color</param>
                /// <param name="Emissive">Emissive Color</param>
                /// <param name="Reflect">Reflect Color</param>
                /// <param name="Specular">Speculer Color</param>
                public Base(Color Ambient, Color Diffuse, Color Emissive, Color Reflect, Color Specular)
                {
                    AmbientColor = Ambient;
                    DiffuseColor = Diffuse;
                    EmissiveColor = Emissive;
                    ReflectiveColor = Reflect;
                    SpecularColor = Specular;
                }

                /// <summary>
                /// Initialize Base (Color4)
                /// </summary>
                /// <param name="Ambient">Ambient Color</param>
                /// <param name="Diffuse">Diffuse Color</param>
                /// <param name="Emissive">Emissive Color</param>
                /// <param name="Reflect">Reflect Color</param>
                /// <param name="Specular">Speculer Color</param>
                public Base(Color4 Ambient, Color4 Diffuse, Color4 Emissive, Color4 Reflect, Color4 Specular)
                {
                    AmbientColor = Ambient;
                    DiffuseColor = Diffuse;
                    EmissiveColor = Emissive;
                    ReflectiveColor = Reflect;
                    SpecularColor = Specular;
                }
            }

            public struct ConstantColor
            {
                public Color4 ConstantColor0 { get; set; }
                public Color4 ConstantColor1 { get; set; }
                public Color4 ConstantColor2 { get; set; }
                public Color4 ConstantColor3 { get; set; }
                public Color4 ConstantColor4 { get; set; }
                public Color4 ConstantColor5 { get; set; }

                /// <summary>
                /// 
                /// </summary>
                /// <returns>DefaultValue (ConstantColor)</returns>
                public static ConstantColor Default()
                {
                    return new ConstantColor(Color4.Black, Color4.Black, Color4.Black, Color4.Black, Color4.Black, Color4.Black);
                }

                /// <summary>
                /// ConstantColor
                /// </summary>
                /// <param name="c0">Slot0</param>
                /// <param name="c1">Slot1</param>
                /// <param name="c2">Slot2</param>
                /// <param name="c3">Slot3</param>
                /// <param name="c4">Slot4</param>
                /// <param name="c5">Slot5</param>
                public ConstantColor(Color4 c0, Color4 c1, Color4 c2, Color4 c3, Color4 c4, Color4 c5)
                {
                    ConstantColor0 = c0;
                    ConstantColor1 = c1;
                    ConstantColor2 = c2;
                    ConstantColor3 = c3;
                    ConstantColor4 = c4;
                    ConstantColor5 = c5;
                }
            }
        }

        public class Blending
        {
            //public class MODE
            //{
            //    //COLOR_BLEND = BLENDMODE
            //    //ALPHA_BLEND = BLENDMODE

            //    //[BLENDMODE]
            //    //[SRC]+[DEST]
            //    //[SRC]-[DEST]
            //    //[DEST]-[SRC]
            //    //MIN([SRC], [DEST])
            //    //MAX([SRC], [DEST])

            //    //[SRC]
            //    //Output Conbiner * [MODE]

            //    //[DEST]
            //    //Frame Buffer * [MODE]


            //    public enum BLENDMODE
            //    {
            //        /// <summary>[BLEND]通常ブレンドを使用します</summary>
            //        BLEND = 1,

            //        /// <summary>[SEPARATE]セパレートブレンドを使用します</summary>
            //        SEPARATE = 2,

            //        /// <summary>[LOGICAL]論理演算 (ブレンド) を使用します</summary>
            //        LOGICAL = 3,

            //        /// <summary>[NONE]ブレンドを使用しません</summary>
            //        NONE = 4
            //    }

            //    //public BLENDMODE BLEND_MODE { get; set; }
            //    public bool IsLogical => UseLogical();

            //    public bool UseLogical()
            //    {
            //        bool b = false;
            //        if (BLEND_MODE == BLENDMODE.LOGICAL) b = true;
            //        //else b = false;
            //        return b;
            //    }

            //    public BLENDMODE BLEND_MODE;
            //    public BlendEquation? BlendEquation;
            //    public LogicalBlendEquation? LogicalBlendEquation;

            //    /// <summary>
            //    /// 
            //    /// </summary>
            //    /// <example>
            //    /// <code>MODE(BLENDMODE.BLEND, new BlendEquation(), null)</code>
            //    /// <code>MODE(BLENDMODE.LOGICAL, null, new LogicalBlendEquation())</code>
            //    /// <code>MODE(BLENDMODE.NONE)</code>
            //    /// <code>MODE(BLENDMODE.NONE, null, null)</code>
            //    /// </example>
            //    /// <param name="BLEND_MODE"></param>
            //    /// <param name="blendEquation"></param>
            //    /// <param name="logicalBlendEquation"></param>
            //    public MODE(BLENDMODE BLEND_MODE, BlendEquation? blendEquation = null, LogicalBlendEquation? logicalBlendEquation = null)
            //    {
            //        if (BLEND_MODE == BLENDMODE.BLEND && BLEND_MODE == BLENDMODE.SEPARATE)
            //        {
            //            BlendEquation = blendEquation != null ? blendEquation : new Blending.BlendEquation();

            //            //BlendEquation = new BlendEquation();
            //        }
            //        else if (BLEND_MODE == BLENDMODE.LOGICAL)
            //        {
            //            LogicalBlendEquation = logicalBlendEquation != null ? logicalBlendEquation : new LogicalBlendEquation();

            //            //LOGICALMODE
            //        }
            //        else if (BLEND_MODE == BLENDMODE.NONE)
            //        {
            //            BlendEquation = null;
            //            LogicalBlendEquation = null;
            //        }
            //    }
            //}


            //COLOR_BLEND = BLENDMODE
            //ALPHA_BLEND = BLENDMODE

            //[BLENDMODE]
            //[SRC]+[DEST]
            //[SRC]-[DEST]
            //[DEST]-[SRC]
            //MIN([SRC], [DEST])
            //MAX([SRC], [DEST])

            //[SRC]
            //Output Conbiner * [MODE]

            //[DEST]
            //Frame Buffer * [MODE]


            #region DEL
            //public enum BLENDMODE
            //{
            //    BLEND = 1,
            //    SEPARATE = 2,
            //    LOGICAL = 3, //
            //    NONE = 4
            //}

            //public BLENDMODE BLEND_MODE { get; set; }
            //public bool IsLogical => UseLogical();

            //public bool UseLogical()
            //{
            //    bool b = false;
            //    if (BLEND_MODE == BLENDMODE.LOGICAL) b = true;
            //    //else b = false;
            //    return b;
            //}
            #endregion

            #region LogicalBlendEquation
            public enum BLEND_ELEMENT_LOGICAL
            {
                SET_ALL_BIT_ZERO = 0,
                SET_ALL_BIT_ONE = 1,
                COMBINER_OUT = 2,
                FRAMEBUFFER = 3,
                NOT_COMBINER_OUT = 4,
                NOT_FRAMEBUFFER_OUT = 5,
                COMBINER_OUT_AND_FRAMEBUFFER = 6,
                COMBINER_OUT_OR_FRAMEBUFFER = 7,
                COMBINER_OUT_XOR_FRAMEBUFFER = 8,
                COMBINER_OUT_AND_NOTFRAMEBUFFER = 9, //Conbiner Out AND (NOT FrameBuffer)
                COMBINER_OUT_OR_NOTFRAMEBUFFER = 10, //Conbiner Out OR (NOT FrameBuffer)
                NOTCOMBINER_OUT_AND_FRAMEBUFFER = 11,
                NOTCOMBINER_OUT_OR_FRAMEBUFFER = 12,
                NOT_COMBINER_OUT_AND_FRAMEBUFFER = 13,
                NOT_COMBINER_OUT_OR_FRAMEBUFFER = 14,
                NOT_COMBINER_OUT_XOR_FRAMEBUFFER = 15
            }

            public struct LogicalBlendEquation
            {
                public BLEND_ELEMENT_LOGICAL BLEND_ELEMENT_LOGICAL;

                public LogicalBlendEquation(BLEND_ELEMENT_LOGICAL bLEND_ELEMENT_LOGICAL)
                {
                    BLEND_ELEMENT_LOGICAL = bLEND_ELEMENT_LOGICAL;
                }
            }
            #endregion

            #region BlendEquation
            public enum BLEND_ELEMENT
            {
                ZERO = 0,
                ONE = 1,
                COMBINER_OUT_RGB = 2,
                ONE_SUB_COMBINER_RGB = 3,
                FRAMEBUFFER_OUT_RGB = 4,
                ONE_SUB_FRAMEBUFFER_RGB = 5,
                COMBINER_OUT_A = 6,
                ONE_SUB_COMBINER_A = 7,
                FRAMEBUFFER_OUT_A = 8,
                ONE_SUB_FRAMEBUFFER_A = 9,
                BLEND_RGB = 10,
                ONE_SUB_BLEND_RGB = 11,
                BLEND_A = 12,
                ONE_SUB_BLEND_A = 13,
                MIN_BRKT_COMBINER_OUT_A_ONE_SUB_FRAMEBUFFER_A_BRKT = 14
            }

            public struct ColorBlendMode
            {
                /// <summary>
                /// Output Conbiner * [MODE]
                /// </summary>
                public int SRC;

                /// <summary>
                /// Frame Buffer * [MODE]
                /// </summary>
                public int DEST;

                public BLEND_ELEMENT Get_SRC => Get_BLEND_ELEMENT(SRC);
                public BLEND_ELEMENT Get_DEST => Get_BLEND_ELEMENT(DEST);

                public BLEND_ELEMENT Get_BLEND_ELEMENT(int val)
                {
                    return (BLEND_ELEMENT)Enum.ToObject(typeof(BLEND_ELEMENT), val);
                }

                public ColorBlendMode(BLEND_ELEMENT COLOR_BLEND, BLEND_ELEMENT ALPHA_BLEND)
                {
                    SRC = (int)COLOR_BLEND;
                    DEST = (int)ALPHA_BLEND;
                }

                public ColorBlendMode(int InSRC, int InDEST)
                {
                    SRC = InSRC;
                    DEST = InDEST;
                }
            }

            public struct AlphaBlendMode
            {
                /// <summary>
                /// Output Conbiner * [MODE]
                /// </summary>
                public int SRC;

                /// <summary>
                /// Frame Buffer * [MODE]
                /// </summary>
                public int DEST;

                public AlphaBlendMode(BLEND_ELEMENT COLOR_BLEND, BLEND_ELEMENT ALPHA_BLEND)
                {
                    SRC = (int)COLOR_BLEND;
                    DEST = (int)ALPHA_BLEND;
                }

                public AlphaBlendMode(int InSRC, int InDEST)
                {
                    SRC = InSRC;
                    DEST = InDEST;
                }
            }

            public enum BlendEquationType
            {
                /// <summary>MODE = [SRC]+[DEST]</summary>
                SRC_ADD_DEST = 0,

                /// <summary>MODE = [SRC]-[DEST]</summary>
                SRC_SUB_DEST = 1,

                /// <summary>MODE = [DEST]-[SRC]</summary>
                DEST_SUB_SRC = 2,

                /// <summary>MODE = MIN([SRC], [DEST])</summary>
                MIN_SRC_DEST = 3,

                /// <summary>MODE = MAX([SRC], [DEST])</summary>
                MAX_SRC_DEST = 4
            }

            /// <summary>
            /// 
            /// </summary>
            public struct BlendEquation
            {
                public ColorBlendMode ColorBlendMode;
                public BlendEquationType ColorBlendEquationType;
                public AlphaBlendMode AlphaBlendMode;
                public BlendEquationType AlphaBlendEquationType;

                /// <summary>
                /// 
                /// </summary>
                /// <example>
                /// <code>BlendEquation(new ColorBlendMode(1, 3), BlendEquationType.DEST_SUB_SRC, new AlphaBlendMode(4, 2), BlendEquationType.DEST_SUB_SRC)</code>
                /// </example>
                /// <param name="colorBlendMode"></param>
                /// <param name="ColorBlendEquationType"></param>
                /// <param name="alphaBlendMode"></param>
                /// <param name="AlphaBlendEquationType"></param>
                public BlendEquation(ColorBlendMode colorBlendMode, BlendEquationType ColorBlendEquationType, AlphaBlendMode alphaBlendMode, BlendEquationType AlphaBlendEquationType)
                {
                    ColorBlendMode = colorBlendMode;
                    this.ColorBlendEquationType = ColorBlendEquationType;
                    AlphaBlendMode = alphaBlendMode;
                    this.AlphaBlendEquationType = AlphaBlendEquationType;
                }
            }
            #endregion

            #region Blending
            public enum BLENDMODE
            {
                /// <summary>[BLEND]通常ブレンドを使用します</summary>
                BLEND = 1,

                /// <summary>[SEPARATE]セパレートブレンドを使用します</summary>
                SEPARATE = 2,

                /// <summary>[LOGICAL]論理演算 (ブレンド) を使用します</summary>
                LOGICAL = 3,

                /// <summary>[NONE]ブレンドを使用しません</summary>
                NONE = 4
            }

            //public BLENDMODE BLEND_MODE { get; set; }
            public bool IsLogical => UseLogical();

            public bool UseLogical()
            {
                bool b = false;
                if (BLEND_MODE == BLENDMODE.LOGICAL) b = true;
                //else b = false;
                return b;
            }

            public BLENDMODE BLEND_MODE;
            public BlendEquation? Blend_Equation;
            public LogicalBlendEquation? LogicalBlend_Equation;

            /// <summary>
            /// Blending Setting
            /// </summary>
            /// <example>
            /// <code>MODE(BLENDMODE.BLEND, new BlendEquation(), null)</code>
            /// <code>MODE(BLENDMODE.LOGICAL, null, new LogicalBlendEquation())</code>
            /// <code>MODE(BLENDMODE.NONE)</code>
            /// <code>MODE(BLENDMODE.NONE, null, null)</code>
            /// </example>
            /// <param name="BLEND_MODE">Select blend mode</param>
            /// <param name="blendEquation">Select blend equation</param>
            /// <param name="logicalBlendEquation"></param>
            public Blending(BLENDMODE BLEND_MODE, BlendEquation? blendEquation = null, LogicalBlendEquation? logicalBlendEquation = null)
            {
                if (BLEND_MODE == BLENDMODE.BLEND && BLEND_MODE == BLENDMODE.SEPARATE)
                {
                    Blend_Equation = blendEquation != null ? blendEquation : new Blending.BlendEquation();

                    //BlendEquation = new BlendEquation();
                }
                else if (BLEND_MODE == BLENDMODE.LOGICAL)
                {
                    LogicalBlend_Equation = logicalBlendEquation != null ? logicalBlendEquation : new LogicalBlendEquation();

                    //LOGICALMODE
                }
                else if (BLEND_MODE == BLENDMODE.NONE)
                {
                    Blend_Equation = null;
                    LogicalBlend_Equation = null;
                }
            }
            #endregion

            public void Test()
            {
                BlendEquation blendEquation = new BlendEquation(new ColorBlendMode(1, 3), BlendEquationType.DEST_SUB_SRC, new AlphaBlendMode(4, 2), BlendEquationType.DEST_SUB_SRC);
                LogicalBlendEquation logicalBlendEquation = new LogicalBlendEquation(BLEND_ELEMENT_LOGICAL.COMBINER_OUT_OR_FRAMEBUFFER);

                Blending blending0 = new Blending(BLENDMODE.BLEND, blendEquation, null);
                Blending blending1 = new Blending(BLENDMODE.SEPARATE, blendEquation, null);
                Blending blending2 = new Blending(BLENDMODE.LOGICAL, null, logicalBlendEquation);
                Blending blending3 = new Blending(BLENDMODE.NONE);
                Blending blending4 = new Blending(BLENDMODE.NONE, null, null);

                var IsLogical = blending3.IsLogical;

                //BlendEquation blendEquation = new BlendEquation(new ColorBlendMode(1, 3), BlendEquationType.DEST_SUB_SRC, new AlphaBlendMode(4, 2), BlendEquationType.DEST_SUB_SRC);
            }
        }

        public class TextureConbinerStage
        {
            public enum SOURCE
            {
                /// <summary>頂点シェーダーの出力結果</summary>
                VERTEX_SHADER_OUT = 0,

                /// <summary>プライマリカラー</summary>
                COLOR_PRIMARY = 1,

                /// <summary>セカンダリカラー</summary>
                COLOR_SECONDARY = 2,

                /// <summary>Texture 0</summary>
                TEXTURE0 = 3,

                /// <summary>Texture 1</summary>
                TEXTURE1 = 4,

                /// <summary>Texture 2</summary>
                TEXTURE2 = 5,

                /// <summary>Texture 3</summary>
                TEXTURE3 = 6,

                /// <summary>前段のバッファ (1段目には表示しない)</summary>
                PREVIOUS_STEP_BUFFER = 13,

                /// <summary>Constant Color</summary>
                CONSTANT_COLOR = 14,

                /// <summary>前段の出力結果 (1段目には表示しない)</summary>
                PREVIOUS_STEP_RESULT = 15
            }

            public enum COLOR_OPERAND
            {
                /// <summary>RGB</summary>
                RGB = 0,

                /// <summary>1 - RGB</summary>
                ONE_SUB_RGB = 1,

                /// <summary>A</summary>
                A = 2,

                /// <summary>1 - A</summary>
                ONE_SUB_A = 3,

                /// <summary>R</summary>
                R = 4,

                /// <summary>1 - R</summary>
                ONE_SUB_R = 5,

                /// <summary>G</summary>
                G = 6,

                /// <summary>1 - B</summary>
                ONE_SUB_G = 7,

                /// <summary>B</summary>
                B = 8,

                /// <summary>1 - G</summary>
                ONE_SUB_B = 9
            }

            public enum ALPHA_OPERAND
            {
                /// <summary>A</summary>
                A = 0,

                /// <summary>1 - A</summary>
                ONE_SUB_A = 1,

                /// <summary>R</summary>
                R = 2,

                /// <summary>1 - R</summary>
                ONE_SUB_R = 3,

                /// <summary>G</summary>
                G = 4,

                /// <summary>1 - B</summary>
                ONE_SUB_G = 5,

                /// <summary>B</summary>
                B = 6,

                /// <summary>1 - G</summary>
                ONE_SUB_B = 7
            }

            public enum OPERANDTYPE
            {
                Color = 0,
                Alpha = 1,
                //NOTSET = -1
            }

            public struct ConbinerSource
            {
                public OPERANDTYPE OperandType;
                public int SOURCE;
                public int OPERAND;

                public SOURCE GetSOURCE()
                {
                    return (SOURCE)Enum.ToObject(typeof(SOURCE), SOURCE);
                }

                /// <summary>
                /// 
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <returns></returns>
                public T GetOPERAND<T>()
                {
                    object obj = new object();
                    if (OperandType == OPERANDTYPE.Color)
                    {
                        obj = (COLOR_OPERAND)Enum.ToObject(typeof(COLOR_OPERAND), OPERAND);
                    }
                    else if (OperandType == OPERANDTYPE.Alpha)
                    {
                        obj = (ALPHA_OPERAND)Enum.ToObject(typeof(ALPHA_OPERAND), OPERAND);
                    }

                    return (T)obj;
                }

                public ConbinerSource(SOURCE SOURCE, COLOR_OPERAND OPERAND)
                {
                    this.OperandType = OPERANDTYPE.Color;
                    this.SOURCE = (int)SOURCE;
                    this.OPERAND = (int)OPERAND;
                }

                public ConbinerSource(SOURCE SOURCE, ALPHA_OPERAND OPERAND)
                {
                    this.OperandType = OPERANDTYPE.Alpha;
                    this.SOURCE = (int)SOURCE;
                    this.OPERAND = (int)OPERAND;
                }

                public ConbinerSource(int SOURCE, int OPERAND, OPERANDTYPE OperandType)
                {
                    this.OperandType = OperandType;
                    this.SOURCE = SOURCE;
                    this.OPERAND = OPERAND;
                }

                public ConbinerSource CombinerColorSource(int SOURCE, int OPERAND)
                {
                    return new ConbinerSource(SOURCE, OPERAND, OPERANDTYPE.Color);
                }

                public ConbinerSource CombinerAlphaSource(int SOURCE, int OPERAND)
                {
                    return new ConbinerSource(SOURCE, OPERAND, OPERANDTYPE.Alpha);
                }
            }

            public enum ConbinerEquation
            {
                /// <summary>SOURCE = A</summary>
                A = 0,

                /// <summary>SOURCE = A * B</summary>
                A_MULTIPLY_B = 1,

                /// <summary>SOURCE = A + B</summary>
                A_ADD_B = 2,

                /// <summary>SOURCE = A + B - 0.5</summary>
                A_ADD_B_SUB_NOUGHT_PT_FIVE = 3,

                /// <summary>SOURCE =  A * C + B * (1 - C)</summary>
                A_MULTIPLY_C_ADD_B_MULTIPLY_BRKT_ONE_SUB_C_BRKT = 4,

                /// <summary>SOURCE =  A - B</summary>
                A_SUB_B = 5,

                /// <summary>SOURCE =  RGB <= DOT(A, B)</summary>
                RGB_DOT_BRKT_A_B_BRKT = 6,

                /// <summary>SOURCE =  RGBB <= DOT(A, B)</summary>
                RGBA_DOT_BRKT_A_B_BRKT = 7,

                /// <summary>SOURCE =  (A + B) * C</summary>
                BRKT_A_ADD_B_BRKT_MULTIPLY_C = 8,

                /// <summary>SOURCE =  (A * B) + C</summary>
                BRKT_A_MULTIPLY_B_BRKT_ADD_C = 9
            }

            public struct Equation
            {
                public ConbinerSource SOURCE_A;
                public ConbinerSource SOURCE_B;
                public ConbinerSource SOURCE_C;

                public int ConbinerEquation;
                public int ScaleFactor;
                public bool IsBuffering;

                public ConbinerEquation GetConbinerEquation()
                {
                    return (ConbinerEquation)Enum.ToObject(typeof(ConbinerEquation), ConbinerEquation);
                }

                #region DefaultEquation
                public static Equation DefaultColorEquation()
                {
                    ConbinerSource SRC_A = new ConbinerSource(SOURCE.VERTEX_SHADER_OUT, COLOR_OPERAND.RGB);
                    ConbinerSource SRC_B = new ConbinerSource(SOURCE.TEXTURE0, COLOR_OPERAND.RGB);
                    ConbinerSource SRC_C = new ConbinerSource(SOURCE.CONSTANT_COLOR, COLOR_OPERAND.RGB);

                    return new Equation(SRC_A, SRC_B, SRC_C, TextureConbinerStage.ConbinerEquation.A_MULTIPLY_B, 1, false);
                }

                public static Equation DefaultAlphaEquation()
                {
                    ConbinerSource SRC_A = new ConbinerSource(SOURCE.VERTEX_SHADER_OUT, ALPHA_OPERAND.A);
                    ConbinerSource SRC_B = new ConbinerSource(SOURCE.TEXTURE0, ALPHA_OPERAND.A);
                    ConbinerSource SRC_C = new ConbinerSource(SOURCE.CONSTANT_COLOR, ALPHA_OPERAND.A);

                    return new Equation(SRC_A, SRC_B, SRC_C, TextureConbinerStage.ConbinerEquation.A_MULTIPLY_B, 1, false);
                }
                #endregion

                #region DefaultValue
                public static Equation DefaultValueColorEquation()
                {
                    ConbinerSource SRC_A = new ConbinerSource(SOURCE.PREVIOUS_STEP_RESULT, COLOR_OPERAND.RGB);
                    ConbinerSource SRC_B = new ConbinerSource(SOURCE.COLOR_PRIMARY, COLOR_OPERAND.RGB);
                    ConbinerSource SRC_C = new ConbinerSource(SOURCE.CONSTANT_COLOR, COLOR_OPERAND.RGB);

                    return new Equation(SRC_A, SRC_B, SRC_C, TextureConbinerStage.ConbinerEquation.A, 1, false);
                }

                public static Equation DefaultValueAlphaEquation()
                {
                    ConbinerSource SRC_A = new ConbinerSource(SOURCE.PREVIOUS_STEP_RESULT, ALPHA_OPERAND.A);
                    ConbinerSource SRC_B = new ConbinerSource(SOURCE.COLOR_PRIMARY, ALPHA_OPERAND.A);
                    ConbinerSource SRC_C = new ConbinerSource(SOURCE.CONSTANT_COLOR, ALPHA_OPERAND.A);

                    return new Equation(SRC_A, SRC_B, SRC_C, TextureConbinerStage.ConbinerEquation.A, 1, false);
                }
                #endregion

                /// <summary>
                /// Equation = 
                /// </summary>
                /// <param name="SRC_A">SOURCE A</param>
                /// <param name="SRC_B">SOURCE B</param>
                /// <param name="SRC_C">SOURCE C</param>
                /// <param name="ConbinerEquation"></param>
                /// <param name="ScaleFactor">ConbierEquation</param>
                public Equation(ConbinerSource SRC_A, ConbinerSource SRC_B, ConbinerSource SRC_C, ConbinerEquation ConbinerEquation, int ScaleFactor, bool IsBuffering = false)
                {
                    SOURCE_A = SRC_A;
                    SOURCE_B = SRC_B;
                    SOURCE_C = SRC_C;
                    this.ConbinerEquation = (int)ConbinerEquation;
                    this.ScaleFactor = ScaleFactor;
                    this.IsBuffering = IsBuffering;

                    //ConbinerEquation * ScaleFactor
                }

                public Equation(ConbinerSource SRC_A, ConbinerSource SRC_B, ConbinerSource SRC_C, int ConbinerEquation, int ScaleFactor, bool IsBuffering = false)
                {
                    SOURCE_A = SRC_A;
                    SOURCE_B = SRC_B;
                    SOURCE_C = SRC_C;
                    this.ConbinerEquation = ConbinerEquation;
                    this.ScaleFactor = ScaleFactor;
                    this.IsBuffering = IsBuffering;

                    //ConbinerEquation * ScaleFactor
                }

                public Equation(int SRC_A, int OPERAND_A, int SRC_B, int OPERAND_B, int SRC_C, int OPERAND_C, OPERANDTYPE OperandType, ConbinerEquation conbinerEquation, int Scale, bool IsBuffering = false)
                {
                    SOURCE_A = new ConbinerSource(SRC_A, OPERAND_A, OperandType);
                    SOURCE_B = new ConbinerSource(SRC_B, OPERAND_B, OperandType);
                    SOURCE_C = new ConbinerSource(SRC_C, OPERAND_C, OperandType);
                    ConbinerEquation = (int)conbinerEquation;
                    ScaleFactor = Scale;
                    this.IsBuffering = IsBuffering;
                }
            }

            public struct TextureConbinerEquation
            {
                public Equation ColorEquation;
                public Equation AlphaEquation;

                public TextureConbinerEquation(Equation ColorEquation, Equation AlphaEquation)
                {
                    this.ColorEquation = ColorEquation;
                    this.AlphaEquation = AlphaEquation;
                }

                public static TextureConbinerEquation DefaultTextureConbinerEquation()
                {
                    return new TextureConbinerEquation(Equation.DefaultValueColorEquation(), Equation.DefaultValueAlphaEquation());
                }
            }

            public void TexConbinerTest()
            {
                //Max : 6
                List<TextureConbinerEquation> texturConbinerEquations = new List<TextureConbinerEquation>();
                texturConbinerEquations.Add(new TextureConbinerEquation(new Equation(new ConbinerSource(1, 1, OPERANDTYPE.Color), new ConbinerSource(2, 1, OPERANDTYPE.Color), new ConbinerSource(4, 3, OPERANDTYPE.Color), ConbinerEquation.A_MULTIPLY_B, 1),
                                                                       new Equation(new ConbinerSource(1, 1, OPERANDTYPE.Alpha), new ConbinerSource(2, 1, OPERANDTYPE.Alpha), new ConbinerSource(4, 3, OPERANDTYPE.Alpha), ConbinerEquation.A_MULTIPLY_B, 1)));

                texturConbinerEquations.Add(new TextureConbinerEquation(new Equation(1, 1, 2, 1, 3, 4, OPERANDTYPE.Color, ConbinerEquation.A_ADD_B, 1), new Equation(3, 1, 2, 1, 1, 3, OPERANDTYPE.Alpha, ConbinerEquation.A_SUB_B, 2)));
            }
        }

        public class FragmentLighting
        {
            public enum LayerConfigType
            {
                TYPE_SPOT = 1,
                TYPE_DISTRIBUTION_0 = 2,
                TYPE_DISTRIBUTION_1 = 3,
                TYPE_REFLECTION_R = 4,
                TYPE_REFLECTION_G = 5,
                TYPE_REFLECTION_B = 6,
                TYPE_FRESNEL = 7
            }

            public LayerConfigType[] LayerConfigTypes { get; set; }


            public enum LayerConfig
            {
                Setting0 = 0,
                Setting1 = 1,
                Setting2 = 2,
                Setting3 = 3,
                Setting4 = 4,
                Setting5 = 5,
                Setting6 = 6,
                Setting7 = 7
            }

            public struct DFG
            {
                public LayerConfigType[] LayerConfigTypes;

                public LayerConfig ToLayerConfig()
                {
                    LayerConfig layerConfig = LayerConfig.Setting0;
                    if (LayerConfigTypes.SequenceEqual(new LayerConfigType[] { LayerConfigType.TYPE_SPOT, LayerConfigType.TYPE_DISTRIBUTION_0, LayerConfigType.TYPE_REFLECTION_R }))
                    {
                        layerConfig = LayerConfig.Setting0;
                    }
                    else if (LayerConfigTypes.SequenceEqual(new LayerConfigType[] { LayerConfigType.TYPE_SPOT, LayerConfigType.TYPE_REFLECTION_R, LayerConfigType.TYPE_FRESNEL }))
                    {
                        layerConfig = LayerConfig.Setting1;
                    }
                    else if (LayerConfigTypes.SequenceEqual(new LayerConfigType[] { LayerConfigType.TYPE_DISTRIBUTION_0, LayerConfigType.TYPE_DISTRIBUTION_1, LayerConfigType.TYPE_REFLECTION_R }))
                    {
                        layerConfig = LayerConfig.Setting2;
                    }
                    //if (LayerConfigTypes.SequenceEqual(new LayerConfigType[] { LayerConfigType.TYPE_SPOT, LayerConfigType.TYPE_DISTRIBUTION_0, LayerConfigType.TYPE_REFLECTION_R }))
                    //{
                    //    layerConfig = LayerConfig.Setting0;
                    //}


                    return layerConfig;
                }

                public DFG(LayerConfigType[] layerConfigTypes)
                {
                    LayerConfigTypes = layerConfigTypes;
                }
            }
        }
    }
}
