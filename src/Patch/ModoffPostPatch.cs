using HarmonyLib;
using modoff.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static UtWWWAsync;

namespace modoff.Patch {
    [HarmonyPatch(typeof(UtWWWAsync), "Post")]

    public class SodoffPatch {
        public static Dictionary<string, string> ConvertByteArrayToDictionary(byte[] data) {
            string strData = System.Text.Encoding.UTF8.GetString(data);
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] keyValuePairs = strData.Split('&');

            foreach (string pair in keyValuePairs) {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2) {
                    string key = Uri.UnescapeDataString(keyValue[0]);
                    string value = Uri.UnescapeDataString(keyValue[1]);
                    dict[key] = value;
                } else {
                    Debug.LogWarning("Malformed key-value pair: " + pair);
                }
            }

            return dict;
        }

        static Semaphore sem = new Semaphore(2, 2);
        static Mutex mut = new Mutex();
        static bool backgroundThd = false;

        static bool Prefix(WWWProcess __instance, string inURL, WWWForm inForm, UtWWWEventHandler inCallback, bool inSendProgressEvents) {
            System.Threading.Tasks.Task.Run(() => {
                Dictionary<string, string> formData = ConvertByteArrayToDictionary(inForm.data);
                InjectedResponse inj = new InjectedResponse();
                string result = "";
                UtAsyncEvent asyncEvent = UtAsyncEvent.COMPLETE;
                try {
                    result = RuntimeStore.dispatcher.Dispatch(inURL, formData);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                    asyncEvent = UtAsyncEvent.ERROR;
                }

                inj.SetData(result);
                ResponseStore.mtx.WaitOne();
                ResponseStore.responses.Enqueue(new ModoffResponse(inj, inCallback, asyncEvent));
                ResponseStore.mtx.ReleaseMutex();
            });

            return false;
        }
    }
}
