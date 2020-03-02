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
    class SetParameter : IExternalCommand
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

                    // Get parameter value
                    Parameter param = ele.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);

                    TaskDialog.Show("Parameter Values", string.Format("Param storage type {0} and value {1}",
                        param.StorageType.ToString(), param.AsDouble()));

                    // Set Parameter value
                    using (Transaction trans = new Transaction(doc, "Set parameter"))
                    {
                        trans.Start();
                        param.Set(7.5);
                        trans.Commit();
                    }


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
