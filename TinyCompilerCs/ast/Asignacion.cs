/**
 * Asignacion.cs
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
using TinyCompiler.util;
using TinyCompiler.semantica;
using Emit = System.Reflection.Emit;

namespace TinyCompiler.ast
{
    public class Asignacion : Sentencia
    {
        protected Variable id;
        protected Expresion expr;

        public Asignacion(Variable id, Expresion expr, int linea, int columna)
            : base(linea, columna)
        {
            this.id = id;
            this.expr = expr;
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_asignacion");
            id.dump(str, n + 2);
            expr.dump(str, n + 2);
        }

        public Variable Id
        {
            get { return id; }
        }

        public Expresion Expr
        {
            get { return expr; }
        }

        public override void checkSemantica(TablaSimbolos tabla)
        {
            //Chequear que si la variable existe, sea una variable
            Object sym = tabla.buscar(id.Nombre);

            if (sym!=null && sym.GetType() != typeof(Variable))
            {
                SemantErrorReport.Instancia.semantError(id, "el identificador "+id.Nombre+" es inválido");
            }

            expr.checkSemantica(tabla);

            //Agregar el id a la tabla de símbolos
            if(sym==null)
                tabla.agregarId(id.Nombre, id);

            id.checkTipo(tabla);
            //chequear el tipo del id y la expresión
            if (!id.Tipo_Expr.Equals(expr.Tipo_Expr))
                SemantErrorReport.Instancia.semantError(id, "no se puede convertir valor de tipo "+expr.Tipo_Expr+" a tipo "+id.Tipo_Expr);
        }

        public override void generar(Emit.ILGenerator il)
        {
            this.Expr.generar(il);
            TinyCompiler.generador.GeneradorCodigo.Almacenar(Id.Nombre.Texto,il);
        }
    } 
}
