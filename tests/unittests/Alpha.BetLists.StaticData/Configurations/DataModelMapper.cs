using System.Collections.Generic;
using Alpha.StaticData.Mappers;

namespace Alpha.BetLists.StaticData.Configurations
{
    public class DataModelMapper : IDataModelMapper
    {
        public IDictionary<string, string> Maps
            => StaticDataMapper.ModelMaps;
    }
}
