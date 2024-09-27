using System.Runtime.InteropServices;
using TreeSitter_Csharp.constants;


public static class LanguageLoader
{
    public static IntPtr LoadLanguage(SupportedLanguages language)
    {
        switch (language)
        {
            case SupportedLanguages.Json:
                return tree_sitter_json();
            case SupportedLanguages.CSharp:
                return tree_sitter_c_sharp();
            case SupportedLanguages.Bash:
                return tree_sitter_bash();
            case SupportedLanguages.C:
                return tree_sitter_c();
            case SupportedLanguages.CMake:
                return tree_sitter_cmake();
            case SupportedLanguages.CPP:
                return tree_sitter_cpp();
            case SupportedLanguages.CSS:
                return tree_sitter_css();
            case SupportedLanguages.DockerFile:
                return tree_sitter_dockerfile();
            case SupportedLanguages.Go:
                return tree_sitter_go();
            case SupportedLanguages.GoMod:
                return tree_sitter_go_mod();
            case SupportedLanguages.Java:
                return tree_sitter_java();
            case SupportedLanguages.JavaScript:
                return tree_sitter_javascript();
            case SupportedLanguages.MarkDown:
                return tree_sitter_markdown();
            case SupportedLanguages.Perl:
                return tree_sitter_perl();
            case SupportedLanguages.Python:
                return tree_sitter_python();
            case SupportedLanguages.Ruby:
                return tree_sitter_ruby();
            case SupportedLanguages.Rust:
                return tree_sitter_rust();
            case SupportedLanguages.Toml:
                return tree_sitter_toml();
            case SupportedLanguages.TypeScript:
                return tree_sitter_typescript();
            default:
                throw new ArgumentException("Unsupported language");
        }
    }

    [DllImport(DllConstants.TreeSitterJsonDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_json();

    [DllImport(DllConstants.TreeSitterCsharpDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_c_sharp();

    [DllImport(DllConstants.TreeSitterBashDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_bash();

    [DllImport(DllConstants.TreeSitterCDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_c();

    [DllImport(DllConstants.TreeSitterCMakeDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_cmake();

    [DllImport(DllConstants.TreeSitterCPPDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_cpp();

    [DllImport(DllConstants.TreeSitterCSSDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_css();

    [DllImport(DllConstants.TreeSitterDockerFileDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_dockerfile();

    [DllImport(DllConstants.TreeSitterGoDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_go();

    [DllImport(DllConstants.TreeSitterGoModDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_go_mod();

    [DllImport(DllConstants.TreeSitterJavaDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_java();

    [DllImport(DllConstants.TreeSitterJavaScriptDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_javascript();

    [DllImport(DllConstants.TreeSitterMarkDownDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_markdown();

    [DllImport(DllConstants.TreeSitterPerlDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_perl();

    [DllImport(DllConstants.TreeSitterPythonDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_python();

    [DllImport(DllConstants.TreeSitterRubyDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_ruby();

    [DllImport(DllConstants.TreeSitterRustDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_rust();

    [DllImport(DllConstants.TreeSitterTomlDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_toml();

    [DllImport(DllConstants.TreeSitterTypeScriptDll, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr tree_sitter_typescript();
}

