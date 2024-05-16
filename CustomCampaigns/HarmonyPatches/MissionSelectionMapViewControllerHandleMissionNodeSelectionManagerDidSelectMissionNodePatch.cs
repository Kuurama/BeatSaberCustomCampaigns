using CustomCampaigns.Campaign.Missions;
using HarmonyLib;
using IPA.Utilities;
using System;

namespace CustomCampaigns.HarmonyPatches
{
    [HarmonyPatch(typeof(MissionSelectionMapViewController), "HandleMissionNodeSelectionManagerDidSelectMissionNode")]
    class MissionSelectionMapViewControllerHandleMissionNodeSelectionManagerDidSelectMissionNodePatch
    {
        static bool Prefix(MissionNodeVisualController missionNodeVisualController, MissionSelectionMapViewController __instance, SongPreviewPlayer ____songPreviewPlayer)
        {
            if (!(missionNodeVisualController.missionNode.missionData is CustomMissionDataSO customMissionDataSo)) return true;

            __instance.SetField("_selectedMissionNode", missionNodeVisualController.missionNode);
            BeatmapLevel level = customMissionDataSo.beatmapLevel;
            if (level != null)
            {
                __instance.InvokeMethod<object, MissionSelectionMapViewController>("SongPlayerCrossfadeToLevelAsync", level);
            }
            __instance.GetField<Action<MissionSelectionMapViewController, MissionNode>, MissionSelectionMapViewController>("didSelectMissionLevelEvent")?.Invoke(__instance, missionNodeVisualController.missionNode);
            return false;
        }
    }
}
