//Image Fade and Fill
$(document).ready(function () {
    $(".img-container").each(function () {
        $(this).children('img').on("load", function () {
            var refRatio = 300 / 300;
            var imgH = $(this).height();
            var imgW = $(this).width();
            console.log(imgH + ' ' + imgW);
            if ((imgW / imgH) < refRatio) {
                $(this).closest('.img-container').addClass("portrait");
            } else {
                $(this).closest('.img-container').addClass("landscape");
            }
        }).each(function () {
            if (this.complete) $(this).load();
        });

    })
});
