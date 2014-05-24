// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under MIT X11 license (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.NRefactory.VB
{
    /// <summary>
    /// Description of VBFormattingOptions.
    /// </summary>
    public class VBFormattingOptions
    {
        public VBFormattingOptions()
        {
            NodeSpacingOptions = NodeSpacingOptions.AfterBeginNode |
                NodeSpacingOptions.AfterNodeBodyStatements |
                NodeSpacingOptions.AfterEndNode |
                NodeSpacingOptions.BeforeBeginNode |
                NodeSpacingOptions.BeforeEndNode |
                NodeSpacingOptions.BeforeNodeBodyStatements;
        }

        public NodeSpacingOptions NodeSpacingOptions { get; set; }

    }

    [Flags]
    public enum NodeSpacingOptions
    {
        None,
        BeforeBeginNode,  //Class, Namespace, Region, If, For, Try 
        AfterBeginNode, //Class, Namespace, Region, If, For, Try 
        BeforeNodeBodyStatements, //Else, ElseIf, Case, Catch, Finally, Etc
        AfterNodeBodyStatements, //Else, ElseIf, Case, Catch, Finally, Etc
        BeforeEndNode, //End Class, End Namespace, End Region, End If, End Case, Next 
        AfterEndNode   //End Class, End Namespace, End Region, End If, End Case, Next 
    }
}
