/**
 * Condicional.cs
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
    public class Condicional : Sentencia
    {
        protected Expresion condicion;
        protected ListaSentencia entonces;
        protected ListaSentencia sino;

        public Condicional(Expresion condicion, ListaSentencia entonces, ListaSentencia sino, int linea, int columna)
            : base(linea, columna)
        {
            this.condicion = condicion;
            this.entonces = entonces;
            this.sino = sino;
        }

        public Expresion Condicion
        {
            get { return condicion; }
            //hijueputa aprenda a programar.... estabaa asi esa mierda get { return Condicion; }
        }

        public ListaSentencia Entonces
        {
            get { return entonces; }
        }

        public ListaSentencia Sino
        {
            get { return sino; }
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_condicional");
            condicion.dump(str, n+4);
            str.WriteLine(Utilidades.pad(n+2) + "_entonces");
            entonces.dump(str, n+4);
            if(sino!=null){
                str.WriteLine(Utilidades.pad(n+2) + "_sino");
                sino.dump(str, n+4);
            }
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            condicion.checkSemantica(tabla);

            //validar que la condición sea de tipo booleana
            if (!condicion.Tipo_Expr.Equals(NucleoLenguaje.tipoBool))
                SemantErrorReport.Instancia.semantError(condicion, "la condición del \"if\" debe ser un valor de tipo _lógico");

            entonces.checkSemantica(tabla);

            if (sino != null)
                sino.checkSemantica(tabla);
        }

        public override void generar(Emit.ILGenerator il)
        {
            Emit.LocalBuilder tmpVarLogico;

            Console.WriteLine("Generando Nodo Condicional (IF)");
            this.condicion.generar(il);

            il.Emit(Emit.OpCodes.Ldc_I4_0);  //Ingreso constante 0
            il.Emit(Emit.OpCodes.Ceq);  //Comparo si es falso (es 0)
            //Almaceno este resultado en una variable temporal
            tmpVarLogico = il.DeclareLocal(typeof(bool));
            il.Emit(Emit.OpCodes.Stloc, tmpVarLogico);
            //cargo el resultado de la variable temporal
            il.Emit(Emit.OpCodes.Ldloc, tmpVarLogico);
            Emit.Label bloqueFalso = il.DefineLabel();
            //salto en caso que sea verdadero el resultado es decir es cero la evaluacion del (pila==0) hago el sino
            il.Emit(Emit.OpCodes.Brtrue, bloqueFalso);

            Entonces.generar(il);

            Emit.Label finSi = il.DefineLabel();
            il.Emit(Emit.OpCodes.Br, finSi);
            il.MarkLabel(bloqueFalso);

            if(Sino!=null)
                Sino.generar(il);

            il.MarkLabel(finSi);

        }

    }
}
