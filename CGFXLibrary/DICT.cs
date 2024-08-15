using CGFXLibrary.CGFXSection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGFXLibrary.CGFXFormat;

namespace CGFXLibrary
{
    /// <summary>
    /// DICT
    /// </summary>
    public class DICT
    {
        public char[] DICT_Header { get; set; } //0x4
        public int DICTSize { get; set; } //0x4
        public int DICT_NumOfEntries { get; set; } //0x4

        public RootNode RootNodeData { get; set; }
        public class RootNode
        {
            public string Name;
            public uint RN_RefBit { get; set; } //0x4
            public ushort RN_LeftIndex { get; set; } //0x2
            public ushort RN_RightIndex { get; set; } //0x2
            public int RN_NameOffset { get; set; } //0x4
            public int RN_DataOffset { get; set; } //0x4

            public RootNode(uint RefBit, ushort LeftIndex, ushort RightIndex, int NameOffset, int DataOffset)
            {
                RN_RefBit = RefBit;
                RN_LeftIndex = LeftIndex;
                RN_RightIndex = RightIndex;
                RN_NameOffset = NameOffset;
                RN_DataOffset = DataOffset;
            }

            public RootNode()
            {
                RN_RefBit = 0;
                RN_LeftIndex = 0;
                RN_RightIndex = 0;
                RN_NameOffset = 0;
                RN_DataOffset = 0;
            }

            public void Read_RootNode(BinaryReader br, byte[] BOM)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                RN_RefBit = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                RN_LeftIndex = BitConverter.ToUInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RN_RightIndex = BitConverter.ToUInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RN_NameOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (RN_NameOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move RN_NameOffset
                    br.BaseStream.Seek(RN_NameOffset, SeekOrigin.Current);

                    ReadByteLine readByteLine = new ReadByteLine(new List<byte>());
                    readByteLine.ReadByte(br, 0x00);

                    Name = new string(readByteLine.ConvertToCharArray());

                    br.BaseStream.Position = Pos;
                }

                RN_DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (RN_DataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move RN_DataOffset
                    br.BaseStream.Seek(RN_DataOffset, SeekOrigin.Current);

                    //Read Section Data (?)

                    br.BaseStream.Position = Pos;
                }
            }

            public int GetLength()
            {
                return 16;
            }
        }

        public List<DICT_Entry> DICT_Entries { get; set; }
        public class DICT_Entry
        {
            public string Name;

            public uint RefBit { get; set; }
            public ushort LeftIndex { get; set; }
            public ushort RightIndex { get; set; }
            public int NameOffset { get; set; }
            public int DataOffset { get; set; }
            public CGFXData CGFXData { get; set; }

            public void Read_DICTEntry(BinaryReader br, byte[] BOM, bool EnableFlag, IO.BinaryIOInterface.BinaryIO Data = null)
            {
                EndianConvert endianConvert = new EndianConvert(BOM);
                RefBit = BitConverter.ToUInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                LeftIndex = BitConverter.ToUInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
                RightIndex = BitConverter.ToUInt16(endianConvert.Convert(br.ReadBytes(2)), 0);
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

                DataOffset = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
                if (DataOffset != 0)
                {
                    long Pos = br.BaseStream.Position;

                    br.BaseStream.Seek(-4, SeekOrigin.Current);

                    //Move DataOffset
                    br.BaseStream.Seek(DataOffset, SeekOrigin.Current);

                    CGFXData = new CGFXData(Data, EnableFlag);
                    CGFXData.Reader(br, BOM);

                    br.BaseStream.Position = Pos;
                }
            }

            public DICT_Entry(uint RefBit, ushort LeftIndex, ushort RightIndex, int NameOffset, int DataOffset, CGFXData CGFXData)
            {
                this.RefBit = RefBit;
                this.LeftIndex = LeftIndex;
                this.RightIndex = RightIndex;
                this.NameOffset = NameOffset;
                this.DataOffset = DataOffset;
                this.CGFXData = CGFXData;
            }

            public DICT_Entry()
            {
                RefBit = 0;
                LeftIndex = 0;
                RightIndex = 0;
                NameOffset = 0;
                DataOffset = 0;
                CGFXData = new CGFXData(null);
            }

            public int GetLength()
            {
                return 16;
            }

            public string GetEntryName()
            {
                return Name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public void ReadDICT(BinaryReader br, byte[] BOM, bool EnableFlag, IO.BinaryIOInterface.BinaryIO Data = null)
        {
            EndianConvert endianConvert = new EndianConvert(BOM);
            DICT_Header = br.ReadChars(4);
            if (new string(DICT_Header) != "DICT") throw new Exception("不明なフォーマットです");

            DICTSize = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            DICT_NumOfEntries = BitConverter.ToInt32(endianConvert.Convert(br.ReadBytes(4)), 0);
            RootNodeData.Read_RootNode(br, BOM);

            for (int i = 0; i < DICT_NumOfEntries; i++)
            {
                DICT_Entry dICT_Entry = new DICT_Entry();
                dICT_Entry.Read_DICTEntry(br, BOM, EnableFlag, Data);
                DICT_Entries.Add(dICT_Entry);
            }
        }

        public int GetLength()
        {
            int H = DICT_Header.Length;
            int Size = 4;
            int NumOfEntries = 4;
            int RNSize = RootNodeData.GetLength();
            int count = DICT_Entries.Select(x => x.GetLength()).Sum();

            return H + Size + NumOfEntries + RNSize + count;
        }

        public void GenerateTree()
        {
            TreeGenerator treeGenerator = new TreeGenerator(RootNodeData, DICT_Entries);
            treeGenerator.GenerateTree();

            bool first = true;
            foreach (var v in treeGenerator.TreeNodes)
            {
                if (first)
                {
                    v.idxEntry = -1;
                    RootNodeData.RN_LeftIndex = (ushort)(v.left.idxEntry + 1);
                    RootNodeData.RN_RightIndex = (ushort)(v.right.idxEntry + 1);
                    RootNodeData.RN_RefBit = 0xFFFFFFFF;
                    first = false;
                    continue;
                }
                DICT_Entries[v.idxEntry].LeftIndex = (ushort)(v.left.idxEntry + 1);
                DICT_Entries[v.idxEntry].RightIndex = (ushort)(v.right.idxEntry + 1);
                DICT_Entries[v.idxEntry].RefBit = (uint)v.refbit;
            }
        }

        public DICT()
        {
            DICT_Header = "DICT".ToCharArray();
            DICTSize = 0;
            DICT_NumOfEntries = 0;
            RootNodeData = new RootNode();
            DICT_Entries = new List<DICT_Entry>();
        }
    }

    #region TreeNode
    public class DICT_TreeNode
    {
        public uint refbit;
        public DICT_TreeNode left;
        public DICT_TreeNode right;
        public int idxEntry;
        public string name;
        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// TreeGenerator [ Referenced from PatriciaTreeGenerator (Every File Explorer) ]
    /// </summary>
    public class TreeGenerator
    {
        public DICT.RootNode RootDICT;
        public List<DICT.DICT_Entry> DICT_Entries;

        public string[] NameArray
        {
            get
            {
                string[] NameArray = new string[DICT_Entries.Count];

                if (DICT_Entries != null || DICT_Entries.Count > 0)
                {
                    for (int i = 0; i < DICT_Entries.Count; i++) NameArray[i] = DICT_Entries[i].Name;
                }

                return NameArray;
            }
        }

        public DICT_TreeNode RootTreeNode;
        public List<DICT_TreeNode> TreeNodes;

        public int MaxStringLength { get; set; } = 0;

        void AddRootTreeNode()
        {
            DICT_TreeNode p = new DICT_TreeNode();
            p.refbit = (uint)(MaxStringLength * 8) - 1;
            p.left = p;
            p.right = p;
            p.idxEntry = 0;
            p.name = new string('\0', MaxStringLength);
            RootTreeNode = p;
        }

        /// <summary>
        /// Add TreeNode
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Index"></param>
        /// <returns>TreeNode</returns>
        public DICT_TreeNode AddTreeNode(string Name, int Index)
        {
            Name = Name.PadRight(MaxStringLength, '\0');
            DICT_TreeNode n = new DICT_TreeNode();
            n.name = Name;
            DICT_TreeNode CurNode = RootTreeNode;
            DICT_TreeNode leftNode = CurNode.left;
            uint bit = (uint)(MaxStringLength * 8) - 1;

            while (CurNode.refbit > leftNode.refbit)
            {
                CurNode = leftNode;
                leftNode = GetBit(Name, leftNode.refbit) ? leftNode.right : leftNode.left;
            }

            while (GetBit(leftNode.name, bit) == GetBit(Name, bit)) bit--;

            CurNode = RootTreeNode;
            leftNode = CurNode.left;

            while ((CurNode.refbit > leftNode.refbit) && (leftNode.refbit > bit))
            {
                CurNode = leftNode;
                leftNode = GetBit(Name, leftNode.refbit) ? leftNode.right : leftNode.left;
            }

            n.refbit = bit;
            n.left = GetBit(Name, n.refbit) ? leftNode : n;
            n.right = GetBit(Name, n.refbit) ? n : leftNode;
            if (GetBit(Name, CurNode.refbit)) CurNode.right = n;
            else CurNode.left = n;
            n.idxEntry = Index;
            TreeNodes.Add(n);
            return n;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static bool GetBit(string name, uint bit)
        {
            if (name == null || bit / 8 >= name.Length) throw new ArgumentException();
            return (((int)name[(int)bit / 8] >> ((int)bit & 7)) & 1) == 1;
        }

        /// <summary>
        /// Sort TreeNode
        /// </summary>
        public void Sort()
        {
            SortedDictionary<String, DICT_TreeNode> alphabet = new SortedDictionary<string, DICT_TreeNode>();
            foreach (DICT_TreeNode p in TreeNodes)
            {
                if (p.name.TrimEnd('\0') == "") continue;
                alphabet.Add(p.name.TrimEnd('\0'), p);
            }

            List<DICT_TreeNode> AlphabetSortedNodes = new List<DICT_TreeNode>();
            foreach (DICT_TreeNode p in alphabet.Values)
            {
                AlphabetSortedNodes.Add(p);
            }

            List<DICT_TreeNode> LengthSorted = new List<DICT_TreeNode>();
            for (int j = 0; j < TreeNodes.Count - 1; j++)
            {
                int longest = -1;
                int longestlength = -1;
                for (int i = 0; i < AlphabetSortedNodes.Count; i++)
                {
                    if (AlphabetSortedNodes[i].name.TrimEnd('\0').Length > longestlength)
                    {
                        longest = i;
                        longestlength = AlphabetSortedNodes[i].name.TrimEnd('\0').Length;
                    }
                }
                LengthSorted.Add(AlphabetSortedNodes[longest]);
                AlphabetSortedNodes.RemoveAt(longest);
            }

            LengthSorted.Insert(0, RootTreeNode);
            TreeNodes = LengthSorted;
        }

        public void GenerateTree()
        {
            foreach (var Name in NameArray)
            {
                if (Name.Length > MaxStringLength)
                {
                    MaxStringLength = Name.Length;
                }
            }

            AddRootTreeNode();

            for (int i = 0; i < NameArray.Length; i++)
            {
                AddTreeNode(NameArray[i], i);
            }
            
            Sort();
        }

        /// <summary>
        /// Initialize DICT TreeGenerator
        /// </summary>
        /// <param name="RootDICT"></param>
        /// <param name="DICT_Entries"></param>
        public TreeGenerator(DICT.RootNode RootDICT, List<DICT.DICT_Entry> DICT_Entries)
        {
            this.RootDICT = RootDICT;
            this.DICT_Entries = DICT_Entries;
        }
    }
    #endregion
}
