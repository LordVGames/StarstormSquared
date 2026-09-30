using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using MonoDetour.Cil;
namespace StarstormSquared;


internal static class ILHelpers
{
    internal static void LogILAroundCurrent(this ILWeaver w, int range)
    {
        for (int i = range * -1; i < (range + 1); i++)
        {
            int newIndex = w.Index + i;
            if (i == 0)
            {
                Log.Warning("");
                Log.Warning($"{w.Current}    CURRENT"); // shows as IL_0000 for some reason?
                Log.Warning("");
            }
            else if (newIndex > -1 && newIndex <= w.Instructions.Count)
            {
                Log.Warning(w.Instructions[w.Index + i]);
            }
        }
    }


    internal static void LogCantHookMessage(string hookFunctionality, string failureMessage)
    {
        Log.Error($"Couldn't setup hook for \"{hookFunctionality}\"!\nReason:\n{failureMessage}");
    }
}