﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaFront.AccesoDatos
{
     public abstract class AbstractDaofactory //1 Fac
    {
        public abstract IPresupuestoDao CrearPresupuestoDao();
    }
}
