

using Antlr4.Runtime.Misc;
using CobolDevExtension;
using LanguageServer.grammar;

public class WorkingStorageSectionVisitor : CobolParserBaseVisitor<object>
{
    private readonly SymbolTable symbolTable = SymbolTable.Instance;

    private CobolDataVariable currentParentVar = new();

    private bool switch88LevelScope = false;
    public override object VisitWorkingStorageSection([NotNull] CobolParser.WorkingStorageSectionContext context)
    {
        foreach (var entry in context.dataDescriptionEntryForWorkingStorageSection())
        {
            if (entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat1() != null)
            {
                if (switch88LevelScope)
                {
                    currentParentVar = new();
                    switch88LevelScope = false;
                } 
                currentParentVar.Name = entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat1().entryName().GetText();
                VisitDataDescriptionEntryFormat1(entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat1());

            }
            if (entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat3() != null)
            {
                switch88LevelScope = true;
                currentParentVar.Name = entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat3().entryName().GetText();
                VisitDataDescriptionEntryFormat3(entry.dataDescriptionEntryForWorkingStorageAndLinkageSection().dataDescriptionEntry().dataDescriptionEntryFormat3());
            }

            symbolTable.AddDataNode(currentParentVar);

        }

        return base.VisitWorkingStorageSection(context);
    }

    public override object VisitDataDescriptionEntryFormat1([NotNull] CobolParser.DataDescriptionEntryFormat1Context context)
    {

        if (context.dataValueClause() == null || context.dataValueClause().Length == 0)
        {
            return base.VisitDataDescriptionEntryFormat1(context);
        }

        CobolDataVariable cobolDataVariable = new CobolDataVariable
        {
            Name = context.entryName().GetText(),
            Level = int.Parse(context.levelNumber().GetText()),
            Value = context.dataValueClause()[0].dataValueClauseLiteral().GetText(),
            Parent = currentParentVar
        };
        currentParentVar.AddChild(cobolDataVariable);

        symbolTable.AddDataNode(cobolDataVariable);
        return base.VisitDataDescriptionEntryFormat1(context);
    }
    


    public override object VisitDataDescriptionEntryFormat3([NotNull] CobolParser.DataDescriptionEntryFormat3Context context)
    {
        CobolDataVariable cobolDataVariable = new CobolDataVariable
        {
            Name = context.entryName().GetText(),
            Level = 88,
            Value = context.dataValueClause().dataValueClauseLiteral().GetText(),
            Parent = currentParentVar
        };
        currentParentVar.AddChild(cobolDataVariable);
        symbolTable.AddDataNode(cobolDataVariable);
        return base.VisitDataDescriptionEntryFormat3(context);
    }
}