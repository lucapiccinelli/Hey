using System;
using System.Collections.Generic;

namespace Hey.Core.Models
{
    public class HeyRememberDto
    {
        public string Domain { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public IEnumerable<DateTime> When { get; set; }
        public string DomainSpecificData;
    }
}