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
    class GetParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UI document and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                // Pick object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                if (pickedObj != null)
                {
                    // Retrieve element
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);

                    // Get parameter
                    Parameter param = ele.LookupParameter("Head Height");
                    InternalDefinition paramDef = param.Definition as InternalDefinition;

                    TaskDialog.Show("Parameters", string.Format("{0} parameter of type {1} with built in parameter {2}",
                        paramDef.Name, paramDef.UnitType, paramDef.BuiltInParameter));
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
