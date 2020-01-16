using System;
using System.Security.Permissions;
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

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message messg)
        {
            // turn the erase background message into a null message
            if (WM_ERASEBKGND == messg.Msg) //if message is is erase background
            {
                messg.Msg = 0x0000; //reset message to null
            }
            base.WndProc(ref messg);
        }

        public TreeNode GetNode(string navigationPath)
        {
            if (String.IsNullOrWhiteSpace(navigationPath))
                throw new ArgumentNullException(nameof(navigationPath));

            string[] routes = navigationPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode currentNode = this.Nodes[routes[0]];

            for (int i = 1; i < routes.Length; i++)
            {
                currentNode = currentNode.Nodes[routes[i].Trim()];
            }
            return currentNode;
        }
    }
}