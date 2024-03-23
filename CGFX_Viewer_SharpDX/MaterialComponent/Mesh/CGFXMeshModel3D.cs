using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.Component.Mesh
{
    public class CGFXMeshModel3D : MeshBuilderComponent.Mesh.GoemetryModel.CGFXMeshGeometryModel3D
    {
        protected override SceneNode OnCreateSceneNode()
        {
            //new TechniqueDescription("CGFXMeshShader")
            var node = base.OnCreateSceneNode();
            node.OnSetRenderTechnique = (host) => { return node.EffectsManager["CGFXMeshShader"]; };
            return node;
        }
    }
}
