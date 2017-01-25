var canvas;
var ctx;
var image;
var slider;

$(document).ready(function () {
    $('#focus-add-button').addClass('disabled');
    $('#focus-preview').hide();

    //Setup Slider
    slider = document.getElementById('focusSlider');
    noUiSlider.create(slider, {
        start: [1],
        step:1,
        tooltips:true,
        range: {
            'min': [1],
            'max': [5]
        }, orientation: 'vertical'
    });
})

function destroyFocusPreview() {
    //Cleanup old focus image from dialog box
    $('#focus-add-button').addClass('disabled');
    $('#focus-upload-button').removeClass('disabled');
    $('#focus-upload-error').text('');
    $('#focus-preview').hide();
    document.getElementById('focusImagePreview').innerHTML = '';
    document.getElementById('focus-images').innerHTML = '';
}

function handleFocusFrames(input) {
    destroyFocusPreview();
    if (input.files.length) {
        var validImages = 0;
        var imageContainer = document.getElementById('focus-images');
        for (var i = 0; i < input.files.length; i++) {
            if (/\.(jpe?g|png|gif)$/i.test(input.files[i].name)) {
                validImages++;
                var img = document.createElement('img');
                img.src = window.URL.createObjectURL(input.files[i]);
                imageContainer.appendChild(img);
            }
        }
        if (validImages == 5) {
            $('#focus-upload-error').text('');
            setup();
        } else {
            $('#focus-upload-error').text('The image stack was not valid');
        }
    } else {
        $('#focus-upload-error').text("You didn't select any images");
    }
}

function setup() {
    //Setup Canvas
    canvas = document.getElementById('focusImagePreview');
    $('#focus-preview').show();
    image = new Image;
    ctx = canvas.getContext('2d');
    var firstImage = $('#focus-images img:first');
    image.src = firstImage.attr("src");
    respondCanvas();
    $(window).resize(respondCanvas);

    //Slider Events
    slider.noUiSlider.on('slide', function () {
        var value = slider.noUiSlider.get();
        var imageContainer = document.getElementById('focus-images');
        image.src = imageContainer.getElementsByTagName('img')[value-1].src;
        redraw();
    });
    $('#focus-add-button').removeClass('disabled');
}

function confirmFocusImage() {
    var d = document.getElementById("focusImages");
    var images = document.getElementById('focus-images').getElementsByTagName('img');
    addImageToGrid(d, images);
    destroyFocusPreview();
}

//Canvas Functions
function respondCanvas() {
    canvas.width = 400;//$('#zoom-canvas-container').width(); //max width
    canvas.height = 400;//$('#zoom-canvas-container').height(); //max height
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

changeImage = function (src) {
    image.src = src;
    redraw();
}

//Base Functions
function convertToDataURLviaCanvas(url, callback) {
    var img = new Image();
    img.crossOrigin = 'Anonymous';
    img.onload = function () {
        var canvas = document.createElement('CANVAS');
        var ctx = canvas.getContext('2d');
        var dataURL;
        canvas.height = this.height;
        canvas.width = this.width;
        ctx.drawImage(this, 0, 0);
        dataURL = canvas.toDataURL("image/png");
        callback(dataURL);
        canvas = null;
    };
    img.src = url;
}

function addImageToGrid(d, images) {
    //Create elements for image
    var li = document.createElement('li');
    d.appendChild(li);
    var div = document.createElement('div');
    div.className = "img-container";
    li.appendChild(div);
    var a = document.createElement('a');
    div.appendChild(a);

    //Create URL holders for each image
    function convertToBase64(urlHolder, image) {
        convertToDataURLviaCanvas(image.src, function (base64Img) {
            urlHolder.src = base64Img;
        }, false)
    }
    for (var i = 0; i < images.length; i++) {
        var urlHolder = document.createElement('img');
        if (i != 2) urlHolder.hidden = 'hidden';
        //urlHolder.src = images[i].src;
        convertToBase64(urlHolder, images[i]);
        a.appendChild(urlHolder);
    }

    //Create delete button
    var del = document.createElement('span');
    del.className = 'delete';
    var icon = document.createElement('span');
    icon.className = 'glyphicon glyphicon-trash';
    del.appendChild(icon);
    a.appendChild(del);
    del.onclick = function () {
        $(this).closest('li').remove();
    };
}