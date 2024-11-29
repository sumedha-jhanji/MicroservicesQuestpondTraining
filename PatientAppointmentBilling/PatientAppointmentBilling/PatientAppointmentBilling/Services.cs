
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PatientAppointmentBilling
{
    public class Services
    {

        public static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            //using internal DI
            //services.AddTransient<IHandler<CreatePatient>, CreatePatientHandler>();
            //services.AddTransient<IHandler<UpdatePatient>, UpdatePatientHandler>();
            //services.AddTransient<IHandler<DeletePatient>, DeletePatientHandler>();

            //using Mediatr
            services.AddMediatR(cfg =>
                       cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddMediatR(cfg =>
                     cfg.RegisterServicesFromAssembly(
                         typeof(AppCQRS.BaseCommand).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

    }
}
