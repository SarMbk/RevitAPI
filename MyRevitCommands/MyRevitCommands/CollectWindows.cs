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
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument and Project document objects
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Create filtered element collector
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            //Create Window filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

            //Apply filter to collector
            // WhereElementIsNotElementType is a method that tells revit to look only through instances and not element types
            // ToElements method returns the set of elements that pass the filter
            IList<Element> windows = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            TaskDialog.Show("Windows", string.Format("{0} windows counted", windows.Count));

            return Result.Succeeded;

        }
    }
}
