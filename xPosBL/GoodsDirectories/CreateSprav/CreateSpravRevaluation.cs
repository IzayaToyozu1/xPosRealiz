using System;
using System.Data;
using System.Linq;
using System.IO;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    public class CreateSpravRevaluation : ICreateSprav<int[]>
    {
        public LineFormation LineForm { get; set; }

        public event EventHandler EventStartCreating;
        public event EventHandler EventEndCreating;
        public event EventHandler<string> EventErrorCreating;

        public int[] Create(string fileName, DataTable goods)
        {
            LineForm = new LineFormation();
            EventStartCreating?.Invoke(this, EventArgs.Empty);
            int[] idTovars = new int[1];
            LineForm.listPromoGoods.Clear();
            LineForm.listIDPromoGoods.Clear();

            StreamWriter file = null;

            string filePath = Directory.GetCurrentDirectory() + @"\sprav\" + fileName;
            if (File.Exists(filePath))
                File.Delete(Directory.GetCurrentDirectory() + @"\sprav\" + fileName);

            if (!Directory.Exists(@"sprav")) Directory.CreateDirectory(@"sprav");
            DataTable dtDeps = SQL.getListDeps();
            DataTable dtGrp = SQL.getListGrp();
            try
            {
                file = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\sprav\" + fileName);

                var grop = from row in goods.AsEnumerable()
                           group row by row.Field<int>("grp") into grp
                           select new
                           {
                               grpID = grp.Key,
                           };

                file.WriteLine("##@@&&");
                file.WriteLine("#");
                file.WriteLine("$$$DELETEWARESBYWARECODE");
                goods.DefaultView.Sort = "grp ASC, name ASC";
                goods.DefaultView.RowFilter = "ntypetovar = 1";
                foreach(DataRowView row in goods.DefaultView)
                {
                    file.WriteLine(row["id_tovar"].ToString());
                }
                file.WriteLine("\n$$$ADDQUANTITY");

                foreach (DataRow rDep in dtDeps.Rows)
                {
                    file.WriteLine(LineForm.StringInserGRP(rDep, "id", "name"));
                }

                int indexRow = 0;
                foreach (var r in grop)
                {
                    DataRow[] row = dtGrp.Select("id = " + r.grpID.ToString());
                    if (row.Count() > 0)
                    {
                        DataRow[] rDeps = dtDeps.Select("id = " + row[0]["id_otdel"].ToString());
                        file.WriteLine(LineForm.StringInserGRP(row[0], "id", "cname", "id"));
                    }
                }

                goods.DefaultView.Sort = "grp ASC, name ASC";
                goods.DefaultView.RowFilter = "ntypetovar <> 1";
                idTovars = new int[goods.Rows.Count];

                foreach (DataRowView row in goods.DefaultView)
                {
                    file.WriteLine(LineForm.StringInserTovar(row));
                    int id_tovar = Convert.ToInt32(row["id_goodsUpdate"].ToString());
                    idTovars[indexRow] = id_tovar;
                    indexRow++;
                }

                file.WriteLine("$$$DELETEWARESBYWARECODE");
                goods.DefaultView.Sort = "grp ASC, name ASC";
                goods.DefaultView.RowFilter = "ntypetovar = 1";

                foreach (DataRowView row in goods.DefaultView)
                {
                    file.WriteLine(row["id_tovar"].ToString());
                }

                if (LineForm.listPromoGoods.Count > 0)
                {
                    file.WriteLine("");
                    file.WriteLine("");
                    file.WriteLine("");
                    file.WriteLine("$$$ADDASPECTREMAINS");
                    foreach (string str in LineForm.listPromoGoods)
                    {
                        file.WriteLine($"{str}");
                    }
                }
            }
            catch(Exception e)
            {
                EventErrorCreating?.Invoke(this, e.Message);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
                EventEndCreating?.Invoke(this, EventArgs.Empty);
                
            }
            return idTovars;
        }

        void ICreateSprav.Create(string fileName, DataTable goods)
        {

        }
    }
}
