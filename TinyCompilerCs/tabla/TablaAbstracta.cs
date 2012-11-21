/**
 * TablaAbstracta.cs
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

namespace TinyCompiler.tabla
{
    public abstract class TablaAbstracta
    {
        private int idx;
        private const int TAM_MAX_SYMB = 1024;
        Dictionary<string, SimboloAbstracto> tabla;
        public static TablaEntero intTabla = new TablaEntero();
        public static TablaId idTabla = new TablaId();
        public static TablaTexto texTabla = new TablaTexto();

        public TablaAbstracta()
        {
            tabla = new Dictionary<string, SimboloAbstracto>();
            idx = 0;
        }

        public SimboloAbstracto agregarSimbolo(String texto, int linea, int columna)
        {
            return agregarSimbolo(texto, TAM_MAX_SYMB, linea, columna);
        }

        public SimboloAbstracto agregarSimbolo(String texto, int tam, int linea, int columna)
        {
            SimboloAbstracto sym = null;

            texto = (texto.Length <= TAM_MAX_SYMB) ? texto : texto.Substring(0, TAM_MAX_SYMB);

            if (!tabla.ContainsKey(texto))
            {
                sym = getNuevoSimbolo(texto, idx++, linea, columna);
                tabla.Add(texto, sym);
            }
            else
            {
                sym = (SimboloAbstracto)tabla[texto].clone();
                sym.Linea = linea;
                sym.Columna = columna;
            }

            return sym;
        }

        public List<SimboloAbstracto> getSimbolos()
        {
            return new List<SimboloAbstracto>(tabla.Values);
        }

        public SimboloAbstracto getSimbolo(string texto)
        {
            return tabla[texto];
        }

        public SimboloAbstracto getSimbolo(int indice){
            foreach(SimboloAbstracto sym in tabla.Values){
                if(sym.EqualsIndice(indice))
                    return sym;
            }
        
            return null;
        }

        protected abstract SimboloAbstracto getNuevoSimbolo(String texto, int indice, int linea, int columna);
    }
}
