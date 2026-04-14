[size=4]Installation:[/size]
[list]
[*][url=https://www.nexusmods.com/stardewvalley/mods/2400]Install the latest version of SMAPI[/url]
[*]Extract "ConfigureMachineSpeed.zip" into the "../Stardew Valley/Mods" folder
[*]Run the game using SMAPI.
[/list]

[size=4]Compatibility:[/size]
[list]
[*]Works with Stardew Valley 1.6 on Linux/Mac/Windows.
[*]Compatible with SMAPI 4.x.
[*]This mod does [u][b]NOT[/b][/u] work for Farmhands in multiplayer (host only).
[*]Untested with custom machines.
[*]No known mod conflicts.
[/list]

[size=4]How To Use:[/size]
[list]
[*]Run the game using SMAPI at least once to generate the "config.json" file in "../Stardew Valley/Mods/ConfigureMachineSpeed"
[*]Edit "config.json" to set the desired processing times for each machine.
[*]You can also use the [url=https://www.nexusmods.com/stardewvalley/mods/5098]Generic Mod Config Menu[/url] to configure the mod.
[*]Changes are applied automatically during gameplay.
[*]Enjoy!
[/list]

[size=4]Config File:[/size]
[list]
[*]The "config.json" file contains only the list of machines and their processing times.
[*]Each machine has three main fields: Id, Name and Time.
[*]Id and Name are identifiers and should not be changed, as doing so may cause the mod to stop working correctly.
[*]Time defines the processing time in in-game minutes.
[*]The minimum time is 10 minutes (always rounded to the nearest multiple of 10).
[*]The maximum time is limited to the machine's original time.
[*]Duplicate machines are not allowed in the config.
[/list]

[size=4]Mod Behavior:[/size]
[list]
[*]Machine times are automatically updated whenever the in-game clock updates (every 10 minutes).
[*]Machines that restart their cycle upon item collection (such as Crystalarium) have their time updated immediately after collection.
[*]Machines whose behavior depends on the start of the day are not affected by the mod (currently under investigation for future support).
[/list]

[size=4]Tested and Supported Machines:[/size]
[list]
[*]Bone Mill
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
[*]Cask
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