namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using Alpha.BetLists.Configurations;
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using Utils;
    using ViewModels.WinLoss;

    public class WinLossQuery : BaseQuery, IRequest<WinLossViewModel>
    {
        protected const int NegativeThree = -3;
        private const int DefaultMaxTotalRowView = 10000;
        private const int DefaultMaxTotalRowExportExcel = 50000;

        public WinLossQuery(IUser viewer) : base(viewer)
        {
            MaxTotalRowConfig = MaxTotalRow = string.IsNullOrWhiteSpace(AppSettings.GetConfigValue("Betlist:MaxTotalRowView")) ? 
                DefaultMaxTotalRowView : int.Parse(AppSettings.GetConfigValue("Betlist:MaxTotalRowView"));
            MaxTotalRowExportExcelConfig = MaxTotalRowExportExcel = string.IsNullOrWhiteSpace(AppSettings.GetConfigValue("Betlist:MaxTotalRowExportExcel")) ?
                DefaultMaxTotalRowExportExcel : int.Parse(AppSettings.GetConfigValue("Betlist:MaxTotalRowExportExcel"));
        }

        public bool IsGroupHeadline { get; set; }

        public virtual DateTime MinDate => IsGroupHeadline
            ? SystemTime.Today.AddMonths(NegativeThree).FirstDayOfMonth()
            : DateTime.MinValue;

        public int MaxTotalRowConfig { get; }

        public int MaxTotalRow { get; }

        public int MaxTotalRowExportExcelConfig { get; }

        public int MaxTotalRowExportExcel { get; }
    }
}