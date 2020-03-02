using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class PlaceLoopElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UI document and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Get Level
            Level level = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .First(x => x.Name == "Ground Floor");

            // Create 5 points that will define geometry of walls
            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(15, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            // Create curves
            List<Curve> curves = new List<Curve>();
            Line l1 = Line.CreateBound(p1, p2);
            Arc l2 = Arc.Create(p2, p4, p3);
            Line l3 = Line.CreateBound(p4, p5);
            Line l4 = Line.CreateBound(p5, p1);
            curves.Add(l1); curves.Add(l2); curves.Add(l3); curves.Add(l4);

            // Create a curve loop
            CurveLoop crvLoop = CurveLoop.Create(curves);

            // Convert millimiters to revit internal units which is feet
            double offset = UnitUtils.ConvertFromInternalUnits(135, DisplayUnitType.DUT_MILLIMETERS);

            //Offset the curve by 135 mil
            CurveLoop offsetcrv = CurveLoop.CreateViaOffset(crvLoop, offset, new XYZ(0, 0, 1));

            // Create curve array
            CurveArray carray = new CurveArray();
            foreach (Curve crv in offsetcrv)
            {
                carray.Append(crv);
            }

            // Create a floor
            try
            {
                using (Transaction trans = new Transaction(doc, "Place Walls"))
                {
                    trans.Start();
                    doc.Create.NewFloor(carray, false);
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
