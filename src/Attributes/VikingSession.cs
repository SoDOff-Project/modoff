using modoff.Model;
using modoff.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace modoff.Attributes {
    public class VikingSession : Attribute {
        public enum Modes { VIKING, USER, VIKING_OR_USER };

        public string ApiToken { get; set; } = "apiToken";
        public Modes Mode { get; set; } = Modes.VIKING;
        public bool UseLock = false;

        public bool Execute(Dictionary<string, string> request, out Viking? viking, out User? user) {
            viking = null;
            user = null;
            if (!request.ContainsKey(ApiToken))
                return false;
            Session? session = RuntimeStore.ctx.Sessions.FirstOrDefault(x => x.ApiToken == Guid.Parse(request[ApiToken]));

            // get viking / user id from session

            string? userVikingId = null;
            if (Mode == Modes.VIKING || (Mode == Modes.VIKING_OR_USER && session?.UserId is null)) {
                userVikingId = session?.VikingId?.ToString();
            } else {
                userVikingId = session?.UserId.ToString();
            }

            if (userVikingId is null) {
                return false;
            }

            // call next (with lock if requested)

            if (UseLock) {
                // NOTE: we can't refer to session.User / session.Viking here,
                //       because it may cause to ignore modifications from the threads we are waiting for
                //       we can use its only after vikingMutex.WaitOne()

                Mutex vikingMutex = new Mutex(false, "SoDOffViking:" + userVikingId);
                try {
                    vikingMutex.WaitOne();
                    user = session.User;
                    viking = session.Viking;
                    return true;
                } finally {
                    vikingMutex.ReleaseMutex();
                }
            } else {
                user = session.User;
                viking = session.Viking;
                return true;
            }
        }
    }
}
