using System;

namespace Alpha.BetLists.Models
{
    public interface IUser
    {
        int Id { get; }

        string Site { get; }

        int CurrencyId { get; }

        string Name { get; }

        string Language { get; }

        byte LevelId { get; }

        int AdminId { get; }

        int SubAccId { get; }

        bool IsInternal { get; }

        bool IsAdminCanViewVNIP { get; }

        string LangKey { get; }

        bool IsEnabledJackpot { get; }

        bool HasPermissionOnProduct(int productId);

        DateTime LimitedDateToShowInfo { get; }

        bool IsEnableSabaPromotion { get; }

        bool IsEnableSabaPromotionForHeadline { get; }

        bool AutoFollowBetIsDummyAgent { get; }
    }
}
