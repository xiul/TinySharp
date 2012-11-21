/**
tiny.cup

 Copyrigth (C) 2012: 
 Ochoa, Luis lochoa (at unet.edu.ve)
 Sanchez, Manuel mbsanchez (at unet.edu.ve)

 This library is free software; you can redistribute it and/or
 modify it under the terms of the GNU Lesser General Public
 License as published by the Free Software Foundation; either
 version 2.1 of the License, or (at your option) any later version.

 This library is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public
 License along with this library; if not, write to the Free Software
 Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 MA 02110-1301 USA
 */

using TUVienna.CS_CUP;
using TUVienna.CS_CUP.Runtime;
using System.Collections;
using TinyCompiler.parser;
using TinyCompiler.tabla;
%%
%{
	private SimboloAbstracto nombreArchivo;
	private int yycolumn;
	
	public int getLinea(){
		return yyline+1;
	}
	
	public int getColumna(){
		return yycolumn+1;
	}

    public void setNombreArchivo(string nombre) {
		nombreArchivo = TablaAbstracta.texTabla.agregarSimbolo(nombre, getLinea(), getColumna());
    }

    public SimboloAbstracto getNombreArchivo() {
		return nombreArchivo;
    }
	
	void updateColumn()
	{
		int i;

		for (i = 0; i<yytext().Length; i++)
			if (yytext()[i] == '\n')
				yycolumn = 0;
			else if (yytext()[i] == '\t')
				yycolumn += 8 - (yycolumn % 8);
			else
				yycolumn++;
	}
%}
%public
%class TinyLexer
%line
%cup

%eofval{
  return new Symbol(TokenDef.EOF);
%eofval}

BLANCO = [ \n\r\t]
COMENTARIO = "{"[^}]*"}"
%%
<YYINITIAL>if			{ Symbol sym = new Symbol(TokenDef.SI, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>then			{ Symbol sym = new Symbol(TokenDef.ENTONCES, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>else			{ Symbol sym = new Symbol(TokenDef.SINO, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>end			{ Symbol sym = new Symbol(TokenDef.FIN, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>repeat		{ Symbol sym = new Symbol(TokenDef.REPITA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>until		{ Symbol sym = new Symbol(TokenDef.HASTA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"+"			{ Symbol sym = new Symbol(TokenDef.SUMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"-"			{ Symbol sym = new Symbol(TokenDef.RESTA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"*"			{ Symbol sym = new Symbol(TokenDef.MULTIPLICACION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"/"			{ Symbol sym = new Symbol(TokenDef.DIVISION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>";"			{ Symbol sym = new Symbol(TokenDef.PUNTO_Y_COMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>","			{ Symbol sym = new Symbol(TokenDef.COMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"<"			{ Symbol sym = new Symbol(TokenDef.MENOR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"="			{ Symbol sym = new Symbol(TokenDef.IGUAL, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>"("			{ Symbol sym = new Symbol(TokenDef.PARENT_IZQ, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>")"			{ Symbol sym = new Symbol(TokenDef.PARENT_DER, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>[0-9]+       { Symbol sym = new Symbol(TokenDef.LIT_ENTERO, TablaAbstracta.intTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>[a-zA-Z]+	{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}	
<YYINITIAL>":="			{ Symbol sym = new Symbol(TokenDef.ASIGNACION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
<YYINITIAL>{BLANCO}		{updateColumn(); break;}
<YYINITIAL>{COMENTARIO} {updateColumn(); break;}
.						{ Symbol sym = new Symbol(TokenDef.ERROR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}