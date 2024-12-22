using HarmonyLib;
using modoff.Runtime;
using UnityEngine;

namespace modoff.Patch {
    [HarmonyPatch(typeof(UtWWWAsync), "Update")]
    public class ModoffReceiverPatch {
        public static bool Prefix() {
            ResponseStore.mtx.WaitOne();
            ModoffResponse res = null;
            if (ResponseStore.responses.Count > 0) {
                res = ResponseStore.responses.Dequeue();
            }
            ResponseStore.mtx.ReleaseMutex();
            if (res != null) {
                if (res.asyncEvent == UtAsyncEvent.PROGRESS)
                    res.inCallback(UtAsyncEvent.PROGRESS, res.injectedRes);
                else if (res.asyncEvent == UtAsyncEvent.NONE)
                    Debug.Log(res.injectedRes.pData);
                else {
                    res.inCallback(res.asyncEvent, res.injectedRes);
                    ModoffLogger.Log("ModOff CallbackDispatcher: " + res.injectedRes.url + " -- callback: " + res.inCallback.Method.Name);
                    Debug.Log("ModOff Callback Dispatcher: " + res.injectedRes.url + " -- callback: " + res.inCallback.Method.Name);
                }
            }
            return true;
        }
    }
}
