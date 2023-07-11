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
                foreach (var bettypeId in AppSettings.MonitoringBetsBettypes)
                {
                    yield return new SelectListItem
                    {
                        Text = CacheRefServiceHelper.GetBetTypeName(int.Parse(bettypeId)),
                        Value = bettypeId,
                        Selected = true
                    };
                }

                yield return new SelectListItem
                {
                    Text = AppResources.Resources.Others,
                    Value = BetTypeIdConstants.Others.ToString(),
                    Selected = true
                };
            }
        }

        public IEnumerable<SelectListItem> EventStatus
        {
            get
            {
                yield return new SelectListItem { Text = AppResources.Resources.All, Value = EventStatusConstants.All.ToString(), Selected = true };
                yield return new SelectListItem { Text = AppResources.Resources.Live, Value = EventStatusConstants.Live.ToString() };
                yield return new SelectListItem { Text = AppResources.Resources.NonLive, Value = EventStatusConstants.NonLive.ToString() };
            }
        }

        public IEnumerable<SelectListItem> SportGroups
        {
            get
            {
                yield return new SelectListItem { Text = AppResources.Resources.All, Value = SportGroupConstants.All.ToString(), Selected = true };
                yield return new SelectListItem { Text = AppResources.Resources.MajorSports, Value = SportGroupConstants.MajorSports.ToString() };
                yield return new SelectListItem { Text = AppResources.Resources.MinorSports, Value = SportGroupConstants.MinorSports.ToString() };
            }
        }
    }
}