namespace Alpha.BetLists.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Criteria.ProgressiveJackpot;
    using Fanex.Common;
    using Fanex.Data;

    public interface ProductDataAccess
    {
        IEnumerable<int> GetEnabledProductIdsForCustomer(GetEnableJackpotProductCriteria criteria);
    }

    public class ProductDataAccessImpl : ProductDataAccess
    {
        public IEnumerable<int> GetEnabledProductIdsForCustomer(GetEnableJackpotProductCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<int>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(Converter.ToInt(reader["productid"]));
                        }
                    }
                }
            }

            return result;
        }
    }
}