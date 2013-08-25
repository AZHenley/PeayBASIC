using System;
using System.Text.RegularExpressions;

namespace PeayBasic
{
    public class TokenDefinition
    {
        public TokenDefinition(
            string type,
            Regex regex)
            : this(type, regex, false)
        {
        }

        public TokenDefinition(
            string type,
            Regex regex,
            bool isIgnored)
        {
            Type = type;
            Regex = regex;
            IsIgnored = isIgnored;
        }

        public bool IsIgnored { get; private set; }
        public Regex Regex { get; private set; }
        public string Type { get; private set; }
    }
}
