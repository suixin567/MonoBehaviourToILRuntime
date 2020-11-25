#if ILRuntime
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using System.Text;
using System.Security.Cryptography;

public static class ILRuntimeCLRBinding
{
    [MenuItem("Tools/ILRuntime/Generate CLR Binding Code by Analysis")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (FileStream fs = new FileStream("Assets/Res/Code/Hotfix.dll.bytes", FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            var data2 = RijndaelDecrypt("11111111111111111111111111111111", bytes);
            MemoryStream ms = new MemoryStream(data2);

            domain.LoadAssembly(ms);
	        //Crossbind Adapter is needed to generate the correct binding code
	        ILHelper.InitILRuntime(domain);
	        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Model/ILBinding");
            //注释反射绑定
            string filePath = "Assets/Model/ILBinding/CLRBindings.cs";
            var oldStr = File.ReadAllText(filePath);
            var newStr = oldStr.Replace("System_Reflection_Assembly_Binding.Register(app);", "");
            File.WriteAllText(filePath, newStr);
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="key"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] RijndaelDecrypt(string key, byte[] data)
    {
        byte[] keybytes = Encoding.UTF8.GetBytes(key);
        byte[] strbytes = data;
        //解密
        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Key = keybytes;
        rijndael.Mode = CipherMode.ECB;
        rijndael.Padding = PaddingMode.PKCS7;
        ICryptoTransform crypto = rijndael.CreateDecryptor();
        //解密后的数据
        byte[] bytes = crypto.TransformFinalBlock(strbytes, 0, strbytes.Length);
        return bytes;
    }

}
#endif
