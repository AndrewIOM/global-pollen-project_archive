$(document).ready(function () {
    //Setup
    var canvas = document.getElementById('displayCanvas'); //<canvas>
    var image = new Image;
    var ctx = canvas.getContext('2d');
    var container = $(canvas).parent(); //<div class='zoom-canvas-container>
    var firstImage = $('#zoom-thumbs a img:first');
    image.src = firstImage.attr("src");
    respondCanvas();
    $(window).resize(respondCanvas);

    //Zooming
    //canvas.addEventListener('DOMMouseScroll', handleScroll, false);
    //canvas.addEventListener('mousewheel', handleScroll, false);

    function respondCanvas() {
        canvas.width = $(container).width(); //max width
        canvas.height = $(container).height(); //max height
        redraw();
    }

    function redraw() {
        ctx.fillStyle = '#333333';
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        var imgObj = new Image();
        imgObj.src = image.src;

        var renderHeight = imgObj.naturalHeight;
        var renderWidth = imgObj.naturalWidth;

        var ratio = imgObj.naturalWidth / imgObj.naturalHeight;
        if (ratio < 1) { //Portrait
            var scaling = 1;
            if (renderHeight > canvas.height) {
                scaling = canvas.height / renderHeight;
                renderHeight = canvas.height;
            }
            renderWidth = renderWidth * scaling;
        } else { //Landscape
            var scaling = 1;
            if (renderWidth > canvas.width) {
                scaling = canvas.width / renderWidth;
                renderWidth = canvas.width;
            }
            renderHeight = renderHeight * scaling;
        }

        var widthOffset = (canvas.width - renderWidth) / 2;
        var heightOffset = (canvas.height - renderHeight) / 2;
        ctx.drawImage(image, widthOffset, heightOffset, renderWidth, renderHeight);
    }

    changeImage = function(src) {
        image.src = src;
        redraw();
    }

});