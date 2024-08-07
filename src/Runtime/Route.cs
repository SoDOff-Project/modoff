using System;

namespace modoff.Runtime {
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal class Route : Attribute {
        public string Url { get; }

        public Route(string url) {
            Url = url;
        }
    }
}
