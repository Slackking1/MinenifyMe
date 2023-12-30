using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Plugin;
using MinenifyMe.Properties;
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
            
            pManager.AddTextParameter("Path", "Pa", "Path to save function to", GH_ParamAccess.item);
            pManager.AddTextParameter("FunctionName", "F", "Name of function", GH_ParamAccess.item, "functionTest");
            pManager.AddBooleanParameter("Run", "R", "Run component", GH_ParamAccess.item, false);
     
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_PointParam("Points", "P", "Points representing the block coordinates", GH_ParamAccess.list);
            pManager.Register_StringParam("BlockType", "B", "Block Type", GH_ParamAccess.list);
            pManager.Register_StringParam("McFunction", "M", "Setblock function commands", GH_ParamAccess.list);
            pManager.Register_BooleanParam("Success", "S", "True if function was saved and split", GH_ParamAccess.item);
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
            List<string> blocktypes = new List<string>();
            List<string> commands = new List<string>();


            
            string pathToSave = "";
            string functionName = "functionTest";

            bool success = false;
            bool run = false;

            //get data from gh
            if (!DA.GetDataList(0, points)) return;
            if (!DA.GetData(1, ref blockType)) return;
            
            if (!DA.GetData(2, ref pathToSave)) return;
            if (!DA.GetData(3, ref functionName)) return;
            if (!DA.GetData(4, ref run)) return;


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
            if (pathToSave == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No path defined");
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
            

            //Make function name all lower characters
            functionName = functionName.ToLower();

//split the list of commands if it exceeds 62000 rows. Generate a tree branch for each list
            //saves the function to the path with the function name, for each branch appends a number to the function name eg. _1, _2, _3.
            //if the method is succesfully run set success to true

            if (run==true)
            {
                if (commands.Count <= 62000)
                {
                    try
                    {
                        File.WriteAllLines(pathToSave + "\\" + functionName + ".mcfunction", commands);
                        //Append a command to the file that contains "say functionName has run"
                        File.AppendAllText(pathToSave + "\\" + functionName + ".mcfunction", "say " + functionName + " has run");
                        success = true;
                    }
                    catch (Exception e)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not save function");
                        return;
                    }
                }
                //add method for splitting the list of commands into branches
                else if (commands.Count > 62000)
                {
                    int numberOfBranches = (int)Math.Ceiling((double)commands.Count / 62000);
                    List<List<string>> commandBranches = new List<List<string>>();
                    for (int i = 0; i < numberOfBranches; i++)
                    {
                        commandBranches.Add(commands.GetRange(i * 62000, Math.Min(62000, commands.Count - i * 62000)));
                    }
                    commands = commandBranches.SelectMany(x => x).ToList();
                    for (int i = 1; i < numberOfBranches; i++)
                    {
                        try
                        {
                            File.WriteAllLines(pathToSave + "\\" + functionName + "_" + i + ".mcfunction", commandBranches[i]);
                            //Append a command to the file that contains "say functionName has run"

                            File.AppendAllText(pathToSave + "\\" + functionName + "_" + i + ".mcfunction", "say " + functionName + "_" + i + " has run");
                            success = true;
                        }
                        catch (Exception e)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not save function");
                            return;
                        }
                    }
                }
            }

            





            //output
            DA.SetDataList(0, points);
            DA.SetDataList(1, blocktypes);
            DA.SetDataList(2, commands);
            DA.SetData(3, success);





        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return new Bitmap(Resources.functionGenerator, new Size(24, 24));
                //return null;
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