using Carpinteria.AccesoDatos;
using Carpinteria.Servicios.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Servicios.Factory
{
    public class ServicerFactory : AbstractServiceFactory
    {
        public override IPresupuestoServicio CrearServicio()
        {
            return new PresupuestoService();
        }
    }
}
