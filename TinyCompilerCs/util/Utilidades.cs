/**
 * Utilidades.cs
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
using TUVienna.CS_CUP.Runtime;
using TinyCompiler.tabla;
using TinyCompiler.parser;
using System.Linq;
using System.Text;

namespace TinyCompiler.util
{
    public class Utilidades
    {
        private static string padding = "                                                                                ";

        public static string tokenEnTexto(Symbol s)
        {
            switch (s.sym)
            {
                case TokenDef.EOF:
                    return "FIN_ARCHIVO";
                case TokenDef.SUMA:
                    return "+";
                case TokenDef.RESTA:
                    return "-";
                case TokenDef.DIVISION:
                    return "/";
                case TokenDef.MULTIPLICACION:
                    return "*";
                case TokenDef.PUNTO_Y_COMA:
                    return ";";
                case TokenDef.MENOR:
                    return "<";
                case TokenDef.IGUAL:
                    return "=";
                case TokenDef.PARENT_IZQ:
                    return "(";
                case TokenDef.PARENT_DER:
                    return ")";
                case TokenDef.ASIGNACION:
                    return ":=";
                case TokenDef.LIT_ENTERO:
                    return "ENTERO";
                case TokenDef.IDENTIFICADOR:
                    return "ID";
                case TokenDef.SI:
                    return "SI";
                case TokenDef.ENTONCES:
                    return "ENTONCES";
                case TokenDef.SINO:
                    return "SINO";
                case TokenDef.FIN:
                    return "FIN";
                case TokenDef.REPITA:
                    return "REPITA";
                case TokenDef.HASTA:
                    return "HASTA";
                case TokenDef.ERROR:
                    return "ERROR";
                default:
                    return ("<Token inválido: " + s.sym + ">");
            }
        }

        public static void imprimirToken(Symbol s)
        {
            Console.Error.Write(tokenEnTexto(s));
            SimboloAbstracto sym = (SimboloAbstracto)s.value; ;

            switch (s.sym)
            {
                case TokenDef.LIT_ENTERO:
                case TokenDef.IDENTIFICADOR:
                    Console.Error.Write(" = " + sym.Texto);
                    break;
                case TokenDef.error:
                case TokenDef.ERROR:
                    Console.Error.Write(" = \"");
                    Console.Error.Write(sym.Texto);
                    Console.Error.Write("\"");
                    break;
            }
            Console.Error.WriteLine("");
        }

        public static void dumpToken(System.IO.TextWriter str, Symbol s)
        {
            SimboloAbstracto sym = (SimboloAbstracto)s.value;
            str.Write("#" + sym.Linea + ":" + sym.Columna + ": " + tokenEnTexto(s));

            switch (s.sym)
            {
                case TokenDef.LIT_ENTERO:
                case TokenDef.IDENTIFICADOR:
                    str.Write(" = " + sym.Texto);
                    break;
                case TokenDef.error:
                case TokenDef.ERROR:
                    str.Write(" = \"");
                    str.Write(sym.Texto);
                    str.Write("\"");
                    break;
            }
            str.WriteLine("");
        }

        public static String pad(int n)
        {
            if (n > 80)
            {
                return padding;
            }
            if (n < 0)
            {
                return "";
            }
            return padding.Substring(0, n);
        }

        public static void fatalError(String msg)
        {
            throw new Exception(msg);
            //System.Console.Error.WriteLine((new Exception(msg)).StackTrace);
            //System.Environment.Exit(1);
        }
    }
}
