/**
 * tiny.cs
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
using System.IO;
using System.Collections.Generic;
using TUVienna.CS_CUP.Runtime;
using TinyCompiler.util;
using TinyCompiler.parser;
using TinyCompiler.ast;
using System.Linq;
using System.Text;
using TinyCompiler.semantica;
using TinyCompiler.generador;

namespace TinyCompiler.tiny
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader input;
            String nombreArchivo;
            Dictionary<String, String> opciones = parseArgs(args);

            //Validar que no existan opciones incorrectas
            if (opciones.ContainsKey("--help") || opciones.Count == 0)
                mostrarUso();
            else if(opcionInvalida(opciones)){
                Console.WriteLine("Error: opción incorrecta");
                mostrarUso();
            }
            
            try
            {
                //Validar el archivo de entrada
                if (!opciones.ContainsKey("--entrada"))
                {
                    Console.WriteLine("Error: falta archivo de entrada");
                    mostrarUso();
                }
                nombreArchivo = opciones["--entrada"];
                input = new System.IO.StreamReader(nombreArchivo);
                TinyLexer lexer = new TinyLexer(input);

                if (opciones.ContainsKey("--mostrar-tokens"))
                {
                    Console.WriteLine("\n\n\n");
                    mostrarTokens(lexer, input);
                }

                lexer.setNombreArchivo(nombreArchivo);
                TinyParser parser = new TinyParser(lexer);
                Programa result = (Programa)(parser.parse().value);
                if (parser.getOmerrs() > 0)
                {
                    System.Console.Error.WriteLine("La compilación ha terminado con " + parser.getOmerrs() + " errores");
                    WriteKeyPressForExit();
                    System.Environment.Exit(1);
                }

                if (opciones.ContainsKey("--mostrar-ast"))
                {
                    Console.WriteLine("\n\n\n");
                    Console.WriteLine("\n####### Inicio del Árbol Sintáctico Abstracto #######");
                    result.dump(System.Console.Out, 0);
                    Console.WriteLine("####### Fin del Árbol Sintáctico Abstracto #######\n");
                }

                AnalizadorSemantico sem = new AnalizadorSemantico(result);
                sem.analizar();

                if (SemantErrorReport.Instancia.tieneErrores())
                {
                    Console.Error.WriteLine("La compilación ha terminado con " + SemantErrorReport.Instancia.Errores + " errores");
                    WriteKeyPressForExit();
                    System.Environment.Exit(1);
                }
                else
                {
                    if (opciones.ContainsKey("--mostrar-tipos"))
                    {
                        Console.WriteLine("\n\n\n");
                        Console.WriteLine("####### Inicio del Árbol Sintáctico Abstracto Anotado #######");
                        result.dump(System.Console.Out, 0);
                        Console.WriteLine("####### Fin del Árbol Sintáctico Abstracto Anotado #######\n");
                    }

                    Console.WriteLine("\n\n\n");

                    string path = Directory.GetCurrentDirectory();
                    Console.WriteLine("El directorio donde se ha generado el ejecutable es {0}", path);
                    Sentencia s = result.Sentencias.getLista()[1];
                    Condicional c = (Condicional)s;


                    GeneradorCodigo gen = new GeneradorCodigo(result, System.IO.Path.GetFileName(nombreArchivo), System.IO.Path.GetFileName(nombreArchivo) + ".exe");

                    if (opciones.ContainsKey("--generar-con-ast"))
                        gen.GenerarEjecutableconAst(false);
                    else if (opciones.ContainsKey("--generar-con-func"))
                        gen.GenerarEjecutableconFuncion(false);
                    else
                        gen.GenerarEjecutableconFuncion(false);

                    Console.WriteLine("La compilación ha finalizado correctamente");
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("!!! ERROR GRAVE: EL COMPILADOR NO GENERADO DE FORMA CORRECTA EL EJECUTABLE !!!");
                throw new System.Exception("ERROR-0001: Un error inesperado no ha permitido llevar a cabo de forma correcta el proceso de compilacion");
            }
            WriteKeyPressForExit();
        }

        static void mostrarUso()
        {
            Console.WriteLine("Usar: ");
            Console.WriteLine("\tTinyCompiler [opciones] programa.tny");
            Console.WriteLine("Opciones:");
            Console.WriteLine("\t--generar-con-func genera el codigo objeto");
            Console.WriteLine("\t\tusando una funcion que recorre el AST");
            Console.WriteLine("\t--generar-con-ast genera el codigo objeto");
            Console.WriteLine("\t\tllamando a la funcion generar del nodo principal del AST [por defecto]");
            Console.WriteLine("\t--mostrar-tokens muestra información de los tokens");
            Console.WriteLine("\t\treconocidos por el analizador léxico");
            Console.WriteLine("\t--mostrar-ast muestra el árbol sintáctico abstracto");
            Console.WriteLine("\t\tcreado en el proceso de análisis sintáctico");
            Console.WriteLine("\t--mostrar-tipos muestra el árbol sintáctico abstracto");
            Console.WriteLine("\t\tindicando los tipos de las expresiones");
            Console.WriteLine("\t--ayuda muestra este mensaje de ayuda");

            WriteKeyPressForExit();
            System.Environment.Exit(1);
        }

        static void mostrarTokens(TinyLexer lexer, System.IO.StreamReader input)
        {
            Console.WriteLine("####### Inicio del análisis léxico #######");
            Symbol symbol;
            while ((symbol = lexer.next_token()).sym != TokenDef.EOF)
            {
                Utilidades.dumpToken(System.Console.Out, symbol);
            }

            input.BaseStream.Position = 0;
            input.DiscardBufferedData();
            Console.WriteLine("####### Fin del análisis léxico #######");
        }

        static bool opcionInvalida(Dictionary<String, String> opciones)
        {
            List<String> opts = new List<string>() {"--help", "--mostrar-tokens",
                                                    "--mostrar-ast", "--mostrar-tipos", "--entrada", "--generar-con-func", "--generar-con-ast"};

            foreach (String val in opciones.Keys)
            {
                if (!opts.Contains(val))
                    return true;
            }

            return false;
        }

        static void WriteKeyPressForExit(ConsoleKey key = ConsoleKey.Enter)
        {
            Console.WriteLine();
            Console.WriteLine("Presione la tecla {0} para salir ...", key);
            while (Console.ReadKey(intercept: true).Key != key) { }
        }

        static Dictionary<string, string> parseArgs(String[] args, bool dump=false)
        {
            Dictionary<string, string> map;

            map = UtilidadCMD.analizadorSimpleCMD(args); ;

            if (dump)
            {
                Console.WriteLine("Opciones: ");
                foreach (KeyValuePair<String, String> pair in map)
                {
                    Console.WriteLine("\t"+pair.Key + ": "+pair.Value);
                }
            }

            return map;
        }
    }
}
