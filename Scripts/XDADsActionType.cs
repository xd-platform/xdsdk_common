namespace XD.SDK.ADSubPackage
{
    public enum XDADsActionType
    {
        /**
     * 应用启动 SDK要求在Activity的onResume方法中，而不是onCreate方法中去上报App启动行为
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        START_APP = 0,

        /**
     * 页面浏览
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        PAGE_VIEW = 3,

        /**
     * 注册
     * 头条参数:String method (注册⽅方式 mobile、weixin、qq等,必须上传,String isSuccess (是否成功,必须上传)
     * 广点通参数不限制
     * 《广点通和头条都支持》
     */
        REGISTER = 20,

        /**
     * 内容浏览
     * 头条参数：String type(内容类型, String name(内容名称, String contentID(内容Id)
     * 广点通参数不限制
     * 《广点通和头条都支持》
     */
        VIEW_CONTENT = 21,

        /**
     * 咨询
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        CONSULT = 4,

        /**
     * 加入购物车
     * 头条参数：String type(商品类型, String name(商品名称, String contentID(商品Id, String number(商品数量,String isSuccess(是否成功，"1"代表成功)
     * 广点通参数不限制
     * 《广点通和头条都支持》
     */
        ADD_TO_CART = 22,

        /**
     * 付费
     * 头条参数：String type(内容类型,非必传,String name(商品/内容名,非必传,String contentID(商品ID/内容ID,非必传,
     * String number(商品数量,非必传,String channel(支付渠道名 如支付宝、微信等,非必传,String currency(真实货币类型,非必传,
     * String isSuccess(是否成功，"1"表示成功,必须上传,String amount(本次支付的真实金额(必须上传，单位:⼈⺠币元，不能为"0"))
     * 广点通不限制
     * 《广点通和头条都支持》
     */
        PURCHASE = 24,

        /**
     * 搜索
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        SEARCH = 5,

        /**
     * 加入收藏
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        ADD_TO_WISHLIST = 6,

        /**
     * 开始结算/提交购买
     * 头条参数：String type(商品类型,String name(商品名称,String contentID(商品Id,
     * String number(商品数量,String isVirtualCurrency(是否是虚拟货币"1"代表是,
     * String virtualCurrency(货币名称,String currency(真实货币名称,
     * String isSuccess(是否成功"1"代表成功, String amount(本次支付的金额)
     * 广点通参数不做限制
     * 《广点通和头条都支持》
     */
        INITIATE_CHECKOUT = 23,

        /**
     * 下单
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        COMPLETE_ORDER = 2,

        /**
     * 下载应用
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        DOWNLOAD_APP = 7,

        /**
     * 评分
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        RATE = 8,

        /**
     * 预定
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        RESERVATION = 9,

        /**
     * 分享
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        SHARE = 1,

        /**
     * 申请，用于金融广告主申请贷款、开卡等
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        APPLY = 10,

        /**
     * 领取卡券，用于web落地页领取卡券等优惠信息的行为
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        CLAIM_OFFER = 11,

        /**
     * 导航，用于web落地页点击跳转到地图的行为
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        NAVIGATE = 12,

        /**
     * 商品推荐，动态创意客户直接传算好的推荐结果时使用
     * 参数不做限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        PRODUCT_RECOMMEND = 13,

        /**
     * 登录
     * 头条参数:String method (注册⽅方式 mobile、weixin、qq等,必须上传,String isSuccess (是否成功,"1"表示成功，必须上传)
     * 广点通参数不限制
     * 《广点通当做自定义事件处理，头条默认支持》
     */
        LOGIN = 14,

        /**
     * 停留时长统计(与ON_PAUSE结合使用，****仅头条SDK有效***)，需要在每个Activity的onResume方法中调⽤
     * <--参数类型--> Context
     * 《仅头条支持》
     */
        ON_RESUME = 25,

        /**
     * 停留时长统计(与ON_RESUME结合使用，****仅头条SDK有效***)，需要在每个Activity的onPause方法中调⽤
     * <--参数类型--> Context
     * 《仅头条支持》
     */
        ON_PAUSE = 26,

        /**
     * 绑定账号
     * 头条参数:String type(账号类型,String isSuccess(是否成功,"1"表示成功)
     * 广点通参数不限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        ACCESS_ACCOUNT = 18,

        /**
     * 完成节点（如教学任务、副本等）
     * 头条参数：String questID(节点Id,String name(节点名称,String type(节点类型,
     * String number(节点编号,String isSuccess(是否成功,"1"表示成功, String desc(节点描述)
     * 广点通参数不限制
     * 《广点通当做自定义事件处理，头条默认支持》
     */
        QUEST = 16,

        /**
     * 付费渠道
     * 头条参数:String channel(付费渠道, String isSuccess(是否成功,"1"表示成功)
     * 广点通参数不限制
     * 《广点通默认支持，头条当做自定义事件处理》
     */
        ACCESS_PAYMENT_CHANNEL = 17,

        /**
     * 升级
     * 头条参数：String level(等级,如"12"，"13")
     * 广点通参数不限制
     * 《广点通当做自定义事件处理，头条默认支持》
     */
        UPDATE_LEVEL = 15,

        /**
     * 设置用户唯一标识
     * 头条参数：String userUniqueId(用户标识)
     * 广点通参数不限制
     * 《广点通当做自定义事件处理，头条默认支持》
     */
        USER_UNIQUE_ID = 19
    }
}