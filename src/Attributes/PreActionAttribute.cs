using System;

namespace modoff.Attributes {
    public abstract class PreActionAttribute : Attribute {
        public string Field { get; }
        public PreActionAttribute(string field) {
            this.Field = field;
        }

        public abstract string Execute(string input);
    }
}
