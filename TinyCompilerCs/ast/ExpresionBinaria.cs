/**
 * ExpresionBinaria.cs
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

namespace TinyCompiler.ast
{
    public abstract class ExpresionBinaria : Expresion
    {
        protected Expresion expr1;
        protected Expresion expr2;

        public ExpresionBinaria(Expresion expr1, Expresion expr2, int linea, int columna)
            : base(linea, columna)
        {
            
            this.expr1 = expr1;
            this.expr2 = expr2;
        }

        public Expresion ExprIzq
        {
            get { return expr1; }
        }

        public Expresion ExprDer
        {
            get { return expr2; }
        }
    }
}
