/**
 * AnalizadorSemantico.cs
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
using TinyCompiler.ast;
using TinyCompiler.tabla;
using TinyCompiler.util;

namespace TinyCompiler.semantica
{
    public class AnalizadorSemantico
    {
        private Programa program;
        private TablaSimbolos tabla;

        public AnalizadorSemantico(Programa program)
        {
            this.program = program;
        }

        private void instalarFuncBasicas(){
            //crear la tabla de simbolos
            tabla = new TablaSimbolos();
        
            //agregar los métodos básicos a la tabla.
            List<FuncionDef> funs = NucleoLenguaje.Instancia.getFuncBasicas();
        
            tabla.crearAmbito();
            foreach(FuncionDef fun in funs){
                tabla.agregarId(fun.Nombre, fun);
            }
        }

        public int analizar()
        {
            instalarFuncBasicas();
            program.checkSemantica(tabla);

            return SemantErrorReport.Instancia.Errores;
        }
    }
}
