using System.Collections.Generic;
using modoff.Model;

namespace modoff.Runtime {
    public static class RuntimeStore {
        public static DBContext ctx;

        public static void Init() {
            ctx = new DBContext();
            ctx.Database.EnsureCreated();
        }
    }
}
