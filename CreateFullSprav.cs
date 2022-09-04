using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace xPosBL.GoodsDirectories.CreateSprav
{
    public class CreateFullSprav : ICreateSprav
    {
        public LineFormation LineForm { get; set; }

        public event EventHandler EventStartCreating;
        public event EventHandler EventEndCreating;
        public event EventHandler<string> EventErrorCreating;

        public void Create(string fileName, DataTable goods)
        {
            LineForm = new LineFormation();
            StreamWriter file = null;
            fileName = Directory.GetCurrentDirectory() + @"\sprav\" + fileName;
            try
            {
                if(File.Exists(fileName))
                    File.Delete(fileName);
                EventStartCreating?.Invoke(this, EventArgs.Empty);
                DataTable dtDeps = SQL.getListDeps();
                DataTable dtGrp = SQL.getListGrp();

                file = new StreamWriter(fileName);

                var grop = from row in goods.AsEnumerable()
                           where row.Field<int>("id_departments") != 6
                           group row by row.Field<int>("grp") into grp
                           select new
                           {
                               grpID = grp.Key,
                           };

                file.WriteLine("##@@&&");
                file.WriteLine("#");
                file.WriteLine("$$$DELETEALLWARES");
                file.WriteLine("$$$DELETEALLASPECTREMAINS");
                file.WriteLine("$$$ADDQUANTITY");

                foreach (DataRow rDep in dtDeps.Rows)
                {
                    file.WriteLine(LineForm.StringInserGRP(rDep, "id", "name"));
                }

                foreach (var r in grop)
                {
                    DataRow[] row = dtGrp.Select("id = " + r.grpID.ToString());
                    if (row.Count() > 0)
                    {
                        DataRow[] rDeps = dtDeps.Select("id = " + row[0]["id_otdel"].ToString());
                        file.WriteLine(LineForm.StringInserGRP(row[0], "id", "cname", "id") );
                    }
                }

                goods.DefaultView.RowFilter = $"ntypetovar <> 1";
                goods.DefaultView.Sort = "grp ASC, name ASC";

                LineForm.Update();
                foreach (DataRowView row in goods.DefaultView)
                {
                    file.WriteLine(LineForm.StringInserTovar(row));
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
                file.Close();
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            finally
            {
                file.Close();
                EventEndCreating?.Invoke(this, EventArgs.Empty);
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Windows\System32\icacls.exe", Directory.GetCurrentDirectory() + @"\sprav\AIn" + " /grant Все:(F)");
                    Process process = Process.Start(startInfo);
                    process.WaitForExit();
                }
                catch
                {
                }
            }
        }
    }
}
