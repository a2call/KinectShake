using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using System.Diagnostics;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    /// <summary>
    /// Interaction logic for OpenGLWin.xaml
    /// </summary>
    public partial class OpenGLWin : Window
    {
        //members
        private GLControl glc;

        float xangle, yangle, zangle;
        float posx, posy, posz;

        private AtomsDB atomsdb = new AtomsDB();
        private Atom atom = null;
        private AtomsDB.StatusType status_ = AtomsDB.StatusType.OK;

        public OpenGLWin()
        {
            InitializeComponent();

            posx = posy = 0;
            xangle = yangle = zangle = 0;
        }

        public void incAngle(float x, float y, float z){
            xangle+=x;
            yangle+=y;
            zangle+=z;
            this.glc.Refresh();
        }

        public void setPos(float x, float y, float z)
        {
            if (atom == null)
            {
                return;
            }

            posx = x-320.0f;
            posy = (480.0f-y)-240.0f;
            posx = posx / 320.0f * 3.33f;
            posy = posy / 240.0f * 2.5f;
            posz = -(z-1000)/128.0f;

            atom.setPos(posx, posy, posz);

            //check for collisions
            status_ = atomsdb.checkIntersection(atom);

            //Debug.WriteLine("pos {0} {1} {2}", posx, posy, posz);
            this.glc.Refresh();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.glc = new GLControl();
            this.glc.Load += new EventHandler(glc_Loaded);
            this.glc.Paint += new System.Windows.Forms.PaintEventHandler(glc_Paint);
            this.glc.Resize += new EventHandler(glc_Resize);
            this.KeyDown += new KeyEventHandler(OpenGLWin_KeyDown);
                        
            this.host.Child = this.glc;
        }

        public void addAtom(Atom.AtomType t)
        {
            if (atom == null)
            {
                this.atom = new Atom(t);
            }
        }

        public void dropAtom()
        {
            if (atom != null)
            {
                this.atomsdb.addAtom(this.atom);
                this.atom = null;
            }
        }

        public AtomsDB.StatusType getStatus()
        {
            return status_;
        }


        void OpenGLWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.O)
            {
                addAtom(Atom.AtomType.O);
            }

            if (e.Key == System.Windows.Input.Key.H)
            {
                addAtom(Atom.AtomType.H);
            }

            if (e.Key == System.Windows.Input.Key.D)
            {
                dropAtom();
            }

            if (e.Key == System.Windows.Input.Key.Left)
            {
                incAngle(0, -3, 0);
            }
            
            if (e.Key == System.Windows.Input.Key.Right)
            {
                incAngle(0, 3, 0);
            }

            if (e.Key == System.Windows.Input.Key.R)
            {
                this.atomsdb.atomlist.Clear();
                atom = null;
            }
            this.glc.Refresh();
        }

        void glc_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glc.ClientSize);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspect = glc.ClientSize.Width * 1.0f / glc.ClientSize.Height;
            //GL.Ortho(-8,-8+16*aspect,-8,8,-10,10);

            Glu.Perspective(45.0f, aspect, 0.1, 100);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();        
        }

        void glc_Loaded(object sender, EventArgs e)
        {
            int w = glc.Width;
            int h = glc.Height;

            // Set up initial modes
            float[] ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] position = { 5.0f, 10.0f, 5.0f, 0.0f };

            float[] lmodel_ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] local_view = { 0.0f };

            GL.Light(LightName.Light0, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light0, LightParameter.Position, position);
            GL.LightModel(LightModelParameter.LightModelAmbient, lmodel_ambient);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, local_view);

            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.ClearColor(System.Drawing.Color.Black);
        }

        void glc_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0, 0, -6);

            GL.Rotate(xangle, 1, 0, 0);
            GL.Rotate(yangle, 0, 1, 0);
            GL.Rotate(zangle, 0, 0, 1);

            GL.Color4(System.Drawing.Color.Red);

            IntPtr q = Glu.NewQuadric();

            if (atom != null)
            {
                GL.PushMatrix();
                GL.Translate(atom.x(), atom.y(), atom.z());
                GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse,
                    atom.color());
                Glu.Sphere(q, atom.r(), 16, 16);
                GL.PopMatrix();
            }

            foreach (Atom a in atomsdb.atomlist)
            {
                GL.PushMatrix();
                GL.Material(MaterialFace.Front, MaterialParameter.AmbientAndDiffuse,
                    a.color());
                GL.Translate(a.x(), a.y(), a.z());
                Glu.Sphere(q, a.r(), 16, 16);
                GL.PopMatrix();
            }
            //Debug.WriteLine(atomsdb.atomlist.Count);


            glc.SwapBuffers();
        }

        /// <summary>
        /// Move object into position.  Use 3rd through 12th parameters to specify the
        /// material property.  Draw a teapot.
        /// </summary>
        private void RenderTeapot(float x, float y, float z,
          float ambr, float ambg, float ambb,
          float difr, float difg, float difb,
          float specr, float specg, float specb, float shine)
        {
            float[] mat = new float[4];

            GL.PushMatrix();
            GL.Translate(x, y, z);
            mat[0] = ambr;
            mat[1] = ambg;
            mat[2] = ambb;
            mat[3] = 1.0f;
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, mat);
            mat[0] = difr;
            mat[1] = difg;
            mat[2] = difb;
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, mat);
            mat[0] = specr;
            mat[1] = specg;
            mat[2] = specb;
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, shine * 128.0f);
            Teapot.DrawSolidTeapot(1f);
            GL.PopMatrix();
        }

        public void drawCursor(int x, int y)
        {
            int [] viewport = new int[4];
            double[] modelview = new double[16];
            double[] projection = new double[16];
            float winx,winy,winz=0;
            double[] glposx = new double[1];
            double[] glposy = new double[1];
            double[] glposz = new double[1];
            GL.GetDouble(GetPName.ModelviewMatrix,modelview);
            GL.GetDouble(GetPName.ProjectionMatrix,projection);
            GL.GetInteger(GetPName.Viewport,viewport);

            winx = x;
            winy = viewport[3]-y;
            GL.ReadPixels((int)winx,(int)winy,1,1,OpenTK.Graphics.PixelFormat.DepthComponent,
                PixelType.Float, ref winz);
            int res = Glu.UnProject(winx, winy, -6, modelview, projection,viewport,
                glposx,glposy,glposz);
            //Debug.WriteLine("cursor {0} {1} {2} {3}", res, glposx[0],glposy[0],glposz[0]);
            //Debug.WriteLine("cursor {0} {1} {2}", winx,winy,winz);
        }
    }
}

