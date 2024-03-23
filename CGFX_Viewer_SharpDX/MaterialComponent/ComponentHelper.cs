using HelixToolkit.Wpf.SharpDX.Core.Components;
using HelixToolkit.Wpf.SharpDX.Render;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDX;

namespace CGFX_Viewer_SharpDX.Component
{
    public class DependencyPropertyHelper
    {
        /// <summary>
        /// Metadata
        /// </summary>
        /// <typeparam name="TargetType">DependencyObjectにキャストさせる型</typeparam>
        /// <typeparam name="InputType">TargetTypeにキャストさせる型</typeparam>
        /// <typeparam name="DpChangeEventArgsT"></typeparam>
        public class Metadata<TargetType, InputType, DpChangeEventArgsT>
        {
            public DpChangeEventArgsT DefaultValue { get; set; }
            public string TargetPropertyName { get; set; }
            public string InputPropertyName { get; set; }

            /// <summary>
            /// MetadataからPropertyMetadataを作成
            /// </summary>
            /// <returns>Create PropertyMetadata</returns>
            protected internal PropertyMetadata ToPropertyMetadata()
            {
                return new PropertyMetadata(DefaultValue, (d, e) => { ComponentHelper.SetDpProp<TargetType, InputType>(d, TargetPropertyName, InputPropertyName, (DpChangeEventArgsT)e.NewValue); });
            }

            /// <summary>
            /// DependencyProperty
            /// </summary>
            /// <typeparam name="OwnerType">依存関係プロパティを登録している所有者の型</typeparam>
            /// <param name="RegisterPropertyName">依存関係プロパティの名前</param>
            /// <returns>DependencyProperty</returns>
            protected internal DependencyProperty RegisterDependencyProperty<OwnerType>(string RegisterPropertyName)
            {
                return DependencyProperty.Register(RegisterPropertyName, typeof(DpChangeEventArgsT), typeof(OwnerType), ToPropertyMetadata());
            }

            /// <summary>
            /// DependencyProperty
            /// </summary>
            /// <typeparam name="OwnerType">依存関係プロパティを登録している所有者の型</typeparam>
            /// <param name="RegisterPropertyName">依存関係プロパティの名前</param>
            /// <param name="propertyMetadata">PropertyMetadata</param>
            /// <returns>DependencyProperty</returns>
            protected internal DependencyProperty RegisterDependencyProperty<OwnerType>(string RegisterPropertyName, PropertyMetadata propertyMetadata)
            {
                return DependencyProperty.Register(RegisterPropertyName, typeof(DpChangeEventArgsT), typeof(OwnerType), propertyMetadata);
            }

            public Metadata(DpChangeEventArgsT DefaultValue, string TargetPropertyName, string InputPropertyName)
            {
                this.DefaultValue = DefaultValue;
                this.TargetPropertyName = TargetPropertyName;
                this.InputPropertyName = InputPropertyName;
            }
        }

        /// <summary>
        /// DependencyPropertyの作成
        /// </summary>
        /// <typeparam name="TargetType"></typeparam>
        /// <typeparam name="InputType"></typeparam>
        /// <typeparam name="DpChangeEventArgsT"></typeparam>
        /// <typeparam name="OwnerType"></typeparam>
        /// <param name="DefValue"></param>
        /// <param name="TargetPropName"></param>
        /// <param name="InputPropValueName"></param>
        /// <param name="DpRegisterName"></param>
        /// <returns></returns>
        public static DependencyProperty RegisterDpProperty<TargetType, InputType, DpChangeEventArgsT, OwnerType>(DpChangeEventArgsT DefValue, string TargetPropName, string InputPropValueName, string DpRegisterName)
        {
            return new Metadata<TargetType, InputType, DpChangeEventArgsT>(DefValue, TargetPropName, InputPropValueName).RegisterDependencyProperty<OwnerType>(DpRegisterName);
        }


        public void Test()
        {
            Metadata<string, int, float> metadata = new Metadata<string, int, float>(3.0f, "SceneNode", "Test");
            var h = metadata.ToPropertyMetadata();
            DependencyProperty dependencyProperty = metadata.RegisterDependencyProperty<Single>("");
        }
    }

    public class VariableHelper
    {
        public class CustomBufferComponent
        {
            public IRenderTechnique _renderTechnique;
            public DeviceContextProxy _deviceContextProxy;
            public ConstantBufferComponent ConstantBufferComponent;

            /// <summary>
            /// ConstantBufferのアタッチ
            /// </summary>
            public void Attach()
            {
                ConstantBufferComponent.Attach(_renderTechnique);
            }

            /// <summary>
            /// ConstantBufferのデタッチ => Dispose();
            /// </summary>
            public void Detach()
            {
                ConstantBufferComponent.Detach();
            }

            public void Dispose()
            {
                ConstantBufferComponent.Dispose();
            }

            /// <summary>
            /// 対象のConstantBuffer内に存在する[BufferPropertyName]に[value]を書き込む
            /// </summary>
            /// <typeparam name="T">Input value type</typeparam>
            /// <param name="BufferPropertyName">Property Name</param>
            /// <param name="value">Input value : typeof(T)</param>
            public void Write<T>(string BufferPropertyName, T value) where T : unmanaged
            {
                ConstantBufferComponent.WriteValueByName(BufferPropertyName, value);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Length"></param>
            /// <returns></returns>
            public IntPtr GetIntPtr(int Length)
            {
                //var k = ints1.ToList().Select(x => x.ToList().Select(y => y.Length).Sum()).Sum();
                return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * Length);
            }

            public void Upload()
            {
                ConstantBufferComponent.Upload(_deviceContextProxy);
            }

            public CustomBufferComponent(IRenderTechnique technique, DeviceContextProxy deviceContextProxy, string cbufferName, int c)
            {
                this._renderTechnique = technique;
                this._deviceContextProxy = deviceContextProxy;
                ConstantBufferComponent = new ConstantBufferComponent(new HelixToolkit.Wpf.SharpDX.Shaders.ConstantBufferDescription(cbufferName, c));
            }
        }


        public void Test(IRenderTechnique renderTechnique, DeviceContextProxy deviceContextProxy)
        {
            List<int> ints = new List<int>();
            ints.Add(Marshal.SizeOf<int[]>());

            IntPtr intPtr_ColorCombinerEquation = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 3);

            CustomBufferComponent customBufferComponent = new CustomBufferComponent(renderTechnique, deviceContextProxy, "CustomBuffer", ints.Sum());
            customBufferComponent.Write("TestValue3", new Vector3(0, 1, 0));
            customBufferComponent.Attach();
            customBufferComponent.Detach();
            customBufferComponent.Dispose();
        }

    }

    public class ComponentHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="T0_Property"></param>
        /// <param name="T1_Property"></param>
        /// <param name="DpPropertyEventArgObj">DependencyPropertyChangedEventArgs</param>
        public static void SetDpProp<T0, T1>(DependencyObject dependencyObject, string T0_Property, string T1_Property, object DpPropertyEventArgObj)
        {
            //((dependencyObject as T0).SceneNode(T0_Property) as T1).Scale(T1_Property) = (Cast T)e.NewValue => DpPropertyEventArgObj;
            var DpToT0 = (T0)(object)dependencyObject; //DependencyObject to T0 : Cast
            var t0 = DpToT0.GetType().GetProperty(T0_Property); //SceneNode(T0_Property)

            //var g = (T1)t0.GetValue(DpToT0);
            var t0_Cast_t1 = (T1)t0.GetValue(DpToT0); //T0 to T1 : Cast
            var T1Property = t0_Cast_t1.GetType().GetProperty(T1_Property); //Scale(T1_Property)
            T1Property.SetValue(t0_Cast_t1, DpPropertyEventArgObj); //T1.Scale = (Cast T)e.NewValue => DpPropertyEventArgObj
        }
    }
}
