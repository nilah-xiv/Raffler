using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace Raffler;

public enum BogoType
{
    None,           // No bonus
    Custom,         // Bonus Tickets field shown
    Buy1Get1,       // 1 bonus per base ticket
    LimitedSupply   // Pull from a session-wide bonus pool (BOGO Session Limit)
}



[Serializable]
public class Configuration : IPluginConfiguration
{
    public bool ShowDebugOption { get; set; } = true;
    public int StartingPotMillions { get; set; } = 10;
    public int BogoSessionLimit { get; set; } = 0;

    public int StartingGil { get; set; } = 0;

    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

    //raffle settings below
    public BogoType RaffleBogoType = BogoType.Buy1Get1;
    public int BogoBonusTickets = 1;
    public float TicketCost = 500f;


    

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
