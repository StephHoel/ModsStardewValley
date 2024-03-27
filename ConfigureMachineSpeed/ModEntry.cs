using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using Utils;
using Utils.Config;

namespace ConfigureMachineSpeed;

public class ModEntry : Mod
{
    private ModConfig _config;

    private readonly float EPSILON = 0.01f;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        _config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
        helper.Events.Input.ButtonPressed += OnButtonPressed;
    }

    private ModConfig processConfig(ModConfig cfg)
    {
        if (cfg.UpdateInterval == 0)
            cfg.UpdateInterval = 1u;

        MachineConfig[] machines = Machines.GetMachines();

        foreach (MachineConfig machineConfig in machines)
        {
            if (!machineConfig.UsePercent && machineConfig.Time <= 0f)
                machineConfig.Time = 10f;
        }

        return cfg;
    }

    private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
    {
        if (Context.IsMainPlayer)
            this.configureAllMachines();
    }

    private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
    {
        if (Context.IsMainPlayer && e.IsMultipleOf(this._config.UpdateInterval))
            this.configureAllMachines();
    }

    private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        if (Context.IsPlayerFree && Game1.currentMinigame == null && (SButton?)e.Button == this._config.ReloadConfigKey)
        {
            _config = processConfig(Helper.ReadConfig<ModConfig>());
            Game1.addHUDMessage(new(I18n.Message(), 2));
        }
    }

    private void configureAllMachines()
    {
        var locations = Locations.GetLocations();

        var machines = Machines.GetMachines();

        foreach (MachineConfig cfg in machines)
        {
            foreach (GameLocation item in locations)
            {
                bool func(KeyValuePair<Vector2, StardewValley.Object> p) => p.Value.name == cfg.Name;
                var pairs = item.objects.Pairs;
                var enumerator2 = pairs.GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        var current2 = enumerator2.Current;

                        if (func(current2))
                            this.configureMachine(cfg, current2.Value);
                    }
                }
                finally
                {
                    enumerator2.Dispose();
                }
            }
        }
    }

    private void configureMachine(MachineConfig cfg, StardewValley.Object obj)
    {
        Cask val = (Cask)(object)((obj is Cask) ? obj : null);

        if (val != null && obj.heldObject.Value != null)
        {
            float num = val.heldObject.Value.ParentSheetIndex switch
            {
                426 => 4f,
                424 => 4f,
                459 => 2f,
                303 => 1.66f,
                346 => 2f,
                _ => 1f
            };

            if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > this.EPSILON && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
            {
                val.agingRate.Value = num * 100f / cfg.Time;
                val.agingRate.Value = (float)Math.Round(val.agingRate.Value, 2);
                NetFloat agingRate = val.agingRate;
                agingRate.Value += 0.001f;
            }
            else if (!cfg.UsePercent && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
            {
                val.agingRate.Value = val.daysToMature.Value / num * 1440f / cfg.Time;
                val.agingRate.Value = (float)Math.Round(val.agingRate.Value, 2);
                NetFloat agingRate2 = val.agingRate;
                agingRate2.Value += 0.001f;
            }
        }
        else if (obj.MinutesUntilReady % 10 != 8 && obj.MinutesUntilReady > 0)
        {
            if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > this.EPSILON)
            {
                obj.MinutesUntilReady = Math.Max((int)(obj.MinutesUntilReady * cfg.Time / 100f / 10f) * 10 - 2, 8);
            }
            else if (!cfg.UsePercent)
            {
                obj.MinutesUntilReady = Math.Max((int)(cfg.Time / 10f) * 10 - 2, 8);
            }
        }
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

        if (configMenu is null) return;

        // register mod
        configMenu.Register(
            mod: this.ModManifest,
            reset: () => _config = new ModConfig(),
            save: () => this.Helper.WriteConfig(_config)
        );

        configMenu.AddSectionTitle(
            mod: ModManifest,
            text: I18n.ConfigTitleGeneralOptions
        );

        // UpdateInterval
        configMenu.AddNumberOption(
            mod: ModManifest,
            name: I18n.ConfigUpdateIntervalName,
            getValue: () => _config.UpdateInterval,
            setValue: val => _config.UpdateInterval = (uint)val,
            min: 1,
            max: 10
        );

        // ReloadConfigKey
        configMenu.AddKeybind(
            mod: ModManifest,
            name: I18n.ConfigReloadConfigKeyName,
            getValue: () => (SButton)_config.ReloadConfigKey,
            setValue: value => _config.ReloadConfigKey = value
        );

        // Machines
        foreach (var machine in Machines.GetMachines())
        {
            // Name
            configMenu.AddSectionTitle(
                mod: ModManifest,
                text: () =>
                {
                    return machine.Name switch
                    {
                        "Bee House" => I18n.BeeHouse(),
                        "Cask" => I18n.Cask(),
                        "Charcoal Kiln" => I18n.CharcoalKiln(),
                        "Cheese Press" => I18n.CheesePress(),
                        "Crystalarium" => I18n.Crystalarium(),
                        "Furnace" => I18n.Furnace(),
                        "Heavy Furnace" => I18n.HeavyFurnace(),
                        "Incubator" => I18n.Incubator(),
                        "Keg" => I18n.Keg(),
                        "Lightning Rod" => I18n.LightningRod(),
                        "Loom" => I18n.Loom(),
                        "Mayonnaise Machine" => I18n.MayonnaiseMachine(),
                        "Oil Maker" => I18n.OilMaker(),
                        "Preserves Jar" => I18n.PreservesJar(),
                        "Recycling Machine" => I18n.RecyclingMachine(),
                        "Seed Maker" => I18n.SeedMaker(),
                        "Slime Egg-Press" => I18n.SlimeEggPress(),
                        "Slime Incubator" => I18n.SlimeIncubator(),
                        "Tapper" => I18n.Tapper(),
                        "Worm Bin" => I18n.WormBin(),
                        "Fish Smoker" => I18n.FishSmoker(),
                        "Dehydrator" => I18n.Dehydrator(),
                        "Solar Panel" => I18n.SolarPanel(),
                        "Ostrich Incubator" => I18n.OstrichIncubator(),
                        "Heavy Tapper" => I18n.HeavyTapper(),
                        "Bone Mill" => I18n.BoneMill(),
                        "Geode Crusher" => I18n.GeodeCrusher(),
                        _ => string.Empty,
                    };
                }
            );

            // Time  - "Time": 1.0,
            configMenu.AddNumberOption(
                mod: ModManifest,
                name: I18n.ConfigTimeName,
                getValue: () => machine.Time,
                setValue: val => machine.Time = (int)val,
                min: 1,
                max: 100
            );

            // UsePercent - "UsePercent": true
            configMenu.AddBoolOption(
                mod: ModManifest,
                name: I18n.ConfigPercentName,
                getValue: () => machine.UsePercent,
                setValue: val => machine.UsePercent = val

            );
        }
    }
}