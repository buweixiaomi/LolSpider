System.Reflection.AmbiguousMatchException: Ambiguous match found.
   at System.Windows.Forms.Control.MarshaledInvoke(Control caller, Delegate method, Object[] args, Boolean synchronous)
   at System.Windows.Forms.Control.Invoke(Delegate method, Object[] args)
   at ....Resolve(IAssemblyReference , String )
   at ....Resolve(IAssemblyReference , String )
   at ..Load(IAssemblyReference , String )
   at Reflector.CodeModel.Memory.AssemblyReference.Resolve()
   at .......ctor(ICollection , INamespace )
   at ....WriteNamespace(INamespace )
   at ..WriteTypeDeclaration(ITypeDeclaration , String , ILanguageWriterConfiguration )
namespace Winista.Text.HtmlParser
{
    using System;
    using Winista.Text.HtmlParser.Data;
    using Winista.Text.HtmlParser.Extractors;
    using Winista.Text.HtmlParser.Filters;
    using Winista.Text.HtmlParser.Http;
    using Winista.Text.HtmlParser.Lex;
    using Winista.Text.HtmlParser.Support;
    using Winista.Text.HtmlParser.Tags;
    using Winista.Text.HtmlParser.Util;
    using Winista.Text.HtmlParser.Visitors;


