using System;
using modoff.Util;

namespace modoff.Attributes {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EncryptResponse : PostActionAttribute {
        const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";

        public override string Execute(string input) {
            return $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<string>{TripleDES.EncryptUnicode(input, key)}</string>";
        }
    }
}
