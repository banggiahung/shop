$(document).ready(function () {
    CKEDITOR.replace("mo-ta-chi-tiet");
    $("#ten-san-pham").change(function (e) {
        e.preventDefault();
        var str = $(this).val();
        $("#slug").val(removeVietnameseTones(str));
    });
    $(document).on('click', '.custom-btn', function () {
        event.preventDefault();
        console.log(1);
        $(this).next().click();
    });
});

function check_file_image(file_img, classError = "") {
    // body...
    var type_image = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
    if (type_image.indexOf(file_img["type"]) < 0) {
        $(classError).html("File ảnh không hợp lệ !!!");
        return false;
    }
    if (file_img["size"] > 2000000) {
        $(classError).html("File ảnh size < 2MB !!!");
        return false;
    }
    
    $(classError).html("");
    return true;
}


jQuery(document).ready(function ($) {
    $('.file-upload-image').off("click").on("click", function (e) {
        e.preventDefault();

     
        var file = $(this).parent().parent().parent().find('.file-upload-default-image');
        file.trigger('click');


    });
    $('.file-upload-default-image').on('change', function () {
        uploadImg($(this));
        $(this).parent().find('.form-control').val($(this).val().replace(/C:\\fakepath\\/i, ''));

    });


});

function uploadImg(el) {
    var file_data = $(el).prop('files')[0];
    var type = file_data.type;
    var fileToLoad = file_data;

    var fileReader = new FileReader();

    fileReader.onload = function (fileLoadedEvent) {
        var srcData = fileLoadedEvent.target.result;

        var newImage = document.createElement('img');

        newImage.src = srcData;
        $(".hien-thi-image-chinh").html(newImage.outerHTML);
    }
    fileReader.readAsDataURL(fileToLoad);

}

// js upload list-ảnh sản phẩm
jQuery(document).ready(function ($) {
    $('.file-upload-list-image').click(function (event) {
        var file = $(this).parent().parent().parent().find('.file-upload-default-list-img');
        file.trigger('click')

    });
    $('.file-upload-default-list-img').on('change', function () {
        $("#hien-thi-image-kem-theo").children('div').remove();
        var fileSelected = $(this).prop('files');
        var str = fileSelected.length + " files";
        $(this).parent().find('.form-control').val(str);
        if (fileSelected.length > 0) {

            for (var i = 0; i < fileSelected.length; i++) {
                var fileToLoad = fileSelected[i];
                var fileReader = new FileReader();
                fileReader.onload = function (fileLoaderEvent) {
                    var srcData = fileLoaderEvent.target.result;
                    var newImage = document.createElement('img');
                    newImage.style.width = "100%";
                    newImage.style.height = "100%";
                    newImage.src = srcData;
                    var div = document.createElement("div");
                    div.setAttribute("class", "col-4 mb-2");

                    div.style.minHeight = "140px";
                    div.style.height = "150px";
                    div.append(newImage);
                    $("#hien-thi-image-kem-theo").append(div.outerHTML);
                }
                fileReader.readAsDataURL(fileToLoad);
            }
        }
    });
});


/// js upload ảnh màu sản phẩm chi tiết 
jQuery(document).ready(function ($) {
    


    let regExp = /[0-9a-zA-Z\^\&\'\@\{\}\[\]\,\$\=\!\-\#\(\)\.\%\+\~\_ ]+$/;
  /*  $(document).on('click', '.custom-btn', function () {
        event.preventDefault();
        console.log(1);
        $(this).next().click();
    });*/
    $(document).on('change', ".default-btn", function (event) {
        event.preventDefault();
        var btn = $(this);
        var file = $(this).prop('files')[0];
        var del = btn.prevAll(".wrapper").children(".cancel-btn").children();
        if (file) {
            const reader = new FileReader();
            reader.onload = function () {
                const result = reader.result;
                btn.prevAll(".wrapper").children(".image").children().attr("src", result);
                btn.prevAll(".wrapper").addClass('chon');
            }

            reader.readAsDataURL(file);
        }
        if (this.value) {
            let valueStore = this.value.match(regExp);
            btn.prevAll(".wrapper").children(".file-name").html(valueStore);
        }
    });
    $(document).on('click', ".cancel-btn", function (event) {
        event.preventDefault();
        $(this).parent().removeClass('chon');
        $(this).prevAll(".image").children().attr("src", "");
        /* Act on the event */
    });


    let masp = $(".change-color").data("id");
    var idselected = $(".change-color").find(":selected").val();
    if (idselected != null) {
        outPutLayoutEditFormDetails(masp, idselected);
    } else {
        $(".content-details-product").html("Không có dữ liệu (Bạn hãy thêm mới)");
    }
    

    /// change-color :
    $(".change-color").change(function (e) {
        e.preventDefault();
        var id = $(this).val();
        outPutLayoutEditFormDetails(masp,id);
        
    })



    //// form create-details :
    $(".btn-add-row-create-details").click(function (e) {
        e.preventDefault();
        outPutLayoutCreateFormDetails();
    });

    $(".btn-add-row-edit-details").click(function (e) {
        e.preventDefault();
        addRowEditDetails();
    });

    $(document).on("click", ".btn-delete-row-size-quantity", function (e) {
        e.preventDefault();
        var count = $(".table-details-product-edit").children("tr").length;
        if (count == 1) {
            Swal.fire("", "", "error");
            return;
        }
        $(this).parent().parent().remove();
    });

    $(document).on("click", ".btn-delete-row-size-quantity-create", function (e) {
        e.preventDefault();
        var count = $(".table-details-product-create").children("tr").length;
        if (count == 1) {
            Swal.fire("", "", "error");
            return;
        }
        $(this).parent().parent().remove();
    });

    $("#ten-mau-create").change(function (e) {
        e.preventDefault();
        var str = $(this).val();
        $("#slug-create").val(removeVietnameseTones(str));
    });
})

function onloadSelectOptionSize(id) {
    var html ="";
    $.ajax({
        url: '/admin/sanpham/ajaxSelectOptionSize',
        type: 'POST',
        dataType: 'json',
        async: false,
        success: function (value) {
            if (value.arrSize != null) {
                for (var i = 0; i < value.arrSize.length; i++) {
                    var str = "";
                    if (value.arrSize[i]["ID"] == id) {
                        str = "selected";
                    }
                    html += "<option value ='" + value.arrSize[i]["ID"]+"' "+str+">" + value.arrSize[i]["TEN_SIZE"]+"</option>"

                }
            }

        }
    })
    return html;
}

function outPutLayoutEditFormDetails(masp , id) {
    $.ajax({
        url: '/admin/sanpham/getForChangeColor/' + masp + "/" + id,
        type: 'GET',
        dataType: 'json',
        success: function (value) {
            if (value.arrColor != null) {
                $(".form-edit-details").attr("action", "/admin/sanpham/editDetails/"+masp+"/"+id);
                $("#ten-mau-edit").val(value.arrColor["TEN_MAU"]);
                $("#ma-mau-edit").val(value.arrColor["MA_MAU"]);
                $(".image-color-edit").attr("src", value.arrColor["IMAGES"]);
                if (value.arrSizeQuantity.length > 0) {
                    $(".table-details-product-edit").children().remove("tr");
                    for (var i = 0; i < value.arrSizeQuantity.length; i++) {
                        var html = "";
                        html += "<tr>";
                        html += "<td>";
                        html += '<input type="hidden" class="id_spct" name="id_spct" value="' + value.arrSizeQuantity[i]["ID"] + '">';
                        html += '<select class="form-select form-select-sm size-edit" name="size-edit" aria-label=".form-select-sm example">';
                        html += onloadSelectOptionSize(value.arrSizeQuantity[i]["ID_SIZE"]);
                        html += '</select>';
                        html += "</td>";

                        html += "<td>";
                        html += '<input type="number" name="quantity-edit" value="' + value.arrSizeQuantity[i]["SO_LUONG"] + '" placeholder="" class="quantity-edit form-control-sm form-control">';
                        html += "</td>";

                        html += '<td align="center">';
                        html += '<button type="button" class="item btn-delete-row-size-quantity" data-toggle="tooltip" data-placement="top" title = "" data-original-title="Delete" >';
                        html += '<i class="zmdi zmdi-delete"></i>';
                        html += '</button>';
                        html += "</td>";
                        html += "</tr>";
                        $(".table-details-product-edit").append(html);
                    }
                }
            }
            

        }
    })
}
function addRowEditDetails() {

    var html = "";
    html += "<tr>";
    html += "<td>";
    html += '<select class="form-select form-select-sm size-edit" name="size-edit" aria-label=".form-select-sm example">';
    html += onloadSelectOptionSize(0);
    html += '</select>';
    html += "</td>";

    html += "<td>";
    html += '<input type="number" name="quantity-edit" min="0" placeholder="Số lượng" class="quantity-edit form-control-sm form-control">';
    html += "</td>";
    html += '<td align="center">';
    html += '<button class="item btn-delete-row-size-quantity-edit" data-toggle="tooltip" data-placement="top" title = "" data-original-title="Delete" >';
    html += '<i class="zmdi zmdi-delete"></i>';
    html += '</button>';
    html += "</td>";
    html += "</tr>";
    $(".table-details-product-edit").append(html);

}
function outPutLayoutCreateFormDetails() {
    
    var html = "";
    html += "<tr>";
    html += "<td>";
    html += '<select class="form-select form-select-sm size-create" name="size-create" aria-label=".form-select-sm example">';
    html += onloadSelectOptionSize(0);
    html += '</select>';
    html += "</td>";

    html += "<td>";
    html += '<input type="number" name="quantity-create" min="0" placeholder="Số lượng" class="quantity-edit form-control-sm form-control">';
    html += "</td>";
    html += '<td align="center">';
    html += '<button class="item btn-delete-row-size-quantity-create" data-toggle="tooltip" data-placement="top" title = "" data-original-title="Delete" >';
    html += '<i class="zmdi zmdi-delete"></i>';
    html += '</button>';
    html += "</td>";
    html += "</tr>";
    $(".table-details-product-create").append(html);
                    
}


function ValidateFormUpdate() {
    if ($("#ten-san-pham").val() == "") {
        $(".validate-ten-sp").html("Tên sản phẩm không được để trống !!!");
        return false;
    }
    $(".validate-ten-sp").html("");

    if ($("#slug").val() == "") {
        $(".validate-slug").html("Slug không được để trống !!!");
        return false;
    }
    $(".validate-slug").html("");

    if ($("#ten-san-pham").val() == "") {
        $(".validate-ten-sp").html("Tên sản phẩm không được để trống !!!");
        return false;
    }
    $(".validate-ten-sp").html("");

    if ($("#gia-ban").val() == "") {
        $(".validate-gia-ban").html("Giá bán sản phẩm không được để trống !!!");
        return false;
    }
    if ($("#gia-nhap").val() == "") {
        $(".validate-gia-nhap").html("Giá nhập sản phẩm không được để trống !!!");
        return false;
    }
    $(".validate-gia-nhap").html("");

    if ($("#giam-gia").val() == "") {
        $(".validate-giam-gia").html("Giảm giá sản phẩm không được để trống !!!");
        return false;
    }
    $(".validate-giam-gia").html("");

    if ($("#don-vi-tinh").val() == "") {
        $(".validate-don-vi-tinh").html("Đơn vị tính không được để trống !!!");
        return false;
    }
    $(".validate-don-vi-tinh").html("");

    if ($("#mo-ta").val() == "") {
        $(".validate-mo-ta").html("Mô tả sản phẩm không được để trống !!!");
        return false;
    }
    $(".validate-mo-ta").html("");

    var file = $("#product-images").prop('files')[0];
    if (file != null) {
        if (!check_file_image(file, ".validate-anh-chinh")) {
            return false;
        }
    }
    $(".validate-anh-chinh").html("");
    var list_file = $("#product-list-img").prop("files");
    if (list_file.length > 0) {
        for (var i = 0; i < list_file.length; i++) {
            if (!check_file_image(list_file[i], ".validate-list-images")) {
                return false;
            }
        }
    }
    $(".validate-list-images").html("");

}


function ValidateSubmitFormDetailsUpdate() {
    if ($("#ten-mau-edit").val() == "") {
        $(".validate-form-update-details").html("Tên màu không được để trống !!!");
        return false;
    }
    $("#slug-mau-edit").val(removeVietnameseTones($("#ten-mau-edit").val()));

    var file = $(".anh-mau").prop('files')[0];
    if (file != null) {
        if (!check_file_image(file, ".validate-form-update-details")) {
            return false;
        }
    }

    var list_size = $(".size-edit");
    for (var i = 0; i < list_size.length-1; i++) {
        for (var j = i + 1; j < list_size.length; j++) {
            if (list_size[i].value == list_size[j].value) {
                $(".validate-form-update-details").html("Size màu bị lặp nhiều lần !!!");
                return false;
            }
        }   
    }

    for (var k = 0; k < $(".quantity-edit").length; k++) {
        if ($(".quantity-edit")[k].value == "") {
            $(".validate-form-update-details").html("Số lượng không được để trống !!!");
            return false;
        }
    }

    $(".validate-form-update-details").html("");
   
}

function ValidateFormCreateDetails() {
    if ($("#ten-mau-create").val() == "") {
        $(".validate-form-create-details").html("Tên màu không được để trống !!!");
        return false;
    }

    var file = $(".anh-mau-create").prop('files')[0];
    if (file != null) {
        if (!check_file_image(file, ".validate-form-create-details")) {
            return false;
        }
    } else {
        $(".validate-form-create-details").html("File ảnh không được để trống !!!");
        return false;
    }

    var list_size = $(".size-create");
    for (var i = 0; i < list_size.length - 1; i++) {
        for (var j = i + 1; j < list_size.length; j++) {
            if (list_size[i].value == list_size[j].value) {
                $(".validate-form-create-details").html("Size màu bị lặp nhiều lần !!!");
                return false;
            }
        }
    }

    for (var k = 0; k < $(".quantity-create").length; k++) {
        if ($(".quantity-create")[k].value == "") {
            $(".validate-form-create-details").html("Số lượng không được để trống !!!");
            return false;
        }
    }

    $(".validate-form-create-details").html("");

}