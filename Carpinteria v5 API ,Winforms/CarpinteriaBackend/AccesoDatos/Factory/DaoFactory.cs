using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.AccesoDatos
{
    class DaoFactory : AbstractDaofactory // 2 FACT HERENCIA
    {
        public override IPresupuestoDao CrearPresupuestoDao()
        {
            return new PresupuestoDao(); // 3 FACT
        }
    }
}
