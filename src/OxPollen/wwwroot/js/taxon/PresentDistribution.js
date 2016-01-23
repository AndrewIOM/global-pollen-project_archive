var gbifWarning = document.getElementById('gbif-warning');
var gbifDescWarning = document.getElementById('gbif-warning-desc');
var warningsContainer = document.getElementById('warnings-container');

$('document').ready(function () {
    var gbifId = $('#GbifId').val();
    if (gbifId == 0) {
        warningsContainer.style.display = '';
        gbifWarning.style.display = '';
        gbifDescWarning.style.display = '';
    } else {
        populateGbifDescription(gbifId);
        gbifMap(gbifId);
    }
});

var gbifMap = function (gbifId) {

    var map = L.map('map', {
        center: [30, 0],
        zoom: 1
    });
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        attribution: 'Imagery © <a href="http://mapbox.com">Mapbox</a>',
        maxZoom: 18,
        id: 'mareep2000.onj49m55',
        accessToken: 'pk.eyJ1IjoibWFyZWVwMjAwMCIsImEiOiJjaWppeGUxdm8wMDQ3dmVtNHNhcHh0cHA1In0.OrAULrL8pJaL9N5WerUUDQ'
    }).addTo(map);

    var baseUrl = 'http://api.gbif.org/v1/map/density/tile?x={x}&y={y}&z={z}&type=TAXON&key=' + gbifId + '&layer=OBS_2000_2010&layer=SP_2000_2010&layer=OBS_2010_2020&layer=SP_2010_2020&layer=LIVING&palette=yellows_reds';
    var gbifAttrib = 'GBIF contributors';
    var gbif = new L.TileLayer(baseUrl, { minZoom: 0, maxZoom: 14, attribution: gbifAttrib }).addTo(map);
}

//GBIF Description
var populateGbifDescription = function (gbifId) {
    var holder = document.getElementById('gbif-description');
    var sourceHolder = document.getElementById('description-source');

    var gbifUri = "http://api.gbif.org/v1/species/" + gbifId;
    ajaxHelper(gbifUri + '/descriptions', 'GET', 'jsonp').done(function (data) {
        var description = '';
        var source = '';
        for (i = 0; i < data.results.length; i++) {
            if (data.results[i].description.length > 0 && data.results[i].language == 'eng' && description == '') {
                description = data.results[i].description.substring(0, 1000) + '...'; //Take first description
                source = data.results[i].source;
            }
        }
        if (description == '') {
            holder.innerHTML = 'Not Available';
            sourceHolder.innerHTML = 'Source: None Available';
            warningsContainer.style.display = '';
            gbifDescWarning.style.display = '';
        } else {
            holder.innerHTML = description;
            sourceHolder.innerHTML = 'Source: ' + source;
        }
    });
}

function ajaxHelper(uri, method, dataType, data) {
    console.log(uri);
    return $.ajax({
        type: method,
        url: uri,
        dataType: dataType,
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXhr, textStatus, errorThrown) {
        console.log(errorThrown);
    });
}
