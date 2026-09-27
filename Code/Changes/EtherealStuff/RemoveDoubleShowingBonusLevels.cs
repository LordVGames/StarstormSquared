using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using SS2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace StarstormSquared.Changes.EtherealStuff;


[MonoDetourTargets]
internal static class RemoveDoubleShowingBonusLevels
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        // there's a better way to do this i think but this is good enough
        Mdh.SS2.EtherealBehavior.AmbientLevelDisplay_Update.Postfix(Prefix);
    }


    private static void Prefix(EtherealBehavior self, ref On.RoR2.UI.AmbientLevelDisplay.orig_Update orig, ref AmbientLevelDisplay self1)
    {
        if (self.bonusLevels < 1)
        {
            return;
        }


        // ty stackoverflow though
        string[] bonusLevelTexts = [.. Regex.Matches(self1.text.text, " <color=#34EB8F>\\+\\d*")
            .OfType<Match>()
            .Select(m => m.Groups[0].Value)];
        if (bonusLevelTexts.Length > 1)
        {
            // makes the new string with the lv. #### part and the 2nd (aka newer) bonus levels text 
            string newThing = self1.text.text.Split(" <c")[0] + bonusLevelTexts[1];
            self1.text.text = newThing;
        }
    }
}