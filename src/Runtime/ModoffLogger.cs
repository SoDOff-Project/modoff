using System;
using System.Collections.Generic;

namespace modoff.Runtime {
    public static class ModoffLogger {
        public static LinkedList<string> messages { get; private set; } = new LinkedList<string>();
        public static void Log(string message) {
            Console.WriteLine(message);
            messages.AddLast(message);
            if (messages.Count > 10)
                messages.RemoveFirst();
        }
    }
}
