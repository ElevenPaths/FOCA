using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace FOCA.ModifiedComponents
{
    /// <summary>
    /// No elimina el parpadeo del todo pero lo reduce bastante
    /// </summary>
    public class TreeViewNoFlickering : TreeView
    {
        private const int WM_ERASEBKGND = 0x0014;

        public TreeViewNoFlickering()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        [SecurityPermission (SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message messg)
        {
            // turn the erase background message into a null message
            if (WM_ERASEBKGND == messg.Msg) //if message is is erase background
            {
                messg.Msg = (int)0x0000; //reset message to null
            }
            base.WndProc(ref messg);
        }
    }
}
