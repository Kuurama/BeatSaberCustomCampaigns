using CustomCampaigns.Campaign.Missions;
using HarmonyLib;

namespace CustomCampaigns.HarmonyPatches
{
    [HarmonyPatch(typeof(AchievementsEvaluationHandler), "HandleCampaignOverallStatsDataDidUpdate")]
    public class AchievementsEvaluationHandlerHandleCampaignOverallStatsDataDidUpdatePatch
    {
        public static bool Prefix(MissionCompletionResults missionCompletionResults, IMissionNode missionNode, AchievementsEvaluationHandler __instance)
        {
            var missionData = missionNode.missionData as CustomMissionDataSO;
            if (missionData == null) return true;

            __instance.GetType().GetMethod("ProcessMissionFinishData", AccessTools.all)?.Invoke(__instance, new object[] { missionNode, missionCompletionResults });
            __instance.GetType().GetMethod("ProcessLevelFinishData",   AccessTools.all)?.Invoke(__instance, new object[] { missionData.beatmapDifficulty, missionCompletionResults.levelCompletionResults });
            return false;
        }
    }
}
