using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Models
{
    public class BrainstormInput
    {
        public string Topic { get; set; }
        public string Context { get; set; }
        public List<string> Goals { get; set; } = new();
        public List<string> Preferences { get; set; } = new();
        public List<string> Tags { get; set; } = new();

        public int Idea_count { get; set; }
    }

    public class BrainstormExamplesOuput
    {
        public List<string> Examples { get; set; }
    }
}
