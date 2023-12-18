using System.Collections.Generic;
using System.Data;
using Alpha.BetLists.Criteria;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Fanex.Common.Extensions;
using Fanex.Data;

namespace Alpha.BetLists.DataAccesses
{
    public interface ProductDataAccess
    {
        IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param);
    }

    public class ProductDataAccessImpl : ProductDataAccess
    {
        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param)
        {
            var products = new List<Product>();
            var criteria = new GetProductsByGroupHeadlineCriteria(param.CustId);
            if (criteria.IsInvalid())
            {
                return products;
            }
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            products.Add(BuildProduct(reader));
                        }
                    }
                }
            }

            return products;
        }

        private static Product BuildProduct(IDataReader reader)
        {
            return new Product(
                reader.ToInt("ProductID"),
                reader.ToString("ProductName"),
                reader.ToDateTime2("EndOfSupportDate"),
                reader.ToBool("isShow"),
                reader.ToInt("Type"),
                reader.ToInt("OrderNo"));
        }
    }
}