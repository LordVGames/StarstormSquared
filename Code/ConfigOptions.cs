using BepInEx.Configuration;
using MiscFixes.Modules;
using StarstormSquared.Changes.Survivors.Chirr;
using StarstormSquared.ModSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine.SceneManagement;
using static StarstormSquared.ConfigOptions.ItemChanges.ErraticGadgetItem;
namespace StarstormSquared;


public static class ConfigOptions
{
    public static ConfigEntry<bool> DebugLogging;


    public static class Chirr
    {
        private const string _categoryName = "Chirr";
        public static ConfigEntry<bool> ChirrMinionsNoHealingItems;
        public static ConfigEntry<bool> ChirrNoTamingTerminals;
        public static ConfigEntry<bool> ChirrTameLingerFix;

        internal static void BindConfigOptions(ConfigFile config)
        {
            ChirrMinionsNoHealingItems = config.BindOption(
                _categoryName,
                "Make tamed enemies not get Chirrs healing items.",
                "This is so her gameplay of keeping her tamed enemy healthy isn't removed once a few good healing items are required.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            ChirrNoTamingTerminals = config.BindOption(
                _categoryName,
                "Make enemies from Relic of Termination untamable",
                "Termination eneimes are insanely strong when tamed by Chirr, this makes them untamable so they're not an almost free win",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            ChirrTameLingerFix = config.BindOption(
                _categoryName,
                "Fix enemies being tamable after debuff expiration",
                "Enemies can be tamed even after the taming debuff expires, this fixes that.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
        }
    }


    public static class Knight
    {
        private const string _categoryName = "Knight";
        public static ConfigEntry<bool> AddKnightPassive;

        internal static void BindConfigOptions(ConfigFile config)
        {
            AddKnightPassive = config.BindOption(
                _categoryName,
                "Add his unused passive",
                "Knight has a (currently) unused passive buff ward for allies that gives some attack speed and movement speed. This implements that.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
        }
    }


    public static class Events
    {
        private const string _categoryName = "Events";
        public static ConfigEntry<bool> NewStormEventText;
        public static ConfigEntry<bool> NewSuperEliteSpawnEventText;
        public static ConfigEntry<bool> SendMessagesToChatInstead;

        internal static void BindConfigOptions(ConfigFile config)
        {
            NewStormEventText = config.BindOption(
                _categoryName,
                "New storm text",
                "Replaces the silly placeholder storm event text with new text.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            NewSuperEliteSpawnEventText = config.BindOption(
                _categoryName,
                "New super elite spawn text",
                "Replaces the silly placeholder super elite spawn event text with new text.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            SendMessagesToChatInstead = config.BindOption(
                _categoryName,
                "Send event messages to chat instead",
                "Makes the event text pop-ups appear as chat messages instead. Helps in multiplayer, as beta events don't show the text pop-ups for clients yet.",
                false
            );
        }
    }


    public static class Elites
    {
        public static class Empyrean
        {
            private const string _categoryName = "Empyrean Elites";
            public static ConfigEntry<bool> RemoveEmpyreanShardDrop;
            public static ConfigEntry<bool> AllowEmpyreanSpawn;
            public static ConfigEntry<bool> AllowEmpyreansInJudgement;
            public static ConfigEntry<int> CustomEmpyreanLevelUpInterval;

            internal static void BindConfigOptions(ConfigFile config)
            {
                RemoveEmpyreanShardDrop = config.BindOption(
                    _categoryName,
                    "Remove shard drop",
                    "Prevents empyrean elites from dropping a random shard on death.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
                AllowEmpyreanSpawn = config.BindOption(
                    _categoryName,
                    "Allow spawning at all",
                    "If they are broken or if you just don't want them to spawn then disable this.",
                    true
                );
                AllowEmpyreansInJudgement = config.BindOption(
                    _categoryName,
                    "Allow spawning during EnemiesReturns Judgement",
                    "Empyreans can replace the normal aeonian spawns during the 1st Arraign phase, enable this if you still want that.",
                    false
                );
                CustomEmpyreanLevelUpInterval = config.BindOptionSlider(
                    _categoryName,
                    "Custom Empyrean Level Up Interval",
                    "Empyreans level up and gain doubled stats + a lot of HP every number of stages after the first stage they can appear on, that being stage 9. Change this number to change how many stages it takes for empyreans to level up.",
                    5
                );
            }
        }


        public static class Ultra
        {
            private const string _categoryName = "Ethereal Related";
            public static ConfigEntry<bool> TweakUltraWardBuff;
            public static ConfigEntry<bool> AddHealingToUltraWardBuff;


            internal static void BindConfigOptions(ConfigFile config)
            {
                TweakUltraWardBuff = config.BindOption(
                    _categoryName,
                    "Tweak the passive buff from ultra elites",
                    "Makes the passive buff ultra elites give off also apply to the ultra elite itself, along with the passive buff giving a tiny amount of % hp regen to everyone but ultra elites. This basically makes it like more of a super duper mending elite.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
                AddHealingToUltraWardBuff = config.BindOption(
                    _categoryName,
                    "Add slight healing to passive buff from ultra elites",
                    "The healing added is currently jank and heals way more than it should, if you still want it then enable this setting.",
                    false,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        public static class Toxic
        {
            private const string _categoryName = "Toxic Elites";
            public static ConfigEntry<bool> EnableToxicElite;

            internal static void BindConfigOptions(ConfigFile config)
            {
                EnableToxicElite = config.BindOption(
                    _categoryName,
                    "Enable elite",
                    "Toxic elites can't be disabled for some reason, so use this if you want to remove them.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        public static class AllLateGameElites
        {
            private const string _categoryName = "Lategame Elites Overall";
            public static ConfigEntry<bool> DisallowSelfDamagingEnemies;
            public static ConfigEntry<bool> RemoveEtherealAndUltraRestriction;
            internal static void BindConfigOptions(ConfigFile config)
            {
                DisallowSelfDamagingEnemies = config.BindOption(
                    _categoryName,
                    "Disallow self-damaging enemies",
                    "Prevents self damaging enemies (namely jellyfish and acid larva) from becoming empyrean, ethereal, or ultra.",
                    true
                );
                RemoveEtherealAndUltraRestriction = config.BindOption(
                    _categoryName,
                    "Remove restriction on ethereal and ultra spawns",
                    "Lets any naturally spawning enemy become ethereal or ultra, even ones that can't become elites normally.",
                    false,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        internal static void BindConfigOptions(ConfigFile config)
        {
            AllLateGameElites.BindConfigOptions(config);
            Empyrean.BindConfigOptions(config);
            Ultra.BindConfigOptions(config);
            Toxic.BindConfigOptions(config);
        }
    }


    public static class Ethereal
    {
        private const string _categoryName = "Ethereal Related";
        public static ConfigEntry<bool> AddCraftingChefToZanzanStage;
        public static ConfigEntry<bool> SpawnSaplingInSpecialSpots;
        public static ConfigEntry<bool> StopZanzanIdleSounds;
        public static ConfigEntry<bool> AddAmbientCapLevels;
        public static ConfigEntry<float> BonusLevelsToAdd;
        public static ConfigEntry<bool> MultiplyByEtherealsCompleted;
        public static ConfigEntry<bool> AddLevelsOnFirstEthereal;
        public static ConfigEntry<bool> SquareEtherealsCompleted;

        internal static void BindConfigOptions(ConfigFile config)
        {
            AddCraftingChefToZanzanStage = config.BindOption(
                _categoryName,
                "Add a crafting chef to the strangers hideout",
                "It is put on top of the roof that Zanzan the faded sits under.",
                true
            );
            SpawnSaplingInSpecialSpots = config.BindOption(
                _categoryName,
                "Make ethereal sapling spawn in stage-specific spots instead of newt altar spots.",
                "In stages without a pre-determined spot they will still spawn in a newt atltar spot.",
                false
            );
            StopZanzanIdleSounds = config.BindOption(
                _categoryName,
                "Stop Zanzans idle sounds",
                "If their idle scav sounds are getting a lil annoying (no offense to them) then enable this to stop the sounds. Changes only take affect on stage load!",
                true
            );
            AddAmbientCapLevels = config.BindOption(
                _categoryName,
                "Add levels to ambient level cap",
                "Should 100 levels be added to the ambient level cap each ethereal level?",
                true
            );
            BonusLevelsToAdd = config.BindOptionSteppedSlider(
                _categoryName,
                "Bonus levels to add",
                "You can configure the amount of bonus levels (separate from the ambient level cap) to add to enemies each ethereal level. This is shown by the green +X number by the ambient level cap.",
                30,
                1,
                0, 999
            );
            MultiplyByEtherealsCompleted = config.BindOption(
                _categoryName,
                "Multiply bonus levels per ethereal",
                "If their idle scav sounds are getting a lil annoying (no offense to them) then enable this to stop the sounds. Changes only take affect on stage load!",
                true
            );
            AddLevelsOnFirstEthereal = config.BindOption(
                _categoryName,
                "Add bonus levels on the first ethereal level",
                "SS2 doesn't add any bonus levels on the first ethereal level (ambient level cap is increased on the first ethereal however). Enable this if you want that to happen",
                false
            );
            SquareEtherealsCompleted = config.BindOption(
                _categoryName,
                "Put completed ethereals count to the power of 2?",
                "Or in other words, if the ethereals completed count should be SQUARED!!!!!!",
                true
            );
        }
    }


    public static class Shards
    {
        private const string _categoryName = "Restore Shard Drops";
        public static ConfigEntry<bool> RestoreGoldShardDrop;
        public static ConfigEntry<bool> RestoreVoidShardDrop;
        public static ConfigEntry<bool> RestoreStormShardDrops;

        internal static void BindConfigOptions(ConfigFile config)
        {

            RestoreGoldShardDrop = config.BindOption(
                _categoryName,
                "Restore gold shard drop",
                "Gold shards drop from completed halcyon shrines.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            RestoreVoidShardDrop = config.BindOption(
                _categoryName,
                "Restore void shard drop",
                "Void shards drop from completed void seeds.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            RestoreStormShardDrops = config.BindOption(
                _categoryName,
                "Restore storm shard drops",
                "Storm shards drop when the boss from a stormborn teleporter is beaten.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
        }
    }


    public static class ItemChanges
    {
        private const string _categoryName = "Item Changes";


        public static class ErraticGadgetItem
        {
            private const string _categoryName = "Item Changes - Erratic Gadget";
            public enum ErraticGadgetChangeType
            {
                None = 0,
                DamageMultAndOnHitProc,
                OnlyDamageMult
            }
            public static ConfigEntry<ErraticGadgetChangeType> Change;
            public static ConfigEntry<bool> AllowVoidLightning;


            internal static void BindConfigOptions(ConfigFile config)
            {
                Change = config.BindOption(
                    _categoryName,
                    "Item Change",
                    "2 Different changes:\n\nDamageMultAndOnHitProc: Doubled lightning damage and chance to do chain lightning on hit. Stacks increase chance and targets hit.\n\nOnlyDamageMult: 3x lightning damage, stacks add to the damage multiplier.\n\nAnd of course a None option for if you don't want either.",
                    ErraticGadgetChangeType.DamageMultAndOnHitProc,
                    Extensions.ConfigFlags.RestartRequired
                );
                AllowVoidLightning = config.BindOption(
                    _categoryName,
                    "Allow void lightning to be affected",
                    "SS2 normally doesn't allow void lightning (aka polylute) to be affected by erratic gadget's doubling. If you want it to be affected, use this.",
                    false,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        public static class PortableReactor
        {
            public static ConfigEntry<bool> PortableReactorChange;


            internal static void BindConfigOptions(ConfigFile config)
            {
                PortableReactorChange = config.BindOption(
                    _categoryName,
                    "Portable Reactor",
                    "Makes portable reactor give 100 armor instead of invulnerability while active.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        internal static void BindConfigOptions(ConfigFile config)
        {
            ErraticGadgetItem.BindConfigOptions(config);
            PortableReactor.BindConfigOptions(config);
        }
    }


    public static class Stages
    {
        public static class SlateMines
        {
            private const string _categoryName = "Slate Mines Stage";
            public static ConfigEntry<bool> FixPinkWater;


            internal static void BindConfigOptions(ConfigFile config)
            {

                FixPinkWater = config.BindOption(
                    _categoryName,
                    "Fix Pink Water",
                    "If the water texture in slate mines is pink, enable this to replace it with a water texture from the game.",
                    false,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        public static class VoidShop
        {
            private const string _categoryName = "Strangers Hideout Stage";
            public static ConfigEntry<bool> TogglePinkGrass;
            private static void TogglePinkGrass_SettingChanged(object sender, EventArgs e)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name != "ss2_voidshop")
                {
                    return;
                }


                foreach (var gameObject in scene.GetRootGameObjects())
                {
                    if (gameObject.name.Contains("FOLIAGE"))
                    {
                        gameObject.transform.Find("Grass").gameObject.SetActive(TogglePinkGrass.Value);
                        return;
                    }
                }
            }


            internal static void BindConfigOptions(ConfigFile config)
            {
                TogglePinkGrass = config.BindOption(
                    _categoryName,
                    "Enable Pink Grass",
                    "The grass texture is currently broken, even when trying to manually load it. Use this config option to turn it on/off to your liking.",
                    false
                );
                TogglePinkGrass.SettingChanged += TogglePinkGrass_SettingChanged;
            }
        }


        internal static void BindConfigOptions(ConfigFile config)
        {
            SlateMines.BindConfigOptions(config);
            VoidShop.BindConfigOptions(config);
        }
    }


    public static class NemesisInvasions
    {
        private const string _categoryName = "Nemesis Invasions";
        public static ConfigEntry<bool> TakeLessKnockback;


        internal static void BindConfigOptions(ConfigFile config)
        {
            TakeLessKnockback = config.BindOption(
                _categoryName,
                "Invaders take less knockback",
                "Makes the nemesis invaders a lot harder to knockback or launch due to abilities/item effects.",
                true
            );
        }
    }


    public static class Interactables
    {
        private const string _categoryName = "Other Interactables";


        public static class PrimalBirthrightChest
        {
            private const string _categoryName = "Primal Birthright Chest";
            public static ConfigEntry<bool> PreventHackedByCaptain;
            public static ConfigEntry<bool> PreventHackedByRobomando;


            internal static void BindConfigOptions(ConfigFile config)
            {
                PreventHackedByCaptain = config.BindOption(
                    _categoryName,
                    "Make Unhackable for Captain",
                    "Should the primal birthright chest be unable to be targeted by captains hacking beacons?",
                    true
                );
                PreventHackedByRobomando = config.BindOption(
                    _categoryName,
                    "Make Unhackable for Robomando",
                    "Should the primal birthright chest be unable to be targeted by robomandos special skill?",
                    false
                );
            }
        }


        internal static void BindConfigOptions(ConfigFile config)
        {
            PrimalBirthrightChest.BindConfigOptions(config);
        }
    }


    internal static void BindAllConfigOptions(ConfigFile config)
    {
        DebugLogging = config.BindOption(
            "General",
            "Enable any extra debug logging",
            "",
            false
        );
        Chirr.BindConfigOptions(config);
        // later
        //Knight.BindConfigOptions(config);
        Events.BindConfigOptions(config);
        Elites.BindConfigOptions(config);
        Ethereal.BindConfigOptions(config);
        Shards.BindConfigOptions(config);
        ItemChanges.BindConfigOptions(config);
        Stages.BindConfigOptions(config);
        NemesisInvasions.BindConfigOptions(config);
        Interactables.BindConfigOptions(config);
    }
}