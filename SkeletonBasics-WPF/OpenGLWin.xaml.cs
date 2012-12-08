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
using OpenTK.Input;
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


        public OpenGLWin()
        {
            InitializeComponent();

            xangle = 0;
            yangle = 0;
            zangle = 0;
        }

        public void incAngle(float x, float y, float z){
            xangle+=x;
            yangle+=y;
            zangle+=z;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.glc = new GLControl();
            this.glc.Load += new EventHandler(glc_Loaded);
            this.glc.Paint += new System.Windows.Forms.PaintEventHandler(glc_Paint);
            this.glc.Resize += new EventHandler(glc_Resize);
            this.host.Child = this.glc;
        }

        void glc_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glc.ClientSize);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //if (glc.ClientSize.Width <= glc.ClientSize.Height)
            //GL.Ortho(-2, -2+4.0*glc.ClientSize.Width/glc.ClientSize.Height,-2,2,-100,100);
            
            float aspect = glc.ClientSize.Width * 1.0f / glc.ClientSize.Height;
            Glu.Perspective(45.0f, aspect, 0, 100);

            //else
            //GL.Ortho(0.0, 16.0 * (float)glc.ClientSize.Width / (float)glc.ClientSize.Height, 0.0, 16.0,
            //        -10.0, 10.0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        void glc_Loaded(object sender, EventArgs e)
        {
            // Make background "chocolate"
            GL.ClearColor(System.Drawing.Color.Chocolate);

            int w = glc.Width;
            int h = glc.Height;

            // Set up initial modes
            float[] ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] position = { 0.0f, 10.0f, 0.0f, 0.0f };

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

            Glu.LookAt(0, 0, 10, 0, 0, 0, 0, 1, 0);

            GL.Rotate(xangle, 1, 0, 0);
            GL.Rotate(yangle, 0, 1, 0);
            GL.Rotate(zangle, 0, 0, 1);

            // Draw a teapot
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            RenderTeapot(0.0f, 0.0f, 0.0f, 0.135f, 0.2225f, 0.1575f,
            0.54f, 0.89f, 0.63f, 0.316228f, 0.316228f, 0.316228f, 0.1f);

            /*GL.Color4(System.Drawing.Color.Red);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex3(0.0f, 1.0f, -5.0f);              // Top
            GL.Vertex3(-1.0f, -1.0f, -5.0f);              // Bottom Left
            GL.Vertex3(0.0f, -0.0f, -5.0f);              // Bottom Right
            GL.End();*/
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

    }
}
