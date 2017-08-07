using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Alpha.BetLists.Models;
using Alpha.BetLists.StaticData.Configurations;
using Alpha.StaticData.NPOIExtensions;
using Alpha.StaticData.Repositories;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;
using NPOI.SS.UserModel;

namespace Alpha.BetLists.StaticData.DataAccesses
{
    public class BetListExcelRepository : ExcelRepository
    {
        public override void InitMapper()
        {
            Mapper = new DataModelMapper();
        }

        public override IWorkbook LoadWorkbook(string excelFileName)
        {
            var templatePath = HttpContext.Current.Server.MapPath($"~/App_Data/ExcelData/{excelFileName}.xls");
            return WorkbookExtension.Load(templatePath);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var templateKey = criteria.GetType().Name;

            if (!StaticDataMapper.CriteriaMaps.ContainsKey(templateKey))
            {
                return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(Enumerable.Empty<TFirst>(), Enumerable.Empty<TSecond>());
            }

            var template = StaticDataMapper.CriteriaMaps[templateKey];
            var workbook = LoadWorkbook(template);

            var tickets = workbook?.GetData("BetTickets", CreateTicket);
            var ticketData = workbook?.GetData("ReferenceData", CreateTicketData);

            return Tuple.Create(Enumerable.Cast<TFirst>(tickets), Enumerable.Cast<TSecond>(ticketData));
        }

        private static ITicketData CreateTicketData(IRow row)
            => TicketDataMapper.Map(row);

        private static TicketDto CreateTicket(IRow row)
            => TicketDtoMapper.Map(row);
    }
}
