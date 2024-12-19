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
    }
}
