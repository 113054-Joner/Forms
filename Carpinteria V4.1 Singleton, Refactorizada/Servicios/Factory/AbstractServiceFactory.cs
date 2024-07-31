using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria.Servicios.Factory
{
    public abstract class AbstractServiceFactory
    {
        public abstract IPresupuestoServicio CrearServicio();
    }
}
