function ChangeImage(UploadImage, prevuewImg) {
    if (UploadImage.file && UploadImage.file[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg).attr('src', target.result);

        }
        reader.readAsDataURL(uploadImage.File[0]);
    }
}