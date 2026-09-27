using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.EtherealStuff;


[MonoDetourTargets(typeof(EtherealBehavior))]
internal static class ConfigureEtherealLevelScaling
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.SS2.EtherealBehavior.SetEtherealsCompleted.ILHook(ChangeLeveling);
    }


    private static void ChangeLeveling(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        w.MatchRelaxed(
            x => x.MatchStfld<EtherealBehavior>("bonusLevels") && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertBeforeCurrent(
            w.Create(OpCodes.Ldarg_1),
            w.CreateDelegateCall((int oldLevelsToAdd, int etherealsCompleted) =>
            {
                int levelsToAdd = (int)ConfigOptions.Ethereal.BonusLevelsToAdd.Value;
                int multValue = 1;
                if (ConfigOptions.Ethereal.MultiplyByEtherealsCompleted.Value)
                {
                    multValue = etherealsCompleted;
                    if (!ConfigOptions.Ethereal.AddLevelsOnFirstEthereal.Value)
                    {
                        multValue = Mathf.Max(0, multValue - 1);
                    }
                }
                if (ConfigOptions.Ethereal.SquareEtherealsCompleted.Value)
                {
                    multValue = (int)Mathf.Pow(2, multValue);
                }
                return levelsToAdd * multValue;
            })
        );


        // removing adding to ambient level cap since levels can added along with some fancier ui anyways
        Instruction skipStart = null!;
        Instruction skipEnd = null!;
        w.MatchRelaxed(
            x => x.MatchLdsfld<EtherealBehavior>("defaultLevelCap") && w.SetInstructionTo(ref skipStart, x),
            x => x.MatchLdcI4(100),
            x => x.MatchLdarg(out _),
            x => x.MatchMul(),
            x => x.MatchAdd(),
            x => x.MatchStsfld<Run>("ambientLevelCap") && w.SetInstructionTo(ref skipEnd, x)
        ).ThrowIfFailure()
        .InsertBranchOver(skipStart, skipEnd);
    }
}