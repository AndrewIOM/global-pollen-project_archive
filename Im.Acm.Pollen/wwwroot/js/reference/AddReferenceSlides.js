/*
 * Reference Slide Digitisation Client-Side
 * Andrew Martin 2016
 */

function createFormField(name, label, placeholder, value) {
    var row = document.createElement('div');
    row.className = 'row';
    var col1 = document.createElement('div');
    col1.className = 'col-md-4';
    var labelHtml = document.createElement('label');
    labelHtml.innerHTML = label;
    var col2 = document.createElement('div');
    col2.className = 'col-md-8';
    var input = document.createElement('input');
    input.setAttribute('placeholder', placeholder);
    input.setAttribute('name', name);
    input.className = 'form-control';
    if (value != undefined) input.value = value;

    col2.appendChild(input);
    col1.appendChild(labelHtml);
    row.appendChild(col1);
    row.appendChild(col2);
    return row;
}

function handleFiles(input) {
    var pending = document.getElementById('pending-uploads');
    if (!input.files.length) {
        pending.innerHTML = "<p>No files uploaded</p>";
    } else {
        pending.innerHTML = "";
        var collectionNumber = window.location.href.split("/").pop();

        var taxa = [];
        for (var i = 0; i < input.files.length; i++) {
            var fileName = input.files[i].name;
            var fileUrl = window.URL.createObjectURL(input.files[i]);

            //Parse filename
            var size = "";
            var family = "";
            var genus = "";
            var species = "";
            var splitFilename = fileName.replace(/\.[^/.]+$/, "").split('_');
            if (splitFilename.length == 1) {
                size = splitFilename[0];
            }
            if (splitFilename.length == 2) {
                size = splitFilename[0];
                family = splitFilename[1];
            }
            if (splitFilename.length == 3) {
                size = splitFilename[0];
                family = splitFilename[1];
                genus = splitFilename[2];
            }
            if (splitFilename.length == 4) {
                size = splitFilename[0];
                family = splitFilename[1];
                genus = splitFilename[2];
                species = splitFilename[3];
            }

            var rank;
            if (species != "") {
                rank = 'Species';
            } else if (genus != "") {
                rank = 'Genus';
            } else {
                rank = 'Family';
            }

            taxa.push({
                rank: rank,
                species: species,
                genus: genus,
                family: family,
                size: size,
                image: [fileUrl]
            });
        }

        var taxaGroupedImages = [];
        for (var i = 0; i < taxa.length; i++) {
            var existing = filterTaxa(taxaGroupedImages, taxa[i].rank, taxa[i].family, taxa[i].genus, taxa[i].species);
            if (existing.length == 0) {
                var match = filterTaxa(taxa, taxa[i].rank, taxa[i].family, taxa[i].genus, taxa[i].species);
                if (match.length == 1) {
                    taxaGroupedImages.push(match[0]);
                } else {
                    //More than one image for this taxon
                    var images = [];
                    for (var j = 1; j < match.length; j++) {
                        match[0].image.push(match[j].image[0]);
                    }
                    taxaGroupedImages.push(match[0]);
                }
            }
        }

        for (var i = 0; i < taxaGroupedImages.length; i++) {
            //Create ajax save form
            var f = document.createElement('form');
            f.id = 'upload-' + (i + 1);
            f.setAttribute("action", "");
            f.setAttribute("onsubmit", "return upload()");
            pending.appendChild(f);

            var row = document.createElement('div');
            row.className = 'row form-panel';
            var col1 = document.createElement('div');
            col1.className = 'col-md-4';
            var col2 = document.createElement('div');
            col2.className = 'col-md-8';
            row.appendChild(col1);
            row.appendChild(col2);
            f.appendChild(row);

            for (var j = 0; j < taxaGroupedImages[i].image.length; j++) {
                var img = document.createElement("img");
                img.src = taxaGroupedImages[i].image[j];
                img.id = "image-upload-" + (i + 1);
                img.style.height = '12em';
                col1.appendChild(img);
            }

            var inputCollection = document.createElement('input');
            inputCollection.hidden = 'hidden';
            inputCollection.name = 'CollectionId';
            inputCollection.value = collectionNumber;
            col2.appendChild(inputCollection);

            var inputRank = document.createElement('select');
            inputRank.id = 'Rank';
            inputRank.name = 'Rank';
            var speciesOption = document.createElement('option');
            speciesOption.value = '3';
            speciesOption.innerHTML = 'Species';
            var genusOption = document.createElement('option');
            genusOption.value = '2';
            genusOption.innerHTML = 'Genus';
            var familyOption = document.createElement('option');
            familyOption.value = '1';
            familyOption.innerHTML = 'Family';
            inputRank.appendChild(familyOption);
            inputRank.appendChild(genusOption);
            inputRank.appendChild(speciesOption);

            var inputFamily = createFormField('Family', 'Family', 'Family', family);
            var inputGenus = createFormField('Genus', 'Genus', 'Genus', genus);
            var inputSpecies = createFormField('Species', 'Species', 'Species', species);
            var inputSize = createFormField('MaxGrainSize', 'Maximum Grain Diameter (nm)', 'Maximum Grain Diameter (nanometres)', size);
            col2.appendChild(inputRank);
            col2.appendChild(inputFamily);
            col2.appendChild(inputGenus);
            col2.appendChild(inputSpecies);
            col2.appendChild(inputSize);

            //Parse rank
            var rank;
            if (species != "") {
                rank = 'Species';
                inputRank.value = 3;
            } else if (genus != "") {
                rank = 'Genus';
                inputRank.value = 2;
            } else {
                rank = 'Family';
                inputRank.value = 1;
            }

            var save = document.createElement('a');
            save.href = 'javascript: upload(' + (i + 1) + ')';
            save.className = 'btn btn-primary';
            save.innerHTML = 'Save';
            col2.appendChild(save);

            //Create progress bar
            var progDiv = document.createElement('div');
            progDiv.className = 'progress';
            progDiv.setAttribute('style', 'display:none');
            var progInnerDiv = document.createElement('div');
            progInnerDiv.className = 'progress-bar progress-bar-striped active';
            progInnerDiv.setAttribute('role', 'progressbar');
            progInnerDiv.setAttribute('style', 'width:0%');
            progDiv.appendChild(progInnerDiv);
            col2.appendChild(progDiv);
            var errorMessage = document.createElement('p');
            errorMessage.className = 'error-message';
            errorMessage.setAttribute('style', 'color:red;display:none');
            col2.appendChild(errorMessage);
        }
    }
}

function getBase64Image(imgElem) {
    // imgElem must be on the same server otherwise a cross-origin error will be thrown "SECURITY_ERR: DOM Exception 18"
    var canvas = document.createElement("canvas");
    canvas.width = imgElem.naturalWidth;
    canvas.height = imgElem.naturalHeight;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(imgElem, 0, 0);
    var dataURL = canvas.toDataURL("image/png");
    return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
}

function upload(formNumber) {
    console.log('uploading');

    var form = document.getElementById('upload-' + formNumber);
    var btns = form.getElementsByClassName('btn');
    var progbar = form.getElementsByClassName('progress-bar')[0];
    var progDiv = form.getElementsByClassName('progress')[0];
    var errorMessage = form.getElementsByClassName('error-message')[0];
    progDiv.setAttribute('style', 'display:auto');
    var images = form.getElementsByTagName('img');
    var formData = new FormData(form);
    var img = [];
    for (var i = 0; i < images.length; i++) {
        img.push(getBase64Image(images[i]));
    }
    formData.append('Images', img);

    ajax = new XMLHttpRequest();
    (ajax.upload || ajax).addEventListener('progress', function (e) {
        var done = e.position || e.loaded
        var total = e.totalSize || e.total;
        progbar.setAttribute('style', 'width:' + Math.round(done / total * 100) + '%');
    });

    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 || ajax.readyState == "complete") {
            if (ajax.status == 200) {
                console.log('success: 200');
                var result = ajax.responseText;
                var resultJson = JSON.parse(result);
                progbar.className = 'progress-bar progress-bar-success progress-bar-striped active';

                //Reconstruct element as summary
                setTimeout(func, 2000);
                function func() {
                    form.innerHTML = "";
                    var row = document.createElement('div');
                    row.className = 'row form-panel success';
                    form.appendChild(row);
                    var summaryText = document.createElement('p');
                    var name = '';
                    if (resultJson['Family'] != null) name = name + resultJson['Family'] + ' ';
                    if (resultJson['Genus'] != null) name = name + resultJson['Genus'] + ' ';
                    if (resultJson['Species'] != null) name = name + resultJson['Species'];
                    summaryText.innerHTML = '<span class="glyphicon glyphicon-floppy-saved"></span> Saved ' + name;
                    row.appendChild(summaryText);
                }
            }
            if (ajax.status == 400 || ajax.status == 500) {
                console.log('no success: 400 or 500');
                var result = ajax.responseText;
                var resultJson = JSON.parse(result);
                progbar.className = 'progress-bar progress-bar-danger progress-bar-striped';
                var newContent = "";
                $.each(resultJson, function (k, v) {
                    newContent = newContent + '<p><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only">Error:</span> ' + v[0] + '</p>';
                });
                errorMessage.innerHTML = newContent;
                errorMessage.setAttribute('style', 'color:red;display:inline;font-size:0.8em');

                btns[0].style.display = '';
            }
        }
    }

    btns[0].style.display = 'none';
    ajax.open("POST", "/Reference/AddGrain", true);
    ajax.send(formData);
}

function filterTaxa(taxa, rank, family, genus, species) {
    var match = [];
    for (var i = 0; i < taxa.length; i++) {
        if (taxa[i].rank == rank && taxa[i].family == family && taxa[i].genus == genus
            && taxa[i].species == species) {
            match.push(taxa[i]);
        }
    }
    return match;
}