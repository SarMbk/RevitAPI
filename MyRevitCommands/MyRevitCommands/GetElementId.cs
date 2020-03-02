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
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Retreive current active UI document
            // commandData is an ExternalCommandData object. From there you access the application property. From there you get the active UI document property
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            // Get Document
            Document doc = uidoc.Document;

            try
            {
                // Pick object (go to uidoc object, then to selection class, then to pickobject method)
                // Pick object takes in a parameter, it asks what kind of element do you want to pick.
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // Retreive element Id
                ElementId eleId = pickedObj.ElementId;

                //Get Element from id
                Element ele = doc.GetElement(eleId);

                //Get element type (stored in an ElementId variable)
                ElementId eTypeId = ele.GetTypeId();
                ElementType eType = doc.GetElement(eTypeId) as ElementType;

                //Display element Id
                if (pickedObj != null)
                {
                    // Show a task dialog
                    TaskDialog.Show(    "Element Classification", eleId.ToString() + Environment.NewLine + 
                                        "Category: " + ele.Category.Name + Environment.NewLine + 
                                        "Instance: " + ele.Name + Environment.NewLine +
                                        "Symbol: " + eType.Name + Environment.NewLine +
                                        "Family Name: " + eType.FamilyName);
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
