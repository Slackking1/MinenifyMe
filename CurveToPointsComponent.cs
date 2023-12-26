using Grasshopper;
using Grasshopper.GUI.Script;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinenifyMe
{
    public class CurveToPointsComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CurveToPointsComponent()
          : base("CurveToPoint", "Curve to point",
            "Converts curves into equavalent block coordinates",
            "MinenifyMe", "Functions")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Curves to convert", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "P", "Points representing the block coordinates", GH_ParamAccess.list);
          
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Definerer variabler til brug i c# kode (overskrides i næste skridt)

            List<Curve> curves = new List<Curve>();
            double divisionLength = 0.333;
            
            List<Point3d> curvePoints = new List<Point3d>();
            Rhino.Geometry.Point3d[] curvePoint;


            

       

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
            if (!DA.GetDataList(0, curves)) return;

    


            // We should now validate the data and warn the user if invalid data is supplied.
            if (curves == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No curves defined");
                return;
            }

      

            //Splits curves into points
            foreach (Curve curve in curves)
            {
                curve.DivideByLength(divisionLength, true, out curvePoint);
                //add curvepoints to list
                foreach (Point3d point in curvePoint)
                {                  
                    curvePoints.Add(point);
                }            
            }

            //rounds all points to nearest integer and filters out double points
            curvePoints = curvePoints.Select(p => new Point3d(Math.Round(p.X), Math.Round(p.Y), Math.Round(p.Z))).Distinct().ToList();



            
            //output
            DA.SetDataList(0, curvePoints);



        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("00d8e131-4437-48c8-9bf6-ebd1c8118cec");
    }
}