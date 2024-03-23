using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Render;
using HelixToolkit.Wpf.SharpDX.Shaders;
using HelixToolkit.Wpf.SharpDX.Utilities;
using HelixToolkit.Wpf.SharpDX;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CGFX_Viewer_SharpDX.Component.Material
{
    public abstract class CustomMaterialVariable : MaterialVariable
    {
        static readonly ILogger logger = HelixToolkit.Logger.LogManager.Create<MaterialVariable>();

        protected new IRenderTechnique Technique { get; }
        protected new IEffectsManager EffectsManager { get; }

        private readonly object updateLock = new object();
        private readonly MaterialCore material;

        private ConstantBufferDescription materialCBDescription;
        private ConstantBufferDescription nonMaterialCBDescription = DefaultNonMaterialBufferDesc;

        private readonly int storageId = -1;
        private ArrayStorage storage;

        public new event EventHandler UpdateNeeded;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMaterialVariable"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="technique">The technique.</param>
        /// <param name="meshMaterialConstantBufferDesc">The Constant Buffer description</param>
        /// <param name="materialCore"></param>
        public CustomMaterialVariable(IEffectsManager manager, IRenderTechnique technique, ConstantBufferDescription meshMaterialConstantBufferDesc, MaterialCore materialCore) : base(manager, technique, meshMaterialConstantBufferDesc, materialCore)
        {
            Technique = technique;
            EffectsManager = manager;
            if (materialCore != null)
            {
                material = materialCore;
                material.PropertyChanged += MaterialCore_PropertyChanged;
            }
            materialCBDescription = meshMaterialConstantBufferDesc;
            if (manager != null)
            {
                storage = manager.StructArrayPool.Register(materialCBDescription.StructSize);
                storageId = storage.GetId();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value (<see cref="{T}" />[])</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteValueArray<T>(string name, ref T[] value) where T : unmanaged
        {
            int structSize = -1;
            if (materialCB != null && materialCB.TryGetVariableByName(name, out var variable))
            {
                if (UnsafeHelper.SizeOf<T>() > variable.Size)
                {
                    structSize = UnsafeHelper.SizeOf<T>();
                    throw new ArgumentException($"Input struct size {structSize} is larger than shader variable {variable.Name} size {variable.Size}");
                }
                else
                {
                    structSize = UnsafeHelper.SizeOf<T>();
                }

                unsafe
                {
                    fixed (T* pValue = value)
                    {
                        if (!storage.Write(storageId, variable.StartOffset, new IntPtr(pValue), structSize))
                        {
                            throw new ArgumentException($"Failed to write value on {name}");
                        }
                    }
                }
            }
            else
            {
                #if DEBUG
                throw new ArgumentException($"Variable not found in constant buffer {materialCB.Name}. Variable = {name}");
                #else
                logger.LogWarning("Variable not found in constant buffer {0}. Variable = {1}", materialCB.Name, name);
                #endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposeManagedResources"></param>
        protected override void OnDispose(bool disposeManagedResources)
        {
            RemoveAndDispose(ref materialCB);
            RemoveAndDispose(ref nonMaterialCB);
            storage.ReleaseId(storageId);
            RemoveAndDispose(ref storage);
            if (disposeManagedResources)
            {
                UpdateNeeded = null;
                if (material != null)
                {
                    material.PropertyChanged -= MaterialCore_PropertyChanged;
                }
                propertyBindings.Clear();
            }
            base.OnDispose(disposeManagedResources);
        }

        #region Material Property Bindings

        private readonly Dictionary<string, Action> propertyBindings = new Dictionary<string, Action>();

        private void MaterialCore_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TriggerPropertyAction(e.PropertyName);
            InvalidateRenderer();
        }
        #endregion
    }
}
