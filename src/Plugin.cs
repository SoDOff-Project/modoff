using BepInEx;
using HarmonyLib;
using modoff.Runtime;
using System;
using UnityEngine;

namespace modoff
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        public void Awake() {
            Harmony harmony = new Harmony("SoDOff");

            harmony.PatchAll();
            Logger.LogInfo("SoDOff patch");
        }
    }
}
