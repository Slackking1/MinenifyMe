﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MinenifyMe
{
    public class MakeBlocksComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MakeBlocksComponent class.
        /// </summary>
        public MakeBlocksComponent()
          : base("MakeBlocks", "McB",
              "Make block for rendering in rhino",
              "MinenifyMe", "Rendering")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "P", "Point", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_BRepParam("Brep", "B", "Brep", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //Inital definitions
            List<Point3d> points = new List<Point3d>();

            Brep brep = new Brep();
            List<Brep> breps = new List<Brep>();

            //retrive info from gh
            if (!DA.GetDataList(0, points)) return;


            //Create a block with a centroid of the input points
            foreach (Point3d point in points)
            {
                
                brep = Brep.CreateFromBox(new BoundingBox(point, point + new Vector3d(1, 1, 1)));
                //DA.SetData(0, brep);
                breps.Add(brep);
                
            }



           DA.SetDataList(0, breps);

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
            get { return new Guid("B0A1C7D4-ADF5-4A98-9BA9-40388B5463EF"); }
        }
    }
}