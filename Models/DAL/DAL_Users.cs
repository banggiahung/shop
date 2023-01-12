using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL
{
    public class DAL_Users
    {
        protected ShopQuanAoDBContext context { get; set; }

        public DAL_Users()
        {
            //khởi tạo thể hiện của csdl
            this.context = new ShopQuanAoDBContext();
        }
       
        //phương thức phân trang và lấy dữ liệu hiển thị
        public List<ACOUNT> GetPageUsers(int page, int itemsPerPage, out int totalCount)
        {
            var list = new List<ACOUNT>();
            var arrUser = this.context.ACOUNTs.Where(a => a.IS_REMOVE == false).OrderBy(c => c.NGAY_DANG_KY).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            foreach (var item in arrUser)
            {
                list.Add(item);
            }
            totalCount = this.context.ACOUNTs.Count();
            return list;
        }
        //lấy chuỗi dữ liệu đầu tiên
        public bool sqlQueryFristOrDefault(string sql)
        {
            var kh = this.context.ACOUNTs.SqlQuery(sql).FirstOrDefault();
            if (kh != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //thêm mới user
        public bool InsertUsers(ACOUNT user)
        {
            try
            {
                
                user.NGAY_DANG_KY = DateTime.Now;
                //thêm mới user
                this.context.ACOUNTs.Add(user);
                this.context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
      
        //câp nhật user
        public bool EditUsers(int id, ACOUNT user)
        {
            try
            {
                //tìm kiếm user
                var acount = this.context.ACOUNTs.Find(id);
                if (acount != null)
                {
                    acount.PHONE = user.PHONE;
                    acount.DIA_CHI = user.DIA_CHI;
                    acount.EMAIL = user.EMAIL;
                    acount.HO_TEN = user.HO_TEN;
                    acount.LINK_ANH = user.LINK_ANH;
                    if(user.MAT_KHAU != null)
                    {
                        acount.MAT_KHAU =user.MAT_KHAU;
                    }
                    acount.TRANG_THAI = user.TRANG_THAI;
                    //save dữ liệu
                    this.context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        //lấy một user theo id
        public ACOUNT getUser(int id)
        {
            return this.context.ACOUNTs.Find(id);
        }
        //lấy một user theo email
        public ACOUNT getAcountEmail( string email)
        {
            return this.context.ACOUNTs.SqlQuery("Select * from Acount where Email = '"+email+ "' and IS_REMOVE = 0;").FirstOrDefault();
        }
        //xóa user theo id
        public bool DeleteUser(int id)
        {
            var user = this.context.ACOUNTs.Find(id);
            if (user != null)
            {
                user.IS_REMOVE = true;
                this.context.SaveChanges();
            }
            return true;
        }
    }
}
