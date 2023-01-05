namespace XD.SDK.Common{
    public class EventManager{
        public static void LoginSuccessEvent(){
            XDGCommonMobileImpl.GetInstance().LoginSuccessEvent();
        }

        public static void LoginFailEvent(string errorMsg){
            XDGCommonMobileImpl.GetInstance().LoginFailEvent(errorMsg);
        }
    }
}