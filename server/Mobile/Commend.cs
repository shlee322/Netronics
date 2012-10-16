using System;
using System.Collections.Generic;

namespace Netronics.Mobile
{
    class Commend
    {
        private List<CommendAction> _actions = new List<CommendAction>();

        public Commend(string name)
        {
        }

        public void AddCmd(Action<Request> action, int minVer)
        {
            _actions.Add(new CommendAction(){Action = action, MinVer = minVer});
        }

        public void Sort()
        {
            _actions.Sort((ca1, ca2) => ca2.MinVer - ca1.MinVer);
        }

        public void Run(Request request)
        {
            foreach (var action in _actions)
            {
                if(request.Client.Ver >= action.MinVer)
                {
                    action.Action(request);
                    return;
                }
            }
        }
    }
}
