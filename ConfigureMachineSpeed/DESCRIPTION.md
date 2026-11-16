[size=4]Installation:[/size]
[list]
[*][url=https://www.nexusmods.com/stardewvalley/mods/2400]Install SMAPI 4.2.0 or later[/url]
[*]Extract "ConfigureMachineSpeed.zip" into the "Stardew Valley/Mods" folder
[*]Run the game using SMAPI.
[/list]

[size=4]Compatibility:[/size]
[list]
[*]Requires Stardew Valley 1.6.x.
[*]Requires SMAPI 4.2.0 or later.
[*]Generic Mod Config Menu is optional.
[*]This mod does [u][b]NOT[/b][/u] work for Farmhands in multiplayer (host only).
[*]Untested with custom machines.
[*]No known mod conflicts.
[/list]

[size=4]How To Use:[/size]
[list]
[*]Run the game using SMAPI at least once to generate the "config.json" file in "Stardew Valley/Mods/ConfigureMachineSpeed"
[*]Edit "config.json" to set the desired processing times for each machine.
[*]You can also use the [url=https://www.nexusmods.com/stardewvalley/mods/5098]Generic Mod Config Menu[/url] to configure the mod.
[*]Changes are applied automatically during gameplay.
[*]Enjoy!
[/list]

[size=4]Config File and Processing Rules:[/size]
[list]
[*]The "config.json" file contains machine IDs and their configured processing times.
[*]The qualified machine ID is the source of truth. The legacy Name field is read only to migrate older configurations to IDs; unresolved legacy names are skipped safely and reported in the SMAPI log.
[*]Time defines the processing time in in-game minutes. Values are normalized to a minimum of 10 minutes and rounded to the nearest multiple of 10.
[*]The configured time is an upper bound: a machine is never made slower than its original processing time.
[*]Qualified IDs take precedence over legacy internal-name matching. Explicit exclusions take precedence over all configured values.
[*]Duplicate machine IDs are collapsed during normalization.
[*]Casks use a dedicated aging path. The mod advances aging in a bounded loop of at most 100 DayUpdate calls and stops when the held item changes or its quality increases.
[*]Machines whose processing depends on DayUpdate, including bee houses, coffee makers, crab pots, dehydrators, worm bins, mushroom logs and incubators, are intentionally excluded from the accelerated processing path.
[*]Tapper, Heavy Tapper, Bait Maker, Lightning Rod, Solar Panel and Anvil are also excluded.
[/list]

[size=4]Mod Behavior:[/size]
[list]
[*]Machine times are automatically updated whenever the in-game clock updates (every 10 minutes) and at the start of the day.
[*]Machines that restart their cycle upon item collection (such as Crystalarium) have their time updated immediately after collection.
[*]Day-Update-dependent machines still require their normal game update cycle.
[*]The GMCM configuration is registered once during the game launch lifecycle.
[/list]

[size=4]Tested and Supported Machines:[/size]
[list]
[*]Bone Mill
[*]Cask
[*]Charcoal Kiln
[*]Cheese Press
[*]Crystalarium
[*]Deconstructor
[*]Fish Smoker
[*]Furnace
[*]Geode Crusher
[*]Heavy Furnace
[*]Keg
[*]Loom
[*]Mayonnaise Machine
[*]Oil Maker
[*]Preserves Jar
[*]Recycling Machine
[*]Seed Maker
[*]Slime Egg-Press
[*]Wood Chipper
[/list]

[size=4]Day-Update Dependent Machines:[/size]
(these still require sleeping to complete their process)
[list]
[*]Bee House
[*]Coffee Maker
[*]Crab Pot
[*]Dehydrator
[*]Deluxe Worm Bin
[*]Incubator
[*]Mushroom Log
[*]Ostrich Incubator
[*]Slime Incubator
[*]Worm Bin
[/list]

[size=4]Original [url=https://www.nexusmods.com/stardewvalley/mods/3519]mod[/url] by [url=https://www.nexusmods.com/stardewvalley/users/65621396]BayesianBandit[/url][/size]

[size=4]Chinese translation by [url=https://www.nexusmods.com/stardewvalley/users/108837033]bancrogft[/url][/size]

[size=4][url=https://github.com/StephHoel/ModsStardewValley/tree/main/ConfigureMachineSpeed]Source Code[/url][/size]

[size=4]For new translations or improvements, open an issue or submit a pull request on GitHub.[/size]
