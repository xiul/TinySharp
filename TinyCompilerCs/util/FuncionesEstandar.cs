/**
 * FuncionesEstandar.cs
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
using TinyCompiler.ast;
using Emit = System.Reflection.Emit;

namespace TinyCompiler.util
{
    public class FuncionesEstandar
    {
        
        //Funcion estandar WRITE parametro(s) una o mas expresiones (variable o constate)
        public static Action<Emit.ILGenerator, ListaParametros> metodoFuncionWrite =
            (Emit.ILGenerator il, ListaParametros lp) =>
            {
                foreach (Expresion expr in lp.getLista())
                {
                    expr.generar(il);
                    il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(System.Int32) }));
                }
            };

        
        //Funcion estandar READ parametro(s) una o mas Variables
        public static Action<Emit.ILGenerator, ListaParametros> metodoFuncionRead =
        (Emit.ILGenerator il, ListaParametros lp) =>
        {
            foreach (Expresion expr in lp.getLista())
            {
                if (expr.GetType() == typeof(Variable))
                {
                    il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("ReadLine", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { }, null));
                    il.Emit(Emit.OpCodes.Call, typeof(System.Int32).GetMethod("Parse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { typeof(string) }, null));
                    Variable v = (Variable)expr;
                    TinyCompiler.generador.GeneradorCodigo.Almacenar(v.Nombre.Texto, il);
                }
                else
                {
                    throw new System.Exception("ERROR-0006 solo se puede generar un read para un nodo del ast de tipo Variable");
                }
            }
        };
    }
}
