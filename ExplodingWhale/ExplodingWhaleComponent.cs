using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace ExplodingWhale
{
    public class ExplodingWhaleComponent : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ExplodingWhaleComponent()
          : base("ExplodingWhale", "Explode",
              "Explode any object into its fields and properties",
              "Params", "Util")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Object to explode into its fields and properties", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Fields", "F", "Fields and properties of object", GH_ParamAccess.list);
            pManager.AddGenericParameter("Values", "V", "Values of fields and properties of object", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //DA.AbortComponentSolution();

           
            object GHwrapper_obj = null;
            if (!DA.GetData("Object", ref GHwrapper_obj)) return;

            /////

            Type GHWrapper_type = GHwrapper_obj.GetType();
            IList<PropertyInfo> GHWrapper_props = new List<PropertyInfo>(GHWrapper_type.GetProperties());

            bool isValid = (bool)GHWrapper_props.First(x => x.Name == "IsValid").GetValue(GHwrapper_obj, null);
            object obj = GHWrapper_props.First(x => x.Name == "Value").GetValue(GHwrapper_obj, null);


            var object_type = obj.GetType();
            IList<PropertyInfo> object_props = new List<PropertyInfo>(object_type.GetProperties());

            List<string> propertyNames = new List<string>();
            List<object> propertyValues = new List<object>();

            foreach (PropertyInfo prop in object_props)
            {
                propertyNames.Add(prop.Name);
                propertyValues.Add(prop.GetValue(obj, null));
                // Do something with propValue
            }

            for (int i = 0; i < propertyNames.Count; i++)
            {
                
            }
            

        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
                return false;
            else
                return true;
        }
        
        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }
        

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public void VariableParameterMaintenance()
        {
            
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            var item1 = Menu_AppendItem(menu, "Item 1", Menu_Clicked, true, true);
            item1.ToolTipText = "Tooltip for item 1";
            item1.Name = "Item 1";

            var item2 = new ToolStripMenuItem("Item 2", null, (sender, e) => { });
            item2.Name = "Item 2";
            menu.Items.Add(item2);

            Menu_AppendItem(menu, "Public", Menu_Clicked, true, true);
            Menu_AppendItem(menu, "Private", Menu_Clicked, true, true);
            Menu_AppendItem(menu, "Static", Menu_Clicked, true, true);
            Menu_AppendItem(menu, "Instance", Menu_Clicked, true, true);

            base.AppendAdditionalMenuItems(menu);
        }

        private void Menu_Clicked(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            //menu.Checked = !menu.Checked;

            Rhino.RhinoApp.WriteLine($"Clicked: {menu.Name}, Checked={menu.Checked}");
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("26d4b024-0795-49aa-bcc6-39a6c2e04ca8"); }
        }
    }
}
