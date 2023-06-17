using System;
using System.Drawing;
using System.Windows.Forms;

public class Form1 : Form
{
    private const int screenWidth = 640;
    private const int screenHeight = 480;

    private Bitmap screenBuffer;
    private Graphics screenGraphics;

    private bool[,] sandGrid;

    public Form1()
    {
        InitializeComponents();
        InitializeSimulation();
    }

    private void InitializeComponents()
    {
        this.Text = "Sand Simulation";
        this.ClientSize = new Size(screenWidth, screenHeight);
        this.Paint += SandSimulation_Paint;
        this.MouseMove += SandSimulation_MouseMove;
    }

    private void InitializeSimulation()
    {
        screenBuffer = new Bitmap(screenWidth, screenHeight);
        screenGraphics = Graphics.FromImage(screenBuffer);

        sandGrid = new bool[screenWidth, screenHeight];

        this.DoubleBuffered = true;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
    }

    private void SandSimulation_Paint(object sender, PaintEventArgs e)
    {
        UpdateSimulation();

        screenGraphics.Clear(Color.Black);

        using (var brush = new SolidBrush(Color.Yellow))
        {
            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    if (sandGrid[x, y])
                    {
                        screenGraphics.FillRectangle(brush, x, y, 1, 1);
                    }
                }
            }
        }

        e.Graphics.DrawImage(screenBuffer, 0, 0);
    }

    private void SandSimulation_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            int x = e.X;
            int y = e.Y;

            if (x >= 0 && x < screenWidth && y >= 0 && y < screenHeight)
            {
                sandGrid[x, y] = true;
            }
        }

        this.Invalidate();
    }

    private void UpdateSimulation()
    {
        bool[,] newSandGrid = new bool[screenWidth, screenHeight];

        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                if (sandGrid[x, y])
                {
                    if (y < screenHeight - 1 && !sandGrid[x, y + 1] && !newSandGrid[x, y + 1])
                    {
                        newSandGrid[x, y + 1] = true;
                    }
                    else if (x > 0 && y < screenHeight - 1 && !sandGrid[x - 1, y + 1] && !newSandGrid[x - 1, y + 1])
                    {
                        newSandGrid[x - 1, y + 1] = true;
                    }
                    else if (x < screenWidth - 1 && y < screenHeight - 1 && !sandGrid[x + 1, y + 1] && !newSandGrid[x + 1, y + 1])
                    {
                        newSandGrid[x + 1, y + 1] = true;
                    }
                    else
                    {
                        newSandGrid[x, y] = true;
                    }
                }
            }
        }

        sandGrid = newSandGrid;
    }

    protected override void OnClosed(EventArgs e)
    {
        screenGraphics.Dispose();
        screenBuffer.Dispose();
        base.OnClosed(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Hintergrund nicht zeichnen, um Flackern zu vermeiden
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        this.Invalidate();
    }



}
