using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire.Annotations;

namespace Hey.Api.Rest.Models
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