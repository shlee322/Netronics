using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Service.Coroutine
{
    class Co
    {
        [ThreadStatic]
        public static Coroutine Current;

        // 중간에서 yield할 수 있는 로직은 반드시 yield return Co.Call 형태를 사용해야 한다.
        public static IYield Call(IEnumerator<IYield> e)
        {
            return new NestedYield(e);
        }

        // k초 뒤에 현재 코루틴을 재개하도록 스케줄링
        // 이런 식으로 IO 함수도 만들 수 있다.
        // IO가 완료되면 실행하도록 콜백을 등록하게 하면 됨

        /*
        public static IEnumerator<IYield> Sleep(int t)
        {
            Coroutine c = Current;
            /*Debug.Assert(c != null);

            // 스케줄러 복잡하게 짜기 귀찮으니 대충 Sleep했다고 치고 실행 큐 마지막에 등록
            Scheduler.Schedule(
                delegate() { c.Resume(); },
                t);///

            yield return new NonNestedYield();
        }
        */
    }

}
