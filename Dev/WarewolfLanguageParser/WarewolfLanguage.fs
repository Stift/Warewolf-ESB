// Implementation file for parser generated by fsyacc
module Parser
#nowarn "64";; // turn off warnings that type variables used in production annotations are instantiated to concrete type
open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing.ParseHelpers
# 1 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"

    open LanguageAST
    open DataASTMutable

# 11 "WarewolfLanguage.fs"
// This type is the type of tokens accepted by the parser
type token = 
  | EOF
  | VARNAME of (string)
  | STRING of (string)
  | FLOAT of (string)
  | INT of (string)
  | OPENLANGUAGE
  | CLOSELANGAUGE
  | OPENBRACKET
  | CLOSEDBRACKET
  | STAR
  | DOT
// This type is used to give symbolic names to token indexes, useful for error messages
type tokenId = 
    | TOKEN_EOF
    | TOKEN_VARNAME
    | TOKEN_STRING
    | TOKEN_FLOAT
    | TOKEN_INT
    | TOKEN_OPENLANGUAGE
    | TOKEN_CLOSELANGAUGE
    | TOKEN_OPENBRACKET
    | TOKEN_CLOSEDBRACKET
    | TOKEN_STAR
    | TOKEN_DOT
    | TOKEN_end_of_input
    | TOKEN_error
// This type is used to give symbolic names to token indexes, useful for error messages
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_langExpression
    | NONTERM_variableExpression
    | NONTERM_recset
    | NONTERM_recsetName
    | NONTERM_index
    | NONTERM_intindex
    | NONTERM_scalar
    | NONTERM_atom

// This function maps tokens to integers indexes
let tagOfToken (t:token) = 
  match t with
  | EOF  -> 0 
  | VARNAME _ -> 1 
  | STRING _ -> 2 
  | FLOAT _ -> 3 
  | INT _ -> 4 
  | OPENLANGUAGE  -> 5 
  | CLOSELANGAUGE  -> 6 
  | OPENBRACKET  -> 7 
  | CLOSEDBRACKET  -> 8 
  | STAR  -> 9 
  | DOT  -> 10 

// This function maps integers indexes to symbolic token ids
let tokenTagToTokenId (tokenIdx:int) = 
  match tokenIdx with
  | 0 -> TOKEN_EOF 
  | 1 -> TOKEN_VARNAME 
  | 2 -> TOKEN_STRING 
  | 3 -> TOKEN_FLOAT 
  | 4 -> TOKEN_INT 
  | 5 -> TOKEN_OPENLANGUAGE 
  | 6 -> TOKEN_CLOSELANGAUGE 
  | 7 -> TOKEN_OPENBRACKET 
  | 8 -> TOKEN_CLOSEDBRACKET 
  | 9 -> TOKEN_STAR 
  | 10 -> TOKEN_DOT 
  | 13 -> TOKEN_end_of_input
  | 11 -> TOKEN_error
  | _ -> failwith "tokenTagToTokenId: bad token"

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
let prodIdxToNonTerminal (prodIdx:int) = 
  match prodIdx with
    | 0 -> NONTERM__startstart 
    | 1 -> NONTERM_start 
    | 2 -> NONTERM_langExpression 
    | 3 -> NONTERM_langExpression 
    | 4 -> NONTERM_langExpression 
    | 5 -> NONTERM_langExpression 
    | 6 -> NONTERM_langExpression 
    | 7 -> NONTERM_langExpression 
    | 8 -> NONTERM_langExpression 
    | 9 -> NONTERM_langExpression 
    | 10 -> NONTERM_langExpression 
    | 11 -> NONTERM_langExpression 
    | 12 -> NONTERM_langExpression 
    | 13 -> NONTERM_variableExpression 
    | 14 -> NONTERM_variableExpression 
    | 15 -> NONTERM_variableExpression 
    | 16 -> NONTERM_variableExpression 
    | 17 -> NONTERM_variableExpression 
    | 18 -> NONTERM_recset 
    | 19 -> NONTERM_recset 
    | 20 -> NONTERM_recset 
    | 21 -> NONTERM_recset 
    | 22 -> NONTERM_recsetName 
    | 23 -> NONTERM_recsetName 
    | 24 -> NONTERM_recsetName 
    | 25 -> NONTERM_recsetName 
    | 26 -> NONTERM_index 
    | 27 -> NONTERM_intindex 
    | 28 -> NONTERM_scalar 
    | 29 -> NONTERM_atom 
    | 30 -> NONTERM_atom 
    | 31 -> NONTERM_atom 
    | 32 -> NONTERM_atom 
    | _ -> failwith "prodIdxToNonTerminal: bad production index"

let _fsyacc_endOfInputTag = 13 
let _fsyacc_tagOfErrorTerminal = 11

// This function gets the name of a token as a string
let token_to_string (t:token) = 
  match t with 
  | EOF  -> "EOF" 
  | VARNAME _ -> "VARNAME" 
  | STRING _ -> "STRING" 
  | FLOAT _ -> "FLOAT" 
  | INT _ -> "INT" 
  | OPENLANGUAGE  -> "OPENLANGUAGE" 
  | CLOSELANGAUGE  -> "CLOSELANGAUGE" 
  | OPENBRACKET  -> "OPENBRACKET" 
  | CLOSEDBRACKET  -> "CLOSEDBRACKET" 
  | STAR  -> "STAR" 
  | DOT  -> "DOT" 

// This function gets the data carried by a token as an object
let _fsyacc_dataOfToken (t:token) = 
  match t with 
  | EOF  -> (null : System.Object) 
  | VARNAME _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | STRING _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | FLOAT _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | INT _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | OPENLANGUAGE  -> (null : System.Object) 
  | CLOSELANGAUGE  -> (null : System.Object) 
  | OPENBRACKET  -> (null : System.Object) 
  | CLOSEDBRACKET  -> (null : System.Object) 
  | STAR  -> (null : System.Object) 
  | DOT  -> (null : System.Object) 
let _fsyacc_gotos = [| 0us; 65535us; 1us; 65535us; 0us; 1us; 5us; 65535us; 0us; 2us; 2us; 18us; 18us; 18us; 19us; 18us; 22us; 19us; 5us; 65535us; 0us; 7us; 2us; 8us; 18us; 8us; 19us; 8us; 22us; 7us; 5us; 65535us; 0us; 5us; 2us; 5us; 18us; 5us; 19us; 5us; 22us; 5us; 5us; 65535us; 0us; 6us; 2us; 6us; 18us; 6us; 19us; 6us; 22us; 6us; 1us; 65535us; 22us; 23us; 0us; 65535us; 5us; 65535us; 0us; 4us; 2us; 4us; 18us; 4us; 19us; 4us; 22us; 4us; 5us; 65535us; 0us; 3us; 2us; 3us; 18us; 3us; 19us; 3us; 22us; 3us; |]
let _fsyacc_sparseGotoTableRowOffsets = [|0us; 1us; 3us; 9us; 15us; 21us; 27us; 29us; 30us; 36us; |]
let _fsyacc_stateToProdIdxsTableElements = [| 1us; 0us; 1us; 0us; 2us; 1us; 13us; 2us; 2us; 17us; 2us; 3us; 14us; 2us; 4us; 15us; 1us; 5us; 1us; 6us; 2us; 6us; 13us; 10us; 7us; 18us; 19us; 20us; 21us; 22us; 23us; 24us; 25us; 28us; 1us; 8us; 1us; 9us; 1us; 10us; 3us; 10us; 20us; 22us; 3us; 10us; 21us; 25us; 1us; 11us; 3us; 11us; 19us; 24us; 1us; 12us; 1us; 13us; 3us; 13us; 21us; 25us; 1us; 16us; 9us; 18us; 19us; 20us; 21us; 22us; 23us; 24us; 25us; 28us; 8us; 18us; 19us; 20us; 21us; 22us; 23us; 24us; 25us; 2us; 18us; 23us; 2us; 18us; 23us; 1us; 18us; 1us; 18us; 1us; 18us; 2us; 19us; 24us; 1us; 19us; 1us; 19us; 1us; 19us; 1us; 20us; 1us; 20us; 1us; 20us; 1us; 21us; 1us; 21us; 1us; 21us; 1us; 22us; 1us; 23us; 1us; 24us; 1us; 25us; 2us; 26us; 30us; 1us; 28us; 1us; 29us; 1us; 30us; 1us; 31us; 1us; 32us; |]
let _fsyacc_stateToProdIdxsTableRowOffsets = [|0us; 2us; 4us; 7us; 10us; 13us; 16us; 18us; 20us; 23us; 34us; 36us; 38us; 40us; 44us; 48us; 50us; 54us; 56us; 58us; 62us; 64us; 74us; 83us; 86us; 89us; 91us; 93us; 95us; 98us; 100us; 102us; 104us; 106us; 108us; 110us; 112us; 114us; 116us; 118us; 120us; 122us; 124us; 127us; 129us; 131us; 133us; 135us; |]
let _fsyacc_action_rows = 48
let _fsyacc_actionTableElements = [|11us; 32768us; 0us; 20us; 1us; 46us; 2us; 47us; 3us; 44us; 4us; 45us; 5us; 9us; 6us; 10us; 7us; 11us; 8us; 12us; 9us; 15us; 10us; 17us; 0us; 49152us; 11us; 16385us; 0us; 20us; 1us; 46us; 2us; 47us; 3us; 44us; 4us; 45us; 5us; 9us; 6us; 10us; 7us; 11us; 8us; 12us; 9us; 15us; 10us; 17us; 0us; 16386us; 0us; 16387us; 0us; 16388us; 0us; 16389us; 0us; 16390us; 1us; 16390us; 13us; 16397us; 1us; 16391us; 1us; 21us; 0us; 16392us; 0us; 16393us; 0us; 16394us; 2us; 16394us; 6us; 38us; 10us; 32us; 2us; 16394us; 6us; 41us; 10us; 35us; 0us; 16395us; 1us; 16395us; 8us; 28us; 0us; 16396us; 11us; 32768us; 0us; 20us; 1us; 46us; 2us; 47us; 3us; 44us; 4us; 45us; 5us; 9us; 6us; 10us; 7us; 11us; 8us; 12us; 9us; 15us; 10us; 17us; 11us; 32768us; 0us; 20us; 1us; 46us; 2us; 47us; 3us; 44us; 4us; 45us; 5us; 9us; 6us; 10us; 7us; 11us; 8us; 14us; 9us; 15us; 10us; 17us; 0us; 16400us; 2us; 32768us; 6us; 43us; 7us; 22us; 11us; 32768us; 0us; 20us; 1us; 46us; 2us; 47us; 3us; 44us; 4us; 42us; 5us; 9us; 6us; 10us; 7us; 11us; 8us; 13us; 9us; 16us; 10us; 17us; 1us; 32768us; 8us; 24us; 2us; 32768us; 6us; 39us; 10us; 25us; 1us; 32768us; 1us; 26us; 1us; 32768us; 6us; 27us; 0us; 16402us; 2us; 32768us; 6us; 40us; 10us; 29us; 1us; 32768us; 1us; 30us; 1us; 32768us; 6us; 31us; 0us; 16403us; 1us; 32768us; 1us; 33us; 1us; 32768us; 6us; 34us; 0us; 16404us; 1us; 32768us; 1us; 36us; 1us; 32768us; 6us; 37us; 0us; 16405us; 0us; 16406us; 0us; 16407us; 0us; 16408us; 0us; 16409us; 4us; 16414us; 8us; 16410us; 11us; 16410us; 12us; 16410us; 13us; 16410us; 0us; 16412us; 0us; 16413us; 0us; 16414us; 0us; 16415us; 0us; 16416us; |]
let _fsyacc_actionTableRowOffsets = [|0us; 12us; 13us; 25us; 26us; 27us; 28us; 29us; 30us; 32us; 34us; 35us; 36us; 37us; 40us; 43us; 44us; 46us; 47us; 59us; 71us; 72us; 75us; 87us; 89us; 92us; 94us; 96us; 97us; 100us; 102us; 104us; 105us; 107us; 109us; 110us; 112us; 114us; 115us; 116us; 117us; 118us; 119us; 124us; 125us; 126us; 127us; 128us; |]
let _fsyacc_reductionSymbolCounts = [|1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 1us; 2us; 1us; 1us; 1us; 1us; 8us; 8us; 7us; 8us; 5us; 6us; 6us; 6us; 1us; 1us; 3us; 1us; 1us; 1us; 1us; |]
let _fsyacc_productionToNonTerminalTable = [|0us; 1us; 2us; 2us; 2us; 2us; 2us; 2us; 2us; 2us; 2us; 2us; 2us; 3us; 3us; 3us; 3us; 3us; 4us; 4us; 4us; 4us; 5us; 5us; 5us; 5us; 6us; 7us; 8us; 9us; 9us; 9us; 9us; |]
let _fsyacc_immediateActions = [|65535us; 49152us; 65535us; 65535us; 65535us; 65535us; 16389us; 16390us; 65535us; 65535us; 16392us; 16393us; 16394us; 65535us; 65535us; 16395us; 65535us; 16396us; 65535us; 65535us; 16400us; 65535us; 65535us; 65535us; 65535us; 65535us; 65535us; 16402us; 65535us; 65535us; 65535us; 16403us; 65535us; 65535us; 16404us; 65535us; 65535us; 16405us; 16406us; 16407us; 16408us; 16409us; 65535us; 16412us; 16413us; 16414us; 16415us; 16416us; |]
let _fsyacc_reductions ()  =    [| 
# 167 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.LanguageExpression)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
                      raise (Microsoft.FSharp.Text.Parsing.Accept(Microsoft.FSharp.Core.Operators.box _1))
                   )
                 : '_startstart));
# 176 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.LanguageExpression)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 31 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                            _1
                   )
# 31 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 187 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : DataASTMutable.WarewolfAtom)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 34 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                              WarewolfAtomAtomExpression _1
                   )
# 34 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 198 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.ScalarIdentifier)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 35 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                  ScalarExpression _1 
                   )
# 35 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 209 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.RecordSetIdentifier)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 36 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                 RecordSetExpression _1 
                   )
# 36 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 220 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : 'recsetName)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 37 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                     RecordSetNameExpression _1 
                   )
# 37 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 231 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : 'variableExpression)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 38 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                             ComplexExpression _1 
                   )
# 38 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 242 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 39 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                      WarewolfAtomAtomExpression (DataString "[[")
                   )
# 39 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 252 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 40 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                       WarewolfAtomAtomExpression (DataString "]]")
                   )
# 40 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 262 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 41 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                     WarewolfAtomAtomExpression (DataString "(")
                   )
# 41 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 272 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 42 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                       WarewolfAtomAtomExpression (DataString ")")
                   )
# 42 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 282 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 43 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                              WarewolfAtomAtomExpression (DataString "*")
                   )
# 43 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 292 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 44 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                             WarewolfAtomAtomExpression (DataString ".")
                   )
# 44 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.LanguageExpression));
# 302 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.LanguageExpression)) in
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : 'variableExpression)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 46 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                 _1::_2
                   )
# 46 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'variableExpression));
# 314 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.ScalarIdentifier)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 47 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                  [ScalarExpression _1] 
                   )
# 47 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'variableExpression));
# 325 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.RecordSetIdentifier)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 48 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                 [RecordSetExpression _1] 
                   )
# 48 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'variableExpression));
# 336 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 49 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                             []
                   )
# 49 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'variableExpression));
# 346 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : DataASTMutable.WarewolfAtom)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 50 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                              [WarewolfAtomAtomExpression _1]
                   )
# 50 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'variableExpression));
# 357 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _4 = (let data = parseState.GetInput(4) in (Microsoft.FSharp.Core.Operators.unbox data : 'index)) in
            let _7 = (let data = parseState.GetInput(7) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 52 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                                          {Name = _2; Column = _7; Index =_4 ;}
                   )
# 52 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.RecordSetIdentifier));
# 370 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _7 = (let data = parseState.GetInput(7) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 53 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                                      {Name = _2; Column = _7; Index =Star ;}
                   )
# 53 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.RecordSetIdentifier));
# 382 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _6 = (let data = parseState.GetInput(6) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 54 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                                    {Name = _2; Column = _6; Index =Last ;}
                   )
# 54 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.RecordSetIdentifier));
# 394 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _4 = (let data = parseState.GetInput(4) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.LanguageExpression)) in
            let _7 = (let data = parseState.GetInput(7) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 55 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                                                {Name = _2; Column = _7; Index = IndexExpression _4 ;}
                   )
# 55 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.RecordSetIdentifier));
# 407 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 57 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                       {Name = _2;Index = Last;}
                   )
# 57 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'recsetName));
# 418 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _4 = (let data = parseState.GetInput(4) in (Microsoft.FSharp.Core.Operators.unbox data : 'index)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 58 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                          {Name = _2; Index = _4;}
                   )
# 58 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'recsetName));
# 430 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 59 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                         {Name = _2; Index = Star;}
                   )
# 59 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'recsetName));
# 441 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            let _4 = (let data = parseState.GetInput(4) in (Microsoft.FSharp.Core.Operators.unbox data : LanguageAST.LanguageExpression)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 60 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                                                                   {Name = _2;Index = IndexExpression _4;}
                   )
# 60 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'recsetName));
# 453 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 62 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                             IntIndex ( System.Int32.Parse (  _1))
                   )
# 62 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'index));
# 464 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 64 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                             IndexExpression ( WarewolfAtomAtomExpression ( tryParseAtom _1))
                   )
# 64 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : 'intindex));
# 475 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _2 = (let data = parseState.GetInput(2) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 66 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                                           _2
                   )
# 66 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : LanguageAST.ScalarIdentifier));
# 486 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 68 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                  tryFloatParseAtom _1 
                   )
# 68 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : DataASTMutable.WarewolfAtom));
# 497 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 69 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                tryParseAtom _1 
                   )
# 69 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : DataASTMutable.WarewolfAtom));
# 508 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 70 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                   DataString _1 
                   )
# 70 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : DataASTMutable.WarewolfAtom));
# 519 "WarewolfLanguage.fs"
        (fun (parseState : Microsoft.FSharp.Text.Parsing.IParseState) ->
            let _1 = (let data = parseState.GetInput(1) in (Microsoft.FSharp.Core.Operators.unbox data : string)) in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 71 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                                  DataString _1 
                   )
# 71 "..\WarewolfLanguageParser\WarewolfLanguage.fsy"
                 : DataASTMutable.WarewolfAtom));
|]
# 531 "WarewolfLanguage.fs"
let tables () : Microsoft.FSharp.Text.Parsing.Tables<_> = 
  { reductions= _fsyacc_reductions ();
    endOfInputTag = _fsyacc_endOfInputTag;
    tagOfToken = tagOfToken;
    dataOfToken = _fsyacc_dataOfToken; 
    actionTableElements = _fsyacc_actionTableElements;
    actionTableRowOffsets = _fsyacc_actionTableRowOffsets;
    stateToProdIdxsTableElements = _fsyacc_stateToProdIdxsTableElements;
    stateToProdIdxsTableRowOffsets = _fsyacc_stateToProdIdxsTableRowOffsets;
    reductionSymbolCounts = _fsyacc_reductionSymbolCounts;
    immediateActions = _fsyacc_immediateActions;
    gotos = _fsyacc_gotos;
    sparseGotoTableRowOffsets = _fsyacc_sparseGotoTableRowOffsets;
    tagOfErrorTerminal = _fsyacc_tagOfErrorTerminal;
    parseError = (fun (ctxt:Microsoft.FSharp.Text.Parsing.ParseErrorContext<_>) -> 
                              match parse_error_rich with 
                              | Some f -> f ctxt
                              | None -> parse_error ctxt.Message);
    numTerminals = 14;
    productionToNonTerminalTable = _fsyacc_productionToNonTerminalTable  }
let engine lexer lexbuf startState = (tables ()).Interpret(lexer, lexbuf, startState)
let start lexer lexbuf : LanguageAST.LanguageExpression =
    Microsoft.FSharp.Core.Operators.unbox ((tables ()).Interpret(lexer, lexbuf, 0))
