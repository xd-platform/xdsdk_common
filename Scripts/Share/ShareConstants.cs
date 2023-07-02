using System;

namespace XD.SDK.Share
{
    public class ShareConstants
    {

       /**
        * 分享目标平台
        */
        public enum ShareTarget{
            WECHAT,
            QQ,
            WEIBO
        }
      /**
        * 分享目标场景
        */
        public enum ShareScene{
            //聊天窗
            SESSION,
            //朋友圈或空间
            TIMELINE
        }
      /**
        * 分享失败错误码
        */
        public enum ShareErrorCode{
            //无效参数，包括参数非法值、对应文件不存在等
            INVALID_PARAMS = -1,
            //对应目标平台参数未配置
            NOT_INIT = -2,
            //对应目标平台未安装
            TARGET_NOT_INSTALLED = -3,
            //其他错误
            OTHER = -4
        }
    }
}