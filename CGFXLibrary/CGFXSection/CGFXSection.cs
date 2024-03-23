//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CGFXLibrary.CGFXSection
//{
//    public class CGFXSection
//    {
//        //Models
//        public CMDL CMDLSection { get; set; }

//        //Textures
//        public TXOB TXOBSection { get; set; }

//        public TXOB TXOB_MaterialSection { get; set; }

//        //LUTS
//        public LUTS LUTSSection { get; set; } //0x00000004

//        //Materials
//        public MTOB MTOBSection { get; set; } //0x00000008

//        //Shaders
//        public SHDR SHDRSection { get; set; } //0x01000080

//        //Cameras
//        public CCAM CCAMSection { get; set; } //0x0A000040

//        //Lights
//        public CVLT CVLTSection { get; set; } //0x22020040
//        public CHLT CHLTSection { get; set; } //0x22010040
//        public CALT CALTSection { get; set; } //0x22040040
//        public CFLT CFLTSection { get; set; } //0xA2000040

//        //Fogs
//        public CFOG CFOGSection { get; set; }

//        //Environments
//        public CENV CENVSection { get; set; }

//        //Skeleton Animations
//        //Texture Animations
//        //Visibility Animations
//        //Camera Animations
//        //Light Animations
//        //Emitters
//        //Unknown

//        public CGFXSection()
//        {
//            CMDLSection = new CMDL();
//            //TXOBSection = new TXOB(TXOB.Type.Texture);
//            TXOBSection = new TXOB(TXOB.Type.Texture);
//            TXOB_MaterialSection = new TXOB(TXOB.Type.MaterialInfo);
//            LUTSSection = new LUTS();
//            MTOBSection = new MTOB();
//            SHDRSection = new SHDR();
//            CCAMSection = new CCAM();

//            CVLTSection = new CVLT();
//            CHLTSection = new CHLT();
//            CALTSection = new CALT();
//            CFLTSection = new CFLT();

//            CFOGSection = new CFOG();
//            CENVSection = new CENV();
//        }
//    }
//}
