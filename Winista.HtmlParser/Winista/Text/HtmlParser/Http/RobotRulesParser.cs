﻿System.Reflection.AmbiguousMatchException: Ambiguous match found.
   at System.Windows.Forms.Control.MarshaledInvoke(Control caller, Delegate method, Object[] args, Boolean synchronous)
   at System.Windows.Forms.Control.Invoke(Delegate method, Object[] args)
   at ....Resolve(IAssemblyReference , String )
   at ....Resolve(IAssemblyReference , String )
   at ..Load(IAssemblyReference , String )
   at Reflector.CodeModel.Memory.AssemblyReference.Resolve()
   at .......ctor(ICollection , INamespace )
   at ....WriteNamespace(INamespace )
   at ..WriteTypeDeclaration(ITypeDeclaration , String , ILanguageWriterConfiguration )
namespace Winista.Text.HtmlParser.Http
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Text;
    using System.Web;
    using Winista.Text.HtmlParser.Support;
    using Winista.Text.HtmlParser.Util;

