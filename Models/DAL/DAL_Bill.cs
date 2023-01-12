using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EntityFramework;
using Models.ADO;
using System.Data.SqlClient;

namespace Models.DAL
{
    public class DAL_Bill
    {
        private ShopQuanAoDBContext context = null;

        public DAL_Bill()
        {
            this.context = new ShopQuanAoDBContext();
        }

        //phân trang hóa đơn
        public List<ModelBills> GetPageBills(string str_search , int page, int itemsPerPage, out int totalCount)
        {
            var list = new List<ModelBills>();

            if (!string.IsNullOrEmpty(str_search))
            {
                var arrBill = this.context.HOA_DON.SqlQuery("Select * from Hoa_Don where "+str_search ).OrderByDescending(c=>c.MA_HD).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
                foreach (var item in arrBill)
                {
                    list.Add(this.ConvertModelBill(item));
                }
                totalCount = this.context.HOA_DON.SqlQuery("Select * from Hoa_Don where " + str_search).Count();
            }
            else
            {
                var arrBill = this.context.HOA_DON.Where(a => a.IS_REMOVE == false).OrderByDescending(c => c.MA_HD).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
                foreach (var item in arrBill)
                {
                    list.Add(this.ConvertModelBill(item));
                }
                totalCount = this.context.HOA_DON.Count();
            }
            
            return list;
        }
        //chuyển đổi hóa đơn: thành hóa đơn và chi tiết hóa đơn
        public ModelBills ConvertModelBill(HOA_DON hd)
        {
            var bill = new ModelBills();
            var listDetailsBill = new DAL_DetailsBill().getListDetailsBill(hd.MA_HD);
            bill.MA_HD = hd.MA_HD;
          
            bill.NGAY_MUA = hd.NGAY_MUA;
            bill.GHI_CHU = hd.GHI_CHU; 
            bill.DIA_CHI_NHAN = hd.DIA_CHI_NHAN;
            bill.SDT_NHAN = hd.SDT_NHAN;
            bill.TEN_NHAN_HANG = hd.TEN_NHAN_HANG;
            bill.TONG_TIEN = (hd.TONG_TIEN ?? 0);
            bill.TRANG_THAI = hd.TRANG_THAI;
            bill.TenStatus = this.context.STATUS_HOA_DON.Find(hd.TRANG_THAI).STATUS_ORDER;
            var listDetalis = new List<ModelDetailsBill>();
            foreach(var item in listDetailsBill)
            {
                var details = new ModelDetailsBill();
                var spct = new DAL_SanPhamChiTiet().returnSPCT(item.ID_SP_CT);
                details.SL_MUA = (item.SL_MUA ?? 0);
                var sanpham = this.context.SAN_PHAM.Where( c=> c.MA_SP == spct.MA_SP ).FirstOrDefault();
                var color = this.context.COLORs.Find(spct.ID_COLOR);
                var size = this.context.SIZEs.Find(spct.ID_SIZE);
                details.Images = (sanpham != null) ?sanpham.LINK_ANH_CHINH : "";
                details.tenColor = (color != null) ? color.TEN_MAU : "";
                details.tenSize = (size != null) ? size.TEN_SIZE : "";
                details.SP_SIZE_MAU =item.SP_SIZE_MAU;
                details.tenSP = (sanpham != null) ? sanpham.TEN_SP : "";
                details.ID = item.ID;
                details.GIA_BAN = (item.GIA_BAN ?? 0);
                details.DON_VI_TINH = item.DON_VI_TINH;
                details.ID_HD = item.ID_HD;
                details.ID_SP_CT = item.ID_SP_CT;
                listDetalis.Add(details);
            }
            bill.listDetalisBill = listDetalis;
            return bill;
        }

        public HOA_DON returnBills(int id)
        {
            return this.context.HOA_DON.Find(id);
        }

        public bool InsertBill (HOA_DON hd , out int id)
        {
            try
            {
                hd.IS_REMOVE = false;
                hd.NGAY_MUA = DateTime.Now;
                var hoadon = this.context.HOA_DON.Add(hd);
                this.context.SaveChanges();
                id = (hoadon.MA_HD!=null) ? hoadon.MA_HD : 0;
                return true;
            }
            catch
            {
                id = 0;
                return false;
            }
        }

        public bool DeleteBill(int id)
        {
            try
            {
                var hoadon = this.context.HOA_DON.Find(id);

                if(hoadon!= null)
                {
                    List<CHI_TIET_HOA_DON> list = this.context.CHI_TIET_HOA_DON.Where(a=>a.ID_HD == id).ToList();
                    if (list.Count > 0)
                    {
                        foreach(CHI_TIET_HOA_DON item in list)
                        {
                            item.IS_REMOVE = false;
                            this.context.SaveChanges();
                        }
                    }
                    hoadon.IS_REMOVE = false;
                    this.context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void UpdateQuantityProductDeleteBill(int mahd)
        {
            var list_cthd = this.context.CHI_TIET_HOA_DON.Where(a => a.ID_HD == mahd).ToList();
            if (list_cthd != null)
            {
                var DAL_SPCT = new DAL_SanPhamChiTiet();
                foreach(var item in list_cthd)
                {
                    var spct = new SAN_PHAM_CHI_TIET();
                    spct = DAL_SPCT.returnSPCT(item.ID_SP_CT);
                    spct.SO_LUONG = spct.SO_LUONG + item.SL_MUA;
                    DAL_SPCT.updateSPCT(item.ID_SP_CT, spct);
                }
            }
        }

        public void UpdateQuantityProductInsertOrder(int mahd)
        {
            var DAL_SPCT = new DAL_SanPhamChiTiet();
            var list_cthd = this.context.CHI_TIET_HOA_DON.Where(a => a.ID_HD == mahd).ToList();
            if (list_cthd != null)
            {
                foreach (var item in list_cthd)
                {
                    var spct = new SAN_PHAM_CHI_TIET();
                    spct = DAL_SPCT.returnSPCT(item.ID_SP_CT);
                    spct.SO_LUONG = spct.SO_LUONG - item.SL_MUA;
                    DAL_SPCT.updateSPCT(item.ID_SP_CT, spct);
                }
                
            }
        }

        public bool CheckSoLuongConVaSoLuongMua( int idspct , int sl_mua )
        {
            var sp = this.context.SAN_PHAM_CHI_TIET.Find(idspct);
            if(sp!= null)
            {
                if(sp.SO_LUONG < sl_mua)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateStatus(int id , int Status)
        {
            try
            {
                var hd = this.context.HOA_DON.Find(id);
                hd.TRANG_THAI = Status;
                this.context.SaveChanges();
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            
        }

        public bool ChangeStatus(int id , int status)
        {

            var hd = this.context.HOA_DON.Find(id);


            if (hd != null)
            {
                bool check = false;
                var listspct = this.context.CHI_TIET_HOA_DON.Where(a => a.ID_HD == hd.MA_HD).ToList();
                foreach (var item in listspct)
                {
                    check = CheckSoLuongConVaSoLuongMua(item.ID_SP_CT, (item.SL_MUA ?? 0));
                }
                if (hd.TRANG_THAI == 2)
                {
                    if (status == 3)
                    {
                        UpdateStatus(id, status);
                       
                        return true;
                    }
                    else if (status == 4)
                    {
                        UpdateQuantityProductDeleteBill(hd.MA_HD);
                        UpdateStatus(id, status);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (hd.TRANG_THAI == 3)
                {
                    return true;
                }
                else
                {
                    if (status == 4)
                    {
                        hd.TRANG_THAI = status;
                        UpdateStatus(id, status);
                    }
                    else if (status == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (check == false)
                        {
                            return false;
                        }
                        UpdateQuantityProductInsertOrder(hd.MA_HD);
                        UpdateStatus(id, status);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
