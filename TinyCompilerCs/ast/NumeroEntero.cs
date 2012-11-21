/**
 * NumeroEntero.cs
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
    public class NumeroEntero : Expresion
    {
        protected SimboloAbstracto token;

        public NumeroEntero(SimboloAbstracto token, int linea, int columna) 
            : base(linea, columna)
        {
            this.token = token;
        }

        public SimboloAbstracto Token
        {
            get { return token; }
        }

        public override void generar(Emit.ILGenerator il)
        {
            il.Emit(Emit.OpCodes.Ldc_I4, TinyCompiler.generador.GeneradorCodigo.convertir_a_entero(Token.Texto));
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_literal");
            if(tipo_expr!=null) str.WriteLine(Utilidades.pad(n+2)+"Tipo: " + tipo_expr); 
            dump_SimboloAbstracto(str, n + 2, token);
        }

        public override void checkTipo(TablaSimbolos tabla) {
            tipo_expr = NucleoLenguaje.tipoEntero;
        }

        public override void checkSemantica(TablaSimbolos tabla) {
            checkTipo(tabla);
        }
    }
}
