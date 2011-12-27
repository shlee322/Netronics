using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Netronics
{
    public class Job
    {
        protected string serivceName;
        protected Serivce serivce;
        protected string groupName = "all";
        protected int oTake = 0;
        protected dynamic oMessage = new JObject();
        protected dynamic oResult = new JObject();
        protected bool receiver = false;
        protected bool isReturnResult = false;

        public Job(string serivce)
        {
            this.serivceName = serivce;
        }

        public Job(Serivce serivce)
        {
            this.serivce = serivce;
            this.serivceName = serivce.getSerivceName();
        }

        public Serivce getSerivce()
        {
            return this.serivce;
        }

        public string getSerivceName()
        {
            return this.serivceName;
        }

        public void setReceiver()
        {
            if (this.serivce == null)
                throw new System.Exception("Serivce가 지정되 있지 않습니다.");

            receiver = true;
        }

        public string getTransactionID()
        {
            return "";
        }

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

        public void returnResult(bool success = true)
        {
            if (!receiver)
                throw new Exception.JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");

            if (this.isReturnResult)
                throw new System.Exception("이미 결과를 반환한 작업입니다.");

            this.isReturnResult = true;

            if (success)
                this.success(this);
            else
                this.fail(this);
        }

        public delegate void Result(Job job);

        public event Result success;
        public event Result fail;
    }
}
