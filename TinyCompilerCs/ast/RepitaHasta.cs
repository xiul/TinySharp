/**
 * RepitaHasta.cs
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
    public class RepitaHasta : Sentencia
    {
        protected Expresion condicion;
        protected ListaSentencia sentencias;

        public RepitaHasta(Expresion condicion, ListaSentencia sentencias, int linea, int columna)
            : base(linea, columna)
        {
            this.condicion = condicion;
            this.sentencias = sentencias;
        }

        public Expresion Condicion {
            get { return condicion; }
        }

        public ListaSentencia Sentencias 
        {
            get { return sentencias; }
        }

        public override void generar(Emit.ILGenerator il)
        {
            Emit.LocalBuilder tmpCondicion;
            tmpCondicion = il.DeclareLocal(typeof(bool));
            Emit.Label sentenciasRepita = il.DefineLabel();
            il.MarkLabel(sentenciasRepita);
            il.Emit(Emit.OpCodes.Nop);		//emito primera sentencia vacia															

            Sentencias.generar(il);

            Condicion.generar(il);

            il.Emit(Emit.OpCodes.Stloc, tmpCondicion);  //almaceno resultado de condicion del mientras	
            il.Emit(Emit.OpCodes.Ldloc, tmpCondicion);  //cargo resultado de condicion del mientras
            il.Emit(Emit.OpCodes.Brfalse, sentenciasRepita);
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_repita-hasta");
            str.WriteLine(Utilidades.pad(n+2) + "_sentencias");
            sentencias.dump(str, n+4);
            condicion.dump(str, n + 2);
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            sentencias.checkSemantica(tabla);

            condicion.checkSemantica(tabla);
            //validar que la condición sea de tipo booleana
            if (!condicion.Tipo_Expr.Equals(NucleoLenguaje.tipoBool))
                SemantErrorReport.Instancia.semantError(condicion, "la condición del \"repeat-until\" debe ser un valor de tipo _lógico");
        }
    }
}
