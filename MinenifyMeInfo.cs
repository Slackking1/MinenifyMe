using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace MinenifyMe
{
    public class MinenifyMeInfo : GH_AssemblyInfo
    {
        public override string Name => "MinenifyMe";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "This library provides tools for converting grasshopper/rhino geometry into minecraft setblock functionsw. Primary purpose is to make the generation of block coordinates consistent and faster than using native grasshopper components.";

        public override Guid Id => new Guid("fe0f8d0d-27af-428f-95f5-fb2f98373ece");

        //Return a string identifying you or your company.
        public override string AuthorName => "Daniel Bo Olesen";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "dbo@oceng.dk";
    }
}