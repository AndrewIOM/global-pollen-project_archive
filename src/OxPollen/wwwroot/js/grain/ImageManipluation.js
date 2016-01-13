var canvasSize = 300;
var cropSize = canvasSize * 0.80;
var dkrm = null;

function loadDarkroom(imgId) {
    console.log("reloading...");
    dkrm = new Darkroom('#' + imgId, {
        // canvas options
        minWidth: 100,
        minHeight: 100,
        maxWidth: 600,
        maxHeight: 300,
        ratio: 4 / 3,
        backgroundColor: '#000',

        plugins: {
            history: false,
            save: false
        },
        init: function () {
            var cropPlugin = this.getPlugin('crop');
            var xoffset = (canvasSize - cropSize) / 2;
            cropPlugin.selectZone(xoffset, xoffset, cropSize, cropSize);
            cropPlugin.requireFocus();
        }
    });
}
function scaleImage(image) {
    var width = image.getWidth();
    var height = image.getHeight();
    var scaleMin = 1;
    var scaleMax = 1;
    var scaleX = 1;
    var scaleY = 1;

    if (null !== dkrm.options.maxWidth && dkrm.options.maxWidth < width) {
        scaleX = dkrm.options.maxWidth / width;
    }
    if (null !== dkrm.options.maxHeight && dkrm.options.maxHeight < height) {
        scaleY = dkrm.options.maxHeight / height;
    }
    scaleMin = Math.min(scaleX, scaleY);
    scaleX = 1;
    scaleY = 1;
    if (null !== dkrm.options.minWidth && dkrm.options.minWidth > width) {
        scaleX = dkrm.options.minWidth / width;
    }
    if (null !== dkrm.options.minHeight && dkrm.options.minHeight > height) {
        scaleY = dkrm.options.minHeight / height;
    }
    scaleMax = Math.max(scaleX, scaleY);
    var scale = scaleMax * scaleMin; // one should be equals to 1

    image.setScaleX(scale);
    image.setScaleY(scale);

    return image;
}
function resetImage(image) {
    if (dkrm) {
        image = scaleImage(image);
        dkrm.canvas.remove(dkrm.image);
        dkrm.canvas.add(image);
        dkrm.canvas.centerObject(image);
        image.setCoords();
        image.sendToBack();
    }
}
function loadImage(event) {
    var dataURI = event.target.result;
    if (dkrm) {
        fabric.Image.fromURL(dataURI, function (ximg) { resetImage(ximg); },
          {
              // options to make the image static
              selectable: false,
              evented: false,
              lockMovementX: true,
              lockMovementY: true,
              lockRotation: true,
              lockScalingX: true,
              lockScalingY: true,
              lockUniScaling: true,
              hasControls: false,
              hasBorders: false
          }
        );
    }
}
function readerError(event) {
    console.error("FileReader failed: Code " + event.target.error.code);
}

function doClick() {
    var el = document.getElementById("fileElem");
    if (el) {
        el.click();
    }
}
function handleImage(files) {
    var d = document.getElementById("img-container");
    if (files.length = 1) {
        var reader = new FileReader();
        reader.onload = loadImage;
        reader.onerror = readerError;
        reader.readAsDataURL(files[0]);
    }
}
function handleFiles(input) {
    var d = document.getElementById("image-thumbnails");
    if (!input.files.length) {
        d.innerHTML = "<p>None</p>";
    } else {
        d.innerHTML = "";
        for (var i = 0; i < input.files.length && i < 4; i++) {
            var div = document.createElement('div');
            div.style.display = 'inline';
            div.className = "image-thumbnail col-md-6";
            d.appendChild(div);
            var img = document.createElement("img");
            img.src = window.URL.createObjectURL(input.files[i]);;
            img.id = "image-upload-" + (i + 1);
            img.style.height = '12em';
            div.appendChild(img);
            loadDarkroom(img.id);
        }
    }
}