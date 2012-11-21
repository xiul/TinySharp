/**
 * LLamadaFuncion.cs
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
    public class LLamadaFuncion : Sentencia
    {
        protected SimboloAbstracto id;
        protected ListaParametros lparams;

        public LLamadaFuncion(SimboloAbstracto id, ListaParametros lparams, int linea, int columna) 
            : base(linea, columna)
        {
            this.id = id;
            this.lparams = lparams;
        }

        public override void generar(Emit.ILGenerator il)
        {
            List<FuncionDef> funs = NucleoLenguaje.Instancia.getFuncBasicas();
            foreach (FuncionDef fun in funs)
            {
                if(Id.Texto.Equals(fun.Nombre.Texto))
                    fun.ejecutarMetodo(il, this.Params);
            }
        }

        public SimboloAbstracto Id
        {
            get { return id; }
        }

        public ListaParametros Params
        {
            get { return lparams; }
        }

        public override void dump(System.IO.TextWriter str, int n) {
            dumpLineaColumna(str, n);
            str.WriteLine("_call");
            dump_SimboloAbstracto(str, n + 2, id);
            lparams.dump(str, n + 2);
        }

        public override void checkSemantica(TablaSimbolos tabla) {
            Object sym = tabla.buscar(id);
            bool exists = true, esVariable;
        
            //chequear que el método exista
            if(sym==null || sym.GetType() != typeof(FuncionDef)){
                SemantErrorReport.Instancia.semantError(this, "no se encuentra la definición para la función \""+id+"\"");
                exists = false;
            }
              
            //chequear que los tipos de los paramétros se correspondan con los de la función
            if(exists){
                FuncionDef f = (FuncionDef) sym;
            
                if(f.Parametros.getLongitud()!=lparams.getLongitud())
                    SemantErrorReport.Instancia.semantError(this, "la cantidad de paramétros no coincide con la definición de la función \""+id+"\"");
                else{
                    for(int i=0;i<lparams.getLongitud();i++){
                        Expresion param = lparams.getLista()[i];
                    
                        //si el paramétro es una variable agregarlo a la tabla de simbolo
                        esVariable = false;
                        if(param.GetType() == typeof(Variable)){
                            if(tabla.buscar(((Variable)param).Nombre)==null)
                                tabla.agregarId(((Variable)param).Nombre, param);
                            esVariable = true;
                        }
                        
                        param.checkSemantica(tabla);
                        if(!param.Tipo_Expr.Equals(f.Parametros.getLista()[i].Tipo) || 
                            (f.Parametros.getLista()[i].EsDireccion && !esVariable))
                            SemantErrorReport.Instancia.semantError(param, "el tipo de paramétro no coincide con la definición de la función \""+id+"\"");
                    }
                }
            }
        }
    }
}
