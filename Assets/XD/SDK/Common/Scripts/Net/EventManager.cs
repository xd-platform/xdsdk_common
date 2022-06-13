namespace XD.SDK.Common{
    public class EventManager{
        public static void LoginSuccessEvent(){
            XDGCommonImpl.GetInstance().LoginSuccessEvent();
        }

        public static void LoginFailEvent(string errorMsg){
            XDGCommonImpl.GetInstance().LoginFailEvent(errorMsg);
        }
    }
}