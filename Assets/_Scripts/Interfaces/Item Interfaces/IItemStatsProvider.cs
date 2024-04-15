using System.Collections.Generic;

public interface IItemStatsProvider
{
    List<string> ProvideStats(Item _item);
}