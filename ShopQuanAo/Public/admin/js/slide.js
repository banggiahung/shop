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