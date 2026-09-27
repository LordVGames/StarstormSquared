# 1.8.0

- Removed some fixes for things now fixed in SS2
- Fixed some changes that broke from SS2 updating
- Removed Armed Backpack change as it's gotten a special missile now
- Fixed captain's primal birthright hacking prevention being skipped if robomando isn't installed
- Hopefully fixed jellyfish and larva not being prevented from spawning as late game elites (if the config option for that is enabled of course)
- Added some config options to let you configure how extra levels for enemies are added by ethereal difficulties
- - Default options are set to do what SS2 normally does
- Added an icon for the mod in risk of options
- (Basically) Fixed new bonus levels text from ethereals showing up twice
- Multiplied damage from the erratic gadget changes now also affect proc coefficient
- Moved the lunar gambler to a different spot in the bazaar away from the reroller and bigger bazaar stuff

# 1.7.0

- Removed a few fixes added to SS2 itself
- Changed a few config option categories around, double check what you have set!
- Fixed super elites not having their proper super elite affix
- Fixed super elites in multiplayer dropping glitched shards that can't be picked up
- Added new icons to super elites (made by Gangrene!)
- Added a config options to prevent captain and/or robomando from hacking primal birthright chests
- - By default: captain not allowed, robomando is allowed
- Added a config option to send event messages to the chat instead of a big text pop-up (not just family events!)
- - This is helpful for multiplayer clients as they currently can't see elite event messages
- Added a config option to stop Zanzan's idle sounds
- - On by default
- `start_elite_event` command now won't run if you don't have the beta content enabled
- Fixed error from a DU-T fix
- Added an unused yet working equipment from DLC3 (ss1 added back unused stuff so this is fine imo)
- Made dupe drones and the zanzan trade menu ignore temp items
- Fixed NRE with dupe drone (clonedrone) search
- Picking up an item mid-dupe now barely adds cooldown for the dupe drone

# 1.6.0

- Fixed "allow empyreans" config description being wrong and made it default to true
- Added more description text for bandit's tranq gun
- Made nemesis invaders much harder to knockback/launch
- Changed a few category names for config options, so some options may have been reset

# 1.5.2

- Fixed fork damage causing DU-T to get 2x orbs per siphon
- I stg I keep forgetting to check for beta

# 1.5.1

- Fixed having DU-T disabled causing this mod to prevent the game from loading
- - This only happened if you had DU-T disabled, not if you had beta content in general turned off

# 1.5.0

- Fixed readme still mentioning removed fixes
- Fixed empyrean disable config text being confusing
- Removed survivor description text about knight only working for the host
- - His slight rework fixed the banner ability for clients
- Fixed HP being messed up while you have the empyrean affix
- Added option to fix slate mines water
- Fixed some things with W.I.P DU-T to make them more playable
- - I am not done with DU-T yet though, but this makes his existing kit work better

# 1.4.2

- Fixed for latest SS2

# 1.4.1

- Removed fixes added officially to SS2
- Added config option to disable empyrean spawns (off by default, but should be turned on for now due to hp not working)
- - Empyrean HP will be fixed later if the SS2 devs don't get to it first

# 1.4.0

- Made shard-tier items able to be picked up while having substandard duplicator by making them world-unique
- Fixed all water + a ramp in slate mines having a pink texture (others will be fixed eventually)
- Gave elite event progress objective new text
- Allowed all enemies to become ethereal/ultra (but still not empyrean)
- Disallowed jellyfish and acid larva from becoming any lategame elite
- Added console command to manually start a specified elite event
- Fixed ethereal sapling spawn place config being the opposite of what was set
- Fixed item change configs causing the game to not load if the would-be changed item is disabled
- Fixed toxic elite toggle being able to enable toxic elites despite beta content being disabled

# 1.3.1

- Fixed empyrean leveling config breaking empyrean scaling, even with the default setting

# 1.3.0

- Mod renamed to StarstormSquared
- Buffed Cyborg's default primary damage (200% > 320%)
- Fixed "Allow spawning during EnemiesReturns Judgement" setting never working
- Added option for the amount of stages needed for empyreans to "level up" (default is what ss2 has set, being 5 stages)
- Added option to make ethereal sapling appear in a guranteed stage-specific spot that aren't newt-altar spots (off by default)
- Added option to put a wandering chef in the stranger's hideout (on by default)
- Re-added removing broken achievements from x4 stimulant, insecticide, and now also field accelerator
- Changed knight banner text to display buff values used in code
- Fixed this mod's assets somehow not being loaded when applying new survivor icons
- Added note about pyro only working when played as host

# 1.2.1

- Fixed compatibility with edited Armed Backpack and LordsItemEdits' Pocket I.C.B.M

# 1.2.0

- Fixed for latest SS2 and latest MonoDetour
- - Removed majority of SS2 fixes provided by this mod since they've been added officially
- Moved starstorm item edits from LordsItemEdits to here
- - Fixed a bunch of things related to the edited Armed Backpack
- - Added new alternative edit for Erratic Gadget that makes it like LordsItemEdits' Pocket ICBM but for lightning
- Added an edit for Portable Reactor
- - (Not from LordsItemEdits)
- Fixed halcyon shrines dropping an extra aurelionite fragment

## 1.1.0

- Added option to configure toxic elites off
- Gave Scavenger's Fortune and Seismic Oscillator an actual description
- Prevents empyrean elites from spawning in enemies returns' judgement stage
- Restored most shard drops
- Made Ultra elite healing a separate config option

## 1.0.1

- Fixed a few edits preventing the game from loading if SS2's beta is turned off

## 1.0.0

- First release