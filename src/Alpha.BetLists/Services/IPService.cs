namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface IPService
    {
        bool IsVNIP(string ipAddress);
    }

    public class IPServiceImpl : IPService
    {
        private const string VNIPListCacheKey = "Alpha.BetLists.Service.IPService.VNIPList";
        private readonly IDynamicRepository dynamicRepository;

        public IPServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            double ipNumber = ConvertIPToNumber(ipAddress);
            var ipVnRange = GetVNIPs();

            return ipVnRange.Any(ip => ip.From <= ipNumber && ipNumber <= ip.To);
        }

        private IEnumerable<IPRange> GetVNIPs()
        {
            var cache = new CacheServiceImpl();

            var vietnamIPs = cache.GetOrSet(
                VNIPListCacheKey,
                () => dynamicRepository.Fetch<IPRange>(new GetIPVNCriteria()));

            return vietnamIPs;
        }

        private int ConvertIPToNumber(string ipAddress)
        {
            IPAddress parsedIpAddress;

            if (IPAddress.TryParse(ipAddress, out parsedIpAddress))
            {
                return BitConverter.ToInt32(parsedIpAddress.GetAddressBytes(), 0);
            }

            return 0;
        }
    }
}