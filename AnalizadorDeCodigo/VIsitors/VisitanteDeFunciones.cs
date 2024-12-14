using TreeSitter_Csharp.models.treeSitterModels.classes;

public class VisitanteDeFunciones
{
    private readonly TSTree _arbol;

    public VisitanteDeFunciones(TSTree arbol)
    {
        _arbol = arbol;
    }

    public IEnumerable<TSNode> ObtenerDeclaracionesDeFunciones()
    {
        var nodoRaiz = _arbol.RootNode();
        foreach (var nodo in VisitarNodo(nodoRaiz))
        {
            if (nodo.Type() == "method_declaration") // Ajusta según el lenguaje y la gramática
            {
                yield return nodo;
            }
        }
    }

    private IEnumerable<TSNode> VisitarNodo(TSNode nodo)
    {
        // Asegúrate de no procesar nodos nulos
        //if (nodo.IsNull())
        //    yield break;

        // Visita cada hijo del nodo
        for (uint i = 0; i < nodo.ChildCount(); i++)
        {
            var hijo = nodo.Child(i);
            yield return hijo;

            // Recurre en los descendientes
            foreach (var descendiente in VisitarNodo(hijo))
            {
                yield return descendiente;
            }
        }
    }
}
