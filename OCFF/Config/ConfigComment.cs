using System;
using System.Collections.Generic;
using System.Text;

namespace OCFF
{
    public class ConfigComment
    {
        public string Comment { get; }

        public ConfigComment(string commment)
        {
            Comment = commment;
        }

        public string Print() => Comment;
    }
}
