using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplodingWhale
{
    public class ExampleObject
    {
        public string PublicField = "Yup, this is a public field";
        private string PrivateField = "Yes, this is a private field";
        public string PublicProperty { get; set; }  = "Yup, this is a public property";
        public string PrivateProperty { get; set; }  = "Yes, this is a private property";
        public static string PublicStaticField = "Wow, a public static field";

        public ExampleObject()
        {

        }

    }

    public class CreateExampleObject : GH_Component
    {
        public CreateExampleObject()
          : base("ExampleObject", "ExampleObject",
              "ExampleObject",
              "Params", "Util")
        {
        }

        public override Guid ComponentGuid => new Guid("62f6335b-5d04-46d2-bbc8-3a60707b70e4");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExampleObject", "O", "ExampleObject", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData("ExampleObject", new ExampleObject());
        }
    }
}
