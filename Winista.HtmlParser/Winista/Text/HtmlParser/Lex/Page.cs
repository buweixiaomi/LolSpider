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
namespace Winista.Text.HtmlParser.Lex
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using Winista.Text.HtmlParser.Http;
    using Winista.Text.HtmlParser.Support;
    using Winista.Text.HtmlParser.Util;


