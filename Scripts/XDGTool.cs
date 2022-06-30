using System;
using TapTap.Common;
using UnityEngine;

namespace XD.SDK.Common{
    
    public class XDGTool{

        public static string userId = "";  //错误日志打印用
        
        public static void Log(string msg){
            Debug.Log("\n------------------ XDGSDK Log v640------------------\n"+msg + "\n\n");
        }
        
        public static void LogError(string msg){
            Log(msg);
            Print("userId:【" + userId + "】"  + msg);
        }
        
        private static void Print(string msg){
            Debug.LogError("\n------------------ XDGSDK Error v640------------------\n" + msg + "\n\n");
        }
        
        public static bool IsEmpty(string str){
            if (str == null){
                return true;
            }
            if (String.IsNullOrEmpty(str)){
                return true;
            }
            if (String.IsNullOrWhiteSpace(str)){
                return true;
            }
            return false;
        }
        
        public  static  bool checkResultSuccess(Result result){
            return result.code == Result.RESULT_SUCCESS && !string.IsNullOrEmpty(result.content);
        }
    }
}