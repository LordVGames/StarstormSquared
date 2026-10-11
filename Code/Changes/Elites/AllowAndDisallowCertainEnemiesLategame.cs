using EnemiesReturns.Components;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2.Components;
using System;
using UnityEngine;
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
        int stackableAffixLdlocNumber = 0;
        int characterBodyLdlocNumber = 0;
        int spawnResultLdargNumber = 0;




        bool didFirstMatch = false;
        ILWeaverResult result = w.MatchMultipleRelaxed(
           onMatch: (Action<ILWeaver>)(mW =>
           {
               if (!didFirstMatch)
               {
                   didFirstMatch = true;
               }
               else
               {
                   return;
               }
           }),
           x => x.MatchLdarg(out spawnResultLdargNumber),
           x => x.MatchLdfld("RoR2.SpawnCard/SpawnResult", "spawnedInstance")
        );




        w.MatchRelaxed(
            x => x.MatchLdfld<SpawnCard>("eliteRules") && w.SetCurrentTo(x),
            x => x.MatchBrfalse(out var exitCodeLabel)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.CreateDelegateCall((bool oldBool) =>
            {
                // always allowing if enabled
                if (ConfigOptions.Elites.AllLateGameElites.RemoveEtherealAndUltraRestriction.Value)
                {
                    return true;
                }
                return oldBool;
            })
        );




        // also preventing jellyfish and larva from becoming lategame elites here because they do nothing 99% of the time
        w.MatchRelaxed(
            x => x.MatchLdloc(0),
            x => x.MatchCallOrCallvirt<CharacterMaster>("get_bodyInstanceObject"),
            x => x.MatchCallOrCallvirt<GameObject>("GetComponent"),
            x => x.MatchStloc(out characterBodyLdlocNumber) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, characterBodyLdlocNumber),
            w.CreateDelegateCall((CharacterBody characterBody) =>
            {
                if (
                    characterBody != null
                    && ConfigOptions.Elites.AllLateGameElites.DisallowSelfDamagingEnemies.Value
                    &&
                    (
                        characterBody.bodyIndex == RoR2Content.BodyPrefabs.JellyfishBody.bodyIndex
                        || characterBody.bodyIndex == DLC1Content.BodyPrefabs.AcidLarvaBody.bodyIndex
                    )
                )
                {
                    return false;
                }
                return true;
            }),
            w.Create(OpCodes.Brtrue, w.Next),
            w.Create(OpCodes.Ret)
        );

        


        // re-adding elite rules but only for empyreans
        ILLabel goToNextAffix = null;
        w.MatchRelaxed(
            x => x.MatchLdloc(out stackableAffixLdlocNumber),
            x => x.MatchCallOrCallvirt<StackableAffix>("IsAvailable"),
            x => x.MatchBrfalse(out goToNextAffix) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, stackableAffixLdlocNumber),
            w.Create(OpCodes.Ldarg, spawnResultLdargNumber),
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