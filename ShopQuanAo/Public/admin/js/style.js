
function removeVietnameseTones(str) {

        //Đổi ký tự có dấu thành không dấu
        str = str.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
        str = str.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
        str = str.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
        str = str.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
        str = str.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
        str = str.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
        str = str.replace(/đ/gi, 'd');
        //Xóa các ký tự đặt biệt
        str = str.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
        //Đổi khoảng trắng thành ký tự gạch ngang
        str = str.replace(/ /gi, "-");
        //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
        //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
        str = str.replace(/\-\-\-\-\-/gi, '-');
        str = str.replace(/\-\-\-\-/gi, '-');
        str = str.replace(/\-\-\-/gi, '-');
        str = str.replace(/\-\-/gi, '-');
        //Xóa các ký tự gạch ngang ở đầu và cuối
        str = '@' + str + '@';
        str = str.replace(/\@\-|\-\@|\@/gi, '');

        return str.toLowerCase();
}

var loaisanpham = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $(".btn-active-lsp").off("click").on("click", function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data("id");

            $.ajax({
                url: '/admin/LoaiSanPham/ChangeStatus',
                data: { id: id },
                dataType: 'json',
                type: "post"
            }).done(function (data) {
                // If successful
                if (data.status == true) {
                    btn.children().removeClass("fa-eye");
                    btn.removeClass("bg-success");
                    btn.addClass("bg-danger");
                    //console.log(btn.children("i").removeClass("fa-eye"));
                    btn.children().removeClass("fa-eye-slash");
                    btn.children().addClass("fa-eye-slash");
                    btn.children().html("&nbsp; Active");
                    btn.children().html("&nbsp; Die");
                } else {
                    btn.children().removeClass("fa-eye-slash");
                    btn.removeClass("bg-danger");
                    btn.addClass("bg-success");
                    btn.children().addClass("fa-eye");

                    btn.children().html("&nbsp; Active");
                }
            })
        });

        $(".btn-delete-lsp").off("click").on("click", function (e){
            e.preventDefault();
            var btn = $(this);
            var id = btn.data("id");
            console.log(id);
            Swal.fire({
                title: 'Delete',
                icon: 'error',
                showCancelButton: true,
                confirmButtonText: `Có`,
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/admin/LoaiSanPham/Delete/'+id,
                        dataType: 'json',
                        type: "get"
                    }).done(function (data) {
                        // If successful
                        if (data.status == "success") {
                            Swal.fire("", "", data.status);
                            var row = $(".table-lsp").children("tbody").find("tr#row_"+id);
                            row.remove();
                        } else {
                            Swal.fire("", "", data.status);
                        }
                    })
                } 
            })

           
        })
    }
}

$(document).ready(function () {
    loaisanpham.init();
});


