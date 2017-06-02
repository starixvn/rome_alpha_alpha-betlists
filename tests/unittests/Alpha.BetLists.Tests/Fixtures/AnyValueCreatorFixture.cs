using Ploeh.AutoFixture;

namespace Alpha.BetLists.Tests.Collections
{
    public class AnyValueCreatorFixture
    {
        private readonly Fixture anyValueGenerator;

        public AnyValueCreatorFixture()
        {
            anyValueGenerator = new Fixture();
        }

        public T Create<T>()
        {
            return anyValueGenerator.Create<T>();
        }
    }
}
