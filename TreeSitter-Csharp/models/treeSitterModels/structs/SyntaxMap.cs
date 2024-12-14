namespace TreeSitter_Csharp.models.treeSitterModels.structs
{
    public static class SyntaxMap
    {
        public static readonly Dictionary<string, List<string>> SyntaxDictionary = new Dictionary<string, List<string>>
        {
            { "expression_statement", new List<string> { "expression" } },
            { "invocation_expression", new List<string> { "function", "arguments" } },
            { "member_access_expression", new List<string> { "expression", "name" } },
            { "variable_declaration", new List<string> { "type", "name", "initializer" } },
            { "argument_list", new List<string> { "argument" } },
            { "binary_expression", new List<string> { "left", "right" } },
            { "try_statement", new List<string> { "body", "catch_clauses", "finally_clause" } },
            { "catch_clause", new List<string> { "catch_declaration", "body" } },
            { "catch_declaration", new List<string> { "type", "name" } },
            { "finally_clause", new List<string> { "body" } },
            { "local_declaration_statement", new List<string> { "variable_declaration" } },
            { "variable_declarator", new List<string> { "name", "initializer" } },
            { "method_declaration", new List<string> { "modifiers", "return_type", "name", "parameter_list", "body" } },
            { "class_declaration", new List<string> { "modifiers", "name", "type_parameters", "base_list", "body" } },
            { "namespace_declaration", new List<string> { "name", "body" } },
            { "compilation_unit", new List<string> { "namespace_declaration", "class_declaration", "using_directive" } },
            { "if_statement", new List<string> { "condition", "consequence", "alternative" } },
            { "for_statement", new List<string> { "initializer", "condition", "update", "body" } },
            { "while_statement", new List<string> { "condition", "body" } },
            { "do_statement", new List<string> { "body", "condition" } },
            { "switch_statement", new List<string> { "expression", "switch_section" } },
            { "switch_section", new List<string> { "case_label", "statement_list" } },
            { "return_statement", new List<string> { "expression" } },
            { "throw_statement", new List<string> { "expression" } },
            { "parameter_list", new List<string> { "parameter" } },
            { "parameter", new List<string> { "type", "name" } },
            { "property_declaration", new List<string> { "modifiers", "type", "name", "accessor_list" } },
            { "accessor_list", new List<string> { "get_accessor", "set_accessor" } },
            { "object_creation_expression", new List<string> { "type", "argument_list", "initializer" } },
            { "array_creation_expression", new List<string> { "type", "rank_specifier", "initializer" } },
            { "initializer_expression", new List<string> { "expression" } },
            { "assignment_expression", new List<string> { "left", "right" } },
            { "parenthesized_expression", new List<string> { "expression" } },
            { "unary_expression", new List<string> { "operator", "operand" } },
            { "conditional_expression", new List<string> { "condition", "consequence", "alternative" } },
            { "lambda_expression", new List<string> { "parameter_list", "body" } },
            { "attribute_list", new List<string> { "attribute" } },
            { "attribute", new List<string> { "name", "argument_list" } },
            { "type_argument_list", new List<string> { "type" } },
            { "type_parameter_list", new List<string> { "type_parameter" } },
            { "constructor_declaration", new List<string> { "modifiers", "name", "parameter_list", "body" } },
            { "enum_declaration", new List<string> { "modifiers", "name", "body" } },
            { "interface_declaration", new List<string> { "modifiers", "name", "type_parameters", "base_list", "body" } },
            { "indexer_declaration", new List<string> { "modifiers", "type", "parameter_list", "accessor_list" } }
        };
    }

}
