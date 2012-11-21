/**
 * Variable.cs
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
using TinyCompiler.util;
using TinyCompiler.tabla;
using TinyCompiler.semantica;
using Emit = System.Reflection.Emit;

namespace TinyCompiler.ast
{
    public class Variable : Expresion
    {
        protected SimboloAbstracto nombre;

        public Variable(SimboloAbstracto variable, int linea, int columna)
            : base(linea, columna)
        {
            this.nombre = variable;
        }

        public override void generar(Emit.ILGenerator il)
        {
            if (!TinyCompiler.generador.GeneradorCodigo.TablaDireccionesSimbolos.ContainsKey(Nombre.Texto))
            {
                throw new System.Exception("ERROR-0007 fallo en analisis semantico Variable no declarada encontrada durante generacion '" + Nombre.Texto + "'");
            }
            else
            {
                il.Emit(Emit.OpCodes.Ldloc, TinyCompiler.generador.GeneradorCodigo.TablaDireccionesSimbolos[Nombre.Texto]);
            }
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_variable");
            if (tipo_expr != null) str.WriteLine(Utilidades.pad(n + 2) + "Tipo: " + tipo_expr);
            dump_SimboloAbstracto(str, n + 2, nombre);
        }

        public SimboloAbstracto Nombre
        {
            get { return nombre; }
        }

        public override void checkTipo(TablaSimbolos tabla) {
            tipo_expr = NucleoLenguaje.tipoEntero;
        }

        public override void checkSemantica(TablaSimbolos tabla) {
            Object sym =  tabla.buscar(nombre);
        
            checkTipo(tabla);
        
            if(sym==null)
                SemantErrorReport.Instancia.semantError(this, "la variable \""+nombre+"\" no ha sido inicializada");
            else if(sym.GetType() != typeof(Variable)) //verificar que sea una nombre de variable
                SemantErrorReport.Instancia.semantError(this, "el identificador \""+nombre+"\" es inválido");
        }
    }
}
