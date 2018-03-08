namespace Alpha.BetLists.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Language { get; }

        byte LevelId { get; }

        int AdminId { get; }

        bool IsInternal { get; }

        bool IsAdminCanViewVNIP { get; }

        string LangKey { get; }


        bool IsEnabledJackpot { get; }

        bool HasPermissionOnProduct(int productId);
    }
}