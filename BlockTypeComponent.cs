using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MinenifyMe
{
    public class BlockTypeComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BlockType class.
        /// </summary>
        public BlockTypeComponent()
          : base("BlockType", "Nickname",
              "Description",
              "MinenifyMe", "Library")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("BlockTypes", "Bt", "Available block from minecraft used together for instance itemselector in human plugin", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //Create a list based on the file blocks.txt, where each line is a list item with a block type
            //List<string> blockTypes = new List<string>();
            string[] lines = System.IO.File.ReadAllLines(@"D:\MinenifyMe\blocks.txt");



            //Set the output to the blockTypes list
            DA.SetDataList(0, lines);


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6E747C49-C31D-4F17-9457-4A7353E35AFB"); }
        }
    }
}