using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using pwc_mongo_helper;
using Model;
namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        IMongoHelper<Document> imh;
        protected void Page_Load(object sender, EventArgs e)
        {
            imh = new MongoHelperFromEntity<Document>("Documents");
            #region Insert
            //for (int i = 1; i < 11; i++)
            //{
            //    if (imh.Insert(new Document("document" + i.ToString())) == null)
            //    {
            //        Response.Write("<script>alert('err')</script>");
            //        break;
            //    }
            //}
            //Response.Write("<script>alert('ok')</script>");
            #endregion
            #region Push
            //if (imh.Push("568231e7d6f684683c253f6a", d => d.Comment, new Comment("CN002", "Hi")))
            //{
            //    Response.Write("<script>alert('ok')</script>");
            //}
            //else
            //{
            //    Response.Write("<script>alert('err')</script>");
            //}
            #endregion
            #region Pull
            //if (imh.Pull<Comment>("568231e7d6f684683c253f6a", d => d.Comment, c => c.StaffID == "CN001"))
            //{
            //    Response.Write("<script>alert('ok')</script>");
            //}
            //else
            //{
            //    Response.Write("<script>alert('err')</script>");
            //}
            #endregion
            #region FindOne
            //Document doc = imh.FindOne("56822480d6f68424c8db56f2").Result;
            #endregion
            #region Find
            //string[] fields = { "Title" };
            //Dictionary<string, int> dicfields = new Dictionary<string, int>();
            //dicfields.Add("Time", 0);
            //List<Document> uu = imh.Find(fields, dicfields, u => u._id == "568231e7d6f684683c253f73").Result;
            #endregion
            #region Replace
            //if (imh.Replace("568231e7d6f684683c253f6a", new Document("document_replace")))
            //{
            //    Response.Write("<script>alert('ok')</script>");
            //}
            //else
            //{
            //    Response.Write("<script>alert('err')</script>");
            //}
            #endregion
            #region Update
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("Title", "document_update");
            //dic.Add("Time", DateTime.Now);
            //if (imh.Update("568231e7d6f684683c253f73", dic))
            //{
            //    Response.Write("<script>alert('ok')</script>");
            //}
            //else
            //{
            //    Response.Write("<script>alert('err')</script>");
            //}
            #endregion
            #region Delete
            if (imh.Delete("568231e7d6f684683c253f73"))
            {
                Response.Write("<script>alert('ok')</script>");
            }
            else
            {
                Response.Write("<script>alert('err')</script>");
            }
            #endregion
            //要注意的地方
            //1.是lambda准确

            //2.在构造对象时，如果这个对象是一个collection中的document时，一定要添加属性"_id"，例如Model中Document中所示，在Insert时，_id不用赋值，后台会赋值，但是所用名称一定按照如下所示
            //private object id;
            //public object _id
            //{
            //    get { return id; }
            //    set { id = value; }
            //}

            //3.当对象中有List行属性时一定要付初值例如Model中Document中所示
            //private List<Comment> _Comment ;
            //public List<Comment> Comment
            //{
            //    get {
            //        if (_Comment == null) { _Comment = new List<Comment>(); }
            //        return _Comment; 
            //    }
            //    set { _Comment = value; }
            //}

            //4.Pull操作中后面的对象为要插入到List中的对象

            //5.Web.config中mongodbConnection为MongoServer地址，我现在里面写的是DEV的地址，可以使用；mongodbDatabase 为数据库名称，可以改成想要的
        }
    }
}