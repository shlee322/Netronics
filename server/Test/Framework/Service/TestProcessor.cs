using System;

namespace Framework.Service
{
    class TestProcessor
    {
        public static void aaaa(Netronics.Template.Service.Task.Task task)
        {
            System.Console.WriteLine("테스트 1단계 - 메시지 수신 성공");
            TestMessage message = task.GetMessage() as TestMessage;
            if(message == null)
                throw new Exception("메시지 수신 실패!");
            Console.WriteLine(message.Name);
            Console.WriteLine(message.Test);
            task.Result(message.Name != "" ? new TestResult {Msg = "테스트 2단계 - 결과값 전송 성공"} : new TestResult {Msg = ""});
        }
    }
}
