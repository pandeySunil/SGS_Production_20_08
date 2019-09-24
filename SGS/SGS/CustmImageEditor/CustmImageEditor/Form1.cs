using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;

namespace CustmImageEditor
{
    public partial class Form1 : Form
    {
        public static Pen selectedPen { get; set; }
        public static int RectSize { get; set; }
        public int PenSize { get; set; }
        public static Graphics graphics { get; set; }
        public static string rectValue { get; set; }
        public static string filePath { get; set; }
        public static List<shape> drawnShape = new List<shape>();
        public static string path { get; set; }
        public static int defaultRectSize { get; set; }
        public static System.Drawing.Graphics rectSizePanelGrapics;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Note: pbImage is the name of the picture box used here.
            var mouseEventArgs = e as MouseEventArgs;
            int x = mouseEventArgs.Location.X;
            int y = mouseEventArgs.Location.Y;
            AddShape(x, y, RectSize, selectedPen, PenSize, rectText.Text,checkBox1.Checked);
            DrawShape(x, y, RectSize, selectedPen, PenSize, rectText.Text,checkBox1.Checked);
            //Bitmap pbImageBitmap = (Bitmap)(pictureBox1.Image);
            //graphics = Graphics.FromImage((Image)pbImageBitmap);
            //Pen whitePen = new Pen(Color.White, 1);
            //label1.Text = "X: " + x + " Y: " + y;
            ////Rectangle rect = new Rectangle(x, y, RectSize, RectSize);
            //selectedPen.Width = PenSize;
            ////graphics.DrawRectangle(selectedPen, rect);
            //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(selectedPen.Color);
            //Font font = new Font("Times New Roman", 12.0f);
            //graphics.DrawString(rectText.Text, font, myBrush, x, y);

            // Refresh the picture box control in order that
            // our graphics operation can be rendered.
           // pictureBox1.Refresh();

            // Calling Dispose() is like calling the destructor of the respective object.
            // Dispose() clears all resources associated with the object, but the object still remains in memory
            // until the system garbage-collects it.
          //  graphics.Dispose();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Pen whitePen = new Pen(Color.White);
            PenSize = 1;
            selectedPen = new Pen(Color.White, 1);
            defaultRectSize = 20;
            RectSize = defaultRectSize;
            RectColor whitePen = new RectColor() { p = new Pen(Color.White), ColorName = "White" };
            RectColor redPen = new RectColor() { p = new Pen(Color.Red), ColorName = "Red" };
            RectColor blackPen = new RectColor() { p = new Pen(Color.Black), ColorName = "Black" };
            RectColor yellowPen = new RectColor() { p = new Pen(Color.Yellow), ColorName = "Yellow" };
            comboBoxColor.DisplayMember = "ColorName";
            comboBoxColor.ValueMember = "p";
            comboBoxColor.SelectedValue = whitePen;
            comboBoxColor.Items.Add(redPen);
            comboBoxColor.Items.Add(blackPen);
            comboBoxColor.Items.Add(yellowPen);
            comboBoxRectSize.ValueMember = "size";
            comboBoxRectSize.DisplayMember = "size";
            comboBoxRectSize.Items.Add(new RectSize() { size = 20 });
            comboBoxRectSize.Items.Add(new RectSize() { size = 40 });
            comboBoxRectSize.Items.Add(new RectSize() { size = 50 });
            comboBoxRectSize.Items.Add(new RectSize() { size = 60 });
            comboBoxRectSize.Items.Add(new RectSize() { size = 80 });

            comboBoxPenSize.ValueMember = "size";
            comboBoxRectSize.DisplayMember = "size";
            comboBoxPenSize.Items.Add(new PenSize() { size = 1 });
            comboBoxPenSize.Items.Add(new PenSize() { size = 2 });
            comboBoxPenSize.Items.Add(new PenSize() { size = 3 });
            comboBoxPenSize.Items.Add(new PenSize() { size = 4 });
            comboBoxPenSize.Items.Add(new PenSize() { size = 5 });
            comboBoxPenSize.Items.Add(new PenSize() { size = 6 });
            comboBoxColor.SelectedIndex = 0;
            comboBoxRectSize.SelectedIndex = 0;
            comboBoxPenSize.SelectedIndex = 0;
            RectSize = 20;

            rectSizePanel.Refresh();
            rectSizePanelGrapics = rectSizePanel.CreateGraphics();
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            rectSizePanelGrapics.FillRectangle(myBrush, new Rectangle(0, 0, RectSize, RectSize));
            



        }
        private void UndoShape()
        {
            if (drawnShape.Count() > 0)
            {
                drawnShape.RemoveAt(drawnShape.Count() - 1);
                Image image = Image.FromFile(filePath);
                this.pictureBox1.Image = image;
                Bitmap pbImageBitmap = (Bitmap)(pictureBox1.Image);
                graphics = Graphics.FromImage((Image)pbImageBitmap);
                foreach (var r in drawnShape)
                {
                    DrawShape(r.x_cortinate, r.y_cortinate, r.reatSize, r.p, r.penSize, r.reactString,r.solid);
                }
            }


        }
        private void AddShape(int x, int y, int reacSize, Pen pen, int penSize, string reactString,bool solid)
        {
            drawnShape.Add(new shape() { x_cortinate = x, y_cortinate = y, reatSize = reacSize, p = pen, penSize = penSize, reactString = reactString, solid = solid });

        }
        private void DrawShape(int x, int y, int reacSize, Pen pen, int penSize, string reactString,bool solid)
        {
            Bitmap pbImageBitmap = (Bitmap)(pictureBox1.Image);
            graphics = Graphics.FromImage((Image)pbImageBitmap);
            Rectangle rect = new Rectangle(x, y, reacSize, reacSize);
            pen.Width = penSize;
            
            
           
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(selectedPen.Color);
            Font font = new Font("Times New Roman", 12.0f);
            graphics.DrawString(reactString, font, myBrush, x, y);
            if (solid)
            { graphics.FillRectangle(myBrush, rect); }
            else
            {
                graphics.DrawRectangle(pen, rect);
            }

            pictureBox1.Refresh();
        }
      

        private void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedPen = (Pen)comboBoxColor.SelectedValue;
            selectedPen = ((RectColor)comboBoxColor.SelectedItem).p;
            //selectedPen.Width = PenSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.Yellow);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var i = new ImageViewer();
                i.Show();

            i.Focus();
            this.Hide();
        }
        private void SaveImage(Image img) {

            Bitmap bm = new Bitmap(img);
            bm.Save(fileSavePath.Text + "\\BaseImage.png", System.Drawing.Imaging.ImageFormat.Png);
            // pictureBox1.Image.Save(@"D:\DayUsers\Sunil Pandey\Practice\Angular\R&D\ImageEditorImages", ImageFormat.Jpeg);
        }
        private void SelectImage()
        {
            filePath = "";
            // Create and open a file selector
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.InitialDirectory = ".";

            // select filter type
            // opnDlg.Filter = "Parm Files (*.parm)|*.parmAll Files (*.*)|*.*";

            if (opnDlg.ShowDialog(this) == DialogResult.OK)
            {
                // If file is in start folder remove path
                FileInfo f = new FileInfo(opnDlg.FileName);

                // use relative path if under application
                // starting directory
                fileSavePath.Text = f.DirectoryName;
                if (f.DirectoryName == Application.StartupPath)
                    filePath = f.Name;  // only file name
                else if (f.DirectoryName.StartsWith(Application.StartupPath))
                    // relative path
                    filePath = f.FullName.Replace(Application.StartupPath, ".");
                else
                    filePath = f.FullName;  // Full path
            }
            Image image = Image.FromFile(filePath);
           // var baseImag = (Image)(new Bitmap(image, new Size(700, 500)));
            SaveImage(image);
          
            var i = filePath.LastIndexOf("\\");
            path = filePath.Substring(0, i);
            filePath = path + "\\BaseImage.png";
            image = Image.FromFile(filePath);
            this.pictureBox1.Image = image;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectImage();
        }

        private void comboBoxPenSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PenSize = ((PenSize)comboBoxPenSize.SelectedItem).size;

        }

        private void comboBoxRectSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RectSize = Convert.ToInt32(comboBoxRectSize.SelectedValue);
            RectSize = ((RectSize)comboBoxRectSize.SelectedItem).size;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UndoShape();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<JsonShape> jsonShapes = new List<JsonShape>();
            var penColor = "";
            foreach (var o in drawnShape) {
                if (o.p.Color == Color.White)
                {
                    penColor = "White";

                }
                else if (o.p.Color == Color.Yellow)
                {
                    penColor = "Yellow";

                }
                else if (o.p.Color == Color.Red)
                {
                    penColor = "Red";

                }
                else {
                    penColor = "Black";
                }

                jsonShapes.Add(new JsonShape() { x_cortinate = o.x_cortinate,
                    y_cortinate = o.y_cortinate,
                    penColor = penColor,
                    reactString = o.reactString,
                    reatSize = o.reatSize,
                    penSize = o.penSize,
                    solid =o.solid,
                    Id = jsonShapes.Count() + 1
                });
            }
            
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(jsonShapes);
          //  var json = JsonConverter.Serialize(drawnShape);
            System.IO.File.WriteAllText(path+ "\\BaseImageConfig.json", str);
          
            MessageBox.Show("Image Data Saved");
        }

        private void resctSizeIncreaseSize_Click(object sender, EventArgs e)
        {
            if (RectSize < 180 || RectSize == 180)
            {
                rectSizePanel.Refresh();
                RectSize = RectSize + 5;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(selectedPen.Color);
                selectedPen.Width = PenSize;
                Font font = new Font("Times New Roman", 12.0f);
                rectSizePanelGrapics.DrawString(rectText.Text, font, myBrush, 4, 4);
                if (checkBox1.Checked)
                { rectSizePanelGrapics.FillRectangle(myBrush, new Rectangle(4, 4, RectSize, RectSize)); }
                else
                {
                    rectSizePanelGrapics.DrawRectangle(selectedPen, new Rectangle(4, 4, RectSize, RectSize));
                }
                //myBrush.Dispose();
            }
        }

        private void rectSizeDecreaseButton_Click(object sender, EventArgs e)
        {
            
            if (RectSize > 20 || RectSize == 20) {
                rectSizePanel.Refresh();
                RectSize = RectSize - 5;
                selectedPen.Width = PenSize;
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(selectedPen.Color);
               
                Font font = new Font("Times New Roman", 12.0f);
                rectSizePanelGrapics.DrawString(rectText.Text, font, myBrush, 4, 4);
                if (checkBox1.Checked)
                { rectSizePanelGrapics.FillRectangle(myBrush, new Rectangle(4, 4, RectSize, RectSize)); }
                else
                {
                    rectSizePanelGrapics.DrawRectangle(selectedPen, new Rectangle(4, 4, RectSize, RectSize));
                }
            }
            
        }
    }

    public class RectColor
    {
        public Pen p { get; set; }
        public string ColorName { get; set; }

    }
    public class JsonRectColor {
        public string ColorName { get; set; }
    }
    public class RectSize
    {
        public int size { get; set; }


    }
    public class PenSize
    {
        public int size { get; set; }

    }
    public class JsonShape {
        public int Id { get; set; }
        public int x_cortinate { get; set; }
        public int y_cortinate { get; set; }
        public int penSize { get; set; }
        public int reatSize { get; set; }
        public string penColor { get; set; }
        public string reactString { get; set; }
        public bool solid { get; set; }

    }
    public class shape
    {
        public int x_cortinate { get; set; }
        public int y_cortinate { get; set; }
        public int penSize { get; set; }
        public int reatSize { get; set; }
        public Pen p { get; set; }
        public string reactString { get; set; }
        public bool solid  { get; set; }

}
}
