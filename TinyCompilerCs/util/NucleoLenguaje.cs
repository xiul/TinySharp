/**
 * NucleoLenguaje.cs
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
using TinyCompiler.ast;
using Emit = System.Reflection.Emit;
using TinyCompiler.generador;

namespace TinyCompiler.util
{
    public sealed class NucleoLenguaje
    {
        public static readonly SimboloAbstracto tipoEntero = TablaAbstracta.idTabla.agregarSimbolo("_entero", 0, 0);
        public static readonly SimboloAbstracto tipoBool = TablaAbstracta.idTabla.agregarSimbolo("_lógico", 0, 0);
        public static readonly SimboloAbstracto idRead = TablaAbstracta.idTabla.agregarSimbolo("read", 0, 0);
        public static readonly SimboloAbstracto idWrite = TablaAbstracta.idTabla.agregarSimbolo("write", 0, 0);
        private List<FuncionDef> funcBasicas;

        private static readonly NucleoLenguaje _instance = new NucleoLenguaje();

        public static NucleoLenguaje Instancia
        {
            get { return _instance; }
        }

        private NucleoLenguaje()
        {
            funcBasicas = new List<FuncionDef>();
            agregarFuncBasicas();
        }

        //Se inicializa la lista de funciones basicas incluyendo la definicion en Intermediate Language de la funcion [FuncionesEstandar.x]
        private void agregarFuncBasicas()
        {
            ListaParamFormal formal;
            FuncionDef f;

            formal = new ListaParamFormal();
            formal.agregarElemento(new ParamFormal(tipoEntero, new SimboloId("x", 0, 0), true, 0, 0));
            f = new FuncionDef(idRead, formal, 0, 0, FuncionesEstandar.metodoFuncionRead);
            funcBasicas.Add(f);

            formal = new ListaParamFormal();
            formal.agregarElemento(new ParamFormal(tipoEntero, new SimboloId("x", 0, 0), 0, 0));
            f = new FuncionDef(idWrite, formal, 0, 0, FuncionesEstandar.metodoFuncionWrite);
            funcBasicas.Add(f);
        }

        public List<FuncionDef> getFuncBasicas()
        {
            return funcBasicas;
        }
    }
}
