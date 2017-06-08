using Alpha.BetLists.AppResources;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.CancelledBet;
using Alpha.BetLists.Site.ViewModels;
using Alpha.BetLists.Site.ViewModels.CancelledBet;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using System.Threading.Tasks;

using static ONEbook.SpreadSheets.Column;

namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    public class CancelledBetExcelHandler : BetListBaseHandler, IAsyncRequestHandler<CancelledBetExcelQuery, ExcelFileViewModel>
    {
        private readonly CancelledBetService Service;
        private readonly Workbook<TicketDto> Workbook;
        private int currentRowIndex;
        public CancelledBetExcelHandler(
            CancelledBetService service,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<TicketDto> workbook) : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            Service = service;
            Workbook = workbook;
            currentRowIndex = 1;
        }

        public async Task<ExcelFileViewModel> Handle(CancelledBetExcelQuery message)
        {
            var parameter = new CancelledBetParam(message.ViewerLevel, message.ViewerId, message.MasterId, message.AgentId, message.MemberId, message.Language);
            var cancelledBet = await Service.GetCancelledBet(parameter);

            var viewModel = new CancelledBetViewModel(message, cancelledBet, Builder, TicketHelper);

            var fileContent = Workbook
                .UseTemplate(GetTemplateInfo(message.ViewerLevel))
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(cancelledBet.BetListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))                
                .Sheet(Resources.CancelledBet)
                .ExportToBytes();

            return new ExcelFileViewModel(Resources.CancelledBet, fileContent);
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
           => GetExcelTemplatePath($"CancelledBet\\{viewerLevel.DisplayName}");

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));           

            return mapper;
        }
    }
}