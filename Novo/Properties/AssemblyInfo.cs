using Microsoft.Owin;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WEBAPI_VOPAK;
using WEBAPI_VOPAK.App_Start;

// As informações gerais sobre um assembly são controladas através do seguinte
// conjunto de atributos a seguir. Altere esses valores de atributo para modificar as informações
// associadas a um assembly.
[assembly: AssemblyTitle("WEBAPI_VOPAK")]
[assembly: AssemblyDescription("WebService (API 2.0) para comunicação com a Unisolution.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("José Luís de Oliveira Santos.")]
[assembly: AssemblyProduct("WEBAPI_VOPAK")]
[assembly: AssemblyCopyright("Copyright ©  2017 by Cadu")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Definir ComVisible como falso torna não visíveis os tipos neste assembly
// para componentes COM. Caso precise acessar um tipo neste assembly a partir de
// COM, defina o atributo ComVisible como true nesse tipo.
[assembly: ComVisible(false)]
// A GUID a seguir será referente à ID do typelib se este projeto for exposto ao COM
[assembly: Guid("8d2e66a4-dc63-4ec0-b359-63f816013682")]
// As informações de versão de um assembly consistem nos seguintes quatro valores:
//
//      Versão Principal
//      Versão Secundária
//      Número da Versão
//      Revisão
//
// É possível especificar todos os valores ou definir como padrão os números de revisão e de versão
// usando o '*' como mostrado abaixo:
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: OwinStartup(typeof (Startup))]
[assembly: AssemblyVersion("1.0.0.0")]
