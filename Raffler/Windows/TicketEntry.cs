using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raffler.Data;

public class TicketEntry
{
    public string PlayerName { get; set; } = string.Empty;
    public int BaseTickets { get; set; }
    public int BonusTickets { get; set; }

    public int TotalTickets => BaseTickets + BonusTickets;

    public override string ToString()
        => $"{PlayerName} — {BaseTickets} + {BonusTickets} = {TotalTickets} tickets";
}

