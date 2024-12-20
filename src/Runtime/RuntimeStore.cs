using System.Collections.Generic;
using modoff.Model;
using modoff.Controllers;

namespace modoff.Runtime {
    public static class RuntimeStore {
        public static DBContext ctx;
        public static Dispatcher dispatcher;

        public static void Init() {
            ctx = new DBContext();
            ctx.Database.EnsureCreated();
            var controllers = new List<Controller> {
                new RegistrationController(ctx)
            };
            dispatcher = new Dispatcher(controllers);
        }
    }
}
