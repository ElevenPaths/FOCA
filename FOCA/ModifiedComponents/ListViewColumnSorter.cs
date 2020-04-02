using FOCA.Database.Entities;
using System;
using System.Collections;
using System.Windows.Forms;

namespace FOCA.Search
{
    public class ListViewColumnSorter : IComparer
    {
        public int SortColumn { set; get; }
        public SortOrder Order { set; get; }
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            SortColumn = 0;
            Order = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            if (listviewX.Tag == null || listviewY.Tag == null)
                return 0;
            if (SortColumn == 4 /*|| (ColumnToSort == 5)*/)
            {
                //Los ficheros no descargados siempre aparecen al final
                if (!((FilesItem)listviewX.Tag).Downloaded && !((FilesItem)listviewY.Tag).Downloaded)
                    return 0;
                else if (((FilesItem)listviewX.Tag).Downloaded && !((FilesItem)listviewY.Tag).Downloaded)
                    return -1;
                else if (!((FilesItem)listviewX.Tag).Downloaded && ((FilesItem)listviewY.Tag).Downloaded)
                    return 1;
            }
            int compareResult;
            if (SortColumn == 0)
                compareResult = int.Parse(listviewX.Text) - int.Parse(listviewY.Text);
            else if (SortColumn == 4)
            {
                if (((FilesItem)listviewX.Tag).Date == ((FilesItem)listviewY.Tag).Date)
                    compareResult = 0;
                else if (((FilesItem)listviewX.Tag).Date > ((FilesItem)listviewY.Tag).Date)
                    compareResult = 1;
                else
                    compareResult = -1;
            }
            else if (SortColumn == 5)
            {
                compareResult = ((FilesItem)listviewX.Tag).Size - ((FilesItem)listviewY.Tag).Size;
            }
            else if (SortColumn == 8)
            {
                if (listviewX.SubItems[8].Text == "-" && listviewY.SubItems[8].Text == "-")
                    return 0;
                else if (listviewX.SubItems[8].Text != "-" && listviewY.SubItems[8].Text == "-")
                    return -1;
                else if (listviewX.SubItems[8].Text == "-" && listviewY.SubItems[8].Text != "-")
                    return 1;
                else
                {
                    DateTime d1 = DateTime.Parse(listviewX.SubItems[8].Text);
                    DateTime d2 = DateTime.Parse(listviewY.SubItems[8].Text);
                    if (d1 == d2)
                        compareResult = 0;
                    else if (d1 > d2)
                        compareResult = 1;
                    else
                        compareResult = -1;
                }

            }
            else
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            if (Order == SortOrder.Ascending)
                return compareResult;
            else if (Order == SortOrder.Descending)
                return -compareResult;
            else
                return 0;
        }
    }
}
