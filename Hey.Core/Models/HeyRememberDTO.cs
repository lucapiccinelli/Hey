using System;
using System.Collections.Generic;

namespace Hey.Core.Models
{
    public class HeyRememberDto
    {
        public HeyRememberDto()
        {
            Domain = string.Empty;
            Type = string.Empty;
            Id = string.Empty;
            When = new [] { DateTime.Now };
            DomainSpecificData = string.Empty;
        }

        public string Domain { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public IEnumerable<DateTime> When { get; set; }
        public bool Recurrent { get; set; }
        public string DomainSpecificData;
    }
}