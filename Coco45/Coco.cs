/*-------------------------------------------------------------------------
Compiler Generator Coco/R,
Copyright (c) 1990, 2004 Hanspeter Moessenboeck, University of Linz
extended by M. Loeberbauer & A. Woess, Univ. of Linz
with improvements by Pat Terry, Rhodes University

This program is free software; you can redistribute it and/or modify it 
under the terms of the GNU General Public License as published by the 
Free Software Foundation; either version 2, or (at your option) any 
later version.

This program is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
for more details.

You should have received a copy of the GNU General Public License along 
with this program; if not, write to the Free Software Foundation, Inc., 
59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.

As an exception, it is allowed to write an extension of Coco/R that is
used as a plugin in non-free software.

If not otherwise stated, any source code generated by Coco/R (other than 
Coco/R itself) does not fall under the GNU General Public License.
-------------------------------------------------------------------------*/
/*-------------------------------------------------------------------------
  Trace output options
  0 | A: prints the states of the scanner automaton
  1 | F: prints the First and Follow sets of all nonterminals
  2 | G: prints the syntax graph of the productions
  3 | I: traces the computation of the First sets
  4 | J: prints the sets associated with ANYs and synchronisation sets
  6 | S: prints the symbol table (terminals, nonterminals, pragmas)
  7 | X: prints a cross reference list of all syntax symbols
  8 | P: prints statistics about the Coco run
  
  Trace output can be switched on by the pragma
    $ { digit | letter }
  in the attributed grammar or as a command-line option
  -------------------------------------------------------------------------*/

using System;
using System.IO;

namespace at.jku.ssw.Coco {

public class Coco {
        
    public static int Main (string[] arg) {
        Console.WriteLine("Coco/R (Apr 19, 2011)");
        string srcName = null, nsName = null, frameDir = null, ddtString = null,
        traceFileName = null, outDir = null;
        bool emitLines = false;
        int retVal = 1;
        for (int i = 0; i < arg.Length; i++) {
            if (arg[i] == "-namespace" && i < arg.Length - 1) nsName = arg[++i].Trim();
            else if (arg[i] == "-frames" && i < arg.Length - 1) frameDir = arg[++i].Trim();
            else if (arg[i] == "-trace" && i < arg.Length - 1) ddtString = arg[++i].Trim();
            else if (arg[i] == "-o" && i < arg.Length - 1) outDir = arg[++i].Trim();
            else if (arg[i] == "-lines") emitLines = true;
            else srcName = arg[i];
        }
        if (arg.Length > 0 && srcName != null) {
            try {
                string srcDir = Path.GetDirectoryName(srcName);
                
                Scanner scanner = new Scanner(srcName);
                Parser parser = new Parser(scanner);

                traceFileName = Path.Combine(srcDir, "trace.txt");
                parser.trace = new StreamWriter(new FileStream(traceFileName, FileMode.Create));
                parser.tab = new Tab(parser);
                parser.dfa = new DFA(parser);
                parser.pgen = new ParserGen(parser);

                parser.tab.srcName = srcName;
                parser.tab.srcDir = srcDir;
                parser.tab.nsName = nsName;
                parser.tab.frameDir = frameDir;
                parser.tab.outDir = (outDir != null) ? outDir : srcDir;
                parser.tab.emitLines = emitLines;
                if (ddtString != null) parser.tab.SetDDT(ddtString);

                parser.Parse();

                parser.trace.Close();
                FileInfo f = new FileInfo(traceFileName);
                if (f.Length == 0) f.Delete();
                else Console.WriteLine("trace output is in " + traceFileName);
                Console.WriteLine("{0} errors detected", parser.errors.count);
                if (parser.errors.count == 0) { retVal = 0; }
            } catch (IOException) {
                Console.WriteLine("-- could not open " + traceFileName);
            } catch (FatalError e) {
                Console.WriteLine("-- " + e.Message);
            }
        } else {
            Console.WriteLine("Usage: Coco Grammar.ATG {{Option}}{0}"
                              + "Options:{0}"
                              + "  -namespace <namespaceName>{0}"
                              + "  -frames    <frameFilesDirectory>{0}"
                              + "  -trace     <traceString>{0}"
                              + "  -o         <outputDirectory>{0}"
                              + "  -lines{0}"
                              + "Valid characters in the trace string:{0}"
                              + "  A  trace automaton{0}"
                              + "  F  list first/follow sets{0}"
                              + "  G  print syntax graph{0}"
                              + "  I  trace computation of first sets{0}"
                              + "  J  list ANY and SYNC sets{0}"
                              + "  P  print statistics{0}"
                              + "  S  list symbol table{0}"
                              + "  X  list cross reference table{0}"
                              + "Scanner.frame and Parser.frame files needed in ATG directory{0}"
                              + "or in a directory specified in the -frames option.",
                              Environment.NewLine);
        }
        return retVal;
    }
        
} // end Coco

} // end namespace