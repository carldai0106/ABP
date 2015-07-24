using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMS.Application.MultiTenancy;
using CMS.Application.MultiTenancy.Dto;
using CMS.Application.User;
using CMS.Application.User.Dto;
using CMS.Domain;
using CMS.Domain.Tenant;

namespace CMS.UnitTest
{
    [TestClass]
    public class TenantAppServiceTest : TestBase<Guid, Guid>
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IUserAppService _userAppService;

        public TenantAppServiceTest()
        {
            _tenantAppService = LocalIocManager.Resolve<ITenantAppService>();
            _userAppService = LocalIocManager.Resolve<IUserAppService>();
        }

        [TestMethod]
        public async Task Create_Update_Delete_Tenant()
        {
            //var dto = new CreateTenantDto
            //{
            //    DisplayName = "carl dai",
            //    IsActive = true,
            //    TenancyName = "bcsint"
            //};

            //await _tenantAppService.CreateTenant(dto);

            //var dtoEdit = _tenantAppService.GetTenant(dto.TenancyName);

            ParameterExpression pi = Expression.Parameter(typeof(int), "i");
            var fexp =
                Expression.Lambda<Func<int, int>>(
                    Expression.Add(pi, Expression.Constant(1))
                    , pi);

            //var f = fexp.Compile();

            //IMustHaveTenant<Guid> x = new Customer<Guid>();
            //x.TenantId = default(Guid);

            //ParameterExpression p1 = Expression.Parameter(typeof(IMustHaveTenant<Guid>), "t");
            //ParameterExpression p2 = Expression.Parameter(typeof(Guid), "tenantId");


            //var exp = Expression.Lambda<Func<IMustHaveTenant<Guid>, Guid, bool>>(
            //        Expression.Equal(Expression.PropertyOrField(p1, "TenantId"), p2), p1, p2
            //    );


            //Debug.WriteLine(exp.Body.ToString());

            //var func = exp.Compile();


            //var rs = func(x, default(Guid));
            //Debug.WriteLine(rs);

            var ex = Get<Guid>();

            Debug.WriteLine(ex.ToString());

            //var pe = Expression.Parameter( typeof( IMustHaveTenant<Guid> )); // <<== Here
            // var memberExpression = Expression.PropertyOrField(pe /* Here */, "TenantId");
            // var equalExpression = Expression.Equal( memberExpression, Expression.Constant(default(Guid)) );
            // var compiled = Expression.Lambda<Func<IMustHaveTenant<Guid>, bool>>(equalExpression, pe).Compile();
            // Debug.WriteLine(compiled(x));
        }

        public Expression Get<TTenantId>() where TTenantId : struct
        {
            ParameterExpression p1 = Expression.Parameter(typeof(IMayHaveTenant<TTenantId>), "t");
            ParameterExpression p2 = Expression.Parameter(typeof(TTenantId?), "tenantId");
            MemberExpression m = Expression.PropertyOrField(p1, "TenantId");
            

            var exp = Expression.Lambda<Func<IMayHaveTenant<TTenantId>, TTenantId?, bool>>(
                    Expression.Equal(m, Expression.Convert(p2, p1.Type)), p1, p2
                );

            return exp;
        }

        

    }



    public class Customer<TTenantId> : IMustHaveTenant<TTenantId> where TTenantId : struct
    {
        public TTenantId TenantId { get; set; }
    }
}
