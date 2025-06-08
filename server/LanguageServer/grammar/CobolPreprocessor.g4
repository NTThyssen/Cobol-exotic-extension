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

grammar CobolPreprocessor;
options {tokenVocab = CobolPreprocessorLexer;}
startRule
   : (compilerOptions | copyStatement | execCicsStatement | execSqlStatement | execSqlImsStatement | replaceOffStatement | replaceArea | ejectStatement | skipStatement | titleStatement | charDataLine | NEWLINE)* EOF
   ;

// compiler options

compilerOptions
   : (PROCESS | CBL) (COMMACHAR? compilerOption | compilerXOpts)+
   ;

compilerXOpts
   : XOPTS LPARENCHAR compilerOption (COMMACHAR? compilerOption)* RPARENCHAR
   ;

compilerOption
   : ADATA | ADV | APOST
   | (ARITH | AR) LPARENCHAR (EXTEND | E_CHAR | COMPAT | C_CHAR) RPARENCHAR
   | AWO
   | BLOCK0
   | (BUFSIZE | BUF) LPARENCHAR literal RPARENCHAR
   | CBLCARD
   | CICS (LPARENCHAR literal RPARENCHAR)?
   | COBOL2 | COBOL3
   | (CODEPAGE | CP) LPARENCHAR literal RPARENCHAR
   | (COMPILE | C_CHAR)
   | CPP | CPSM
   | (CURRENCY | CURR) LPARENCHAR literal RPARENCHAR
   | DATA LPARENCHAR literal RPARENCHAR
   | (DATEPROC | DP) (LPARENCHAR (FLAG | NOFLAG)? COMMACHAR? (TRIG | NOTRIG)? RPARENCHAR)?
   | DBCS
   | (DECK | D_CHAR)
   | DEBUG
   | (DIAGTRUNC | DTR)
   | DLL
   | (DUMP | DU)
   | (DYNAM | DYN)
   | EDF | EPILOG
   | EXIT
   | (EXPORTALL | EXP)
   | (FASTSRT | FSRT)
   | FEPI
   | (FLAG | F_CHAR) LPARENCHAR (E_CHAR | I_CHAR | S_CHAR | U_CHAR | W_CHAR) (COMMACHAR (E_CHAR | I_CHAR | S_CHAR | U_CHAR | W_CHAR))? RPARENCHAR
   | FLAGSTD LPARENCHAR (M_CHAR | I_CHAR | H_CHAR) (COMMACHAR (D_CHAR | DD | N_CHAR | NN | S_CHAR | SS))? RPARENCHAR
   | GDS | GRAPHIC
   | INTDATE LPARENCHAR (ANSI | LILIAN) RPARENCHAR
   | (LANGUAGE | LANG) LPARENCHAR (ENGLISH | CS | EN | JA | JP | KA | UE) RPARENCHAR
   | LEASM | LENGTH | LIB | LIN
   | (LINECOUNT | LC) LPARENCHAR literal RPARENCHAR
   | LINKAGE | LIST
   | MAP
   | MARGINS LPARENCHAR literal COMMACHAR literal (COMMACHAR literal)? RPARENCHAR
   | (MDECK | MD) (LPARENCHAR (C_CHAR | COMPILE | NOC | NOCOMPILE) RPARENCHAR)?
   | NAME (LPARENCHAR (ALIAS | NOALIAS) RPARENCHAR)?
   | NATLANG LPARENCHAR (CS | EN | KA) RPARENCHAR
   | NOADATA | NOADV | NOAWO
   | NOBLOCK0
   | NOCBLCARD | NOCICS | NOCMPR2
   | (NOCOMPILE | NOC) (LPARENCHAR (S_CHAR | E_CHAR | W_CHAR) RPARENCHAR)?
   | NOCPSM
   | (NOCURRENCY | NOCURR)
   | (NODATEPROC | NODP)
   | NODBCS | NODEBUG
   | (NODECK | NOD)
   | NODLL | NODE
   | (NODUMP | NODU)
   | (NODIAGTRUNC | NODTR)
   | (NODYNAM | NODYN)
   | NOEDF | NOEPILOG | NOEXIT
   | (NOEXPORTALL | NOEXP)
   | (NOFASTSRT | NOFSRT)
   | NOFEPI
   | (NOFLAG | NOF)
   | NOFLAGMIG | NOFLAGSTD
   | NOGRAPHIC
   | NOLENGTH | NOLIB | NOLINKAGE | NOLIST
   | NOMAP
   | (NOMDECK | NOMD)
   | NONAME
   | (NONUMBER | NONUM)
   | (NOOBJECT | NOOBJ)
   | (NOOFFSET | NOOFF)
   | NOOPSEQUENCE
   | (NOOPTIMIZE | NOOPT)
   | NOOPTIONS
   | NOP | NOPROLOG
   | NORENT
   | (NOSEQUENCE | NOSEQ)
   | (NOSOURCE | NOS)
   | NOSPIE | NOSQL
   | (NOSQLCCSID | NOSQLC)
   | (NOSSRANGE | NOSSR)
   | NOSTDTRUNC
   | (NOTERMINAL | NOTERM) | NOTEST | NOTHREAD
   | NOVBREF
   | (NOWORD | NOWD)
   | NSEQ
   | (NSYMBOL | NS) LPARENCHAR (NATIONAL | NAT | DBCS) RPARENCHAR
   | NOVBREF
   | (NOXREF | NOX)
   | NOZWB
   | (NUMBER | NUM)
   | NUMPROC LPARENCHAR (MIG | NOPFD | PFD) RPARENCHAR
   | (OBJECT | OBJ)
   | (OFFSET | OFF)
   | OPMARGINS LPARENCHAR literal COMMACHAR literal (COMMACHAR literal)? RPARENCHAR
   | OPSEQUENCE LPARENCHAR literal COMMACHAR literal RPARENCHAR
   | (OPTIMIZE | OPT) (LPARENCHAR (FULL | STD) RPARENCHAR)?
   | OPTFILE | OPTIONS | OP
   | (OUTDD | OUT) LPARENCHAR cobolWord RPARENCHAR
   | (PGMNAME | PGMN) LPARENCHAR (CO | COMPAT | LM | LONGMIXED | LONGUPPER | LU | M_CHAR | MIXED | U_CHAR | UPPER) RPARENCHAR
   | PROLOG
   | (QUOTE | Q_CHAR)
   | RENT
   | RMODE LPARENCHAR (ANY | AUTO | literal) RPARENCHAR
   | (SEQUENCE | SEQ) (LPARENCHAR literal COMMACHAR literal RPARENCHAR)?
   | (SIZE | SZ) LPARENCHAR (MAX | literal) RPARENCHAR
   | (SOURCE | S_CHAR)
   | SP
   | SPACE LPARENCHAR literal RPARENCHAR
   | SPIE
   | SQL (LPARENCHAR literal RPARENCHAR)?
   | (SQLCCSID | SQLC)
   | (SSRANGE | SSR)
   | SYSEIB
   | (TERMINAL | TERM)
   | TEST (LPARENCHAR (HOOK | NOHOOK)? COMMACHAR? (SEP | SEPARATE | NOSEP | NOSEPARATE)? COMMACHAR? (EJPD | NOEJPD)? RPARENCHAR)?
   | THREAD
   | TRUNC LPARENCHAR (BIN | OPT | STD) RPARENCHAR
   | VBREF
   | (WORD | WD) LPARENCHAR cobolWord RPARENCHAR
   | (XMLPARSE | XP) LPARENCHAR (COMPAT | C_CHAR | XMLSS | X_CHAR) RPARENCHAR
   | (XREF | X_CHAR) (LPARENCHAR (FULL | SHORT)? RPARENCHAR)?
   | (YEARWINDOW | YW) LPARENCHAR literal RPARENCHAR
   | ZWB
   ;

// exec cics statement

execCicsStatement
   : EXEC CICS charData END_EXEC DOT?
   ;

// exec sql statement

execSqlStatement
   : EXEC SQL charDataSql END_EXEC DOT?
   ;

// exec sql ims statement

execSqlImsStatement
   : EXEC SQLIMS charData END_EXEC DOT?
   ;

// copy statement

copyStatement
   : COPY copySource (NEWLINE* (directoryPhrase | familyPhrase | replacingPhrase | SUPPRESS))* NEWLINE* DOT
   ;

copySource
   : (literal | cobolWord | filename) ((OF | IN) copyLibrary)?
   ;

copyLibrary
   : literal | cobolWord
   ;

replacingPhrase
   : REPLACING NEWLINE* replaceClause (NEWLINE+ replaceClause)*
   ;

// replace statement

replaceArea
   : replaceByStatement (copyStatement | charData)* replaceOffStatement?
   ;

replaceByStatement
   : REPLACE (NEWLINE* replaceClause)+ DOT
   ;

replaceOffStatement
   : REPLACE OFF DOT
   ;

replaceClause
   : replaceable NEWLINE* BY NEWLINE* replacement (NEWLINE* directoryPhrase)? (NEWLINE* familyPhrase)?
   ;

directoryPhrase
   : (OF | IN) NEWLINE* (literal | cobolWord)
   ;

familyPhrase
   : ON NEWLINE* (literal | cobolWord)
   ;

replaceable
   : literal | cobolWord | pseudoText | charDataLine
   ;

replacement
   : literal | cobolWord | pseudoText | charDataLine
   ;

// eject statement

ejectStatement
   : EJECT DOT?
   ;

// skip statement

skipStatement
   : (SKIP1 | SKIP2 | SKIP3) DOT?
   ;

// title statement

titleStatement
   : TITLE literal DOT?
   ;

// literal ----------------------------------

pseudoText
   : DOUBLEEQUALCHAR charData? DOUBLEEQUALCHAR
   ;

charData
   : (charDataLine | NEWLINE)+
   ;

charDataSql
   : (charDataLine | COPY | REPLACE | NEWLINE)+
   ;

charDataLine
   : (cobolWord | literal | filename | TEXT | DOT | LPARENCHAR | RPARENCHAR)+
   ;

cobolWord
   : IDENTIFIER | charDataKeyword
   ;

literal
   : NONNUMERICLITERAL | NUMERICLITERAL
   ;

filename
   : FILENAME
   ;

// keywords ----------------------------------

charDataKeyword
   : ADATA | ADV | ALIAS | ANSI | ANY | APOST | AR | ARITH | AUTO | AWO
   | BIN | BLOCK0 | BUF | BUFSIZE | BY
   | CBL | CBLCARD | CO | COBOL2 | COBOL3 | CODEPAGE | COMMACHAR | COMPAT | COMPILE | CP | CPP | CPSM | CS | CURR | CURRENCY
   | DATA | DATEPROC | DBCS | DD | DEBUG | DECK | DIAGTRUNC | DLI | DLL | DP | DTR | DU | DUMP | DYN | DYNAM
   | EDF | EJECT | EJPD | EN | ENGLISH | EPILOG | EXCI | EXIT | EXP | EXPORTALL | EXTEND
   | FASTSRT | FLAG | FLAGSTD | FULL | FSRT
   | GDS | GRAPHIC
   | HOOK
   | IN | INTDATE
   | JA | JP
   | KA
   | LANG | LANGUAGE | LC | LENGTH | LIB | LILIAN | LIN | LINECOUNT | LINKAGE | LIST | LM | LONGMIXED | LONGUPPER | LU
   | MAP | MARGINS | MAX | MD | MDECK | MIG | MIXED
   | NAME | NAT | NATIONAL | NATLANG
   | NN
   | NO
   | NOADATA | NOADV | NOALIAS | NOAWO
   | NOBLOCK0
   | NOC | NOCBLCARD | NOCICS | NOCMPR2 | NOCOMPILE | NOCPSM | NOCURR | NOCURRENCY
   | NOD | NODATEPROC | NODBCS | NODE | NODEBUG | NODECK | NODIAGTRUNC | NODLL | NODU | NODUMP | NODP | NODTR | NODYN | NODYNAM
   | NOEDF | NOEJPD | NOEPILOG | NOEXIT | NOEXP | NOEXPORTALL
   | NOF | NOFASTSRT | NOFEPI | NOFLAG | NOFLAGMIG | NOFLAGSTD | NOFSRT
   | NOGRAPHIC
   | NOHOOK
   | NOLENGTH | NOLIB | NOLINKAGE | NOLIST
   | NOMAP | NOMD | NOMDECK
   | NONAME | NONUM | NONUMBER
   | NOOBJ | NOOBJECT | NOOFF | NOOFFSET | NOOPSEQUENCE | NOOPT | NOOPTIMIZE | NOOPTIONS
   | NOP | NOPFD | NOPROLOG
   | NORENT
   | NOS | NOSEP | NOSEPARATE | NOSEQ | NOSEQUENCE | NOSOURCE | NOSPIE | NOSQL | NOSQLC | NOSQLCCSID | NOSSR | NOSSRANGE | NOSTDTRUNC
   | NOTERM | NOTERMINAL | NOTEST | NOTHREAD | NOTRIG
   | NOVBREF
   | NOWORD
   | NOX | NOXREF
   | NOZWB
   | NSEQ | NSYMBOL | NS
   | NUM | NUMBER | NUMPROC
   | OBJ | OBJECT | ON | OF | OFF | OFFSET | OPMARGINS | OPSEQUENCE | OPTIMIZE | OP | OPT | OPTFILE | OPTIONS | OUT | OUTDD
   | PFD | PGMN | PGMNAME | PPTDBG | PROCESS | PROLOG
   | QUOTE
   | RENT | REPLACING | RMODE
   | SEQ | SEQUENCE | SEP | SEPARATE | SHORT | SIZE | SOURCE | SP | SPACE | SPIE | SQL | SQLC | SQLCCSID | SS | SSR | SSRANGE | STD | SYSEIB | SZ
   | TERM | TERMINAL | TEST | THREAD | TITLE | TRIG | TRUNC
   | UE | UPPER
   | VBREF
   | WD
   | XMLPARSE | XMLSS | XOPTS | XREF
   | YEARWINDOW | YW
   | ZWB
   | C_CHAR | D_CHAR | E_CHAR | F_CHAR | H_CHAR | I_CHAR | M_CHAR | N_CHAR | Q_CHAR | S_CHAR | U_CHAR | W_CHAR | X_CHAR
   ;