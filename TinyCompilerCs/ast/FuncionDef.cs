/**
 * FuncionDef.cs
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
using TinyCompiler.generador;

namespace TinyCompiler.ast
{
    public class FuncionDef : NodoArbol
    {
        protected SimboloAbstracto nombre;
        protected ListaParamFormal parametros;
        protected Action<Emit.ILGenerator, ListaParametros> cuerpoDelMetodo;

        public FuncionDef(SimboloAbstracto nombre, ListaParamFormal parametros, int linea, int columna, Action<Emit.ILGenerator, ListaParametros> cuerpoDelMetodo) 
            : base(linea, columna)
        {
            this.nombre = nombre;
            this.parametros = parametros;
            this.cuerpoDelMetodo = cuerpoDelMetodo;
        }

        public SimboloAbstracto Nombre
        {
            get { return nombre; }
        }

        public ListaParamFormal Parametros
        {
            get { return parametros; }
        }
       
        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_función");
            dump_SimboloAbstracto(str, n+2, nombre);
            parametros.dump(str, n + 2);
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            //TODO no existe verificacion semantica en este nodo
        }

        public void ejecutarMetodo(Emit.ILGenerator il, ListaParametros lp)
        {
            cuerpoDelMetodo(il, lp);
        }

        public override void generar(Emit.ILGenerator il)
        {
            throw new NotImplementedException();
        }
    }
}
