using System;
using System.Text;
using TapTap.Common;
using UnityEngine;
using Random = System.Random;

namespace XD.SDK.Common
{
    public class XDGTool
    {
        public static string userId = ""; //错误日志打印用
        private static string xdid = "";

        public static void Log(string msg)
        {
            Debug.Log("\n------------------ XDGSDK Log V6.13.2------------------\n" + msg + "\n\n");
        }

        public static void LogError(string msg)
        {
            if (string.IsNullOrEmpty(xdid))
            {
                try
                {
                    XDGCommonMobileImpl.GetInstance().GetDid(did =>
                    {
                        xdid = did;
                        Print($"userId:【{userId}】, did: 【{xdid}】, msg:{msg}");
                    });
                }
                catch (Exception e)
                {
                    Print($"userId:【{userId}】, msg:{msg}。 get did error:{e.Message}");
                }
            }
            else
            {
                Print($"userId:【{userId}】, did: 【{xdid}】, msg:{msg}");
            }
        }

        private static void Print(string msg)
        {
            Debug.LogError("\n------------------ XDGSDK Error V6.13.2------------------\n" + msg + "\n\n");
        }

        public static bool IsEmpty(string str)
        {
            if (str == null)
            {
                return true;
            }

            if (String.IsNullOrEmpty(str))
            {
                return true;
            }

            if (String.IsNullOrWhiteSpace(str))
            {
                return true;
            }

            return false;
        }

        public static bool checkResultSuccess(Result result)
        {
            return result.code == Result.RESULT_SUCCESS && !string.IsNullOrEmpty(result.content);
        }

        private static string LetterStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GetRandomStr(int length)
        {
            StringBuilder SB = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                SB.Append(LetterStr.Substring(rd.Next(0, LetterStr.Length), 1));
            }

            return SB.ToString();
        }
    }
}