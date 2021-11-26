using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel.Parameters;
using System.Collections; 

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace ExplodingWhale
{
    public class ExplodingWhaleComponent : GH_Component, IGH_VariableParameterComponent
    {
        public List<string> Fields = new List<string>() { "PublicField", "PrivateField" };
        public List<string> Properties = new List<string>() { "PublicProperty", "PrivateProperty" };

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
            //pManager.AddGenericParameter("Object", "O", "Object to explode into its fields and properties", GH_ParamAccess.item);
            
            // This should be a tree access since we must ensure all input are of the same object type
            pManager.AddGenericParameter("Object", "O", "Object to explode into its fields and properties", GH_ParamAccess.tree); 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            //pManager.AddTextParameter("Fields", "F", "Fields and properties of object", GH_ParamAccess.list);
            //pManager.AddGenericParameter("Values", "V", "Values of fields and properties of object", GH_ParamAccess.list);
        }

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //DA.AbortComponentSolution();

            // Get object from GH object wrapper
            object GHwrapper_obj = null;
            if (!DA.GetData("Object", ref GHwrapper_obj)) return;

            Type GHWrapper_type = GHwrapper_obj.GetType();
            IList<PropertyInfo> GHWrapper_props = new List<PropertyInfo>(GHWrapper_type.GetProperties());

            bool isValid = (bool)GHWrapper_props.First(x => x.Name == "IsValid").GetValue(GHwrapper_obj, null);
            object obj = GHWrapper_props.First(x => x.Name == "Value").GetValue(GHwrapper_obj, null);

            // Try get values from object fields and properties
            var object_type = obj.GetType();
            //IList<MemberInfo> members = new List<MemberInfo>(object_type.GetMembers()); //Use members insted??
            IList<FieldInfo> objectFields = new List<FieldInfo>(object_type.GetFields());

            List<string> fieldNames = new List<string>();
            List<object> fieldValues = new List<object>();

            foreach (FieldInfo field in objectFields)
            {
                fieldNames.Add(field.Name);
                fieldValues.Add(field.GetValue(obj));
            }

            IList<PropertyInfo> objectProps = new List<PropertyInfo>(object_type.GetProperties());

            List<string> propertyNames = new List<string>();
            List<object> propertyValues = new List<object>();

            foreach (PropertyInfo prop in objectProps)
            {
                propertyNames.Add(prop.Name);
                propertyValues.Add(prop.GetValue(obj, null));
            }

            // If the fields and properties do not match with previus iteration, the user needs to manually update the fields to get the values of the new fields etc.

            for (int i = 0; i < fieldValues.Count; i++)
            {
                DA.SetData(i, fieldValues[i]);
            }

            for (int i = 0; i < propertyValues.Count; i++)
            {
                DA.SetData(fieldValues.Count + i, propertyValues[i]);
            }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return true;
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
            var param = new Param_GenericObject();
            //param.Name = GH_ComponentParamServer.InventUniqueNickname("ABCDEFGHIJKLMNOPQRSTUVWXYZ", Params.Input);
            //param.NickName = param.Name;
            //param.Description = "Property Name";
            //param.Optional = true;
            //param.Access = GH_ParamAccess.item;

            return param;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            //this.Params
            Params.OnParametersChanged();
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);

            GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Update outputs", new EventHandler(this.UpdateOutputs));
        }

        private void UpdateOutputs(object o, EventArgs e)
        {
            while (this.Params.Output.Count > 0)
                Params.UnregisterOutputParameter(Params.Output[Params.Output.Count - 1]);

            for (int i = 0; i < Fields.Count; i++)
            {
                var param = new Param_GenericObject();
                param.Name = Fields[i];
                param.NickName = Fields[i];
                Params.RegisterOutputParam(param);
            }

            for (int i = 0; i < Properties.Count; i++)
            {
                var param = new Param_GenericObject();
                param.Name = Properties[i];
                param.NickName = Properties[i];
                Params.RegisterOutputParam(param);
            }

            //while (this.Params.Output.Count < this.Properties.Count + this.Fields.Count)
            //{
            //    var param = new Param_GenericObject();
            //    param.Name = $"Param {Params.Count()}";
            //    param.NickName = $"Param {Params.Count()}";
            //    this.Params.RegisterOutputParam(param);
            //}
            //while (this.Params.Output.Count > this.Properties.Count + this.Fields.Count)
            //    this.Params.UnregisterOutputParameter(this.Params.Output[this.Params.Output.Count - 1]);
            Params.OnParametersChanged();
            VariableParameterMaintenance();
            ExpireSolution(true);
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
