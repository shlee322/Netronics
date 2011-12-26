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

        public Job(string serivce)
        {
            this.serivceName = serivce;
        }

        public Job(Serivce serivce)
        {
            this.serivce = serivce;
            this.serivceName = serivce.getSerivceName();
        }

        public string getSerivceName()
        {
            return this.serivceName;
        }

        public void setReceiver()
        {
            receiver = true;
        }

        public string group
        {
            set
            {
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
                if (!receiver)
                    this.oMessage = value;
                else
                    throw new Exception.JobPermissionException("Sender가 아니므로 메시지를 편집 할 수 없습니다.");
            }
            get
            {
                return this.oMessage;
            }
        }

        public dynamic result {
            set
            {
                if(receiver)
                    this.oResult = value;
                throw new Exception.JobPermissionException("Receiver가 아니므로 결과값을 편집 할 수 없습니다.");
            }
            get
            {
                return this.oResult;
            }
        }

        public void callSuccess()
        {
            this.success(this);
        }

        public void callFail()
        {
            this.fail(this);
        }


        public delegate void Result(Job job);

        public event Result success;
        public event Result fail;
    }
}
