

jQuery(document).ready(function ($) {
    //reset form 
    $(".btn-reset").off("click").on("click", function (e) {
        $(".validate-ten-size").html("");
        $(".validate-mo-ta").html("");
        $(".validate-slug").html("");
        $("#id").val("");
        $("#ten-size").val("");
        $("#slug").val("");
        $("#size").val("#ff0000");
    })

    // add_size
    $(".btn-add-size").off("click").on("click", function (e) {
        var tensize = $("#ten-size").val();
        var slug = $("#slug").val();
        var mota = $("#mo-ta").val();
        if (ValidateForm()) {
            return;
        }
        if (check_isset_size_insert("slug", slug)) {
            $(".validate-ten-size").html("Tên Size đã tồn tại");
            return;
        } else {
            $(".validate-ten-size").html("");
        }
       

        $.ajax({
            url: '/admin/size/create',
            type: 'POST',
            dataType: 'json',
            data: {
                tenSize: tensize,
                Slug: slug,
                MoTa: mota
            },
            success: function (value) {
                if (value.status == "success") {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,
                        preConfirm: () => {
                            location.href = "/admin/size";
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


    $('.btn-edit-size').off("click").on("click", function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        $.ajax({
            url: '/admin/size/edit/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.value != null) {
                    $("#id").val(data.value["ID"]);
                    $("#ten-size").val(data.value["TEN_SIZE"]);
                    $("#slug").val(data.value["SLUG"]);
                    $("#mo-ta").val(data.value["MO_TA"]);
                }
            }
        })

    });


    /// update size :
    $(".btn-update-size").off("click").on("click", function (e) {
        e.preventDefault();

        var id = $("#id").val();
        var tensize = $("#ten-size").val();
        var slug = $("#slug").val();
        var mota = $("#mo-ta").val();
        if (id == "") {
            Swal.fire({
                icon: "warning",
                text: "Mời bạn chọn size cần sửa"
            })
            return;
        }
        if (ValidateForm()) {
            return;
        }
        if (check_isset_size_update(id, "slug", slug)) {
            $(".validate-ten-size").html("Tên size đã tồn tại");
            return;
        } else {
            $(".validate-ten-size").html("");
        }
  
        $.ajax({
            url: '/admin/size/edit/' + id,
            type: 'POST',
            dataType: 'json',
            data: {
                tenSize: tensize,
                Slug: slug,
                MoTa: mota
            },
            success: function (value) {
                if (value.status == "success") {
                    Swal.fire({
                        icon: value.status,
                        text: value.text,
                        preConfirm: () => {
                            location.href = "/admin/size";
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
    $(".btn-active-size").off("click").on("click", function (e) {
        e.preventDefault();
        var btn = $(this);
        var id = btn.data("id");

        $.ajax({
            url: '/admin/size/ChangeStatus',
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


    //Remove size:
    $(".btn-delete-size").off("click").on("click", function (e) {
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
                    url: '/admin/size/Delete/' + id,
                    dataType: 'json',
                    type: "get"
                }).done(function (data) {
                    // If successful
                    if (data.status == "success") {
                        Swal.fire({
                            icon: data.status,
                            preConfirm: () => {
                                location.href = "/admin/size";
                            }
                        })
                    } else {
                        Swal.fire("", "", data.status);
                    }
                })
            }
        })
    });

    $("#ten-size").on("change", function (e) {
        var str = removeVietnameseTones($(this).val());
        $("#slug").val(str);
    });
});

function check_isset_size_insert(key, str) {
    var check = false;
    $.ajax({
        url: '/admin/size/ValidateInsert',
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

function check_isset_size_update(id, key, str) {
    var check = false;
    $.ajax({
        url: '/admin/size/ValidateEdit/' + id,
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
    var tensize = $("#ten-size").val();
    var slug = $("#slug").val();
    var mota = $("#mo-ta").val();
    if (tensize.length > 15 || tensize.length <= 0) {
        $(".validate-ten-size").html("Tên Size phải có độ dài ( 1 -> 15) ký tự");
        return true;
    }
    $(".validate-ten-size").html("");
    if (slug.length > 20 || slug.length <= 0) {
        $(".validate-slug").html("Slug phải có độ dài ( 1 -> 20) ký tự");
        return true;
    }
    $(".validate-slug").html("");

    if (mota.trim().length < 1 || mota.trim().length > 255) {
        $(".validate-mo-ta").html("Mô tả phải có độ dài ( 1 -> 250) ký tự");
        return true;
    }
    $(".validate-mo-ta").html("");
    return false;

}