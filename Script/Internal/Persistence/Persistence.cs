﻿#if UNITY_EDITOR || UNITY_STANDALONE
using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using LC.Newtonsoft.Json;

namespace XD.SDK.Common.PC.Internal {
    /// <summary>
    /// 通用 JSON 序列化工具
    /// </summary>
    internal class Persistence {
        private readonly string filePath;

        internal Persistence(string path) {
            filePath = path;
        }

        internal async Task<T> Load<T>() where T : class {
            if (!File.Exists(filePath)) {
                return null;
            }

            string text;
            using (FileStream fs = File.OpenRead(filePath)) {
                byte[] buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, (int)fs.Length);
                text = Encoding.UTF8.GetString(buffer);
            }
            try {
                return JsonConvert.DeserializeObject<T>(text);
            } catch (Exception e) {
                XDLogger.Error(e);
                Delete();
                return null;
            }
        }

        internal async Task Save<T>(T obj) {
            if (obj == null) {
                XDLogger.Error("Saved object is null.");
                return;
            }

            string text;
            try {
                text = JsonConvert.SerializeObject(obj);
            } catch (Exception e) {
                XDLogger.Error(e);
                return;
            }

            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }

            using (FileStream fs = File.Create(filePath)) {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        internal void Delete() {
            if (!File.Exists(filePath)) {
                return;
            }

            File.Delete(filePath);
        }
    }
}
#endif