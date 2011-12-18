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
        protected string groupName = "single";
        protected dynamic oMessage = new JObject();

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

        public string group { set { this.groupName = value; } get { return this.groupName; } }
        public dynamic message { set { this.oMessage = value; } get { return this.oMessage; } }

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
