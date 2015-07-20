﻿using System;
using System.Data.Entity.Validation;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.TestBase.SampleApplication.People;
using Shouldly;
using Xunit;

namespace Abp.TestBase.SampleApplication.Tests.Uow
{
    public class UnitOfWork_Event_Tests : SampleApplicationTestBase<int, long>
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IUnitOfWorkManager<int, long> _unitOfWorkManager;

        public UnitOfWork_Event_Tests()
        {
            _personRepository = Resolve<IRepository<Person>>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager<int, long>>();
        }

        [Fact]
        public void Should_Trigger_Completed_When_Uow_Succeed()
        {
            var completeCount = 0;
            var disposeCount = 0;

            using (var uow = _unitOfWorkManager.Begin())
            {
                _personRepository.Insert(new Person { ContactListId = 1, Name = "john" });

                _unitOfWorkManager.Current.Completed += (sender, args) =>
                                                        {
                                                            _unitOfWorkManager.Current.ShouldBe(null);
                                                            completeCount++;
                                                        };

                _unitOfWorkManager.Current.Disposed += (sender, args) =>
                                                       {
                                                           _unitOfWorkManager.Current.ShouldBe(null);
                                                           completeCount.ShouldBe(1);
                                                           disposeCount++;
                                                       };

                uow.Complete();
            }

            UsingDbContext(context => context.People.Any(p => p.Name == "john").ShouldBe(true));

            completeCount.ShouldBe(1);
            disposeCount.ShouldBe(1);
        }

        [Fact]
        public void Should_Trigger_Failed_When_Throw_Exception_In_Uow()
        {
            var failedCount = 0;
            var disposeCount = 0;

            Assert.Throws<ApplicationException>(
                new Action(() =>
                {
                    using (var uow = _unitOfWorkManager.Begin())
                    {
                        _personRepository.Insert(new Person()); //Name is intentionally not set to cause exception

                        _unitOfWorkManager.Current.ShouldNotBe(null);

                        _unitOfWorkManager.Current.Failed += (sender, args) =>
                        {
                            _unitOfWorkManager.Current.ShouldBe(null);
                            args.Exception.ShouldBe(null); //Can not set it!
                            failedCount++;
                        };

                        _unitOfWorkManager.Current.Disposed += (sender, args) =>
                        {
                            _unitOfWorkManager.Current.ShouldBe(null);
                            failedCount.ShouldBe(1);
                            disposeCount++;
                        };

                        throw new ApplicationException("This is throwed to make uow failed");

                        uow.Complete();
                    }
                }));

            failedCount.ShouldBe(1);
            disposeCount.ShouldBe(1);
        }

        [Fact]
        public void Should_Trigger_Failed_When_SaveChanges_Fails()
        {
            var failedCount = 0;
            var disposeCount = 0;

            using (var uow = _unitOfWorkManager.Begin())
            {
                _personRepository.Insert(new Person()); //Name is intentionally not set to cause exception

                _unitOfWorkManager.Current.Failed += (sender, args) =>
                {
                    _unitOfWorkManager.Current.ShouldBe(null);
                    args.Exception.ShouldNotBe(null);
                    args.Exception.ShouldBeOfType(typeof(DbEntityValidationException));
                    failedCount++;
                };

                _unitOfWorkManager.Current.Disposed += (sender, args) =>
                {
                    _unitOfWorkManager.Current.ShouldBe(null);
                    failedCount.ShouldBe(1);
                    disposeCount++;
                };

                Assert.Throws<DbEntityValidationException>(() => uow.Complete());
            }

            failedCount.ShouldBe(1);
            disposeCount.ShouldBe(1);
        }
    }
}