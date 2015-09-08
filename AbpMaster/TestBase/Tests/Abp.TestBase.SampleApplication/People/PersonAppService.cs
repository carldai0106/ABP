﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.TestBase.SampleApplication.People.Dto;

namespace Abp.TestBase.SampleApplication.People
{
    public class PersonAppService : ApplicationService<int, long>, IPersonAppService
      
    {
        private readonly IRepository<Person> _personRepository;

        public PersonAppService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        [DisableAuditing]
        public ListResultOutput<PersonDto> GetPeople(GetPeopleInput input)
        {
            var query = _personRepository.GetAll();

            if (!input.NameFilter.IsNullOrEmpty())
            {
                query = query.Where(p => p.Name.Contains(input.NameFilter));
            }

            var people = query.ToList();

            return new ListResultOutput<PersonDto>(people.MapTo<List<PersonDto>>());
        }

        public async Task CreatePersonAsync(CreatePersonInput input)
        {
            await _personRepository.InsertAsync(input.MapTo<Person>());
        }

        [AbpAuthorize("CanDeletePerson")]
        public async Task DeletePerson(EntityRequestInput input)
        {
            await _personRepository.DeleteAsync(input.Id);
        }

        public string TestPrimitiveMethod(int a, string b, EntityRequestInput c)
        {
            return a + "#" + b + "#" + c.Id;
        }
    }
}
