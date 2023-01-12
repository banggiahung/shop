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
/// list-img 
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
                    div.setAttribute("class", "col-4");

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

jQuery(document).ready(function ($) {
    $(document).on("click", ".btn-remove-row-size-quantity", function () {
        $(this).parent().parent().remove();
    })

    $(document).on("click", ".btn-add-row-size-quantity", function () {
        var div = $(this).parent().prev();


        var div_row = document.createElement("div");
        div_row.setAttribute("class", "row form-group align-items-center mb-2");

        var div_col_1 = document.createElement("div");
        div_col_1.setAttribute("class", "col-5");
        var select = document.createElement('select');
        select.setAttribute("class", "size form-control-sm form-control");
        select.setAttribute("name", "size");
        /*select option */
        $.ajax({
            url: '/admin/sanpham/ajaxSelectOptionSize',
            type: 'POST',
            dataType: 'json',
            success: function (value) {
                if (value.arrSize != null) {
                    for (var i = 0; i < value.arrSize.length; i++) {
                        var option = document.createElement("option");
                        option.value = value.arrSize[i]["ID"];
                        option.innerHTML = value.arrSize[i]["TEN_SIZE"];
                        select.append(option);
                    }

                }

            }
        })
        /*end*/
        div_col_1.append(select);

        var div_col_2 = document.createElement("div");
        div_col_2.setAttribute("class", "col-5");

        var input = document.createElement('input');
        input.type = "number";
        input.setAttribute("class", "quantity form-control-sm form-control");
        input.setAttribute("name", "quantity");
        input.setAttribute("placeholder", "Số Lượng");
        div_col_2.append(input);

        var div_col_3 = document.createElement("div");
        div_col_3.setAttribute("class", "col-2");
        var span = document.createElement("span");
        span.setAttribute("class", "btn-remove-row-size-quantity fa fa-times-circle text-danger");
        div_col_3.append(span);


        div_row.append(div_col_1, div_col_2, div_col_3);

        div.append(div_row);
    })

    $(".btn-add-row-details-product").off("click").on("click", function (e) {
        e.preventDefault();

        var td1 = document.createElement("td");
        var span = document.createElement("span");
        span.setAttribute("class", "btn fa fa-times-circle delete-row-color");
        span.setAttribute("style", "font-size:20px ; color:red;");
        td1.append(span);

        var td2 = document.createElement("td");
        var input_text_1 = document.createElement('input');
        input_text_1.type = "text";
        input_text_1.setAttribute("class", "ten-mau form-control-sm form-control");
        input_text_1.setAttribute("name", "ten-mau");
        td2.append(input_text_1);


        var td3 = document.createElement("td");
        var input_color = document.createElement('input');
        input_color.type = "color";
        input_color.setAttribute("class", "ma-mau form-control-sm form-control");
        input_color.setAttribute("style", " height: calc(1.5em + .75rem + 2px)");
        input_color.setAttribute("value", "#ff0000");
        input_color.setAttribute("placeholder", "Tên màu .....");
        input_color.setAttribute("name", "ma-mau");
        td3.append(input_color);



        var td4 = document.createElement("td");
        var div = document.createElement("div");
        div.setAttribute("class", "upload-image-details");
        var button = document.createElement("button");
        button.setAttribute("class", "custom-btn");
        button.type = "button";
        button.innerHTML = "Choose a file";
        var file = document.createElement("input");
        file.type = "file";
        file.setAttribute("class", "anh-mau default-btn");
        file.setAttribute("name", "anh-mau");
        file.setAttribute("hidden", "hidden");
        var div1 = document.createElement("div");
        div1.setAttribute("class", "wrapper")
        var div2 = document.createElement("div");
        div2.setAttribute("class", "image");
        var img = document.createElement("img");
        img.setAttribute("src", "");
        img.setAttribute("alt", "Chọn File");
        var div3 = document.createElement("div");
        div3.setAttribute("class", "content");
        var div3_1 = document.createElement("div");
        div3_1.setAttribute("class", "icon");
        var i1 = document.createElement("i");
        i1.setAttribute("class", "fas fa-cloud-upload-alt");
        var div3_2 = document.createElement("div");
        div3_2.setAttribute("class", "text");
        div3_2.innerHTML = "No file chosen, yet";
        div3_1.append(i1);
        var div4 = document.createElement("div");
        div4.setAttribute("class", "cancel-btn");
        var i2 = document.createElement("i");
        i2.setAttribute("class", "fas fa-times");
        div4.append(i2);
        var div5 = document.createElement("div");
        div5.setAttribute("class", "file-name");
        div5.innerHTML = "File name here";
        div2.append(img);
        div3.append(div3_1, div3_2);
        div1.append(div2, div3, div4, div5);
        div.append(div1, button, file);
        td4.append(div);

        var td5 = document.createElement("td");
        td5.setAttribute("colspan", "2");

        var div_parent = document.createElement("div");
        div_parent.setAttribute("class", "size-and-quantity");
        var div_row = document.createElement("div");
        div_row.setAttribute("class", "row form-group align-items-center");

        var div_col_1 = document.createElement("div");
        div_col_1.setAttribute("class", "col-5");
        var select = document.createElement('select');
        select.setAttribute("class", "size form-control-sm form-control");
        select.setAttribute("name", "size");

        /*select option */
        $.ajax({
            url: '/admin/sanpham/ajaxReturnInformationSanPham',
            type: 'POST',
            dataType: 'json',
            success: function (value) {
                if (value.arrSize != null) {
                    for (var i = 0; i < value.arrSize.length; i++) {
                        var option = document.createElement("option");
                        option.value = value.arrSize[i]["ID"];
                        option.innerHTML = value.arrSize[i]["TEN_SIZE"];
                        select.append(option);
                    }

                }

            }
        })
        /*end*/

        div_col_1.append(select);

        var div_col_2 = document.createElement("div");
        div_col_2.setAttribute("class", "col-5");

        var input = document.createElement('input');
        input.type = "number";
        input.setAttribute("class", "quantity form-control-sm form-control");
        input.setAttribute("name", "quantity");
        input.setAttribute("placeholder", "Số Lượng");
        div_col_2.append(input);

        var div_col_3 = document.createElement("div");
        div_col_3.setAttribute("class", "col-2");
        var span1 = document.createElement("span");
        span1.setAttribute("class", "btn-remove-row-size-quantity fa fa-times-circle text-danger");
        div_col_3.append(span1);
        div_col_3.append(span1);
        div_row.append(div_col_1, div_col_2, div_col_3);
        var div_span = document.createElement("div");
        var span2 = document.createElement("span");
        span2.setAttribute("class", "btn-add-row-size-quantity fa fa-plus-circle text-primary");
        div_span.append(span2);
        div_parent.append(div_row);
        td5.append(div_parent, div_span);



        var tr = document.createElement("tr");
        tr.append(td1, td2, td3, td4, td5);
        jQuery(".tbody-details-san-pham").append(tr);
    })

})
function DeleteRow(val) {
    var length_row = $(".table-color tbody").children('tr');
    if (length_row.length > 1) {
        var row = val.parentElement.parentElement.remove();
    } else {
        Swal.fire("", "", "error");
    }
    // body...

}




let regExp = /[0-9a-zA-Z\^\&\'\@\{\}\[\]\,\$\=\!\-\#\(\)\.\%\+\~\_ ]+$/;



jQuery(document).ready(function ($) {

    $(document).on('click', '.custom-btn', function () {
        event.preventDefault();
        $(this).next().click();
    });
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


    $(document).on('click', ".delete-row-color", function (event) {
        event.preventDefault();
        var count_tr = $(".table-color tbody").children('tr');
        var span = $(this);
        if (count_tr.length > 1) {
            var row = span.parent().parent().remove();
        } else {
            Swal.fire("", "", "error");
        }
    });


    $(document).on('change', "#ten-san-pham", function (event) {
        event.preventDefault();
        var str = removeVietnameseTones($(this).val());
        $("#slug").val(str);
    });
});




function AjaxCheckIssetSanPham(key, str) {
    var check;
    $.ajax({
        url: '/admin/sanpham/ValidateInsertSlug',
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


function returnInformationDetailsProduct() {

    var arrData = new Array();
    var countTr = $(".tbody-details-san-pham").children();
    for (var i = 0; i < countTr.length; i++) {
        var object = new Object();
        object.tenMau = $(".ten-mau")[i].value;
        object.slug = removeVietnameseTones($(".ten-mau")[i].value);
        object.maMau = countTr[i].cells[2].children[0].defaultValue;
        var arrSize = [];
        var size_quantity = $(".size-and-quantity")[0].children;
        for (var j = 0; j < size_quantity.length; j++) {
            var arr = new Object();
            arr.size = size_quantity[j].children[0].children[0].value;
            arr.quantity = size_quantity[j].children[1].children[0].value;
            if (arrSize.length > 0) {
                for (var k = 0; k < arrSize.length; k++) {
                    if (arrSize[k].size == arr.size) {
                        return false;
                    }
                }
            }
            arrSize.push(arr);
        }
        object.sizeQuantity = arrSize;
        arrData.push(object);
    }
    return arrData;
}

function ValidateFileImage(file_img, classError = "") {
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


function ValidateData() {
    $(".validate").html("");
    var check = false;
    var masp = $("#ma-san-pham").val();
    var tensp = $("#ten-san-pham").val();
    var idlsp = $(".loai-san-pham").val();
    var slug = $("#slug").val();
    var dvTinh = $("#don-vi-tinh").val();
    var moTa = $("#mo-ta").val();
    var moTaChiTiet = $("#mo-ta-chi-tiet").val();

    if (idlsp == "") {
        $(".validate-lsp").html("Mã loại sản phẩm không được để trống !!!");
        check = true;
      
    }
   
    if (AjaxCheckIssetSanPham("MA_SP", masp.trim())) {
        $(".validate-masp").html("Mã sản phẩm đã tồn tại !!!");
        check = true;
    }
    regex = /^[a-zA-Z0-9]+$/; // Chỉ chấp nhận ký tự alphabet thường hoặc ký tự hoa
    if (!regex.test(masp)) { // true nếu text chỉ chứa ký tự alphabet thường hoặc hoa, false trong trường hợp còn lại. 
        $(".validate-masp").html("Mã sản phẩm không được ký tự đặc biệt !!!");
        check = true;
    }
    if (masp.length < 1 || masp.length > 20) {
        $(".validate-masp").html("Mã sản phẩm không được có độ dài ( 1 -> 20 ) ký tự !!!");
        check = true;
    }

    if (AjaxCheckIssetSanPham("SLUG", slug.trim()) && tensp.length > 1) {
        Swal.fire("Error", "Tên sản phẩm đã tồn tại !!!", "error");
        check = true;
    }

    if (tensp.length < 1 || tensp.length > 50) {
        $(".validate-ten-sp").html("Tên sản phẩm có độ dài ( 1 -> 50 ) ký tự !!!");
        check = true;
    }

    if (dvTinh.length < 1 || dvTinh.length > 50) {
        $(".validate-don-vi-tinh").html("Đơn vị tính sản phẩm có độ dài ( 1 -> 50 ) ký tự !!!");
        check = true;
    }
    if (moTa.length < 1 || moTa.length > 255) {
        $(".validate-mo-ta").html("Mô tả sản phẩm có độ dài ( 1 -> 255 ) ký tự !!!");
        check = true;
    }
    if ($("#gia-nhap").val() == "") {
        $(".validate-gia-nhap").html("Giá nhập không được để trống !!!");
        check = true;
    }
    if ($("#gia-ban").val() == "") {
        $(".validate-gia-ban").html("Giá bán không được để trống !!!");
        check = true;
    }
    if ($("#giam-gia").val() == "") {
        $(".validate-giam-gia").html("Giảm giá không được để trống !!!");
        check = true;
    }

    var file_img = $("#product-images").prop('files');
    if (file_img.length == 0) {
        $(".validate-images").html("Ảnh chính sản phẩm không được để trống !!!");
        check = true;
    } else {
        if (ValidateFileImage(file_img[0], ".validate-images") == false) {
            check = true;
        }
    }

    var fileSelected = $('.file-upload-default-list-img').prop('files');
    if (fileSelected.length == 0) {
        $(".validate-list-images").html("Ảnh kèm theo sản phẩm không được để trống !!!");
        check = true;
    } else {
        for (var k = 0; k < fileSelected.length; k++) {
            if (ValidateFileImage(fileSelected[k], ".validate-list-images") == false) {
                check = true;
            }
        }
    }
    var length_sl = $(".quantity");
    for (var i = 0; i < length_sl.length; i++) {
        if (length_sl[i].value === "") {
            $(".validate-table-insert").html("Số lượng   không được để trống !!! ");
            check = true;
            break;
        }
    }

    for (var i = 0; i < $(".anh-mau").length; i++) {
        if ($(".anh-mau")[i].files.length == 0) {
            $(".validate-table-insert").html("Ảnh màu không được để trống");
            check = true;
            break;
        }
        else {
            if (ValidateFileImage($(".anh-mau")[i].files[0], ".validate-table-insert") == false) {
                check = true;
            }
        }
    }

    var list_ten_mau = $(".ten-mau");
    for (var i = 0; i < list_ten_mau.length ; i++) {
        var status = false;
        if (removeVietnameseTones(list_ten_mau[i].value) == "") {
            check = true;
            $(".validate-table-insert").html("Tên màu không được để trống !!!");
            break;
        }
        for (var j = i+1; j < list_ten_mau.length; j++) {
            if (removeVietnameseTones(list_ten_mau[i].value) == removeVietnameseTones(list_ten_mau[j].value)) {
                status = check = true;
                $(".validate-table-insert").html("Tên màu không được trùng nhau !!!");
                break;
            }
        }
        if (status) { break;}
    }

    return check;

}


///Thêm sản phẩm bằng ajax :
jQuery(document).ready(function ($) {
    $(".btn-submit-product").off("click").on("click", function (e) {
        e.preventDefault();
        
        var status = false;
        status = ValidateData();

        var noibat = $("#noibat").prop("checked");
        var hienthi = $("#hienthi").prop("checked");
        var ma_sp = $("#ma-san-pham").val();
        var lsp = $(".loai-san-pham").val();
        var ten_sp = $("#ten-san-pham").val();
        var slug = $("#slug").val();
        var dvTinh = $("#don-vi-tinh").val();
        var moTa = $("#mo-ta").val();
        var moTaChiTiet = CKEDITOR.instances["mo-ta-chi-tiet"].getData();
        var giaNhap = $("#gia-nhap").val();
        var giaBan = $("#gia-ban").val();
        var giamGia = $("#giam-gia").val();

        var details = returnInformationDetailsProduct();

        if (details == false) {
            status = true;
        }
        if (status) {
            Swal.fire("", "", "error");
            return;
        } 

        var sp_ct = JSON.stringify(details);
        var formData = new FormData();
        formData.append("loai-san-pham", lsp);
        formData.append("ma-sp", ma_sp);
        formData.append("gia-nhap", giaNhap);
        formData.append("gia-ban", giaBan);
        formData.append("giam-gia", giamGia);
        formData.append("ten-sp", ten_sp);
        formData.append("slug", slug);
        formData.append("don-vi-tinh", dvTinh);
        formData.append("mo-ta", moTa);
        formData.append("mo-ta-chi-tiet", moTaChiTiet);
        formData.append("details", sp_ct);
        formData.append("noi-bat", noibat);
        formData.append("hien-thi", hienthi);
        formData.append("anh-chinh", $("#product-images").prop('files')[0]);
        for (var i = 0; i < $("#product-list-img").prop('files').length; i++) {
            formData.append("list-anh", $("#product-list-img").prop('files')[i]);
        }
        for (var i = 0; i < $(".anh-mau").length; i++) {
            formData.append("anh-mau", $(".anh-mau")[i].files[0]);
        }

        $.ajax({
            type: "POST",
            enctype: 'multipart/form-data',
            url: "/admin/sanpham/create",
            data: formData,
            processData: false,
            contentType: false,

            cache: false,
            dataType: "json",
            success: function (data) {
                Swal.fire("", "", data.status);
             
                if (data.status == "success") {
                    setTimeout(function () { location.reload() }, 1000);
                }
            }
        });
    })
});
