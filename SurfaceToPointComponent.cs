using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace MinenifyMe
{
    public class SurfaceToPointComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SurfaceToFunctionComponent class.
        /// </summary>
        public SurfaceToPointComponent()
          : base("SurfaceToPoint", "StF",
              "Generates points on surface",
              "MinenifyMe", "Functions")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.Register_SurfaceParam("Surface", "S", "Surface to convert", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "P", "Points representing the block coordinates", GH_ParamAccess.list);
            //pManager.Register_SurfaceParam("Surfaces", "S", "Surfaces representing the block coordinates", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Create variable for input surface
            Rhino.Geometry.Surface surface = null;

            List<Surface> surfaces = new List<Surface>();
            // List to hold our generated points
            List<Rhino.Geometry.Point3d> points = new List<Rhino.Geometry.Point3d>();

            // If there's no input at our first parameter (index 0), give a warning and return.
            if (!DA.GetDataList(0, surfaces)) return;


            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            //if (!DA.GetDataList(0, curves)) return;


            // Get the boundingbox of the surface, this contains all information needed to generate the grid
            foreach (Surface surface1 in surfaces)
            {
                surface = surface1;
                var bbox = surface.GetBoundingBox(false);


                for (double x = bbox.Min.X; x <= bbox.Max.X; x += 0.66)
                {
                    for (double y = bbox.Min.Y; y <= bbox.Max.Y; y += 0.66)
                    {
                        for (double z = bbox.Min.Z; z <= bbox.Max.Z; z += 0.66)
                        {
                            // Create a point at the current x, y and z value
                            Rhino.Geometry.Point3d pt = new Rhino.Geometry.Point3d(x, y, z);

                            // Check if the point is on the surface
                            double u, v;
                            if (surface.ClosestPoint(pt, out u, out v))
                            {
                                // If it is, add the surface point to the list
                                var surfacePt = surface.PointAt(u, v);
                                points.Add(surfacePt);
                            }
                        }
                    }
                }
            }

            //rounds all points to nearest integer and filters out double points
            List<Point3d> surfacePoints = new List<Point3d>();
            surfacePoints = points.Select(p => new Point3d(Math.Round(p.X), Math.Round(p.Y), Math.Round(p.Z))).Distinct().ToList();


            // Assign the points to the output parameter (index 0)
            DA.SetDataList(0, surfacePoints);


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
            get { return new Guid("0C3B5185-014D-49E4-927A-3DDFA7C58A2A"); }
        }
    }
}