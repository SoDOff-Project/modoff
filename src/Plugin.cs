using BepInEx;
using HarmonyLib;
using modoff.Runtime;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace modoff
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public void Awake() {
            Harmony harmony = new Harmony("SoDOff");

            harmony.PatchAll();
            Logger.LogInfo("SoDOff patch");

            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args) {
                if (args.Name.Contains("System.Runtime.InteropServices.RuntimeInformation"))
                    return typeof(RuntimeInformation).Assembly;
                return null;
            };

            RuntimeStore.Init();
        }

        private void OnGUI() {
            GUIStyle style = new GUIStyle {
                fontSize = 18,
                normal = { textColor = Color.white }
            };
            int pos = 10;
            foreach (var item in ModoffLogger.messages) {
                GUI.Label(new Rect(10, pos, 5000, 30), item, style);
                pos += 35;
            }
        }
    }
}
