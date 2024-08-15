using CGFXLibrary.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFXLibrary
{
    public class CGFXDataItemInterface
    {
        public interface ICGFXDataItem
        {
            bool EnableFlagValue { get; set; }
            uint FlagValue { get; set; }
            Flags CGFXFlags { get; set; }
        }

        /// <summary>
        /// AbstractCGFXDataItem
        /// </summary>
        public abstract class AbstractCGFXDataItem : BinaryIOInterface.BinaryIO, ICGFXDataItem
        {
            public abstract bool EnableFlagValue { get; set; }
            public uint FlagValue { get; set; }
            public Flags CGFXFlags { get; set; }

            /// <summary>
            /// Initialize AbstractCGFXDataItem
            /// </summary>
            /// <param name="EnableFlag">EnableFlag</param>
            protected AbstractCGFXDataItem(bool EnableFlag = true)
            {
                this.EnableFlagValue = EnableFlag;
                if (EnableFlag == true)
                {
                    this.EnableFlagValue = EnableFlag;
                    this.FlagValue = 0;
                }
                else if (EnableFlag == false)
                {
                    this.EnableFlagValue = EnableFlag;
                    this.FlagValue = 0xFFFFFFFF;
                }
            }
        }
    }
}
