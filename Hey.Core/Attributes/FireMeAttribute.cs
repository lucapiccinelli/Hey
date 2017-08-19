using System;

namespace Hey.Core.Attributes
{
    public class FireMeAttribute : Attribute
    {
        public string Id { get; }

        public FireMeAttribute(string id)
        {
            Id = id;
        }
    }
}