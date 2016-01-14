//Image Fill
$(".img-container").each(function () {
    var refRatio = 300 / 300;
    var imgH = $(this).children("img").height();
    var imgW = $(this).children("img").width();
    if ((imgW / imgH) < refRatio) {
        $(this).addClass("portrait");
    } else {
        $(this).addClass("landscape");
    }
})