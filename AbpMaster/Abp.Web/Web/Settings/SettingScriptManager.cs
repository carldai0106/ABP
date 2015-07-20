using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;

namespace Abp.Web.Settings
{
    /// <summary>
    /// This class is used to build setting script.
    /// </summary>
    public class SettingScriptManager<TTenantId, TUserId> : ISettingScriptManager<TTenantId, TUserId>, ISingletonDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager<TTenantId, TUserId> _settingManager;

        public SettingScriptManager(ISettingDefinitionManager settingDefinitionManager, ISettingManager<TTenantId, TUserId> settingManager)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
        }

        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.setting = abp.setting || {};");
            script.AppendLine("    abp.setting.values = {");

            var settingDefinitions = _settingDefinitionManager
                .GetAllSettingDefinitions()
                .Where(sd => sd.IsVisibleToClients);

            var added = 0;
            foreach (var settingDefinition in settingDefinitions)
            {
                if (added > 0)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }

                script.Append("        '" + settingDefinition.Name.Replace("'", @"\'") + "': '" + (await _settingManager.GetSettingValueAsync(settingDefinition.Name)).Replace(@"\", @"\\").Replace("'", @"\'") + "'");

                ++added;
            }

            script.AppendLine();
            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}