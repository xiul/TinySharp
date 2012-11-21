/**
 * Igual.cs
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
    public class Igual : ExpresionBinaria
    {
        public Igual(Expresion expr1, Expresion expr2, int linea, int columna) 
            : base(expr1, expr2, linea, columna)
        {
        }
    
        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_comparacion");
            if (tipo_expr != null) str.WriteLine(Utilidades.pad(n + 2) + "Tipo: " + tipo_expr);
            expr1.dump(str, n+2);
            expr2.dump(str, n+2);
        }

        public override void checkTipo(TablaSimbolos tabla)
        {
            if (!expr1.Tipo_Expr.Equals(NucleoLenguaje.tipoEntero) ||
               !expr2.Tipo_Expr.Equals(NucleoLenguaje.tipoEntero))
                SemantErrorReport.Instancia.semantError(this, "la operación igual_a no se puede aplicar a valores de tipo " + expr1.Tipo_Expr + " y " + expr2.Tipo_Expr);
            tipo_expr = NucleoLenguaje.tipoBool;
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            expr1.checkSemantica(tabla);
            expr2.checkSemantica(tabla);
            checkTipo(tabla);
        }

        public override void generar(Emit.ILGenerator il)
        {
            this.ExprIzq.generar(il);
            this.ExprDer.generar(il);
            il.Emit(Emit.OpCodes.Ceq);
        }
    }
}
