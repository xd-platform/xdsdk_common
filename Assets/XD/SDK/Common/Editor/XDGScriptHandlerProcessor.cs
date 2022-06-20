using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XD.SDK.Common.Editor
{
    public class XDGScriptHandlerProcessor : System.IDisposable
    {
        private string filePath;

        public XDGScriptHandlerProcessor(string fPath)
        {
            filePath = fPath;
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError(filePath + "路径下文件不存在");
                return;
            }
        }

        public void WriteBelow(string below, string text)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string all = streamReader.ReadToEnd();
            // 兼容不同 OS 的 Line Separators
            below = Regex.Replace(below, "\r\n", "\n", RegexOptions.IgnoreCase);
            all = Regex.Replace(all, "\r\n", "\n", RegexOptions.IgnoreCase);
            streamReader.Close();
            int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到字符串" + below);
                return;
            }

            int endIndex = all.LastIndexOf("\n", beginIndex + below.Length, StringComparison.Ordinal);
            all = all.Substring(0, endIndex) + "\n" + text + "\n" + all.Substring(endIndex);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(all);
            streamWriter.Close();
        }

        public void Replace(string below, string newText)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string all = streamReader.ReadToEnd();
            streamReader.Close();
            int beginIndex = all.IndexOf(below, StringComparison.Ordinal);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到字符串" + below);
                return;
            }

            all = all.Replace(below, newText);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(all);
            streamWriter.Close();
        }

        public void Dispose()
        {
        }
    }

    public class XDGFileHelper
    {
        public static void CopyAndReplaceDirectory(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
                Directory.Delete(dstPath,true);
            
            if (File.Exists(dstPath))
                File.Delete(dstPath);

            Directory.CreateDirectory(dstPath);
            
            //.framework文件 和 meta文件不拷贝
            foreach (var file in Directory.GetFiles(srcPath)){
                var name = Path.GetFileName(file);
                if (!name.EndsWith(".meta")){ 
                    File.Copy(file, Path.Combine(dstPath, name));   
                }
            }
            
            foreach (var dir in Directory.GetDirectories(srcPath)){
                var name = Path.GetFileName(dir);
                if (!name.EndsWith(".framework") && !name.EndsWith(".xcframework")){ 
                    CopyAndReplaceDirectory(dir, Path.Combine(dstPath, name));
                }
            }
        }

        public static string FilterFile(string srcPath,string filterName){
            if(!Directory.Exists(srcPath)){
                return null;
            }          
            foreach(var dir in Directory.GetDirectories(srcPath))
            {
                string fileName = Path.GetFileName(dir);
                if (fileName.StartsWith(filterName))
                {   
                    Debug.Log("筛选到指定文件夹:" + Path.Combine(srcPath,Path.GetFileName(dir)));
                    return Path.Combine(srcPath,Path.GetFileName(dir));
                }
            } 
            return null; 
        }

    }


}
