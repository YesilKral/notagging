using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace NoTagging;

[MinimumApiVersion(150)]
public class NoTagging : BasePlugin
{
    public override string ModuleName => "No Tagging / Fast Velocity";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Custom";

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerHurt>((@event, info) =>
        {
            var victim = @event.Userid;
            if (victim != null && victim.IsValid && victim.PawnIsAlive)
            {
                var pawn = victim.PlayerPawn.Value;
                if (pawn != null && pawn.IsValid)
                {
                    pawn.VelocityModifier = 1.0f;
                }
            }
            return HookResult.Continue;
        }, HookMode.Post);

        RegisterListener<Listeners.OnServerPreEntityUpdate>(_ =>
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null && player.IsValid && player.PawnIsAlive)
                {
                    var pawn = player.PlayerPawn.Value;
                    if (pawn != null && pawn.IsValid && pawn.VelocityModifier < 1.0f)
                    {
                        pawn.VelocityModifier = 1.0f;
                    }
                }
            }
        });
    }
}
