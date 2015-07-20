﻿using Abp.Localization;
using Abp.Modules;
using System.Reflection;
using Abp.Reflection;
using AutoMapper;
using Castle.Core.Logging;

namespace Abp.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AbpAutoMapperModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }

        private readonly ITypeFinder _typeFinder;

        private static bool _createdMappingsBefore;
        private static readonly object SyncObj = new object();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFinder"></param>
        public AbpAutoMapperModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        public override void PreInitialize<TTenantId, TUserId>()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            lock (SyncObj)
            {
                //We should prevent duplicate mapping in an application, since AutoMapper is static.
                if (_createdMappingsBefore)
                {
                    return;
                }

                FindAndAutoMapTypes();
                CreateOtherMappings();

                _createdMappingsBefore = true;
            }
        }

        private void FindAndAutoMapTypes()
        {
            var types = _typeFinder.Find(type =>
                type.IsDefined(typeof(AutoMapAttribute)) ||
                type.IsDefined(typeof(AutoMapFromAttribute)) ||
                type.IsDefined(typeof(AutoMapToAttribute))
                );

            Logger.DebugFormat("Found {0} classes defines auto mapping attributes", types.Length);
            foreach (var type in types)
            {
                Logger.Debug(type.FullName);
                AutoMapperHelper.CreateMap(type);
            }
        }

        private void CreateOtherMappings()
        {
            Mapper.CreateMap<LocalizableString, string>().ConvertUsing(ls => LocalizationManager.GetString(ls.SourceName, ls.Name));
        }
    }
}
