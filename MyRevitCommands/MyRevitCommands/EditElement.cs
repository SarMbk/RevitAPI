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
    public class EditElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                //Pick Object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Display Element Id
                if (pickedObj != null)
                {
                    //Retrieve Element
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);

                    using (Transaction trans = new Transaction(doc, "Edit Elements"))
                    {
                        trans.Start();

                        //Move Element
                        XYZ moveVec = new XYZ(3, 3, 0);
                        ElementTransformUtils.MoveElement(doc, eleId, moveVec);

                        //Rotate Element
                        LocationPoint loc = ele.Location as LocationPoint;
                        XYZ axisStart = loc.Point;
                        XYZ axisEnd = new XYZ(axisStart.X, axisStart.Y, axisStart.Z + 1);
                        Line rotationAxis = Line.CreateBound(axisStart, axisEnd);
                        ElementTransformUtils.RotateElement(doc, eleId, rotationAxis, 45);

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