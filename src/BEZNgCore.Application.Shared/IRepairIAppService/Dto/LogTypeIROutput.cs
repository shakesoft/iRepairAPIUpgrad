using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BEZNgCore.IRepairIAppService.Dto
{
    public class LogTypeIROutput
    {
        public string LogType { get; set; }
        public string Value { get; set; }
        public LogTypeIROutput(string logtype, string value) => (LogType, Value) = (logtype, value);
    }
    public class PageListItem
    {
        public bool Enabled;
        public string Text;
        public string Value;
        public PageListItem(string text, string value, bool enabled) => (Text, Value, Enabled) = (text, value, enabled);
        
    }
}
