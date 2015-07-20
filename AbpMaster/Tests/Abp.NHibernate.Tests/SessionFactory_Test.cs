using NHibernate;
using Xunit;

namespace Abp.NHibernate.Tests
{
    public class SessionFactory_Test : NHibernateTestBase<int, long>
    {
        private readonly ISessionFactory _sessionFactory;

        public SessionFactory_Test()
        {
            _sessionFactory = Resolve<ISessionFactory>();
        }

        [Fact]
        public void Should_OpenSession_Work()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                //nothing...
            }
        }
    }
}