using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ShimmerAPI
{
    public partial class Orientation3D : Form
    {
        Control PControlForm;
        double Angle, x, y, z;

        public Orientation3D()
        {
            InitializeComponent();
        }

        public void setControl(Control controlForm)
        {
            this.PControlForm = controlForm;
        }

        private void Orientation3D_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(Configuration_FormClosing);
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            int w = glControl.Width;
            int h = glControl.Height;
            OpenTK.Graphics.OpenGL.GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
            OpenTK.Graphics.OpenGL.GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);

            Matrix4 lookat = Matrix4.LookAt(0.0f, 0.0f, -4.0f, 0.0f, 0.0f, -6.0f, 0.0f, 1.0f, 0.0f);
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
            OpenTK.Graphics.OpenGL.GL.LoadMatrix(ref lookat);

            float[] m = new float[16];
            BuildPerspProjMat(m, 45.0f, w / h, 0.01f, 1000.0f);
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
            OpenTK.Graphics.OpenGL.GL.LoadMatrix(m);
        }

        void BuildPerspProjMat(float[] m, float fov, float aspect, float znear, float zfar)
        {
            float xymax = znear * (float)Math.Tan(fov * Math.PI / 360);
            float ymin = -xymax;
            float xmin = -xymax;

            float width = xymax - xmin;
            float height = xymax - ymin;

            float depth = zfar - znear;
            float q = -(zfar + znear) / depth;
            float qn = -2 * (zfar * znear) / depth;

            float w = 2 * znear / width;
            w = w / aspect;
            float h = 2 * znear / height;

            m[0] = w;
            m[1] = 0;
            m[2] = 0;
            m[3] = 0;

            m[4] = 0;
            m[5] = h;
            m[6] = 0;
            m[7] = 0;

            m[8] = 0;
            m[9] = 0;
            m[10] = q;
            m[11] = -1;

            m[12] = 0;
            m[13] = 0;
            m[14] = qn;
            m[15] = 0;
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();
            OpenTK.Graphics.OpenGL.GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);
            OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
            OpenTK.Graphics.OpenGL.GL.LoadIdentity();
            OpenTK.Graphics.OpenGL.GL.Translate(0, 0, -5.0);
            OpenTK.Graphics.OpenGL.GL.Rotate(Angle, x, y, z);
            OpenTK.Graphics.OpenGL.GL.PointSize(3);
            OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.Front, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
            OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.Back, OpenTK.Graphics.OpenGL.PolygonMode.Fill);

            OpenTK.Graphics.OpenGL.GL.Begin(OpenTK.Graphics.OpenGL.PrimitiveType.Quads);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(255, 0, 255));
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, -1.0f);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(0, 255, 255));
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, 1.0f);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(255, 255, 0));
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, -1.0f);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(255, 0, 0));
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, -1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, 1.0f);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(0, 255, 0));
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(-1.0f, 1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, -1.0f);

            OpenTK.Graphics.OpenGL.GL.Color3(Color.FromArgb(0, 0, 255));
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, -1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, 1.0f, 1.0f);
            OpenTK.Graphics.OpenGL.GL.Vertex3(1.0f, -1.0f, 1.0f);

            OpenTK.Graphics.OpenGL.GL.End();

            glControl.SwapBuffers();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.glControl.Invalidate();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            PControlForm.resetTheOrientation();
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            PControlForm.setTheOrientation();
        }

        public void setAxisAngle(double a, double x, double y, double z)
        {
            this.Angle = a;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            PControlForm.ToolStripMenuItemShow3DOrientation.Checked = false;
            this.Hide(); // hide the form instead of closing
            e.Cancel = true; // this cancels the close event.
        }
    }

}
