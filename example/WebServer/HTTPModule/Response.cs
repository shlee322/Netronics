using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPModule
{
    class Response
    {
        private string _content;

        public void SetContent(string content)
        {
            _content = content;
        }

        public string GetContent()
        {
            return _content;
        }
    }
}
