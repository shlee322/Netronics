using System;
//using log4net;

namespace Framework.Service
{
    class TestProcessor
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(TestProcessor));  
        public static void aaaa(Netronics.Template.Service.Task.Task task)
        {
            //Logger.Debug("테스트 1단계 - 메시지 수신 성공");
            TestMessage message = task.GetMessage() as TestMessage;
            if(message == null)
                throw new Exception("메시지 수신 실패!");
            //Logger.Debug(message.Name);
            //Logger.Debug(message.Test);
            task.Result(message.Name != "" ? new TestResult {Msg = "테스트 2단계 - 결과값 전송 성공"} : new TestResult {Msg = ""});
        }
    }
}
