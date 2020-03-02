using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class PlaceFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UI document and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Get Family symbol
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> symbols = collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();

            // Find family in list of symbols
            FamilySymbol symbol = null;
            foreach (Element ele in symbols)
            {
                if (ele.Name == "1525 x 762mm")
                {
                    symbol = ele as FamilySymbol;
                    break;
                }
            }

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                    }
                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), symbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    trans.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
