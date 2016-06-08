function updateList(entryBox, rank) {
    var query = '';
    if (rank == 'Species') {
        //Combine genus and species for canonical name
        var genus = document.getElementById('Genus').value;
        query += genus + " ";
    }
    query += entryBox.value;
    var request = "/api/plantlist/suggest?rank=" + rank + "&q=" + query;
    ajaxHelper(request, 'GET', 'json').done(function (data) {
        var list = document.getElementById(rank + 'List');
        $('#' + rank + 'List').css('display', 'block');
        list.innerHTML = "";
        for (var i = 0; i < data.length; i++) {
            if (i > 10) continue;
            var option = document.createElement('li');
            var link = document.createElement('a');
            option.appendChild(link);
            link.innerHTML = data[i].LatinName;

            var matchCount = 0;
            for (var j = 0; j < data.length; j++) {
                if (data[j].LatinName == data[i].LatinName) {
                    matchCount++;
                }
            };

            if (rank == 'Genus') {
                var familySpan = document.createElement('span');
                familySpan.innerHTML = (data[i].ParentLatinName + ',' + matchCount);
                familySpan.className = 'family-name';
                link.appendChild(familySpan);
            }
            link.addEventListener('click', function (e) {
                var name = this.innerHTML.split('<')[0];
                if (rank == 'Species') {
                    var species = name.split(' ')[1];
                    $('#' + rank).val(species);
                } else {
                    $('#' + rank).val(name);
                }

                //Autofill family name
                var family = this.getElementsByClassName("family-name")[0].innerHTML.split(',')[0];
                var matchCount = this.getElementsByClassName("family-name")[0].innerHTML.split(',')[1];
                if (matchCount == 1) {
                    $('#Family').val(family);
                };
                $('#' + rank + 'List').fadeOut();
            });
            list.appendChild(option);
        }
        $('.family-name').css('display', 'none');
    });
}

function disable(rank) {
    var element;
    if (rank == 'Family') element = 'FamilyList';
    if (rank == 'Genus') element = 'GenusList';
    if (rank == 'Species') element = 'SpeciesList';

    setTimeout(func, 100);
    function func() {
        $('#' + element).fadeOut();
    }
}

//Base Functions
function ajaxHelper(uri, method, dataType, data) {
    //self.error('');
    return $.ajax({
        type: method,
        url: uri,
        dataType: dataType,
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXhr, textStatus, errorThrown) {
        console.log(errorThrown);
        //self.error(errorThrown);
    });
}