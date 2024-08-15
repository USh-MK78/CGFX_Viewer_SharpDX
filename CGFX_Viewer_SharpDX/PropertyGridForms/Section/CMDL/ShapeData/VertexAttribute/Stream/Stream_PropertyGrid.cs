using CGFX_Viewer_SharpDX.CGFXPropertyGridSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGFXLibrary;
using CGFXLibrary.SOBJ_Format.ShapeData;

namespace CGFX_Viewer_SharpDX.PropertyGridForms.Section.CMDL.ShapeData.VertexAttribute.Stream
{
    [TypeConverter(typeof(CGFX_CustomPropertyGridClass.CustomSortTypeConverter))]
    public class Stream_PropertyGrid
    {
        public CGFXLibrary.VertexAttribute.Usage VertexAttributeUsageFlag { get; set; }
        public CGFXLibrary.VertexAttribute.Flag VertexAttributeFlag { get; set; }
        public int VertexStreamLength { get; set; }
        public int VertexStreamOffset { get; set; }
        public List<byte> VertexStreamList { get; set; }
        public int LocationAddress { get; set; } //VertexStreamOffset seems to be used when this one is 0...
        public int MemoryArea { get; set; }
        public List<Polygon> PolygonList { get; set; }

        public int UnknownData1 { get; set; }
        public int UnknownData2 { get; set; }

        public int VertexDataEntrySize { get; set; }
        public int NrVertexStreams { get; set; }
        public int VertexStreamsOffsetListOffset { get; set; }

        public List<Shape.VertexAttribute.Stream.VertexStream> VertexStream_List = new List<Shape.VertexAttribute.Stream.VertexStream>();
        [Editor(typeof(VertexStream.VertexStreamEditor), typeof(UITypeEditor))]
        public List<Shape.VertexAttribute.Stream.VertexStream> VertexStreams { get => VertexStream_List; set => VertexStream_List = value; }

        public Stream_PropertyGrid(Shape.VertexAttribute.Stream stream)
        {
            VertexAttributeUsageFlag = stream.VertexAttributeUsageFlag;
            VertexAttributeFlag = stream.VertexAttributeFlag;
            VertexStreamLength = stream.VertexStreamLength;
            VertexStreamOffset = stream.VertexStreamOffset;
            VertexStreamList = stream.VertexStreamList;
            LocationAddress = stream.LocationAddress;
            MemoryArea = stream.MemoryArea;

            PolygonList = stream.PolygonList;

            UnknownData1 = stream.UnknownData1;
            UnknownData2 = stream.UnknownData2;

            VertexDataEntrySize = stream.VertexDataEntrySize;
            NrVertexStreams = stream.NrVertexStreams;
            VertexStreamsOffsetListOffset = stream.VertexStreamsOffsetListOffset;

            VertexStreams = stream.VertexStreams;
        }

        public Stream_PropertyGrid()
        {
            //BufferObject = 0;
            //LocationFlag = 0;
            VertexAttributeUsageFlag = new CGFXLibrary.VertexAttribute.Usage(-1);
            VertexAttributeFlag = new CGFXLibrary.VertexAttribute.Flag(-1);
            VertexStreamLength = 0;
            VertexStreamOffset = 0;
            VertexStreamList = new List<byte>();
            LocationAddress = 0;
            MemoryArea = 0;
            PolygonList = new List<Polygon>();

            UnknownData1 = 0;
            UnknownData2 = 0;

            VertexDataEntrySize = 0;
            NrVertexStreams = 0;
            VertexStreamsOffsetListOffset = 0;
            VertexStreams = new List<Shape.VertexAttribute.Stream.VertexStream>();
        }
    }
}
