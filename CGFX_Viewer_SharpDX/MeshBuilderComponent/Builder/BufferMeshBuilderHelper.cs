using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace CGFX_Viewer_SharpDX.MeshBuilderComponent.Builder
{
    internal class CustomMeshBuilderHelper
    {
        public static float DotProduct(ref Vector3 first, ref Vector3 second)
        {
            float result;
            Vector3.Dot(ref first, ref second, out result);
            return result;
        } 

    }

}
