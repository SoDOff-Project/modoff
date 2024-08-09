using System;

namespace modoff.Attributes {
    public abstract class PostActionAttribute : Attribute {
        public abstract string Execute(string input);
    }
}
