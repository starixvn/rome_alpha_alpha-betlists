namespace Alpha.BetLists.DataAccesses
{
    using System.Collections.Generic;
    using Criteria.BetBuilder;
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.DataAccesses;
    using Fanex.BetDetails.BetBuilder.Model;
    using Fanex.Data.Repository;

    public class BetBuilderDataAccess : IBetBuilderDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetBuilderDataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<BetBuilderDetailModel> GetBetBuilderDetail(IGetBetBuilderDetailParameter criteria)
        {
            var command = new GetBetBuilderDetailCriteria(criteria);

            return dynamicRepository.Fetch<BetBuilderDetailModel>(command);
        }
    }
}