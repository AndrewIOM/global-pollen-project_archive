//Single images upload
function handleFiles(input) {
    var d = document.getElementById("images");
    if (input.files.length) {
        for (var i = 0; i < input.files.length; i++) {
            if (/\.(jpe?g|png|gif)$/i.test(input.files[i].name)) {
                setupReader(input.files[i], d);
            }
        }
    }
}

function setupReader(file, d) {
    //Create elements for image
    var li = document.createElement('li');
    d.appendChild(li);
    var div = document.createElement('div');
    div.className = "img-container";
    li.appendChild(div);
    var a = document.createElement('a');
    div.appendChild(a);
    var img = document.createElement("img");
    var uriArea = document.createElement('textarea');
    uriArea.hidden = 'hidden';
    div.appendChild(uriArea);
    convertToDataURLviaCanvas(window.URL.createObjectURL(file), function (base64Img) {
        img.src = base64Img;
        uriArea.value = base64Img;
    });

    //Format image for container
    var refRatio = 300 / 300;
    var imgH = img.height;
    var imgW = img.width;
    if ((imgW / imgH) < refRatio) {
        div.className = "img-container portrait";
    } else {
        div.className = "img-container landscape";
    }
    a.appendChild(img);

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

//Ajax upload request
function uploadFile(button) {
    $('#submit1').prop('disabled', true);
    $('#submit1').addClass('disabled');
    $('#submit2').prop('disabled', true);
    $('#submit2').addClass('disabled');

    var collectionId = document.getElementById('CollectionId').value;

    //Get form data
    var form = document.getElementById('addGrainForm');
    var formData = new FormData(form);

    //Static Images
    var images = document.getElementById('images').getElementsByTagName('textarea');
    var imgsB64 = [];
    for (var i = 0; i < images.length; i++) {
        var imgEncoded = images[i].value;
        imgEncoded = imgEncoded.slice(imgEncoded.indexOf(',') + 1);
        imgsB64.push(imgEncoded);
        formData.append('Images[' + i + ']', imgEncoded);
    }

    //Focus Images
    var focusImageFrames = document.getElementById('focusImages').getElementsByTagName('img');
    var focusImages = [];
    var focusImageCount = 0;
    for (var i = 0; i < focusImageFrames.length; i += 5) {
        formData.append('FocusImages[' + focusImageCount + '].FocusLowUrl', focusImageFrames[i].src);
        formData.append('FocusImages[' + focusImageCount + '].FocusMedLowUrl', focusImageFrames[i+1].src);
        formData.append('FocusImages[' + focusImageCount + '].FocusMedUrl', focusImageFrames[i+2].src);
        formData.append('FocusImages[' + focusImageCount + '].FocusMedHighUrl', focusImageFrames[i+3].src);
        formData.append('FocusImages[' + focusImageCount + '].FocusHighUrl', focusImageFrames[i+4].src);
    }

    //Progress Bar
    var progbar = form.getElementsByClassName('progress-bar')[0];
    var progDiv = form.getElementsByClassName('progress')[0];
    var submit = document.getElementById('submit');
    progbar.className = 'progress-bar progress-bar-striped active';
    submit1.className = 'btn btn-primary disabled';
    submit2.className = 'btn btn-primary disabled';
    progDiv.setAttribute('style', 'display:""');

    //Ajax Request
    ajax = new XMLHttpRequest();
    (ajax.upload || ajax).addEventListener('progress', function (e) {
        var done = e.position || e.loaded
        var total = e.totalSize || e.total;
        var progress = Math.round(done / total * 100) + '%';
        progbar.setAttribute('style', 'width:' + progress);
        progbar.innerHTML = progress;
    });

    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 || ajax.readyState == "complete") {
            if (ajax.status == 200) {
                progbar.className = 'progress-bar progress-bar-success progress-bar-striped active';
                if (button.id == 'submit1') {
                    location.reload();
                } else {
                    location.href = "/Reference/Collection/" + collectionId;
                }
            }
            if (ajax.status == 400 || ajax.status == 500) {
                var result = ajax.responseText;
                var resultJson = JSON.parse(result);
                console.log(resultJson);
                progbar.className = 'progress-bar progress-bar-danger progress-bar-striped';
                submit1.className = 'btn btn-primary';
                submit2.className = 'btn btn-primary';
                var errorBox = document.getElementById('validation-errors-box');
                $('#validation-errors-box').css('display', '');
                var newContent = "";
                $.each(resultJson, function (k, v) {
                    newContent = newContent + '<p><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only">Error:</span> ' + v[0] + '</p>';
                });
                errorBox.innerHTML = newContent;
                $("html, body").animate({ scrollTop: 0 }, "slow");
                $('#submit').prop('disabled', false);
                $('#submit').removeClass('disabled');
                progDiv.setAttribute('style', 'display:none');
            }
        }
    }

    ajax.open("POST", "/Reference/AddGrain/" + collectionId);
    console.log(formData);
    ajax.send(formData);
}


//Focus Image Upload
function focusFramesUpload(input) {
    var d = document.getElementById("images");
    if (input.files.length) {
        var validImages = 0;
        for (var i = 0; i < input.files.length; i++) {
            if (/\.(jpe?g|png|gif)$/i.test(input.files[i].name)) {
                validImages++;
            }
        }
        if (validImages != 5) return; //TODO Add Error

    }
}