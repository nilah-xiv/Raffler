using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace Raffler;

public enum BogoType
{
    None,
    Buy1Get1,
    Buy1Get2,
    EveryOther,
    MaxPerPurchase
}


[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

    //raffle settings below
    public BogoType RaffleBogoType = BogoType.Buy1Get1;
    public int BogoBonusTickets = 1;
    public float TicketCost = 500f;


    // the below exist just to make saving less cumbersome

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
