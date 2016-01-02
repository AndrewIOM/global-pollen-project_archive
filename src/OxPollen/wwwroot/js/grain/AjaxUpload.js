function uploadFile() {
    $('#submit').prop('disabled', true);
    $('#submit').addClass('disabled');

    //Reset base64 images before submission
    var image1 = document.getElementById('ImageOne');
    var image2 = document.getElementById('ImageTwo');
    var image3 = document.getElementById('ImageThree');
    var image4 = document.getElementById('ImageFour');
    image1.value = '';
    image2.value = '';
    image3.value = '';
    image4.value = '';

    //Save image state to Url from DarkroomJS instances
    var instances = document.getElementsByClassName('darkroom-source-container');
    var imgs = [];
    for (var i = 0; i < instances.length; i++) {
        imgs.push(instances[i].getElementsByClassName('lower-canvas')[0]);
    }

    //var imgs = document.getElementsByClassName('darkroom-source-container')
    //    .getElementsByClassName('lower-canvas');
    if (imgs.length >= 1) {
        image1.value = imgs[0].toDataURL();
    }
    if (imgs.length >= 2) {
        image2.value = imgs[1].toDataURL();
    }
    if (imgs.length >= 3) {
        image3.value = imgs[2].toDataURL();
    }
    if (imgs.length >= 4) {
        image4.value = imgs[3].toDataURL();
    }

    console.log(imgs);
    console.log(image1.value);
    console.log(image2.value);
    console.log(image3.value);
    console.log(image4.value);

    //Get form data
    var form = document.getElementById('addGrainForm');
    var formData = new FormData(form);
    console.log(formData);

    //Ajax Request
    ajax = new XMLHttpRequest();
    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 || ajax.readyState == "complete") {
            if (ajax.status == 200) {
                location.href = "/Grain/Index";
            }
            if (ajax.status == 400 || ajax.status == 500) {
                var result = ajax.responseText;
                var resultJson = JSON.parse(result);
                console.log(resultJson);
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
            }
        }
    }

    ajax.open("POST", "/Grain/Add");
    ajax.send(formData);
}