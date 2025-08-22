# Charm Patch

This is a collection of small changes that I think improve the balance of certain charms in the game, each of which can be toggled from the menu.

WARNING - Any changes you make to settings in the menu won't take effect until you've saved at a bench.

Berserker's Fury - Fury of the Fallen has a chance while active to negate damage taken

Dark Dashmaster - Dashmaster reduces the cooldown of Shadow Dash in addition to regular Dash

Joni's Kindness - Joni's Blessing gives more Lifeblood Masks

Merciful Melody - Carefree Melody has a chance to heal when triggered

Outer Shell - Baldur Shell absorbs more attacks before breaking

## Grubberfly's Elegy
Grubberfly's Reach - Longnail and Mark of Pride increase the range of Grubberfly's Elegy

Joni's Elegy - Grubberfly's Elegy always triggers when the player is at full health, even if they took damage while equipped with Joni's Blessing

## Nail Arts
Critical Blow - Heavy Blow increases damage dealt by Nail Arts

Mantis Arts - Longnail and Mark of Pride increase the range of Nail Arts

Mighty Arts - Fragile/Unbreakable Strength increases the damage dealt by Nail Arts

Quick Arts - Quick Slash reduces the charge time of Nail Arts

## Integrations with other mods

Blue Hive - TheMathGeek314's Unlimited Hiveblood mod heals Lifeblood Masks given by Joni's Blessing in addition to regular ones.

## Recommended Charm Changer Settings

Set Flukenest cost to 2

Set Dreamshield cost to 2

Set Grimmchild Attack Cooldown to 1.33

Instead of using Joni's Kindness, you can set Joni's Blessing Health Increase to 60%

Instead of using Merciful Melody, you can also increase Carefree Melody's block chance by 5% at all levels

## Special Thanks To
timbobaggins for the idea behind Critical Blow

## Patch Notes
1.5.1.0
- Bug fix so game works without Charm Changer

1.5.0.0
- Integrated with DanielSteginkUtils
- Added data caching between saves so patches don't slow down the game by checking external mod settings
- Reduced Critical Blow's damage bonus to 27%
- Modified Quick Arts to use Charm Changer's settings instead of the active cooldowns for calculations
- Added a check to Blue Hive so it doesn't trigger multiple times. It also no longer heals blue health that doesn't come from Joni's Blessing
- Reduced Berserker's Fury block chance to 3%
- Reduced Merciful Melody's healing chance to about 7%

1.4.2.0
-	Bug fix - Quick Arts didn't actually work as intended
-	Bug fix - Dark Dashmaster now synergizes with mods that modify Shadow Dash cooldown, such as Pale Court
-	Outer Shell now synergizes with Exaltation's Stone Shell
-	Minor code updates

1.4.1.1
-	Bug fix where Grubberfly's Reach produced sideways beam attacks instead of vertical ones when attacking up/down

1.4.1.0
-	Bug fix where Grubberfly's Reach stacked, causing Grubberfly beams from FOTF to grow repeatedly

1.4.0.0
-	Modified patches to use Charm Changer settings if installed
-	Numerous bug fixes. It's embarassing how sloppy my testing of 1.3 must've been
	- 	Grubberfly's Reach interacted with Dreamshield instead of Mark of Pride
	-	Mighty Arts broken by unnecessary type check
	-	Blue Hive triggered without Hiveblood equipped
	-	Menu glitch where Mighty Arts and Mantis Arts toggled each other

1.3.0.0
-	3 new patches
	-	Grubberfly's Reach increases Grubberfly's Elegy range when Longnail and/or Mark of Pride are equipped
	-	Joni's Elegy allows Grubberfly's Elegy to trigger after taking damage while Joni's Elegy is equipped, so long as the damage has healed
	-	Mighty Arts increases the damage dealt by Nail Arts
-	Rebalanced Quick Arts to further reduce the cooldown of Nail Arts by the same amount as regular nail strikes
-	Reorganized menu to use a nested structure
-	Fixed Blue Hive's graphics glitch

1.2.1.0
-	Re-added Outer Shell. I misread that Charm Changer lets you add more than 4 hits; thank you timbobaggins for noticing this error.
-	Added recommendation to reduce Flukenest cost to 2

1.2.0.1
-	New patch Quick Arts reduces the charge time of Nail Arts when Quick Slash is equipped
-	Critical Blow's damage buffed from 30% to 40%

1.2.0.0
-	Added 2 new patches
	-	Critical Blow increases the damage dealt by Nail Arts when Heavy Blow is equipped
	-	Mantis Arts increases the range of Nail Arts when Longnail and Mark of Pride are equipped
-	Fixed Dark Dashmaster so that if you turn it on/off, its effects take place immediately instead of when you update your charms

1.1.0.0
-	Added menu options so upgrades could be toggled. 
-	Changed Dark Dashmaster's cooldown reduction from 33% to 40%
-	Removed the following upgrades as they are already handled by Exempt-Medic's Charm Changer mod:
	-	Cheaper Dreamshield - Reduced cost of Dreamshield by 1. Use CharmChanger to set Dreamshield cost to 2.
	-	Ferocious Grimmchild - Increased Grimmchild attack speed by 33%. Use CharmChanger to set Grimmchild Attack Cooldown to 1.33.
	-	Outer Shell - Added 2 extra hits to Baldur Shell. Use Charm Changer to set Baldur Shell Blocks to 6 instead of 4.
	-	NOTE - Joni's Kindness increases the the health gained from Joni's Blessing by 2. This is still available, but you can achieve a similar effect if you use Charm Changer to set Joni's Blessing Health Increase to 60%.
