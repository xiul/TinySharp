/**
 * SimboloId.cs
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
    public class SimboloId : SimboloAbstracto
    {
        public SimboloId(int indice, string texto, int linea, int columna)
               : base(indice, texto, linea, columna)
        
        {
        }

        public SimboloId(string texto, int linea, int columna) 
               : base(texto, linea, columna)
        {
        }

        public override object clone()
        {
            return new SimboloId(indice, texto, linea, columna);
        }
    }
}
