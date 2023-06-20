namespace Alpha.BetLists.DataAccesses
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.IPInfo;
    using Alpha.IPInfo.DataAccess;
    using Alpha.IPInfo.Models;
    using Fanex.Data.Repository;

    public class IPInfoDataAccess : IIPInfoDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public IPInfoDataAccess(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public Task<IEnumerable<IPV4Information>> GetIPV4Info(string ipList)
            => dynamicRepository.FetchAsync<IPV4Information>(new GetIPV4InfoCriteria(ipList));

        public Task<IEnumerable<IPV4Information>> GetIPV4InfoByTopCountries(string countries)
            => dynamicRepository.FetchAsync<IPV4Information>(new GetIPV4InfoByTopCountryCriteria(countries));

        public Task<IEnumerable<IPV6Information>> GetIPV6Info(string ipList)
            => dynamicRepository.FetchAsync<IPV6Information>(new GetIPV6InfoCriteria(ipList));
    }
}
