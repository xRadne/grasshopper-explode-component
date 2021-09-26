using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ExplodingWhale
{
    public class ExplodingWhaleInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "ExplodingWhale";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("27fd3b34-03d0-4edd-a9d0-c672abc1ea49");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
