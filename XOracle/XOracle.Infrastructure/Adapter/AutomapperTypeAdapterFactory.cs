using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class AutomapperTypeAdapterFactory : IFactory<ITypeAdapter>
    {
        public AutomapperTypeAdapterFactory()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(a => a.GetTypes())
                                    .Where(t => t.BaseType == typeof(Profile));

            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.SelfProfiler`2")
                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                }
            });
        }

        public async Task<ITypeAdapter> Create()
        {
            return new AutomapperTypeAdapter();
        }
    }
}
