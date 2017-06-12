namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Criteria;
    using Criteria.MemberBetList;
    using Criteria.RunningBetList;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StaticRepository : DynamicRepository, IDynamicRepository
    {
        public override IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            if (criteria is GetProductsCriteria)
            {
                return (IEnumerable<T>)ProductExcelRepository.GetProducts();
            }

            if (criteria is GetScoreMapBetTypesCriteria)
            {
                return (IEnumerable<T>)ForcastExcelRepository.GetScoreMaps();
            }

            return base.Fetch<T>(criteria);
        }

        public new async Task<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria)
        {
            if (criteria is GetBetListRunningByDateCriteria)
            {
                var betlist = await RunningBetListExcelRepository.GetRunningBetList();
                var result = Tuple.Create(Enumerable.Cast<TFirst>(betlist.Item1), Enumerable.Cast<TSecond>(betlist.Item2));

                return await Task.FromResult(result);
            }

            if (criteria is GetBetListAllByDateCriteria)
            {
                var betlist = await MemberBetListExcelRepository.GetMemberBetList();
                var result = Tuple.Create(Enumerable.Cast<TFirst>(betlist.Item1), Enumerable.Cast<TSecond>(betlist.Item2));

                return await Task.FromResult(result);
            }

            return await base.FetchMultipleAsync<TFirst, TSecond>(criteria);
        }
    }
}