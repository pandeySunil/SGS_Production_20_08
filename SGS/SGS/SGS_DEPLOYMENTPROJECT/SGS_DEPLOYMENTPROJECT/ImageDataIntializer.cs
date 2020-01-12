using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SGS_DEPLOYMENTPROJECT
{
    public class ImageDataIntializer
    {
        public List<shape> drawnShapes = new List<shape>();
        public List<JsonShape> jsonShapes;
        public List<string> configFileNames { get; set; }
        public Image image { get; set; }
        public Bitmap pbImageBitmap { get; set; }
        public Dictionary<string, Bitmap> OriginalImages = new Dictionary<string, Bitmap>();
        private Dictionary<string, List<shape>> ImageJsonDataDic = new Dictionary<string, List<shape>>();
        private List<string> getAllNames(string ext) {
            List<string> fileNames = new List<string>();
            //fileNames = Directory.GetFiles(Helper.assetFolderPath, "*.*", SearchOption.AllDirectories)
            //.Where(file => new string[] {ext}
            //.Contains(Path.GetExtension(file)))
            //.ToList();
            foreach (var p in System.IO.Directory.GetFiles(Helper.assetFolderPath, "*."+ext).ToList()) {
                fileNames.Add(Path.GetFileName(p));
            }
            return fileNames;
        }

        private void LoadImageConfigFileNames() {
            configFileNames = getAllNames("json");
            foreach (string s in configFileNames) {
                ImageJsonDataDic.Add(s, ImageConfigDataLoad(s,new List<shape>()));
            }

        }
        public Bitmap GetBitmap(int shapepointer,string imageFileId)
        {
            try
            {
                //imageFileId = imageFileId.Substring(0, imageFileId.IndexOf('.'));
                if (ImageJsonDataDic != null && ImageJsonDataDic.Count == 0)
                {
                    LoadImageConfigFileNames();
                }

                drawnShapes = ImageJsonDataDic[imageFileId + ".json"];
                var filePath = Helper.assetFolderPath + "\\" + imageFileId + ".PNG";
                if (shapepointer == 0)
                {
                    return new Bitmap(filePath);
                }
                else
                {
                    if (drawnShapes.Count() > shapepointer)
                    {
                        image = Image.FromFile(filePath);
                        DrawShape(drawnShapes[shapepointer].x_cortinate, drawnShapes[shapepointer].y_cortinate,
                            drawnShapes[shapepointer].reatSize, drawnShapes[shapepointer].p, drawnShapes[shapepointer].penSize, drawnShapes[shapepointer].reactString,
                           drawnShapes[shapepointer].solid);
                    }
                    return pbImageBitmap;
                }
            }
            catch (Exception Ex) {
                MessageBox.Show(Ex.Message);
                return null;
            }
        }
        public Dictionary<string, Bitmap> LoadOrginalImages()
        {
            if (configFileNames == null) {
                configFileNames = getAllNames("json");

            }
            if (ImageJsonDataDic != null && ImageJsonDataDic.Count == 0)
            {
                LoadImageConfigFileNames();
            }
            string i;
            foreach (string f in configFileNames) {
                i = f.Substring(0, f.IndexOf('.'));
                OriginalImages.Add(f,GetBitmap(0,i));
            }
            return OriginalImages;
        }
        private void DrawShape(int x, int y, int reacSize, Pen pen, int penSize, string reactString, bool solid)
        {
            pbImageBitmap = (Bitmap)(image);
            var graphics = Graphics.FromImage((Image)pbImageBitmap);
            Rectangle rect = new Rectangle(x, y, reacSize, reacSize);
            pen.Width = penSize;


            //  pictureBox1.Refresh();
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(pen.Color);
            if (solid)
            {

                graphics.FillRectangle(myBrush, rect);
            }
            else
            {
                graphics.DrawRectangle(pen, rect);
            }
            Font font = new Font("Times New Roman", 12.0f);
            graphics.DrawString(reactString, font, myBrush, x, y);


        }
        public List<shape> ImageConfigDataLoad(string fileName, List<shape> shapes)
        {

            try
            {

                using (StreamReader r = new StreamReader(Helper.assetFolderPath + "\\"+fileName))
                {
                    Console.WriteLine("Reading File..");
                    string json = r.ReadToEnd();
                    Console.WriteLine("Reading File Done");
                    jsonShapes = JsonConvert.DeserializeObject<List<JsonShape>>(json);



                }
            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return null;
            }
            shapes.Add(new shape()
            {
                x_cortinate = jsonShapes[0].x_cortinate,
                y_cortinate = jsonShapes[0].y_cortinate,
                penSize = jsonShapes[0].penSize,
                reatSize = jsonShapes[0].reatSize,
                reactString = jsonShapes[0].reactString,
                p = new Pen(Color.White),
                solid = jsonShapes[0].solid

            });
            foreach (var r in jsonShapes)
            {
                Pen p = new Pen(Color.Green);
                if (r.penColor == "White")
                {
                    p = new Pen(Color.White);
                }
                else if (r.penColor == "Black")
                {
                    p = new Pen(Color.Black);
                }
                else if (r.penColor == "Yellow")
                {
                    p = new Pen(Color.Yellow);
                }
                else if (r.penColor == "Red")
                {
                    p = new Pen(Color.Red);
                }
                shapes.Add(new shape()
                {
                    x_cortinate = r.x_cortinate,
                    y_cortinate = r.y_cortinate,
                    penSize = r.penSize,

                    reatSize = r.reatSize,
                    reactString = r.reactString,
                    p = p,
                    solid = r.solid

                });

            }
            return shapes;
        }
        
    }


}
