using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    /// <summary>
    /// Job란 백엔드단에서 처리되는 데이터의 가장 작은 단위이다.
    /// </summary>
    public class Job
    {
        protected string serivceName;
        protected Serivce serivce;
        protected string groupName = "all";
        protected int oTake = 0;
        protected dynamic oMessage = new JObject();
        protected dynamic oResult = new JObject();
        protected bool receiver = false;
        protected bool isReceiveResult = true;
        protected string oTransactionID = null;

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="serivce">Job를 처리할 Serivce</param>
        public Job(string serivce)
        {
            this.serivceName = serivce;
        }

        /// <summary>
        /// 새로운 Job을 생성
        /// </summary>
        /// <param name="serivce">Job을 처리할 Serivce</param>
        public Job(Serivce serivce)
        {
            this.serivce = serivce;
            this.serivceName = serivce.getSerivceName();
        }

        /// <summary>
        /// Job를 처리하는 서비스를 구하는 메서드 (기본적으로 사용안함)
        /// </summary>
        /// <returns>Job를 처리하는 서비스</returns>
        public Serivce getSerivce()
        {
            return this.serivce;
        }

        /// <summary>
        /// Job를 처리하는 서비스 이름을 구하는 메서드
        /// </summary>
        /// <returns>Job를 처리하는 서비스 이름</returns>
        public string getSerivceName()
        {
            return this.serivceName;
        }

        /// <summary>
        /// Job를 Receiver로 전환하는 메서드+ (프레임워크 내부에서만 사용)
        /// </summary>
        public void setReceiver()
        {
            if (this.serivce == null)
                throw new System.Exception("Serivce가 지정되 있지 않습니다.");

            receiver = true;
        }

        /// <summary>
        /// Job 처리후 결과값을 수신 받을지 여부
        /// </summary>
        public bool receiveResult
        {
            set
            {
                if (receiver)
                    throw new Exception.JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                this.isReceiveResult = value;
            }
            get
            {
                return isReceiveResult;
            }
        }

        /// <summary>
        /// Transaction ID (프레임워크 내부에서만 사용)
        /// </summary>
        public string transaction
        {
            set
            {
                if (!receiver)
                    throw new Exception.JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

                this.oTransactionID = value;
            }
            get
            {
                return this.oTransactionID;
            }
        }

        /// <summary>
        /// Job를 처리할 Serivce Group
        /// </summary>
        public string group
        {
            set
            {
                if (receiver)
                    throw new Exception.JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");

                this.groupName = value;
            }
            get
            {
                return this.groupName;
            }
        }

        /// <summary>
        /// Job를 처리할 Serivce 갯수
        /// 0일 경우 모든 Serivce가 처리함
        /// </summary>
        public int take
        {
            set
            {
                if (receiver)
                    throw new Exception.JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");
                this.oTake = value;
            }
            get
            {
                return this.oTake;
            }
        }

        /// <summary>
        /// Job Message
        /// </summary>
        public dynamic message
        {
            set
            {
                if (receiver)
                    throw new Exception.JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");
                
                this.oMessage = value;
            }
            get
            {
                return this.oMessage;
            }
        }

        /// <summary>
        /// Job Result
        /// </summary>
        public dynamic result {
            set
            {
                if (!receiver)
                    throw new Exception.JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");
                
                this.oResult = value;
            }
            get
            {
                return this.oResult;
            }
        }

        /// <summary>
        /// Job를 처리한 결과를 리턴하는 메서드
        /// </summary>
        /// <param name="serivce">Job를 처리한 Serivce</param>
        /// <param name="success">성공여부</param>
        public void returnResult(Serivce serivce, bool success = true)
        {
            if (!receiver)
                throw new Exception.JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

            ResultEventArgs arg = new ResultEventArgs(this, success);

            if (success)
                this.success(serivce, arg);
            else
                this.fail(serivce, arg);
        }

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
                return this.job;
            }

            /// <summary>
            /// 성공여부를 리턴하는 메서드
            /// </summary>
            /// <returns>성공여부</returns>
            public bool getSuccess()
            {
                return this.success;
            }
        }

        /// <summary>
        /// 결과를 수신 받는 Delegate
        /// </summary>
        /// <param name="sender">Job를 처리한 Serivce</param>
        /// <param name="e">Job 결과 인자값</param>
        public delegate void Result(Serivce sender, ResultEventArgs e);

        /// <summary>
        /// Job 처리 성공 이벤트
        /// </summary>
        public event Result success;
        /// <summary>
        /// Job 처리 실패 이벤트
        /// </summary>
        public event Result fail;
    }
}
