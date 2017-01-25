$(document).ready(function () {
    var canvas = document.getElementById('displayCanvas'); //<canvas>
    var image = new Image;
    var ctx = canvas.getContext('2d');
    trackTransforms(ctx);
    var container = $(canvas).parent(); //<div class='zoom-canvas-container>
    var currentScaleFactor = 1;

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

    //Setup Zoom Buttons
    $('#grain-zoomin').click(function () {
        lastX = canvas.width / 2;
        lastY = canvas.height / 2;
        zoom(1);
    });
    $('#grain-zoomout').click(function () {
        lastX = canvas.width / 2;
        lastY = canvas.height / 2;
        zoom(-1);
    });

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
        fillCanvas();
        redraw();
    }

    function fillCanvas() {
        var p1 = ctx.transformedPoint(0, 0);
        var p2 = ctx.transformedPoint(canvas.width, canvas.height);
        ctx.fillStyle = '#333333';
        ctx.fillRect(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y);
    }

    function redraw() {
        var imgObj = new Image();
        imgObj.onload = function () {
            var renderHeight = imgObj.naturalHeight;
            var renderWidth = imgObj.naturalWidth;

            var ratio = imgObj.naturalWidth / imgObj.naturalHeight;
            var scaling = 1;
            if (renderHeight > canvas.height) {
                scaling = canvas.height / renderHeight;
                renderHeight = canvas.height;
            }
            renderWidth = renderWidth * scaling;
            var scaling = 1;
            if (renderWidth > canvas.width) {
                scaling = canvas.height / renderHeight;
                renderHeight = canvas.height;
            }

            renderHeight = renderHeight * scaling;

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
        fillCanvas();
        redraw();
    }

    //Slider Events
    slider.noUiSlider.on('slide', function () {
        var value = slider.noUiSlider.get();
        image.src = currentImageSet[value - 1].src;
        redraw();
    });

    //Zoom and Pan Functions
    var lastX = canvas.width / 2, lastY = canvas.height / 2;
    var dragStart, dragged;
    canvas.addEventListener('mousedown', function (evt) {
        document.body.style.mozUserSelect = document.body.style.webkitUserSelect = document.body.style.userSelect = 'none';
        lastX = evt.offsetX || (evt.pageX - canvas.offsetLeft);
        lastY = evt.offsetY || (evt.pageY - canvas.offsetTop);
        dragStart = ctx.transformedPoint(lastX, lastY);
        dragged = false;
    }, false);
    canvas.addEventListener('mousemove', function (evt) {
        lastX = evt.offsetX || (evt.pageX - canvas.offsetLeft);
        lastY = evt.offsetY || (evt.pageY - canvas.offsetTop);
        dragged = true;
        if (dragStart) {
            var pt = ctx.transformedPoint(lastX, lastY);
            ctx.translate(pt.x - dragStart.x, pt.y - dragStart.y);
            fillCanvas();
            redraw();
        }
    }, false);
    canvas.addEventListener('mouseup', function (evt) {
        dragStart = null;
        if (!dragged) zoom(evt.shiftKey ? -1 : 1);
    }, false);

    var scaleFactor = 1.1;
    var zoom = function (clicks) {
        //Enforce lower and upper zoom limits
        if ((currentScaleFactor < 20 && clicks > 0) || (currentScaleFactor > -5 && clicks < 0)) {
            var pt = ctx.transformedPoint(lastX, lastY);
            ctx.translate(pt.x, pt.y);
            var factor = Math.pow(scaleFactor, clicks);
            currentScaleFactor = currentScaleFactor + clicks;
            ctx.scale(factor, factor);
            ctx.translate(-pt.x, -pt.y);
            fillCanvas();
            redraw();
        }
    }

    var handleScroll = function (evt) {
        var delta = evt.wheelDelta ? evt.wheelDelta / 40 : evt.detail ? -evt.detail : 0;
        if (delta) zoom(delta);
        return evt.preventDefault() && false;
    };
    canvas.addEventListener('DOMMouseScroll', handleScroll, false);
    canvas.addEventListener('mousewheel', handleScroll, false);

});

function trackTransforms(ctx) {
    var svg = document.createElementNS("http://www.w3.org/2000/svg", 'svg');
    var xform = svg.createSVGMatrix();
    ctx.getTransform = function () { return xform; };

    var savedTransforms = [];
    var save = ctx.save;
    ctx.save = function () {
        savedTransforms.push(xform.translate(0, 0));
        return save.call(ctx);
    };
    var restore = ctx.restore;
    ctx.restore = function () {
        xform = savedTransforms.pop();
        return restore.call(ctx);
    };

    var scale = ctx.scale;
    ctx.scale = function (sx, sy) {
        xform = xform.scaleNonUniform(sx, sy);
        return scale.call(ctx, sx, sy);
    };
    var rotate = ctx.rotate;
    ctx.rotate = function (radians) {
        xform = xform.rotate(radians * 180 / Math.PI);
        return rotate.call(ctx, radians);
    };
    var translate = ctx.translate;
    ctx.translate = function (dx, dy) {
        xform = xform.translate(dx, dy);
        return translate.call(ctx, dx, dy);
    };
    var transform = ctx.transform;
    ctx.transform = function (a, b, c, d, e, f) {
        var m2 = svg.createSVGMatrix();
        m2.a = a; m2.b = b; m2.c = c; m2.d = d; m2.e = e; m2.f = f;
        xform = xform.multiply(m2);
        return transform.call(ctx, a, b, c, d, e, f);
    };
    var setTransform = ctx.setTransform;
    ctx.setTransform = function (a, b, c, d, e, f) {
        xform.a = a;
        xform.b = b;
        xform.c = c;
        xform.d = d;
        xform.e = e;
        xform.f = f;
        return setTransform.call(ctx, a, b, c, d, e, f);
    };
    var pt = svg.createSVGPoint();
    ctx.transformedPoint = function (x, y) {
        pt.x = x; pt.y = y;
        return pt.matrixTransform(xform.inverse());
    }
}