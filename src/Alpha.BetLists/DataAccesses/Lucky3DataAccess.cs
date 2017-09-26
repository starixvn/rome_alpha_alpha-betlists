namespace Alpha.BetLists.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.Lucky3;
    using Fanex.BetDetails.Lucky3.Criteria;
    using Fanex.BetDetails.Lucky3.DataAccesses;
    using Fanex.BetDetails.Lucky3.Models;
    using Fanex.Data.Repository;

    public class Lucky3DataAccess : ILucky3DataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public Lucky3DataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<Lucky3DetailModel> GetLucky3Detail(IGetLucky3DetailParameter criteria)
        {
            var command = new GetLucky3DetailCriteria(criteria);

            return dynamicRepository.Fetch<Lucky3DetailModel>(command);
        }
    }
}