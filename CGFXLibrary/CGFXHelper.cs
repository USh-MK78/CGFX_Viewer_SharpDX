using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace CGFXLibrary
{
    public class ReadByteLine
    {
        public List<byte> charByteList { get; set; }

        public ReadByteLine(List<byte> Input)
        {
            charByteList = Input;
        }

        public ReadByteLine(List<char> Input)
        {
            charByteList = Input.Select(x => (byte)x).ToArray().ToList();
        }

        public void ReadByte(BinaryReader br, byte Split = 0x00)
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte PickStr = br.ReadByte();
                charByteList.Add(PickStr);
                if (PickStr == Split)
                {
                    break;
                }
            }
        }

        public void ReadMultiByte(BinaryReader br, byte Split = 0x00)
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] PickStr = br.ReadBytes(2);
                charByteList.Add(PickStr[0]);
                charByteList.Add(PickStr[1]);
                if (PickStr[0] == Split)
                {
                    break;
                }
            }
        }

        public void ReadByte(BinaryReader br, char Split = '\0')
        {
            //var br = br.BaseStream;
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte PickStr = br.ReadByte();
                charByteList.Add(PickStr);
                if (PickStr == Split)
                {
                    break;
                }
            }
        }

        public void WriteByte(BinaryWriter bw)
        {
            bw.Write(ConvertToCharArray());
        }

        public char[] ConvertToCharArray()
        {
            return charByteList.Select(x => (char)x).ToArray();
        }

        public int GetLength()
        {
            return charByteList.ToArray().Length;
        }
    }

    public class Flags
	{

        // [CGFX IndentFlag (反転前)]
        //
        //      Models (CMDL) (Primitive) => Flag : 92 00 00 40
        //    Models (CMDL) (HasSkeletal) => Flag : 12 00 00 40
        //       Textures (TXOB, Texture) => Flag : 11 00 00 20
        //      Textures (TXOB, Material) => Flag : 02 00 00 40
        //             LookupTable (LUTS) => Flag : 00 00 00 04
        //               Materials (MTOB) => Flag : 00 00 00 08
        //                 Shaders (SHDR) => Flag : 01 00 00 80
        //                 Cameras (CCAM) => Flag : 0A 00 00 40
        //                  Lights (CVLT) => Flag : 22 02 00 40
        //                  Lights (CHLT) => Flag : 22 01 00 40
        //                  Lights (CALT) => Flag : 22 04 00 40
        //                  Lights (CFLT) => Flag : A2 00 00 40
        //                    Fogs (CFOG) => Flag : 42 00 00 40
        //            Environments (CENV) => Flag : 00 00 80 00
        //              Animations (CANM) => Flag : 00 00 00 00 (IdentFlag : None) [Skeleton. Texture, Visibility, Camera, Light Animations]
        //                Particle (CNOD) => Flag : 01 00 00 40
        //                 Emitter (PEMT) => Flag : 06 00 00 40
        //                     StringData => Flag : 00 00 00 10
        //                      Int32Data => Flag : 00 00 00 20
        //                     RealNumber => Flag : 00 00 00 80
        //                    SOBJ (Mesh) => Flag : 00 00 00 01
        //                   SOBJ (Shape) => Flag : 01 00 00 10

        //                     Animation0 => Flag : 08 00 00 00
        //                     Animation1 => Flag : 20 00 00 00
        //                     Animation2 => Flag : 28 00 00 00
        //                     Animation3 => Flag : 30 00 00 00
        //                     Animation4 => Flag : 38 00 00 00

        public enum CGFXIdentFlag : uint
        {
            NONE = 0b_0000_0000_0000_0000_0000_0000_0000_0000,
            F1 = 0b_0000_0000_0000_0000_0000_0000_0000_0001, //???
            F2 = 0b_0000_0000_0000_0000_0000_0000_0000_0010, //IsSectionData (CMDL, CFOG, CFLT, CCAM etc...)
            F3 = 0b_0000_0000_0000_0000_0000_0000_0000_0100, //IsScene (?)
            F4 = 0b_0000_0000_0000_0000_0000_0000_0000_1000, //Has CameraSection
            F5 = 0b_0000_0000_0000_0000_0000_0000_0001_0000, //HasSkeletalModel (CMDL)
            F6 = 0b_0000_0000_0000_0000_0000_0000_0010_0000, //IsLightData
            F7 = 0b_0000_0000_0000_0000_0000_0000_0100_0000, //IsFogData
            F8 = 0b_0000_0000_0000_0000_0000_0000_1000_0000, //HasPrimitiveModel (CMDL)
            F9 = 0b_0000_0000_0000_0000_0000_0001_0000_0000, //0 = Fragment Light (default?), 1 = Hemisphere Light
            F10 = 0b_0000_0000_0000_0000_0000_0010_0000_0000, //HasVertexLight
            F11 = 0b_0000_0000_0000_0000_0000_0100_0000_0000, //HasAmbientLight
            F12 = 0b_0000_0000_0000_0000_0000_1000_0000_0000,
            F13 = 0b_0000_0000_0000_0000_0001_0000_0000_0000,
            F14 = 0b_0000_0000_0000_0000_0010_0000_0000_0000,
            F15 = 0b_0000_0000_0000_0000_0100_0000_0000_0000,
            F16 = 0b_0000_0000_0000_0000_1000_0000_0000_0000,

            F17 = 0b_0000_0000_0000_0001_0000_0000_0000_0000,
            F18 = 0b_0000_0000_0000_0010_0000_0000_0000_0000,
            F19 = 0b_0000_0000_0000_0100_0000_0000_0000_0000, //ColorProperty (CFOG)
            F20 = 0b_0000_0000_0000_1000_0000_0000_0000_0000,
            F21 = 0b_0000_0000_0001_0000_0000_0000_0000_0000,
            F22 = 0b_0000_0000_0010_0000_0000_0000_0000_0000,
            F23 = 0b_0000_0000_0100_0000_0000_0000_0000_0000,
            F24 = 0b_0000_0000_1000_0000_0000_0000_0000_0000, //CENV
            F25 = 0b_0000_0001_0000_0000_0000_0000_0000_0000, //Mesh (CMDL)
            F26 = 0b_0000_0010_0000_0000_0000_0000_0000_0000, //IsSkeleton
            F27 = 0b_0000_0100_0000_0000_0000_0000_0000_0000, //LookUpTable
            F28 = 0b_0000_1000_0000_0000_0000_0000_0000_0000, //Material
            F29 = 0b_0001_0000_0000_0000_0000_0000_0000_0000, //Geometry, (IdentFlag & 0x0FFFFFFF = 0 => UserData (String))
            F30 = 0b_0010_0000_0000_0000_0000_0000_0000_0000, //Texture, (IdentFlag & 0x0FFFFFFF = 0 => UserData (Int32))
            F31 = 0b_0100_0000_0000_0000_0000_0000_0000_0000, //Model
            F32 = 0b_1000_0000_0000_0000_0000_0000_0000_0000, //Shader (IdentFlag & 0x0FFFFFFF = 0 => UserData (RealNumber[float, Color]))

            CMDL = F31 | F8 | F5 | F2,
            CCAM = F31 | F4 | F2,
            CFOG = F31 | F7 | F2,

            TXOB_v0 = F30 | F3,
            TXOB_v1 = F30 | F5 | F1,

            Animation0 = F6, //20 00 00 00 => 00 00 00 20
            Animation1 = F6 | F4, //28 00 00 00 => 00 00 00 28 
            Animation2 = F6 | F5 | F4,

            F32_F1 = F32 | F1,
        }

        public byte[] IdentFlag { get; set; } //0x4, Ex : [CFOG] 42 00 00 40 => 40 00 00 42

        #region New ways to determine IdentFlag (?)
        public byte[] RevIdentFlag => IdentFlag.Reverse().ToArray();
        public byte BitData0
        {
            get
            {
                return RevIdentFlag[0];
            }
            set
            {
                RevIdentFlag[0] = value;
            }
        }

        public byte BitData1
        {
            get
            {
                return RevIdentFlag[1];
            }
            set
            {
                RevIdentFlag[1] = value;
            }
        }

        public byte BitData2
        {
            get
            {
                return RevIdentFlag[2];
            }
            set
            {
                RevIdentFlag[2] = value;
            }
        }

        public byte BitData3
        {
            get
            {
                return RevIdentFlag[3];
            }
            set
            {
                RevIdentFlag[3] = value;
            }
        }

        #region Bit0
        //[Value]
        //IdentFlag[4] : 0 => IsPrimitive, 1 => IsSceneEnvironment, 2 => IsLight, 3 => HasSkeletalModel, 4 => IsCamera, 5 => IsEmitter, 6 => IsScene, 7 => IsShader
        //
        //0, 3
        //IdentFlag[4] : 0 => HasSkeletalModel, 1 => IsSceneEnvironment, 2 => IsLight, 3 => IsPrimitive, 4 => IsCamera, 5 => IsEmitter, 6 => IsScene, 7 => IsShader

        public bool IsPrimitive
        {
            get
            {
                return (BitData0 & 1) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 1);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 1);
            //}
        }

        public bool IsSceneEnvironment
        {
            get
            {
                return ((BitData0 & 2) >> 1) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 2);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 2);
            //}
        }

        public bool IsLight
        {
            get
            {
                return ((BitData0 & 4) >> 2) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 4);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 4);
            //}
        }

        public bool HasSkeletalModel
        {
            get
            {
                return ((BitData0 & 8) >> 3) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 8);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 8);
            //}
        }

        public bool IsCamera
        {
            get
            {
                return ((BitData0 & 10) >> 4) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 10);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 10);
            //}
        }

        public bool IsEmitter
        {
            get
            {
                return ((BitData0 & 20) >> 5) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 20);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 20);
            //}
        }

        public bool IsScene
        {
            get
            {
                return ((BitData0 & 40) >> 6) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 40);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 40);
            //}
        }

        public bool IsShader
        {
            get
            {
                return ((BitData0 & 80) >> 7) == 1 ? true : false;
            }
            //set
            //{
            //    if (value == true) BitData0 = (byte)(BitData0 | 80);
            //    else if (value == false) BitData0 = (byte)(BitData0 ^ 80);
            //}
        }
        #endregion

        #region Bit1

        #endregion

        #region Bit2

        #endregion

        #region Bit3
        public bool IsType0
        {
            get
            {
                return (BitData3 & 1) == 1 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 1);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 1);
            }
        }

        public bool IsType1
        {
            get
            {
                return (BitData3 & 2) == 2 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 2);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 2);
            }
        }

        public bool IsType2
        {
            get
            {
                return (BitData3 & 4) == 4 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 4);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 4);
            }
        }

        public bool IsType3
        {
            get
            {
                return (BitData3 & 8) == 8 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 8);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 8);
            }
        }

        public bool IsType4
        {
            get
            {
                return (BitData3 & 10) == 10 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 10);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 10);
            }
        }

        public bool IsType5
        {
            get
            {
                return (BitData3 & 20) == 20 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 20);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 20);
            }
        }

        public bool IsType6
        {
            get
            {
                return (BitData3 & 40) == 40 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 40);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 40);
            }
        }

        public bool IsType7
        {
            get
            {
                return (BitData3 & 80) == 80 ? true : false;
            }
            set
            {
                if (value == true) BitData3 = (byte)(BitData3 | 80);
                else if (value == false) BitData3 = (byte)(BitData3 ^ 80);
            }
        }
        #endregion

        #endregion

        public uint GetIdentFlagUInt()
        {
            return BitConverter.ToUInt32(IdentFlag, 0);
        }

        public CGFXIdentFlag[] GetCGFXIdentFlags()
        {
            List<CGFXIdentFlag> CGFXIdentFlagList = new List<CGFXIdentFlag>();
            CGFXIdentFlag flag = (CGFXIdentFlag)Enum.ToObject(typeof(CGFXIdentFlag), GetIdentFlagUInt());
            if (flag.HasFlag(CGFXIdentFlag.NONE)) CGFXIdentFlagList.Add(CGFXIdentFlag.NONE);
            if (flag.HasFlag(CGFXIdentFlag.F1)) CGFXIdentFlagList.Add(CGFXIdentFlag.F1);
            if (flag.HasFlag(CGFXIdentFlag.F2)) CGFXIdentFlagList.Add(CGFXIdentFlag.F2);
            if (flag.HasFlag(CGFXIdentFlag.F3)) CGFXIdentFlagList.Add(CGFXIdentFlag.F3);
            if (flag.HasFlag(CGFXIdentFlag.F4)) CGFXIdentFlagList.Add(CGFXIdentFlag.F4);
            if (flag.HasFlag(CGFXIdentFlag.F5)) CGFXIdentFlagList.Add(CGFXIdentFlag.F5);
            if (flag.HasFlag(CGFXIdentFlag.F6)) CGFXIdentFlagList.Add(CGFXIdentFlag.F6);
            if (flag.HasFlag(CGFXIdentFlag.F7)) CGFXIdentFlagList.Add(CGFXIdentFlag.F7);
            if (flag.HasFlag(CGFXIdentFlag.F8)) CGFXIdentFlagList.Add(CGFXIdentFlag.F8);
            if (flag.HasFlag(CGFXIdentFlag.F9)) CGFXIdentFlagList.Add(CGFXIdentFlag.F9);
            if (flag.HasFlag(CGFXIdentFlag.F10)) CGFXIdentFlagList.Add(CGFXIdentFlag.F10);
            if (flag.HasFlag(CGFXIdentFlag.F11)) CGFXIdentFlagList.Add(CGFXIdentFlag.F11);
            if (flag.HasFlag(CGFXIdentFlag.F12)) CGFXIdentFlagList.Add(CGFXIdentFlag.F12);
            if (flag.HasFlag(CGFXIdentFlag.F13)) CGFXIdentFlagList.Add(CGFXIdentFlag.F13);
            if (flag.HasFlag(CGFXIdentFlag.F14)) CGFXIdentFlagList.Add(CGFXIdentFlag.F14);
            if (flag.HasFlag(CGFXIdentFlag.F15)) CGFXIdentFlagList.Add(CGFXIdentFlag.F15);
            if (flag.HasFlag(CGFXIdentFlag.F16)) CGFXIdentFlagList.Add(CGFXIdentFlag.F16);
            if (flag.HasFlag(CGFXIdentFlag.F17)) CGFXIdentFlagList.Add(CGFXIdentFlag.F17);
            if (flag.HasFlag(CGFXIdentFlag.F18)) CGFXIdentFlagList.Add(CGFXIdentFlag.F18);
            if (flag.HasFlag(CGFXIdentFlag.F19)) CGFXIdentFlagList.Add(CGFXIdentFlag.F19);
            if (flag.HasFlag(CGFXIdentFlag.F20)) CGFXIdentFlagList.Add(CGFXIdentFlag.F20);
            if (flag.HasFlag(CGFXIdentFlag.F21)) CGFXIdentFlagList.Add(CGFXIdentFlag.F21);
            if (flag.HasFlag(CGFXIdentFlag.F22)) CGFXIdentFlagList.Add(CGFXIdentFlag.F22);
            if (flag.HasFlag(CGFXIdentFlag.F23)) CGFXIdentFlagList.Add(CGFXIdentFlag.F23);
            if (flag.HasFlag(CGFXIdentFlag.F24)) CGFXIdentFlagList.Add(CGFXIdentFlag.F24);
            if (flag.HasFlag(CGFXIdentFlag.F25)) CGFXIdentFlagList.Add(CGFXIdentFlag.F25);
            if (flag.HasFlag(CGFXIdentFlag.F26)) CGFXIdentFlagList.Add(CGFXIdentFlag.F26);
            if (flag.HasFlag(CGFXIdentFlag.F27)) CGFXIdentFlagList.Add(CGFXIdentFlag.F27);
            if (flag.HasFlag(CGFXIdentFlag.F28)) CGFXIdentFlagList.Add(CGFXIdentFlag.F28);
            if (flag.HasFlag(CGFXIdentFlag.F29)) CGFXIdentFlagList.Add(CGFXIdentFlag.F29);
            if (flag.HasFlag(CGFXIdentFlag.F30)) CGFXIdentFlagList.Add(CGFXIdentFlag.F30);
            if (flag.HasFlag(CGFXIdentFlag.F31)) CGFXIdentFlagList.Add(CGFXIdentFlag.F31);
            if (flag.HasFlag(CGFXIdentFlag.F32)) CGFXIdentFlagList.Add(CGFXIdentFlag.F32);

            return CGFXIdentFlagList.ToArray();
        }

        public CGFXIdentFlag GetFlags()
        {
            return (CGFXIdentFlag)Enum.ToObject(typeof(CGFXIdentFlag), GetIdentFlagUInt());
        }

        public Flags(byte[] bytes)
        {
            IdentFlag = bytes;
        }

        public override string ToString()
        {
            return "IdentFlags";
        }
    }

    public class VertexAttribute
    {
        public class Usage
        {
            public int TypeNum { get; set; }
            public UsageType UsageTypes => (UsageType)TypeNum;

            public Usage(int InputUsageTypeNum)
            {
                TypeNum = InputUsageTypeNum;
            }

            public Usage(UsageType usageType)
            {
                TypeNum = (int)usageType;
            }

            public int GetComponentCount()
            {
                int Count = -1;
                if (UsageTypes == UsageType.Position) Count = 3;
                if (UsageTypes == UsageType.Normal) Count = 3;
                if (UsageTypes == UsageType.Tangent) Count = 3;
                if (UsageTypes == UsageType.Color) Count = 4;
                if (UsageTypes == UsageType.TextureCoordinate0) Count = 2;
                if (UsageTypes == UsageType.TextureCoordinate1) Count = 2;
                if (UsageTypes == UsageType.TextureCoordinate2) Count = 2;
                return Count;
            }

            public enum UsageType
            {
                Position = 0,
                Normal = 1,
                Tangent = 2,
                Color = 3,
                TextureCoordinate0 = 4,
                TextureCoordinate1 = 5,
                TextureCoordinate2 = 6,
                BoneIndex = 7,
                BoneWeight = 8,
                UserAttribute0 = 9,
                UserAttribute1 = 10,
                UserAttribute2 = 11,
                UserAttribute3 = 12,
                UserAttribute4 = 13,
                UserAttribute5 = 14,
                UserAttribute6 = 15,
                UserAttribute7 = 16,
                UserAttribute8 = 17,
                UserAttribute9 = 18,
                UserAttribute10 = 19,
                UserAttribute11 = 20,
                Interleave = 21,
                Quantity = 22
            }
        }

        public class Flag
        {
            public int FlagNum { get; set; }
            public FlagType FlagTypes => (FlagType)FlagNum;

            public Flag(int InputFlagTypeNum)
            {
                FlagNum = InputFlagTypeNum;
            }

            public Flag(FlagType flagType)
            {
                FlagNum = (int)flagType;
            }

            public enum FlagType
            {
                VertexParam = 1,
                Interleave = 2
            }
        }
    }

    public class TaskHelper
	{
        public static Task<T> RunTask<T>(object obj)
		{
            Task<T> r = Task.Run(() => { return (T)obj; });
            return r;
		}

        public static Task<T> RunTask<T>(BinaryReader br, int Offset, object obj)
        {
            Task<T> r = Task.Run(() => 
            {
                long Pos = br.BaseStream.Position;

                br.BaseStream.Seek(-4, SeekOrigin.Current);

                //Move DataOffset
                br.BaseStream.Seek(Offset, SeekOrigin.Current);

                var output = (T)obj;

                br.BaseStream.Position = Pos;


                return output;
            });

            return r;
        }
    }

    public class Converter
	{
        public static bool ByteToBoolean(byte Input)
		{
            //if ((Input == 0 && Input == 1) != true) throw new Exception("value is not 0 or 1");
            //bool n = Input == 0 ? false : Input == 1 ? true : false;

            bool b = new bool();
            if (Input == 0) b = false;
            if (Input == 1) b = true;
            return b;
		}

        public static byte BooleanToByte(bool Input)
		{
            return Convert.ToByte(Input);
		}

        //public static bool ShortToBoolean(short Input)
        //{
        //    bool b = new bool();
        //    if (Input == 0) b = false;
        //    if (Input == 1) b = true;
        //    return b;
        //}

        //public static byte BooleanToShort(bool Input)
        //{
        //    return Convert.ToByte(Input);
        //}

        public double[] Vector3TodoubleArray(Vector3 vector3)
        {
            double vX = vector3.X;
            double vY = vector3.Y;
            double vZ = vector3.Z;

            return new double[] { vX, vY, vZ };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BVector3D"></param>
        /// <returns></returns>
        public static Vector3 ByteArrayToVector3(byte[][] BVector3D)
        {
            float Value_X = BitConverter.ToSingle(BVector3D[0], 0);
            float Value_Y = BitConverter.ToSingle(BVector3D[1], 0);
            float Value_Z = BitConverter.ToSingle(BVector3D[2], 0);

            return new Vector3(Value_X, Value_Y, Value_Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Vector3"></param>
        /// <returns></returns>
        public static byte[][] Vector3DToByteArray(Vector3 Vector3)
        {
            byte[] Byte_X = BitConverter.GetBytes(Convert.ToSingle(Vector3.X));
            byte[] Byte_Y = BitConverter.GetBytes(Convert.ToSingle(Vector3.Y));
            byte[] Byte_Z = BitConverter.GetBytes(Convert.ToSingle(Vector3.Z));

            return new byte[][] { Byte_X, Byte_Y, Byte_Z };
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="BPoint3D"></param>
        ///// <returns></returns>
        //public static Point3D ByteArrayToPoint3D(byte[][] BPoint3D)
        //{
        //    double Value_X = BitConverter.ToSingle(BPoint3D[0], 0);
        //    double Value_Y = BitConverter.ToSingle(BPoint3D[1], 0);
        //    double Value_Z = BitConverter.ToSingle(BPoint3D[2], 0);

        //    return new Point3D(Value_X, Value_Y, Value_Z);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Point3D"></param>
        ///// <returns></returns>
        //public static byte[][] Point3DToByteArray(Point3D Point3D)
        //{
        //    byte[] Byte_X = BitConverter.GetBytes(Convert.ToSingle(Point3D.X));
        //    byte[] Byte_Y = BitConverter.GetBytes(Convert.ToSingle(Point3D.Y));
        //    byte[] Byte_Z = BitConverter.GetBytes(Convert.ToSingle(Point3D.Z));

        //    return new byte[][] { Byte_X, Byte_Y, Byte_Z };
        //}

        public enum ConvertType
        {
            Boolean,
            Char,
            Double,
            Int16,
            Int32,
            Int64,
            Single,
            UInt16,
            UInt32,
            UInt64
        }

        public static T CustomBitConverter<T>(byte[] byteAry, int startIndex, ConvertType convertType)
        {
            object obj = new object();
            if (convertType == ConvertType.Boolean) obj = BitConverter.ToBoolean(byteAry, startIndex);
            if (convertType == ConvertType.Char) obj = BitConverter.ToChar(byteAry, startIndex);
            if (convertType == ConvertType.Double) obj = BitConverter.ToDouble(byteAry, startIndex);
            if (convertType == ConvertType.Int16) obj = BitConverter.ToInt16(byteAry, startIndex);
            if (convertType == ConvertType.Int32) obj = BitConverter.ToInt32(byteAry, startIndex);
            if (convertType == ConvertType.Int64) obj = BitConverter.ToInt64(byteAry, startIndex);
            if (convertType == ConvertType.Single) obj = BitConverter.ToSingle(byteAry, startIndex);
            if (convertType == ConvertType.UInt16) obj = BitConverter.ToUInt16(byteAry, startIndex);
            if (convertType == ConvertType.UInt32) obj = BitConverter.ToUInt32(byteAry, startIndex);
            if (convertType == ConvertType.UInt64) obj = BitConverter.ToUInt64(byteAry, startIndex);
            return (T)obj;
        }

        public static T CustomBitConverter<T>(byte[] byteAry, int startIndex)
        {
            object obj = new object();
            if (typeof(T) == typeof(bool)) obj = BitConverter.ToBoolean(byteAry, startIndex);
            if (typeof(T) == typeof(char)) obj = BitConverter.ToChar(byteAry, startIndex);
            if (typeof(T) == typeof(double)) obj = BitConverter.ToDouble(byteAry, startIndex);
            if (typeof(T) == typeof(short)) obj = BitConverter.ToInt16(byteAry, startIndex);
            if (typeof(T) == typeof(int)) obj = BitConverter.ToInt32(byteAry, startIndex);
            if (typeof(T) == typeof(long)) obj = BitConverter.ToInt64(byteAry, startIndex);
            if (typeof(T) == typeof(float)) obj = BitConverter.ToSingle(byteAry, startIndex);
            if (typeof(T) == typeof(ushort)) obj = BitConverter.ToUInt16(byteAry, startIndex);
            if (typeof(T) == typeof(uint)) obj = BitConverter.ToUInt32(byteAry, startIndex);
            if (typeof(T) == typeof(ulong)) obj = BitConverter.ToUInt64(byteAry, startIndex);
            return (T)obj;
        }
    }

    public class BinaryReadHelper
	{
        public byte[] BOMs;
        public BinaryReader BR { get; set; }
        public T[] ReadArray<T>(int Count, int ByteLength)
		{
            T[] Ary = new T[Count];
            for (int i = 0; i < Count; i++) Ary[i] = Converter.CustomBitConverter<T>(BR.ReadBytes(ByteLength), 0);
            return Ary;
		}

        public byte[] ReadArray(int Count)
		{
            byte[] Ary = new byte[Count];
            for (int i = 0; i < Count; i++) Ary[i] = BR.ReadByte();
            return Ary;
        }

        public BinaryReadHelper(BinaryReader br, byte[] BOM)
		{
            BOMs = BOM;
            BR = br;
        }
	}

    public struct Face
    {
        public int f0 { get; set; }
        public int f1 { get; set; }
        public int f2 { get; set; }

        public IList<int> FaceToTriangle()
        {
            return new int[] { f0, f1, f2 }.ToList();
        }

        public Face(int i0, int i1, int i2)
        {
            this.f0 = i0;
            this.f1 = i1;
            this.f2 = i2;
        }
    }

    public class CGFXVector
	{

        public struct Vector3<T>
        {
            public T X;
            public T Y;
            public T Z;

            public Vector3(T x, T y, T z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public Vector3 ToVector3D()
            {
                Vector3 vector3D = new Vector3(0, 0, 0);
                if (typeof(T) == typeof(float))
                {
                    List<Vector3<byte>> vector3Ds = new List<Vector3<byte>>();

                    //Vector3<byte[]> vector3byte = new Vector3<byte[]>(0x00, 0x00, 0x00);
                    
                }
                if (typeof(T) == typeof(double))
                {

                }

                return vector3D;
            }
        }

        //public struct Vector3<T>
        //{
        //    public T[] X;
        //    public T[] Y;
        //    public T[] Z;

        //    //public Vector3(T x, T y, T z)
        //    //{
        //    //    this.X = x;
        //    //    this.Y = y;
        //    //    this.Z = z;
        //    //}

        //    public Vector3(T[] x, T[] y, T[] z)
        //    {
        //        this.X = x;
        //        this.Y = y;
        //        this.Z = z;
        //    }

        //    public Vector3 ToVector3D()
        //    {
        //        Vector3 vector3D = new Vector3(0, 0, 0);
        //        if (typeof(T) == typeof(float))
        //        {
        //            List<Vector3<byte>> vector3Ds = new List<Vector3<byte>>();

        //            Vector3<byte> vector3byte = new Vector3<byte>(0x00, 0x00, 0x00);

        //        }
        //        if (typeof(T) == typeof(double))
        //        {

        //        }

        //        return vector3D;
        //    }
        //}

    }

    public class CGFXHelper
    {
        //public Matrix Matrix { get; set; }

        public class Matrix3x3
        {
            public double M11 { get; set; }
            public double M12 { get; set; }
            public double M13 { get; set; }

            public double M21 { get; set; }
            public double M22 { get; set; }
            public double M23 { get; set; }

            public double M31 { get; set; }
            public double M32 { get; set; }
            public double M33 { get; set; }

            public Matrix3x3(double ScaleU, double ScaleV, double Rotate, double TranslateU, double TranslateV)
            {
                //System.Windows.Media.Matrix matrix = new System.Windows.Media.Matrix();
                //matrix.Scale(ScaleU, ScaleV);
                //matrix.RotateAt(Rotate, 0, 0);
                //matrix.Translate(TranslateU, TranslateV);

                M11 = ScaleU * Math.Cos(Rotate);
                M12 = -Math.Sin(Rotate);
                M13 = TranslateU;

                M21 = Math.Sin(Rotate);
                M22 = ScaleV * Math.Cos(Rotate);
                M23 = TranslateV;

                M31 = 0;
                M32 = 0;
                M33 = 1;
            }
        }

        //public static Matrix3x3 dq()
        //{
        //    //M11, M12, M13
        //    //M21, M22, M23
        //    //M31, M32, M33
        //    Matrix3x3 matrix3X3 = new Matrix3x3(2, 3, -90, 30, 180);
        //    return matrix3X3;

        //}


        //public static MemoryStream BitmapToMemoryStream(Bitmap bitmap)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        //    //bitmap.Dispose();

        //    return memoryStream;
        //}
    }

    public class Polygon
    {
        public ScaleFactor Scale_Factor { get; set; }
        public class ScaleFactor
        {
            public float VtScale { get; set; }
            public float NrScale { get; set; }
            //public float TrScale { get; set; } //Unnecessary (?)
            public float TexCoord0Scale { get; set; }
            public float TexCoord1Scale { get; set; }
            public float TexCoord2Scale { get; set; }

            public ScaleFactor(float Vertex, float Normal, float TexCoord0, float TexCoord1, float TexCoord2)
            {
                VtScale = Vertex;
                NrScale = Normal;
                TexCoord0Scale = TexCoord0;
                TexCoord1Scale = TexCoord1;
                TexCoord2Scale = TexCoord2;
            }

            public ScaleFactor()
            {
                VtScale = 0;
                NrScale = 0;
                TexCoord0Scale = 0;
                TexCoord1Scale = 0;
                TexCoord2Scale = 0;

                //VtScale = 1.0f;
                //NrScale = 1.0f;
                //TexCoordScale = 1.0f;
                //TexCoord2Scale = 1.0f;
                //TexCoord3Scale = 1.0f;
            }
        }

        //public float ScaleFactor { get; set; } = 1;
        public Vector3 Vertex { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
        public Vector2 TexCoord0 { get; set; }
        public Vector2 TexCoord1 { get; set; }
        public Vector2 TexCoord2 { get; set; }
        public Color ColorData { get; set; }

        public struct Color
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;

            /// <summary>
            /// Convert to float array : 0=>R, 1=>G, 2=>B, 3=>A
            /// </summary>
            /// <returns></returns>
            public float[] ToArray()
            {
                var vR = (float)Math.Round((R / 255F), 2, MidpointRounding.AwayFromZero);
                var vG = (float)Math.Round((G / 255F), 2, MidpointRounding.AwayFromZero);
                var vB = (float)Math.Round((B / 255F), 2, MidpointRounding.AwayFromZero);
                var vA = (float)Math.Round((A / 255F), 2, MidpointRounding.AwayFromZero);

                return new float[] { vR, vG, vB, vA };
            }

            public Color(byte ColorR, byte ColorG, byte ColorB, byte ColorA)
            {
                R = ColorR;
                G = ColorG;
                B = ColorB;
                A = ColorA;
            }
        }

        //public struct TextureCoordinate
        //{
        //    public double X;
        //    public double Y;

        //    public System.Windows.Point ToPoint()
        //    {
        //        return new System.Windows.Point(X, Y);
        //    }

        //    public TextureCoordinate(double pX, double pY)
        //    {
        //        X = pX;
        //        Y = pY;
        //    }
        //}

        public void UpToY()
        {
            Vertex = new Vector3(Vertex.X, Vertex.Z, Vertex.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">T = <see cref="float"/> or T = <see cref="double"/></typeparam>
        /// <param name="dataType"><see cref="DataType"/></param>
        /// <returns>T[]</returns>
        public T[] GetScaledValue<T>(DataType dataType) where T : struct
        {
            T[] obj = new T[(int)dataType];
            if (dataType == DataType.Vt) obj = new T[] { (T)(object)(Vertex.X * Scale_Factor.VtScale), (T)(object)(Vertex.Y * Scale_Factor.VtScale), (T)(object)(Vertex.Z * Scale_Factor.VtScale) };
            else if (dataType == DataType.Nr) obj = new T[] { (T)(object)(Normal.X * Scale_Factor.NrScale), (T)(object)(Normal.Y * Scale_Factor.NrScale), (T)(object)(Normal.Z * Scale_Factor.NrScale) };
            else if (dataType == DataType.Tr) obj = new T[] { (T)(object)Tangent.X, (T)(object)Tangent.Y, (T)(object)Tangent.Z }; //No ScaleFactor
            else if (dataType == DataType.TexCoord0) obj = new T[] { (T)(object)(TexCoord0.X * Scale_Factor.TexCoord0Scale), (T)(object)(TexCoord0.Y * Scale_Factor.TexCoord0Scale) };
            else if (dataType == DataType.TexCoord1) obj = new T[] { (T)(object)(TexCoord1.X * Scale_Factor.TexCoord1Scale), (T)(object)(TexCoord1.Y * Scale_Factor.TexCoord1Scale) };
            else if (dataType == DataType.TexCoord2) obj = new T[] { (T)(object)(TexCoord2.X * Scale_Factor.TexCoord2Scale), (T)(object)(TexCoord2.Y * Scale_Factor.TexCoord2Scale) };
            return obj;
        }

        //public T[] ConvertToAnyArray<T>()
        //{
        //    return new T[]
        //}

        public void Test()
        {

            var g = GetScaledValue<float>(DataType.Vt);
            Vector3 vector3 = new Vector3(g);
        }

        public enum DataType
        {
            Vt = 3,
            Nr = 3,
            Tr = 3,
            TexCoord0 = 2,
            TexCoord1 = 2,
            TexCoord2 = 2,
            Color = 4,
        }

        public Polygon(Vector3 Vt, Vector3 Nr, Vector3 Tr, Vector2 TexCoord_0, Vector2 TexCoord_1, Vector2 TexCoord_2, Color color, ScaleFactor scaleFactor)
        {
            Scale_Factor = scaleFactor;
            Vertex = Vt;
            Normal = Nr;
            Tangent = Tr;
            TexCoord0 = TexCoord_0;
            TexCoord1 = TexCoord_1;
            TexCoord2 = TexCoord_2;
            ColorData = color;
        }

        public Polygon()
        {
            Scale_Factor = new ScaleFactor();
            Vertex = new Vector3();
            Normal = new Vector3();
            Tangent = new Vector3();
            TexCoord0 = new Vector2();
            TexCoord1 = new Vector2();
            TexCoord2 = new Vector2();
            ColorData = new Color();
        }
    }

    // [ Note ]
    //
    //    //if (Type == 0) UV = new UVTransform(-1, 0, 0, -1, 0, 0); //Default(?)
    //    //if (Type == 1) matrix = new Matrix(-1, 0, 0, -1, 0, 0);
    //    //if (Type == 2) matrix = new Matrix(1, 0, 0, 1, 0, 0);
    //    //if (Type == 3) matrix = new Matrix(0, -1, -1, 0, 0, 0);

    //    UVTransform UV = new UVTransform();
    //    if (Type == 0) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(0)), new Vector2(-1, -1), new Vector2(0, 0)); //Default(?)
    //    else if (Type == 1) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(0)), new Vector2(-1, -1), new Vector2(0, 0));
    //    else if (Type == 2) UV = new UVTransform(0, new Vector2(1, 1), new Vector2(0, 0));
    //    else if (Type == 3) UV = new UVTransform(Convert.ToSingle(HTK_3DES_WpfSharpDX.TSRSystem.AngleToRadian(90)), new Vector2(-1, -1), new Vector2(0, 0));
    //    return UV;
}
