

jQuery(document).ready(function ($) {
    //reset form 
    $(".btn-reset").off("click").on("click", function (e) {
        $(".validate-ten-mau").html("");
        $(".validate-ma-mau").html("");
        $(".validate-slug").html("");
        $("#id").val("");
        $("#ten-mau").val("");
        $("#slug").val("");
        $("#color").val("#ff0000");
    })

    // add_color
    $(".btn-add-color").off("click").on("click", function (e) {
        var tenColor = $("#ten-mau").val();
        var slug = $("#slug").val();
        var maColor = $("#color").val();
        if (ValidateForm()) {
            return;
        }
        if (check_isset_color_insert("SLUG", slug)) {
            $(".validate-ten-mau").html("Tên màu đã tồn tại");
            return;
        } else {
            $(".validate-ten-mau").html("");
        }
        if (check_isset_color_insert("MA_MAU", maColor)) {
            $(".validate-ma-mau").html("Mã màu đã tồn tại");
            return;
        } else {
            $(".validate-ma-mau").html("");
        }
       
        $.ajax({
            url: '/admin/color/create',
            type: 'POST',
            dataType: 'json',
            data: {
                TenMau: tenColor,
                Slug: slug,
                MaMau: maColor
            },
            success: function (value) {
                if (value.status == "success") {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,
                        preConfirm: () => {
                            location.href = "/admin/color";
                        }
                    })
                } else {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,
                        
                    })
                }
                
            }
        })
    });

    
    $('.btn-edit-color').off("click").on("click", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            url: '/admin/color/edit/'+id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.value != null) {
                    $("#id").val(data.value["ID"]);
                    $("#ten-mau").val(data.value["TEN_MAU"]);
                    $("#slug").val(data.value["SLUG"]);
                    $("#color").val(data.value["MA_MAU"]);
                }
            }
        })

    });


    /// update color :
    $(".btn-update-color").off("click").on("click", function (e) {
        e.preventDefault();
      
        var id = $("#id").val();
        var tenColor = $("#ten-mau").val();
        var slug = $("#slug").val();
        var maColor = $("#color").val();
        if (id == "") {
            Swal.fire({
                icon: "warning",
                text: "Mời bạn chọn color cần sửa"
            })
            return;
        }
        if (ValidateForm()) {
            return;
        }
        if (check_isset_color_update(id, "SLUG", slug)) {
            $(".validate-ten-mau").html("Tên màu đã tồn tại");
            return;
        } else {
            $(".validate-ten-mau").html("");
        }
        if (check_isset_color_update(id, "MA_MAU", maColor)) {
            $(".validate-ma-mau").html("Mã màu đã tồn tại");
            return;
        } else {
            $(".validate-ma-mau").html("");
        }

        $.ajax({
            url: '/admin/color/edit/'+id,
            type: 'POST',
            dataType: 'json',
            data: {
                TenMau: tenColor,
                Slug: slug,
                MaMau: maColor
            },
            success: function (value) {
                if (value.status == "success") {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,
                        preConfirm: () => {
                            location.href = "/admin/color";
                        }
                    })
                } else {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,

                    })
                }

            }
        })
    })


    // change-status 
    $(".btn-active-color").off("click").on("click", function (e) {
        e.preventDefault();
        var btn = $(this);
        var id = btn.data("id");

        $.ajax({
            url: '/admin/color/ChangeStatus',
            data: { id: id },
            dataType: 'json',

            type: "post"
        }).done(function (data) {
            // If successful
            if (data.status == true) {
                btn.children().removeClass("fa-eye");
                btn.removeClass("badge-success");
                btn.addClass("badge-danger");
                //console.log(btn.children("i").removeClass("fa-eye"));
                btn.children().removeClass("fa-eye-slash");
                btn.children().addClass("fa-eye-slash");
                btn.children().html("&nbsp; Active");
                btn.children().html("&nbsp; Die");
            } else {
                btn.children().removeClass("fa-eye-slash");
                btn.removeClass("badge-danger");
                btn.addClass("badge-success");
                btn.children().addClass("fa-eye");

                btn.children().html("&nbsp; Active");
            }
        })
    });


    //Remove Color:
    $(".btn-delete-color").off("click").on("click", function (e) {
        e.preventDefault();
        var btn = $(this);
        var id = btn.data("id");
        console.log(id);
        Swal.fire({
            title: 'Delete',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: `Có`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                $.ajax({
                    url: '/admin/Color/Delete/' + id,
                    dataType: 'json',
                    type: "get"
                }).done(function (data) {
                    // If successful
                    if (data.status == "success") {
                        Swal.fire({
                            icon: data.status,
                            preConfirm: () => {
                                location.href = "/admin/color";
                            }
                        })
                    } else {
                        Swal.fire("", "", data.status);
                    }
                })
            }
        })
    });

    $("#ten-mau").on("change", function (e) {
        var str = removeVietnameseTones($(this).val());
        $("#slug").val(str);
    });
});

function check_isset_color_insert( key , str) {
    var check = false;
    $.ajax({
        url: '/admin/color/ValidateInsert',
        type: 'POST',
        data: {
            key: key,
            value : str
        },
        dataType: 'json',
        async: false,
        success: function (value) {
            check = value.status;
        }
    });
    return check;
}

function check_isset_color_update(id , key, str) {
    var check = false;
    $.ajax({
        url: '/admin/color/ValidateEdit/'+id,
        type: 'POST',
        data: {
            key: key,
            value: str
        },
        dataType: 'json',
        async: false,
        success: function (value) {
            check = value.status;
        }
    });
    return check;
}

function ValidateForm() {
    var tenColor = $("#ten-mau").val();
    var slug = $("#slug").val();
    var maColor = $("#color").val();
    if (tenColor.length > 15 || tenColor.length <= 0) {
        $(".validate-ten-mau").html("Tên màu phải có độ dài ( 1 -> 15) ký tự");
        return true;
    }
    if (slug.length > 20 || slug.length <= 0) {
        $(".validate-slug").html("Slug phải có độ dài ( 1 -> 20) ký tự");
        return true;
    }
    $(".validate-slug").html("");
    return false;

}