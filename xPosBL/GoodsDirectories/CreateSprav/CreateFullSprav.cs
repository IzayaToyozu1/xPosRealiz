using System;
using System.Collections.Generic;
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
            fileName = fileName + "_NotWork";
            bool isVvo = fileName.Contains("_VVO");
            LineForm = new LineFormation();
            StreamWriter file = null;
            fileName = Directory.GetCurrentDirectory() + @"\sprav\" + fileName;
            try
            {
                EventStartCreating?.Invoke(this, EventArgs.Empty);
                DataTable dtDeps = SQL.getListDeps();
                if (isVvo)
                {
                    dtDeps.Clear();
                }
                DataRow newRow = dtDeps.NewRow();
                newRow["id"] = 6;
                newRow["name"] = "ВВО";
                dtDeps.Rows.Add(newRow);
                DataTable dtGrp = SQL.getListGrp();

                file = new StreamWriter(fileName);

                var grop = from row in goods.AsEnumerable()
                           where (row.Field<int>("id_departments") != 6 && !isVvo) 
                                 || (row.Field<int>("id_departments") == 6 && isVvo)
                           group row by row.Field<int>("grp") into grp
                           select new
                           {
                               grpID = grp.Key,
                           }; 
                var mark = from row in goods.AsEnumerable()
                    //where row.Field<int>("IsCatPromTovar") != -1
                    group row by row.Field<int>("id_post") into post
                    select new
                    {
                        idPost = post.Key,
                    };

                DataTable dtPosts = new DataTable();
                //dtPosts.Columns.Add(new DataColumn("name", typeof(string)));
                //dtPosts.Columns.Add(new DataColumn("numPost", typeof(string)));
                //dtPosts.Columns.Add(new DataColumn("inn", typeof(int)));
                //dtPosts.Columns.Add(new DataColumn("id", typeof(int)));
                if (mark.Any())
                {
                    IEnumerable<DataRow> p = new DataTable().AsEnumerable();
                    foreach (var item in mark)
                    {
                        
                        var post = isVvo ? SQL.GetPostsVvo(item.idPost) : SQL.GetPosts(item.idPost);
                        p = p.AsEnumerable().Union(post.AsEnumerable());
                    }

                    if (p.Any())
                        dtPosts = p.CopyToDataTable();
                    else
                        dtPosts = null;
                }

                file.WriteLine("##@@&&");
                file.WriteLine("#");
                file.WriteLine("$$$DELETEALLAGENTDETAILS\r\n$$$ADDAGENTDETAILS");
                if (dtPosts != null && dtPosts.Rows.Count > 0)
                    foreach (DataRow row in dtPosts.Rows)
                    {
                        string post = LineForm.StringInserPost(row);
                        file.WriteLine(post);
                    }

                file.WriteLine("$$$DELETEALLWARES");
                file.WriteLine("$$$DELETEALLASPECTREMAINS");
                file.WriteLine("$$$ADDQUANTITY");

                foreach (DataRow rDep in dtDeps.Rows)
                {
                    file.WriteLine(LineForm.StringInserGRP(rDep, "id", "name"));
                }

                foreach (var r in grop)
                {
                    DataRow[] row = dtGrp.Select("id = " + r.grpID);
                    if (row.Count() > 0)
                    {
                        DataRow[] rDeps = dtDeps.Select("id = " + row[0]["id_otdel"].ToString());
                        file.WriteLine(LineForm.StringInserGRP(row[0], "id", "cname", rDeps[0][0].ToString()));
                    }
                }

                string filter;
                if (isVvo)
                {
                    filter = $"ntypetovar <> 1";
                }
                else
                {
                    filter = $"ntypetovar <> 1 and id_departments <> 6";
                }


                goods.DefaultView.RowFilter = filter;
                goods.DefaultView.Sort = "grp ASC, name ASC";
                
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
            catch (Exception e)
            {
                EventErrorCreating?.Invoke(this, e.Message);
                file.Close();
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
            finally
            {
                file.Close();
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
    #region Если не будет формироваться полный справочник


    /*
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

                LineForm.kek();
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
    }*/
    #endregion
}
