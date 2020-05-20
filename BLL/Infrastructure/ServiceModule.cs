using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;
using Ninject.Modules;
using DAL.Repositories;


namespace BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}
