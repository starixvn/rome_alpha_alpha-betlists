namespace Alpha.BetLists.Site.Handlers.Parlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels.Parlay;

    public interface ComboParlayDetailWorkbook
    {
        Workbook<IEnumerable<ComboParlayDetailWrapper>> UseTemplate(TemplateInfo templateInfo);

        Workbook<IEnumerable<ComboParlayDetailWrapper>> Sheet(string sheetName);

        Workbook<IEnumerable<ComboParlayDetailWrapper>> Title(string titleText);

        Workbook<IEnumerable<ComboParlayDetailWrapper>> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        ComboParlayDetailWorkbook SetData(ComboParlayDetailViewModel comboParlayDetails);

        ComboParlayDetailWorkbook SetRowMapper(
            Mapper<IEnumerable<ComboParlayDetailWrapper>> detailsMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> detailsSubTotalMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> footerOutstandingMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> footerTotalWinLostMapper);
    }

    public class ComboParlayDetailWorkbookImpl : NpoiWorkbook<IEnumerable<ComboParlayDetailWrapper>>, ComboParlayDetailWorkbook
    {
        private int currentRowIndex;

        public ComboParlayDetailViewModel ComboParlayDetails { get; private set; }

        public Row OddRowTemplate { get; private set; }

        public Row EvenRowTemplate { get; private set; }

        public Row SubTotalRowTemplate { get; private set; }

        public Row FooterRowTemplate { get; private set; }

        public Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsMapper { get; private set; }

        public Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsSubTotalMapper { get; private set; }

        public Mapper<IEnumerable<ComboParlayDetailWrapper>> FooterOutstandingMapper { get; private set; }

        public Mapper<IEnumerable<ComboParlayDetailWrapper>> FooterTotalWinLostMapper { get; private set; }

        public ComboParlayDetailWorkbook SetData(ComboParlayDetailViewModel comboParlayDetails)
        {
            ComboParlayDetails = comboParlayDetails;

            return this;
        }

        public ComboParlayDetailWorkbook SetRowMapper(
            Mapper<IEnumerable<ComboParlayDetailWrapper>> detailsMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> detailsSubTotalMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> footerOutstandingMapper,
            Mapper<IEnumerable<ComboParlayDetailWrapper>> footerTotalWinLostMapper)
        {
            DetailsMapper = detailsMapper;
            DetailsSubTotalMapper = detailsSubTotalMapper;
            FooterOutstandingMapper = footerOutstandingMapper;
            FooterTotalWinLostMapper = footerTotalWinLostMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            SubTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            FooterRowTemplate = sheet.GetRow(currentRowIndex);

            return sheet;
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (ComboParlayDetails.ComboParlayDetailWrapperGroups.Any())
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var detailGroup in ComboParlayDetails.ComboParlayDetailWrapperGroups)
            {
                FillDetails(sheet, detailGroup);
                FillSubTotal(sheet, detailGroup);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            FillFooterValue(sheet, isTotalWinLost: false);
            FillFooterValue(sheet, isTotalWinLost: true);
        }

        private void FillFooterValue(Sheet sheet, bool isTotalWinLost)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(FooterRowTemplate);
            FillCells(
                row,
                ComboParlayDetails.ComboParlayDetailWrappers,
                isTotalWinLost ? FooterTotalWinLostMapper.Mappings : FooterOutstandingMapper.Mappings);
        }

        private void FillDetails(Sheet sheet, IEnumerable<ComboParlayDetailWrapper> detailGroup)
        {
            var beginRowIndex = currentRowIndex;
            var isEven = false;

            foreach (var detail in detailGroup)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(isEven ? EvenRowTemplate : OddRowTemplate);
                isEven = !isEven;
                FillCells(row, new List<ComboParlayDetailWrapper> { detail }, DetailsMapper.Mappings);
            }

            var endRowIndex = currentRowIndex - 1;
            MergeDetailsDateTimeCells(sheet, beginRowIndex, endRowIndex);
        }

        private void FillSubTotal(Sheet sheet, IEnumerable<ComboParlayDetailWrapper> detailGroup)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(SubTotalRowTemplate);
            FillCells(row, detailGroup, DetailsSubTotalMapper.Mappings);
        }

        private void MergeDetailsDateTimeCells(Sheet sheet, int beginRowIndex, int endRowIndex)
        {
            var npoiSheet = sheet as NpoiSheet;
            npoiSheet.Sheet.AddMergedRegion(new CellRangeAddress(beginRowIndex, endRowIndex, 0, 0));
        }
    }
}