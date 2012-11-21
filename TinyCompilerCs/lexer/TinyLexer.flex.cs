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


public class TinyLexer : TUVienna.CS_CUP.Runtime.Scanner {
	private const int YY_BUFFER_SIZE = 512;
	private const int YY_F = -1;
	private const int YY_NO_STATE = -1;
	private const int YY_NOT_ACCEPT = 0;
	private const int YY_START = 1;
	private const int YY_END = 2;
	private const int YY_NO_ANCHOR = 4;
	private const int YY_BOL = 128;
	private const int YY_EOF = 129;

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
	private System.IO.TextReader yy_reader;
	private int yy_buffer_index;
	private int yy_buffer_read;
	private int yy_buffer_start;
	private int yy_buffer_end;
	private char[] yy_buffer;
	private int yyline;
	private bool yy_at_bol;
	private int yy_lexical_state;

	public TinyLexer (System.IO.TextReader yy_reader1) : this() {
		if (null == yy_reader1) {
			throw (new System.Exception("Error: Bad input stream initializer."));
		}
		yy_reader = yy_reader1;
	}

	private TinyLexer () {
		yy_buffer = new char[YY_BUFFER_SIZE];
		yy_buffer_read = 0;
		yy_buffer_index = 0;
		yy_buffer_start = 0;
		yy_buffer_end = 0;
		yyline = 0;
		yy_at_bol = true;
		yy_lexical_state = YYINITIAL;
	}

	private bool yy_eof_done = false;
	private const int YYINITIAL = 0;
	private static readonly int[] yy_state_dtrans =new int[] {
		0
	};
	private void yybegin (int state) {
		yy_lexical_state = state;
	}
	private int yy_advance ()
	{
		int next_read;
		int i;
		int j;

		if (yy_buffer_index < yy_buffer_read) {
			return yy_buffer[yy_buffer_index++];
		}

		if (0 != yy_buffer_start) {
			i = yy_buffer_start;
			j = 0;
			while (i < yy_buffer_read) {
				yy_buffer[j] = yy_buffer[i];
				++i;
				++j;
			}
			yy_buffer_end = yy_buffer_end - yy_buffer_start;
			yy_buffer_start = 0;
			yy_buffer_read = j;
			yy_buffer_index = j;
			next_read = yy_reader.Read(yy_buffer,
					yy_buffer_read,
					yy_buffer.Length - yy_buffer_read);
			if ( next_read<=0) {
				return YY_EOF;
			}
			yy_buffer_read = yy_buffer_read + next_read;
		}

		while (yy_buffer_index >= yy_buffer_read) {
			if (yy_buffer_index >= yy_buffer.Length) {
				yy_buffer = yy_double(yy_buffer);
			}
			next_read = yy_reader.Read(yy_buffer,
					yy_buffer_read,
					yy_buffer.Length - yy_buffer_read);
			if ( next_read<=0) {
				return YY_EOF;
			}
			yy_buffer_read = yy_buffer_read + next_read;
		}
		return yy_buffer[yy_buffer_index++];
	}
	private void yy_move_end () {
		if (yy_buffer_end > yy_buffer_start &&
		    '\n' == yy_buffer[yy_buffer_end-1])
			yy_buffer_end--;
		if (yy_buffer_end > yy_buffer_start &&
		    '\r' == yy_buffer[yy_buffer_end-1])
			yy_buffer_end--;
	}
	private bool yy_last_was_cr=false;
	private void yy_mark_start () {
		int i;
		for (i = yy_buffer_start; i < yy_buffer_index; ++i) {
			if ('\n' == yy_buffer[i] && !yy_last_was_cr) {
				++yyline;
			}
			if ('\r' == yy_buffer[i]) {
				++yyline;
				yy_last_was_cr=true;
			} else yy_last_was_cr=false;
		}
		yy_buffer_start = yy_buffer_index;
	}
	private void yy_mark_end () {
		yy_buffer_end = yy_buffer_index;
	}
	private void yy_to_mark () {
		yy_buffer_index = yy_buffer_end;
		yy_at_bol = (yy_buffer_end > yy_buffer_start) &&
		            ('\r' == yy_buffer[yy_buffer_end-1] ||
		             '\n' == yy_buffer[yy_buffer_end-1] ||
		             2028/*LS*/ == yy_buffer[yy_buffer_end-1] ||
		             2029/*PS*/ == yy_buffer[yy_buffer_end-1]);
	}
	private string yytext () {
		return (new string(yy_buffer,
			yy_buffer_start,
			yy_buffer_end - yy_buffer_start));
	}
	private int yylength () {
		return yy_buffer_end - yy_buffer_start;
	}
	private char[] yy_double (char[] buf) {
		int i;
		char[] newbuf;
		newbuf = new char[2*buf.Length];
		for (i = 0; i < buf.Length; ++i) {
			newbuf[i] = buf[i];
		}
		return newbuf;
	}
	private const int YY_E_INTERNAL = 0;
	private const int YY_E_MATCH = 1;
	private string[] yy_error_string = {
		"Error: Internal error.\n",
		"Error: Unmatched input.\n"
	};
	private void yy_error (int code,bool fatal) {
		 System.Console.Write(yy_error_string[code]);
		 System.Console.Out.Flush();
		if (fatal) {
			throw new System.Exception("Fatal Error.\n");
		}
	}
	private static int[][] unpackFromString(int size1, int size2, string st) {
		int colonIndex = -1;
		string lengthString;
		int sequenceLength = 0;
		int sequenceInteger = 0;

		int commaIndex;
		string workString;

		int[][] res = new int[size1][];
		for(int i=0;i<size1;i++) res[i]=new int[size2];
		for (int i= 0; i < size1; i++) {
			for (int j= 0; j < size2; j++) {
				if (sequenceLength != 0) {
					res[i][j] = sequenceInteger;
					sequenceLength--;
					continue;
				}
				commaIndex = st.IndexOf(',');
				workString = (commaIndex==-1) ? st :
					st.Substring(0, commaIndex);
				st = st.Substring(commaIndex+1);
				colonIndex = workString.IndexOf(':');
				if (colonIndex == -1) {
					res[i][j]=System.Int32.Parse(workString);
					continue;
				}
				lengthString =
					workString.Substring(colonIndex+1);
				sequenceLength=System.Int32.Parse(lengthString);
				workString=workString.Substring(0,colonIndex);
				sequenceInteger=System.Int32.Parse(workString);
				res[i][j] = sequenceInteger;
				sequenceLength--;
			}
		}
		return res;
	}
	private int[] yy_acpt = {
		/* 0 */ YY_NOT_ACCEPT,
		/* 1 */ YY_NO_ANCHOR,
		/* 2 */ YY_NO_ANCHOR,
		/* 3 */ YY_NO_ANCHOR,
		/* 4 */ YY_NO_ANCHOR,
		/* 5 */ YY_NO_ANCHOR,
		/* 6 */ YY_NO_ANCHOR,
		/* 7 */ YY_NO_ANCHOR,
		/* 8 */ YY_NO_ANCHOR,
		/* 9 */ YY_NO_ANCHOR,
		/* 10 */ YY_NO_ANCHOR,
		/* 11 */ YY_NO_ANCHOR,
		/* 12 */ YY_NO_ANCHOR,
		/* 13 */ YY_NO_ANCHOR,
		/* 14 */ YY_NO_ANCHOR,
		/* 15 */ YY_NO_ANCHOR,
		/* 16 */ YY_NO_ANCHOR,
		/* 17 */ YY_NO_ANCHOR,
		/* 18 */ YY_NO_ANCHOR,
		/* 19 */ YY_NO_ANCHOR,
		/* 20 */ YY_NO_ANCHOR,
		/* 21 */ YY_NO_ANCHOR,
		/* 22 */ YY_NO_ANCHOR,
		/* 23 */ YY_NO_ANCHOR,
		/* 24 */ YY_NOT_ACCEPT,
		/* 25 */ YY_NO_ANCHOR,
		/* 26 */ YY_NO_ANCHOR,
		/* 27 */ YY_NO_ANCHOR,
		/* 28 */ YY_NO_ANCHOR,
		/* 29 */ YY_NO_ANCHOR,
		/* 30 */ YY_NO_ANCHOR,
		/* 31 */ YY_NO_ANCHOR,
		/* 32 */ YY_NO_ANCHOR,
		/* 33 */ YY_NO_ANCHOR,
		/* 34 */ YY_NO_ANCHOR,
		/* 35 */ YY_NO_ANCHOR,
		/* 36 */ YY_NO_ANCHOR,
		/* 37 */ YY_NO_ANCHOR,
		/* 38 */ YY_NO_ANCHOR,
		/* 39 */ YY_NO_ANCHOR,
		/* 40 */ YY_NO_ANCHOR,
		/* 41 */ YY_NO_ANCHOR,
		/* 42 */ YY_NO_ANCHOR,
		/* 43 */ YY_NO_ANCHOR
	};
	private int[] yy_cmap = unpackFromString(1,130,
"29:9,27:2,29:2,27,29:18,27,29:7,22,23,16,14,19,15,29,17,24:10,26,18,20,21,2" +
"9:3,25:26,29:6,12,25:2,9,5,2,25,4,1,25:2,7,25,6,25,11,25,10,8,3,13,25:5,28," +
"29,30,29:2,0:2")[0];

	private int[] yy_rmap = unpackFromString(1,44,
"0,1,2,1:10,3,4,1,5,1:2,5:5,6,5,6,7,1,8,9,10,11,12,13,14,15,16,17,18,19,20,2" +
"1,22")[0];

	private int[][] yy_nxt = unpackFromString(23,31,
"1,2,25,38,25,33,25:4,43,25:2,41,3,4,5,6,7,8,9,10,11,12,13,25,14,15,26,28:2," +
"-1:32,25,16,25:11,-1:11,25,-1:29,13,-1:27,17,-1:10,25:13,-1:11,25,-1:6,24:2" +
"9,18,-1,25:8,19,25:4,-1:11,25,-1:6,25:5,20,25:7,-1:11,25,-1:6,25:4,21,25:8," +
"-1:11,25,-1:6,25:6,22,25:6,-1:11,25,-1:6,25:2,23,25:10,-1:11,25,-1:6,25:5,2" +
"7,35,25:6,-1:11,25,-1:6,25:4,29,25:8,-1:11,25,-1:6,25:7,30,25:5,-1:11,25,-1" +
":6,31,25:12,-1:11,25,-1:6,25:11,32,25,-1:11,25,-1:6,25:3,34,25:9,-1:11,25,-" +
"1:6,25:2,36,25:10,-1:11,25,-1:6,25:4,37,25:8,-1:11,25,-1:6,25:5,39,25:7,-1:" +
"11,25,-1:6,25:10,40,25:2,-1:11,25,-1:6,25:4,42,25:8,-1:11,25,-1:5");

	public TUVienna.CS_CUP.Runtime.Symbol next_token ()
 {
		int yy_lookahead;
		int yy_anchor = YY_NO_ANCHOR;
		int yy_state = yy_state_dtrans[yy_lexical_state];
		int yy_next_state = YY_NO_STATE;
		int yy_last_accept_state = YY_NO_STATE;
		bool yy_initial = true;
		int yy_this_accept;

		yy_mark_start();
		yy_this_accept = yy_acpt[yy_state];
		if (YY_NOT_ACCEPT != yy_this_accept) {
			yy_last_accept_state = yy_state;
			yy_mark_end();
		}
		while (true) {
			if (yy_initial && yy_at_bol) yy_lookahead = YY_BOL;
			else yy_lookahead = yy_advance();
			yy_next_state = YY_F;
			yy_next_state = yy_nxt[yy_rmap[yy_state]][yy_cmap[yy_lookahead]];
			if (YY_EOF == yy_lookahead && true == yy_initial) {

  return new Symbol(TokenDef.EOF);
			}
			if (YY_F != yy_next_state) {
				yy_state = yy_next_state;
				yy_initial = false;
				yy_this_accept = yy_acpt[yy_state];
				if (YY_NOT_ACCEPT != yy_this_accept) {
					yy_last_accept_state = yy_state;
					yy_mark_end();
				}
			}
			else {
				if (YY_NO_STATE == yy_last_accept_state) {
					throw (new System.Exception("Lexical Error: Unmatched Input."));
				}
				else {
					yy_anchor = yy_acpt[yy_last_accept_state];
					if (0 != (YY_END & yy_anchor)) {
						yy_move_end();
					}
					yy_to_mark();
					switch (yy_last_accept_state) {
					case 1:
						break;
					case -2:
						break;
					case 2:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -3:
						break;
					case 3:
						{ Symbol sym = new Symbol(TokenDef.SUMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -4:
						break;
					case 4:
						{ Symbol sym = new Symbol(TokenDef.RESTA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -5:
						break;
					case 5:
						{ Symbol sym = new Symbol(TokenDef.MULTIPLICACION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -6:
						break;
					case 6:
						{ Symbol sym = new Symbol(TokenDef.DIVISION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -7:
						break;
					case 7:
						{ Symbol sym = new Symbol(TokenDef.PUNTO_Y_COMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -8:
						break;
					case 8:
						{ Symbol sym = new Symbol(TokenDef.COMA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -9:
						break;
					case 9:
						{ Symbol sym = new Symbol(TokenDef.MENOR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -10:
						break;
					case 10:
						{ Symbol sym = new Symbol(TokenDef.IGUAL, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -11:
						break;
					case 11:
						{ Symbol sym = new Symbol(TokenDef.PARENT_IZQ, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -12:
						break;
					case 12:
						{ Symbol sym = new Symbol(TokenDef.PARENT_DER, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -13:
						break;
					case 13:
						{ Symbol sym = new Symbol(TokenDef.LIT_ENTERO, TablaAbstracta.intTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -14:
						break;
					case 14:
						{ Symbol sym = new Symbol(TokenDef.ERROR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -15:
						break;
					case 15:
						{updateColumn(); break;}
					case -16:
						break;
					case 16:
						{ Symbol sym = new Symbol(TokenDef.SI, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -17:
						break;
					case 17:
						{ Symbol sym = new Symbol(TokenDef.ASIGNACION, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -18:
						break;
					case 18:
						{updateColumn(); break;}
					case -19:
						break;
					case 19:
						{ Symbol sym = new Symbol(TokenDef.FIN, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -20:
						break;
					case 20:
						{ Symbol sym = new Symbol(TokenDef.ENTONCES, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -21:
						break;
					case 21:
						{ Symbol sym = new Symbol(TokenDef.SINO, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -22:
						break;
					case 22:
						{ Symbol sym = new Symbol(TokenDef.HASTA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -23:
						break;
					case 23:
						{ Symbol sym = new Symbol(TokenDef.REPITA, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -24:
						break;
					case 25:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -25:
						break;
					case 26:
						{ Symbol sym = new Symbol(TokenDef.ERROR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -26:
						break;
					case 27:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -27:
						break;
					case 28:
						{ Symbol sym = new Symbol(TokenDef.ERROR, new SimboloTexto(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -28:
						break;
					case 29:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -29:
						break;
					case 30:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -30:
						break;
					case 31:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -31:
						break;
					case 32:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -32:
						break;
					case 33:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -33:
						break;
					case 34:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -34:
						break;
					case 35:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -35:
						break;
					case 36:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -36:
						break;
					case 37:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -37:
						break;
					case 38:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -38:
						break;
					case 39:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -39:
						break;
					case 40:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -40:
						break;
					case 41:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -41:
						break;
					case 42:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -42:
						break;
					case 43:
						{ Symbol sym = new Symbol(TokenDef.IDENTIFICADOR, TablaAbstracta.idTabla.agregarSimbolo(yytext(), getLinea(), getColumna())); updateColumn(); return sym;}
					case -43:
						break;
					default:
						yy_error(YY_E_INTERNAL,false);break;
					}
					yy_initial = true;
					yy_state = yy_state_dtrans[yy_lexical_state];
					yy_next_state = YY_NO_STATE;
					yy_last_accept_state = YY_NO_STATE;
					yy_mark_start();
					yy_this_accept = yy_acpt[yy_state];
					if (YY_NOT_ACCEPT != yy_this_accept) {
						yy_last_accept_state = yy_state;
						yy_mark_end();
					}
				}
			}
		}
	}
}
