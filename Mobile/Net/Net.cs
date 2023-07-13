using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace XD.SDK.Common{
    [DisallowMultipleComponent]
    public class Net : MonoBehaviour{
        private static GameObject netObject;

        public static void GetRequest(string url,
            Dictionary<string, object> parameters,
            Action<string> methodForResult,
            Action<int, string> methodForError){
            if (string.IsNullOrEmpty(url)){
                methodForError(-1, "empty url");
                return;
            }

            Net nt = Instance();
            nt.StartCoroutine(nt.Get(url, parameters, methodForResult, methodForError));
        }

        public static void GetRequest(string url,
            Action<string> methodForResult,
            Action<int, string> methodForError){
            if (string.IsNullOrEmpty(url)){
                methodForError(-1, "empty url");
                return;
            }

            Net nt = Instance();
            nt.StartCoroutine(nt.Get(url, null, methodForResult, methodForError));
        }

        public static void PostRequest(string url,
            Dictionary<string, object> parameters,
            Action<string> methodForResult,
            Action<int, string> methodForError){
            if (string.IsNullOrEmpty(url)){
                methodForError(-1, "empty url");
                return;
            }

            Net nt = Instance();
            nt.StartCoroutine(nt.Post(url, null, parameters, methodForResult, methodForError));
        }

        public static void PostRequest(string url, Dictionary<string, string> headers,
            Dictionary<string, object> parameters,
            Action<string> methodForResult,
            Action<int, string> methodForError){
            if (string.IsNullOrEmpty(url)){
                methodForError(-1, "empty url");
                return;
            }

            Net nt = Instance();
            nt.StartCoroutine(nt.Post(url, headers, parameters, methodForResult, methodForError));
        }

        private static Net Instance(){
            if (netObject == null){
                netObject = new GameObject{
                    name = "XDGSDKNet"
                };
                netObject.AddComponent<Net>();
                DontDestroyOnLoad(netObject);
            }

            return netObject.GetComponent<Net>();
        }

        private IEnumerator Post(string url, Dictionary<string, string> headers, Dictionary<string, object> parameters,
            Action<string> methodForResult, Action<int, string> methodForError){
            string finalUrl = url;
            Dictionary<string, object> finalParameter = parameters ?? new Dictionary<string, object>();

            String jsonString = MiniJSON.Json.Serialize(finalParameter);
            Byte[] formData = Encoding.UTF8.GetBytes(jsonString);

            UnityWebRequest w = UnityWebRequest.Post(finalUrl, "");
            w.uploadHandler = new UploadHandlerRaw(formData);
            w.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            w.timeout = 6;

            if (headers != null){
                Dictionary<string, string>.KeyCollection keys = headers.Keys;
                foreach (string headerKey in keys){
                    w.SetRequestHeader(headerKey, headers[headerKey]);
                }
            }

            yield return w.SendWebRequest();

            if (!string.IsNullOrEmpty(w.error)){
                XDGTool.Log("数据失败：\n" + finalUrl + "\n\n" + w.downloadHandler.text);
                XDGTool.Log("ERROR 信息：" + w.error);
                methodForError(-1, "Network Error");
                w.Dispose();
                yield break;
            } else{
                string data = w.downloadHandler.text;
                if (data != null){
                    XDGTool.Log("发起Post请求：" + finalUrl + "\n\nbody参数：" + jsonString + "\n\n响应结果：" + data);
                    methodForResult(data);
                    yield break;
                } else{
                    XDGTool.Log("请求失败，response 为空。url: " + finalUrl);
                    methodForError(-1, "Network Error");
                }
            }
        }

        private IEnumerator Get(string url, Dictionary<string, object> parameters,
            Action<string> methodForResult,
            Action<int, string> methodForError){
            string finalUrl = url + "?" + DictToQueryString(parameters);

            UnityWebRequest w = UnityWebRequest.Get(finalUrl);
            w.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            w.timeout = 6;

            yield return w.SendWebRequest();

            if (!string.IsNullOrEmpty(w.error)){
                XDGTool.Log("数据失败：\n" + finalUrl + "\n\n" + w.downloadHandler.text + "\n\n失败信息:" + w.error);
                methodForError(-1, "Network Error");
                w.Dispose();
                yield break;
            } else{
                string data = w.downloadHandler.text;
                if (data != null){
                    XDGTool.Log("发起Get请求：" + finalUrl + "\n\n响应结果：" + data);
                    methodForResult(data);
                    yield break;
                } else{
                    XDGTool.Log("请求失败，response 为空。url: " + finalUrl);
                    methodForError(-1, "Network Error");
                }
            }
        }

        private static string DictToQueryString(IDictionary<string, object> dict){
            if (dict == null){
                return "";
            }

            List<string> list = new List<string>();
            foreach (var item in dict){
                list.Add(item.Key + "=" + item.Value);
            }

            return string.Join("&", list.ToArray());
        }

        private static string DictToQueryString2(IDictionary<string, string> dict){
            if (dict == null){
                return "";
            }

            List<string> list = new List<string>();
            foreach (var item in dict){
                list.Add(item.Key + "=" + item.Value);
            }

            return string.Join("&", list.ToArray());
        }
    }
}