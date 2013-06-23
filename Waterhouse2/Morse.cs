using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterhouse2
{
    public static class Morse
    {
        public const string _A = "._";
        public const string _B = "_...";
        public const string _C = "_._.";
        public const string _D = "_..";
        public const string _E = ".";
        public const string _F = ".._.";
        public const string _G = "__.";
        public const string _H = "....";
        public const string _I = "..";
        public const string _J = ".___";
        public const string _K = "_._.";
        public const string _L = "._..";
        public const string _M = "__";
        public const string _N = "_.";
        public const string _O = "___";
        public const string _P = ".__.";
        public const string _Q = "__._";
        public const string _R = "._.";
        public const string _S = "...";
        public const string _T = "_";
        public const string _U = ".._";
        public const string _V = "..._";
        public const string _W = ".__";
        public const string _X = "_.._";
        public const string _Y = "_.__";
        public const string _Z = "__..";
        public const string _1 = ".____";
        public const string _2 = "..___";
        public const string _3 = "...__";
        public const string _4 = "...._";
        public const string _5 = ".....";
        public const string _6 = "_....";
        public const string _7 = "__...";
        public const string _8 = "___..";
        public const string _9 = "____.";
        public const string _0 = "_____";
        public const string _NA = "!";


        public static string GetMorse(char c)
        {
            c = c.ToString().ToUpper().ToCharArray()[0];
            switch (c) {
	            case 'A':
		            return _A;
	            case 'B':
		            return _B;
	            case 'C':
		            return _C;
	            case 'D':
		            return _D;
	            case 'E':
		            return _E;
	            case 'F':
		            return _F;
	            case 'G':
		            return _G;
	            case 'H':
		            return _H;
	            case 'I':
		            return _I;
	            case 'J':
		            return _J;
	            case 'K':
		            return _K;
	            case 'L':
		            return _L;
	            case 'M':
		            return _M;
	            case 'N':
		            return _N;
	            case 'O':
		            return _O;
	            case 'P':
		            return _P;
	            case 'Q':
		            return _Q;
	            case 'R':
		            return _R;
	            case 'S':
		            return _S;
	            case 'T':
		            return _T;
	            case 'U':
		            return _U;
	            case 'V':
		            return _V;
	            case 'W':
		            return _W;
	            case 'X':
		            return _X;
	            case 'Y':
		            return _Y;
	            case 'Z':
		            return _Z;
	            case '1':
		            return _1;
	            case '2':
		            return _2;
	            case '3':
		            return _3;
	            case '4':
		            return _4;
	            case '5':
		            return _5;
	            case '6':
		            return _6;
	            case '7':
		            return _7;
	            case '8':
		            return _8;
	            case '9':
		            return _9;
	            case '0':
		            return _0;
	            default:
		            return _NA;
}
            
        }
    }
   
}
