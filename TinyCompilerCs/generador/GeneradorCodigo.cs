/**
 * GeneradorCodigo.cs
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
using TinyCompiler.util;

//Librerias para la generacion de codigo
using System.Globalization;
using Collections = System.Collections.Generic;
using Reflect = System.Reflection;
using Emit = System.Reflection.Emit;
using IO = System.IO;

namespace TinyCompiler.generador
{
    public class GeneradorCodigo
    {
        private Programa ast;
        private String nombreCodigoFuente;
        private String nombrePrograma;

        //ILGenerator Permite Generar un flujo o stream de Microsoft intermediate language (MSIL) o Intermediate Language (IL) del ECMA-335.
        Emit.ILGenerator il = null; 
        
        //Tabla con las direcciones de memoria de los simbolos que se generan (sin manejo de ambitos)
        public static Collections.Dictionary<string, Emit.LocalBuilder> TablaDireccionesSimbolos = new Collections.Dictionary<string, Emit.LocalBuilder>();
        Collections.Stack<Emit.Label> sentenciasAbandona = new Collections.Stack<Emit.Label>(6);

        Reflect.AssemblyName nombre;
        Emit.AssemblyBuilder asmb;
        Emit.ModuleBuilder modb;
        Emit.TypeBuilder typeBuilder;
        Emit.MethodBuilder methb;

        public GeneradorCodigo(Programa ast, String origen, String nombrePrograma)
        {
            this.ast = ast;
            this.nombreCodigoFuente = origen;
            this.nombrePrograma = nombrePrograma;
        }
        
        public void GenerarEjecutableconFuncion(bool depurable = false)
        {
            inicializar(nombreCodigoFuente, nombrePrograma);
            generar(ast);
            finalizar(nombrePrograma);
        }

        public void GenerarEjecutableconAst(bool depurable = false)
        {
            inicializar(nombreCodigoFuente, nombrePrograma);
            ast.generar(il);
            finalizar(nombrePrograma);
        }

        private void inicializar(string nombreCodigoFuente, string nombrePrograma)
        {
            if (TablaDireccionesSimbolos.Count > 0)
            {
                throw new System.Exception("ERROR-0002: Tabla de direcciones de variables ha sido inicializada previamente");

            }

            String rutaEjecutable = IO.Path.GetDirectoryName(nombrePrograma);

            //Describo el identificador unico del ensamblado que se esta generando
            nombre = new Reflect.AssemblyName(IO.Path.GetFileNameWithoutExtension(nombrePrograma));

            //Creo la representacion basica del ensamblado creado de forma dinamica (assembly)
            if (rutaEjecutable.Length > 0)
                asmb = System.AppDomain.CurrentDomain.DefineDynamicAssembly(nombre, Emit.AssemblyBuilderAccess.RunAndSave, rutaEjecutable);
            else
                asmb = System.AppDomain.CurrentDomain.DefineDynamicAssembly(nombre, Emit.AssemblyBuilderAccess.RunAndSave);




            Console.WriteLine("Common Runtime usado para generar el ejecutable del programa: " + asmb.ImageRuntimeVersion);
            Console.WriteLine("Se esta ejecutando el compilador bajo el CLR version {0}", Environment.Version);

            //Defino un nuevo modulo de .net de forma dinamica
            modb = asmb.DefineDynamicModule(IO.Path.GetFileName(nombrePrograma), false);

            //Creo una nueva clase en el Il Generator stream
            typeBuilder = modb.DefineType("pseudoGenerado");

            //Debido a que los programas en este tiny solo cuentan con un unico metodo principal (Main) de forma estructurada
            //como un truco defino el metodo Main en un objeto por defecto para ejecutar todas las acciones
            //expresadas por el lenguaje alli.
            //formato: NOMBRE -> Main   TIPO-> Static  RETORNA-> void  PARAMETROS-> vacios
            methb = typeBuilder.DefineMethod("Main", Reflect.MethodAttributes.HideBySig | Reflect.MethodAttributes.Static | Reflect.MethodAttributes.Public, typeof(void), System.Type.EmptyTypes);

            //Inicializo/Creo el generador/stream de codigo IL con el metodo donde generare el codigo actualmente
            this.il = methb.GetILGenerator();
            
            //Para iniciar el programa Emito una primera instruccion vacia (no hace nada)
            il.Emit(Emit.OpCodes.Nop);
        }

        private void finalizar(string nombreEjecutable)
        {
            //Creo el assembler .net y defino las ultimas opciones de configuracion
            il.Emit(Emit.OpCodes.Ret);
            Type pp = typeBuilder.CreateType();
            modb.CreateGlobalFunctions();
            asmb.SetEntryPoint(methb, Reflect.Emit.PEFileKinds.ConsoleApplication);
            asmb.Save(IO.Path.GetFileName(nombreEjecutable));
            //Ahora procedo a limpiar la tabla de direcciones para su proximo uso.
            TablaDireccionesSimbolos.Clear();
            this.il = null;
        }

       //Almacena el valor del tope de la pila en el nombre de variable proporcionado (no maneja ambito)
        public static void Almacenar(string nombre, Emit.ILGenerator _il)
        {   if (!TablaDireccionesSimbolos.ContainsKey(nombre))
            {
                TablaDireccionesSimbolos[nombre] = _il.DeclareLocal(typeof(System.Int32));  //Solo variables enteras
            }

            Emit.LocalBuilder variableLocal = TablaDireccionesSimbolos[nombre];

            if (!variableLocal.LocalType.HasElementType) //(es una variable simple) verdadero solo en caso de vectores o matrices
            {
                _il.Emit(Emit.OpCodes.Stloc, TablaDireccionesSimbolos[nombre]);
            }
            else //es una matriz o vector y actuo acorde
            {
                throw new System.Exception("No se soportan arrays o matrices en las asignaciones");
            }
        }

        public static System.Int32 convertir_a_entero(string numero)
        {
            int valor;
            try
            {
                valor = Convert.ToInt32(numero);
            }
            catch (FormatException e)
            {
                throw new System.Exception("ERROR-0003: La entrada a convertir a entero no es una secuencia de digitos: " + numero + "  " + e.Message);
            }
            catch (OverflowException e)
            {
                throw new System.Exception("ERROR-0004: La entrada a convertir a entero no puede ser representado en 32 bits:" + numero + "  " + e.Message);
            }

                if (valor < Int32.MaxValue)
                {
                    return valor;
                }
                else
                {
                    throw new System.Exception("ERROR-0005 El codigo objeto se ha generado con errores de rango en enteros");
                }
                
        }

        
        //procedimiento recursivo que genera el arbol
        private void generar(NodoArbol nodoActual)
        {

            //Valido nodos nulos para evitar incluir condicionales en generacion de nodos cuando es nulo
            if (nodoActual == null)
                return;

            if (nodoActual.GetType() == typeof(Programa))
            {
                Programa n = (Programa) nodoActual;
                generar(n.Sentencias);
            }
            else if (nodoActual.GetType() == typeof(ListaSentencia))
            {
                ListaSentencia ls = (ListaSentencia)nodoActual;
                foreach (Sentencia sent in ls.getLista())
                    generar(sent);
            }
            else if (nodoActual.GetType() == typeof(Condicional))
            {
                Emit.LocalBuilder tmpVarLogico;
                Condicional n = (Condicional) nodoActual;
                
                generar(n.Condicion);

                this.il.Emit(Emit.OpCodes.Ldc_I4_0);  //Ingreso constante 0
                this.il.Emit(Emit.OpCodes.Ceq);  //Comparo si es falso (es 0)
                //Almaceno este resultado en una variable temporal
                tmpVarLogico = this.il.DeclareLocal(typeof(bool));
                this.il.Emit(Emit.OpCodes.Stloc, tmpVarLogico);
                //cargo el resultado de la variable temporal
                this.il.Emit(Emit.OpCodes.Ldloc, tmpVarLogico);
                Emit.Label bloqueFalso = this.il.DefineLabel();
                //salto en caso que sea verdadero el resultado es decir es cero la evaluacion del (pila==0) hago el sino
                this.il.Emit(Emit.OpCodes.Brtrue, bloqueFalso);

                generar(n.Entonces);

                Emit.Label finSi = this.il.DefineLabel();
                this.il.Emit(Emit.OpCodes.Br, finSi);
                this.il.MarkLabel(bloqueFalso);

                generar(n.Sino);

                this.il.MarkLabel(finSi);
            }
            else if (nodoActual.GetType() == typeof(RepitaHasta))
            {
                RepitaHasta n = (RepitaHasta)nodoActual;
                Emit.LocalBuilder tmpCondicion;
                tmpCondicion = this.il.DeclareLocal(typeof(bool));
                Emit.Label sentenciasRepita = this.il.DefineLabel();
                this.il.MarkLabel(sentenciasRepita);
                this.il.Emit(Emit.OpCodes.Nop);		//emito primera sentencia vacia															

                generar(n.Sentencias);

                generar(n.Condicion);

                this.il.Emit(Emit.OpCodes.Stloc, tmpCondicion);  //almaceno resultado de condicion del mientras	
                this.il.Emit(Emit.OpCodes.Ldloc, tmpCondicion);  //cargo resultado de condicion del mientras
                this.il.Emit(Emit.OpCodes.Brfalse, sentenciasRepita);

            }
            else if (nodoActual.GetType() == typeof(Asignacion))
            {
                Asignacion n = (Asignacion)nodoActual;

                generar(n.Expr);

                Almacenar(n.Id.Nombre.Texto,this.il);
            }
            else if (nodoActual.GetType() == typeof(LLamadaFuncion)) //Leer, Escribir
            {
                LLamadaFuncion n = (LLamadaFuncion)nodoActual;
                List<FuncionDef> funs = NucleoLenguaje.Instancia.getFuncBasicas();
                //En la lista de definiciones de funciones estandar busco la definicion de la llamada para funcion deseada
                foreach (FuncionDef fun in funs)
                {
                    if (n.Id.Texto.Equals(fun.Nombre.Texto))
                        fun.ejecutarMetodo(il, n.Params);
                }

            }
            else if (nodoActual.GetType() == typeof(NumeroEntero))
            {
                NumeroEntero n = (NumeroEntero)nodoActual;
                this.il.Emit(Emit.OpCodes.Ldc_I4, convertir_a_entero(n.Token.Texto));
            }
            else if (nodoActual.GetType() == typeof(Variable))
            {
                Variable n = (Variable)nodoActual;
                if (!TablaDireccionesSimbolos.ContainsKey(n.Nombre.Texto))
                {
                    throw new System.Exception("ERROR-0008 fallo en analisis semantico Variable no declarada encontrada durante generacion '" + n.Nombre.Texto + "'");
                }
                else
                {
                    this.il.Emit(Emit.OpCodes.Ldloc, TablaDireccionesSimbolos[n.Nombre.Texto]);
                }
            }
            else if (nodoActual.GetType() == typeof(Suma))
            {
                Suma n = (Suma)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Add);
            }
            else if (nodoActual.GetType() == typeof(Resta))
            {
                Resta n = (Resta)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Sub);
            }
            else if (nodoActual.GetType() == typeof(Multiplicacion))
            {
                Multiplicacion n = (Multiplicacion)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Mul);
            }
            else if (nodoActual.GetType() == typeof(Division))
            {
                Division n = (Division)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Div);
            }
            else if (nodoActual.GetType() == typeof(Menor))
            {
                Menor n = (Menor)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Clt);
            }
            else if (nodoActual.GetType() == typeof(Igual))
            {
                Igual n = (Igual)nodoActual;
                generar(n.ExprIzq);
                generar(n.ExprDer);
                this.il.Emit(Emit.OpCodes.Ceq);
            }
            else
            {
                Console.WriteLine("ERROR-0006 Error tipo de Nodo no identificado");
            }
        }


    }
}
