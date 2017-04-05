namespace Alpha.BetLists.Builders.Models
{
    using System.Collections.Generic;
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using Fanex.Caching;

    public static class PTCommFactory
    {
        private const string PTCommCacheKey = "Alpha.BetLists.Builders.PTComm";

        public static PTComm CreateInstance(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
        {
            var cacheService = new CacheServiceImpl();

            if (ticket.BetTypeId == (33 | 1233))
            {
                return cacheService.GetOrSet(
                    $"{PTCommCacheKey}.PTComm33",
                    () => { return new PTComm33(ticket, ticketData, userLevel).InitPTCommData(); });
            }

            if (ticket.BetTypeId == 500)
            {
                return cacheService.GetOrSet(
                   $"{PTCommCacheKey}.PTComm500",
                   () => { return new PTComm500(ticket, userLevel).InitPTCommData(); });
            }

            if (ticket.BetTypeId == 501)
            {
                return cacheService.GetOrSet(
                    $"{PTCommCacheKey}.PTComm501",
                    () => { return new PTComm501(ticket, userLevel).InitPTCommData(); });
            }

            if (ticket.BetTypeId == (71 | 73))
            {
                return cacheService.GetOrSet(
                   $"{PTCommCacheKey}.PTComm71",
                   () => { return new PTComm71(ticket, userLevel).InitPTCommData(); });
            }

            return cacheService.GetOrSet(
                   $"{PTCommCacheKey}.PTComm",
                   () => { return new PTComm(ticket, userLevel).InitPTCommData(); });
        }
    }
}