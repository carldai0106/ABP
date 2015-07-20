﻿using System.Threading.Tasks;
using Abp.Auditing;
using Abp.TestBase.SampleApplication.People;
using Abp.TestBase.SampleApplication.People.Dto;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Xunit;

namespace Abp.TestBase.SampleApplication.Tests.Auditing
{
    public class SimpleAuditing_Test : SampleApplicationTestBase<int, long>
    {
        private readonly IPersonAppService _personAppService;

        private IAuditingStore<int, long> _auditingStore;

        public SimpleAuditing_Test()
        {
            _personAppService = Resolve<IPersonAppService>();
            Resolve<IAuditingConfiguration>().IsEnabledForAnonymousUsers = true;
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();
            _auditingStore = Substitute.For<IAuditingStore<int, long>>();
            LocalIocManager.IocContainer.Register(
                Component.For<IAuditingStore<int, long>>().UsingFactoryMethod(() => _auditingStore).LifestyleSingleton()
                );
        }

        #region CASES WRITE AUDIT LOGS

        [Fact]
        public async Task Should_Write_Audits_For_Conventional_Methods()
        {
            /* All application service methods are audited as conventional. */

            await _personAppService.CreatePersonAsync(new CreatePersonInput { ContactListId = 1, Name = "john" });
            var o = Arg.Any<AuditInfo<int, long>>();
            var received = _auditingStore.Received();
            received.SaveAsync(o);
        }

        [Fact]
        public void Should_Write_Audits_For_Audited_Class_Virtual_Methods_As_Default()
        {
            Resolve<MyServiceWithClassAudited>().Test1();
            _auditingStore.Received().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        [Fact]
        public void Should_Write_Audits_For_Audited_Methods()
        {
            Resolve<MyServiceWithMethodAudited>().Test1();
            _auditingStore.Received().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        #endregion

        #region CASES DON'T WRITE AUDIT LOGS

        [Fact]
        public void Should_Not_Write_Audits_For_Conventional_Methods_If_Disabled_Auditing()
        {
            /* GetPeople has DisableAuditing attribute. */

            _personAppService.GetPeople(new GetPeopleInput());
            _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        [Fact]
        public void Should_Not_Write_Audits_For_Audited_Class_Non_Virtual_Methods_As_Default()
        {
            Resolve<MyServiceWithClassAudited>().Test2();
            _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        [Fact]
        public void Should_Not_Write_Audits_For_Not_Audited_Methods()
        {
            Resolve<MyServiceWithMethodAudited>().Test2();
            _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        [Fact]
        public void Should_Not_Write_Audits_For_Not_Audited_Classes()
        {
            Resolve<MyServiceWithNotAudited>().Test1();
            _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditInfo<int, long>>());
        }

        #endregion

        [Audited]
        public class MyServiceWithClassAudited
        {
            public virtual void Test1()
            {

            }

            public void Test2()
            {

            }
        }

        public class MyServiceWithMethodAudited
        {
            [Audited]
            public virtual void Test1()
            {

            }

            public virtual void Test2()
            {

            }
        }

        public class MyServiceWithNotAudited
        {
            public virtual void Test1()
            {

            }
        }
    }
}
