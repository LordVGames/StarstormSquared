using System;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
namespace StarstormSquared.Changes.Elites.Empyrean;


[MonoDetourTargets(typeof(SS2.Components.Empyrean), GenerateControlFlowVariants = true)]
internal static class CustomEmpyreanLevelUpInterval
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.Empyrean.MakeElite.ILHook(MakeLevelUpIntervalConfigurable);
    }


    private static void MakeLevelUpIntervalConfigurable(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchLdloc(1),
            x => x.MatchLdcI4(5) && w.SetCurrentTo(x),
            x => x.MatchDiv(),
            x => x.MatchConvR4(),
            x => x.MatchCall(out _),
            x => x.MatchStloc(2)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Making the empyrean level up interval configurable", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            w.CreateDelegateCall((int oldValue) =>
            {
                return ConfigOptions.Elites.Empyrean.CustomEmpyreanLevelUpInterval.Value;
            })
        );
    }
}