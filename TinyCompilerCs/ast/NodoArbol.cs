﻿/**
 * NodoArbol.cs
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
using Emit = System.Reflection.Emit;
using TinyCompiler.tabla;
using TinyCompiler.util;

namespace TinyCompiler.ast
{
    public abstract class NodoArbol
    {
        protected int linea, columna;

        public NodoArbol(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }

        public int Linea
        {
            get {return linea; }
        }

        public int Columna
        {
            get { return columna; }
        }

        protected void dump_SimboloAbstracto(System.IO.TextWriter cout, int n, SimboloAbstracto sym) {
	        cout.Write(Utilidades.pad(n));
	        cout.WriteLine(sym.Texto);
        }
    
        protected void dumpLineaColumna(System.IO.TextWriter cout, int n) {
	        cout.Write(Utilidades.pad(n) + "#" + linea + ":"+columna+": ");
        }

        public abstract void generar(Emit.ILGenerator il);       
        public abstract void dump(System.IO.TextWriter str, int n);
        public abstract void checkSemantica(TablaSimbolos tabla);
    }
}
