namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Helpers;

    public class FilterConfigurationViewModel
    {
        public FilterConfigurationViewModel(bool showWelcomePopup)
        {
            ShowWelcomePopup = showWelcomePopup;
        }

        public bool ShowWelcomePopup { get; }

        public IEnumerable<SelectListItem> BetTypes
        {
            get
            {
                foreach (var item in AppSettings.MonitoringBetsBettypes)
                {
                    yield return new SelectListItem
                    {
                        Text = BetTypeHelper.GetBetTypeName(int.Parse(item)),
                        Value = item,
                        Selected = true
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> LiveStatus
        {
            get
            {
                yield return BuildSelectListItem(AppResources.Resources.All, LiveStatusConstants.All, true);
                yield return BuildSelectListItem(AppResources.Resources.Live, LiveStatusConstants.Live);
                yield return BuildSelectListItem(AppResources.Resources.NonLive, LiveStatusConstants.NonLive);
                yield return BuildSelectListItem(AppResources.Resources.Mix, LiveStatusConstants.Mix);
            }
        }

        public IEnumerable<SelectListItem> SportGroups
        {
            get
            {
                yield return BuildSelectListItem(AppResources.Resources.All, SportGroupConstants.All, true);
                yield return BuildSelectListItem(AppResources.Resources.MajorSports, SportGroupConstants.MajorSports);
                yield return BuildSelectListItem(AppResources.Resources.MinorSports, SportGroupConstants.MinorSports);
            }
        }

        private static SelectListItem BuildSelectListItem(string text, byte value, bool selected = false)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = selected };
    }
}