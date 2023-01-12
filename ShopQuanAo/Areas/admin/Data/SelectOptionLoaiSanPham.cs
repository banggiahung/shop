using Models.DAL;
using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopQuanAo.Areas.admin.Data
{
    public class SelectOptionLoaiSanPham
    {
      
        public int ID_LOAI_SP { get; set; }

        public string TEN_LOAI_SP { get; set; }

        public string SLUG { get; set; }

        public bool? TRANG_THAI { get; set; }

        public int? ID_CHA { get; set; }

        public bool? IS_REMOVE { get; set; }

        public DateTime? NGAY_TAO { get; set; }
    }

    public class DeQuySelectOption
    {
        public List<SelectOptionLoaiSanPham> listDanhSach;
        public DeQuySelectOption()
        {
            listDanhSach = new List<SelectOptionLoaiSanPham>();
            this.setListDanhSach();
        }
        public void setListDanhSach()
        {
            var listLSP = new DAL_LoaiSanPham().getDanhSach();
            foreach(var item in listLSP)
            {
                var option = new SelectOptionLoaiSanPham();
                option.ID_LOAI_SP = item.ID_LOAI_SP;
                option.ID_CHA = item.ID_CHA;
                option.TEN_LOAI_SP = item.TEN_LOAI_SP;
                option.SLUG = item.SLUG;
                option.TRANG_THAI = item.TRANG_THAI;
                option.IS_REMOVE = item.IS_REMOVE;
                this.listDanhSach.Add(option);
            }
        }

        public List<SelectOptionLoaiSanPham> ListSelectOption()
        {
            var list_dm = this.listDanhSach;
            var list_option = new List<SelectOptionLoaiSanPham>();
            list_option = this.deQuyOptionDanhMuc(this.listDanhSach, list_option, 0);
            return list_option;

        }
        public List<SelectOptionLoaiSanPham> deQuyOptionDanhMuc(List<SelectOptionLoaiSanPham> list, List<SelectOptionLoaiSanPham> list_option, int parentID = 0, string str = "")
        {

            foreach (var item in list)
            {
                if (item.ID_CHA == parentID)
                {

                    item.TEN_LOAI_SP = str + " " + item.TEN_LOAI_SP;
                    list_option.Add(item);
                    this.deQuyOptionDanhMuc(list, list_option, item.ID_LOAI_SP, string.Concat(str, "--"));
                }
            }
            return list_option;
        }

    }

    public class DeQuyCategory
    {
        public List<LOAI_SAN_PHAM> listDanhSach;
        public DeQuyCategory()
        {
            listDanhSach = new DAL_LoaiSanPham().getDanhSach();
        }
        public List<LOAI_SAN_PHAM> getChildenLoaiSanPham(List<LOAI_SAN_PHAM> listLSP, int id = 0)
        {
            foreach (var item in this.listDanhSach)
            {
                
                if (item.ID_CHA == id)
                { 
                    listLSP.Add(item);
                    this.getChildenLoaiSanPham(listLSP, item.ID_LOAI_SP);
                }
            }
            return listLSP;
        }

        public List<LOAI_SAN_PHAM> getParentLoaiSanPham(List<LOAI_SAN_PHAM> listLSP , int parent_id)
        {
            foreach (var item in this.listDanhSach)
            {
                if (item.ID_LOAI_SP == parent_id)
                {
                    listLSP.Add(item);
                    
                    if(item.ID_CHA == 0)
                    {
                        return listLSP;
                    }
                    this.getParentLoaiSanPham(listLSP, item.ID_CHA??0);
                }
            }
            return listLSP;
        }
    }
}

