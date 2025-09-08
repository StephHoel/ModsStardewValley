using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Events;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        var configurator = new MachineConfigurator();

        var config = helper.ReadConfig<ModConfig>();

        var onGameLaunched = new OnGameLaunched(config, helper, ModManifest);
        var onSaveLoaded = new OnSaveLoaded(config, configurator);
        var onDayStarted = new OnDayStarted(config, configurator);
        var onUpdateTicking = new OnUpdateTicking(config, configurator);
        var onButtonPressed = new OnButtonPressed(config, helper);

        var newMachines = Machines.GetNewMachines();

        if (config.Machines.Length != newMachines.Length)
        {
            var machinesExcept = newMachines.Except(config.Machines, new MachinesComparer());
        
            var mac = config.Machines.Concat(machinesExcept).ToArray();
            
            config.Machines = mac;
            
            helper.WriteConfig(config);
        }

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.GameLoop.DayStarted += onDayStarted.Main;
        helper.Events.GameLoop.SaveLoaded += onSaveLoaded.Main;
        helper.Events.GameLoop.UpdateTicking += onUpdateTicking.Main;
        helper.Events.Input.ButtonPressed += onButtonPressed.Main;
    }
}