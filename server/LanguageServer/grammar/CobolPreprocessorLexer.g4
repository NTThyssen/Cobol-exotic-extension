/*
* Copyright (C) 2017, Ulrich Wolffgang <ulrich.wolffgang@proleap.io>
* All rights reserved.
*
* This software may be modified and distributed under the terms
* of the MIT license. See the LICENSE file for details.
*/

/*
* COBOL Preprocessor Grammar for ANTLR4
*
* This is a preprocessor grammar for COBOL, which is part of the COBOL
* parser at https://github.com/uwol/proleap-cobol-parser.
*/

// lexer rules ---------------------------------------------------------------xz-----------------
lexer grammar CobolPreprocessorLexer;
channels{COMMENTS}
// keywords
ADATA : A D A T A;
ADV : A D V;
ALIAS : A L I A S;
ANSI : A N S I;
ANY : A N Y;
APOST : A P O S T;
AR : A R;
ARITH : A R I T H;
AUTO : A U T O;
AWO : A W O;
BIN : B I N;
BLOCK0 : B L O C K '0';
BUF : B U F;
BUFSIZE : B U F S I Z E;
BY : B Y;
CBL : C B L;
CBLCARD : C B L C A R D;
CICS : C I C S;
CO : C O;
COBOL2 : C O B O L '2';
COBOL3 : C O B O L '3';
CODEPAGE : C O D E P A G E;
COMPAT : C O M P A T;
COMPILE : C O M P I L E;
COPY : C O P Y;
CP : C P;
CPP : C P P;
CPSM : C P S M;
CS : C S;
CURR : C U R R;
CURRENCY : C U R R E N C Y;
DATA : D A T A;
DATEPROC : D A T E P R O C;
DBCS : D B C S;
DD : D D;
DEBUG : D E B U G;
DECK : D E C K;
DIAGTRUNC : D I A G T R U N C;
DLI : D L I;
DLL : D L L;
DP : D P;
DTR : D T R;
DU : D U;
DUMP : D U M P;
DYN : D Y N;
DYNAM : D Y N A M;
EDF : E D F;
EJECT : E J E C T;
EJPD : E J P D;
EN : E N;
ENGLISH : E N G L I S H;
END_EXEC : E N D '-' E X E C;
EPILOG : E P I L O G;
EXCI : E X C I;
EXEC : E X E C;
EXIT : E X I T;
EXP : E X P;
EXPORTALL : E X P O R T A L L;
EXTEND : E X T E N D;
FASTSRT : F A S T S R T;
FEPI : F E P I;
FLAG : F L A G;
FLAGSTD : F L A G S T D;
FSRT : F S R T;
FULL : F U L L;
GDS : G D S;
GRAPHIC : G R A P H I C;
HOOK : H O O K;
IN : I N;
INTDATE : I N T D A T E;
JA : J A;
JP : J P;
KA : K A;
LANG : L A N G;
LANGUAGE : L A N G U A G E;
LC : L C;
LEASM : L E A S M;
LENGTH : L E N G T H;
LIB : L I B;
LILIAN : L I L I A N;
LIN : L I N;
LINECOUNT : L I N E C O U N T;
LINKAGE : L I N K A G E;
LIST : L I S T;
LM : L M;
LONGMIXED : L O N G M I X E D;
LONGUPPER : L O N G U P P E R;
LPARENCHAR : '(';
LU : L U;
MAP : M A P;
MARGINS : M A R G I N S;
MAX : M A X;
MD : M D;
MDECK : M D E C K;
MIG : M I G;
MIXED : M I X E D;
NAME : N A M E;
NAT : N A T;
NATIONAL : N A T I O N A L;
NATLANG : N A T L A N G;
NN : N N;
NO : N O;
NOADATA : N O A D A T A;
NOADV : N O A D V;
NOALIAS : N O A L I A S;
NOAWO : N O A W O;
NOBLOCK0 : N O B L O C K '0';
NOC : N O C;
NOCBLCARD : N O C B L C A R D;
NOCICS : N O C I C S;
NOCMPR2 : N O C M P R '2';
NOCOMPILE : N O C O M P I L E;
NOCPSM : N O C P S M;
NOCURR : N O C U R R;
NOCURRENCY : N O C U R R E N C Y;
NOD : N O D;
NODATEPROC : N O D A T E P R O C;
NODBCS : N O D B C S;
NODE : N O D E;
NODEBUG : N O D E B U G;
NODECK : N O D E C K;
NODIAGTRUNC : N O D I A G T R U N C;
NODLL : N O D L L;
NODU : N O D U;
NODUMP : N O D U M P;
NODP : N O D P;
NODTR : N O D T R;
NODYN : N O D Y N;
NODYNAM : N O D Y N A M;
NOEDF : N O E D F;
NOEJPD : N O E J P D;
NOEPILOG : N O E P I L O G;
NOEXIT : N O E X I T;
NOEXP : N O E X P;
NOEXPORTALL : N O E X P O R T A L L;
NOF : N O F;
NOFASTSRT : N O F A S T S R T;
NOFEPI : N O F E P I;
NOFLAG : N O F L A G;
NOFLAGMIG : N O F L A G M I G;
NOFLAGSTD : N O F L A G S T D;
NOFSRT : N O F S R T;
NOGRAPHIC : N O G R A P H I C;
NOHOOK : N O H O O K;
NOLENGTH : N O L E N G T H;
NOLIB : N O L I B;
NOLINKAGE : N O L I N K A G E;
NOLIST : N O L I S T;
NOMAP : N O M A P;
NOMD : N O M D;
NOMDECK : N O M D E C K;
NONAME : N O N A M E;
NONUM : N O N U M;
NONUMBER : N O N U M B E R;
NOOBJ : N O O B J;
NOOBJECT : N O O B J E C T;
NOOFF : N O O F F;
NOOFFSET : N O O F F S E T;
NOOPSEQUENCE : N O O P S E Q U E N C E;
NOOPT : N O O P T;
NOOPTIMIZE : N O O P T I M I Z E;
NOOPTIONS : N O O P T I O N S;
NOP : N O P;
NOPFD : N O P F D;
NOPROLOG : N O P R O L O G;
NORENT : N O R E N T;
NOS : N O S;
NOSEP : N O S E P;
NOSEPARATE : N O S E P A R A T E;
NOSEQ : N O S E Q;
NOSOURCE : N O S O U R C E;
NOSPIE : N O S P I E;
NOSQL : N O S Q L;
NOSQLC : N O S Q L C;
NOSQLCCSID : N O S Q L C C S I D;
NOSSR : N O S S R;
NOSSRANGE : N O S S R A N G E;
NOSTDTRUNC : N O S T D T R U N C;
NOSEQUENCE : N O S E Q U E N C E;
NOTERM : N O T E R M;
NOTERMINAL : N O T E R M I N A L;
NOTEST : N O T E S T;
NOTHREAD : N O T H R E A D;
NOTRIG : N O T R I G;
NOVBREF : N O V B R E F;
NOWD : N O W D;
NOWORD : N O W O R D;
NOX : N O X;
NOXREF : N O X R E F;
NOZWB : N O Z W B;
NS : N S;
NSEQ : N S E Q;
NSYMBOL : N S Y M B O L;
NUM : N U M;
NUMBER : N U M B E R;
NUMPROC : N U M P R O C;
OBJ : O B J;
OBJECT : O B J E C T;
OF : O F;
OFF : O F F;
OFFSET : O F F S E T;
ON : O N;
OP : O P;
OPMARGINS : O P M A R G I N S;
OPSEQUENCE : O P S E Q U E N C E;
OPT : O P T;
OPTFILE : O P T F I L E;
OPTIMIZE : O P T I M I Z E;
OPTIONS : O P T I O N S;
OUT : O U T;
OUTDD : O U T D D;
PFD : P F D;
PPTDBG : P P T D B G;
PGMN : P G M N;
PGMNAME : P G M N A M E;
PROCESS : P R O C E S S;
PROLOG : P R O L O G;
QUOTE : Q U O T E;
RENT : R E N T;
REPLACE : R E P L A C E;
REPLACING : R E P L A C I N G;
RMODE : R M O D E;
RPARENCHAR : ')';
SEP : S E P;
SEPARATE : S E P A R A T E;
SEQ : S E Q;
SEQUENCE : S E Q U E N C E;
SHORT : S H O R T;
SIZE : S I Z E;
SOURCE : S O U R C E;
SP : S P;
SPACE : S P A C E;
SPIE : S P I E;
SQL : S Q L;
SQLC : S Q L C;
SQLCCSID : S Q L C C S I D;
SQLIMS : S Q L I M S;
SKIP1 : S K I P '1';
SKIP2 : S K I P '2';
SKIP3 : S K I P '3';
SS : S S;
SSR : S S R;
SSRANGE : S S R A N G E;
STD : S T D;
SUPPRESS : S U P P R E S S;
SYSEIB : S Y S E I B;
SZ : S Z;
TERM : T E R M;
TERMINAL : T E R M I N A L;
TEST : T E S T;
THREAD : T H R E A D;
TITLE : T I T L E;
TRIG : T R I G;
TRUNC : T R U N C;
UE : U E;
UPPER : U P P E R;
VBREF : V B R E F;
WD : W D;
WORD : W O R D;
XMLPARSE : X M L P A R S E;
XMLSS : X M L S S;
XOPTS: X O P T S;
XP : X P;
XREF : X R E F;
YEARWINDOW : Y E A R W I N D O W;
YW : Y W;
ZWB : Z W B;

C_CHAR : C;
D_CHAR : D;
E_CHAR : E;
F_CHAR : F;
H_CHAR : H;
I_CHAR : I;
M_CHAR : M;
N_CHAR : N;
Q_CHAR : Q;
S_CHAR : S;
U_CHAR : U;
W_CHAR : W;
X_CHAR : X;


// symbols
COMMENTTAG : '*>';
COMMACHAR : ',';
DOT : '.';
DOUBLEEQUALCHAR : '==';

// literals
NONNUMERICLITERAL : STRINGLITERAL | HEXNUMBER;
NUMERICLITERAL : [0-9]+;

fragment HEXNUMBER :
	X '"' [0-9A-F]+ '"'
	| X '\'' [0-9A-F]+ '\''
;

fragment STRINGLITERAL :
	'"' (~["\n\r] | '""' | '\'')* '"'
	| '\'' (~['\n\r] | '\'\'' | '"')* '\''
;

IDENTIFIER : [a-zA-Z0-9]+ ([-_]+ [a-zA-Z0-9]+)*;
FILENAME : [a-zA-Z0-9]+ '.' [a-zA-Z0-9]+;


// whitespace, line breaks, comments, ...
NEWLINE : '\r'? '\n';
COMMENTLINE : COMMENTTAG ~('\n' | '\r')* -> channel(HIDDEN);
WS : [ \t\f;]+ -> channel(HIDDEN);
TEXT : ~('\n' | '\r');


// case insensitive chars
fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');