using System;
using System.Collections;
using System.Windows.Forms;

namespace FOCA.Search
{
    public class ListViewColumnSorterValues : IComparer
    {
        public int SortColumn { set; get; }
        public SortOrder Order { set; get; }
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorterValues()
        {
            SortColumn = 0;
            Order = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {

            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int compareResult = 0;
            if (SortColumn == 0)
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            else if (SortColumn == 1)
            {
                if (listviewX.SubItems.Count != 2 || listviewY.SubItems.Count != 2)
                    return 0;
                else
                {
                    int xi, yi;
                    if (int.TryParse(listviewX.SubItems[SortColumn].Text, out xi) &&
                        int.TryParse(listviewY.SubItems[SortColumn].Text, out yi))
                    {
                        compareResult = xi - yi;
                    }
                    else
                    {
                        compareResult = string.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
                    }
                }
            }
            if (Order == SortOrder.Ascending)
                return compareResult;
            else if (Order == SortOrder.Descending)
                return -compareResult;
            else
                return 0;
        }
    }
}
