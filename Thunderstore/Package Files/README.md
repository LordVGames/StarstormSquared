# StarstormSquared

Most of these changes were from my own build of SS2 that I've had for a while. With SS2 now having the beta content included via a config option I've moved all my changes to SS2 over into a separate mod. I did these changes because I want the SS2 beta to be the best it can currently be, and also because I personally want some things changed/rebalanced.

The majority of the changes here can be toggled on/off. Many of the changes that fill in placeholders will be removed once the placeholder is officially filled in.

## Changes

### Items

<details>

<summary>Item Changes (click to open dropdown)</summary>

| Item  | New Description |
| :-------------: | ------------- |
| **Erratic Gadget** <br> *Change 1* <br> <img width="256" height="128" alt="texIconPickupErraticGadget" src="https://github.com/user-attachments/assets/c237f79c-a223-46a1-82ee-33103ae6bb6d" /> | <img width="587" height="73" alt="image" src="https://github.com/user-attachments/assets/1e0b8170-c849-4853-b5d1-5a3f74edc095" /> <br><br> This doubles lighting damage instead of doubling lightning procs to help performance some. Also, void lightning (a.k.a polylute) was never affected by erratic gadget, so I've added it to the description so it's known.
| **Erratic Gadget** <br> *Change 2* <br> <img width="256" height="128" alt="texIconPickupErraticGadget" src="https://github.com/user-attachments/assets/c237f79c-a223-46a1-82ee-33103ae6bb6d" /> | <img width="590" height="59" alt="Risk_of_Rain_2_VLG9tdx8G6" src="https://github.com/user-attachments/assets/dacc95d4-d89d-4855-b200-809b0d92d636" /> <br><br> This is for when you also don't want the special on-hit procs from erratic gadget too, now being just a big lightning damage multiplier similar to Pocket I.C.B.M from another mod. Will help performance even more.
| **Erratic Gadget** <br> *BOTH Changes* <br> <img width="256" height="128" alt="texIconPickupErraticGadget" src="https://github.com/user-attachments/assets/c237f79c-a223-46a1-82ee-33103ae6bb6d" /> | There's a config option to allow void lightning (aka polylute) to be affected anyways
| **Portable Reactor** <br> <br> <img width="256" height="128" alt="texIconPortableReactor" src="https://github.com/user-attachments/assets/55845c8a-f374-441e-ab12-d272f552c364" /> | <img width="604" height="57" alt="image" src="https://github.com/user-attachments/assets/2e53d4b0-ae5f-4fb2-95f7-7be89d7e0345" /> <br><br> I don't like how portable reactor is just a free win pass while active, and especially when it's stacked. I've changed the invulnerability to +100 armor, making it more of a constant oddly shaped opal effect while active so while you're still more tanky, the start of a stage isn't completely free.


</details>

- The Scavenger's Fortune and Seismic Oscillator items have a proper name and description

### Chirr

- Minions don't receive your healing items
- Enemies spawned from Relic of Termination are untamable
- Fixed Chirr's tame lingering past the debuff's expiration
- - Just a bandaid fix, idrk how to fully fix the problem

### Cyborg

- Buffed default primary damage (200% > 320%)
- Given a new survivor icon
- - It's not super good but it's better than the placeholder

### Knight

- Given a new survivor icon
- - It's not super good but it's better than the placeholder

### DU-T

- Fixed damage mode siphoning not giving charge
- Made both siphon modes scale with attack speed
- Gave skills placeholder icons instead of just white
- Gave names and descriptions to most of DU-T's skills
- - The text is based on SS1 DU-T's skill text
- - DU-T's secondary is still non existant so just give yourself hooks of heresy until one is made

### Duplicator Drones

- Ignores temporary items
- Picking up an item mid-dupe now barely adds any cooldown for the dupe drone
- Fixed an NRE with the search method
- - Basically less log clutter caused by them
- - Dupe drones might work more reliably too?

### Empyrean Elites

- Can be disabled
- No longer drops shard(s) on death
- Cannot spawn during EnemiesReturns' judgement sequence
- The amount stages needed for empyreans to level up is now configurable

### Toxic Elites

- Can be disabled

### Ethereal-Related

- Shards have actual icons along with new names/descriptions
- - With how much I was getting the shards I did this to do this to make them feel more complete
- Most shard drops have been restored
- Zanzan's trade menu & trade teleporter menus have new text for their menu titles
- Zanzan's trade menu now ignores temporary items
- - If you have both normal and temporary versions of an item, you can still trade with your normal versions until you have just temporary ones left.
- Item potential drops from the trade teleporter show for clients now
- - Not really a fix as I just made it use the vanilla potential that still works for clients instead of the cloned one
- Added option to make the ethereal sapling appear in some stage-specific spots
- - Will still go to newt altar spots on unsupported maps
- Added option to add a wandering chef to the stranger's hideout
- - Requires you to have Alloyed Collective on
- Added option to make all enemies able to become ethereal and/or ultra, even when they normally can't be an elite
- - Off by default
- Added options to let you configure how enemy levels are added with each ethereal
- Fixed the bonus levels (the green text) from ethereals showing up twice

### Ultra elites

- Have their subtitle from SS1
- The passive buff is a lil different
- - The 20% more movement speed and 10% more damage is given also given to the ultra itself
- - All allies except for the ultra get a tiny amount of % hp regen (under separate config option that is off by default)

### Event-Related

- Beta storms and elite events have new text
- - I know the existing text are silly placeholders, but eventually I wanted actual text for the events
- Super elites have new subtitles
- Super elite aspects have new(ish) names
- - I've gotten one via rerolling a dropped aspect so it's possible to get these with other mods
- Elite event objective has new text
- Added new icons for super elites
- - Credit to Gangrene for these!

### Other

- The lunar gambler has text for its name and interaction prompts.
- Lunar gambler is moved to between newt and the stage seers
- - So now it's away from the reroller and any biggerbazaar chests
- Jellyfish and acid larva cannot become any lategame elite (those being empyrean, ethereal, and ultra)
- Adds back an unused yet working equipment from Alloyed Collective
- - SS1 added back or re-implemented unused stuff so doing this fits this mod too imo
- - Equipment still requires Alloyed Collective to be on

### Other Fixes
- Added option to fix slate mines water texture being pink
- Added option to turn grass in stranger's hideout on/off
- - The grass is currently broken and solid pink