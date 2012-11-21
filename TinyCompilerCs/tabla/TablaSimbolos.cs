/**
 * TablaSimbolos.cs
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

namespace TinyCompiler.tabla
{
    public class TablaSimbolos
    {
        private ArrayStack<Dictionary<SimboloAbstracto, Object>> tbl;
    
        public TablaSimbolos() {
	        tbl = new ArrayStack<Dictionary<SimboloAbstracto, Object>>();
        }
    
        public void crearAmbito() {
            tbl.Push(new Dictionary<SimboloAbstracto, Object>());
        }

        public void quitarAmbito() {
	        if (tbl.IsEmpty()) {
	            Utilidades.fatalError("quitarAmbito: no se puede remover el ámbito de la tabla.");
	        }
	        tbl.Pop();
        }

        public void agregarId(SimboloAbstracto id, Object info) {
	        if (tbl.IsEmpty()) {
	            Utilidades.fatalError("agregarId: no se puede agregar id sin un ámbito.");
	        }
	        tbl.Peek()[id] = info;
        }

        public Object buscar(SimboloAbstracto sym) {
	        if (tbl.IsEmpty()) {
	            Utilidades.fatalError("buscar: no existen ámbitos en la tabla.");
	        }

	        for (int i = tbl.Count - 1; i >= 0; i--) {
                if(tbl[i].ContainsKey(sym)){
	                return tbl[i][sym];
                }
	        }
	        return null;
        }

        public Object buscarAmbitoActual(SimboloAbstracto sym) {
	        if (tbl.IsEmpty()) {
	            Utilidades.fatalError("explorar: no existen ámbitos en la tabla.");
	        }
            if(tbl.Peek().ContainsKey(sym))
	            return tbl.Peek()[sym];

            return null;
        }
    
        public override String ToString() {
	        String res = "";
	        for (int i = tbl.Count - 1, j = 0; i >= 0; i--, j++) {
	            res += "Scope " + j + ": " + tbl[i] + "\n";
	        }
	        return res;
        }
        public void combinar(TablaSimbolos table){
            Dictionary<SimboloAbstracto, Object> env = table.tbl.Peek();
            env.Concat(tbl.Peek());
        }
    
        public TablaSimbolos copy(){
            TablaSimbolos table = new TablaSimbolos();
            for (int i = 0; i < tbl.Count; i++) {
                table.crearAmbito();
                table.tbl.Peek().Concat(tbl[i]);
            }
        
            return table;
        }
    }
}
