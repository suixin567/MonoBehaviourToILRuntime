using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
#if !ILRuntime
using System.Reflection;
#else
using ILRuntime.CLR.TypeSystem;
using System.IO;
#endif
using libx;

namespace ETModel
{
	public sealed class Hotfix
	{
#if ILRuntime
		public ILRuntime.Runtime.Enviorment.AppDomain appDomain;
		private MemoryStream dllStream;
		private MemoryStream pdbStream;
#else
		private Assembly assembly;
#endif

		private IStaticMethod start;

		private List<Type> hotfixTypes;

		public Action Update;
		public Action LateUpdate;
		public Action FixedUpdate;

		public Action OnApplicationQuit;

		public void GotoHotfix()
		{
#if ILRuntime
			ILHelper.InitILRuntime(this.appDomain);
#endif
			this.start.Run();
		}

		public List<Type> GetHotfixTypes()
		{
			return this.hotfixTypes;
		}

		public void LoadHotfixAssembly()
		{
			byte[] assBytes = ResMgr.LoadDll(@"Assets/Res/Code/Hotfix.dll.bytes");
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			byte[] pdbBytes = ResMgr.LoadDll(@"Assets/Res/Code/Hotfix.pdb.bytes");
			Debug.Log("开发阶段:使用.pdb");		
#endif

#if ILRuntime
			Debug.Log($"当前使用的是ILRuntime模式");
			this.appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();

			this.dllStream = new MemoryStream(assBytes);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			this.pdbStream = new MemoryStream(pdbBytes);
			this.appDomain.LoadAssembly(this.dllStream, this.pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#else
			this.appDomain.LoadAssembly(this.dllStream, null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
#endif
			this.start = new ILStaticMethod(this.appDomain, "ETHotfix.Init", "Start", 0);
			
			this.hotfixTypes = this.appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
#else
			Debug.Log($"当前使用的是Mono模式");
			try
			{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
				this.assembly = Assembly.Load(assBytes, pdbBytes);
#else
			this.assembly = Assembly.Load(assBytes, null);
#endif
			}
			catch (Exception e)
			{
				Debug.LogError("这里报错可能是Mono模式开启了IL2CPP！" + e);
			}
			Type hotfixInit = this.assembly.GetType("ETHotfix.Init");
			this.start = new MonoStaticMethod(hotfixInit, "Start");

			this.hotfixTypes = this.assembly.GetTypes().ToList();
#endif
		}


		public void Unity2Hotfix(GameObject go, Type t)
		{
#if ILRuntime
			IType classType = appDomain.GetType(t.FullName);
			IType[] genericArguments = new IType[] { classType };
			Debug.LogWarning("替换Unity域脚本为热更域脚本" + t);
			appDomain.InvokeGenericMethod("ETHotfix.Init", "Unity2Hotfix", genericArguments, null, go);
#endif
		}

	}


}