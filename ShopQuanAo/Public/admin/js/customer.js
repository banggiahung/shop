
/// js upload ảnh màu sản phẩm chi tiết 
jQuery(document).ready(function ($) {



    let regExp = /[0-9a-zA-Z\^\&\'\@\{\}\[\]\,\$\=\!\-\#\(\)\.\%\+\~\_ ]+$/;
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

})