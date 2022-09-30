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
            if (string.IsNullOrEmpty(all))
            {
                Debug.LogError("读取文件失败 ---  文件路径 : " + filePath);
            }
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
            if (string.IsNullOrEmpty(all))
            {
                Debug.LogError("读取文件失败 ---  文件路径 : " + filePath);
            }
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

            all = all.Replace(below, newText);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(all);
            streamWriter.Close();
        }

        public void Dispose()
        {
        }
    }
}
