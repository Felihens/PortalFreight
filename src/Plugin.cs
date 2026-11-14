using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace PortalFreight
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "felih.PortalFreight";
        public const string NAME = "Portal Freight";
        public const string VERSION = "1.0.0";

        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<bool> HideMarks;
        internal static ConfigEntry<bool> DetailedLog, FastArrival;
        internal static ConfigEntry<float> MinLoadSeconds, GlowRange;

        private void Awake()
        {
            Log = Logger;
            Enabled = Config.Bind("1 - General", "Enabled", true,
                "Carry anything through portals: ores, metal bars, eggs and every other item the game holds back, " +
                "whatever the world settings say. Only your own game is changed: it works on any server.");
            HideMarks = Config.Bind("1 - General", "HideNoPortalMarks", true,
                "Hide the \"can't go through portals\" mark on items in your bag and the line saying so in their tooltip.");
            FastArrival = Config.Bind("2 - Teleport", "FastArrival", true,
                "End the loading screen as soon as the place you go to is loaded. The game always waits at least 8 seconds " +
                "for a far portal, even when everything is ready long before.");
            MinLoadSeconds = Config.Bind("2 - Teleport", "MinLoadSeconds", 3f,
                new ConfigDescription("Shortest loading screen with FastArrival, in seconds (the game: 8).", new AcceptableValueRange<float>(2.5f, 8f)));
            GlowRange = Config.Bind("3 - Portals", "GlowRange", 0.7f,
                new ConfigDescription("How close you must be for a portal to light up and hum, as a share of the game's distance " +
                "(1 = the game's own, 0.7 = a bit closer, so portals you walk past stay quiet).", new AcceptableValueRange<float>(0.2f, 1f)));
            DetailedLog = Config.Bind("9 - Debug", "DetailedLog", false, "Writes details to the BepInEx log.");
            new Harmony(GUID).PatchAll(typeof(Plugin).Assembly);
            Log.LogInfo(NAME + " " + VERSION + " loaded");
        }

        internal static void Debug(string msg)
        {
            if (DetailedLog.Value) Log.LogInfo(msg);
        }
    }

    // The game asks the bag whether it may go through a portal (Inventory.IsTeleportable) both to light the portal
    // up and when you step in. Its argument says "this portal lets everything through" (the game's own portals that
    // do so pass true): here every portal passes true, so nothing is held back. Items the game marks as never
    // allowed anywhere (a tool tier of 1000 or more, which no normal item has) stay held back by the game itself.
    [HarmonyPatch(typeof(Inventory), "IsTeleportable")]
    internal static class AnythingGoes
    {
        private static bool _said;

        private static void Prefix(ref bool allowAllItems)
        {
            if (!Plugin.Enabled.Value || allowAllItems) return;
            allowAllItems = true;
            if (!_said) { _said = true; Plugin.Debug("portals let everything through"); }
        }
    }

    // the "no portal" mark on the items in the bag
    [HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]
    internal static class NoMarks
    {
        private static readonly FieldInfo Elements = AccessTools.Field(typeof(InventoryGrid), "m_elements");
        private static FieldInfo _mark;

        private static void Postfix(InventoryGrid __instance)
        {
            if (!Plugin.Enabled.Value || !Plugin.HideMarks.Value || Elements == null) return;
            IList list = Elements.GetValue(__instance) as IList;
            if (list == null) return;
            foreach (object e in list)
            {
                if (e == null) continue;
                if (_mark == null) _mark = AccessTools.Field(e.GetType(), "m_noteleport");
                if (_mark == null) return;
                Image img = _mark.GetValue(e) as Image;
                if (img != null && img.enabled) img.enabled = false;
            }
        }
    }

    // the "can't go through portals" line in the item's tooltip
    [HarmonyPatch]
    internal static class NoLine
    {
        private const string Line = "<color=orange>$item_noteleport</color>";

        private static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (MethodInfo m in typeof(ItemDrop.ItemData).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                if (m.Name == "GetTooltip" && m.ReturnType == typeof(string)) yield return m;
        }

        private static void Postfix(ref string __result)
        {
            if (!Plugin.Enabled.Value || !Plugin.HideMarks.Value || string.IsNullOrEmpty(__result) || __result.IndexOf(Line) < 0) return;
            __result = __result.Replace("\n" + Line, "").Replace(Line, "");
        }
    }
}

namespace PortalFreight
{
    // The game's teleport (Player.UpdateTeleport): after 2 seconds of black screen you are put at the far portal, and
    // for a far one the screen stays black until 8 seconds have passed and the place is loaded. Here, once the place
    // has been loaded for half a second and MinLoadSeconds have passed, the timer is moved on to 8 so the game ends
    // the teleport itself, the same way (it still looks for the floor first).
    [HarmonyPatch(typeof(Player), "UpdateTeleport")]
    internal static class FastArrival
    {
        private const float GameWait = 8f, Settle = 0.5f;
        private static float _readySince = -1f;

        private static void Prefix(Player __instance, bool ___m_teleporting, bool ___m_distantTeleport, ref float ___m_teleportTimer, Vector3 ___m_teleportTargetPos)
        {
            if (!___m_teleporting) { _readySince = -1f; return; }
            if (!Plugin.Enabled.Value || !Plugin.FastArrival.Value || !___m_distantTeleport || __instance != Player.m_localPlayer) return;
            if (___m_teleportTimer <= 2.2f || ___m_teleportTimer >= GameWait || ZNetScene.instance == null) return;
            if (!ZNetScene.instance.IsAreaReady(___m_teleportTargetPos)) { _readySince = -1f; return; }
            if (_readySince < 0f) _readySince = ___m_teleportTimer;
            if (___m_teleportTimer - _readySince < Settle || ___m_teleportTimer < Plugin.MinLoadSeconds.Value) return;
            Plugin.Debug("teleport ends after " + ___m_teleportTimer.ToString("0.0") + " s instead of " + GameWait + " s");
            ___m_teleportTimer = GameWait;
        }
    }

    // portals light up and hum only when you are a bit closer
    [HarmonyPatch(typeof(TeleportWorld), "Awake")]
    internal static class GlowRange
    {
        private static void Postfix(TeleportWorld __instance)
        {
            if (!Plugin.Enabled.Value) return;
            __instance.m_activationRange *= Mathf.Clamp(Plugin.GlowRange.Value, 0.2f, 1f);
        }
    }
}
