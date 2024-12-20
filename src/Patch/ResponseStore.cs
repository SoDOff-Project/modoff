using System.Collections.Generic;
using System.Threading;

namespace modoff.Patch {
    public static class ResponseStore {
        public static Queue<ModoffResponse> responses = new Queue<ModoffResponse>();
        public static Mutex mtx = new Mutex();
    }
}
