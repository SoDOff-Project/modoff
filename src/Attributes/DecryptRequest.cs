using System;
using modoff.Util;

namespace modoff.Attributes {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DecryptRequest : PreActionAttribute {
        const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";

        public DecryptRequest(string field) : base(field) {}

        public override string Execute(string input) {
            return TripleDES.DecryptUnicode(input, key);
        }
    }
}