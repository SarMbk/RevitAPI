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
    public class DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                //Pick object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Display element Id
                if(pickedObj != null)
                {
                    // Create a transaction
                    using (Transaction trans = new Transaction(doc, "Delete Element"))
                    {
                        trans.Start();
                        doc.Delete(pickedObj.ElementId);
                        TaskDialog tDialog = new TaskDialog("Delete Element");
                        tDialog.MainContent = "Are you sure you want to delete the element?";
                        tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if (tDialog.Show() == TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + "was deleted.");
                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + "was NOT deleted.");
                        }
                    }
                }
                return Result.Succeeded;
            }
            catch(Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
            
            
        }
    }
}
