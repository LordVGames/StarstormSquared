using RoR2;
using RoR2.Skills;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Survivors.DUT;


internal static class GiveSkillIcons
{
    [SystemInitializer(dependencies: typeof(BodyCatalog))]
    internal static void GiveDUTSkillIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        GameObject dutGameObject = BodyCatalog.FindBodyPrefab("DUTBody");
        if (dutGameObject == null || !dutGameObject.TryGetComponent<CharacterBody>(out var dutBody))
        {
            Log.Warning("DU-T could not be found to give new skill icons to!");
            return;
        }
        if (!dutBody.TryGetComponent<SkillLocator>(out var dutSkillLocator))
        {
            Log.Warning("DU-T didn't have a skill locator???");
            return;
        }


        dutSkillLocator.primary.skillFamily.defaultSkillDef.icon = LoadedAssets.RailgunnerShotIcon;
        dutSkillLocator.utility.skillFamily.defaultSkillDef.icon = LoadedAssets.ToolbotDashIcon;
        // mode starts in damage mode, so switch icon should show as healing to start
        dutSkillLocator.special.skillFamily.defaultSkillDef.icon = LoadedAssets.ToolbotSwapIcon;
    }
}