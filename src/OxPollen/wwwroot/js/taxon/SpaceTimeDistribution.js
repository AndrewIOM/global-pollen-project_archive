//Variables
var map;
var neotomaPoints = L.layerGroup();
var greenIcon = L.icon({
    iconUrl: '/images/pollen1.png',
    iconSize: [5, 5], // size of the icon
    iconAnchor: [0, 0] // point of the icon which will correspond to marker's location
});

//Space-Time-Map
var populateSpaceTimeMap = function (gbifId, neotomaId) {
    console.log('GBIF ID: ' + gbifId);
    console.log('Neotoma ID: ' + neotomaId);

    var gbifWarning = document.getElementById('gbif-warning');
    var neotomaWarning = document.getElementById('neotoma-warning');
    var warningsContainer = document.getElementById('warnings-container');

    if (gbifId == 0) {
        gbifWarning.style.display = '';
    }
    if (neotomaId == 0) {
        neotomaWarning.style.display = '';
    }

    map = L.map('map', {
        layers: [neotomaPoints],
        center: [30, 0],
        zoom: 1
    });
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
        attribution: 'Imagery © <a href="http://mapbox.com">Mapbox</a>',
        maxZoom: 18,
        id: 'mareep2000.onj49m55',
        accessToken: 'pk.eyJ1IjoibWFyZWVwMjAwMCIsImEiOiJjaWppeGUxdm8wMDQ3dmVtNHNhcHh0cHA1In0.OrAULrL8pJaL9N5WerUUDQ'
    }).addTo(map);

    if (gbifId != 0) {
        console.log('Adding GBIF to Map');
        var baseUrl = 'http://api.gbif.org/v1/map/density/tile?x={x}&y={y}&z={z}&type=TAXON&key=' + gbifId + '&layer=OBS_2000_2010&layer=SP_2000_2010&layer=OBS_2010_2020&layer=SP_2010_2020&layer=LIVING&palette=yellows_reds';
        var gbifAttrib = 'GBIF contributors';
        var gbif = new L.TileLayer(baseUrl, { minZoom: 0, maxZoom: 14, attribution: gbifAttrib }).addTo(map);
    }

    var slider = $("#time-slicer").slider({
        min: 0,
        max: 15000,
        ticks: [0, 1000, 5000, 10000, 15000],
        value: [0, 1],
        step: 100,
        labelledby: ['time-slicer-a', 'time-slicer-b']
    }).on('slideStop', updateMapData).data('slider');

    function updateMapData() {
        //$("#time-slicer").slider("disable");
        //Clear existing data
        neotomaPoints.clearLayers();
        map.removeLayer(gbif);

        //Figure out oldest
        var oldestYear;
        var youngestYear;
        var sliderLeft = slider.getValue()[0];
        var sliderRight = slider.getValue()[1];
        if (sliderLeft > sliderRight) {
            oldestYear = sliderLeft;
            youngestYear = sliderRight;
        } else {
            oldestYear = sliderRight;
            youngestYear = sliderLeft;
        }

        //Refresh GBIF and Neotoma
        if (youngestYear == 0) {
            if (gbifId != 0) {
                gbif.addTo(map);
            }
        }
        if (oldestYear != 0) {
            //Use Neotoma Data
            if (neotomaId != 0) {
                var allNeotomaPoints = updateNeotomaPoints(neotomaId, oldestYear, youngestYear);
            }
        }
    }

}

//GBIF Description
var populateGbifDescription = function (gbifId) {

    var gbifWarning = document.getElementById('gbif-warning');
    var gbifDescWarning = document.getElementById('gbif-warning-desc');
    var warningsContainer = document.getElementById('warnings-container');
    var holder = document.getElementById('gbif-description');
    var sourceHolder = document.getElementById('description-source');

    if (gbifId == 0) {
        holder.innerHTML = 'Not Available';
        sourceHolder.innerHTML = 'Source: None Available';
        warningsContainer.style.display = '';
        gbifDescWarning.style.display = '';
        gbifWarning.style.display = '';
    } else {
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
}

//Base Functions
var updateNeotomaPoints = function (neotomaId, oldestYear, youngestYear) {

    var neotomaUri = "http://api.neotomadb.org/v1/data/datasets?callback=neotomaCallback&taxonids=" + neotomaId + "&ageof=taxon&ageold=" + oldestYear + "&ageyoung=" + youngestYear;
    console.log('Updating Neotoma Points... ' + neotomaUri);

    $.ajax({
        url: neotomaUri,
        cache: true,
        jsonp: false,
        jsonpCallback: 'neotomaCallback',
        cache: 'true',
        dataType: 'jsonp'
    });
}

function neotomaCallback(result) {
    if (result.success == 0) {
        warnings.style.display = '';
        warningsContainer.style.display = '';
    } else {
        for (var i = 0; i < result.data.length; i++) {
            neotomaPoints.addLayer(L.marker([result.data[i].Site.LatitudeNorth, result.data[i].Site.LongitudeEast], { icon: greenIcon }));
        }
        //$("#time-slicer").slider("enable");
    }
}

function ajaxHelper(uri, method, dataType, data) {
    console.log(uri);
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
