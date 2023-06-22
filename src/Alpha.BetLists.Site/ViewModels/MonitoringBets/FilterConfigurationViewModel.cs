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

        public IEnumerable<SelectListItem> Bettypes
        {
            get
            {
                foreach (var bettypeId in AppSettings.MonitoringBetsBettypes)
                {
                    yield return new SelectListItem
                    {
                        Text = CacheRefServiceHelper.GetBetTypeName(int.Parse(bettypeId)),
                        Value = bettypeId,
                        Selected = true
                    };
                }

                yield return new SelectListItem { Text = AppResources.Resources.OtherBetTypes, Value = "99", Selected = true };
            }
        }

        public IEnumerable<SelectListItem> MatchLiveIndicators
        {
            get
            {
                yield return new SelectListItem { Text = AppResources.Resources.Live, Value = "1", Selected = true };
                yield return new SelectListItem { Text = AppResources.Resources.NonLive, Value = "0"};
            }
        }

        public IEnumerable<SelectListItem> GroupOfSports
        {
            get
            {
                yield return new SelectListItem { Text = AppResources.Resources.All, Value = GroupOfSportsConstants.All.ToString(), Selected = true };
                yield return new SelectListItem { Text = AppResources.Resources.MajorSports, Value = GroupOfSportsConstants.MajorSports.ToString() };
                yield return new SelectListItem { Text = AppResources.Resources.MinorSports, Value = GroupOfSportsConstants.MinorSports.ToString() };
            }
        }
    }
}