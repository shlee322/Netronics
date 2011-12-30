using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netronics
{
    public interface Serivce
    {
        string getSerivceName();

        void init();
        void start();
        void stop();

        bool isGroup(string name);
        string[] getGroupArray();
        float getLoad();

        void processingJob(Serivce serivce, Job job);
    }
}
