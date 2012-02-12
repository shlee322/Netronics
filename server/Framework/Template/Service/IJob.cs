namespace Netronics.Template.Service
{
    internal interface IJob
    {
        /// <summary>
        /// Job를 처리할 수 있는 서비스의 이름
        /// </summary>
        /// <returns>서비스 이름 리스트</returns>
        string[] GetService();

        /// <summary>
        /// Job 처리후 결과값을 수신 받을지 여부
        /// </summary>
        bool IsReceiveResult();

        /// <summary>
        /// Job를 처리할 Service 갯수
        /// 0일 경우 모든 Service가 처리함
        /// </summary>
        /// <returns>Job를 처리할 Service 갯수</returns>
        /// 
        uint Take();

        /// <summary>
        /// Task를 처리하는 메서드
        /// </summary>
        /// <param name="localService">Task를 받는 Service</param>
        /// <param name="task">Task</param>
        void ProcessingTask(Service localService, Task task);
    }
}