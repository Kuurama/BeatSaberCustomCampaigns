using CustomCampaigns.Campaign.Missions;
using CustomCampaigns.Managers;
using HarmonyLib;
using SongCore;
using System;

namespace CustomCampaigns.HarmonyPatches
{
    [HarmonyPatch(typeof(CampaignFlowCoordinator), "StartLevel")]
    class CampaignFlowCoordinatorStartLevelPatch
    {
        static bool Prefix(Action beforeSceneSwitchCallback, CampaignFlowCoordinator __instance, MissionSelectionNavigationController ____missionSelectionNavigationController, MenuTransitionsHelper ____menuTransitionsHelper, PlayerDataModel ____playerDataModel, EnvironmentsListModel ____environmentsListModel)
        {
            if (CustomCampaignManager.isCampaignLevel) return false;

            var missionData = ____missionSelectionNavigationController.selectedMissionNode.missionData as CustomMissionDataSO;
            if (missionData == null) return true;

            var levelId = missionData.beatmapLevel.levelID;
            var beatmapLevel = Loader.BeatmapLevelsModelSO.GetBeatmapLevel(levelId);
            var beatmapKey = new BeatmapKey(levelId, missionData.beatmapCharacteristic, missionData.beatmapDifficulty);

            GameplayModifiers gameplayModifiers = missionData.gameplayModifiers;
            MissionObjective[] missionObjectives = missionData.missionObjectives;
            PlayerSpecificSettings playerSpecificSettings = ____playerDataModel.playerData.playerSpecificSettings;
            ColorSchemesSettings colorSchemesSettings = ____playerDataModel.playerData.colorSchemesSettings;
            ColorScheme overrideColorScheme = colorSchemesSettings.overrideDefaultColors ? colorSchemesSettings.GetSelectedColorScheme() : null;

            ____menuTransitionsHelper.StartMissionLevel("", beatmapKey, beatmapLevel, overrideColorScheme, gameplayModifiers, missionObjectives, playerSpecificSettings, ____environmentsListModel, beforeSceneSwitchCallback,
                levelFinishedCallback: (Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults> ) __instance.GetType().GetMethod("HandleMissionLevelSceneDidFinish", AccessTools.all)?.CreateDelegate(typeof(Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>), __instance),
                levelRestartedCallback: (Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>) __instance.GetType().GetMethod("HandleMissionLevelSceneRestarted", AccessTools.all)?.CreateDelegate(typeof(Action<MissionLevelScenesTransitionSetupDataSO, MissionCompletionResults>), __instance));

            return false;
        }
    }
}
