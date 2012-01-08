using System;
using System.Threading;
using System.Threading.Tasks;
using Netronics.Exception;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    /// <summary>
    /// Job란 백엔드단에서 처리되는 데이터의 가장 작은 단위이다.
    /// </summary>
    public class Job : IDisposable
    {
        #region Delegates

        /// <summary>
        /// 결과를 수신 받는 Delegate
        /// </summary>
        /// <param name="sender">Job를 처리한 Service</param>
        /// <param name="e">Job 결과 인자값</param>
        public delegate void Result(Service sender, ResultEventArgs e);

        #endregion

        protected bool disposed;

        protected string groupName = "all";
        protected bool isReceiveResult = true;
        protected dynamic oMessage = new JObject();
        protected dynamic oResult = new JObject();
        protected int oTake;
        protected string oTransactionID;

        protected int processorCount;
        protected bool receiver;
        protected Service service;
        protected string serviceName;

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="Service">Job를 처리할 Service</param>
        public Job(string service)
        {
            serviceName = service;
        }

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="Service">Job을 처리할 Service</param>
        public Job(Service service)
        {
            this.service = service;
            serviceName = service.getServiceName();
        }

        /// <summary>
        /// Job 처리후 결과값을 수신 받을지 여부
        /// </summary>
        public bool receiveResult
        {
            set
            {
                if (receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                isReceiveResult = value;
            }
            get { return isReceiveResult; }
        }

        /// <summary>
        /// Transaction ID (프레임워크 내부에서만 사용)
        /// </summary>
        public string transaction
        {
            set
            {
                if (!receiver)
                    throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                oTransactionID = value;
            }
            get { return oTransactionID; }
        }

        /// <summary>
        /// Job를 처리할 Service Group
        /// </summary>
        public string group
        {
            set
            {
                if (receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                groupName = value;
            }
            get { return groupName; }
        }

        /// <summary>
        /// Job를 처리할 Service 갯수
        /// 0일 경우 모든 Service가 처리함
        /// </summary>
        public int take
        {
            set
            {
                if (receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");
                oTake = value;
            }
            get { return oTake; }
        }

        /// <summary>
        /// Job Message
        /// </summary>
        public dynamic message
        {
            set
            {
                if (receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                oMessage = value;
            }
            get { return oMessage; }
        }

        /// <summary>
        /// Job Result
        /// </summary>
        public dynamic result
        {
            set
            {
                if (!receiver)
                    throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                oResult = value;
            }
            get { return oResult; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    service = null;
                    oMessage = null;
                    oResult = null;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Job를 처리하는 서비스를 구하는 메서드 (기본적으로 사용안함)
        /// </summary>
        /// <returns>Job를 처리하는 서비스</returns>
        public Service getService()
        {
            return service;
        }

        /// <summary>
        /// Job를 처리하는 서비스 이름을 구하는 메서드
        /// </summary>
        /// <returns>Job를 처리하는 서비스 이름</returns>
        public string getServiceName()
        {
            return serviceName;
        }

        /// <summary>
        /// Job를 Receiver로 전환하는 메서드+ (프레임워크 내부에서만 사용)
        /// </summary>
        public void setReceiver()
        {
            receiver = true;
        }

        /// <summary>
        /// Job를 처리한 결과를 리턴하는 메서드
        /// </summary>
        /// <param name="Service">Job를 처리한 Service</param>
        /// <param name="success">성공여부</param>
        public void returnResult(Service service, bool success = true)
        {
            Parallel.Invoke(() =>
                                {
                                    if (!receiver)
                                        throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                                    var arg = new ResultEventArgs(this, success);

                                    if (success)
                                        this.success(service, arg);
                                    else
                                        fail(service, arg);

                                    if (Interlocked.Decrement(ref processorCount) < 1)
                                        Dispose();
                                });
        }

        public void addProcessor()
        {
            Interlocked.Increment(ref processorCount);
        }

        /// <summary>
        /// Job 처리 성공 이벤트
        /// </summary>
        public event Result success;

        /// <summary>
        /// Job 처리 실패 이벤트
        /// </summary>
        public event Result fail;

        #region Nested type: ResultEventArgs

        /// <summary>
        /// Job 결과 이벤트 인자
        /// </summary>
        public class ResultEventArgs : EventArgs
        {
            protected Job job;
            protected bool success;

            /// <summary>
            /// Job 결과 이벤트 인자 생성
            /// </summary>
            /// <param name="job">처리된 Job</param>
            /// <param name="success">성공여부</param>
            public ResultEventArgs(Job job, bool success)
            {
                this.job = job;
                this.success = success;
            }

            /// <summary>
            /// 처리된 Job를 구하는 메서드
            /// </summary>
            /// <returns>처리된 Job</returns>
            public Job getJob()
            {
                return job;
            }

            /// <summary>
            /// 성공여부를 리턴하는 메서드
            /// </summary>
            /// <returns>성공여부</returns>
            public bool getSuccess()
            {
                return success;
            }
        }

        #endregion
    }
}