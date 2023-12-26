using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Plugin;
using Rhino.Geometry;

namespace MinenifyMe
{
    public class FunctionGeneratorComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FunctionGeneratorComponent class.
        /// </summary>
        public FunctionGeneratorComponent()
          : base("FunctionGenerator", "McF",
              "Makes minecraft function",
              "MinenifyMe", "Functions")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points to make blocks from", GH_ParamAccess.list);
            pManager.AddTextParameter("BlockType", "B", "Block Type", GH_ParamAccess.item, "stone");

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "P", "Points representing the block coordinates", GH_ParamAccess.list);
            pManager.Register_StringParam("BlockType", "B", "Block Type", GH_ParamAccess.list);
            pManager.Register_StringParam("McFunction", "M", "Setblock function commands", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //empty definnitions
            List<Point3d> points = new List<Point3d>();
            string blockType = "stone";
            List<String> blocktypes = new List<string>();
            List<string> commands = new List<string>();

            //get data from gh
            if (!DA.GetDataList(0, points)) return;
            if (!DA.GetData(1, ref blockType)) return;

            // We should now validate the data and warn the user if invalid data is supplied.
            if (blockType == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Block type not defined");
                return;
            }
            if (points == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No curves defined");
                return;
            }




            //rounds all points to nearest integer and filters out double points
            points = points.Select(p => new Point3d(Math.Round(p.X), Math.Round(p.Y), Math.Round(p.Z))).Distinct().ToList();
            
            //make list of blocks
            foreach (Point3d point in points)
            {
                blocktypes.Add(blockType);
            }
            
            //make list of commands
            foreach (Point3d point in points)
            {
                commands.Add("setblock " + "~" + point.X + " " + "~" + point.Z + " " + "~" + point.Y + " " + blockType);
            }
            
            //output
            DA.SetDataList(0, points);
            DA.SetDataList(1, blocktypes);
            DA.SetDataList(2, commands);





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
            get { return new Guid("784B0070-B65E-46D8-858A-5D7074045FF4"); }
        }
    }
}