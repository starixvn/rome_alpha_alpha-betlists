namespace Alpha.BetLists.Site.Requests.JackpotDetails
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class JackpotDetailsExcelQuery : JackpotDetailsQuery, IRequest<ExcelFileViewModel>
    {
        public JackpotDetailsExcelQuery(IUser viewer)
            : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}