using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot
{
    public class Settings
    {
        public string Token { get; set; }

        public string[] Prefixes { get; set; } = new string[] { "/" };

        public bool EnableDMs { get; set; } = false;

        public bool CaseSensitive { get; set; } = true;

        public Settings()
        {
        }

    }
}
