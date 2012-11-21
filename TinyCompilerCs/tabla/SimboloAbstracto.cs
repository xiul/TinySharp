/**
 * SimboloAbstracto.cs
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

namespace TinyCompiler.tabla
{
    public abstract class  SimboloAbstracto //: IEqualityComparer<SimboloAbstracto>
    {
        protected int indice;
        protected string texto;
        protected int linea;
        protected int columna;

        public SimboloAbstracto(int indice, string texto, int linea, int columna)
        {
            this.indice = indice;
            this.texto = texto;
            this.linea = linea;
            this.columna = columna;
        }

        public SimboloAbstracto(string texto, int linea, int columna) {
            this.texto = texto;
            this.linea = linea;
            this.columna = columna;
            indice = -1;
        }
       
        public bool EqualsTexto(string str){
            return texto == str;
        }
    
        public bool EqualsIndice(int index){
            return indice==index;
        }
  
        public override bool Equals(object otro) {
            return otro != null && (typeof(SimboloAbstracto).IsAssignableFrom(otro.GetType())) && 
	               ((SimboloAbstracto)otro).indice == this.indice;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7;
                hash = 29 * hash + this.indice;
                hash = 29 * hash + (this.texto != null ? this.texto.GetHashCode() : 0);
                return hash;
            }
        }

        public int Indice
        {
            set { this.indice = value; }
            get { return this.indice; }
        }

        public string Texto
        {
            set { this.texto = value; }
            get { return this.texto; }
        }

        public int Linea
        {
            set { this.linea = value; }
            get { return this.linea; }
        }

        public int Columna
        {
            set { this.columna = value; }
            get { return this.columna; }
        }

        public override string ToString(){
            return texto;
        }
    
        public abstract object clone();
    }
}
