using System;
using System.Collections.Generic;
using System.Linq;

namespace Hey.Core.Models
{
    public class HeyRememberDto
    {
        public HeyRememberDto()
        {
            Domain = string.Empty;
            Type = string.Empty;
            Name = string.Empty;
            Id = string.Empty;
            When = new [] { DateTime.Now };
            DomainSpecificData = string.Empty;
        }


        public HeyRememberDto(HeyRememberDto other)
        {
            Domain = other.Domain;
            Type = other.Type;
            Name = other.Name;
            Id = other.Id;
            When = new DateTime[other.When.Length];
            other.When.CopyTo(When, 0);
            DomainSpecificData = other.DomainSpecificData;
            Recurrent = other.Recurrent;
        }

        public override string ToString()
        {
            return $"HeyRember {Domain}/{Type}/{Name}/{Id} on {When[0]}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            HeyRememberDto o = (HeyRememberDto) obj;
            return o.Domain == Domain &&
                   o.Type == Type &&
                   o.Name == Name &&
                   o.Id == Id &&
                   o.When.SequenceEqual(When) &&
                   o.DomainSpecificData == DomainSpecificData &&
                   o.Recurrent == Recurrent;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Id { get; set; }
        public string Domain { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime[] When { get; set; }
        public bool Recurrent { get; set; }
        public string DomainSpecificData;
    }
}