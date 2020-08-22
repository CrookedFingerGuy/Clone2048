using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.Windows;

namespace Clone2048
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            RForm rForm = new RForm("CrookedFingerGuy's 2048");            

            RenderLoop.Run(rForm, () => rForm.rLoop());

            rForm.Dispose();

        }
    }
}
