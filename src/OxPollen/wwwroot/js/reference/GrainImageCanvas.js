$(document).ready(function () {
    var canvas = document.getElementById('displayCanvas'); //<canvas>
    var image = new Image;
    var ctx = canvas.getContext('2d');
    var container = $(canvas).parent(); //<div class='zoom-canvas-container>

    var currentImageSet = $('#zoom-thumbs a:first img');
    var isFocusImage = currentImageSet.length > 1;
    var firstImage;
    if (isFocusImage) {
        firstImage = currentImageSet[2]; //TODO Assumes 5 images
    } else {
        firstImage = currentImageSet[0];
    }
    image.src = firstImage.src;
    respondCanvas();
    $(window).resize(respondCanvas);

    //Setup Slider
    var slider = document.getElementById('focusSlider');
    noUiSlider.create(slider, {
        start: [3],
        step: 1,
        tooltips: true,
        range: {
            'min': [1],
            'max': [5]
        }, orientation: 'vertical'
    });
    if (!isFocusImage) {
        slider.setAttribute('disabled', true);
        $('#focusSlider').css('display', 'none');
    }

    function respondCanvas() {
        canvas.width = $(container).width(); //max width
        canvas.height = $(container).height(); //max height
        redraw();
    }

    function redraw() {
        ctx.fillStyle = '#333333';
        ctx.fillRect(0, 0, canvas.width, canvas.height);

        var imgObj = new Image();
        imgObj.onload = function () {
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
        };
        imgObj.src = image.src;
    }

    changeImage = function (a) {
        currentImageSet = a.getElementsByTagName('img');
        isFocusImage = currentImageSet.length > 1;
        if (isFocusImage) {
            image.src = currentImageSet[2].src; //TODO Assumes 5 images
        } else {
            image.src = currentImageSet[0].src;
        }
        if (isFocusImage) {
            slider.removeAttribute('disabled');
            $('#focusSlider').css('display', '');
        } else {
            slider.setAttribute('disabled', true);
            $('#focusSlider').css('display', 'none');
            slider.noUiSlider.set(3);
        }
        redraw();
    }

    //Slider Events
    slider.noUiSlider.on('slide', function () {
        var value = slider.noUiSlider.get();
        image.src = currentImageSet[value - 1].src;
        redraw();
    });
});