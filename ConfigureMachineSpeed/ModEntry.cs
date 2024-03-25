using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Framework.ModLoading.Rewriters.StardewValley_1_6;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;

namespace ConfigureMachineSpeed;

public class ModEntry : Mod
{
    private ModConfig _config;

    private readonly float EPSILON = 0.01f;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        _config = helper.ReadConfig<ModConfig>();
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicking += OnUpdateTicking;
        helper.Events.Input.ButtonPressed += OnButtonPressed;
    }

    private ModConfig processConfig(ModConfig cfg)
    {
        if (cfg.UpdateInterval == 0)
        {
            cfg.UpdateInterval = 1u;
        }
        MachineConfig[] machines = cfg.Machines;
        foreach (MachineConfig machineConfig in machines)
        {
            if (!machineConfig.UsePercent && machineConfig.Time <= 0f)
            {
                machineConfig.Time = 10f;
            }
        }
        return cfg;
    }

    private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
    {
        if (Context.IsMainPlayer)
        {
            this.configureAllMachines();
        }
    }

    private void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
    {
        if (Context.IsMainPlayer && e.IsMultipleOf(this._config.UpdateInterval))
        {
            this.configureAllMachines();
        }
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
        IEnumerable<GameLocation> locations = ModEntry.GetLocations();
        MachineConfig[] machines = this._config.Machines;
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
                        {
                            this.configureMachine(cfg, current2.Value);
                        }
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
            float num = 1f;
            switch (val.heldObject.Value.ParentSheetIndex)
            {
                case 426:
                    num = 4f;
                    break;

                case 424:
                    num = 4f;
                    break;

                case 459:
                    num = 2f;
                    break;

                case 303:
                    num = 1.66f;
                    break;

                case 346:
                    num = 2f;
                    break;
            }
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
                obj.MinutesUntilReady = Math.Max((int)((float)obj.MinutesUntilReady * cfg.Time / 100f / 10f) * 10 - 2, 8);
            }
            else if (!cfg.UsePercent)
            {
                obj.MinutesUntilReady = Math.Max((int)(cfg.Time / 10f) * 10 - 2, 8);
            }
        }
    }

    public static IEnumerable<GameLocation> GetLocations()
    {
        return Game1.locations.Concat(from location in Game1.locations.OfType<BuildableGameLocationFacade>()
                                      from building in (IEnumerable<Building>)location.buildings
                                      where building.indoors.Value != null
                                      select building.indoors.Value);
    }
}