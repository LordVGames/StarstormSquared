using System;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2.Components;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Elites;


[MonoDetourTargets(typeof(CustomEliteDirector), GenerateControlFlowVariants = true)]
internal static class AllowAndDisallowCertainEnemiesLategame
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.CustomEliteDirector.ModifySpawn.ILHook(CustomEliteDirector_ModifySpawn);
    }
    

    private static void CustomEliteDirector_ModifySpawn(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel continueCodeLabel = w.DefineLabel();
        ILLabel exitCodeLabel = w.DefineLabel();
        int eliteRulesBoolResultLdlocNumber = 0;
        int tempMatchingLocNumber = 0;
        int stackableAffixLdlocNumber = 0;


        w.MatchRelaxed(
            x => x.MatchLdfld<SpawnCard>("eliteRules"),
            x => x.MatchLdcI4(0),
            x => x.MatchCgtUn(),
            x => x.MatchStloc(out eliteRulesBoolResultLdlocNumber) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertBeforeCurrent(
            w.CreateDelegateCall((bool oldBool) =>
            {
                // always allowing if enabled
                if (ConfigOptions.Elites.AllLateGameElites.RemoveEtherealAndUltraRestriction.Value)
                {
                    return false;
                }
                return oldBool;
            })
        );


        // also preventing jellyfish and larva from becoming lategame elites here because they do nothing 99% of the time
        w.MatchRelaxed(
            x => x.MatchLdloc(eliteRulesBoolResultLdlocNumber),
            x => x.MatchBrfalse(out continueCodeLabel),
            x => x.MatchBr(out exitCodeLabel) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc_1),
            w.CreateDelegateCall((CharacterBody body) =>
            {
                if (
                    body != null
                    && ConfigOptions.Elites.AllLateGameElites.DisallowSelfDamagingEnemies.Value
                    &&
                    (
                        body.bodyIndex == RoR2Content.BodyPrefabs.JellyfishBody.bodyIndex
                        || body.bodyIndex == DLC1Content.BodyPrefabs.AcidLarvaBody.bodyIndex
                    )
                )
                {
                    return true;
                }
                return false;
            }),
            w.Create(OpCodes.Brfalse, continueCodeLabel),
            w.Create(OpCodes.Br, exitCodeLabel)
        );


        // re-adding elite rules but only for empyreans
        ILLabel goToNextAffix = null;
        w.MatchRelaxed(
            x => x.MatchLdloc(out stackableAffixLdlocNumber),
            x => x.MatchCallOrCallvirt<StackableAffix>("IsAvailable"),
            x => x.MatchStloc(out tempMatchingLocNumber),
            x => x.MatchLdloc(tempMatchingLocNumber),
            x => x.MatchBrfalse(out goToNextAffix) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, stackableAffixLdlocNumber),
            w.Create(OpCodes.Ldarg_2),
            w.CreateDelegateCall((StackableAffix stackableAffix, SpawnCard.SpawnResult spawnResult) =>
            {
                if (
                    stackableAffix.GetType().Name == "Empyrean"
                    && spawnResult.spawnRequest.spawnCard.eliteRules != SpawnCard.EliteRules.Default
                )
                {
                    return false;
                }
                return true;
            }),
            w.Create(OpCodes.Brfalse, goToNextAffix)
        );
    }
}