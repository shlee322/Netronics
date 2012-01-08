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
        public delegate void ResultDelegate(Service sender, ResultEventArgs e);

        #endregion

        protected bool Disposed;

        protected string GroupName = "all";
        protected bool IsReceiveResult = true;
        protected dynamic _Message = new JObject();
        protected dynamic _Result = new JObject();
        protected int _Take;
        protected string _TransactionID;

        protected int ProcessorCount;
        protected bool Receiver;
        protected Service Service;
        protected string ServiceName;

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="service">Job를 처리할 Service</param>
        public Job(string service)
        {
            ServiceName = service;
        }

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="service">Job을 처리할 Service</param>
        public Job(Service service)
        {
            this.Service = service;
            ServiceName = service.GetServiceName();
        }

        /// <summary>
        /// Job 처리후 결과값을 수신 받을지 여부
        /// </summary>
        public bool ReceiveResult
        {
            set
            {
                if (Receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                IsReceiveResult = value;
            }
            get { return IsReceiveResult; }
        }

        /// <summary>
        /// Transaction ID (프레임워크 내부에서만 사용)
        /// </summary>
        public string Transaction
        {
            set
            {
                if (!Receiver)
                    throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                _TransactionID = value;
            }
            get { return _TransactionID; }
        }

        /// <summary>
        /// Job를 처리할 Service Group
        /// </summary>
        public string Group
        {
            set
            {
                if (Receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                GroupName = value;
            }
            get { return GroupName; }
        }

        /// <summary>
        /// Job를 처리할 Service 갯수
        /// 0일 경우 모든 Service가 처리함
        /// </summary>
        public int Take
        {
            set
            {
                if (Receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");
                _Take = value;
            }
            get { return _Take; }
        }

        /// <summary>
        /// Job Message
        /// </summary>
        public dynamic Message
        {
            set
            {
                if (Receiver)
                    throw new JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                _Message = value;
            }
            get { return _Message; }
        }

        /// <summary>
        /// Job Result
        /// </summary>
        public dynamic Result
        {
            set
            {
                if (!Receiver)
                    throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                _Result = value;
            }
            get { return _Result; }
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
            if (!Disposed)
            {
                if (disposing)
                {
                    Service = null;
                    _Message = null;
                    _Result = null;
                }

                Disposed = true;
            }
        }

        /// <summary>
        /// Job를 처리하는 서비스를 구하는 메서드 (기본적으로 사용안함)
        /// </summary>
        /// <returns>Job를 처리하는 서비스</returns>
        public Service GetService()
        {
            return Service;
        }

        /// <summary>
        /// Job를 처리하는 서비스 이름을 구하는 메서드
        /// </summary>
        /// <returns>Job를 처리하는 서비스 이름</returns>
        public string GetServiceName()
        {
            return ServiceName;
        }

        /// <summary>
        /// Job를 Receiver로 전환하는 메서드+ (프레임워크 내부에서만 사용)
        /// </summary>
        public void SetReceiver()
        {
            Receiver = true;
        }

        /// <summary>
        /// Job를 처리한 결과를 리턴하는 메서드
        /// </summary>
        /// <param name="service">Job를 처리한 Service</param>
        /// <param name="success">성공여부</param>
        public void ReturnResult(Service service, bool success = true)
        {
            Parallel.Invoke(() =>
                                {
                                    if (!Receiver)
                                        throw new JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                                    var arg = new ResultEventArgs(this, success);

                                    if (success)
                                        this.Success(service, arg);
                                    else
                                        Fail(service, arg);

                                    if (Interlocked.Decrement(ref ProcessorCount) < 1)
                                        Dispose();
                                });
        }

        public void AddProcessor()
        {
            Interlocked.Increment(ref ProcessorCount);
        }

        /// <summary>
        /// Job 처리 성공 이벤트
        /// </summary>
        public event ResultDelegate Success;

        /// <summary>
        /// Job 처리 실패 이벤트
        /// </summary>
        public event ResultDelegate Fail;

        #region Nested type: ResultEventArgs

        /// <summary>
        /// Job 결과 이벤트 인자
        /// </summary>
        public class ResultEventArgs : EventArgs
        {
            protected Job Job;
            protected bool Success;

            /// <summary>
            /// Job 결과 이벤트 인자 생성
            /// </summary>
            /// <param name="job">처리된 Job</param>
            /// <param name="success">성공여부</param>
            public ResultEventArgs(Job job, bool success)
            {
                Job = job;
                Success = success;
            }

            /// <summary>
            /// 처리된 Job를 구하는 메서드
            /// </summary>
            /// <returns>처리된 Job</returns>
            public Job GetJob()
            {
                return Job;
            }

            /// <summary>
            /// 성공여부를 리턴하는 메서드
            /// </summary>
            /// <returns>성공여부</returns>
            public bool GetSuccess()
            {
                return Success;
            }
        }

        #endregion
    }
}