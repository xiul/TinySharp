/**
 * ParamFormal.cs
 * 
 *  Copyrigth (C) 2012: 
 *  Ochoa, Luis lochoa (at unet.edu.ve)
 *  Sanchez, Manuel mbsanchez (at unet.edu.ve)
 *  
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301 USA
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCompiler.tabla;
using Emit = System.Reflection.Emit;

namespace TinyCompiler.ast
{
    public class ParamFormal : NodoArbol
    {
        protected SimboloAbstracto tipo;
        protected SimboloAbstracto id;
        protected bool esDireccion;

        public ParamFormal(SimboloAbstracto tipo, SimboloAbstracto id, int linea, int columna) 
            : base(linea, columna)
        {
            this.tipo = tipo;
            this.id = id;
            this.esDireccion = false;
        }

        public ParamFormal(SimboloAbstracto tipo, SimboloAbstracto id, bool esDireccion, int linea, int columna) 
            : base(linea, columna)
        {
            this.tipo = tipo;
            this.id = id;
            this.esDireccion = esDireccion;
        }

        public override void generar(Emit.ILGenerator il)
        {
            throw new NotImplementedException();
        }

        public SimboloAbstracto Tipo
        {
            get { return tipo; }
        }
        public SimboloAbstracto Id
        {
            get { return id; }
        }
        public bool EsDireccion
        {
            get { return esDireccion; }
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_parametro_formal");
            dump_SimboloAbstracto(str, n+2, tipo);
            dump_SimboloAbstracto(str, n+2, id);
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            //TODO no existe verificacion semantica en este nodo
        }
    }
}
