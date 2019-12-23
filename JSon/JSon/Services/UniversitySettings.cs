using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSon.Services
{
    public class UniversitySettings:ISettings
    {

        public int Capacity { get; set; }

        public UniversitySettings(IConfiguration configuration)
        {

            var universityConfig = configuration.GetSection("UniversitySettings");
            Capacity = universityConfig.GetValue<int>("Capacity");
        }
    }
}
