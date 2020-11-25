using System;
using System.Collections.Generic;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using UnityEngine;

namespace ETModel
{
	public static class ILHelper
	{
		public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
			//由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
			appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
			//这里做一些ILRuntime的注册，这里我们注册值类型Binder，注释和解注下面的代码来对比性能差别
			appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
			appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
			appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());

			InitDelegates(appdomain);
			SetupCLRRedirection(appdomain);

			// 注册适配器(需要在适配器添加 ILAdapterAttribute 标签)
			Assembly assembly = typeof(Init).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				object obj = Activator.CreateInstance(type);
				CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
				if (adaptor == null)
				{
					continue;
				}
				appdomain.RegisterCrossBindingAdaptor(adaptor);
			}

			CLRBindings.Initialize(appdomain);
			LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}

		private static void InitDelegates(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			ILRegType.RegisterFunctionDelegate(appdomain);
			ILRegType.RegisterDelegateConvertor(appdomain);
			ILRegType.RegisterMethodDelegate(appdomain);
		}

		unsafe static void SetupCLRRedirection(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			//这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
			var arr = typeof(GameObject).GetMethods();
			foreach (var i in arr)
			{
				if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
				{
					appdomain.RegisterCLRMethodRedirection(i, AddComponent);
				}
				else if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
				{
					appdomain.RegisterCLRMethodRedirection(i, GetComponent);
				}
			}
		}

		unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
		{
			//CLR重定向的说明请看相关文档和教程，这里不多做解释
			ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

			var ptr = __esp - 1;
			//成员方法的第一个参数为this
			GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
			if (instance == null)
				throw new System.NullReferenceException();
			__intp.Free(ptr);

			var genericArgument = __method.GenericArguments;
			//AddComponent应该有且只有1个泛型参数
			if (genericArgument != null && genericArgument.Length == 1)
			{
				var type = genericArgument[0];
				object res;
				if (type is CLRType)
				{
					//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
					res = instance.AddComponent(type.TypeForCLR);
				}
				else
				{
					//热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
					var ilInstance = new ILTypeInstance(type as ILType, false);//手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
																			   //接下来创建Adapter实例
					var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
					//unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
					clrInstance.ILInstance = ilInstance;
					clrInstance.AppDomain = __domain;
					//这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
					ilInstance.CLRInstance = clrInstance;

					res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance

					clrInstance.Awake();//因为Unity调用这个方法时还没准备好所以这里补调一次
				}

				return ILIntepreter.PushObject(ptr, __mStack, res);
			}

			return __esp;
		}
		unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
		{
			//CLR重定向的说明请看相关文档和教程，这里不多做解释
			ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

			var ptr = __esp - 1;
			//成员方法的第一个参数为this
			GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
			if (instance == null)
				throw new System.NullReferenceException();
			__intp.Free(ptr);

			var genericArgument = __method.GenericArguments;
			//AddComponent应该有且只有1个泛型参数
			if (genericArgument != null && genericArgument.Length == 1)
			{
				var type = genericArgument[0];
				object res = null;
				if (type is CLRType)
				{
					//Unity主工程的类不需要任何特殊处理，直接调用Unity接口
					res = instance.GetComponent(type.TypeForCLR);
				}
				else
				{
					//因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
					var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
					for (int i = 0; i < clrInstances.Length; i++)
					{
						var clrInstance = clrInstances[i];
						if (clrInstance.ILInstance != null)//ILInstance为null, 表示是无效的MonoBehaviour，要略过
						{
							if (clrInstance.ILInstance.Type == type)
							{
								res = clrInstance.ILInstance;//交给ILRuntime的实例应该为ILInstance
								break;
							}
						}
					}
				}

				return ILIntepreter.PushObject(ptr, __mStack, res);
			}

			return __esp;
		}
	}
}